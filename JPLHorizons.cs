using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class JPLHorizons
    {
        private string sMsg = "";

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        public double Longitude = -1.3150645;
        public double Latitude = 50.7432162;
        public double Altitude = 57.184;

        // Returns the current RA/DEc of the passed object along with other object attributes
        public string QueryObject(string sID)
        {
            string result = "";

            string sWebServiceURL = "https://ssd.jpl.nasa.gov/api/horizons.api?format=text&OBJ_DATA='NO'&MAKE_EPHEM='YES'&EPHEM_TYPE='OBSERVER'&APPARENT='REFRACTED'&CSV_FORMAT='YES'&QUANTITIES='D'";
            sWebServiceURL += "&CENTER='coord'&COORD_TYPE='GEODETIC'&SITE_COORD='";
            sWebServiceURL += Longitude.ToString() + "," + Latitude.ToString() + "," + (Altitude/1000).ToString() + "'";
            sWebServiceURL += "&TLIST='" + DateTime.UtcNow.ToString() + "'";
            sWebServiceURL += "&COMMAND='" + sID + "'";

            WebClient lwebClient = new WebClient();
            lwebClient.Encoding = Encoding.UTF8;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.DownloadString(sWebServiceURL);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "JPL: QueryObject " + sID + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "JPL:QueryObject ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        // Searches for all minor bodies at a specified RA/Dec within a search box to a limiting magnitude
        // Format RA = (hh-mm-ss[.ss]   Dec = dd-mm-ss[.ss] Replace minus sign with M 
        public string SmallBodySearchBox(string RA, string Dec, int LimitingMagnitude, double Dimension)
        {
            string result = "";

            string sWebServiceURL = "https://ssd-api.jpl.nasa.gov/sb_ident.api?";
            sWebServiceURL += "lat=" + Latitude.ToString() + "&lon=" + Longitude.ToString() + "&alt=" + (Altitude / 1000).ToString();
            sWebServiceURL += "&two-pass=true&suppress-first-pass=true";
            sWebServiceURL += "&obs-time=" +DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss");
            sWebServiceURL += "&vmag-lim=" + LimitingMagnitude.ToString();
            sWebServiceURL += "&fov-ra-center=" + RA + "&fov-dec-center=" + Dec + "&fov-ra-hwidth=" + Dimension.ToString() + "&fov-dec-hwidth=" + Dimension.ToString();

            WebClient lwebClient = new WebClient();
            lwebClient.Encoding = Encoding.UTF8;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.DownloadString(sWebServiceURL);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "JPL:SmallBodySearchBox RA=" + RA + " Dec=" + Dec + " Mag=" + LimitingMagnitude + " Size=" + Dimension.ToString() + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "JPL:SmallBodySearchBox ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }
    }
}
