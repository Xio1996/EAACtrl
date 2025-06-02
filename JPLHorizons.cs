using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace EAACtrl
{
    public class JPLEphemerisOrbitalElement
    {
        public DateTime DateTime { get; set; }
        public double RA { get; set; }
        public double DEC { get; set; }
        public double APmag { get; set; }
        public double Delta { get; set; }
        public double DeltaDot { get; set; }
        public double S_O_T { get; set; }
        public double S_T_O { get; set; }
        public double SkyMotion { get; set; }
        public double SkyMotPA { get; set; }
        public double RelVelAng { get; set; }
        public double LunSkyBrt { get; set; }
        public double SkySNR { get; set; }
    }

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

        public List<JPLEphemerisOrbitalElement> ProcessJPLEphemeris(string sEphemeris)
        {
            if (string.IsNullOrEmpty(sEphemeris))
            {
                sMsg = "JPL: ProcessJPLEphemeris - No ephemeris data provided.\r\n";
                return null;
            }

            List<JPLEphemerisOrbitalElement> orbitalElements = new List<JPLEphemerisOrbitalElement>();

            string[] lines = sEphemeris.Split(new string[] { "$$SOE", "$$EOE" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length > 1)
            {
                string dataSection = lines[1];
                string[] dataLines = dataSection.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in dataLines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string[] values = line.Split(' '); // Split by space
                        if (values.Length >= 13) // Ensure enough values exist
                        {
                            try
                            {
                                JPLEphemerisOrbitalElement orbitalElement = new JPLEphemerisOrbitalElement
                                {
                                    DateTime = DateTime.ParseExact(values[0] + " " + values[1], "yyyy-MMM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                                    RA = double.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture),
                                    DEC = double.Parse(values[3], System.Globalization.CultureInfo.InvariantCulture),
                                    APmag = double.Parse(values[4], System.Globalization.CultureInfo.InvariantCulture),
                                    Delta = double.Parse(values[5], System.Globalization.CultureInfo.InvariantCulture),
                                    DeltaDot = double.Parse(values[6], System.Globalization.CultureInfo.InvariantCulture),
                                    S_O_T = double.Parse(values[7], System.Globalization.CultureInfo.InvariantCulture),
                                    S_T_O = double.Parse(values[8], System.Globalization.CultureInfo.InvariantCulture),
                                    SkyMotion = double.Parse(values[9], System.Globalization.CultureInfo.InvariantCulture),
                                    SkyMotPA = double.Parse(values[10], System.Globalization.CultureInfo.InvariantCulture),
                                    RelVelAng = double.Parse(values[11], System.Globalization.CultureInfo.InvariantCulture),
                                    LunSkyBrt = double.Parse(values[12], System.Globalization.CultureInfo.InvariantCulture),
                                    SkySNR = double.Parse(values[13], System.Globalization.CultureInfo.InvariantCulture)
                                };
                                orbitalElements.Add(orbitalElement);
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error parsing line: {line}.  Error: {ex.Message}");
                            }
                        }
                    }
                }
            }

            return orbitalElements;
        }

        // Returns the current RA/Dec of the passed object along with other object attributes
        public string Ephemeris(string objectID, DateTime queryTimeUTC)
        {
            string result = "";

            string sWebServiceURL = " https://ssd.jpl.nasa.gov/api/horizons.api?format=text&OBJ_DATA='NO'&MAKE_EPHEM='YES'&EPHEM_TYPE='OBSERVER'&QUANTITIES='1,9,20,21,29'&CSV_FORMAT='YES'&CENTER='coord'&COORD_TYPE='GEODETIC'&SITE_COORD='";
            sWebServiceURL += $"{Longitude.ToString()},{Latitude.ToString()},{(Altitude / 1000).ToString()}'";
            sWebServiceURL += $"&TLIST='{queryTimeUTC.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss")}'";
            sWebServiceURL += $"&COMMAND='{objectID}'";

            try
            {
                result = GetRequest(sWebServiceURL);

                sMsg = $"JPL: QueryObject {objectID}, {sMsg}\r\n";
            }
            catch (Exception)
            {
                sMsg = $"JPL:QueryObject ERR, {sMsg} \r\n";
                result = "exception";
            }
            return result;
        }

        public enum StepSizeUnit
        { 
            days,
            hours,
            minutes,
            years,
            months,
            unitless
        }

        public string Ephemeris(string objectID, DateTime startTimeUTC, DateTime endTimeUTC, StepSizeUnit unit, int interval)
        {
            string result = "";

            string sWebServiceURL = " https://ssd.jpl.nasa.gov/api/horizons.api?format=text&OBJ_DATA='NO'&MAKE_EPHEM='YES'&EPHEM_TYPE='OBSERVER'&QUANTITIES='1,9,20,21,29'&CSV_FORMAT='YES'&CENTER='coord'&COORD_TYPE='GEODETIC'&SITE_COORD='";
            sWebServiceURL += $"{Longitude.ToString()},{Latitude.ToString()},{(Altitude / 1000).ToString()}'";
            //sWebServiceURL += $"&TLIST='{queryTimeUTC.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss")}'";
            sWebServiceURL += $"&COMMAND='{objectID}'";

            try
            {
                result = GetRequest(sWebServiceURL);

                sMsg = $"JPL: QueryObject {objectID}, {sMsg}\r\n";
            }
            catch (Exception)
            {
                sMsg = $"JPL:QueryObject ERR, {sMsg} \r\n";
                result = "exception";
            }
            return result;
        }

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
