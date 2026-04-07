using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EAACtrl
{
    // Public API class for embedding in other apps.
    // Usage:
    //   var selector = new AlignmentSelector(); // optional: pass custom catalog
    //   var visible = selector.GetVisibleStars(utc, lat, lon);
    //   var best5 = selector.GetBestN(utc, lat, lon, 5);
    //   var best3 = selector.GetBest3Helper(utc, lat, lon);
    internal class AlignmentSelector
    {
        public IReadOnlyList<Star> Catalog { get; private set; }

        public AlignmentSelector(IReadOnlyList<Star> catalog = null)
        {
            Catalog = catalog ?? StarCatalog.GetCatalog();
        }

        public List<StarAltAz> GetVisibleStars(DateTime utc, double latDeg, double lonDeg,
            double minAltDeg = 15.0, double maxAltDeg = 60.0, Func<double, double, bool> azFilter = null)
        {
            var result = new List<StarAltAz>();
            foreach (var s in Catalog)
            {
                AltAz aa = AstronomyTools.RaDecToAltAz(s.RaHours, s.DecDeg, utc, latDeg, lonDeg);
                if (aa.AltitudeDeg >= minAltDeg && aa.AltitudeDeg <= maxAltDeg)
                {
                    if (azFilter == null || azFilter(aa.AzimuthDeg, aa.AltitudeDeg))
                        result.Add(new StarAltAz(s, aa.AltitudeDeg, aa.AzimuthDeg));
                }
            }
            return result;
        }

        public static bool DefaultAzFilter(double azDeg, double altDeg)
        {
            return azDeg <= 5.0 || azDeg >= 100.0;
        }

        public List<StarAltAz> GetBestN(DateTime utc, double latDeg, double lonDeg, int n,
            double minAltDeg = 15.0, double maxAltDeg = 60.0, Func<double, double, bool> azFilter = null)
        {
            var visible = GetVisibleStars(utc, latDeg, lonDeg, minAltDeg, maxAltDeg, azFilter);
            return CombinationFinder.FindBestN(visible, n);
        }

        public List<StarAltAz> GetBest5(DateTime utc, double latDeg, double lonDeg,
            double minAltDeg = 15.0, double maxAltDeg = 60.0, Func<double, double, bool> azFilter = null)
        {
            return GetBestN(utc, latDeg, lonDeg, 5, minAltDeg, maxAltDeg, azFilter);
        }

        public List<StarAltAz> GetBest3Helper(DateTime utc, double latDeg, double lonDeg,
            double minAltDeg = 15.0, double maxAltDeg = 60.0, Func<double, double, bool> azFilter = null)
        {
            var visible = GetVisibleStars(utc, latDeg, lonDeg, minAltDeg, maxAltDeg, azFilter);
            return CombinationFinder.FindBest3ForAltAzAlignment(visible);
        }

        public List<StarAltAz> OrderClockwiseFrom(IEnumerable<StarAltAz> stars, double startAz, bool clockwise = true)
        {
            return OrderingHelper.OrderClockwiseFrom(stars, startAz, clockwise);
        }

        public List<StarAltAz> OrderByMinimalSlew(IEnumerable<StarAltAz> stars, double startAz)
        {
            return OrderingHelper.OrderByMinimalSlew(stars, startAz);
        }

        public static (double AzOffsetDeg, double AltOffsetDeg, double AzRmsDeg, double AltRmsDeg)
            ComputeZeroOffsets(IList<StarAltAz> truePlaces, IList<Tuple<double, double>> measured)
        {
            // measured: Tuple<AzDeg, AltDeg> to avoid ValueTuple dependency
            var measuredPairs = measured.Select(t => (AzDeg: t.Item1, AltDeg: t.Item2)).ToList();
            return AlignmentHelper.ComputeZeroOffsets(truePlaces, measuredPairs);
        }
    }

    // Small altitude/azimuth container (avoids ValueTuple)
    public struct AltAz
    {
        public double AltitudeDeg;
        public double AzimuthDeg;
        public AltAz(double alt, double az) { AltitudeDeg = alt; AzimuthDeg = az; }
    }

    public class Star
    {
        public string Name { get; private set; }
        public double RaHours { get; private set; }
        public double DecDeg { get; private set; }
        public double Magnitude { get; private set; }

        public Star(string name, double raHours, double decDeg, double magnitude)
        {
            Name = name;
            RaHours = raHours;
            DecDeg = decDeg;
            Magnitude = magnitude;
        }
    }

    public class StarAltAz
    {
        public Star Star { get; private set; }
        public double AltitudeDeg { get; private set; }
        public double AzimuthDeg { get; private set; }

        public StarAltAz(Star star, double alt, double az)
        {
            Star = star;
            AltitudeDeg = alt;
            AzimuthDeg = az;
        }
    }

    public static class StarCatalog
    {
        public static List<Star> GetCatalog()
        {
            // attempt to load hyg.csv or stars.csv from app folder
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string[] candidates = { Path.Combine(baseDir, "hyg.csv"), Path.Combine(baseDir, "stars.csv"), Path.Combine(baseDir, "bright.csv") };
                foreach (var path in candidates)
                {
                    if (File.Exists(path))
                    {
                        var list = LoadCatalogFromCsv(path, 51.0);
                        if (list != null && list.Count > 0)
                            return list;
                    }
                }
            }
            catch
            {
                // fall through to builtin
            }

                return new List<Star>
            {
                new Star("Sirius", 6.75248, -16.7161, -1.46),
                new Star("Arcturus", 14.26102, 19.1825, -0.05),
                new Star("Vega", 18.61565, 38.7837, 0.03),
                new Star("Capella", 5.27815, 45.9979, 0.08),
                new Star("Rigel", 5.24230, -8.2016, 0.12),
                new Star("Procyon", 7.65500, 5.2250, 0.38),
                new Star("Betelgeuse", 5.91953, 7.4070, 0.50),
                new Star("Altair", 19.84639, 8.8683, 0.77),
                new Star("Aldebaran", 4.59868, 16.5093, 0.85),
                new Star("Antares", 16.49013, -26.4320, 0.96),
                new Star("Spica", 13.41988, -11.1613, 0.98),
                new Star("Pollux", 7.75527, 28.0262, 1.14),
                new Star("Fomalhaut", 22.96085, -29.6222, 1.16),
                new Star("Deneb", 20.69053, 45.2803, 1.25),
                new Star("Regulus", 10.13953, 11.9672, 1.35),
                new Star("Adhara", 6.97709, -28.9721, 1.50),
                new Star("Castor", 7.57666, 31.8883, 1.58),
                new Star("Shaula", 17.56033, -37.0639, 1.62),
                new Star("Alkaid", 13.79233, 49.3133, 1.86),
                new Star("Mirfak", 3.40567, 49.8611, 1.79),
                new Star("Alnitak", 5.67933, -1.9426, 1.74),
                new Star("Alnilam", 5.60356, -1.2019, 1.69),
                new Star("Mintaka", 5.53344, -0.2992, 2.25),
                new Star("Alpheratz", 0.13980, 29.0904, 2.06),
                new Star("Mirach", 1.16233, 35.6206, 2.06),
                new Star("Almach", 2.09733, 42.3297, 2.10),
                new Star("Markab", 23.06233, 15.2053, 2.49),
                new Star("Scheat", 23.06283, 28.0828, 2.43),
                new Star("Algenib", 21.73633, 44.9475, 2.84),
                new Star("Menkalinan", 21.69133, 43.1883, 2.02),
                new Star("Alphard", 9.45933, -8.6583, 1.98),
                new Star("Rasalhague", 17.58233, 12.5606, 2.08),
                new Star("Kochab", 14.84533, 74.1556, 2.07),
                new Star("Pherkad", 14.84433, 71.8333, 3.00),
                new Star("Dubhe", 11.06233, 61.7511, 1.79),
                new Star("Merak", 11.03033, 56.3825, 2.34),
                new Star("Alioth", 12.90033, 55.9597, 1.76),
                new Star("Mizar", 13.39833, 54.9250, 2.23),
                new Star("Polaris", 2.53033, 89.2641, 1.97),
                new Star("Elnath", 5.43833, 28.6075, 1.65),
                new Star("Alcyone", 3.79133, 24.1050, 2.87),
            };
        }

        // --- Begin: Move static local functions to class-level private static methods ---

        private static List<Star> LoadCatalogFromCsv(string path, double observerLatDeg)
        {
            // parse CSV with header, tolerant parsing:
            // detect columns: name/proper, ra (deg) or ra_hours, dec (deg), mag
            var lines = File.ReadAllLines(path);
            if (lines.Length < 2) return null;

            string header = lines[0];
            var cols = SplitCsvLine(header).Select(h => h.Trim().ToLowerInvariant()).ToList();
            int idxName = FindHeaderIndex(cols, new[] { "proper", "name", "proper_name", "hd_name", "id" });
            int idxRaDeg = FindHeaderIndex(cols, new[] { "ra", "ra_deg", "ra_degrees", "radeg" });
            int idxRaHours = FindHeaderIndex(cols, new[] { "ra_hours", "ra_h", "ra_hour", "rahours" });
            int idxDec = FindHeaderIndex(cols, new[] { "dec", "dec_deg", "dec_degrees", "decdeg" });
            int idxMag = FindHeaderIndex(cols, new[] { "mag", "mag_v", "magnitude" });

            if ((idxRaDeg < 0 && idxRaHours < 0) || idxDec < 0 || idxMag < 0)
                return null; // insufficient columns

            var records = new List<Tuple<string, double, double, double>>();

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                var parts = SplitCsvLine(line);
                try
                {
                    string name = idxName >= 0 && idxName < parts.Length ? parts[idxName] : string.Format("star{0}", i);
                    double raHours = 0.0;
                    double rah;
                    double radeg;
                    if (idxRaHours >= 0 && idxRaHours < parts.Length && double.TryParse(parts[idxRaHours], NumberStyles.Float, CultureInfo.InvariantCulture, out rah))
                        raHours = rah;
                    else if (idxRaDeg >= 0 && idxRaDeg < parts.Length && double.TryParse(parts[idxRaDeg], NumberStyles.Float, CultureInfo.InvariantCulture, out radeg))
                        raHours = radeg / 15.0;
                    else
                        continue;

                    double decDeg;
                    if (!double.TryParse(parts[idxDec], NumberStyles.Float, CultureInfo.InvariantCulture, out decDeg)) continue;
                    double mag;
                    if (!double.TryParse(parts[idxMag], NumberStyles.Float, CultureInfo.InvariantCulture, out mag)) continue;

                    // Check visibility from observer latitude: dec >= -(90 - lat)
                    double minDec = -(90.0 - observerLatDeg);
                    if (decDeg >= minDec) // can rise above horizon at some time
                        records.Add(Tuple.Create(name.Trim('"'), raHours, decDeg, mag));
                }
                catch
                {
                    // ignore malformed line
                }
            }

            // pick brightest 100 by magnitude (ascending)
            var selected = records.OrderBy(r => r.Item4).Take(100)
                .Select(r => new Star(string.IsNullOrWhiteSpace(r.Item1) ? string.Format("{0:F4}h/{1:F2}", r.Item2, r.Item3) : r.Item1, r.Item2, r.Item3, r.Item4))
                .ToList();

            return selected;
        }

        private static int FindHeaderIndex(List<string> headers, string[] possible)
        {
            foreach (var p in possible)
            {
                int i = headers.IndexOf(p.ToLowerInvariant());
                if (i >= 0) return i;
            }
            // try more tolerant matching
            for (int i = 0; i < headers.Count; i++)
            {
                foreach (var p in possible)
                {
                    if (headers[i].Contains(p.ToLowerInvariant())) return i;
                }
            }
            return -1;
        }

        // naive CSV split handling simple quoted fields
        private static string[] SplitCsvLine(string line)
        {
            var parts = new List<string>();
            bool inQuotes = false;
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }
                if (c == ',' && !inQuotes)
                {
                    parts.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            parts.Add(sb.ToString());
            return parts.ToArray();
        }

        // --- End: Move static local functions to class-level private static methods ---
    }

    public static class AstronomyTools
    {
        public static AltAz RaDecToAltAz(double raHours, double decDeg, DateTime utc, double latDeg, double lonDeg)
        {
            double lstHours = SiderealTime.LocalSiderealTimeHours(utc, lonDeg);

            double haHours = lstHours - raHours;
            haHours = NormalizeHours(haHours);
            double haRad = DegToRad(haHours * 15.0);

            double decRad = DegToRad(decDeg);
            double latRad = DegToRad(latDeg);

            double sinAlt = Math.Sin(latRad) * Math.Sin(decRad) + Math.Cos(latRad) * Math.Cos(decRad) * Math.Cos(haRad);
            sinAlt = Clamp(sinAlt, -1.0, 1.0);
            double altRad = Math.Asin(sinAlt);

            double cosAlt = Math.Cos(altRad);
            double sinA = -Math.Cos(decRad) * Math.Sin(haRad) / (Math.Abs(cosAlt) < 1e-12 ? 1e-12 : cosAlt);
            double cosA = (Math.Sin(decRad) - Math.Sin(altRad) * Math.Sin(latRad)) / (Math.Abs(cosAlt) < 1e-12 ? 1e-12 : (cosAlt * Math.Cos(latRad)));

            double azRad = Math.Atan2(sinA, cosA);
            double azDeg = RadToDeg(azRad);
            azDeg = (azDeg + 360.0) % 360.0;

            return new AltAz(RadToDeg(altRad), azDeg);
        }

        static double DegToRad(double d) { return d * Math.PI / 180.0; }
        static double RadToDeg(double r) { return r * 180.0 / Math.PI; }
        static double NormalizeHours(double h)
        {
            h %= 24.0;
            if (h < 0) h += 24.0;
            return h;
        }
        static double Clamp(double v, double lo, double hi)
        {
            if (v < lo) return lo;
            if (v > hi) return hi;
            return v;
        }
    }

    public static class SiderealTime
    {
        public static double LocalSiderealTimeHours(DateTime utc, double lonDegrees)
        {
            double jd = JulianDate(utc);
            double d = jd - 2451545.0;
            double gmstHours = 18.697374558 + 24.06570982441908 * d;
            gmstHours = NormalizeHours(gmstHours);
            double lst = gmstHours + lonDegrees / 15.0;
            lst = NormalizeHours(lst);
            return lst;
        }

        public static double JulianDate(DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc) date = date.ToUniversalTime();

            int year = date.Year;
            int month = date.Month;
            double day = date.Day + (date.Hour + (date.Minute + date.Second / 60.0) / 60.0) / 24.0 + date.Millisecond / 86400000.0;

            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }

            int A = year / 100;
            int B = 2 - A + (A / 4);

            double jd = Math.Floor(365.25 * (year + 4716))
                        + Math.Floor(30.6001 * (month + 1))
                        + day + B - 1524.5;

            return jd;
        }

        static double NormalizeHours(double h)
        {
            h %= 24.0;
            if (h < 0) h += 24.0;
            return h;
        }
    }

    public static class CombinationFinder
    {
        const double AltDiffMin = 10.0;
        const double AltDiffMax = 30.0;
        const double AzDiffMin = 45.0;
        const double AzDiffMax = 135.0;

        static int ScorePair(StarAltAz a, StarAltAz b)
        {
            double altDiff = Math.Abs(a.AltitudeDeg - b.AltitudeDeg);
            double azDiff = AzimuthDifference(a.AzimuthDeg, b.AzimuthDeg);
            int score = 0;
            if (altDiff >= AltDiffMin && altDiff <= AltDiffMax) score++;
            if (azDiff >= AzDiffMin && azDiff <= AzDiffMax) score++;
            return score;
        }

        public static int ScoreSet(IList<StarAltAz> set)
        {
            int score = 0;
            for (int i = 0; i < set.Count; i++)
                for (int j = i + 1; j < set.Count; j++)
                    score += ScorePair(set[i], set[j]);
            return score;
        }

        public static List<StarAltAz> FindBestN(List<StarAltAz> candidates, int n)
        {
            if (candidates.Count < n) return null;

            List<StarAltAz> best = null;
            int bestScore = -1;
            double bestMagSum = double.MaxValue;

            foreach (var comb in EnumerateCombinations(candidates, n))
            {
                int score = ScoreSet(comb);
                double magSum = comb.Sum(s => s.Star.Magnitude);
                if (score > bestScore || (score == bestScore && magSum < bestMagSum))
                {
                    bestScore = score;
                    best = comb.ToList();
                    bestMagSum = magSum;
                }
            }

            return best;
        }

        public static List<StarAltAz> FindBest3ForAltAzAlignment(List<StarAltAz> candidates)
        {
            const double similarThreshold = 5.0;
            const double thirdDiffThreshold = 30.0;

            if (candidates.Count < 3) return null;
            List<StarAltAz> best = null;
            int bestScore = -1;
            double bestMag = double.MaxValue;

            for (int i = 0; i < candidates.Count; i++)
            {
                for (int j = i + 1; j < candidates.Count; j++)
                {
                    double altDiff12 = Math.Abs(candidates[i].AltitudeDeg - candidates[j].AltitudeDeg);
                    if (altDiff12 > similarThreshold) continue;

                    for (int k = 0; k < candidates.Count; k++)
                    {
                        if (k == i || k == j) continue;
                        double altDiff3i = Math.Abs(candidates[k].AltitudeDeg - candidates[i].AltitudeDeg);
                        double altDiff3j = Math.Abs(candidates[k].AltitudeDeg - candidates[j].AltitudeDeg);

                        if (altDiff3i >= thirdDiffThreshold || altDiff3j >= thirdDiffThreshold)
                        {
                            var combo = new[] { candidates[i], candidates[j], candidates[k] };
                            int score = ScoreSet(combo);
                            double magSum = combo.Sum(s => s.Star.Magnitude);
                            if (score > bestScore || (score == bestScore && magSum < bestMag))
                            {
                                bestScore = score;
                                best = combo.ToList();
                                bestMag = magSum;
                            }
                        }
                    }
                }
            }

            return best;
        }

        static double AzimuthDifference(double aDeg, double bDeg)
        {
            double diff = Math.Abs(aDeg - bDeg) % 360.0;
            if (diff > 180.0) diff = 360.0 - diff;
            return diff;
        }

        static IEnumerable<IList<T>> EnumerateCombinations<T>(List<T> list, int k)
        {
            if (k == 0)
            {
                yield return new List<T>();
                yield break;
            }
            int n = list.Count;
            var indices = new int[k];
            for (int i = 0; i < k; i++) indices[i] = i;
            while (true)
            {
                var comb = new List<T>(k);
                for (int i = 0; i < k; i++) comb.Add(list[indices[i]]);
                yield return comb;

                int t = k - 1;
                while (t >= 0 && indices[t] == n - k + t) t--;
                if (t < 0) break;
                indices[t]++;
                for (int i = t + 1; i < k; i++) indices[i] = indices[i - 1] + 1;
            }
        }
    }

    public static class OrderingHelper
    {
        public static List<StarAltAz> OrderClockwiseFrom(IEnumerable<StarAltAz> stars, double startAz, bool clockwise = true)
        {
            double s = Normalize360(startAz);
            return stars
                .OrderBy(saa =>
                {
                    double diff = (saa.AzimuthDeg - s + 360.0) % 360.0;
                    return clockwise ? diff : (360.0 - diff) % 360.0;
                })
                .ToList();
        }

        public static List<StarAltAz> OrderByMinimalSlew(IEnumerable<StarAltAz> stars, double startAz)
        {
            var remaining = stars.ToList();
            if (remaining.Count == 0) return new List<StarAltAz>();

            int startIdx = remaining
                .Select((s, i) => new { s, i, d = Math.Abs(ShortestAzimuthDiff(s.AzimuthDeg, startAz)) })
                .OrderBy(x => x.d)
                .First().i;

            var ordered = new List<StarAltAz> { remaining[startIdx] };
            remaining.RemoveAt(startIdx);

            while (remaining.Count > 0)
            {
                int nextIdx = 0;
                double bestDist = double.MaxValue;
                var cur = ordered[ordered.Count - 1]; // compatible with C# pre-8
                for (int i = 0; i < remaining.Count; i++)
                {
                    double d = AngularSeparationDeg(cur, remaining[i]);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        nextIdx = i;
                    }
                }
                ordered.Add(remaining[nextIdx]);
                remaining.RemoveAt(nextIdx);
            }

            return ordered;
        }

        static double AngularSeparationDeg(StarAltAz a, StarAltAz b)
        {
            double aAlt = DegToRad(a.AltitudeDeg), aAz = DegToRad(a.AzimuthDeg);
            double bAlt = DegToRad(b.AltitudeDeg), bAz = DegToRad(b.AzimuthDeg);

            double ax = Math.Cos(aAlt) * Math.Sin(aAz);
            double ay = Math.Cos(aAlt) * Math.Cos(aAz);
            double az = Math.Sin(aAlt);

            double bx = Math.Cos(bAlt) * Math.Sin(bAz);
            double by = Math.Cos(bAlt) * Math.Cos(bAz);
            double bz = Math.Sin(bAlt);

            double dot = ax * bx + ay * by + az * bz;
            dot = Clamp(dot, -1.0, 1.0);
            return RadToDeg(Math.Acos(dot));
        }

        static double ShortestAzimuthDiff(double aDeg, double bDeg)
        {
            double diff = (aDeg - bDeg + 540.0) % 360.0 - 180.0;
            return diff;
        }

        static double Normalize360(double d)
        {
            d %= 360.0;
            if (d < 0) d += 360.0;
            return d;
        }

        static double DegToRad(double d) => d * Math.PI / 180.0;
        static double RadToDeg(double r) => r * 180.0 / Math.PI;
        // Replace the Clamp method in OrderingHelper with a version that always returns a value
        static double Clamp(double v, double lo, double hi)
        {
            if (v < lo) return lo;
            if (v > hi) return hi;
            return v;
        }
    }

    static class AlignmentHelper
    {
        // measured is list of (AzDeg, AltDeg)
        public static (double AzOffsetDeg, double AltOffsetDeg, double AzRmsDeg, double AltRmsDeg)
            ComputeZeroOffsets(IList<StarAltAz> truePlaces, IList<(double AzDeg, double AltDeg)> measured)
        {
            if (truePlaces == null) throw new ArgumentNullException("truePlaces");
            if (measured == null) throw new ArgumentNullException("measured");
            if (truePlaces.Count != measured.Count) throw new ArgumentException("Lists must have same length.");

            int n = truePlaces.Count;
            var azDiffs = new double[n];
            var altDiffs = new double[n];

            for (int i = 0; i < n; i++)
            {
                double trueAz = Normalize360(truePlaces[i].AzimuthDeg);
                double measAz = Normalize360(measured[i].AzDeg);
                azDiffs[i] = ShortestSignedAngleDeg(trueAz, measAz); // true - measured, signed [-180,180]
                altDiffs[i] = truePlaces[i].AltitudeDeg - measured[i].AltDeg;
            }

            double azOffset = azDiffs.Average();
            double altOffset = altDiffs.Average();

            double azRms = Math.Sqrt(azDiffs.Select(d => (d - azOffset) * (d - azOffset)).Average());
            double altRms = Math.Sqrt(altDiffs.Select(d => (d - altOffset) * (d - altOffset)).Average());

            return (azOffset, altOffset, azRms, altRms);
        }

        static double ShortestSignedAngleDeg(double targetDeg, double measuredDeg)
        {
            double diff = Normalize360(targetDeg) - Normalize360(measuredDeg);
            if (diff > 180.0) diff -= 360.0;
            if (diff <= -180.0) diff += 360.0;
            return diff;
        }

        static double Normalize360(double deg)
        {
            deg %= 360.0;
            if (deg < 0) deg += 360.0;
            return deg;
        }
    }

}
