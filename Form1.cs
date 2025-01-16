using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Text.Json;
using System.IO;
using System.Data;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Speech.Synthesis;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Http;
using System.Diagnostics;

namespace EAACtrl
{
    public partial class frmEAACP : Form
    {
        private static System.Timers.Timer aTimer;

        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        private int iSortCount = 0;

        private int iCmdCount = 0;
        private bool bExpanded = false;
        private bool bOverlayVisible = true;
        private frmTextOverlay frmTextOverlay = new frmTextOverlay();
        private int CurrentPlanetarium;

        private static readonly HttpClient httpClient = new HttpClient();
        private string sMsg = "";

        private MqttClient mqttSharpCap;
        private string mqttClientID = "";
        // TerraNova
        //private string mqttBroker = "192.168.0.143";
        // Odin
        private string mqttBroker = "192.168.0.83";

        private string TargetName = "";
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

        // KStars
        private KStars KStars = new KStars();

        // SAMP Client class
        private SAMP SAMP = new SAMP();

        // JPL Client Class
        private JPLHorizons JPLHorizons = new JPLHorizons();

        // Database Class
        private Database Database = new Database();

        // ASCOM Telescope
        private EAATelescope EAATelescope = new EAATelescope();

        // Astro Class
        private AstroCalc AstroCalc = new AstroCalc();

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
                    case "PlateSolve":
                        WriteMessage(sMsg + "\r\n");
                        break;
                    case "PlateSolveAlign":
                        WriteMessage(sMsg + "\r\n");

                        CentreObject(Double.Parse(CmdParams[1]), Double.Parse(CmdParams[2]));

                       /* if (EAATelescope.AddAlignmentPoint(Double.Parse(CmdParams[1]), Double.Parse(CmdParams[2])))
                        {
                            Speak("Alignment point added.");
                        }
                       */
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
            try
            {
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
            catch (Exception ex)
            {
                WriteMessage("MQTTConnect ERROR - " + ex.Message + "\r\n");
            }
        }
        private void SharpCapCmd(string cmd)
        {
            if (!cbNoSharpCap.Checked)
            { 
                MQTTConnect();

                WriteMessage("SharpCapCmd: " + cmd + "\r\n");
                mqttSharpCap.Publish("SharpCap/command", Encoding.UTF8.GetBytes(cmd));
            }
        }

        private void OnTimedEvent2(Object source, ElapsedEventArgs e)
        {
            try
            {
                aTimer.Enabled = false;
                iSortCount++;
                bool theSkyFOVIUpdated = false;

                APGetCmd getCmd = new APGetCmd();
                getCmd.script = "EAAControl2";
                getCmd.parameters = new APGetCmdParams();
                getCmd.parameters.Cmd = 0;
                getCmd.parameters.Option = 0;
                if (iSortCount >=4)
                {
                    if (cbAPAutoSort.Checked)
                    {
                        getCmd.parameters.Option = 1;
                    }

                    if (CurrentPlanetarium == 3 && cbTSAutoFOVI.Checked)
                    {
                        TheSky.AltAzFOVICorrection();
                        theSkyFOVIUpdated = true;
                    }

                    iSortCount = 0;
                }

                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
                if (getCmd.parameters.Option == 0)
                {
                    if (theSkyFOVIUpdated)
                    {
                        txtStatusMsg.Invoke(new Action(() => txtStatusMsg.Text = "KeepAlive " + TheSky.Message + " " + iCmdCount.ToString() + " " + sOut));
                    }
                    else
                    {
                        txtStatusMsg.Invoke(new Action(() => txtStatusMsg.Text = "KeepAlive " + iCmdCount.ToString() + " " + sOut));
                    }
                }
                else
                {
                    if (theSkyFOVIUpdated)
                    {
                        txtStatusMsg.Invoke(new Action(() => txtStatusMsg.Text = "KeepAlive + Sort + " + TheSky.Message + " " + iCmdCount.ToString() + " " + sOut));
                    }
                    else
                    {
                        txtStatusMsg.Invoke(new Action(() => txtStatusMsg.Text = "KeepAlive + Sort " + iCmdCount.ToString() + " " + sOut));

                    }
                }

                
            }
            catch (Exception) { }
        }

       private void SetTimer()
        {
            aTimer = new System.Timers.Timer(5000);
            aTimer.Elapsed += OnTimedEvent2;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
 
        public frmEAACP()
        {
            InitializeComponent();

            cbCaptureProfile.SelectedIndex = Properties.Settings.Default.CaptureMode;
            tabPlanetarium.SelectedIndex = Properties.Settings.Default.Planatarium;
            CurrentPlanetarium = tabPlanetarium.SelectedIndex;
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
            cbAPAutoSort.Checked = Properties.Settings.Default.APAutoSort;
            cbTSAutoFOVI.Checked = Properties.Settings.Default.TSAutoFOVI;
            txtScriptFolder.Text = Properties.Settings.Default.StScriptFolder;
            Stellarium.ScriptFolder = txtScriptFolder.Text;
            lblTelescope.Text = Properties.Settings.Default.ASCOMTelescope;

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

            // Background thread to update the observation total time clock.
            workerObserveTime.WorkerReportsProgress = true;
            workerObserveTime.WorkerSupportsCancellation = true;
        }

        private void Speak(string Speech)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak(Speech);
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
                this.Width = 250;
                //frmEAACP.ActiveForm.Width = 240;
                btnExpand.Text = ">>";
                if (EAATelescope.Connected)
                {
                    StopTelescopeStatus();
                }
            }
            else
            {
                frmEAACP.ActiveForm.Width = 630;
                btnExpand.Text = "<<";
                if (EAATelescope.Connected)
                {
                    if (tcExtra.SelectedIndex == 1)
                    {
                        StellCount = 0;
                        StartTelescopeStatus();
                    }
                }
            }
            bExpanded = !bExpanded;
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            ExpandUI();
        }

        private void frmEAACP_Load(object sender, EventArgs e)
        {
            this.Width = 250;
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
            Properties.Settings.Default.APAutoSort = cbAPAutoSort.Checked;
            Properties.Settings.Default.TSAutoFOVI = cbTSAutoFOVI.Checked;
            Properties.Settings.Default.StScriptFolder = txtScriptFolder.Text;
            Properties.Settings.Default.ASCOMTelescope = lblTelescope.Text;

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
                tbMessages.Invoke(new Action(() => tbMessages.AppendText(DateTime.Now.ToString("HH:mm:ss") + ": " + sMsg)));
                //tbMessages.AppendText(DateTime.Now.ToString("HH:mm:ss") + ": " + sMsg);
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

        private void StartObserveClock()
        {
            if (workerObserveTime.IsBusy != true)
            {
                workerObserveTime.RunWorkerAsync();
            }
        }

        private void StopTelescopeStatus()
        {
            if (workerTelescopeStatus.WorkerSupportsCancellation == true)
            {
                workerTelescopeStatus.CancelAsync();
            }
        }

        private void StartTelescopeStatus()
        {
            if (workerTelescopeStatus.IsBusy != true)
            {
                workerTelescopeStatus.RunWorkerAsync();
            }
        }

        private void StopObserveClock()
        {
            if (workerObserveTime.WorkerSupportsCancellation == true)
            {
                workerObserveTime.CancelAsync();
            }
        }

        private void workerObserveTime_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int i = 0;

            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    worker.ReportProgress(0);
                    break;
                }
                else 
                { 
                    System.Threading.Thread.Sleep(1000);
                    worker.ReportProgress(i++);
                }
            }
        }

        private void workerObserveTime_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TimeSpan tsCounter = new TimeSpan(0,0,e.ProgressPercentage);
            lblObserveTime.Text = tsCounter.ToString(); 
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

        public string APExecuteScript(string ScriptPayload)
        {
            string result = "";

            string apWebServices = "http://localhost:8080?cmd=launch&auth=jrzLsmZVY33rwxyd38MS&cmdformat=json&responseformat=json&payload=";
            apWebServices += ScriptPayload;

            try
            {
                aTimer.Enabled = false;
                myTimer.Enabled = false;
                result = GetRequest(apWebServices);
                iCmdCount++;
            }
            catch (Exception){ }

            aTimer.Enabled = true;
            myTimer.Enabled = true;
            return result;
        }

        private void SyncPlanetarium()
        {
            double dblFOV = -1.0;
            
            APCmdObject SelectedObject = APGetSelectedObject();
            if (SelectedObject == null)
            {
                MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Used by SAMP to set current object
            StoreSelectedObject(SelectedObject.RA2000.ToString(), SelectedObject.Dec2000.ToString());

            if (cbImagerZoom.Checked)
            {
               dblFOV = 1.05;
            }

            switch (tabPlanetarium.SelectedIndex)
            {
                case 0: // Stellarium
                    string RA = ""; string Dec = "";
                    // Format RA/Dec to hms and dms
                    RA = APHelper.RADecimalHoursToHMS(SelectedObject.RA2000, @"hh\hmm\mss\.ff\s");
                    Dec = APHelper.DecDecimalToDMS(SelectedObject.Dec2000);

                    if ("ok" == Stellarium.SyncStellariumToAPObject(SelectedObject.ID, RA, Dec, SelectedObject.Type))
                    {
                        WriteMessage(Stellarium.Message);
                        if (cbImagerZoom.Checked)
                        {
                            Stellarium.SetStellariumFOV(0.66);
                            WriteMessage(Stellarium.Message);
                        }
                    }
                    break;
                case 3: // TheSky
                    // Minor planets need the MPL prefix to be found as a target in TS
                    if (SelectedObject.Type == "Minor")
                    {
                        SelectedObject.ID = "MPL " + SelectedObject.ID;
                    }

                    TheSky.SetTSTargetPosition(SelectedObject.ID, SelectedObject.RA2000.ToString(), SelectedObject.Dec2000.ToString(), dblFOV);
                    WriteMessage(TheSky.Message);
                    if (cbFOVICorr.Checked)
                    {
                        TheSky.AltAzFOVICorrection();
                        WriteMessage(TheSky.Message);
                    }
                    break;

                case 2: // Starry Night
                    if (!cbImagerZoom.Checked) {
                        dblFOV = 0.0; // Do not zoom. Keep current FOV
                    }
                    StarryNight.SetSNTargetPosition(SelectedObject.ID, SelectedObject.RA2000.ToString(), SelectedObject.Dec2000.ToString(), dblFOV);
                    WriteMessage(StarryNight.Message);
                    break;
                case 1: // CdC (SkyChart)
                    if (cbImagerZoom.Checked) {  
                        dblFOV = 1.0; // Set zoom for imager
                    }
                    SkyChart.SkychartTargetPosition(SelectedObject.ID, SelectedObject.RA2000.ToString(), SelectedObject.Dec2000.ToString(), dblFOV);
                    WriteMessage(SkyChart.Message);
                    break;
                case 4:
                    KStars.SyncObject(SelectedObject.RA2000, SelectedObject.Dec2000);
                    break;
            }

            if (SAMP.SAMP_PrivateKey!="")
            {
                SAMP.Samp_coord_pointAt_sky((SelectedObject.RA2000 * 15).ToString(), SelectedObject.Dec2000.ToString());
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            SyncPlanetarium();
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            WriteMessage("Target start.\r\n");
            StopObserveClock();

            // Get the selected object (if any) from AstroPlanner
            APCmdObject SelectedObject = APGetSelectedObject();
            if (SelectedObject == null)
            {
                WriteMessage("AP GetTarget - FAILED!\r\n");
                MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Used by SAMP to set location
            StoreSelectedObject(SelectedObject.RA2000.ToString(), SelectedObject.Dec2000.ToString());

            // Update the UI
            TargetName = APHelper.TargetDisplay(SelectedObject);
            SetOverlayText(TargetName);
            this.Text = "EAACtrl 3.0 - " + TargetName;
            
            if (cbSyncPlanetarium.Checked)
            {
                SyncPlanetarium();  // Sync the selected planetarium app.
            }

            // Send target to SharpCap and place SharpCap into Find mode
            SharpCapCmd("Target|" + TargetName);

            // Use AstroPlanner to slew to the object.
            if (cbSlewOnTarget.Checked)
            {
                DialogResult dialogResult = MessageBox.Show("Confirm telescope SLEW?", "EAACtrl", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    WriteMessage("Slew confirmed by user.\r\n");
                    //APSlewTelescope();

                    AstroCalc astroCalc = new AstroCalc();
                    astroCalc.J2000ToJNOW(SelectedObject.RA2000, SelectedObject.Dec2000, out double RANow, out double DecNow);
                    if (EAATelescope.Slew(RANow, DecNow))
                    {
                        WriteMessage("Slewing...\r\n");
                    }
                    else 
                    {
                        WriteMessage("Slew - command failed!\r\n");
                    }
                    
                }
                else
                {
                    WriteMessage("Slew - cancelled by user.\r\n");
                }
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            string sProfile = "";
            
            StopObserveClock();

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
            
            StartObserveClock();
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
                    string sDestImagePath = sImagePath.Replace(@"C:\", @"C:\"); // Not needed unless path on desktop changes
                    //string sSrcImagePath = sImagePath.Replace(@"C:\", @"\\TERRANOVA\");
                    string sSrcImagePath = sImagePath.Replace(@"C:\", @"\\ODIN\");
                    Directory.CreateDirectory(sDestImagePath.Substring(0, sDestImagePath.LastIndexOf(@"\")));
                    File.Copy(sSrcImagePath, sDestImagePath, true);

                    if (sCameraSettingsPath != null)
                    {
                        // Copy image settings file to desktop PC SharpCap folder
                        string sDestSettingsPath = sCameraSettingsPath.Replace(@"C:\", @"C:\"); // Not needed unless path on desktop changes
                        //string sSrcSettingsPath = sCameraSettingsPath.Replace(@"C:\", @"\\TERRANOVA\");
                        string sSrcSettingsPath = sCameraSettingsPath.Replace(@"C:\", @"\\ODIN\");
                        Directory.CreateDirectory(sDestSettingsPath.Substring(0, sDestSettingsPath.LastIndexOf(@"\")));
                        File.Copy(sSrcSettingsPath, sDestSettingsPath, true);

                        // Create or append an observation in AstroPlanner
                        APLog oAPLogCmd = new APLog ();
                        oAPLogCmd.script = "EAAControl2";
                        
                        oAPLogCmd.parameters = new APLogParams();
                        oAPLogCmd.parameters.Cmd = 3;
                        oAPLogCmd.parameters.ObjectInfo = Uri.EscapeDataString(sObjectInfo);
                        oAPLogCmd.parameters.ImgSettingsPath = Uri.EscapeDataString(sDestSettingsPath);
                        oAPLogCmd.parameters.ImgPath = Uri.EscapeDataString(sDestImagePath);
                        oAPLogCmd.parameters.ImgInfo = Uri.EscapeDataString(sImageInfo);

                        if (bCreate)
                        {
                            // AstroPlanner add observation and attach image.
                            oAPLogCmd.parameters.Option = 1;
                        }
                        else
                        {
                            // AstroPlanner append observation and attach image to current observation.
                            oAPLogCmd.parameters.Option = 2;
                        }

                        string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APLog>(oAPLogCmd)));
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
            StopObserveClock();

            SharpCapCmd("Find");
        }

        // *** EAACtrl app needs to perfrom these functions not AP (AP will pass selected objects or add/update objects)
        private void btnSCDSA_Click(object sender, EventArgs e)
        {
            StopObserveClock();
        }

        private void btnAsteroidsFOV_Click(object sender, EventArgs e)
        {
            APExecute(8, 0);
        }

        private void btnPlanetaryMoons_Click(object sender, EventArgs e)
        {
            APExecute(7, 0);
        }

// ******* NEW CODE FEB 2024

// Get the currently selected object in AstroPlanner (null is none selected)
        private APCmdObject APGetSelectedObject()
        {
            try
            {
                APGetCmd getCmd = new APGetCmd();
                getCmd.script = "EAAControl2";
                getCmd.parameters = new APGetCmdParams();
                getCmd.parameters.Cmd = 1;
                getCmd.parameters.Option = 1;

                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
                if (sOut.Contains("}]}") && !sOut.Contains("}]}}"))
                {
                    sOut += "}";
                }

                APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
                if (apObjects.error == 0 && apObjects.results.Objects != null)
                {
                    return apObjects.results.Objects[0];
                }
            } catch (Exception) {}

            return null; // Nothing selected
        }

        private APGetCmdResult APGetObjects(int Cmd, int Option, string ObjType, string Params)
        {
            APGetCmd getCmd = new APGetCmd();
            getCmd.script = "EAAControl2";
            getCmd.parameters= new APGetCmdParams();
            getCmd.parameters.Cmd = Cmd;
            getCmd.parameters.Option = Option;
            getCmd.parameters.ObjType = ObjType;
            getCmd.parameters.Params = Params;
            
            string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
            // Corrects a bug in AP that does not close the JSON documents correctly (missing })
            if (sOut.Contains("}]}") && !sOut.Contains("}]}}"))
            {
                sOut += "}";
            }
            APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
            if (apObjects.error == 0 && apObjects.results != null)
            {
                return apObjects;
            }

            return null;
        }

        private void APSlewTelescope()
        {
            APGetCmd getCmd = new APGetCmd();
            getCmd.script = "EAAControl2";
            getCmd.parameters = new APGetCmdParams();
            getCmd.parameters.Cmd = 4; // Slew Telescope
            getCmd.parameters.Option = 0;

            APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
        }

        private APCmdObject APExecute(int Cmd, int Option)
        {
            try
            {
                APGetCmd getCmd = new APGetCmd();
                getCmd.script = "EAAControl2";
                getCmd.parameters = new APGetCmdParams();
                getCmd.parameters.Cmd = Cmd;
                getCmd.parameters.Option = Option;

                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APGetCmd>(getCmd)));
                APGetCmdResult apObjects = JsonSerializer.Deserialize<APGetCmdResult>(sOut);
                if (apObjects.error == 0 && apObjects.results.Objects != null)
                {
                    return apObjects.results.Objects[0];
                }
            }
            catch (Exception) { }

            return null; // Nothing selected
        }

        private string DSAFormat(APCmdObject obj)
        {
            // IDs|Names|Type|RA(decimal hours)|Dec(degrees)|VMag|RMax(arcmin)|RMin(arcmin)|PosAngle
            string sOut = obj.ID + "|" + obj.Name + "|";
            sOut += APHelper.DisplayTypeFromAPType(obj.Type) + "|";
            sOut += obj.RA2000.ToString() + "|" + obj.Dec2000.ToString() + "|";
            sOut += obj.Magnitude.ToString() + "|";
            
            APHelper.SizeInfoString sizeInfo = APHelper.ObjectSizeString(obj.Size);
            sOut += sizeInfo.MajorAxis + "|" + sizeInfo.MinorAxis + "|";
            if (obj.PosAngle != -999)
            {
                sOut += obj.PosAngle.ToString();
            }

            sOut += "\r\n";

            return sOut;            
        }

        private void btnAPGetObject_Click(object sender, EventArgs e)
        {
            if (cbAPGetObjects.SelectedIndex == 4 || cbAPGetObjects.SelectedIndex == 3)
            {
                APCmdObject SelectedObject = APGetSelectedObject();
                if (SelectedObject == null)
                {
                    MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbAPGetObjects.SelectedIndex == 4)
                {
                    string RA = (SelectedObject.RA2000 * 15).ToString();
                    SAMP.Samp_coord_pointAt_sky(RA, SelectedObject.Dec2000.ToString());
                }
                else
                {
                    //CDS format 06 43 19.680 + 26 58 33.60
                    string RA = APHelper.RADecimalHoursToHMS(SelectedObject.RA2000, @"hh\ mm\ ss\.ff");
                    string Dec = APHelper.DecDecimalToDMSSp(SelectedObject.Dec2000);
                    Clipboard.SetText(RA + " " + Dec);
                    Speak("Coordinates on clipboard.");
                }
                
                return;
            }

            if (cbAPGetObjects.SelectedIndex == 1 || cbAPGetObjects.SelectedIndex==2)
            {
                string sOut = "";

                APGetCmdResult apOut = APGetObjects(1, 2, "", "");
                if (apOut == null)
                {
                    MessageBox.Show("No objects selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (APCmdObject obj in apOut.results.Objects)
                {
                    if (cbAPGetObjects.SelectedIndex == 2)
                    {
                        // List of IDs - Good for observing lists in other applications
                        sOut += obj.ID + "\r\n";
                    }

                    if (cbAPGetObjects.SelectedIndex == 1)
                    {
                        // SharpCap DSA format sourced from AP only.
                        sOut += DSAFormat(obj);
                    }
                }

                Clipboard.SetText(sOut);
                if (cbAPGetObjects.SelectedIndex < 2)
                {
                    Speak("DSA on clipboard");
                }
                else
                {
                    Speak("Objects on clipboard");
                }
            }

            // DSA (single object) with JPL lookup if minor body
            // ToDo: Cater for all selected objects.
            if (cbAPGetObjects.SelectedIndex == 0)
            {
                APExecute(6, 0);
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

        private void SetStellariumDSOCatalogues(string Catalogues)
        {
            switch (Catalogues)
            {
                case "No DSO":
                    if (Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "7") != "exception")
                    {
                        btnAllCats.Text = "Std DSO";
                        WriteMessage(Stellarium.Message);
                    }
                    break;
                case "Std DSO":
                    if (Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "255852135") != "exception")
                    {
                        btnAllCats.Text = "All DSO";
                        WriteMessage(Stellarium.Message);
                    }
                    break;
                case "All DSO":
                    if (Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "255852279") != "exception")
                    {
                        btnAllCats.Text = "All+D DSO";
                        WriteMessage(Stellarium.Message);
                    }
                    break;
                case "All+D DSO":
                    if (Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "0") != "exception")
                    {
                        btnAllCats.Text = "No DSO";
                        WriteMessage(Stellarium.Message);
                    }
                    break;
            }
        }

        private void btnAllCats_Click_1(object sender, EventArgs e)
        {
            SetStellariumDSOCatalogues(btnAllCats.Text);
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
            APCmdObject apObject = null;

            switch (cbPLGetObject.SelectedIndex)
            {
                case 0:
                    switch (tabPlanetarium.SelectedIndex)
                    {
                        case 0:
                            // Add Selected Object in Stellarium to current AP Plan
                            apObject = Stellarium.StellariumGetSelectedObjectInfo();
                            if (apObject != null)
                            {
                                APPutCmd aPPutCmd = new APPutCmd();
                                aPPutCmd.script = "EAAControl2";
                                aPPutCmd.parameters = new APPutCmdParams();
                                aPPutCmd.parameters.Cmd = 2;
                                aPPutCmd.parameters.Option = 1;
                                aPPutCmd.parameters.Objects = new List<APCmdObject> { apObject };
                                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APPutCmd>(aPPutCmd)));
                            }
                            else
                            {
                                WriteMessage(Stellarium.Message);
                            }
                            break;
                        case 2:
                            apObject = StarryNight.GetSNSelectedObject();
                            if (apObject != null)
                            {
                                APPutCmd aPPutCmd = new APPutCmd();
                                aPPutCmd.script = "EAAControl2";
                                aPPutCmd.parameters = new APPutCmdParams();
                                aPPutCmd.parameters.Cmd = 2;
                                aPPutCmd.parameters.Option = 1;
                                aPPutCmd.parameters.Objects = new List<APCmdObject> { apObject };
                                string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APPutCmd>(aPPutCmd)));

                            }
                            else
                            {
                                WriteMessage(StarryNight.Message);
                            }
                            break;
                    }
                break;
                case 1:
                    switch(tabPlanetarium.SelectedIndex)
                    {
                        case 0:
                            apObject = Stellarium.StellariumGetSelectedObjectInfo();
                            if (apObject != null)
                            {
                                Clipboard.SetText(apObject.ID);
                                Speak("Object ID copied");
                            }
                            break;
                        case 2:
                            apObject = StarryNight.GetSNSelectedObject();
                            if (apObject != null)
                            {
                                Clipboard.SetText(apObject.ID);
                                Speak("Object ID copied");
                            }
                            break;
                    }
                    break;
            }
         }

        private void tabPlanetarium_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPlanetarium = tabPlanetarium.SelectedIndex;
        }

        private string CreateSearchParams(double SearchRA, double SearchDec)
        {
            string sParams, sType = string.Empty;

            sParams = Properties.Settings.Default.sfMagnitude;
            if (Properties.Settings.Default.sfAll)
            {
                sType = "All";
            }
            else if (Properties.Settings.Default.sfStars)
            {
                sType = "Star";
            }
            else if (Properties.Settings.Default.sfGalaxies)
            {
                sType = "Galaxy";
            }
            else if (Properties.Settings.Default.sfQuasars)
            {
                sType = "Quasar";
            }
            else if (Properties.Settings.Default.sfDouble)
            {
                sType = "Double";
            }
            else if (Properties.Settings.Default.sfVariable)
            {
                sType = "Variable";
            }
            else if (Properties.Settings.Default.sfGlobulars)
            {
                sType = "Cluster";
            }
            else if (Properties.Settings.Default.sfNebulae)
            {
                sType = "Nebula";
            }

            sParams += "|" + sType;
            if (Properties.Settings.Default.sfNoMag)
            {
                sParams += "|1";
            }
            else
            {
                sParams += "|0";
            }

            if (Properties.Settings.Default.SearchRadius > 0)
            {
                sParams += "|" + Properties.Settings.Default.SearchRadius.ToString();
            }
            else
            {
                sParams += "|0.5";
            }

            sParams += "|" + SearchRA.ToString() + "|" + SearchDec.ToString();

            return sParams;
        }

        private void btnFOVSearch_Click(object sender, EventArgs e)
        {
            double SearchRA = 999, SearchDec = 999;
            if (Properties.Settings.Default.sfPlanetarium)
            {
                APCmdObject ap = null;
                switch (tabPlanetarium.SelectedIndex)
                {
                    case 0: // Stellarium
                        ap = Stellarium.StellariumGetSelectedObjectInfo();
                        break;
                }

                if (ap == null)
                {
                    Speak("No object selected in planetarium");
                    return;
                }

                SearchRA = ap.RA2000;
                SearchDec = ap.Dec2000;
            }

            if (Properties.Settings.Default.sfDatasource == 0)
            {
                // Store the search results for DSA and Search List display
                List<string[]> listOfSearchResults;

                APGetCmdResult apOut = APGetObjects(5, 2, "", CreateSearchParams(SearchRA, SearchDec));
                if (apOut == null)
                {
                    Speak("No search results");
                    //MessageBox.Show("No objects selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                listOfSearchResults = Stellarium.DrawObjects(apOut);

                // Add search objects in SharpCap DSA format to the clipboard
                if (Properties.Settings.Default.sSharpCapDSA)
                {
                    string sDSA = "";
                    foreach (APCmdObject obj in apOut.results.Objects)
                    {
                        sDSA += DSAFormat(obj);
                    }
                    Clipboard.SetText(sDSA);
                }

                // Show search results window
                if (Properties.Settings.Default.sResultsList)
                {
                    using (SearchResults frmOpt = new SearchResults())
                    {
                        frmOpt.EAACP = this;
                        frmOpt.TopMost = true;
                        frmOpt.Results = listOfSearchResults;
                        if (frmOpt.ShowDialog() == DialogResult.OK)
                        {

                        }
                    }
                }
            }
            else if (Properties.Settings.Default.sfDatasource == 1)
            {

            }
            else if (Properties.Settings.Default.sfDatasource == 2)
            {
                if (!Properties.Settings.Default.sfPlanetarium)
                {
                    APCmdObject apOut = APGetSelectedObject();
                    if (apOut == null)
                    {
                        Speak("No object selected");
                        return;
                    }

                    SearchRA = apOut.RA2000;
                    SearchDec = apOut.Dec2000;
                }

                string ObjectType = "All";
                if (Properties.Settings.Default.sfGalaxies)
                {
                    ObjectType = "G";
                }
                else if (Properties.Settings.Default.sfQuasars)
                {
                    ObjectType = "Q";
                }

                AstroCalc Astro = new AstroCalc();
                DataTable dt = null;
                //Astro.J2000ToJNOW(apOut.RA2000, apOut.Dec2000, out double RANOW, out double DecNOW);

                //dt = Database.GladeBoundingSearch(Astro.BoundingBox(apOut.RA2000*15, apOut.Dec2000,0.5,0.25),double.Parse(Properties.Settings.Default.sfMagnitude), ObjectType);

                dt = Database.GladeConeSearch(SearchRA*15, SearchDec, Properties.Settings.Default.SearchRadius, double.Parse(Properties.Settings.Default.sfMagnitude), ObjectType);
                if (dt == null || dt.Rows.Count==0)
                {
                    Speak("No search results");
                    return;
                }

                Stellarium.DrawObjects(dt);

                // Show search results window
                if (Properties.Settings.Default.sResultsList)
                {
                    using (SearchResults frmOpt = new SearchResults())
                    {
                        frmOpt.EAACP = this;
                        frmOpt.TopMost = true;
                        frmOpt.Results = null;
                        frmOpt.ResultsDataTable = dt;
                        if (frmOpt.ShowDialog() == DialogResult.OK)
                        {

                        }
                    }
                }
            }

            return;
        }

        private void btnFOVClear_Click(object sender, EventArgs e)
        {
            Stellarium.ClearObjects();
        }

        private void btnFOVOptions_Click(object sender, EventArgs e)
        {
            using (StelFOVOptions frmOpt = new StelFOVOptions())
            {
                frmOpt.TopMost = true;
                if (frmOpt.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }

        private void btnObsClkRst_Click(object sender, EventArgs e)
        {
            StartObserveClock();
        }


        private int StellCount = 0;
        private void DisplayTelescopeInformation()
        {
            if (EAATelescope.Connect())
            {

                double RANOW = EAATelescope.RightAscension;
                double DecNOW = EAATelescope.Declination;

                lblTeleRA.Text = APHelper.RADecimalHoursToHMS(RANOW, @"hh\hmm\mss\.ff\s");
                lblTeleDec.Text = APHelper.DecDecimalToDMS(DecNOW);

                AstroCalc.JNOWToJ2000(RANOW, DecNOW, out double RA2000, out double Dec2000);

                lblTeleRA2000.Text = APHelper.RADecimalHoursToHMS(RA2000, @"hh\hmm\mss\.ff\s");
                lblTeleDec2000.Text = APHelper.DecDecimalToDMS(Dec2000);

                if (AstroCalc.JNOWToAltAz(RANOW, DecNOW, out double Alt, out double Azi))
                {
                    lblAlt.Text = Alt.ToString("F3") + "°";
                    lblAzi.Text = Azi.ToString("F3") + "°";
                }

                if (EAATelescope.Slewing)
                {
                    StellCount = 0;
                    lblTeleStatus.Text = "Slewing";
                    // Stellarium 
                    if (tabPlanetarium.SelectedIndex == 0)
                    {
                        Stellarium.MarkTelescopePosition(lblTeleRA2000.Text, lblTeleDec2000.Text, 1000);
                    }
                }
                else if (EAATelescope.Tracking)
                {
                    lblTeleStatus.Text = "Tracking";
                    // Stellarium 
                    if (tabPlanetarium.SelectedIndex == 0)
                    {
                        if (StellCount == 4)
                        {
                            StellCount = 0; ;
                        }

                        if (StellCount==0)
                        {
                            Stellarium.MarkTelescopePosition(lblTeleRA2000.Text, lblTeleDec2000.Text, 2000);
                        }

                        StellCount++;
                    }
                }
            }
        }

        private void btnTelescope_Click(object sender, EventArgs e)
        {
            if (EAATelescope.Choose())
            {
                lblTelescope.Text = Properties.Settings.Default.ASCOMTelescope;
            }
        }

        private void cbConnect_CheckedChanged(object sender, EventArgs e)
        {
            if (cbConnect.Checked)
            {
                if (EAATelescope.Connect())
                {
                    DisplayTelescopeInformation();

                    // Only get this once. It is JNOW for CPWI.
                    lblTeleEquCoordType.Text = EAATelescope.EquatorialSystem.ToString();
                    Speak("Telescope connected");
                    StartTelescopeStatus();
                    WriteMessage("Telescope: Connected\r\n");

                }
                else
                {
                    //StopTelescopeStatus();
                    cbConnect.Checked = false;

                }
            }
            else 
            {
                StopTelescopeStatus();
                EAATelescope.Disconnect();
                WriteMessage("Telescope: Disconnected\r\n");

            }
        }

        private void workerTelescopeStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int i = 0;

            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    worker.ReportProgress(0);
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                    worker.ReportProgress(i++);
                }
            }
        }

        private void workerTelescopeStatus_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DisplayTelescopeInformation();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (EAATelescope.AbortSlew())
            {
                Speak("Slew aborted");
            }
            else
            {
                Speak("Failed to abort slew!");
            }
        }

        private void tcExtra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EAATelescope.Connected)
            {
                if (tcExtra.SelectedIndex == 1)
                {
                    StellCount = 0;
                    StartTelescopeStatus();
                }
                else
                {
                    StopTelescopeStatus();
                }
            }
        }

        private void btnStabilise_Click(object sender, EventArgs e)
        {
            EAATelescope.Stabilise();
        }

        private bool ScreenShot(int Width, int Height, string Filepath)
        {
            bool bResult = false;

            if (Width == -1) Width = Screen.PrimaryScreen.Bounds.Width;
            if (Height == -1) Height = Screen.PrimaryScreen.Bounds.Height;

            Bitmap screenshot = new Bitmap(Width, Height);

            // Create a graphics object from the bitmap
            Graphics graphics = Graphics.FromImage(screenshot);

            // Capture the screen
            graphics.CopyFromScreen(0, 0, 0, 0, screenshot.Size);

            // Save the screenshot to a file
            screenshot.Save(Filepath, ImageFormat.Png);

            return bResult;
        }

        private void LogAttachment(string ObjectName, string FilePath, string Notes)
        {
            // Create or append an observation in AstroPlanner
            APLog oAPLogCmd = new APLog();
            oAPLogCmd.script = "EAAControl2";

            oAPLogCmd.parameters = new APLogParams();
            oAPLogCmd.parameters.Cmd = 3;
            oAPLogCmd.parameters.ObjectInfo = Uri.EscapeDataString(ObjectName);
            oAPLogCmd.parameters.ImgSettingsPath = "";
            oAPLogCmd.parameters.ImgPath = Uri.EscapeDataString(FilePath);
            oAPLogCmd.parameters.ImgInfo = Notes;
            oAPLogCmd.parameters.Option = 3;

            string sOut = APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APLog>(oAPLogCmd)));
        }

        private void btnLogSharpCap_Click(object sender, EventArgs e)
        {
            // Get the selected object (if any) from AstroPlanner
            APCmdObject SelectedObject = APGetSelectedObject();
            if (SelectedObject == null)
            {
                WriteMessage("Add Attachment - FAILED!\r\n");
                MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string ObjectName = APHelper.TargetDisplay(SelectedObject);

            string FilePath = "C:\\SharpCap Captures\\Attachments\\SC_" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss.fK") + ".png";
            ScreenShot(2290, -1, FilePath);
            LogAttachment(ObjectName, FilePath,"SharpCap Screenshot");
            WriteMessage("Log - " + SelectedObject.ID + " SC screenshot\r\n");
        }

        private void btnLogFullScreen_Click(object sender, EventArgs e)
        {
            // Get the selected object (if any) from AstroPlanner
            APCmdObject SelectedObject = APGetSelectedObject();
            if (SelectedObject == null)
            {
                WriteMessage("Add Attachment - FAILED!\r\n");
                MessageBox.Show("No object selected in AstroPlanner.", "EAACtrl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ObjectName = APHelper.TargetDisplay(SelectedObject);

            string FilePath = "C:\\SharpCap Captures\\Attachments\\PS_" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss.fK") + ".png";
            ScreenShot(-1, -1, FilePath);
            LogAttachment(ObjectName, FilePath, "Primary Screen Screenshot");
            WriteMessage("Log - " + SelectedObject.ID + " Full screenshot\r\n");
        }

        private void cbStellariumShowStars_CheckedChanged(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("actionShow_Stars", cbStellariumShowStars.Checked.ToString());
            WriteMessage(Stellarium.Message);
        }

        private void cbStellariumShowSSO_CheckedChanged(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("actionShow_Planets", cbStellariumShowSSO.Checked.ToString());
            WriteMessage(Stellarium.Message);
        }

        private void btnAddAlignmentPoint_Click(object sender, EventArgs e)
        {
            // Step 1: Solve current image
            SharpCapCmd("PlateSolveAlign");

            //Wait until RA/Dec returned or 10 seconds has elasped (in which case fail)

            // Step 2: Calculate offset (display it somewhere)
            //double Offset = AstroCalc.DeltaFromTwoRADec(0.0, 0.0, 1.0, 1.0);

            // Step 3: Add alignment point
            //if (EAATelescope.AddAlignmentPoint(0.0,0.0))
            //{
            //    Speak("Alignment point added.");
            //}
            
            //WriteMessage(EAATelescope.Message + "\r\n");
        }

        private void btnAPASCOMTest_Click(object sender, EventArgs e)
        {
            EAATelescope.Connect();
            
            string sDriverVersion = EAATelescope.DriverVersion;

            bool bCanPark = EAATelescope.CanPark;

            bool bAtPark = EAATelescope.AtPark;

            EAATelescope.UnPark();

            bool bCanSetTracking = EAATelescope.CanSetTracking;

            EAATelescope.Tracking = true;

        }

        private void cbStellariumMinorBodyMarkers_CheckedChanged(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("actionShow_Planets_ShowMinorBodyMarkers", cbStellariumMinorBodyMarkers.Checked.ToString());
            WriteMessage(Stellarium.Message);
        }

        private void btnSlew_Click(object sender, EventArgs e)
        {
            // Fetch currently selected object in Stellarium
            APCmdObject obj = Stellarium.StellariumGetSelectedObjectInfo();
            if (obj != null)
            {
                DialogResult dialogResult = MessageBox.Show("Confirm telescope SLEW?", "EAACtrl", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    WriteMessage("Slew Stellarium - Slew confirmed by user.\r\n");

                    if (AstroCalc.J2000ToAltAz(obj.RA2000, obj.Dec2000, out double Alt, out double Az))
                    {
                        // Warn if slewing to an object below the local horizon.
                        if (Alt < 0)
                        {
                            dialogResult = MessageBox.Show("WARNING Object is " + Alt.ToString("F3") + "° below horizon. Confirm slew?", "EAACtrl", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialogResult == DialogResult.No)
                            {
                                WriteMessage("Slew Stellarium: Below horizon, slew cancelled by user.\r\n");
                                return;
                            }
                        }
                    }
                    else 
                    {
                        WriteMessage("Slew Stellarium: Alt-Az calculation failed.\r\n");
                        Speak("Slew error");
                        return;
                    }
                    
                    AstroCalc astroCalc = new AstroCalc();
                    astroCalc.J2000ToJNOW(obj.RA2000, obj.Dec2000, out double RANow, out double DecNow);
                    if (EAATelescope.Slew(RANow, DecNow))
                    {
                        WriteMessage("Slew Stellarium - Slewing...\r\n");
                    }
                    else
                    {
                        WriteMessage("Slew Stellarium - command failed!\r\n");
                    }
                }
                else
                {
                    WriteMessage("Slew Stellarium - cancelled by user.\r\n");
                }
            }
            else
            {
                Speak("No stellarium object selected");
            }
        }

        private void btnImagerView_Click(object sender, EventArgs e)
        {
            Stellarium.SetStellariumFOV(0.66);
            WriteMessage(Stellarium.Message);
        }

        // Finds the differnce between the objects RA/Dec after a slew and the telescope's actual platesolve position.
        // If the difference is greater than a specified accuarcy a new slew coordinate is calaculated to compensate for the error.
        public bool CentreObject(double PlateSolveRA, double PlateSolveDec)
        {
            bool bResult = false;
            try
            {
                double RATele = EAATelescope.RightAscension;
                double DecTele = EAATelescope.Declination;

                WriteMessage("PSRAJ2000=" + PlateSolveRA.ToString() + " PSDecJ2000=" +  PlateSolveDec.ToString() + "\r\n");
                WriteMessage("RATeleNOW=" + RATele.ToString() + " DecTeleNOW=" + DecTele.ToString() + "\r\n");

                AstroCalc.J2000ToJNOW(PlateSolveRA, PlateSolveDec, out double RANOW, out double DecNow);

                WriteMessage("PSRAJNOW=" + RANOW.ToString() + " PSRANOW=" + DecNow.ToString() + "\r\n");

                double RADiff = RATele - RANOW;
                double DecDiff = DecTele - DecNow;

                WriteMessage("RADiff=" + RADiff.ToString() + " DecDiff=" + DecDiff.ToString() + "\r\n");

                // TODO: Need to make sure calculated RA/Dec are in valid range.
                double SlewRA = RATele + RADiff;
                double SlewDec = DecTele + RADiff;

                WriteMessage("SlewRA=" + SlewRA.ToString() + " SlewDec=" + SlewDec.ToString() + "\r\n");

            }
            catch (Exception ex)
            {
                WriteMessage("CentreObject - " + ex.Message);
            }

            //EAATelescope.Slew(SlewRA, SlewDec);

            return bResult;
        }

        private void btnTracking_Click(object sender, EventArgs e)
        {
            if (EAATelescope.Tracking)
            {
                EAATelescope.Tracking = false;
                WriteMessage("Tracking OFF\r\n");
            }
            else
            {
                EAATelescope.Tracking = true;
                WriteMessage("Tracking ON\r\n");
            }
        }
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
        public string Params { get; set; }
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
        public string GalaxyType { get; set; }
        public int PosAngle { get; set; }
        public double Magnitude { get; set; }
        public double RA2000 { get; set; }
        public double Dec2000 { get; set; }
        public double ParallacticAngle { get; set; }
    }
    
    public class APLog
    {
        public string script { get; set; }
        public APLogParams parameters { get; set; }
    }

    public class APLogParams
    {
        public int Cmd { get; set; }
        public int Option { get; set; }
        public string ObjectInfo { get; set; }
        public string ImgSettingsPath { get; set; }
        public string ImgPath { get; set; }
        public string ImgInfo { get; set; }
    }

}