using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class Stellarium
    {
        private string sMsg = "";

        public string Message
        { 
            get 
            { 
                return sMsg;
            } 
        }

        public string IPAddress = "127.0.0.1";
        public string Port = "8090";

        public string SetStelAction(string sName)
        {
            string result = "";

            string sWebServiceURL = @"http://" + IPAddress + ":" + Port + "/api/stelaction/do";
            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.UploadString(sWebServiceURL, "POST", "id=" + sName);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SetStAction: " + sName + "(" + elapsedTime + "\r\n";
            }
            catch (Exception e)
            {
                sMsg = "SetStAction ERROR " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }

            return result;
        }

        public string SetStelProperty(string sName, string sValue)
        {
            string result = "";

            string sWebServiceURL = @"http://" + IPAddress + ":" + Port + "/api/stelproperty/set";
            WebClient lwebClient = new WebClient();
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.UploadString(sWebServiceURL, "POST", "id=" + sName + "&value=" + sValue);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SetStProp: " + sName + ":" + sValue + "(" + elapsedTime + "\r\n";
            }
            catch (Exception e)
            {
                sMsg = "SetStProp ERROR " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string SetStellariumFOV(int iFOV)
        {
            string result = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/fov";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("fov", iFOV.ToString());

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SetStFOV " + iFOV.ToString() + "deg (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "SetStFOV ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string SyncStellariumToPosition(double RA, double Dec)
        {
            string result = "";
            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/focus";
            string sPos = RA.ToString() + ", " + Dec.ToString();

            // Convert selected object's RA to degrees and then both RA and Dec to radians
            RA = RA * 15 * Math.PI / 180;
            Dec = Dec * Math.PI / 180;

            // Calculate 3D vector for Stellarium
            double dblX = Math.Cos(Dec) * Math.Cos(RA);
            double dblY = Math.Cos(Dec) * Math.Sin(RA);
            double dblZ = Math.Sin(Dec);

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("position", "[" + dblX.ToString() + "," + dblY.ToString() + "," + dblZ.ToString() + "]");

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SyncStToPos " + sPos + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "SyncStToPos ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string StellariumToAltAzPosition(double Alt, double Az)
        {
            string result = "";
            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/view";
            string sPos = Alt.ToString() + ", " + Az.ToString();

            Az = 180 - Az;
            // Convert to radians
            Az = Az * Math.PI / 180;
            Alt = Alt * Math.PI / 180;

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("az", Az.ToString());
            nvcParams.Add("alt", Alt.ToString());

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SyncStToAltAz " + sPos + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "SyncStToAltAz ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string StellariumMove(double X, double Y)
        {
            string result = "";
            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/move";
            string sPos = X.ToString() + ", " + Y.ToString();

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("x", X.ToString());
            nvcParams.Add("y", Y.ToString());

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "StMove " + sPos + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "StMove ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string SyncStellariumToID(string sID)
        {
            string result = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/main/focus";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("target", sID);

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SyncToID " + sID + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "SyncStToID ERROR \r\n";
            }
            finally
            {
                lwebClient?.Dispose();
            }
            return result;
        }

        public string SyncStellariumToAPObject(string sID, string sRA, string sDec, string sType)
        {
            string result = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/scripts/direct";

            string sInput = "sObject=\"" + sID + "\";sRA=\"" + sRA + "\";sDec=\"" + sDec + "\";sType=\"" + sType + "\";\r\n";
            string sCode = "objmap = core.getObjectInfo(sObject);\r\nsInfo = new String(core.mapToString(objmap));\r\nsFound=sInfo.slice(5,10);\r\n\r\nif (sFound==\"found\")\r\n{\r\n\tCustomObjectMgr.addCustomObject(sObject, sRA, sDec, true);\r\n\tcore.selectObjectByName(sObject,true);\r\n\tcore.moveToSelectedObject();\r\n\tStelMovementMgr.setFlagTracking(true);\r\n\tcore.addToSelectedObjectInfoString(\"AP Type: \" + sType,false)\r\n}\r\nelse\r\n{\r\n\tcore.output(\"Object found\");\r\n\tcore.selectObjectByName(sObject,true);\r\n\tcore.moveToSelectedObject();\r\n\tStelMovementMgr.setFlagTracking(true);\r\n}";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("code", sInput + sCode);

            WebClient lwebClient = new WebClient();
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SyncStToAPObj " + sID + " " + sRA + ", " + sDec + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "SyncStToAPObj ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string StellariumRemoveMarker(string sMarkerName)
        {
            string result = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/scripts/direct";

            string sInput = "sMarker=\"" + sMarkerName + "\";\r\n";
            string sCode = "if (sMarker==\"\")\r\n{\r\n\tCustomObjectMgr.removeCustomObjects();\r\n}\r\nelse\r\n{\r\n\tCustomObjectMgr.removeCustomObject(sMarker);\r\n}";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("code", sInput + sCode);

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "StRemoveMarker (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "StRemoveMarker ERROR \r\n";
            }
            finally
            {
                lwebClient?.Dispose();
            }
            return result;
        }

        public JsonNode StellariumGetSelectedObjectInfo()
        {
            string result = "";
            JsonNode oSelectedObject = null;

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/objects/info?format=json";

            WebClient lwebClient = new WebClient();
            lwebClient.Encoding = Encoding.UTF8;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.DownloadString(sWebServiceURL);
                if (result != "")
                {
                    oSelectedObject = JsonNode.Parse(result);
                }

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "StGetSelectedObj (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "StGetSelectedObj ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return oSelectedObject;
        }

    }
}
