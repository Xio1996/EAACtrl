namespace EAACtrl
{
    partial class frmEAACP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEAACP));
            this.btnTarget = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.cbImagerZoom = new System.Windows.Forms.CheckBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.btnLogPlus = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.txtStatusMsg = new System.Windows.Forms.TextBox();
            this.cbCaptureProfile = new System.Windows.Forms.ComboBox();
            this.tcExtra = new System.Windows.Forms.TabControl();
            this.tpTools = new System.Windows.Forms.TabPage();
            this.cbPLGetObject = new System.Windows.Forms.ComboBox();
            this.btnGetPlanetariumObject = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOverlayText = new System.Windows.Forms.Button();
            this.txtOverlay = new System.Windows.Forms.TextBox();
            this.cbTextOverlay = new System.Windows.Forms.CheckBox();
            this.cbAPGetObjects = new System.Windows.Forms.ComboBox();
            this.btnAPGetObject = new System.Windows.Forms.Button();
            this.btnStelGetObjectInfo = new System.Windows.Forms.Button();
            this.btnPlanetaryMoons = new System.Windows.Forms.Button();
            this.btnAsteroidsFOV = new System.Windows.Forms.Button();
            this.btnSCDSA = new System.Windows.Forms.Button();
            this.tpConfig = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSAMPConnect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSyncPlanetarium = new System.Windows.Forms.CheckBox();
            this.cbSlewOnTarget = new System.Windows.Forms.CheckBox();
            this.cbFOVICorr = new System.Windows.Forms.CheckBox();
            this.tpDev = new System.Windows.Forms.TabPage();
            this.tbMessages = new System.Windows.Forms.TextBox();
            this.tabPlanetarium = new System.Windows.Forms.TabControl();
            this.tabpStellarium = new System.Windows.Forms.TabPage();
            this.btnStellariumClearMarkers = new System.Windows.Forms.Button();
            this.btnAllCats = new System.Windows.Forms.Button();
            this.btnCV = new System.Windows.Forms.Button();
            this.btnNV = new System.Windows.Forms.Button();
            this.btnS = new System.Windows.Forms.Button();
            this.btnE = new System.Windows.Forms.Button();
            this.btnW = new System.Windows.Forms.Button();
            this.btnN = new System.Windows.Forms.Button();
            this.tabpTheSky = new System.Windows.Forms.TabPage();
            this.btnAzAltFOVI = new System.Windows.Forms.Button();
            this.tabpSN = new System.Windows.Forms.TabPage();
            this.btnSNCV = new System.Windows.Forms.Button();
            this.btnSNNV = new System.Windows.Forms.Button();
            this.btnSNS = new System.Windows.Forms.Button();
            this.btnSNE = new System.Windows.Forms.Button();
            this.btnSNW = new System.Windows.Forms.Button();
            this.btnSNN = new System.Windows.Forms.Button();
            this.tcExtra.SuspendLayout();
            this.tpTools.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tpConfig.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpDev.SuspendLayout();
            this.tabPlanetarium.SuspendLayout();
            this.tabpStellarium.SuspendLayout();
            this.tabpTheSky.SuspendLayout();
            this.tabpSN.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTarget
            // 
            this.btnTarget.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTarget.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnTarget.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTarget.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTarget.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTarget.Location = new System.Drawing.Point(5, 4);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(104, 32);
            this.btnTarget.TabIndex = 2;
            this.btnTarget.Text = "Target";
            this.btnTarget.UseVisualStyleBackColor = false;
            this.btnTarget.Click += new System.EventHandler(this.btnTarget_Click);
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.Color.Gold;
            this.btnView.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnView.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnView.Location = new System.Drawing.Point(5, 118);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(136, 32);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "Sync Planetarium";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cbImagerZoom
            // 
            this.cbImagerZoom.AutoSize = true;
            this.cbImagerZoom.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 10F);
            this.cbImagerZoom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbImagerZoom.Location = new System.Drawing.Point(143, 123);
            this.cbImagerZoom.Name = "cbImagerZoom";
            this.cbImagerZoom.Size = new System.Drawing.Size(71, 23);
            this.cbImagerZoom.TabIndex = 3;
            this.cbImagerZoom.Text = "Imager";
            this.cbImagerZoom.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFind.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFind.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFind.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFind.Location = new System.Drawing.Point(5, 42);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(104, 32);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "SC Find";
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCapture.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCapture.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCapture.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCapture.Location = new System.Drawing.Point(115, 4);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(104, 32);
            this.btnCapture.TabIndex = 8;
            this.btnCapture.Text = "Observe";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnLog
            // 
            this.btnLog.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnLog.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnLog.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLog.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLog.Location = new System.Drawing.Point(5, 80);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(104, 32);
            this.btnLog.TabIndex = 9;
            this.btnLog.Text = "Log";
            this.btnLog.UseVisualStyleBackColor = false;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnLogPlus
            // 
            this.btnLogPlus.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnLogPlus.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnLogPlus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLogPlus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogPlus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLogPlus.Location = new System.Drawing.Point(115, 80);
            this.btnLogPlus.Name = "btnLogPlus";
            this.btnLogPlus.Size = new System.Drawing.Size(104, 32);
            this.btnLogPlus.TabIndex = 15;
            this.btnLogPlus.Text = "Log+";
            this.btnLogPlus.UseVisualStyleBackColor = false;
            this.btnLogPlus.Click += new System.EventHandler(this.btnLogPlus_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnExpand.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExpand.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExpand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExpand.Location = new System.Drawing.Point(177, 302);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(37, 32);
            this.btnExpand.TabIndex = 18;
            this.btnExpand.Text = ">>";
            this.btnExpand.UseVisualStyleBackColor = false;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // txtStatusMsg
            // 
            this.txtStatusMsg.Font = new System.Drawing.Font("Arial Nova", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusMsg.Location = new System.Drawing.Point(225, 306);
            this.txtStatusMsg.Name = "txtStatusMsg";
            this.txtStatusMsg.Size = new System.Drawing.Size(369, 22);
            this.txtStatusMsg.TabIndex = 19;
            // 
            // cbCaptureProfile
            // 
            this.cbCaptureProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCaptureProfile.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 10F);
            this.cbCaptureProfile.FormattingEnabled = true;
            this.cbCaptureProfile.Items.AddRange(new object[] {
            "Profile 1",
            "Profile 2",
            "Profile 3"});
            this.cbCaptureProfile.Location = new System.Drawing.Point(115, 42);
            this.cbCaptureProfile.Name = "cbCaptureProfile";
            this.cbCaptureProfile.Size = new System.Drawing.Size(104, 25);
            this.cbCaptureProfile.TabIndex = 20;
            // 
            // tcExtra
            // 
            this.tcExtra.Controls.Add(this.tpTools);
            this.tcExtra.Controls.Add(this.tpConfig);
            this.tcExtra.Controls.Add(this.tpDev);
            this.tcExtra.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.tcExtra.Location = new System.Drawing.Point(223, 6);
            this.tcExtra.Name = "tcExtra";
            this.tcExtra.SelectedIndex = 0;
            this.tcExtra.Size = new System.Drawing.Size(376, 298);
            this.tcExtra.TabIndex = 22;
            // 
            // tpTools
            // 
            this.tpTools.Controls.Add(this.cbPLGetObject);
            this.tpTools.Controls.Add(this.btnGetPlanetariumObject);
            this.tpTools.Controls.Add(this.groupBox2);
            this.tpTools.Controls.Add(this.cbAPGetObjects);
            this.tpTools.Controls.Add(this.btnAPGetObject);
            this.tpTools.Controls.Add(this.btnStelGetObjectInfo);
            this.tpTools.Controls.Add(this.btnPlanetaryMoons);
            this.tpTools.Controls.Add(this.btnAsteroidsFOV);
            this.tpTools.Controls.Add(this.btnSCDSA);
            this.tpTools.Location = new System.Drawing.Point(4, 26);
            this.tpTools.Name = "tpTools";
            this.tpTools.Padding = new System.Windows.Forms.Padding(3);
            this.tpTools.Size = new System.Drawing.Size(368, 268);
            this.tpTools.TabIndex = 0;
            this.tpTools.Text = "Tools";
            this.tpTools.UseVisualStyleBackColor = true;
            // 
            // cbPLGetObject
            // 
            this.cbPLGetObject.BackColor = System.Drawing.Color.PaleGreen;
            this.cbPLGetObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPLGetObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbPLGetObject.FormattingEnabled = true;
            this.cbPLGetObject.Items.AddRange(new object[] {
            "DSA",
            "Object ID only",
            "Object full information (csv)",
            "RA/Dec J2000 (CDS/Simbad)",
            "AstroPlanner, add to plan",
            "SAMP coords (Aladin)"});
            this.cbPLGetObject.Location = new System.Drawing.Point(130, 47);
            this.cbPLGetObject.Name = "cbPLGetObject";
            this.cbPLGetObject.Size = new System.Drawing.Size(230, 25);
            this.cbPLGetObject.TabIndex = 36;
            // 
            // btnGetPlanetariumObject
            // 
            this.btnGetPlanetariumObject.BackColor = System.Drawing.Color.PaleGreen;
            this.btnGetPlanetariumObject.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnGetPlanetariumObject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGetPlanetariumObject.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGetPlanetariumObject.Location = new System.Drawing.Point(6, 44);
            this.btnGetPlanetariumObject.Name = "btnGetPlanetariumObject";
            this.btnGetPlanetariumObject.Size = new System.Drawing.Size(120, 32);
            this.btnGetPlanetariumObject.TabIndex = 35;
            this.btnGetPlanetariumObject.Text = "PL Get Object";
            this.btnGetPlanetariumObject.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOverlayText);
            this.groupBox2.Controls.Add(this.txtOverlay);
            this.groupBox2.Controls.Add(this.cbTextOverlay);
            this.groupBox2.Location = new System.Drawing.Point(9, 126);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(351, 74);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Object Text Overlay";
            // 
            // btnOverlayText
            // 
            this.btnOverlayText.Font = new System.Drawing.Font("Segoe UI Variable Small Semibol", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOverlayText.Location = new System.Drawing.Point(289, 18);
            this.btnOverlayText.Name = "btnOverlayText";
            this.btnOverlayText.Size = new System.Drawing.Size(56, 32);
            this.btnOverlayText.TabIndex = 30;
            this.btnOverlayText.Text = "Apply";
            this.btnOverlayText.UseVisualStyleBackColor = true;
            this.btnOverlayText.Click += new System.EventHandler(this.btnOverlayText_Click_2);
            // 
            // txtOverlay
            // 
            this.txtOverlay.Location = new System.Drawing.Point(6, 22);
            this.txtOverlay.Name = "txtOverlay";
            this.txtOverlay.Size = new System.Drawing.Size(277, 25);
            this.txtOverlay.TabIndex = 29;
            // 
            // cbTextOverlay
            // 
            this.cbTextOverlay.AutoSize = true;
            this.cbTextOverlay.Checked = true;
            this.cbTextOverlay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTextOverlay.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 10F);
            this.cbTextOverlay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbTextOverlay.Location = new System.Drawing.Point(7, 50);
            this.cbTextOverlay.Name = "cbTextOverlay";
            this.cbTextOverlay.Size = new System.Drawing.Size(70, 23);
            this.cbTextOverlay.TabIndex = 28;
            this.cbTextOverlay.Text = "Visible";
            this.cbTextOverlay.UseVisualStyleBackColor = true;
            this.cbTextOverlay.CheckedChanged += new System.EventHandler(this.cbTextOverlay_CheckedChanged_1);
            // 
            // cbAPGetObjects
            // 
            this.cbAPGetObjects.BackColor = System.Drawing.Color.LightSalmon;
            this.cbAPGetObjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAPGetObjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAPGetObjects.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbAPGetObjects.FormattingEnabled = true;
            this.cbAPGetObjects.Items.AddRange(new object[] {
            "DSA (JPL lookup for minor bodies)",
            "DSA (AP only)",
            "IDs (newline list, TheSky)",
            "RA/Dec J2000 (CDS/Simbad)",
            "SAMP Sky Coords (Aladin)"});
            this.cbAPGetObjects.Location = new System.Drawing.Point(130, 9);
            this.cbAPGetObjects.Name = "cbAPGetObjects";
            this.cbAPGetObjects.Size = new System.Drawing.Size(230, 25);
            this.cbAPGetObjects.TabIndex = 13;
            // 
            // btnAPGetObject
            // 
            this.btnAPGetObject.BackColor = System.Drawing.Color.LightSalmon;
            this.btnAPGetObject.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAPGetObject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAPGetObject.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAPGetObject.Location = new System.Drawing.Point(6, 6);
            this.btnAPGetObject.Name = "btnAPGetObject";
            this.btnAPGetObject.Size = new System.Drawing.Size(120, 32);
            this.btnAPGetObject.TabIndex = 12;
            this.btnAPGetObject.Text = "AP GetObjects";
            this.btnAPGetObject.UseVisualStyleBackColor = false;
            this.btnAPGetObject.Click += new System.EventHandler(this.btnAPGetObject_Click);
            // 
            // btnStelGetObjectInfo
            // 
            this.btnStelGetObjectInfo.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnStelGetObjectInfo.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnStelGetObjectInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStelGetObjectInfo.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStelGetObjectInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStelGetObjectInfo.Location = new System.Drawing.Point(154, 220);
            this.btnStelGetObjectInfo.Name = "btnStelGetObjectInfo";
            this.btnStelGetObjectInfo.Size = new System.Drawing.Size(74, 42);
            this.btnStelGetObjectInfo.TabIndex = 11;
            this.btnStelGetObjectInfo.Text = "ST GetObject";
            this.btnStelGetObjectInfo.UseVisualStyleBackColor = false;
            this.btnStelGetObjectInfo.Click += new System.EventHandler(this.btnStelGetObjectInfo_Click);
            // 
            // btnPlanetaryMoons
            // 
            this.btnPlanetaryMoons.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnPlanetaryMoons.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPlanetaryMoons.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPlanetaryMoons.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPlanetaryMoons.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlanetaryMoons.Location = new System.Drawing.Point(142, 85);
            this.btnPlanetaryMoons.Name = "btnPlanetaryMoons";
            this.btnPlanetaryMoons.Size = new System.Drawing.Size(130, 32);
            this.btnPlanetaryMoons.TabIndex = 7;
            this.btnPlanetaryMoons.Text = "Moons...";
            this.btnPlanetaryMoons.UseVisualStyleBackColor = false;
            this.btnPlanetaryMoons.Click += new System.EventHandler(this.btnPlanetaryMoons_Click);
            // 
            // btnAsteroidsFOV
            // 
            this.btnAsteroidsFOV.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAsteroidsFOV.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAsteroidsFOV.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAsteroidsFOV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAsteroidsFOV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAsteroidsFOV.Location = new System.Drawing.Point(6, 86);
            this.btnAsteroidsFOV.Name = "btnAsteroidsFOV";
            this.btnAsteroidsFOV.Size = new System.Drawing.Size(130, 32);
            this.btnAsteroidsFOV.TabIndex = 6;
            this.btnAsteroidsFOV.Text = "Asteroids FOV...";
            this.btnAsteroidsFOV.UseVisualStyleBackColor = false;
            this.btnAsteroidsFOV.Click += new System.EventHandler(this.btnAsteroidsFOV_Click);
            // 
            // btnSCDSA
            // 
            this.btnSCDSA.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSCDSA.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSCDSA.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSCDSA.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSCDSA.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSCDSA.Location = new System.Drawing.Point(233, 229);
            this.btnSCDSA.Name = "btnSCDSA";
            this.btnSCDSA.Size = new System.Drawing.Size(121, 25);
            this.btnSCDSA.TabIndex = 4;
            this.btnSCDSA.Text = "SC DSA obsolete";
            this.btnSCDSA.UseVisualStyleBackColor = false;
            this.btnSCDSA.Click += new System.EventHandler(this.btnSCDSA_Click);
            // 
            // tpConfig
            // 
            this.tpConfig.Controls.Add(this.groupBox3);
            this.tpConfig.Controls.Add(this.groupBox1);
            this.tpConfig.Controls.Add(this.cbFOVICorr);
            this.tpConfig.Location = new System.Drawing.Point(4, 26);
            this.tpConfig.Name = "tpConfig";
            this.tpConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfig.Size = new System.Drawing.Size(368, 268);
            this.tpConfig.TabIndex = 2;
            this.tpConfig.Text = "Config";
            this.tpConfig.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.btnSAMPConnect);
            this.groupBox3.Location = new System.Drawing.Point(9, 60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(351, 57);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SAMP (Aladin)";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(94, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 32);
            this.button1.TabIndex = 33;
            this.button1.Text = "Disconnect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnSAMPConnect
            // 
            this.btnSAMPConnect.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSAMPConnect.Location = new System.Drawing.Point(8, 20);
            this.btnSAMPConnect.Name = "btnSAMPConnect";
            this.btnSAMPConnect.Size = new System.Drawing.Size(80, 32);
            this.btnSAMPConnect.TabIndex = 32;
            this.btnSAMPConnect.Text = "Connect";
            this.btnSAMPConnect.UseVisualStyleBackColor = true;
            this.btnSAMPConnect.Click += new System.EventHandler(this.btnSAMPConnect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSyncPlanetarium);
            this.groupBox1.Controls.Add(this.cbSlewOnTarget);
            this.groupBox1.Location = new System.Drawing.Point(10, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 50);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target";
            // 
            // cbSyncPlanetarium
            // 
            this.cbSyncPlanetarium.AutoSize = true;
            this.cbSyncPlanetarium.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 10F);
            this.cbSyncPlanetarium.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbSyncPlanetarium.Location = new System.Drawing.Point(127, 22);
            this.cbSyncPlanetarium.Name = "cbSyncPlanetarium";
            this.cbSyncPlanetarium.Size = new System.Drawing.Size(136, 23);
            this.cbSyncPlanetarium.TabIndex = 32;
            this.cbSyncPlanetarium.Text = "Sync Planetarium";
            this.cbSyncPlanetarium.UseVisualStyleBackColor = true;
            // 
            // cbSlewOnTarget
            // 
            this.cbSlewOnTarget.AutoSize = true;
            this.cbSlewOnTarget.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 10F);
            this.cbSlewOnTarget.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbSlewOnTarget.Location = new System.Drawing.Point(7, 22);
            this.cbSlewOnTarget.Name = "cbSlewOnTarget";
            this.cbSlewOnTarget.Size = new System.Drawing.Size(114, 23);
            this.cbSlewOnTarget.TabIndex = 31;
            this.cbSlewOnTarget.Text = "Slew to target";
            this.cbSlewOnTarget.UseVisualStyleBackColor = true;
            // 
            // cbFOVICorr
            // 
            this.cbFOVICorr.AutoSize = true;
            this.cbFOVICorr.Checked = true;
            this.cbFOVICorr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFOVICorr.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 10F);
            this.cbFOVICorr.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbFOVICorr.Location = new System.Drawing.Point(17, 235);
            this.cbFOVICorr.Name = "cbFOVICorr";
            this.cbFOVICorr.Size = new System.Drawing.Size(119, 23);
            this.cbFOVICorr.TabIndex = 14;
            this.cbFOVICorr.Text = "TS Alt-Az FOVI";
            this.cbFOVICorr.UseVisualStyleBackColor = true;
            // 
            // tpDev
            // 
            this.tpDev.Controls.Add(this.tbMessages);
            this.tpDev.Location = new System.Drawing.Point(4, 26);
            this.tpDev.Name = "tpDev";
            this.tpDev.Padding = new System.Windows.Forms.Padding(3);
            this.tpDev.Size = new System.Drawing.Size(368, 268);
            this.tpDev.TabIndex = 1;
            this.tpDev.Text = "Log";
            this.tpDev.UseVisualStyleBackColor = true;
            // 
            // tbMessages
            // 
            this.tbMessages.Font = new System.Drawing.Font("Arial Nova", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMessages.Location = new System.Drawing.Point(7, 6);
            this.tbMessages.Multiline = true;
            this.tbMessages.Name = "tbMessages";
            this.tbMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMessages.Size = new System.Drawing.Size(355, 256);
            this.tbMessages.TabIndex = 20;
            this.tbMessages.WordWrap = false;
            // 
            // tabPlanetarium
            // 
            this.tabPlanetarium.Controls.Add(this.tabpStellarium);
            this.tabPlanetarium.Controls.Add(this.tabpTheSky);
            this.tabPlanetarium.Controls.Add(this.tabpSN);
            this.tabPlanetarium.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 10F);
            this.tabPlanetarium.Location = new System.Drawing.Point(3, 156);
            this.tabPlanetarium.Name = "tabPlanetarium";
            this.tabPlanetarium.SelectedIndex = 0;
            this.tabPlanetarium.Size = new System.Drawing.Size(213, 146);
            this.tabPlanetarium.TabIndex = 30;
            // 
            // tabpStellarium
            // 
            this.tabpStellarium.Controls.Add(this.btnStellariumClearMarkers);
            this.tabpStellarium.Controls.Add(this.btnAllCats);
            this.tabpStellarium.Controls.Add(this.btnCV);
            this.tabpStellarium.Controls.Add(this.btnNV);
            this.tabpStellarium.Controls.Add(this.btnS);
            this.tabpStellarium.Controls.Add(this.btnE);
            this.tabpStellarium.Controls.Add(this.btnW);
            this.tabpStellarium.Controls.Add(this.btnN);
            this.tabpStellarium.Location = new System.Drawing.Point(4, 26);
            this.tabpStellarium.Name = "tabpStellarium";
            this.tabpStellarium.Padding = new System.Windows.Forms.Padding(3);
            this.tabpStellarium.Size = new System.Drawing.Size(205, 116);
            this.tabpStellarium.TabIndex = 0;
            this.tabpStellarium.Text = "Stellarium";
            this.tabpStellarium.UseVisualStyleBackColor = true;
            // 
            // btnStellariumClearMarkers
            // 
            this.btnStellariumClearMarkers.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnStellariumClearMarkers.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnStellariumClearMarkers.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStellariumClearMarkers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStellariumClearMarkers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStellariumClearMarkers.Location = new System.Drawing.Point(10, 79);
            this.btnStellariumClearMarkers.Name = "btnStellariumClearMarkers";
            this.btnStellariumClearMarkers.Size = new System.Drawing.Size(104, 32);
            this.btnStellariumClearMarkers.TabIndex = 37;
            this.btnStellariumClearMarkers.Text = "Clear Markers";
            this.btnStellariumClearMarkers.UseVisualStyleBackColor = false;
            this.btnStellariumClearMarkers.Click += new System.EventHandler(this.btnStellariumClearMarkers_Click);
            // 
            // btnAllCats
            // 
            this.btnAllCats.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAllCats.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAllCats.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAllCats.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAllCats.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAllCats.Location = new System.Drawing.Point(10, 43);
            this.btnAllCats.Name = "btnAllCats";
            this.btnAllCats.Size = new System.Drawing.Size(104, 32);
            this.btnAllCats.TabIndex = 36;
            this.btnAllCats.Text = "Def Cats";
            this.btnAllCats.UseVisualStyleBackColor = false;
            this.btnAllCats.Click += new System.EventHandler(this.btnAllCats_Click_1);
            // 
            // btnCV
            // 
            this.btnCV.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnCV.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCV.Location = new System.Drawing.Point(162, 7);
            this.btnCV.Name = "btnCV";
            this.btnCV.Size = new System.Drawing.Size(36, 32);
            this.btnCV.TabIndex = 35;
            this.btnCV.Text = "CV";
            this.btnCV.UseVisualStyleBackColor = false;
            this.btnCV.Click += new System.EventHandler(this.btnCV_Click_1);
            // 
            // btnNV
            // 
            this.btnNV.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnNV.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnNV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNV.Location = new System.Drawing.Point(123, 7);
            this.btnNV.Name = "btnNV";
            this.btnNV.Size = new System.Drawing.Size(37, 32);
            this.btnNV.TabIndex = 34;
            this.btnNV.Text = "NV";
            this.btnNV.UseVisualStyleBackColor = false;
            this.btnNV.Click += new System.EventHandler(this.btnNV_Click_1);
            // 
            // btnS
            // 
            this.btnS.BackColor = System.Drawing.Color.PaleGreen;
            this.btnS.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnS.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnS.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnS.Location = new System.Drawing.Point(64, 7);
            this.btnS.Name = "btnS";
            this.btnS.Size = new System.Drawing.Size(28, 32);
            this.btnS.TabIndex = 33;
            this.btnS.Text = "S";
            this.btnS.UseVisualStyleBackColor = false;
            this.btnS.Click += new System.EventHandler(this.btnS_Click_1);
            // 
            // btnE
            // 
            this.btnE.BackColor = System.Drawing.Color.PaleGreen;
            this.btnE.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnE.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnE.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnE.Location = new System.Drawing.Point(93, 7);
            this.btnE.Name = "btnE";
            this.btnE.Size = new System.Drawing.Size(28, 32);
            this.btnE.TabIndex = 32;
            this.btnE.Text = "E";
            this.btnE.UseVisualStyleBackColor = false;
            this.btnE.Click += new System.EventHandler(this.btnE_Click_1);
            // 
            // btnW
            // 
            this.btnW.BackColor = System.Drawing.Color.PaleGreen;
            this.btnW.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnW.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnW.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnW.Location = new System.Drawing.Point(35, 7);
            this.btnW.Name = "btnW";
            this.btnW.Size = new System.Drawing.Size(28, 32);
            this.btnW.TabIndex = 31;
            this.btnW.Text = "W";
            this.btnW.UseVisualStyleBackColor = false;
            this.btnW.Click += new System.EventHandler(this.btnW_Click_1);
            // 
            // btnN
            // 
            this.btnN.BackColor = System.Drawing.Color.PaleGreen;
            this.btnN.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnN.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnN.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnN.Location = new System.Drawing.Point(6, 7);
            this.btnN.Name = "btnN";
            this.btnN.Size = new System.Drawing.Size(28, 32);
            this.btnN.TabIndex = 30;
            this.btnN.Text = "N";
            this.btnN.UseVisualStyleBackColor = false;
            this.btnN.Click += new System.EventHandler(this.btnN_Click_1);
            // 
            // tabpTheSky
            // 
            this.tabpTheSky.Controls.Add(this.btnAzAltFOVI);
            this.tabpTheSky.Location = new System.Drawing.Point(4, 26);
            this.tabpTheSky.Name = "tabpTheSky";
            this.tabpTheSky.Padding = new System.Windows.Forms.Padding(3);
            this.tabpTheSky.Size = new System.Drawing.Size(205, 116);
            this.tabpTheSky.TabIndex = 1;
            this.tabpTheSky.Text = "TheSky";
            this.tabpTheSky.UseVisualStyleBackColor = true;
            // 
            // btnAzAltFOVI
            // 
            this.btnAzAltFOVI.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAzAltFOVI.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAzAltFOVI.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAzAltFOVI.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAzAltFOVI.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAzAltFOVI.Location = new System.Drawing.Point(6, 6);
            this.btnAzAltFOVI.Name = "btnAzAltFOVI";
            this.btnAzAltFOVI.Size = new System.Drawing.Size(104, 32);
            this.btnAzAltFOVI.TabIndex = 6;
            this.btnAzAltFOVI.Text = "Alt-Az FOVI";
            this.btnAzAltFOVI.UseVisualStyleBackColor = false;
            this.btnAzAltFOVI.Click += new System.EventHandler(this.btnAzAltFOVI_Click_1);
            // 
            // tabpSN
            // 
            this.tabpSN.Controls.Add(this.btnSNCV);
            this.tabpSN.Controls.Add(this.btnSNNV);
            this.tabpSN.Controls.Add(this.btnSNS);
            this.tabpSN.Controls.Add(this.btnSNE);
            this.tabpSN.Controls.Add(this.btnSNW);
            this.tabpSN.Controls.Add(this.btnSNN);
            this.tabpSN.Location = new System.Drawing.Point(4, 26);
            this.tabpSN.Name = "tabpSN";
            this.tabpSN.Size = new System.Drawing.Size(205, 116);
            this.tabpSN.TabIndex = 2;
            this.tabpSN.Text = "SN 8.1";
            this.tabpSN.UseVisualStyleBackColor = true;
            // 
            // btnSNCV
            // 
            this.btnSNCV.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSNCV.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSNCV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSNCV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSNCV.Location = new System.Drawing.Point(162, 7);
            this.btnSNCV.Name = "btnSNCV";
            this.btnSNCV.Size = new System.Drawing.Size(36, 32);
            this.btnSNCV.TabIndex = 41;
            this.btnSNCV.Text = "CV";
            this.btnSNCV.UseVisualStyleBackColor = false;
            this.btnSNCV.Click += new System.EventHandler(this.btnSNCV_Click);
            // 
            // btnSNNV
            // 
            this.btnSNNV.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSNNV.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSNNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSNNV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSNNV.Location = new System.Drawing.Point(124, 7);
            this.btnSNNV.Name = "btnSNNV";
            this.btnSNNV.Size = new System.Drawing.Size(37, 32);
            this.btnSNNV.TabIndex = 40;
            this.btnSNNV.Text = "NV";
            this.btnSNNV.UseVisualStyleBackColor = false;
            this.btnSNNV.Click += new System.EventHandler(this.btnSNNV_Click);
            // 
            // btnSNS
            // 
            this.btnSNS.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSNS.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSNS.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSNS.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSNS.Location = new System.Drawing.Point(66, 7);
            this.btnSNS.Name = "btnSNS";
            this.btnSNS.Size = new System.Drawing.Size(28, 32);
            this.btnSNS.TabIndex = 39;
            this.btnSNS.Text = "S";
            this.btnSNS.UseVisualStyleBackColor = false;
            this.btnSNS.Click += new System.EventHandler(this.btnSNS_Click);
            // 
            // btnSNE
            // 
            this.btnSNE.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSNE.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSNE.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSNE.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSNE.Location = new System.Drawing.Point(95, 7);
            this.btnSNE.Name = "btnSNE";
            this.btnSNE.Size = new System.Drawing.Size(28, 32);
            this.btnSNE.TabIndex = 38;
            this.btnSNE.Text = "E";
            this.btnSNE.UseVisualStyleBackColor = false;
            this.btnSNE.Click += new System.EventHandler(this.btnSNE_Click);
            // 
            // btnSNW
            // 
            this.btnSNW.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSNW.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSNW.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSNW.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSNW.Location = new System.Drawing.Point(37, 7);
            this.btnSNW.Name = "btnSNW";
            this.btnSNW.Size = new System.Drawing.Size(28, 32);
            this.btnSNW.TabIndex = 37;
            this.btnSNW.Text = "W";
            this.btnSNW.UseVisualStyleBackColor = false;
            this.btnSNW.Click += new System.EventHandler(this.btnSNW_Click);
            // 
            // btnSNN
            // 
            this.btnSNN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSNN.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSNN.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSNN.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSNN.Location = new System.Drawing.Point(8, 7);
            this.btnSNN.Name = "btnSNN";
            this.btnSNN.Size = new System.Drawing.Size(28, 32);
            this.btnSNN.TabIndex = 36;
            this.btnSNN.Text = "N";
            this.btnSNN.UseVisualStyleBackColor = false;
            this.btnSNN.Click += new System.EventHandler(this.btnSNN_Click);
            // 
            // frmEAACP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(602, 338);
            this.Controls.Add(this.tabPlanetarium);
            this.Controls.Add(this.tcExtra);
            this.Controls.Add(this.cbCaptureProfile);
            this.Controls.Add(this.txtStatusMsg);
            this.Controls.Add(this.btnExpand);
            this.Controls.Add(this.btnLogPlus);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.cbImagerZoom);
            this.Controls.Add(this.btnTarget);
            this.Controls.Add(this.btnView);
            this.Font = new System.Drawing.Font("Arial Nova", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEAACP";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "EAACtrl (1.1)";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEAACP_FormClosing);
            this.Load += new System.EventHandler(this.frmEAACP_Load);
            this.Shown += new System.EventHandler(this.frmEAACP_Shown);
            this.tcExtra.ResumeLayout(false);
            this.tpTools.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tpConfig.ResumeLayout(false);
            this.tpConfig.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpDev.ResumeLayout(false);
            this.tpDev.PerformLayout();
            this.tabPlanetarium.ResumeLayout(false);
            this.tabpStellarium.ResumeLayout(false);
            this.tabpTheSky.ResumeLayout(false);
            this.tabpSN.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnTarget;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.CheckBox cbImagerZoom;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnLogPlus;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.TextBox txtStatusMsg;
        private System.Windows.Forms.ComboBox cbCaptureProfile;
        private System.Windows.Forms.TabControl tcExtra;
        private System.Windows.Forms.TabPage tpTools;
        private System.Windows.Forms.TabPage tpDev;
        private System.Windows.Forms.TextBox tbMessages;
        private System.Windows.Forms.Button btnSCDSA;
        private System.Windows.Forms.Button btnAsteroidsFOV;
        private System.Windows.Forms.Button btnPlanetaryMoons;
        private System.Windows.Forms.TabPage tpConfig;
        private System.Windows.Forms.CheckBox cbFOVICorr;
        private System.Windows.Forms.Button btnStelGetObjectInfo;
        private System.Windows.Forms.Button btnAPGetObject;
        private System.Windows.Forms.ComboBox cbAPGetObjects;
        private System.Windows.Forms.TabControl tabPlanetarium;
        private System.Windows.Forms.TabPage tabpStellarium;
        private System.Windows.Forms.TabPage tabpTheSky;
        private System.Windows.Forms.TabPage tabpSN;
        private System.Windows.Forms.Button btnAllCats;
        private System.Windows.Forms.Button btnCV;
        private System.Windows.Forms.Button btnNV;
        private System.Windows.Forms.Button btnS;
        private System.Windows.Forms.Button btnE;
        private System.Windows.Forms.Button btnW;
        private System.Windows.Forms.Button btnN;
        private System.Windows.Forms.Button btnAzAltFOVI;
        private System.Windows.Forms.Button btnSNCV;
        private System.Windows.Forms.Button btnSNNV;
        private System.Windows.Forms.Button btnSNS;
        private System.Windows.Forms.Button btnSNE;
        private System.Windows.Forms.Button btnSNW;
        private System.Windows.Forms.Button btnSNN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbSyncPlanetarium;
        private System.Windows.Forms.CheckBox cbSlewOnTarget;
        private System.Windows.Forms.Button btnStellariumClearMarkers;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSAMPConnect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOverlayText;
        private System.Windows.Forms.TextBox txtOverlay;
        private System.Windows.Forms.CheckBox cbTextOverlay;
        private System.Windows.Forms.Button btnGetPlanetariumObject;
        private System.Windows.Forms.ComboBox cbPLGetObject;
    }
}

