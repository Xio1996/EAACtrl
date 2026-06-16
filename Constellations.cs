using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Globalization;

namespace EAACtrl
{
    internal class ConstellationFinder
    {
        private readonly List<ConstellationFeature> _constellations;
        // Convert WKT with lon/lat (X=lon in -180..180) to WKT with RA/Dec (X=RA in 0..360)
        public static string ConvertWktLonLatToRaDecWkt(string wkt)
        {
            if (string.IsNullOrWhiteSpace(wkt)) return wkt;

            // preserve SRID=...; prefix if present
            string prefix = "";
            string body = wkt.Trim();
            if (body.StartsWith("SRID=", StringComparison.OrdinalIgnoreCase))
            {
                int semi = body.IndexOf(';');
                if (semi > 0)
                {
                    prefix = body.Substring(0, semi + 1); // includes ';'
                    body = body.Substring(semi + 1).Trim();
                }
            }

            // Regex to find coordinate pairs: <number> <number>
            var coordPair = new Regex(@"(-?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?)[\s,]+(-?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?)",
                                      RegexOptions.Compiled);

            int pairIndex = 0;
            string replaced = coordPair.Replace(body, match =>
            {
                // We expect successive matches to be coordinate pairs; convert X (lon) to RA
                string sx = match.Groups[1].Value;
                string sy = match.Groups[2].Value;

                if (!double.TryParse(sx, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon) ||
                    !double.TryParse(sy, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat))
                {
                    // if parse fails, return original
                    return match.Value;
                }

                // normalize lon -> RA in [0,360)
                double ra = lon % 360.0;
                if (ra < 0) ra += 360.0;

                // format with invariant culture; keep 6 decimals
                string sra = ra.ToString("F6", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
                string slat = lat.ToString("F6", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');

                pairIndex++;
                return sra + " " + slat;
            });

            return prefix + replaced;
        }
        public ConstellationFinder()
        {
            // 1. Get the current assembly where the JSON is embedded
            var assembly = Assembly.GetExecutingAssembly();

            // 2. Locate the resource name. 
            string resourceName = "EAACtrl.Constellations.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Embedded resource '{resourceName}' not found. " +
                        "Ensure its Build Action is set to 'Embedded Resource'.");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonContent = reader.ReadToEnd();
                    var doc = JsonSerializer.Deserialize<GeoJsonRoot>(jsonContent);
                    _constellations = doc?.Features ?? new List<ConstellationFeature>();
                }
            }
        }

        /// <summary>
        /// Finds the constellation at the given J2000 coordinates.
        /// </summary>
        /// <param name="raHours">Right Ascension in hours (0 to 24)</param>
        /// <param name="decDegrees">Declination in degrees (-90 to 90)</param>
        /// <returns>The abbreviation/name of the constellation, or "Unknown"</returns>
        public string GetConstellation(double raHours, double decDegrees)
        {
            // 1. Convert RA hours to degrees (1 hour = 15 degrees)
            double raDegrees = raHours * 15.0;

            // 2. Map [0, 360] RA degrees to the [-180, 180] Longitude scale used in the GeoJSON
            if (raDegrees > 180.0)
            {
                raDegrees -= 360.0;
            }

            // 3. Test against each constellation's boundaries
            foreach (var feature in _constellations)
            {
                if (feature.Geometry?.Coordinates == null) continue;

                // MultiPolygon structure: List of Polygons -> List of Rings -> List of Points [X, Y]
                foreach (var polygon in feature.Geometry.Coordinates)
                {
                    // The first ring is always the exterior boundary
                    if (polygon.Count > 0 && IsPointInPolygon(raDegrees, decDegrees, polygon[0]))
                    {
                        // Optional: Check if it's inside any interior holes (polygon[1], polygon[2], etc.)
                        bool inHole = false;
                        for (int i = 1; i < polygon.Count; i++)
                        {
                            if (IsPointInPolygon(raDegrees, decDegrees, polygon[i]))
                            {
                                inHole = true;
                                break;
                            }
                        }

                        if (!inHole)
                        {
                            // Returns the abbreviation (e.g., "And"). Change to feature.Properties.Name for full name.
                            return feature.Properties?.Id ?? feature.Properties?.Name ?? "Unknown";
                        }
                    }
                }
            }

            return "Unknown";
        }

        /// <summary>
        /// Returns the constellation's boundary as a Well-Known Text (WKT) MULTIPOLYGON string
        /// suitable for PostgreSQL / PostGIS geometry insertion or querying.
        /// </summary>
        /// <param name="abbreviation">The 3-letter abbreviation of the constellation (e.g., "Boo", "And")</param>
        /// <returns>A WKT MULTIPOLYGON string, or null if the constellation is not found.</returns>
        public string GetConstellationWkt(string abbreviation)
        {
            if (string.IsNullOrWhiteSpace(abbreviation)) return null;

            // Find the feature matching the abbreviation (case-insensitive)
            var feature = _constellations.Find(f =>
                string.Equals(f.Properties?.Id, abbreviation, StringComparison.OrdinalIgnoreCase));

            if (feature?.Geometry?.Coordinates == null) return null;
            // Build MULTIPOLYGON WKT directly using RA in [0,360) and Dec as Y.
            var sb = new StringBuilder();
            sb.Append("MULTIPOLYGON(");

            for (int p = 0; p < feature.Geometry.Coordinates.Count; p++)
            {
                var polygon = feature.Geometry.Coordinates[p];
                sb.Append("(");

                for (int r = 0; r < polygon.Count; r++)
                {
                    var ring = polygon[r];
                    sb.Append("(");

                    // Append all points, converting lon->RA in [0,360) and formatting with invariant culture.
                    for (int pt = 0; pt < ring.Count; pt++)
                    {
                        var point = ring[pt];
                        double lon = point[0];
                        double lat = point[1];
                        double ra = (lon % 360.0 + 360.0) % 360.0;
                        string sra = ra.ToString("F6", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
                        string slat = lat.ToString("F6", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
                        sb.Append(sra);
                        sb.Append(" ");
                        sb.Append(slat);

                        if (pt < ring.Count - 1)
                            sb.Append(", ");
                    }

                    // Ensure ring is closed: if first and last (after RA normalization) differ, append first point.
                    if (ring.Count > 0)
                    {
                        var first = ring[0];
                        var last = ring[ring.Count - 1];
                        double firstRa = (first[0] % 360.0 + 360.0) % 360.0;
                        double lastRa = (last[0] % 360.0 + 360.0) % 360.0;
                        double firstDec = first[1];
                        double lastDec = last[1];
                        if (Math.Abs(firstRa - lastRa) > 1e-9 || Math.Abs(firstDec - lastDec) > 1e-9)
                        {
                            string sfa = firstRa.ToString("F6", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
                            string sfd = firstDec.ToString("F6", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
                            sb.Append(", ");
                            sb.Append(sfa);
                            sb.Append(" ");
                            sb.Append(sfd);
                        }
                    }

                    sb.Append(")");
                    if (r < polygon.Count - 1)
                        sb.Append(", ");
                }

                sb.Append(")");
                if (p < feature.Geometry.Coordinates.Count - 1)
                    sb.Append(", ");
            }

            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Ray-casting algorithm to detect if a point is inside a polygon ring.
        /// </summary>
        private bool IsPointInPolygon(double x, double y, List<List<double>> polygonRing)
        {
            bool inside = false;
            int count = polygonRing.Count;

            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                double xi = polygonRing[i][0];
                double yi = polygonRing[i][1];
                double xj = polygonRing[j][0];
                double yj = polygonRing[j][1];

                bool intersect = ((yi > y) != (yj > y))
                    && (x < (xj - xi) * (y - yi) / (yj - yi) + xi);

                if (intersect) inside = !inside;
            }

            return inside;
        }

        #region GeoJSON DTO Classes
        private class GeoJsonRoot
        {
            [JsonPropertyName("features")]
            public List<ConstellationFeature> Features { get; set; }
        }

        private class ConstellationFeature
        {
            [JsonPropertyName("properties")]
            public ConstellationProperties Properties { get; set; }

            [JsonPropertyName("geometry")]
            public GeometryData Geometry { get; set; }
        }

        private class ConstellationProperties
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } // e.g., "And"

            [JsonPropertyName("name")]
            public string Name { get; set; } // e.g., "Andromeda"
        }

        private class GeometryData
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            // MultiPolygon structure: [Polygon][Ring][Point][X/Y]
            [JsonPropertyName("coordinates")]
            public List<List<List<List<double>>>> Coordinates { get; set; }
        }
        #endregion
    }
}
