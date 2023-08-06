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
            this.btnAzAltFOVI = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.cbLogFind = new System.Windows.Forms.CheckBox();
            this.btnLogPlus = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.txtStatusMsg = new System.Windows.Forms.TextBox();
            this.cbCaptureProfile = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tcExtra = new System.Windows.Forms.TabControl();
            this.tpTools = new System.Windows.Forms.TabPage();
            this.grpbMBDS = new System.Windows.Forms.GroupBox();
            this.rbTheSky = new System.Windows.Forms.RadioButton();
            this.rbJPLHorizons = new System.Windows.Forms.RadioButton();
            this.btnPlanetaryMoons = new System.Windows.Forms.Button();
            this.btnAsteroidsFOV = new System.Windows.Forms.Button();
            this.btnSCDSA = new System.Windows.Forms.Button();
            this.tpConfig = new System.Windows.Forms.TabPage();
            this.btnOverlayText = new System.Windows.Forms.Button();
            this.txtOverlay = new System.Windows.Forms.TextBox();
            this.cbSlewOnTarget = new System.Windows.Forms.CheckBox();
            this.cbTextOverlay = new System.Windows.Forms.CheckBox();
            this.cbFOVICorr = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPlanetarium = new System.Windows.Forms.ComboBox();
            this.tpDev = new System.Windows.Forms.TabPage();
            this.tbMessages = new System.Windows.Forms.TextBox();
            this.btnAllCats = new System.Windows.Forms.Button();
            this.tcExtra.SuspendLayout();
            this.tpTools.SuspendLayout();
            this.grpbMBDS.SuspendLayout();
            this.tpConfig.SuspendLayout();
            this.tpDev.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTarget
            // 
            this.btnTarget.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTarget.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnTarget.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTarget.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTarget.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTarget.Location = new System.Drawing.Point(96, 4);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(88, 25);
            this.btnTarget.TabIndex = 2;
            this.btnTarget.Text = "Target";
            this.btnTarget.UseVisualStyleBackColor = false;
            this.btnTarget.Click += new System.EventHandler(this.btnTarget_Click);
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnView.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnView.Location = new System.Drawing.Point(4, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(88, 25);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "Sync";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cbImagerZoom
            // 
            this.cbImagerZoom.AutoSize = true;
            this.cbImagerZoom.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbImagerZoom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbImagerZoom.Location = new System.Drawing.Point(10, 158);
            this.cbImagerZoom.Name = "cbImagerZoom";
            this.cbImagerZoom.Size = new System.Drawing.Size(85, 19);
            this.cbImagerZoom.TabIndex = 3;
            this.cbImagerZoom.Text = "Imager FOV";
            this.cbImagerZoom.UseVisualStyleBackColor = true;
            // 
            // btnAzAltFOVI
            // 
            this.btnAzAltFOVI.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAzAltFOVI.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAzAltFOVI.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAzAltFOVI.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAzAltFOVI.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAzAltFOVI.Location = new System.Drawing.Point(5, 118);
            this.btnAzAltFOVI.Name = "btnAzAltFOVI";
            this.btnAzAltFOVI.Size = new System.Drawing.Size(58, 25);
            this.btnAzAltFOVI.TabIndex = 5;
            this.btnAzAltFOVI.Text = "FOVI";
            this.btnAzAltFOVI.UseVisualStyleBackColor = false;
            this.btnAzAltFOVI.Click += new System.EventHandler(this.btnAzAltFOVI_Click);
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFind.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFind.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFind.Location = new System.Drawing.Point(4, 32);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(88, 25);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCapture.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCapture.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCapture.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCapture.Location = new System.Drawing.Point(96, 32);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(88, 25);
            this.btnCapture.TabIndex = 8;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnLog
            // 
            this.btnLog.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnLog.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnLog.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLog.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLog.Location = new System.Drawing.Point(4, 90);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(88, 25);
            this.btnLog.TabIndex = 9;
            this.btnLog.Text = "Log";
            this.btnLog.UseVisualStyleBackColor = false;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // cbLogFind
            // 
            this.cbLogFind.AutoSize = true;
            this.cbLogFind.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cbLogFind.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLogFind.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbLogFind.Location = new System.Drawing.Point(98, 157);
            this.cbLogFind.Name = "cbLogFind";
            this.cbLogFind.Size = new System.Drawing.Size(81, 19);
            this.cbLogFind.TabIndex = 13;
            this.cbLogFind.Text = "Log && Find";
            this.cbLogFind.UseVisualStyleBackColor = true;
            // 
            // btnLogPlus
            // 
            this.btnLogPlus.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnLogPlus.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnLogPlus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLogPlus.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogPlus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLogPlus.Location = new System.Drawing.Point(96, 89);
            this.btnLogPlus.Name = "btnLogPlus";
            this.btnLogPlus.Size = new System.Drawing.Size(88, 25);
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
            this.btnExpand.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExpand.Location = new System.Drawing.Point(156, 116);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(28, 25);
            this.btnExpand.TabIndex = 18;
            this.btnExpand.Text = ">>";
            this.btnExpand.UseVisualStyleBackColor = false;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // txtStatusMsg
            // 
            this.txtStatusMsg.Font = new System.Drawing.Font("Arial Nova", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusMsg.Location = new System.Drawing.Point(190, 154);
            this.txtStatusMsg.Name = "txtStatusMsg";
            this.txtStatusMsg.Size = new System.Drawing.Size(377, 22);
            this.txtStatusMsg.TabIndex = 19;
            // 
            // cbCaptureProfile
            // 
            this.cbCaptureProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCaptureProfile.Font = new System.Drawing.Font("Arial Nova", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCaptureProfile.FormattingEnabled = true;
            this.cbCaptureProfile.Items.AddRange(new object[] {
            "CaptureMode1",
            "CaptureMode2",
            "CaptureMode3"});
            this.cbCaptureProfile.Location = new System.Drawing.Point(86, 62);
            this.cbCaptureProfile.Name = "cbCaptureProfile";
            this.cbCaptureProfile.Size = new System.Drawing.Size(98, 22);
            this.cbCaptureProfile.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(42, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "Profile";
            // 
            // tcExtra
            // 
            this.tcExtra.Controls.Add(this.tpTools);
            this.tcExtra.Controls.Add(this.tpConfig);
            this.tcExtra.Controls.Add(this.tpDev);
            this.tcExtra.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.tcExtra.Location = new System.Drawing.Point(190, 4);
            this.tcExtra.Name = "tcExtra";
            this.tcExtra.SelectedIndex = 0;
            this.tcExtra.Size = new System.Drawing.Size(377, 147);
            this.tcExtra.TabIndex = 22;
            // 
            // tpTools
            // 
            this.tpTools.Controls.Add(this.grpbMBDS);
            this.tpTools.Controls.Add(this.btnPlanetaryMoons);
            this.tpTools.Controls.Add(this.btnAsteroidsFOV);
            this.tpTools.Controls.Add(this.btnSCDSA);
            this.tpTools.Location = new System.Drawing.Point(4, 26);
            this.tpTools.Name = "tpTools";
            this.tpTools.Padding = new System.Windows.Forms.Padding(3);
            this.tpTools.Size = new System.Drawing.Size(369, 117);
            this.tpTools.TabIndex = 0;
            this.tpTools.Text = "Tools";
            this.tpTools.UseVisualStyleBackColor = true;
            // 
            // grpbMBDS
            // 
            this.grpbMBDS.Controls.Add(this.rbTheSky);
            this.grpbMBDS.Controls.Add(this.rbJPLHorizons);
            this.grpbMBDS.Location = new System.Drawing.Point(6, 34);
            this.grpbMBDS.Name = "grpbMBDS";
            this.grpbMBDS.Size = new System.Drawing.Size(175, 59);
            this.grpbMBDS.TabIndex = 9;
            this.grpbMBDS.TabStop = false;
            this.grpbMBDS.Text = "Minor Bodies DS";
            // 
            // rbTheSky
            // 
            this.rbTheSky.AutoSize = true;
            this.rbTheSky.Enabled = false;
            this.rbTheSky.Location = new System.Drawing.Point(7, 36);
            this.rbTheSky.Name = "rbTheSky";
            this.rbTheSky.Size = new System.Drawing.Size(69, 21);
            this.rbTheSky.TabIndex = 1;
            this.rbTheSky.Text = "TheSky";
            this.rbTheSky.UseVisualStyleBackColor = true;
            // 
            // rbJPLHorizons
            // 
            this.rbJPLHorizons.AutoSize = true;
            this.rbJPLHorizons.Checked = true;
            this.rbJPLHorizons.Location = new System.Drawing.Point(7, 17);
            this.rbJPLHorizons.Name = "rbJPLHorizons";
            this.rbJPLHorizons.Size = new System.Drawing.Size(103, 21);
            this.rbJPLHorizons.TabIndex = 0;
            this.rbJPLHorizons.TabStop = true;
            this.rbJPLHorizons.Text = "JPL Horizons";
            this.rbJPLHorizons.UseVisualStyleBackColor = true;
            // 
            // btnPlanetaryMoons
            // 
            this.btnPlanetaryMoons.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnPlanetaryMoons.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPlanetaryMoons.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPlanetaryMoons.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlanetaryMoons.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlanetaryMoons.Location = new System.Drawing.Point(188, 3);
            this.btnPlanetaryMoons.Name = "btnPlanetaryMoons";
            this.btnPlanetaryMoons.Size = new System.Drawing.Size(128, 25);
            this.btnPlanetaryMoons.TabIndex = 7;
            this.btnPlanetaryMoons.Text = "Planetary Moons...";
            this.btnPlanetaryMoons.UseVisualStyleBackColor = false;
            this.btnPlanetaryMoons.Click += new System.EventHandler(this.btnPlanetaryMoons_Click);
            // 
            // btnAsteroidsFOV
            // 
            this.btnAsteroidsFOV.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAsteroidsFOV.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAsteroidsFOV.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAsteroidsFOV.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAsteroidsFOV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAsteroidsFOV.Location = new System.Drawing.Point(78, 3);
            this.btnAsteroidsFOV.Name = "btnAsteroidsFOV";
            this.btnAsteroidsFOV.Size = new System.Drawing.Size(103, 25);
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
            this.btnSCDSA.Location = new System.Drawing.Point(6, 3);
            this.btnSCDSA.Name = "btnSCDSA";
            this.btnSCDSA.Size = new System.Drawing.Size(65, 25);
            this.btnSCDSA.TabIndex = 4;
            this.btnSCDSA.Text = "SC DSA";
            this.btnSCDSA.UseVisualStyleBackColor = false;
            this.btnSCDSA.Click += new System.EventHandler(this.btnSCDSA_Click);
            // 
            // tpConfig
            // 
            this.tpConfig.Controls.Add(this.btnOverlayText);
            this.tpConfig.Controls.Add(this.txtOverlay);
            this.tpConfig.Controls.Add(this.cbSlewOnTarget);
            this.tpConfig.Controls.Add(this.cbTextOverlay);
            this.tpConfig.Controls.Add(this.cbFOVICorr);
            this.tpConfig.Controls.Add(this.label2);
            this.tpConfig.Controls.Add(this.cbPlanetarium);
            this.tpConfig.Location = new System.Drawing.Point(4, 26);
            this.tpConfig.Name = "tpConfig";
            this.tpConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfig.Size = new System.Drawing.Size(369, 117);
            this.tpConfig.TabIndex = 2;
            this.tpConfig.Text = "Config";
            this.tpConfig.UseVisualStyleBackColor = true;
            // 
            // btnOverlayText
            // 
            this.btnOverlayText.Font = new System.Drawing.Font("Segoe UI Variable Small Semibol", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOverlayText.Location = new System.Drawing.Point(305, 59);
            this.btnOverlayText.Name = "btnOverlayText";
            this.btnOverlayText.Size = new System.Drawing.Size(56, 25);
            this.btnOverlayText.TabIndex = 27;
            this.btnOverlayText.Text = "Apply";
            this.btnOverlayText.UseVisualStyleBackColor = true;
            this.btnOverlayText.Click += new System.EventHandler(this.btnOverlayText_Click);
            // 
            // txtOverlay
            // 
            this.txtOverlay.Location = new System.Drawing.Point(8, 59);
            this.txtOverlay.Name = "txtOverlay";
            this.txtOverlay.Size = new System.Drawing.Size(291, 25);
            this.txtOverlay.TabIndex = 26;
            // 
            // cbSlewOnTarget
            // 
            this.cbSlewOnTarget.AutoSize = true;
            this.cbSlewOnTarget.Enabled = false;
            this.cbSlewOnTarget.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSlewOnTarget.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbSlewOnTarget.Location = new System.Drawing.Point(8, 91);
            this.cbSlewOnTarget.Name = "cbSlewOnTarget";
            this.cbSlewOnTarget.Size = new System.Drawing.Size(100, 19);
            this.cbSlewOnTarget.TabIndex = 25;
            this.cbSlewOnTarget.Text = "Slew on Target";
            this.cbSlewOnTarget.UseVisualStyleBackColor = true;
            // 
            // cbTextOverlay
            // 
            this.cbTextOverlay.AutoSize = true;
            this.cbTextOverlay.Checked = true;
            this.cbTextOverlay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTextOverlay.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTextOverlay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbTextOverlay.Location = new System.Drawing.Point(8, 40);
            this.cbTextOverlay.Name = "cbTextOverlay";
            this.cbTextOverlay.Size = new System.Drawing.Size(88, 19);
            this.cbTextOverlay.TabIndex = 24;
            this.cbTextOverlay.Text = "Text Overlay";
            this.cbTextOverlay.UseVisualStyleBackColor = true;
            this.cbTextOverlay.CheckedChanged += new System.EventHandler(this.cbTextOverlay_CheckedChanged);
            // 
            // cbFOVICorr
            // 
            this.cbFOVICorr.AutoSize = true;
            this.cbFOVICorr.Checked = true;
            this.cbFOVICorr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFOVICorr.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbFOVICorr.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbFOVICorr.Location = new System.Drawing.Point(215, 10);
            this.cbFOVICorr.Name = "cbFOVICorr";
            this.cbFOVICorr.Size = new System.Drawing.Size(114, 19);
            this.cbFOVICorr.TabIndex = 14;
            this.cbFOVICorr.Text = "TheSky FOVI Corr";
            this.cbFOVICorr.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Planetarium";
            // 
            // cbPlanetarium
            // 
            this.cbPlanetarium.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlanetarium.FormattingEnabled = true;
            this.cbPlanetarium.Items.AddRange(new object[] {
            "TheSky Pro",
            "Stellarium"});
            this.cbPlanetarium.Location = new System.Drawing.Point(88, 6);
            this.cbPlanetarium.Name = "cbPlanetarium";
            this.cbPlanetarium.Size = new System.Drawing.Size(121, 25);
            this.cbPlanetarium.TabIndex = 12;
            this.cbPlanetarium.SelectedIndexChanged += new System.EventHandler(this.cbPlanetarium_SelectedIndexChanged);
            // 
            // tpDev
            // 
            this.tpDev.Controls.Add(this.tbMessages);
            this.tpDev.Location = new System.Drawing.Point(4, 26);
            this.tpDev.Name = "tpDev";
            this.tpDev.Padding = new System.Windows.Forms.Padding(3);
            this.tpDev.Size = new System.Drawing.Size(369, 117);
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
            this.tbMessages.Size = new System.Drawing.Size(366, 107);
            this.tbMessages.TabIndex = 20;
            // 
            // btnAllCats
            // 
            this.btnAllCats.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAllCats.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAllCats.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAllCats.Font = new System.Drawing.Font("Segoe UI Variable Text Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllCats.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAllCats.Location = new System.Drawing.Point(68, 117);
            this.btnAllCats.Name = "btnAllCats";
            this.btnAllCats.Size = new System.Drawing.Size(82, 25);
            this.btnAllCats.TabIndex = 23;
            this.btnAllCats.Text = "Def Cats";
            this.btnAllCats.UseVisualStyleBackColor = false;
            this.btnAllCats.Click += new System.EventHandler(this.btnAllCats_Click);
            // 
            // frmEAACP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(579, 180);
            this.Controls.Add(this.btnAllCats);
            this.Controls.Add(this.tcExtra);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCaptureProfile);
            this.Controls.Add(this.txtStatusMsg);
            this.Controls.Add(this.btnExpand);
            this.Controls.Add(this.btnLogPlus);
            this.Controls.Add(this.cbLogFind);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.btnAzAltFOVI);
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
            this.Text = "EAACtrl";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEAACP_FormClosing);
            this.Load += new System.EventHandler(this.frmEAACP_Load);
            this.tcExtra.ResumeLayout(false);
            this.tpTools.ResumeLayout(false);
            this.grpbMBDS.ResumeLayout(false);
            this.grpbMBDS.PerformLayout();
            this.tpConfig.ResumeLayout(false);
            this.tpConfig.PerformLayout();
            this.tpDev.ResumeLayout(false);
            this.tpDev.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnTarget;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.CheckBox cbImagerZoom;
        private System.Windows.Forms.Button btnAzAltFOVI;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.CheckBox cbLogFind;
        private System.Windows.Forms.Button btnLogPlus;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.TextBox txtStatusMsg;
        private System.Windows.Forms.ComboBox cbCaptureProfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tcExtra;
        private System.Windows.Forms.TabPage tpTools;
        private System.Windows.Forms.TabPage tpDev;
        private System.Windows.Forms.TextBox tbMessages;
        private System.Windows.Forms.Button btnSCDSA;
        private System.Windows.Forms.Button btnAsteroidsFOV;
        private System.Windows.Forms.Button btnPlanetaryMoons;
        private System.Windows.Forms.GroupBox grpbMBDS;
        private System.Windows.Forms.RadioButton rbJPLHorizons;
        private System.Windows.Forms.RadioButton rbTheSky;
        private System.Windows.Forms.TabPage tpConfig;
        private System.Windows.Forms.CheckBox cbFOVICorr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbPlanetarium;
        private System.Windows.Forms.CheckBox cbSlewOnTarget;
        private System.Windows.Forms.CheckBox cbTextOverlay;
        private System.Windows.Forms.Button btnAllCats;
        private System.Windows.Forms.TextBox txtOverlay;
        private System.Windows.Forms.Button btnOverlayText;
    }
}

