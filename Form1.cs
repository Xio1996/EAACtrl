using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static EAACtrl.frmEAACP;


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

        private string SelectedRA = "";
        private string SelectedDec = "";

        // *** NEW Objects Feb 2024
        // AstroPlanner Helper class - replaces AP functions in EAACtrl Panel
        private APHelper APHelper = new APHelper();

        // Stellarium Communication and processing class
        private Stellarium Stellarium = new Stellarium();

        // TheSky Professional
        private TheSky TheSky = new TheSky();

        // Starry Night 8 Pro
        private StarryNight StarryNight = new StarryNight();
        
        // CdC
        private SkyChart SkyChart = new SkyChart();

        // SAMP Client class
        private SAMP SAMP = new SAMP();

        void StoreSelectedObject(string RA, string Dec)
        {
            SelectedRA = (double.Parse(RA) * 15).ToString();
            SelectedDec = Dec;
        }

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
            cbSAMPProfile.SelectedIndex = Properties.Settings.Default.SAMPProfile;
            txtSAMPWebURL.Text = Properties.Settings.Default.SAMPWebHub;
            tcExtra.SelectedIndex = Properties.Settings.Default.ExpandedTab;
            cbSAMPSetFOV.SelectedIndex = Properties.Settings.Default.SAMPFOV;
            cbSAMPImage.SelectedIndex = Properties.Settings.Default.SAMPImage;
            txtCdCAddress.Text = Properties.Settings.Default.CdCAddress;
            txtCdCPort.Text = Properties.Settings.Default.CdCPort;
            txtStellariumAddress.Text = Properties.Settings.Default.StellariumAddress;
            txtStellariumPort.Text = Properties.Settings.Default.StellariumPort;

            if (cbSAMPProfile.SelectedIndex == 0)
            {
                SAMP.StandardProfile = true;
            }

            if (txtSAMPWebURL.Text == "")
            {
                txtSAMPWebURL.Text = @"http://127.0.0.1:21012";
            }

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
                Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "7");
            }

            WriteMessage("EAACtrl started.\r\n");
        }

        private void frmEAACP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SAMP.SAMP_PrivateKey != "")
            {
                SAMP.SampDisconnect();
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
            Properties.Settings.Default.SAMPWebHub = txtSAMPWebURL.Text;
            Properties.Settings.Default.SAMPProfile = cbSAMPProfile.SelectedIndex;
            Properties.Settings.Default.SAMPFOV = cbSAMPSetFOV.SelectedIndex;
            Properties.Settings.Default.SAMPImage = cbSAMPImage.SelectedIndex;
            Properties.Settings.Default.CdCAddress = txtCdCAddress.Text;
            Properties.Settings.Default.CdCPort = txtCdCPort.Text;
            Properties.Settings.Default.StellariumAddress = txtStellariumAddress.Text;
            Properties.Settings.Default.StellariumPort = txtStellariumPort.Text;

            Properties.Settings.Default.Save();

            MQTTDisconnect();
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

        private void WriteMessage(string sMsg)
        {
            try
            {
                tbMessages.AppendText(DateTime.Now.ToString("HH:mm:ss") + ": " + sMsg);
            }
            catch { }
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

        private string SendAPCmd(string sCmdName, Root APCmd)
        {
            string result = "";

            string scriptpayload = "http://localhost:8080?cmd=launch&auth=xyz&cmdformat=json&responseformat=json&payload=";
            scriptpayload += Uri.EscapeDataString(JsonSerializer.Serialize<Root>(APCmd));
            WebClient lwebClient = new WebClient();
            lwebClient.Encoding=Encoding.UTF8;
            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.DownloadString(scriptpayload);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

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
                    StoreSelectedObject(oSelectedObject.results.RA, oSelectedObject.results.Dec);

                    if (cbImagerZoom.Checked)
                    {
                        dblFOV = 1.05;
                    }

                    switch (tabPlanetarium.SelectedIndex)
                    {
                        case 0: // Stellarium
                            string sResult = Stellarium.SyncStellariumToAPObject(oSelectedObject.results.id,
                                                                   oSelectedObject.results.SRA,
                                                                   oSelectedObject.results.SDec,
                                                                   oSelectedObject.results.Type);
                            WriteMessage(Stellarium.Message);
                            if (sResult == "ok")
                            {
                                bOK = true;
                            }

                            if (bOK)
                            {
                                if (cbImagerZoom.Checked)
                                {
                                    Stellarium.SetStellariumFOV(1);
                                    WriteMessage(Stellarium.Message);
                                }
                            }
                            break;
                        case 1: // TheSky
                            // Minor planets need the MPL prefix to be found as a target in TS
                            if (oSelectedObject.results.Type == "Minor")
                            {
                                oSelectedObject.results.id = "MPL " + oSelectedObject.results.id;
                            }

                            TheSky.SetTSTargetPosition(oSelectedObject.results.id, oSelectedObject.results.RA, oSelectedObject.results.Dec, dblFOV);
                            WriteMessage(TheSky.Message);
                            if (cbFOVICorr.Checked)
                            {
                                TheSky.AltAzFOVICorrection();
                                WriteMessage(TheSky.Message);
                            }
                            break;

                        case 2: // Starry Night
                            if (!cbImagerZoom.Checked)
                            {
                                // Do not zoom. Keep current FOV.
                                dblFOV = 0.0;
                            }

                            StarryNight.SetSNTargetPosition(oSelectedObject.results.id, oSelectedObject.results.RA, oSelectedObject.results.Dec, dblFOV);
                            break;
                        case 3: // CdC (SkyChart)
                            if (cbImagerZoom.Checked)
                            {
                                // Set zoom for imager
                                dblFOV = 1.0;
                            }

                            SkyChart.SkychartTargetPosition(oSelectedObject.results.id, oSelectedObject.results.RA, oSelectedObject.results.Dec, dblFOV);
                            break;
                    }

                    if (SAMP.SAMP_PrivateKey!="")
                    {
                        SAMP.Samp_coord_pointAt_sky((double.Parse(oSelectedObject.results.RA) * 15).ToString(), oSelectedObject.results.Dec);
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
                    StoreSelectedObject(oSelectedObject.results.RA, oSelectedObject.results.Dec);

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

        private void ProcessCaptureInfo(bool bCreate, string sObjectInfo, string sImageInfo, string sImagePath, string sCameraSettingsPath)
        {
            //Get the objects ID
            if (sObjectInfo != null)
            {
                //string sID = sObjectInfo.Substring(0, sObjectInfo.IndexOf(","));
                //WriteMessage("Logging: " + sObjectInfo + "\r\n");

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
                        //WriteMessage("ImageSettingsPath is NULL!\r\n");
                    }
                }
                else
                {
                    //WriteMessage("ImagePath is NULL!\r\n");
                }
            }
            else
            {
                //WriteMessage("ObjectInfo is NULL!\r\n");
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            SharpCapCmd("Log");
        }
        private void btnLogPlus_Click(object sender, EventArgs e)
        {
            SharpCapCmd("LogAppend");
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            SharpCapCmd("Find");
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
       
        // ******* NEW CODE FEB 2024

        private string APExecuteScript(string ScriptPayload)
        {
            string result = "";

            string apWebServices = "http://localhost:8080?cmd=launch&auth=xyz&cmdformat=json&responseformat=json&payload=";
            apWebServices += ScriptPayload;
            WebClient lwebClient = new WebClient();
            lwebClient.Encoding = Encoding.UTF8;
            try
            {
                aTimer.Enabled = false;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                result = lwebClient.DownloadString(apWebServices);

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                iCmdCount++;

                WriteMessage("APExecuteScript (" + elapsedTime + ")\r\n");

            }
            catch (Exception e)
            {
                WriteMessage("APExecuteScript ERROR " + e.Message + "\r\n");
            }
            finally
            {
                lwebClient.Dispose();
            }

            aTimer.Enabled = true;

            return result;
        }

        private APGetCmdResult APGetObjects(int Cmd, int Option, string ObjType)
        {
            APGetCmd getCmd = new APGetCmd();
            getCmd.script = "EAAControl2";
            getCmd.parameters= new APGetCmdParams();
            getCmd.parameters.Cmd = Cmd;
            getCmd.parameters.Option = Option;
            getCmd.parameters.ObjType = ObjType;
            
            string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
            APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
            if (apObjects.error == 0 && apObjects.results != null)
            {
                return apObjects;
            }

            return null;
        }

        private void btnStelGetObjectInfo_Click(object sender, EventArgs e)
        {
            APGetCmdResult apOut = APGetObjects(1, 1,"");
            foreach (APCmdObject obj in apOut.results.Objects)
            {
                string sConst = APHelper.ConstellationFullName(obj.Constellation);
                string sType = APHelper.DisplayTypeFromAPType (obj.Type);
                string sTargetName = APHelper.TargetDisplay(obj);
                WriteMessage(sConst + " " + sType + " " + sTargetName + "\r\n");
                Stellarium.SyncStellariumToAPObject(obj.ID, obj.RA2000.ToString(), obj.Dec2000.ToString(), obj.Type);
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

                        StoreSelectedObject(oSelectedObject.results.RA, oSelectedObject.results.Dec);

                        SAMP.Samp_coord_pointAt_sky(RA, oSelectedObject.results.Dec);
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
            TheSky.AltAzFOVICorrection();
            WriteMessage(TheSky.Message);
        }

        private void btnN_Click_1(object sender, EventArgs e)
        {
            Stellarium.StellariumToAltAzPosition(45, 0);
            WriteMessage(Stellarium.Message);
            Stellarium.SetStellariumFOV(85);
            WriteMessage(Stellarium.Message);
        }
        
        private void btnW_Click_1(object sender, EventArgs e)
        {
            Stellarium.StellariumToAltAzPosition(45, 270);
            WriteMessage(Stellarium.Message);
            Stellarium.SetStellariumFOV(85);
            WriteMessage(Stellarium.Message);
        }

        private void btnS_Click_1(object sender, EventArgs e)
        {
            Stellarium.StellariumToAltAzPosition(45, 180);
            WriteMessage(Stellarium.Message);
            Stellarium.SetStellariumFOV(85);
            WriteMessage(Stellarium.Message);
        }

        private void btnE_Click_1(object sender, EventArgs e)
        {
            Stellarium.StellariumToAltAzPosition(45, 90);
            WriteMessage(Stellarium.Message);
            Stellarium.SetStellariumFOV(85);
            WriteMessage(Stellarium.Message);
        }

        private void btnNV_Click_1(object sender, EventArgs e)
        {
            Stellarium.SetStellariumFOV(72);
            WriteMessage(Stellarium.Message);
        }

        private void btnCV_Click_1(object sender, EventArgs e)
        {
            Stellarium.SetStellariumFOV(36);
            WriteMessage(Stellarium.Message);
        }

        private void btnStellariumClearMarkers_Click(object sender, EventArgs e)
        {
            // Removes all markers from Stellarium that were created manually (shift + left click) or via the EAACtrl app.
            Stellarium.StellariumRemoveMarker("");
            WriteMessage(Stellarium.Message);
        }

        private void btnAllCats_Click_1(object sender, EventArgs e)
        {
            switch (btnAllCats.Text)
            {
                case "Def Cats":
                    if (Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "255852135") != "exception")
                    {
                        btnAllCats.Text = "All Cats";
                        WriteMessage(Stellarium.Message);
                    }
                    break;
                case "All Cats":
                    if (Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "255852279") != "exception")
                    {
                        btnAllCats.Text = "All+D Cats";
                        WriteMessage(Stellarium.Message);
                    }
                    break;
                case "All+D Cats":
                    if (Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "7") != "exception")
                    {
                        btnAllCats.Text = "Def Cats";
                        WriteMessage(Stellarium.Message);
                    }
                    break;
            }
        }

        private void btnSNN_Click(object sender, EventArgs e)
        {
            StarryNight.SetSNAltAz(45, 0, 118);
            WriteMessage(StarryNight.Message);
        }

        private void btnSNW_Click(object sender, EventArgs e)
        {
            StarryNight.SetSNAltAz(45, 270, 118);
            WriteMessage(StarryNight.Message);
        }

        private void btnSNS_Click(object sender, EventArgs e)
        {
            StarryNight.SetSNAltAz(45, 180, 118);
            WriteMessage(StarryNight.Message);
        }

        private void btnSNE_Click(object sender, EventArgs e)
        {
            StarryNight.SetSNAltAz(45, 90, 118);
            WriteMessage(StarryNight.Message);
        }

        private void btnSNNV_Click(object sender, EventArgs e)
        {
            StarryNight.SetSNFOV(118);
            WriteMessage(StarryNight.Message);
        }

        private void btnSNCV_Click(object sender, EventArgs e)
        {
            StarryNight.SetSNFOV(36);
            WriteMessage(StarryNight.Message);
        }

        private void btnSAMPConnect_Click_1(object sender, EventArgs e)
        {
            SAMP.SampRegister();
            WriteMessage(SAMP.Message);
            if (SAMP.SAMP_PrivateKey != "")
            {
                SAMP.SampMetaData();
                WriteMessage(SAMP.Message);
                //Samp_getRegisteredClients(sSAMP_PrivateKey);
                //Samp_coord_get_sky(sSAMP_PrivateKey);
            }
        }

        private void btnSAMPDisconnect_Click(object sender, EventArgs e)
        {
            SAMP.SampDisconnect();
            WriteMessage(SAMP.Message);
        }

        private void cbSAMPProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSAMPProfile.SelectedIndex == 0) 
            {
                SAMP.StandardProfile = true;
            }
            else
            {
                SAMP.StandardProfile = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SAMP.Samp_script_aladin_send("zoom 54arcmin");
            WriteMessage(SAMP.Message);
        }

        private string SAMPFOV()
        {
            string sFOV = "";

            switch (cbSAMPSetFOV.SelectedIndex)
            {
                case 0:
                    sFOV = "54arcmin";
                    break;
                case 1:
                    sFOV = "5deg";
                    break;
                case 2:
                    sFOV = "1deg";
                    break;
                case 3:
                    sFOV = "30arcmin";
                    break;
                case 4:
                    sFOV = "25arcmin";
                    break;
                case 5:
                    sFOV = "10arcmin";
                    break;
            }

            return sFOV;
        }

        private void btnSAMPSetFOV_Click(object sender, EventArgs e)
        {
            SAMP.Samp_script_aladin_send("zoom " + SAMPFOV());
            WriteMessage(SAMP.Message);
        }

        private void btnSAMPImage_Click(object sender, EventArgs e)
        {
            SAMP.Samp_script_aladin_send("get HiPS(" + cbSAMPImage.Items[cbSAMPImage.SelectedIndex] + ") " + SelectedRA + " " + SelectedDec + " " + SAMPFOV());
            WriteMessage(SAMP.Message);
        }

        private void btnSAMPDo_Click(object sender, EventArgs e)
        {

            SAMP.Samp_script_aladin_send("get HiPS(" + cbSAMPImage.Items[cbSAMPImage.SelectedIndex] + ") " + SelectedRA + " " + SelectedDec + " " + SAMPFOV());
            WriteMessage(SAMP.Message);
        }

        private void btnSkychartN_Click(object sender, EventArgs e)
        {
            WriteMessage("Skychart SETNORTH\r\n");
            SkyChart.SkychartViewDirection("SETNORTH");
            SkyChart.SkychartFOV(100);
        }

        private void btnSkyChartW_Click(object sender, EventArgs e)
        {
            WriteMessage("Skychart SETWEST\r\n");
            SkyChart.SkychartViewDirection("SETWEST");
            SkyChart.SkychartFOV(100);
        }

        private void btnSkyChartS_Click(object sender, EventArgs e)
        {
            WriteMessage("Skychart SETSOUTH\r\n");
            SkyChart.SkychartViewDirection("SETSOUTH");
            SkyChart.SkychartFOV(100);
        }

        private void btnSkyChartE_Click(object sender, EventArgs e)
        {
            WriteMessage("Skychart SETEAST\r\n");
            SkyChart.SkychartViewDirection("SETEAST");
            SkyChart.SkychartFOV(100);
        }

        private void btnSkyChartNV_Click(object sender, EventArgs e)
        {
            WriteMessage("Skychart FOV NV\r\n");
            SkyChart.SkychartFOV(100);
        }

        private void btnSkyChartCV_Click(object sender, EventArgs e)
        {
            WriteMessage("Skychart FOV CV\r\n");
            SkyChart.SkychartFOV(45);
        }

        private void btnGetPlanetariumObject_Click(object sender, EventArgs e)
        {
            switch (cbPLGetObject.SelectedIndex)
            {
                case 0:
     
                    // Add Selected Object in Stellarium to current AP Plan
                    Root oAPCmd = new Root(); 
                    oAPCmd.script = "EAAControl";
                    oAPCmd.parameters = new Parameters();
                    oAPCmd.parameters.cmd = "10";

                    string sResult = SendAPCmd("GETOBJECTS", oAPCmd);
                    break;
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
    // New AP classes
    public class APPutCmd
    {
        public string script { get; set; }
        public APPutCmdParams parameters { get; set; }
    }

    public class APPutCmdParams
    {
        public int Cmd { get; set; }
        public int Option { get; set; }
        public List<APCmdObject> Objects { get; set; }
    }

    public class APGetCmd
    {
        public string script { get; set; }
        public APGetCmdParams parameters { get; set; }
    }

    public class APGetCmdParams
    {
        public int Cmd { get; set; }
        public int Option { get; set; }
        public string ObjType { get; set; }
    }

    public class APGetCmdResult
    {
        public int error { get; set; }
        public APGetResults results { get; set; }
    }

    public class APGetResults
    {
        public List<APCmdObject> Objects { get; set; }
    }

    public class APCmdObject
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Constellation { get; set; }
        public string Catalogue { get; set; }
        public string Distance { get; set; }
        public int PosAngle { get; set; }
        public double Magnitude { get; set; }
        public double RA2000 { get; set; }
        public double Dec2000 { get; set; }
    }
}