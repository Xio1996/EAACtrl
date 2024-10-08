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
    }
}
