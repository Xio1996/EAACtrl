using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class SolarAltitude
    {
        // Constants
        private const double EarthRadius = 6371.0; // km
        private const double DegreesToRadians = Math.PI / 180.0;

        public double CalculateAltitude(double latitude, double longitude, DateTime time)
        {
            // Convert to UTC
            time = time.ToUniversalTime();

            // 1. Calculate the number of days since J2000.0
            DateTime J2000 = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            double days = (time - J2000).TotalDays;

            // 2. Mean longitude, mean anomaly
            double meanLongitude = (280.46 + 0.9856474 * days) % 360;
            double meanAnomaly = (357.528 + 0.9856003 * days) % 360;

            // 3. Ecliptic longitude
            double lambda = meanLongitude + 1.915 * Math.Sin(meanAnomaly * Math.PI / 180.0)
                                          + 0.020 * Math.Sin(2 * meanAnomaly * Math.PI / 180.0);

            // 4. Obliquity of the ecliptic
            double epsilon = 23.439 - 0.0000004 * days;

            // 5. Sun's declination
            double decl = Math.Asin(Math.Sin(epsilon * Math.PI / 180.0) * Math.Sin(lambda * Math.PI / 180.0)) * 180.0 / Math.PI;

            // 6. Equation of time (in minutes)
            double y = Math.Tan((epsilon / 2) * Math.PI / 180.0);
            y *= y;
            double eqTime = 4 * (y * Math.Sin(2 * meanLongitude * Math.PI / 180.0)
                - 2 * 0.0167 * Math.Sin(meanAnomaly * Math.PI / 180.0)
                + 4 * 0.0167 * y * Math.Sin(meanAnomaly * Math.PI / 180.0) * Math.Cos(2 * meanLongitude * Math.PI / 180.0)
                - 0.5 * y * y * Math.Sin(4 * meanLongitude * Math.PI / 180.0)
                - 1.25 * 0.0167 * 0.0167 * Math.Sin(2 * meanAnomaly * Math.PI / 180.0));

            // 7. Solar noon (in fractional hours UTC)
            double timeOffset = eqTime + 4 * longitude;
            double tst = time.Hour * 60 + time.Minute + time.Second / 60.0 + timeOffset;
            double ha = (tst / 4.0) - 180.0; // hour angle in degrees

            // 8. Convert to radians
            double latRad = latitude * Math.PI / 180.0;
            double decRad = decl * Math.PI / 180.0;
            double haRad = ha * Math.PI / 180.0;

            // 9. Calculate altitude
            double altitude = Math.Asin(Math.Sin(latRad) * Math.Sin(decRad) +
                                        Math.Cos(latRad) * Math.Cos(decRad) * Math.Cos(haRad));
            return altitude * 180.0 / Math.PI;
        }
    }
}
