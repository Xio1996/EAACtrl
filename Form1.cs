using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using System.IO;
using CefSharp.Web;
using System.Text.Json.Serialization;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using CefSharp.DevTools.Network;
using System.Security.Policy;
using uPLibrary.Networking.M2Mqtt;
using CefSharp.DevTools.IO;
using uPLibrary.Networking.M2Mqtt.Messages;
using static EAACtrl.frmEAACP;
using System.ComponentModel.Design;

namespace EAACtrl
{
    public partial class frmEAACP : Form
    {
       // private WebClient webClient = new WebClient();
        private static System.Timers.Timer aTimer;
        private int iCmdCount = 0;
        private bool bExpanded = false;
        private bool bOverlayVisible = true;
        private frmTextOverlay frmTextOverlay = new frmTextOverlay();

        private MqttClient mqttSharpCap;
        private string mqttClientID = "";
        private string mqttBroker = "192.168.0.143";

        private bool bmStandardProfile = true;
        private bool bmSAMPConnected = false;
        private string sSAMP_PrivateKey = "";
        private string sSamp_hub_url = @"http://127.0.0.1:21012"; //Web Profile

        private string StarryNightMsgPath = @"C:\StarryNightMsg\snmsg.txt";

        void SharpCap_MqttMsgReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string[] CmdParams = null;

            // handle message received
            string sTopic = e.Topic;
            string sMsg = Encoding.UTF8.GetString(e.Message);

            if (sMsg != "")
            {
                CmdParams = sMsg.Split('|');
            }

            if (sTopic == "SharpCap/out")
            {
                switch (CmdParams[0])
                {
                    case "FAILED":
                        WriteMessage("SharpCap - FAILED:" + CmdParams[1] + "\r\n");
                        break;
                    case "Log":
                        ProcessCaptureInfo(true, CmdParams[1], CmdParams[2], CmdParams[3], CmdParams[4]);
                        break;
                    case "LogAppend":
                        ProcessCaptureInfo(false, CmdParams[1], CmdParams[2], CmdParams[3], CmdParams[4]);
                        break;
                    default:
                        break;
                }
            }

        }

        private void MQTTDisconnect()
        {
            if (mqttSharpCap != null)
            {
                if (mqttSharpCap.IsConnected)
                {
                    mqttSharpCap.Disconnect();
                }
            }
        }

        private void MQTTConnect()
        {
            bool bConnect = false;

            if (mqttSharpCap == null)
            {
                WriteMessage("MQTT Initialise\r\n");
                mqttSharpCap = new MqttClient(mqttBroker);
                mqttClientID = Guid.NewGuid().ToString();
                mqttSharpCap.MqttMsgPublishReceived += SharpCap_MqttMsgReceived;

                bConnect = true;
            }
            else if (!mqttSharpCap.IsConnected)
            {
                bConnect = true;
            }

            if (bConnect)
            {
                WriteMessage("MQTT connect...\r\n");
                mqttSharpCap.Connect(mqttClientID);
                mqttSharpCap.Subscribe(new string[] { "SharpCap/out" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                WriteMessage("MQTT connected: Broker " + mqttBroker + " on 1883, Protocol " + mqttSharpCap.ProtocolVersion.ToString() + "\r\n");
            }
        }
        private void SharpCapCmd(string cmd)
        {
            MQTTConnect();

            WriteMessage("SharpCapCmd: " + cmd + "\r\n");
            mqttSharpCap.Publish("SharpCap/command", Encoding.UTF8.GetBytes(cmd));
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "0";
            this.SendAPCmd("KeepAlive", oAPCmd);
            //this.SendAPCmdAsync("0", "", "KeepAlive");
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(5000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public frmEAACP()
        {
            InitializeComponent();

            cbCaptureProfile.SelectedIndex = Properties.Settings.Default.CaptureMode;
            tabPlanetarium.SelectedIndex = Properties.Settings.Default.Planatarium;
            cbTextOverlay.Checked = Properties.Settings.Default.TextOverlay;
            cbSlewOnTarget.Checked = Properties.Settings.Default.SlewOnTarget;
            cbImagerZoom.Checked = Properties.Settings.Default.ImagerFOV;
            cbFOVICorr.Checked = Properties.Settings.Default.FOVICorr;
            cbSyncPlanetarium.Checked = Properties.Settings.Default.SyncPlanetariumOnTarget;
            cbAPGetObjects.SelectedIndex = Properties.Settings.Default.APGetObjectsFormat;
            cbPLGetObject.SelectedIndex = Properties.Settings.Default.PLGetObjectsFormat;
            bExpanded = Properties.Settings.Default.Expanded;
            tcExtra.SelectedIndex = Properties.Settings.Default.ExpandedTab;

            //PlanetariumUI(tabPlanetarium.SelectedIndex);

            if (Properties.Settings.Default.YPos == -1)
            {
                this.CenterToScreen();
            }
            else
            {
                if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width > Properties.Settings.Default.XPos + this.Width)
                {
                    this.Left = Properties.Settings.Default.XPos;
                    if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height > Properties.Settings.Default.YPos + this.Height)
                    {
                        this.Top = Properties.Settings.Default.YPos;
                    }
                    else
                    {
                        this.CenterToScreen();
                    }
                }
                else
                {
                    this.CenterToScreen();
                }
            }

            SetTimer();
        }

        private void frmEAACP_Shown(object sender, EventArgs e)
        {
            if (bExpanded)
            {
                bExpanded = false;
                ExpandUI();
            }
        }

        private void frmEAACP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sSAMP_PrivateKey != "")
            {
                SampDisconnect(sSAMP_PrivateKey);
            }

            Properties.Settings.Default.CaptureMode = cbCaptureProfile.SelectedIndex;
            Properties.Settings.Default.Planatarium = tabPlanetarium.SelectedIndex;
            Properties.Settings.Default.TextOverlay = cbTextOverlay.Checked;
            Properties.Settings.Default.SlewOnTarget = cbSlewOnTarget.Checked;
            Properties.Settings.Default.ImagerFOV = cbImagerZoom.Checked;
            Properties.Settings.Default.FOVICorr = cbFOVICorr.Checked;
            Properties.Settings.Default.YPos = this.Top;
            Properties.Settings.Default.XPos = this.Left;
            Properties.Settings.Default.SyncPlanetariumOnTarget = cbSyncPlanetarium.Checked;
            Properties.Settings.Default.APGetObjectsFormat = cbAPGetObjects.SelectedIndex;
            Properties.Settings.Default.PLGetObjectsFormat = cbPLGetObject.SelectedIndex;
            Properties.Settings.Default.Expanded = bExpanded;
            Properties.Settings.Default.ExpandedTab = tcExtra.SelectedIndex;
            Properties.Settings.Default.Save();

            MQTTDisconnect();
        }

        private string sTSScriptHeader = "/* Java Script */\r\n/* Socket Start Packet */\r\n";
        private string sSetTSTargetPosition = "\r\n\r\nvar Out=\"\";\r\nvar ObjectFound = false;\r\n\r\n/* Find target and centre or if not found set chart centre position */\r\ntry {\r\n\tsky6StarChart.Find(ObjectName);\r\n\t/* Centre target */\r\n\tTheSkyXAction.execute(\"TARGET_CENTER\");\r\n\tObjectFound=true;\r\n}\r\ncatch(err){\tOut = err.message;}\r\n\r\ntry {\r\n\tif (!ObjectFound) {\r\n\t\t/* set RA and Dec in decimal degrees */\r\n\t\tsky6StarChart.RightAscension = RA;\r\n\t\tsky6StarChart.Declination = Dec;\r\n\t\tObjectFound = true;\r\n\t}\r\n}\r\ncatch (err) { Out += err.message; }\r\n\r\nif (ObjectFound) {\r\n\t/* Set FOV degrees */\r\n\tif (FOV > 0) {\r\n\t\tsky6StarChart.FieldOfView = FOV;\r\n\t}\r\n}\r\n\r\n/* Socket End Packet */";


        private string TCPMessage(String server, Int32 port, String message)
        {
            // String to store the response ASCII representation.
            String responseData = String.Empty;

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                // Receive the server response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                stream.Close();
                client.Close();
                
                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                WriteMessage("TCP Message (" + elapsedTime + ")\r\n");

            }
            catch (Exception)
            {
                ;
            }

            return responseData;
        }

        private string SampRegister(bool StandardProfile = true)
        {
            string result = "";
            //string sXML = "<?xml version='1.0'?><methodCall><methodName>samp.hub.register</methodName><params><param><value><struct><member><name>samp.name</name><value><string>EAACtrl</string></value></member></struct></value></param></params></methodCall>";
            string sSAMPRegister = "<?xml version='1.0'?><methodCall><methodName>samp.hub.register</methodName><params><param><value>[SECRETorNAME]</value></param></params></methodCall>";

            if (StandardProfile)
            {
                // Locate home folder and read the lock file
                string sHomeFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                if (File.Exists(sHomeFolder + @"\.samp"))
                {
                    // Read the lock file
                    string sSAMPLockFileContent = File.ReadAllText(sHomeFolder + @"\.samp");

                    int ipos = sSAMPLockFileContent.IndexOf("samp.secret=");
                    int ilastpos = sSAMPLockFileContent.IndexOf(Environment.NewLine, ipos);

                    string sSamp_secret = sSAMPLockFileContent.Substring(ipos + 12, ilastpos - (ipos + 12));

                    sSAMPRegister = sSAMPRegister.Replace("[SECRETorNAME]", sSamp_secret);

                    ipos = sSAMPLockFileContent.IndexOf("samp.hub.xmlrpc.url=");
                    ilastpos = sSAMPLockFileContent.IndexOf(Environment.NewLine, ipos);

                    sSamp_hub_url = sSAMPLockFileContent.Substring(ipos + 20, ilastpos - (ipos + 20));

                    WriteMessage("SampRegister: Standard Profile, " + "Secret=" + sSamp_secret + ", " + "URL=" + sSamp_hub_url + "\r\n");
                }
                else { return ("nolockfile"); }
            }
            else 
            {
                bmStandardProfile = false;
                sSAMPRegister = sSAMPRegister.Replace("[SECRETorNAME]", "EAACtrl");
                sSAMPRegister = sSAMPRegister.Replace("samp.hub.register", "samp.webhub.register");
                WriteMessage("SampRegister: Web Profile, " + "URL=" + sSamp_hub_url + "\r\n");
            }
            
            WebClient lwebClient = new WebClient();
            
            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(sSamp_hub_url, "POST", sSAMPRegister);

                if (result != "")
                {
                    int iStart = 0, iEnd = 0, iPos = 0;
                    iPos = result.IndexOf("samp.private-key", 0);
                    iStart = result.IndexOf("<value>", iPos) + 7;
                    iEnd = result.IndexOf("</value>", iStart);

                    result = result.Substring(iStart, iEnd - iStart);

                    sSAMP_PrivateKey = result;

                    TimeSpan ts = stopwatch.Elapsed;

                    string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                    WriteMessage("SampRegister: key=" + result + " (" + elapsedTime + ")\r\n");

                    bmSAMPConnected = true;

                }
                else
                {
                    WriteMessage("SampRegister: Can't Connect!\r\n");
                }
            }
            catch (Exception e)
            {
                WriteMessage("SampRegister ERROR " + e.Message + "\r\n");
                result = "exception";
            }
            finally
            {
                lwebClient?.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string SampDisconnect(string sPrivateKey)
        {
            string result = "";
            //string sXML = "<?xml version='1.0'?><methodCall><methodName>samp.webhub.register</methodName><params><param><value><struct><member><name>samp.name</name><value><string>EAACtrl</string></value></member></struct></value></param></params></methodCall>";
            string sSAMPDisconnect = "<?xml version='1.0'?><methodCall><methodName>samp.hub.unregister</methodName><params><param><value>" + sPrivateKey + "</value></param></params></methodCall>";
            
            if (!bmStandardProfile)
            {
                sSAMPDisconnect = sSAMPDisconnect.Replace("samp.hub.unregister", "samp.webhub.unregister");
            }

            WebClient lwebClient = new WebClient();
            
            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(sSamp_hub_url, "POST", sSAMPDisconnect);

                bmSAMPConnected = false;

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                WriteMessage("SampDisconnect (" + elapsedTime + ")\r\n");
            }
            catch (Exception e)
            {
                WriteMessage("SetAction ERROR " + e.Message + "\r\n");
                result = "exception";
            }
            finally 
            { 
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string SampMetaData(string sPrivateKey)
        {
            string result = "";
            string sSAMPMetaData = "<?xml version='1.0'?><methodCall><methodName>samp.hub.declareMetadata</methodName><params><param><value><string>[PRIVATEKEY]</string></value></param>" +
                "<param><value><struct><member><name>samp.name</name><value><string>EAACtrl</string></value></member><member><name>samp.description</name>" +
                "<value><string>Manges EAA workflow by coordinating astronomy apps such as AstroPlanner, SharpCap, Stellarium etc.</string></value></member>" +
                "<member><name>samp.author</name><value><string>Pete Gallop</string></value></member></struct></value></param></params></methodCall>";

            sSAMPMetaData = sSAMPMetaData.Replace("[PRIVATEKEY]", sPrivateKey);

            if (!bmStandardProfile)
            {
                sSAMPMetaData = sSAMPMetaData.Replace("samp.hub.declareMetadata", "samp.webhub.declareMetadata");
            }

            WebClient lwebClient = new WebClient();

            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(sSamp_hub_url, "POST", sSAMPMetaData);

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                WriteMessage("SampMetaData (" + elapsedTime + ")\r\n");

            }
            catch (Exception e)
            {
                WriteMessage("SampMetaData " + e.Message + "\r\n");
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string Samp_coord_pointAt_sky(string sPrivateKey, string RA, string Dec)
        {

            if (!bmSAMPConnected)
            { return "NOTCONNECTED"; }

            string result = "";
            string sSAMPCoordPointAt = "<?xml version='1.0'?><methodCall><methodName>samp.hub.notifyAll</methodName><params>";
            sSAMPCoordPointAt += "<param><value>" + sPrivateKey + "</value></param>";
            sSAMPCoordPointAt += "<param><value><struct><member><name>samp.mtype</name><value>coord.pointAt.sky</value></member><member><name>samp.params</name><value><struct>";
            sSAMPCoordPointAt += "<member><name>ra</name><value>" + RA + "</value></member>";
            sSAMPCoordPointAt += "<member><name>dec</name><value>" + Dec + "</value></member>";
            sSAMPCoordPointAt += "</struct></value></member></struct></value></param></params></methodCall>";

            WebClient lwebClient = new WebClient();

            if (!bmStandardProfile)
            {
                sSAMPCoordPointAt = sSAMPCoordPointAt.Replace("samp.hub.notifyAll", "samp.webhub.notifyAll");
            }

            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(sSamp_hub_url, "POST", sSAMPCoordPointAt);

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                WriteMessage("SampCoordPointAt " + RA + "," + Dec + " (" + elapsedTime + ")\r\n");

            }
            catch (Exception e)
            {
                WriteMessage("SampCoordPointAt " + e.Message + "\r\n");
                result = "exception";
            }
            finally 
            { 
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string SetStelAction(string sName)
        {
            string result = "";

            string sWebServiceURL = @"http://localhost:8090/api/stelaction/do";

            WebClient lwebClient = new WebClient();

            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.UploadString(sWebServiceURL, "POST", "id=" + sName);

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                WriteMessage("SetStAction: " + sName + "(" + elapsedTime + "\r\n");

            }
            catch (Exception e)
            {
                WriteMessage("SetStAction ERROR " + e.Message + "\r\n");
                result = "exception";
            }
            finally 
            { 
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string SetStelProperty(string sName, string sValue)
        {
            string result = "";

            string sWebServiceURL = @"http://localhost:8090/api/stelproperty/set";
            WebClient lwebClient = new WebClient();
            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.UploadString(sWebServiceURL, "POST", "id=" + sName + "&value=" + sValue);

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                WriteMessage("SetStProp: " + sName + ":" + sValue + "(" + elapsedTime + "\r\n");
            }
            catch (Exception e)
            {
                WriteMessage("SetStProp ERROR " + e.Message + "\r\n");
                result = "exception";
            }
            finally 
            {
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string SetStellariumFOV(int iFOV)
        {
            string result = "";

            string sWebServiceURL = "http://localhost:8090/api/main/fov";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("fov", iFOV.ToString());

            WebClient lwebClient = new WebClient();

            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                WriteMessage("SetStFOV " + iFOV.ToString() + "deg (" + elapsedTime + ")\r\n");
            }
            catch (Exception)
            {
                WriteMessage("SetStFOV ERROR \r\n");
            }
            finally 
            { 
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string SyncStellariumToPosition(double RA, double Dec)
        {
            string result = "";
            string sWebServiceURL = "http://localhost:8090/api/main/focus";
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
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                WriteMessage("SyncStToPos " + sPos + " (" + elapsedTime + ")\r\n");
            }
            catch (Exception)
            {
                WriteMessage("SyncStToPos ERROR \r\n");
            }
            finally
            {
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }


        private string StellariumToAltAzPosition(double Alt, double Az)
        {
            string result = "";
            string sWebServiceURL = "http://localhost:8090/api/main/view";
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
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                WriteMessage("SyncStToAltAz " + sPos + " (" + elapsedTime+ ")\r\n");
            }
            catch (Exception)
            {
                WriteMessage("SyncStToAltAz ERROR \r\n");
            }
            finally 
            { 
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string StellariumMove(double X, double Y)
        {
            string result = "";
            string sWebServiceURL = "http://localhost:8090/api/main/move";
            string sPos = X.ToString() + ", " + Y.ToString();

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("x", X.ToString());
            nvcParams.Add("y", Y.ToString());

            WebClient lwebClient = new WebClient();

            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                WriteMessage("StMove " + sPos + " (" + elapsedTime + ")\r\n");
            }
            catch (Exception)
            {
                WriteMessage("StMove ERROR \r\n");
            }
            finally
            {
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string SyncStellariumToID(string sID)
        {
            string result = "";

            string sWebServiceURL = "http://localhost:8090/api/main/focus";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("target", sID);

            WebClient lwebClient = new WebClient();

            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                WriteMessage("SyncToID " + sID + " (" + elapsedTime + ")\r\n");
            }
            catch (Exception)
            {
                WriteMessage("SyncStToID ERROR \r\n");
            }
            finally 
            { 
                lwebClient?.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string SyncStellariumToAPObject(string sID, string sRA, string sDec, string sType)
        {
            string result = "";

            string sWebServiceURL = "http://localhost:8090/api/scripts/direct";

            string sInput = "sObject=\"" + sID + "\";sRA=\"" + sRA + "\";sDec=\"" + sDec + "\";sType=\"" + sType + "\";\r\n";
            string sCode = "objmap = core.getObjectInfo(sObject);\r\nsInfo = new String(core.mapToString(objmap));\r\nsFound=sInfo.slice(5,10);\r\n\r\nif (sFound==\"found\")\r\n{\r\n\tCustomObjectMgr.addCustomObject(sObject, sRA, sDec, true);\r\n\tcore.selectObjectByName(sObject,true);\r\n\tcore.moveToSelectedObject();\r\n\tStelMovementMgr.setFlagTracking(true);\r\n\tcore.addToSelectedObjectInfoString(\"AP Type: \" + sType,false)\r\n}\r\nelse\r\n{\r\n\tcore.output(\"Object found\");\r\n\tcore.selectObjectByName(sObject,true);\r\n\tcore.moveToSelectedObject();\r\n\tStelMovementMgr.setFlagTracking(true);\r\n}";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("code", sInput + sCode);

            WebClient lwebClient = new WebClient();
            try
            {
                

                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                WriteMessage("SyncStToAPObj " + sID + " " + sRA + ", " + sDec + " (" + elapsedTime + ")\r\n");
            }
            catch (Exception)
            {
                WriteMessage("SyncStToAPObj ERROR \r\n");
            }
            finally 
            { 
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private string StellariumRemoveMarker(string sMarkerName)
        {
            string result = "";

            string sWebServiceURL = "http://localhost:8090/api/scripts/direct";

            string sInput = "sMarker=\"" + sMarkerName + "\";\r\n";
            string sCode = "if (sMarker==\"\")\r\n{\r\n\tCustomObjectMgr.removeCustomObjects();\r\n}\r\nelse\r\n{\r\n\tCustomObjectMgr.removeCustomObject(sMarker);\r\n}";

            NameValueCollection nvcParams = new NameValueCollection();
            nvcParams.Add("code", sInput + sCode);

            WebClient lwebClient = new WebClient();

            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = Encoding.UTF8.GetString(lwebClient.UploadValues(sWebServiceURL, nvcParams));

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                WriteMessage("StRemoveMarker (" + elapsedTime + ")\r\n");
            }
            catch (Exception)
            {
                WriteMessage("StRemoveMarker ERROR \r\n");
            }
            finally 
            {
                lwebClient?.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private StellObject StellariumGetSelectedObjectInfo()
        {
            string result = "";
            StellObject oSelectedObject = null;

            string sWebServiceURL = "http://localhost:8090/api/objects/info?format=json";

            WebClient lwebClient = new WebClient();

            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.DownloadString(sWebServiceURL);
                if (result != "")
                {
                    oSelectedObject = JsonSerializer.Deserialize<StellObject>(result);
                }

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                WriteMessage("StGetSelectedObj (" + elapsedTime + ")\r\n");
            }
            catch (Exception)
            {
                WriteMessage("StGetSelectedObj ERROR \r\n");
            }
            finally 
            { 
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return oSelectedObject;
        }

        private string SendAPCmd(string sCmdName, Root APCmd)
        {
            string result = "";

            string scriptpayload = "http://localhost:8080?cmd=launch&auth=xyz&cmdformat=json&responseformat=json&payload=";
            scriptpayload += Uri.EscapeDataString(JsonSerializer.Serialize<Root>(APCmd));
            WebClient lwebClient = new WebClient();
            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.DownloadString(scriptpayload);

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                iCmdCount++;

                if (APCmd.parameters.cmd != "0")
                {
                    WriteMessage("APCmd " + sCmdName + " (" + elapsedTime + ")\r\n");
                }
                else
                {
                    txtStatusMsg.Text = iCmdCount.ToString() + " " + APCmd.parameters.cmd + "-" + result + " Runtime: " + elapsedTime;
                }
            }
            catch (Exception e)
            {
                if (APCmd.parameters.cmd != "0")
                {
                    WriteMessage("APCmd ERROR " + sCmdName + " " + e.Message + "\r\n");
                }
            }
            finally 
            { 
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        // Connects to Rotator. Calculates the PA and sets rotator angle to PA. This is reflected in TheSky FOVI as long as the FOVI is set as linked to the camera's rotator.
        private void AltAzFOVICorrection()
        {
            try
            {
                string sAltAzFOVICorr = "/* Java Script */\r\n/* Socket Start Packet */\r\nvar Out;\r\n/* Find centre of chart and calculate PA for rotator */\r\ntry {\r\n\tif (ccdsoftCamera.rotatorIsConnected() == 0) {\r\n\t\tccdsoftCamera.rotatorConnect();\r\n\t\tOut = \"Rotator Connected,\"\r\n\t}\r\n\r\n\tif (ccdsoftCamera.rotatorIsConnected() > 0) {\r\n\t\tccdsoftCamera.rotatorGotoPositionAngle(sky6StarChart.Rotation);\r\n\t\tOut = \"  PA:\" + sky6StarChart.Rotation.toFixed(3);\r\n\t}\r\n\telse {\r\n\t\tOut = \"-2\";\r\n\t}\r\n}\r\ncatch (err) { Out = \"-1\"; }\r\n/* Socket End Packet */";
                string sPA = TCPMessage("127.0.0.1", 3040, sAltAzFOVICorr);
                WriteMessage("TS AltAzFOVI" + sPA.Substring(0, sPA.LastIndexOf("|")) + "\r\n");
            }
            catch (Exception) { WriteMessage("TS AltAzFOVI failed!\r\n"); }
        }

        private void SlewToTarget(string RA, string Dec)
        {
            string sSlewScript = "/* Java Script */\r\n/* Socket Start Packet */\r\nif (sky6RASCOMTele.IsConnected==0)//Connect failed for some reason\r\n{\r\n\tOut = \"-1\"\r\n}\r\nelse\r\n{\r\n\tsky6RASCOMTele.Asynchronous = true;\r\n\tsky6RASCOMTele.SlewToRaDec(\"" + RA + "\", \"" + Dec + "\",\"\");\r\n\tOut  = \"0\";\r\n}\r\n/* Socket End Packet */";
            TCPMessage("127.0.0.1", 3040, sSlewScript);
        }

        private void SetTSTargetPosition(string id, string RA, string Dec, double FOV)
        {
            try
            {
                WriteMessage("TS Target Id=" + id + " RA:" + RA.ToString() + " Dec:" + Dec.ToString() + "\r\n");
                string sScriptParams = "var ObjectName = \"" + id + "\";var RA = " + RA + ";var Dec = " + Dec + ";var FOV = " + FOV.ToString() + ";";

                TCPMessage("127.0.0.1", 3040, sTSScriptHeader + sScriptParams + sSetTSTargetPosition);
            }
            catch (Exception)
            {
                WriteMessage("TS Target fail!\r\n");
            }
        }

        private void SetSNTargetPosition(string id, string RA, string Dec, double FOV)
        {
            double dblRA = Double.Parse(RA);
            double dblDec = Double.Parse(Dec);

            WriteMessage("SetSNTarPos " + id + ", " + RA + ", " + Dec + ", " + FOV.ToString() + "\r\n");
            
            File.WriteAllText(StarryNightMsgPath, "target|" + id + "|" + dblRA.ToString() + "|" + dblDec.ToString() + "|" + FOV.ToString());
        }

        private void SetSNFOV(double FOV)
        {
            WriteMessage("SetSNFOV " + FOV.ToString() + "\r\n");
            File.WriteAllText(StarryNightMsgPath, "fov|" + FOV.ToString());

        }

        private void SetSNAltAz(double Alt, double Az, double FOV)
        {
            WriteMessage("SetSNAltAz " + Az.ToString() + ", " + Alt.ToString() + "\r\n");
            File.WriteAllText(StarryNightMsgPath, "altaz|" + Alt.ToString() + "|" + Az.ToString() + "|" + FOV.ToString());
        }

        private void SyncPlanetarium()
        {
            string result = "";
            double dblFOV = -1.0;
            bool bOK = false;

            // Get current AP object name and RA/DEC
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "1";

            result = SendAPCmd("GetTarget", oAPCmd);
            if (result != "")
            {
                SelectedObject oSelectedObject = JsonSerializer.Deserialize<SelectedObject>(result);
                if (oSelectedObject.error == 0 && oSelectedObject.results != null)
                {
                    if (cbImagerZoom.Checked)
                    {
                        dblFOV = 1.05;
                    }

                    switch (tabPlanetarium.SelectedIndex)
                    {
                        case 0: // Stellarium
                            string sResult = SyncStellariumToAPObject(oSelectedObject.results.id,
                                                                   oSelectedObject.results.SRA,
                                                                   oSelectedObject.results.SDec,
                                                                   oSelectedObject.results.Type);

                            if (sResult == "ok")
                            {
                                bOK = true;
                            }

                            if (bOK)
                            {
                                if (cbImagerZoom.Checked)
                                {
                                    SetStellariumFOV(1);
                                }
                            }
                            break;
                        case 1: // TheSky
                            // Minor planets need the MPL prefix to be found as a target in TS
                            if (oSelectedObject.results.Type == "Minor")
                            {
                                oSelectedObject.results.id = "MPL " + oSelectedObject.results.id;
                            }

                            SetTSTargetPosition(oSelectedObject.results.id, oSelectedObject.results.RA, oSelectedObject.results.Dec, dblFOV);

                            if (cbFOVICorr.Checked)
                            {
                                AltAzFOVICorrection();
                            }
                            break;

                        case 2: // Starry Night
                            if (!cbImagerZoom.Checked)
                            {
                                // Do not zoom. Keep current FOV.
                                dblFOV = 0.0;
                            }

                            SetSNTargetPosition(oSelectedObject.results.id, oSelectedObject.results.RA, oSelectedObject.results.Dec, dblFOV);
                            break;
                    }

                    if (sSAMP_PrivateKey!="")
                    {
                        Samp_coord_pointAt_sky(sSAMP_PrivateKey, (double.Parse(oSelectedObject.results.RA)*15).ToString() , oSelectedObject.results.Dec);
                    }
                }
                else
                {
                    WriteMessage("Sync AP-TS Error -1\r\n");
                }
            }
            else
            {
                WriteMessage("Sync AP-TS Error -2\r\n");
            }
        }
        private void SyncPlanetariumOLD()
        {
            string result = "";
            double dblFOV = -1.0;
            bool bOK = false;

            // Get current AP object name and RA/DEC
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "1";

            result = SendAPCmd("GetTarget", oAPCmd);
            if (result != "")
            {
                SelectedObject oSelectedObject = JsonSerializer.Deserialize<SelectedObject>(result);
                if (oSelectedObject.error == 0 && oSelectedObject.results != null)
                {
                    if (cbImagerZoom.Checked)
                    {
                        dblFOV = 1.05;
                    }

                    if (tabPlanetarium.SelectedIndex == 0)
                    {

                        // Minor planets need the MPL prefix to be found as a target in TS
                        if (oSelectedObject.results.Type == "Minor")
                        {
                            oSelectedObject.results.id = "MPL " + oSelectedObject.results.id;
                        }

                        SetTSTargetPosition(oSelectedObject.results.id, oSelectedObject.results.RA, oSelectedObject.results.Dec, dblFOV);

                        if (cbFOVICorr.Checked)
                        {
                            AltAzFOVICorrection();
                        }
                    }
                    else
                    {
                        string sResult = SyncStellariumToID(oSelectedObject.results.id);
                        if (sResult == "true")
                        {
                            WriteMessage("SyncByID AP-ST done.\r\n");
                            bOK = true;
                        }
                        else
                        {
                            sResult = SyncStellariumToPosition(Convert.ToDouble(oSelectedObject.results.RA), Convert.ToDouble(oSelectedObject.results.Dec));
                            if (sResult == "ok")
                            {
                                WriteMessage("SyncByPos AP-ST done.\r\n");
                                bOK = true;
                            }
                            else
                            {
                                WriteMessage("SyncByPos AP-ST ERROR.\r\n");
                            }
                        }

                        if (bOK)
                        {
                            if (cbImagerZoom.Checked)
                            {
                                SetStellariumFOV(1);
                            }
                        }
                    }
                }
                else
                {
                    WriteMessage("Sync AP-TS Error -1\r\n");
                }
            }
            else
            {
                WriteMessage("Sync AP-TS Error -2\r\n");
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            SyncPlanetarium();
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            string sResult = "";
            bool bSlew = false;
            Root oAPCmd = new Root();

            WriteMessage("Target start.\r\n");

            // Get the selected target information from AstroPlanner
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "1";

            sResult = SendAPCmd("GetTarget", oAPCmd);
            if (sResult != "")
            {
                SelectedObject oSelectedObject = JsonSerializer.Deserialize<SelectedObject>(sResult);
                if (oSelectedObject.error == 0 && oSelectedObject.results != null)
                {
                    SetOverlayText(oSelectedObject.results.target);
                    SharpCapCmd("Target|" + oSelectedObject.results.target);
                    frmEAACP.ActiveForm.Text = "EAACtrl - " + oSelectedObject.results.target;
                }
            }
            else
            {
                WriteMessage("AP GetTarget - FAILED!\r\n");
                return;
            }

            if (cbSyncPlanetarium.Checked)
            {
                SyncPlanetarium();
            }

            if (cbSlewOnTarget.Checked)
            {
                DialogResult dialogResult = MessageBox.Show(this, "Confirm telescope SLEW?", "Telescope Slew", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    WriteMessage("Slew confirmed by user.\r\n");
                    bSlew = true;
                }
                else
                {
                    WriteMessage("Slew - cancelled by user.\r\n");
                    bSlew = false;
                }
            }

            if (bSlew)
            {
                // Set the target for SharpCap and slew to the object (if possible)
                oAPCmd = new Root();
                oAPCmd.script = "EAAControl";
                oAPCmd.parameters = new Parameters();
                oAPCmd.parameters.cmd = "8";

                sResult = SendAPCmd("Slew", oAPCmd);
                if (sResult != "")
                {
                    SelectedObject oSelectedObject = JsonSerializer.Deserialize<SelectedObject>(sResult);
                    if (oSelectedObject.error == 0 && oSelectedObject.results != null)
                    {
                        string sStatus = "";
                        // Check for status return
                        switch (oSelectedObject.results.Status)
                        {
                            case "ok":
                                sStatus = "Slew - Telescope slew completed.";
                                break;
                            case "notel":
                                sStatus = "Slew - Telescope is not connected!";
                                break;
                            case "hor":
                                sStatus = "Slew - Obj below horizon!";
                                break;
                            case "slew":
                                sStatus = "Slew - Outside telescope limits!";
                                break;
                            case "uhor":
                                sStatus = "Slew - Obj below user horizon!";
                                break;
                            default:
                                sStatus = "Slew - Unknow result!!";
                                break;
                        }

                        WriteMessage(sStatus + "\r\n");
                    }
                    else
                    {
                        WriteMessage("Slew - FAILED, Results object!\r\n");
                    }
                }
                else
                {
                    WriteMessage("Slew - FAILED, Command failure!\r\n");
                }
            }
        }

        private void btnAzAltFOVI_Click(object sender, EventArgs e)
        {
            AltAzFOVICorrection();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            SharpCapCmd("Find");
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            string sProfile = "";

            switch (cbCaptureProfile.SelectedIndex)
            {
                case 0:
                    sProfile = "CaptureMode1";
                    break; 
                case 1:
                    sProfile = "CaptureMode2";
                    break; 
                case 2:
                    sProfile = "CaptureMode3";
                    break;
            }
            SharpCapCmd("Capture|" + sProfile);
        }

        private void WriteMessage(string sMsg)
        {
            tbMessages.AppendText(DateTime.Now.ToString("HH:mm:ss") + ": " + sMsg);
        }

        private void SetOverlayText(string sObjectName)
        {
            if (sObjectName != "")
            {
                frmTextOverlay.Controls["lblText"].Text = sObjectName;
            }
            else
            {
                frmTextOverlay.Controls["lblText"].Text = @"EAA with an 8-inch SCT";
            }
        }

        private void ProcessCaptureInfo(bool bCreate, string sObjectInfo, string sImageInfo, string sImagePath, string sCameraSettingsPath)
        {
            //Get the objects ID
            if (sObjectInfo != null)
            {
                //string sID = sObjectInfo.Substring(0, sObjectInfo.IndexOf(","));
                WriteMessage("Logging: " + sObjectInfo + "\r\n");

                if (sImagePath != null)
                {
                    // Copy image file to desktop PC SharpCap folder
                    string sDestImagePath = sImagePath.Replace(@"C:\", @"D:\");
                    string sSrcImagePath = sImagePath.Replace(@"C:\", @"\\TERRANOVA\");
                    Directory.CreateDirectory(sDestImagePath.Substring(0, sDestImagePath.LastIndexOf(@"\")));
                    File.Copy(sSrcImagePath, sDestImagePath, true);

                    if (sCameraSettingsPath != null)
                    {
                        // Copy image settings file to desktop PC SharpCap folder
                        string sDestSettingsPath = sCameraSettingsPath.Replace(@"C:\", @"D:\");
                        string sSrcSettingsPath = sCameraSettingsPath.Replace(@"C:\", @"\\TERRANOVA\");
                        Directory.CreateDirectory(sDestSettingsPath.Substring(0, sDestSettingsPath.LastIndexOf(@"\")));
                        File.Copy(sSrcSettingsPath, sDestSettingsPath, true);

                        // Create or append an observation in AstroPlanner
                        Root oAPCmd = new Root();
                        oAPCmd.script = "EAAControl";
                        oAPCmd.parameters = new Parameters();
                        oAPCmd.parameters.param1 = Uri.EscapeDataString(sObjectInfo);
                        oAPCmd.parameters.param2 = Uri.EscapeDataString(sDestSettingsPath);
                        oAPCmd.parameters.param3 = Uri.EscapeDataString(sDestImagePath);
                        oAPCmd.parameters.param4 = Uri.EscapeDataString(sImageInfo);

                        if (bCreate)
                        {
                            // AstroPlanner add observation and attach image.
                            oAPCmd.parameters.cmd = "6";
                            string sRes = SendAPCmd("Log", oAPCmd);
                        }
                        else
                        {
                            // AstroPlanner append observation and attach image to current observation.
                            oAPCmd.parameters.cmd = "7";
                            string sRes = SendAPCmd("LogAppend", oAPCmd);
                        }
                    }
                    else
                    {
                        WriteMessage("ImageSettingsPath is NULL!\r\n");
                    }
                }
                else
                {
                    WriteMessage("ImagePath is NULL!\r\n");
                }
            }
            else
            {
                WriteMessage("ObjectInfo is NULL!\r\n");
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            SharpCapCmd("Log");
        }

        private void ExpandUI()
        {
            if (bExpanded)
            {
                frmEAACP.ActiveForm.Width = 240;
                btnExpand.Text = ">>";
            }
            else
            {
                frmEAACP.ActiveForm.Width = 617;
                btnExpand.Text = "<<";
            }

            bExpanded = !bExpanded;
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            ExpandUI();
        }

        private void frmEAACP_Load(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;

            this.Width = 240;
            if (bOverlayVisible)
            {
                frmTextOverlay.Show();
            }

            if (tabPlanetarium.SelectedIndex == 1)
            {
                SetStelProperty("NebulaMgr.catalogFilters", "7");
            }

            WriteMessage("EAACtrl started.\r\n");
        }

        private void btnSCDSA_Click(object sender, EventArgs e)
        {
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "3";

            string sRes = SendAPCmd("DSA", oAPCmd);
        }

        private void btnAsteroidsFOV_Click(object sender, EventArgs e)
        {
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "4";

            SendAPCmd("ASTEROIDSFOV", oAPCmd);
        }

        private void btnPlanetaryMoons_Click(object sender, EventArgs e)
        {
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "5";

            SendAPCmd("PLMOONS", oAPCmd);
        }

        private void btnLogPlus_Click(object sender, EventArgs e)
        {
            SharpCapCmd("LogAppend");
        }

        private void btnStelGetObjectInfo_Click(object sender, EventArgs e)
        {
            StellObject obj = StellariumGetSelectedObjectInfo();
            if (obj != null) 
            {
                // Convert RA/DEC to CDC portal values and place on clipboard
            }
        }

        private void btnAPGetObject_Click(object sender, EventArgs e)
        {

            if (cbAPGetObjects.SelectedIndex == 4)
            {
                // Get current AP object name and RA/DEC
                Root oAPCmd = new Root();
                oAPCmd.script = "EAAControl";
                oAPCmd.parameters = new Parameters();
                oAPCmd.parameters.cmd = "1";

                string result = SendAPCmd("GetTarget", oAPCmd);
                if (result != "")
                {
                    SelectedObject oSelectedObject = JsonSerializer.Deserialize<SelectedObject>(result);
                    if (oSelectedObject.error == 0 && oSelectedObject.results != null)
                    {
                        string RA = (double.Parse(oSelectedObject.results.RA)*15).ToString();
                        Samp_coord_pointAt_sky(sSAMP_PrivateKey, RA, oSelectedObject.results.Dec);
                    }
                }
            }
            else
            {
                Root oAPCmd = new Root();
                oAPCmd.script = "EAAControl";
                oAPCmd.parameters = new Parameters();
                oAPCmd.parameters.cmd = "9";
                oAPCmd.parameters.param1 = cbAPGetObjects.SelectedIndex.ToString();

                string sResult = SendAPCmd("GETOBJECTS", oAPCmd);
                if (sResult != "")
                {
                    SelectedObjects oSelectedObjects = JsonSerializer.Deserialize<SelectedObjects>(sResult);
                    if (oSelectedObjects.error == 0)
                    {
                        if (oSelectedObjects.results.Status == "ok")
                        {
                            // We have the objects!
                            // For now AP will add the required format to the clipboard
                            // Later on I may want to process the selected objects list here.
                        }
                    }
                }
            }
        }

 

        private void btnAzAltFOVI_Click_1(object sender, EventArgs e)
        {
            AltAzFOVICorrection();
        }

        private void btnN_Click_1(object sender, EventArgs e)
        {
            //SetStelAction("actionLook_Towards_North");
            StellariumToAltAzPosition(45, 0);
            SetStellariumFOV(85);
        }
        

        private void btnW_Click_1(object sender, EventArgs e)
        {
            //SetStelAction("actionLook_Towards_West");
            StellariumToAltAzPosition(45, 270);
            SetStellariumFOV(85);
        }

        private void btnS_Click_1(object sender, EventArgs e)
        {
            //SetStelAction("actionLook_Towards_South");
            StellariumToAltAzPosition(45, 180);
            SetStellariumFOV(85);
        }

        private void btnE_Click_1(object sender, EventArgs e)
        {
            //SetStelAction("actionLook_Towards_East");
            StellariumToAltAzPosition(45, 90);
            SetStellariumFOV(85);
        }

        private void btnNV_Click_1(object sender, EventArgs e)
        {
            SetStellariumFOV(72);
        }

        private void btnCV_Click_1(object sender, EventArgs e)
        {
            SetStellariumFOV(36);
        }

        private void btnAllCats_Click_1(object sender, EventArgs e)
        {
            switch (btnAllCats.Text)
            {
                case "Def Cats":
                    if (SetStelProperty("NebulaMgr.catalogFilters", "255852135") != "exception")
                    {
                        btnAllCats.Text = "All Cats";
                    }
                    break;
                case "All Cats":
                    if (SetStelProperty("NebulaMgr.catalogFilters", "255852279") != "exception")
                    {
                        btnAllCats.Text = "All+D Cats";
                    }
                    break;
                case "All+D Cats":
                    if (SetStelProperty("NebulaMgr.catalogFilters", "7") != "exception")
                    {
                        btnAllCats.Text = "Def Cats";
                    }
                    break;
            }
        }

        private void btnSNN_Click(object sender, EventArgs e)
        {
            SetSNAltAz(45, 0, 118);
        }

        private void btnSNW_Click(object sender, EventArgs e)
        {
            SetSNAltAz(45, 270, 118);
        }

        private void btnSNS_Click(object sender, EventArgs e)
        {
            SetSNAltAz(45, 180, 118);
        }

        private void btnSNE_Click(object sender, EventArgs e)
        {
            SetSNAltAz(45, 90, 118);
        }

        private void btnSNNV_Click(object sender, EventArgs e)
        {
            SetSNFOV(118);
        }

        private void btnSNCV_Click(object sender, EventArgs e)
        {
            SetSNFOV(36);
        }

        private void btnStellariumClearMarkers_Click(object sender, EventArgs e)
        {
            // Removes all markers from Stellarium that were created manually (shift + left click) or via the EAACtrl app.
            StellariumRemoveMarker("");
        }

        private void btnSAMPConnect_Click(object sender, EventArgs e)
        {
            SampRegister();
            if (sSAMP_PrivateKey != "")
            {
                SampMetaData(sSAMP_PrivateKey);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SampDisconnect(sSAMP_PrivateKey);
        }

        private void btnOverlayText_Click_2(object sender, EventArgs e)
        {
            frmTextOverlay.Controls["lblText"].Text = txtOverlay.Text;
        }

        private void cbTextOverlay_CheckedChanged_1(object sender, EventArgs e)
        {
            if (bOverlayVisible)
            {
                bOverlayVisible = false;
                frmTextOverlay.Hide();
            }
            else
            {
                bOverlayVisible = true;
                frmTextOverlay.Show();
            }
        }
    }

    public class Results
    {
        public string Status { get; set; }
        public string RA { get; set; }
        public string id { get; set; }
        public string Dec { get; set; }
        public string Type { get; set; }
        public string target { get; set; }
        public string SRA { get; set; }
        public string SDec { get; set; }
    }

    public class SelectedObject
    {
        public int error { get; set; }
        public Results results { get; set; }
    }

    // Selected Objects returned JSON objects
    public class APObject
    {
        public string target { get; set; }
        public string RA { get; set; }
        public string sDec { get; set; }
        public string sRA { get; set; }
        public string id { get; set; }
        public string Type { get; set; }
        public string Dec { get; set; }
    }

    public class SelectedObjectsResults
    {
        public string Status { get; set; }
        public List<APObject> APObjects { get; set; }
    }

    public class SelectedObjects
    {
        public int error { get; set; }
        public SelectedObjectsResults results { get; set; }
    }

    //AP Command JSON
    public class Parameters
    {
        public string cmd { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string param4 { get; set; }
    }

    public class Root
    {
        public string script { get; set; }
        public Parameters parameters { get; set; }
    }

    public class StellObject
    {
        public double decJ2000 { get; set; }
        public double raJ2000 { get; set; }
        public string type { get; set; }

    }
}