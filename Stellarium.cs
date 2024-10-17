using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class Stellarium
    {
        private APHelper APHelper = new APHelper();
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
        public string ScriptFolder = "";

        private Dictionary<string, string> StelTypeMappings = new Dictionary<string, string>()
        {
            {"galaxy","Galaxy"}, {"active galaxy","ActGal"}, {"globular star cluster","Globular"}, {"open star cluster","Open"},
            {"star cluster","Open"}, {"cluster","Open"}, {"HII region","Neb"}, {"planetary nebula","P Neb"},
            {"reflection nebula","R Neb"}, {"dark nebula","DkNeb"}, {"nebula","Neb"},
            {"double star","Dbl"},{"star","Star"},{"supernova remnant","SNR"},{"asteroid","Minor"},
            {"comet","ext_Comet"},{"planet","Planet"},{"moon","Planetary Moon"},{"artificial satellite","Artificial Satellite"}
        };

        public string APTypeFromStellariumType(string ObjectType)
        {
            try
            {
                if (ObjectType != null || ObjectType != "")
                {
                    return StelTypeMappings[ObjectType];
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return ObjectType;
            }

            return "";
        }

        public double StelRAtoAPRA(double RA)
        { 
            if (RA > 0)
            {
                return RA / 15.0;
            }
            return (RA + 360.0) / 15.0; 
        }

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

        public string SetStellariumFOV(double iFOV)
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

        public List<string[]> DrawObjects(APGetCmdResult varObjects)
        {
            string sOut = "obj = [";
            // Store the search results for DSA and Search List display
            List<string[]> listOfSearchResults = new List<string[]>();

            bool bFilterOnDistance = false;
            double DistMin, DistMax = 0.0;

            if (double.TryParse(Properties.Settings.Default.sfDistMin, out DistMin) && double.TryParse(Properties.Settings.Default.sfDistMax, out DistMax))
            {
                bFilterOnDistance = true;
            }


            foreach (APCmdObject obj in varObjects.results.Objects)
            {
                if (bFilterOnDistance)
                {
                    if (double.TryParse(obj.Distance, out double RV))
                    {
                        // Radial velocity to distance Mpc (good out to approx 50 Mpc)
                        double dblDistance = RV / Properties.Settings.Default.Hubble;

                        if (dblDistance < DistMin || dblDistance > DistMax)
                        {
                            continue;
                        }
                    }
                    else continue;
                }

                string RA = ""; string Dec = "";
                // Format RA/Dec to hms and dms
                RA = APHelper.RADecimalHoursToHMS(obj.RA2000, @"hh\hmm\mss\.ff\s");
                Dec = APHelper.DecDecimalToDMS(obj.Dec2000);

                if (Properties.Settings.Default.sResultsList)
                {
                    string sMag = "";
                    if (obj.Magnitude != 999)
                    {
                        sMag = Math.Round(obj.Magnitude, 2).ToString();
                    }

                    string sDistance = "";
                    if (double.TryParse(obj.Distance, out double distance))
                    {
                        // Radial velocity to distance Mpc (good out to approx 50 Mpc)
                        sDistance = (distance / Properties.Settings.Default.Hubble).ToString();
                    }

                    listOfSearchResults.Add(new string[] { obj.ID, obj.Name, obj.Type, sMag, obj.GalaxyType, sDistance, obj.Catalogue, RA, Dec, obj.RA2000.ToString(), obj.Dec2000.ToString(), obj.Size, obj.PosAngle.ToString(), obj.Constellation});
                }

                string objectLabel = "";
                if (Properties.Settings.Default.soID)
                {
                    objectLabel = obj.ID + " ";
                }
                if (Properties.Settings.Default.soNames)
                {
                    objectLabel += obj.Name + " ";
                }
                if (Properties.Settings.Default.soMag)
                {
                    if (obj.Magnitude != 999)
                    {
                        objectLabel += Math.Round(obj.Magnitude, 2) + " ";
                    }
                }
                if (Properties.Settings.Default.soType)
                {
                    objectLabel += obj.Type + " ";
                }
                if (Properties.Settings.Default.soGalaxyType)
                {
                    objectLabel += obj.GalaxyType + " ";
                }

                if (Properties.Settings.Default.soDistance)
                {
                    if (double.TryParse(obj.Distance, out double distance))
                    {
                        // Radial velocity to distance Mpc (good out to approx 50 Mpc)
                        double dblDistance = distance / Properties.Settings.Default.Hubble;

                        double dblDivider = 0;
                        string sUnit = " ly";

                        switch (Properties.Settings.Default.DistanceUnit)
                        {
                            case 0: // Light Years
                                dblDistance = dblDistance * 3261563.7769443;
                                if (dblDistance > 1000000000)
                                {
                                    dblDivider = 1000000000.0;
                                    sUnit = " Bly";
                                }
                                else if (dblDistance > 1000000)
                                {
                                    dblDivider = 1000000.0;
                                    sUnit = " Mly";
                                }
                                break;
                            case 1: // Parsecs
                                sUnit = " pc";
                                dblDivider = 1;
                                dblDistance = dblDistance * 1000000.0;
                                break;
                            case 2:  // Mega Parsecs
                                sUnit = " Mpc";
                                dblDivider = 1;
                                break;
                        }

                        if (distance / Properties.Settings.Default.Hubble > 50)
                        {
                            sUnit += "*";
                        }

                        // Comma seperate 1,000s
                        objectLabel += string.Format("{0:n1}", dblDistance / dblDivider) + sUnit + " ";

                    }
                }

                if (Properties.Settings.Default.ShowRV)
                {
                    if (double.TryParse(obj.Distance, out double radial_velocity))
                    {
                        objectLabel += radial_velocity.ToString() + @" km/s ";
                    }
                }

                if (Properties.Settings.Default.soCatalogue)
                {
                    objectLabel += obj.Catalogue;
                }

                sOut += "[\"" + objectLabel + "\",\"" + RA + "\",\"" + Dec + "\"],";
            }
            // Remove the trailing comma
            sOut = sOut.Substring(0, sOut.Length - 1) + "];";

            DrawObjects(sOut);

            return listOfSearchResults;

        }

        public void DrawObjects(DataTable varObjects)
        {
            string sOut = "obj = [";

            bool bFilterOnDistance = false;
            double DistMin, DistMax = 0.0;

            if (double.TryParse(Properties.Settings.Default.sfDistMin, out DistMin) && double.TryParse(Properties.Settings.Default.sfDistMax, out DistMax))
            {
                bFilterOnDistance = true;
            }


            foreach (DataRow row in varObjects.Rows)
            {
                if (bFilterOnDistance)
                {
                    if (double.TryParse(row["Distance Mpc"].ToString(), out double RV))
                    {
                        // Radial velocity to distance Mpc (good out to approx 50 Mpc)
                        double dblDistance = RV / Properties.Settings.Default.Hubble;

                        if (dblDistance < DistMin || dblDistance > DistMax)
                        {
                            continue;
                        }
                    }
                    else continue;
                }

                string RA = ""; string Dec = "";
                // Format RA/Dec to hms and dms
                RA = row["RA"].ToString();
                Dec = row["Dec"].ToString();

                string objectLabel = "";
                if (Properties.Settings.Default.soID)
                {
                    objectLabel = row["ID"].ToString() + " ";
                }
                if (Properties.Settings.Default.soNames)
                {
                    objectLabel += row["Names"].ToString() + " ";
                }
                if (Properties.Settings.Default.soMag)
                {
                     objectLabel += row["Magnitude"].ToString() + " ";
                }
                if (Properties.Settings.Default.soType)
                {
                    objectLabel += row["Type"].ToString() + " ";
                }
                if (Properties.Settings.Default.soGalaxyType)
                {
                    objectLabel += row["Galaxy Type"].ToString() + " ";
                }

                if (Properties.Settings.Default.soDistance)
                {
                    if (double.TryParse(row["Distance Mpc"].ToString(), out double dblDistance))
                    {
                        double distance = dblDistance;
                        double dblDivider = 0;
                        string sUnit = " ly";

                        switch (Properties.Settings.Default.DistanceUnit)
                        {
                            case 0: // Light Years
                                dblDistance = dblDistance * 3261563.7769443;
                                if (dblDistance > 1000000000)
                                {
                                    dblDivider = 1000000000.0;
                                    sUnit = " Bly";
                                }
                                else if (dblDistance > 1000000)
                                {
                                    dblDivider = 1000000.0;
                                    sUnit = " Mly";
                                }
                                break;
                            case 1: // Parsecs
                                sUnit = " pc";
                                dblDivider = 1;
                                dblDistance = dblDistance * 1000000.0;
                                break;
                            case 2:  // Mega Parsecs
                                sUnit = " Mpc";
                                dblDivider = 1;
                                break;
                        }

                        if (distance / Properties.Settings.Default.Hubble > 50)
                        {
                            sUnit += "*";
                        }

                        // Comma seperate 1,000s
                        objectLabel += string.Format("{0:n1}", dblDistance / dblDivider) + sUnit + " ";

                    }
                }

                if (Properties.Settings.Default.ShowRV)
                {
                    if (double.TryParse(row["Distance Mpc"].ToString(), out double radial_velocity))
                    {
                        objectLabel += (radial_velocity * Properties.Settings.Default.Hubble).ToString() + @" km/s ";
                    }
                }

                if (Properties.Settings.Default.soCatalogue)
                {
                    objectLabel += row["Catalogue"].ToString();
                }

                sOut += "[\"" + objectLabel + "\",\"" + RA + "\",\"" + Dec + "\"],";
            }
            // Remove the trailing comma
            sOut = sOut.Substring(0, sOut.Length - 1) + "];";

            DrawObjects(sOut);
        }

        public void DrawObjects(string varObjects)
        {
            string result = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/scripts/run";


            Color FontColour = Properties.Settings.Default.StFontColour;
            string hexFontColor = $"#{FontColour.R:X2}{FontColour.G:X2}{FontColour.B:X2}";
            FontColour = Properties.Settings.Default.StGraphicColour;
            string hexGraphicColor = $"#{FontColour.R:X2}{FontColour.G:X2}{FontColour.B:X2}";

            string sCode = "\r\nMarkerMgr.deleteAllMarkers();LabelMgr.deleteAllLabels();\r\nfor (i=0;i<obj.length;i++){MarkerMgr.markerEquatorial(obj[i][1], obj[i][2],true,true,\"" + Properties.Settings.Default.StGraphic + "\",\"" + hexGraphicColor + "\"," + Properties.Settings.Default.StGraphicSize + ",true);LabelMgr.labelEquatorial(obj[i][0],obj[i][1], obj[i][2],true," + Properties.Settings.Default.StFontSize + ",\"" + hexFontColor + "\",\"E\",12);}";
            
            File.WriteAllText(ScriptFolder + "\\drawobjects.ssc", varObjects + sCode);

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("id", "drawobjects.ssc");
            WebClient lwebClient = new WebClient();
            
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));
               

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "DrawObjects " + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "DrawObjects ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
        }

        public void ClearObjects()
        {
            string result = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/scripts/direct";

            string sCode = "MarkerMgr.deleteAllMarkers();LabelMgr.deleteAllLabels();";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("code", sCode);

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));


                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "ClearObjects " + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "ClearObjects ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
        }

        public string StellariumGetDesignation(string Designation, string Ignore, bool FirstOnly)
        {
            string sOut = "";
            string[] sIDs;
            bool bFirst = true;
            try
            {
                Designation.Replace(" ", "");
                if (Designation != "")
                {
                    sIDs = Designation.Split('-');

                    if (FirstOnly)
                    {
                        return sIDs[0].Trim();
                    }

                    foreach (string sID in sIDs)
                    {
                        if (sID.Trim() != Ignore)
                        {
                            if (!bFirst)
                            {
                                sOut += ", ";
                            }

                            sOut += sID.Trim();
                            bFirst = false;
                        }
                    }
                }
            }
            catch (Exception) { }

            return sOut;
        }

        public string StDegtoArcmins(double StDegrees)
        {
            if (!double.IsNaN(StDegrees))
            {
                return (StDegrees * 60).ToString("#.##");
            }

            return "";
        }

        public APCmdObject StellariumGetSelectedObjectInfo()
        {
            string result = "";
            JsonNode oSelectedObject = null;
            APCmdObject apObject = new APCmdObject();


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

                    apObject.RA2000 = StelRAtoAPRA((double)oSelectedObject["raJ2000"]);
                    apObject.Dec2000 = (double)oSelectedObject["decJ2000"];
                    apObject.Type = APTypeFromStellariumType((string)oSelectedObject["object-type"]);
                    apObject.Catalogue = "Stellarium";
                    apObject.Constellation = (string)oSelectedObject["iauConstellation"];

                    string soName = (string)oSelectedObject["name"];
                    string soDesignation = (string)oSelectedObject["designations"];
                    string soLocalisedName = (string)oSelectedObject["localized-name"];
                    bool bUsedLocalised = false;

                    // Decide which name(s) we are going to use for AP ID
                    if (soLocalisedName != "")
                    {
                        apObject.ID = soLocalisedName;
                        bUsedLocalised = true;
                    }
                    else if (soName != "")
                    {
                        apObject.ID = soName;
                    }
                    else if (soDesignation != "" && soDesignation != null)
                    {
                        apObject.ID = StellariumGetDesignation(soDesignation, "", true);
                    }
                    else
                    {
                        apObject.ID = "Stellarium";
                    }

                    // Add other names to AP Name field
                    if (soDesignation != "" && soDesignation != null)
                    {
                        apObject.Name = StellariumGetDesignation(soDesignation, apObject.ID, false);
                    }

                    if (bUsedLocalised)
                    {
                        if (apObject.Name != null && apObject.Name != "")
                        {
                            apObject.Name += "," + soName;
                        }
                        else
                        {
                            apObject.Name = soName;
                        }
                    }

                    bool bMagFound = false;
                    if (oSelectedObject["vmag"] != null)
                    {
                        if (!double.IsNaN((double)(oSelectedObject["vmag"])))
                        {
                            double vmag = (double)(oSelectedObject["vmag"]);
                            if (vmag < 99)
                            {
                                apObject.Magnitude = vmag;
                                bMagFound = true;
                            }
                        }
                    }

                    if (!bMagFound)
                    {
                        if (oSelectedObject["bMag"]!=null)
                        { 
                            if (!double.IsNaN((double)(oSelectedObject["bmag"])))
                            {
                                double vmag = (double)(oSelectedObject["bmag"]);
                                if (vmag < 99)
                                {
                                    apObject.Magnitude = vmag;
                                    bMagFound = true;
                                }
                            }
                        }
                    }

                    if (!bMagFound)
                    {
                        // No magnitude data  (999 = no magnitude in AP)
                        apObject.Magnitude = 999;
                    }

                    double dblMajorAxis = double.NaN;
                    if (oSelectedObject["axis-major-dd"] != null)
                    { 
                        dblMajorAxis = (double)(oSelectedObject["axis-major-dd"]);
                    }

                    double dblMinorAxis = double.NaN;

                    if (oSelectedObject["axis-minor-dd"] != null)
                    {
                        dblMinorAxis = (double)(oSelectedObject["axis-minor-dd"]);
                    }

                    if (!double.IsNaN(dblMajorAxis) && !double.IsNaN(dblMinorAxis))
                    {
                        if (dblMajorAxis > 0 && dblMinorAxis > 0)
                        {
                            // Convert degrees to arcminutes
                            apObject.Size = StDegtoArcmins(dblMajorAxis) + "x" + StDegtoArcmins(dblMinorAxis);
                        }
                    }
                    else
                    {
                        if (oSelectedObject["size-dd"] != null)
                        {
                            double dblSize = (double)oSelectedObject["size-dd"];
                            if (!double.IsNaN(dblSize))
                            {
                                if (dblSize > 0)
                                {
                                    apObject.Size = StDegtoArcmins(dblSize);
                                }
                            }
                        }
                    }
                }

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "StGetSelectedObj (" + elapsedTime + ")\r\n";
            }
            catch (Exception e)
            {
                sMsg = "StGetSelectedObj ERROR - " + e.Message  + "\r\n";
                apObject = null;
            }
            finally
            {
                lwebClient.Dispose();
            }
            return apObject;
        }

        public void MarkTelescopePosition(string sRA, string sDec, int autoDeleteTimeoutMs)
        {
            string result = "";

            string sWebServiceURL = "http://" + IPAddress + ":" + Port + "/api/scripts/direct";

            string sCode = "MarkerMgr.markerEquatorial(\"" + sRA + "\",\"" + sDec + "\",true,true,\"crossed-circle\",\"#ffff00\",20,true," + autoDeleteTimeoutMs.ToString() + ",false);";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("code", sCode);

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));


                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "ClearObjects " + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception)
            {
                sMsg = "MarkTelescopePosition ERROR \r\n";
            }
            finally
            {
                lwebClient.Dispose();
            }
        }

    }
}
