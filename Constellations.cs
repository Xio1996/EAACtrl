using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EAACtrl
{
    internal class ConstellationFinder
    {
        private readonly List<ConstellationFeature> _constellations;

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
