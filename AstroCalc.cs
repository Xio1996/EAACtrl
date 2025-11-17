using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASCOM.Astrometry;
using ASCOM.Utilities;

namespace EAACtrl
{
    internal class AstroCalc
    {
        public void J2000ToJNOW(double RA2000, double Dec2000, out double RANOW, out double DecNOW, bool Apparent = false)
        {
            ASCOM.Astrometry.Transform.Transform T = new ASCOM.Astrometry.Transform.Transform();
            ASCOM.Utilities.Util U = new ASCOM.Utilities.Util();        

            T.SiteLatitude = Properties.Settings.Default.SiteLat;
            T.SiteLongitude = Properties.Settings.Default.SiteLng;
            T.SiteElevation = Properties.Settings.Default.SiteElev;
            T.SiteTemperature = 20.0;
            T.JulianDateTT = U.DateLocalToJulian(DateTime.Now);
            T.SetJ2000(RA2000, Dec2000);

            if (Apparent)
            {
                RANOW = T.RAApparent;
                DecNOW = T.DECApparent;
            }
            else
            {
                RANOW = T.RATopocentric;
                DecNOW = T.DECTopocentric;
            }

            T.Dispose();
        }

        public void JNOWToJ2000(double RANOW, double DecNOW, out double RA2000, out double Dec2000)
        {
            ASCOM.Astrometry.Transform.Transform T = new ASCOM.Astrometry.Transform.Transform();
            ASCOM.Utilities.Util U = new ASCOM.Utilities.Util();

            T.SiteLatitude = Properties.Settings.Default.SiteLat;
            T.SiteLongitude = Properties.Settings.Default.SiteLng;
            T.SiteElevation = Properties.Settings.Default.SiteElev;
            T.SiteTemperature = 20.0;
            T.JulianDateTT = U.DateLocalToJulian(DateTime.Now);
            T.SetTopocentric(RANOW, DecNOW);
            
            RA2000 = T.RAJ2000;
            Dec2000 = T.DecJ2000;

            T.Dispose();
        }

        public bool J2000ToAltAz(double RA, double Dec, out double Altitude, out double Azimuth)
        {
            bool bResult = false;
            Altitude = 999; Azimuth = 999;

            try
            {
                ASCOM.Utilities.Util U = new ASCOM.Utilities.Util();
                ASCOM.Astrometry.Transform.Transform T = new ASCOM.Astrometry.Transform.Transform();

                T.SiteLatitude = Properties.Settings.Default.SiteLat;
                T.SiteLongitude = Properties.Settings.Default.SiteLng;
                T.SiteElevation = Properties.Settings.Default.SiteElev;
                T.SiteTemperature = 20.0;
                T.JulianDateTT = U.DateLocalToJulian(DateTime.Now);
                T.SetJ2000(RA, Dec);
                
                Altitude = T.ElevationTopocentric;
                Azimuth = T.AzimuthTopocentric;

                bResult = true;
            }
            catch {}

            return bResult;
        }

        public bool JNOWToAltAz(double RA, double Dec, out double Altitude, out double Azimuth)
        {
            bool bResult = false;
            Altitude = 999; Azimuth = 999;

            try
            {
                ASCOM.Utilities.Util U = new ASCOM.Utilities.Util();
                ASCOM.Astrometry.Transform.Transform T = new ASCOM.Astrometry.Transform.Transform();

                T.SiteLatitude = Properties.Settings.Default.SiteLat;
                T.SiteLongitude = Properties.Settings.Default.SiteLng;
                T.SiteElevation = Properties.Settings.Default.SiteElev;
                T.SiteTemperature = 20.0;
                T.JulianDateTT = U.DateLocalToJulian(DateTime.Now);
                T.SetTopocentric(RA, Dec);

                Altitude = T.ElevationTopocentric;
                Azimuth = T.AzimuthTopocentric;

                bResult = true;
            }
            catch { }

            return bResult;
        }


        public List<(double,double)> BoundingBox(double CentreRA, double CentreDec, double Height, double Width)
        {
            
            List<(double, double)> BoundingPolygonRough = new List<(double, double)>();
            List<(double, double)> BoundingPolygon = new List<(double, double)>();

            BoundingPolygonRough.Add((CentreRA - Width / 2.0, CentreDec + Height / 2.0));
            BoundingPolygonRough.Add((CentreRA + Width / 2.0, CentreDec + Height / 2.0));
            BoundingPolygonRough.Add((CentreRA + Width / 2.0, CentreDec - Height / 2.0));
            BoundingPolygonRough.Add((CentreRA - Width / 2.0, CentreDec - Height / 2.0));

            BoundingPolygonRough.Add((CentreRA - Width / 2.0, CentreDec + Height / 2.0));

            foreach (var polygon in BoundingPolygonRough)
            {
                double RA = polygon.Item1;
                double Dec= polygon.Item2;

                // Check RA does not exceed bounds
                if (polygon.Item1 < 0)
                {
                    RA = 360.0 + polygon.Item1;
                }
                else if (polygon.Item1 > 360)
                {
                    RA = polygon.Item1-360.0;
                }

                // Check if Dec does not exceed bounds
                if (polygon.Item2 > 90)
                {
                    Dec = (polygon.Item2 - 90.0)-90.0;
                }
                else if (polygon.Item2 < -90.0) 
                {
                    Dec = (Math.Abs(polygon.Item2) - 90.0)+-90.0;
                }

                BoundingPolygon.Add((RA,Dec));
            }

            return BoundingPolygon;
        }

        // From ASCOMPlateAlign v1.1
        public double DeltaFromTwoRADec(double RA1, double Dec1, double RA2, double Dec2, bool RAInDegrees = true)
        {
            // Use Haversine for small angles
            double D1 = (Dec1) * Math.PI / 180.0;
            double D2 = (Dec2) * Math.PI / 180.0;

            //RA is in Hours, Convert to radians
            double DeltaRA = (RA2 - RA1) * Math.PI / 12.0;

            if (RAInDegrees) DeltaRA *= 12.0 / 180.0;

            double Sin_2_Dec = Math.Sin(0.5 * (D2 - D1));
            Sin_2_Dec *= Sin_2_Dec;

            double Sin_2_RA = Math.Sin(0.5 * DeltaRA);
            Sin_2_RA *= Sin_2_RA;

            double HalfSinDelta = Sin_2_Dec + Math.Cos(D1) * Math.Cos(D2) * Sin_2_RA;
            HalfSinDelta = Math.Sqrt(HalfSinDelta);

            double Delta = 2.0 * Math.Asin(HalfSinDelta) * 180D / Math.PI;

            return Delta;
        }

        private const double Deg2Rad = Math.PI / 180.0;
        private const double Rad2Deg = 180.0 / Math.PI;

        /// <summary>
        /// Calculates the sky coordinate reached by moving a given angular distance
        /// from an origin point along a specified position angle.
        /// </summary>
        /// <param name="originRAHours">Origin right‑ascension in hours (0‑24).</param>
        /// <param name="originDecDeg">Origin declination in degrees (‑90‑+90).</param>
        /// <param name="posAngleDeg">Position angle in degrees, measured east‑of‑north.</param>
        /// <param name="distArcSec">Angular distance to travel, in arc‑seconds.</param>
        /// <param name="destRAHours">Resulting right‑ascension (output) in hours.</param>
        /// <param name="destDecDeg">Resulting declination (output) in degrees.</param>
        public void OffsetCoordinates(
            double originRAHours,
            double originDecDeg,
            double posAngleDeg,
            double distArcSec,
            out double destRAHours,
            out double destDecDeg)
        {
            // ----- 1️⃣  Convert everything to radians -----
            double ra0 = originRAHours * 15.0 * Deg2Rad;          // hours → degrees → rad
            double dec0 = originDecDeg * Deg2Rad;
            double pa = posAngleDeg * Deg2Rad;
            double d = distArcSec / 3600.0 * Deg2Rad;            // arc‑min → deg → rad

            // ----- 2️⃣  Spherical trigonometry (great‑circle offset) -----
            // Formulae from the “spherical law of cosines” / “vector rotation”:
            // sin δ₂ = sin δ₁·cos d + cos δ₁·sin d·cos θ
            // Δα    = atan2( sin d·sin θ , cos δ₁·cos d − sin δ₁·sin d·cos θ )
            double sinDec0 = Math.Sin(dec0);
            double cosDec0 = Math.Cos(dec0);
            double sinD = Math.Sin(d);
            double cosD = Math.Cos(d);
            double sinPA = Math.Sin(pa);
            double cosPA = Math.Cos(pa);

            // Destination declination
            double sinDec2 = sinDec0 * cosD + cosDec0 * sinD * cosPA;
            double dec2 = Math.Asin(sinDec2);   // still in radians

            // Destination right‑ascension offset
            double y = sinD * sinPA;
            double x = cosDec0 * cosD - sinDec0 * sinD * cosPA;
            double deltaRA = Math.Atan2(y, x);    // radians, signed

            double ra2 = ra0 + deltaRA;           // radians

            // ----- 3️⃣  Normalise RA into the 0‑24 h range -----
            // Convert back to hours first, then wrap.
            destRAHours = (ra2 * Rad2Deg / 15.0) % 24.0;
            if (destRAHours < 0) destRAHours += 24.0;

            // ----- 4️⃣  Convert declination back to degrees -----
            destDecDeg = dec2 * Rad2Deg;
        }
    }
}
