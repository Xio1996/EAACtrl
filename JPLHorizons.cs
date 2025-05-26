using System;
using System.Diagnostics;
using System.Net.Http;

namespace EAACtrl
{
    internal class JPLHorizons
    {
        private string sMsg = "";
        private static readonly HttpClient httpClient = new HttpClient();

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        private string GetRequest(string url)
        {
            string result = "";
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = httpClient.GetStringAsync(url).GetAwaiter().GetResult();

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = $"Request to {url} {elapsedTime}\r\n";
            }
            catch (HttpRequestException e)
            {
                sMsg = $"Request {url} ERROR {e.Message}\r\n";
                result = "exception";
            }

            return result;
        }

        public double Longitude = -1.3150645;
        public double Latitude = 50.7432162;
        public double Altitude = 57.184;

        // Returns the current RA/Dec of the passed object along with other object attributes
        public string QueryObject(string sID)
        {
            string result = "";

            string sWebServiceURL = " https://ssd.jpl.nasa.gov/api/horizons.api?format=text&OBJ_DATA='NO'&MAKE_EPHEM='YES'&EPHEM_TYPE='OBSERVER'&QUANTITIES='1,9,10,20,21,29'&CENTER='coord'&COORD_TYPE='GEODETIC'&SITE_COORD='";
            sWebServiceURL += $"{Longitude.ToString()},{Latitude.ToString()},{(Altitude/1000).ToString()}'";
            sWebServiceURL += $"&TLIST='{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}'";
            sWebServiceURL += $"&COMMAND='{sID}'";

            try
            {
                result = GetRequest(sWebServiceURL);

                sMsg = $"JPL: QueryObject {sID}, {sMsg}\r\n";
            }
            catch (Exception)
            {
                sMsg = $"JPL:QueryObject ERR, {sMsg} \r\n";
                result = "exception";
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

            try
            {
                result = GetRequest(sWebServiceURL);

                sMsg = $"JPL:SmallBodySearchBox RA={RA} Dec={Dec} Mag={LimitingMagnitude} Size={Dimension}, {sMsg}\r\n";
            }
            catch (Exception)
            {
                sMsg = $"JPL:SmallBodySearchBox ERR, {sMsg} \r\n";
                result = "exception";
            }

            return result;
        }
    }
}
