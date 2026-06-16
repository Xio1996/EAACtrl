// C#
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EAACtrl
{
    public static class WktHelper
    {
        // P: RA in degrees (0..360), Dec in degrees
        public struct P { public double RA; public double Dec; public P(double ra,double dec){RA=ra;Dec=dec;} }

        // Boundary polygon: outer ring + optional holes (each as list of P)
        public class BoundaryPolygon { public List<P> Outer; public List<List<P>> Holes = new List<List<P>>(); }

        // Create MULTIPOLYGON WKT for one constellation (polygons may include holes)
        public static string ToMultiPolygonWkt(IEnumerable<BoundaryPolygon> polygons)
        {
            var polyWkts = new List<string>();
            foreach (var bp in polygons)
            {
                var outer = NormalizeRingForLon(bp.Outer);
                var outerWkt = RingToWkt(outer);
                var holeWkts = bp.Holes.Select(h => RingToWkt(NormalizeRingForLon(h)));
                var allRings = new[] { outerWkt }.Concat(holeWkts);
                polyWkts.Add($"({string.Join(",", allRings)})");
            }
            var multipoly = $"SRID=4326;MULTIPOLYGON({string.Join(",", polyWkts)})";
            return multipoly;
        }

        // Convert ring (RA/Dec) to normalized lon/lat ring, keep continuity and close the ring
        private static List<(double lon,double lat)> NormalizeRingForLon(List<P> ring)
        {
            if (ring == null || ring.Count == 0) return new List<(double,double)>();

            // convert RA->lon in -180..180
            var lonLat = ring.Select(p =>
            {
                double lon = p.RA % 360.0;
                if (lon > 180.0) lon -= 360.0;
                return (lon, lat: p.Dec);
            }).ToList();

            // choose reference lon (mean) and shift points to be near reference to avoid 360 jumps
            double refLon = lonLat.Average(pt => pt.lon);
            for (int i = 0; i < lonLat.Count; i++)
            {
                double lon = lonLat[i].lon;
                while (lon - refLon > 180) lon -= 360;
                while (lon - refLon < -180) lon += 360;
                lonLat[i] = (lon, lonLat[i].lat);
            }

            // ensure ring is closed
            var first = lonLat[0];
            var last = lonLat[lonLat.Count - 1];
            if (Math.Abs(first.lon - last.lon) > 1e-9 || Math.Abs(first.lat - last.lat) > 1e-9)
                lonLat.Add(first);

            return lonLat;
        }

        private static string RingToWkt(List<(double lon,double lat)> ring)
        {
            var sb = new StringBuilder();
            sb.Append("(");
            string sep = "";
            foreach (var pt in ring)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1} {2}", sep, pt.lon, pt.lat);
                sep = ",";
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
