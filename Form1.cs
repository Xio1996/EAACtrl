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

namespace EAACtrl
{
    public partial class frmEAACP : Form
    {
        private WebClient webClient = new WebClient();
        private static System.Timers.Timer aTimer;
        private int iCmdCount = 0;
        private bool bExpanded = false;
        private bool bOverlayVisible = true;
        private frmTextOverlay frmTextOverlay= new frmTextOverlay();

        //Path to Laptop SharpCap Captures folder
        private string csSharpCapPath = @"\\TERRANOVA\SharpCap Captures\";

        private  void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",e.SignalTime);
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "0";
            this.SendAPCmd("KeepAlive", oAPCmd);
            //this.SendAPCmdAsync("0", "", "KeepAlive");
        }

        private  void SetTimer()
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
            
            cbCaptureProfile.SelectedIndex = 0;

            SetTimer();

           // SendAPCmdAsync("0", "", "KeepAlive");
        }

        private string sTSScriptHeader = "/* Java Script */\r\n/* Socket Start Packet */\r\n";
        private string sSetTSTargetPosition = "\r\n\r\nvar Out=\"\";\r\nvar ObjectFound = false;\r\n\r\n/* Find target and centre or if not found set chart centre position */\r\ntry {\r\n\tsky6StarChart.Find(ObjectName);\r\n\t/* Centre target */\r\n\tTheSkyXAction.execute(\"TARGET_CENTER\");\r\n\tObjectFound=true;\r\n}\r\ncatch(err){\tOut = err.message;}\r\n\r\ntry {\r\n\tif (!ObjectFound) {\r\n\t\t/* set RA and Dec in decimal degrees */\r\n\t\tsky6StarChart.RightAscension = RA;\r\n\t\tsky6StarChart.Declination = Dec;\r\n\t\tObjectFound = true;\r\n\t}\r\n}\r\ncatch (err) { Out += err.message; }\r\n\r\nif (ObjectFound) {\r\n\t/* Set FOV degrees */\r\n\tif (FOV > 0) {\r\n\t\tsky6StarChart.FieldOfView = FOV;\r\n\t}\r\n}\r\n\r\n/* Socket End Packet */";


        static string TCPMessage(String server, Int32 port, String message)
        {
            // String to store the response ASCII representation.
            String responseData = String.Empty;

            try
            {
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
            }
            catch (Exception)
            {
;
            }

            return responseData;
        }

        private string SendAPCmd(string sCmdName, Root APCmd)
        {
            string result = "";
            //cmd = Uri.EscapeDataString(cmd);
            //cmdparams = Uri.EscapeDataString(cmdparams);

            string scriptpayload = "http://localhost:8080?cmd=launch&auth=xyz&cmdformat=json&responseformat=json&payload=";
            scriptpayload += Uri.EscapeDataString(JsonSerializer.Serialize<Root>(APCmd));
            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = webClient.DownloadString(scriptpayload);

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                txtStatusMsg.Text = iCmdCount.ToString() + " " + APCmd.parameters.cmd + "-" + result + " Runtime: " + elapsedTime;
                iCmdCount++;

                if (APCmd.parameters.cmd != "0")
                {
                    WriteMessage("APCmd=" + sCmdName + " " + elapsedTime + "\r\n");
                }
            }
            catch (Exception)
            {
                if (APCmd.parameters.cmd != "0")
                {
                    WriteMessage("APCmd ERROR " + sCmdName + "\r\n");
                }
            }

            aTimer.Enabled = true;

            return result;
        }

        // Connects to Rotator. Calculates the PA and sets rotator angle to PA. This is reflected in TheSky FOVI as long as the FOVI is set as linked to the camera's rotator.
        private void AltAzFOVICorrection()
        {
            try
            {
                string sAltAzFOVICorr = "/* Java Script */\r\n/* Socket Start Packet */\r\nvar Out;\r\n/* Find centre of chart and calculate PA for rotator */\r\ntry\r\n{\r\n\tif (ccdsoftCamera.rotatorIsConnected() == 0)\r\n\t{\r\n\t\tccdsoftCamera.rotatorConnect();\r\n\t\tOut = \"Rotator Connected,\"\r\n\t}\r\n\r\n\tif (ccdsoftCamera.rotatorIsConnected() > 0)\r\n\t{\r\n\t\tvar cRA = sky6StarChart.RightAscension;\r\n\t\tvar cDec = sky6StarChart.Declination;\r\n\r\n\t\tsky6Utils.ConvertRADecToAzAlt(cRA, cDec)\r\n\t\tsky6Utils.ConvertAzAltToRADec(sky6Utils.dOut0, sky6Utils.dOut1 - 0.25)\r\n\r\n\t\tsky6Utils.ComputePositionAngle(cRA, cDec, sky6Utils.dOut0, sky6Utils.dOut1)\r\n\r\n\t\tccdsoftCamera.rotatorGotoPositionAngle(sky6Utils.dOut0);\r\n\r\n\t\tOut = \" RA:\" + cRA.toFixed(3) + \" Dec:\" + cDec.toFixed(3) + \" PA:\" + sky6Utils.dOut0.toFixed(3);\r\n\t}\r\n\telse {\r\n\t\tOut = \"-2\";\r\n\t}\r\n}\r\ncatch (err) { Out = \"-1\"; }\r\n/* Socket End Packet */\r\n";
                string sPA = TCPMessage("127.0.0.1", 3040, sAltAzFOVICorr);
                WriteMessage("TS AltAzFOVI" + sPA.Substring(0, sPA.LastIndexOf("|")) + "\r\n");
            }
            catch(Exception) { WriteMessage("TS AltAzFOVI failed!\r\n"); }
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
            catch(Exception)
            {
                WriteMessage("TS Target fail!\r\n");
            }
        }

        private void SharpCapMsg(string Command)
        {
            try
            {
                WriteMessage("SC Cmd= " + Command + "\r\n");
                File.WriteAllText(csSharpCapPath + @"SCIPC.txt", Command);
            }
            catch (Exception)
            {
                WriteMessage("SC Msg ERROR= " + Command + "\r\n");
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            string result = "";
            double dblFOV = -1.0;

            // Get current AP object name and RA/DEC
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "1";

            result = SendAPCmd("GetSelectedObjects",oAPCmd);
            if (result!="")
            {
                SelectedObject oSelectedObject = JsonSerializer.Deserialize<SelectedObject>(result);
                if (oSelectedObject.error == 0 && oSelectedObject.results != null)
                {
                    if (cbImagerZoom.Checked)
                    {
                        dblFOV = 1.05;
                    }

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
                    WriteMessage("Sync AP-TS Error -1\r\n");
                }
            }
            else
            {
                WriteMessage("Sync AP-TS Error -2\r\n");
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {

            string sObjectInfoPath = csSharpCapPath + @"ObjectInfo.txt";
            Root oAPCmd = new Root();
            oAPCmd.script = "EAAControl";
            oAPCmd.parameters = new Parameters();
            oAPCmd.parameters.cmd = "2";
            try
            {
                // Remove the old target
                if (File.Exists(sObjectInfoPath))
                {
                    File.Delete(sObjectInfoPath);
                }
            }
            catch(Exception ex)
            {
                WriteMessage ($"Target file delete err: {ex.Message}");
            }

            string result = SendAPCmd("SetTarget", oAPCmd);
            
            SetOverlayText();

            if (cbSlewOnTarget.Checked)
            {
                // Get current AP object name and RA/DEC
                oAPCmd = new Root();
                oAPCmd.script = "EAAControl";
                oAPCmd.parameters = new Parameters();
                oAPCmd.parameters.cmd = "2";

                result = SendAPCmd("GetSelectedObjects", oAPCmd);
                if (result != "")
                {
                    SelectedObject oSelectedObject = JsonSerializer.Deserialize<SelectedObject>(result);
                    if (oSelectedObject.error == 0 && oSelectedObject.results != null)
                    {
                        SlewToTarget(oSelectedObject.results.RA, oSelectedObject.results.Dec);
                    }
                }
            }
        }

        private void btnAzAltFOVI_Click(object sender, EventArgs e)
        {
            AltAzFOVICorrection();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            SharpCapMsg("Find");
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            SharpCapMsg(cbCaptureProfile.Text);
        }

        private void WriteMessage(string sMsg)
        {
            
            tbMessages.AppendText(DateTime.Now.ToString("h:mm:ss tt") + ": " + sMsg);
        }

        private void SetOverlayText()
        {
            string sFilename = csSharpCapPath + @"ObjectInfo.txt";
            int iTimeCount = 0;

            while (!File.Exists(sFilename) && iTimeCount <= 5)
            {
                WriteMessage("(" + iTimeCount.ToString() + ") Waiting for target file...\r\n");
                Thread.Sleep(1000);
                iTimeCount++;
            }

            if (File.Exists(sFilename)) 
            { 
                frmTextOverlay.Controls["lblText"].Text = File.ReadAllText(sFilename);
            }
            else
            {
                frmTextOverlay.Controls["lblText"].Text = @"EAA with an 8-inch SCT";
            }
        }

        private void ProcessCaptureInfo(bool bCreate)
        {
            string sCaptureInfoFile = csSharpCapPath + @"CaptureInfo.txt";
            int iTimeCount = 0;

            while (!File.Exists(sCaptureInfoFile) && iTimeCount <=6)
            {
                WriteMessage("(" + iTimeCount.ToString() + ") Waiting for capture...\r\n");
                Thread.Sleep(1000);
                iTimeCount++;
            }

            if (File.Exists(sCaptureInfoFile)) 
            { 
                StreamReader sr = File.OpenText(sCaptureInfoFile);
                string sObjectInfo = sr.ReadLine();
                string sImageInfo = sr.ReadLine(); 
                string sImagePath = sr.ReadLine();
                string sImageSettingsPath = sr.ReadLine();
                
                sr.Close();

                // Copy image and settings file to desktop SharpCap folder
                string sDestImagePath = sImagePath.Replace(@"C:\", @"D:\");
                string sDestSettingsPath = sImageSettingsPath.Replace(@"C:\", @"D:\");

                string sSrcImagePath = sImagePath.Replace(@"C:\", @"\\TERRANOVA\");
                string sSrcSettingsPath = sImageSettingsPath.Replace(@"C:\", @"\\TERRANOVA\");

                Directory.CreateDirectory(sDestImagePath.Substring(0, sDestImagePath.LastIndexOf(@"\")));
                Directory.CreateDirectory(sDestSettingsPath.Substring(0, sDestSettingsPath.LastIndexOf(@"\")));

                File.Copy(sSrcImagePath, sDestImagePath, true);
                File.Copy(sSrcSettingsPath, sDestSettingsPath, true);

                File.Delete(sCaptureInfoFile);

                Root oAPCmd = new Root();
                oAPCmd.script = "EAAControl";
                oAPCmd.parameters = new Parameters();
                oAPCmd.parameters.param1 = Uri.EscapeDataString(sObjectInfo);
                oAPCmd.parameters.param2 = Uri.EscapeDataString(sDestSettingsPath);
                oAPCmd.parameters.param3 = Uri.EscapeDataString(sDestImagePath);

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
                WriteMessage("Logging fail! No file.\r\n");
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
          if (cbLogFind.Checked) 
            {
                SharpCapMsg("LogAndFind");
            }
            else 
            {
                SharpCapMsg("Log");
            }

            // Read CaptureInfo.txt file
            ProcessCaptureInfo(true);
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            if (bExpanded)
            {
                frmEAACP.ActiveForm.Width = 206;
                btnExpand.Text = ">>";
            }
            else
            {
                frmEAACP.ActiveForm.Width = 545;
                btnExpand.Text = "<<";
            }

            bExpanded = !bExpanded;
        }

        private void frmEAACP_Load(object sender, EventArgs e)
        {
            this.Width = 206;
            if (bOverlayVisible)
            {
                frmTextOverlay.Show();
            }
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

        private void cbTextOverlay_CheckedChanged(object sender, EventArgs e)
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

        private void btnLogPlus_Click(object sender, EventArgs e)
        {
            if (cbLogFind.Checked)
            {
                SharpCapMsg("LogAndFind");
            }
            else
            {
                SharpCapMsg("Log");
            }

            // Read CaptureInfo.txt file
            ProcessCaptureInfo(false);
        }
    }

    public class Results
    {
        public string RA { get; set; }
        public string id { get; set; }
        public string Dec { get; set; }
        public string Type { get; set; }
    }

    public class SelectedObject
    {
        public int error { get; set; }
        public Results results { get; set; }
    }

    //AP Command JSON
    public class Parameters
    {
        public string cmd { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }

    }

    public class Root
    {
        public string script { get; set; }
        public Parameters parameters { get; set; }
    }

}
