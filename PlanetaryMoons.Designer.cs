namespace EAACtrl
{
    partial class PlanetaryMoons
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
            this.dgvMoons = new System.Windows.Forms.DataGridView();
            this.btnAddToAP = new System.Windows.Forms.Button();
            this.cbDSAFormat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSharpCapDSA = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStellariumPlot = new System.Windows.Forms.Button();
            this.btnStellariumClear = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTimeStamp = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMoons)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMoons
            // 
            this.dgvMoons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMoons.Location = new System.Drawing.Point(12, 12);
            this.dgvMoons.Name = "dgvMoons";
            this.dgvMoons.Size = new System.Drawing.Size(776, 366);
            this.dgvMoons.TabIndex = 0;
            // 
            // btnAddToAP
            // 
            this.btnAddToAP.Enabled = false;
            this.btnAddToAP.Location = new System.Drawing.Point(525, 384);
            this.btnAddToAP.Name = "btnAddToAP";
            this.btnAddToAP.Size = new System.Drawing.Size(147, 23);
            this.btnAddToAP.TabIndex = 1;
            this.btnAddToAP.Text = "Add/Update AstroPlanner";
            this.btnAddToAP.UseVisualStyleBackColor = true;
            // 
            // cbDSAFormat
            // 
            this.cbDSAFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDSAFormat.FormattingEnabled = true;
            this.cbDSAFormat.Items.AddRange(new object[] {
            "Minimal Information",
            "Full Information"});
            this.cbDSAFormat.Location = new System.Drawing.Point(292, 384);
            this.cbDSAFormat.Name = "cbDSAFormat";
            this.cbDSAFormat.Size = new System.Drawing.Size(121, 21);
            this.cbDSAFormat.TabIndex = 2;
            this.cbDSAFormat.SelectedIndexChanged += new System.EventHandler(this.cbDSAFormat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 387);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "DSA Format";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(710, 419);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSharpCapDSA
            // 
            this.btnSharpCapDSA.Location = new System.Drawing.Point(418, 383);
            this.btnSharpCapDSA.Name = "btnSharpCapDSA";
            this.btnSharpCapDSA.Size = new System.Drawing.Size(101, 23);
            this.btnSharpCapDSA.TabIndex = 5;
            this.btnSharpCapDSA.Text = "SharpCap DSA";
            this.btnSharpCapDSA.UseVisualStyleBackColor = true;
            this.btnSharpCapDSA.Click += new System.EventHandler(this.btnSharpCapDSA_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(630, 419);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnStellariumPlot
            // 
            this.btnStellariumPlot.Location = new System.Drawing.Point(12, 419);
            this.btnStellariumPlot.Name = "btnStellariumPlot";
            this.btnStellariumPlot.Size = new System.Drawing.Size(97, 23);
            this.btnStellariumPlot.TabIndex = 7;
            this.btnStellariumPlot.Text = "Stellarium Plot";
            this.btnStellariumPlot.UseVisualStyleBackColor = true;
            this.btnStellariumPlot.Click += new System.EventHandler(this.btnStellariumPlot_Click);
            // 
            // btnStellariumClear
            // 
            this.btnStellariumClear.Location = new System.Drawing.Point(206, 418);
            this.btnStellariumClear.Name = "btnStellariumClear";
            this.btnStellariumClear.Size = new System.Drawing.Size(97, 23);
            this.btnStellariumClear.TabIndex = 8;
            this.btnStellariumClear.Text = "Stellarium Clear";
            this.btnStellariumClear.UseVisualStyleBackColor = true;
            this.btnStellariumClear.Click += new System.EventHandler(this.btnStellariumClear_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Current Time (PC)",
            "Stellarium Time"});
            this.comboBox1.Location = new System.Drawing.Point(81, 384);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 387);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Time Source";
            // 
            // chkTimeStamp
            // 
            this.chkTimeStamp.AutoSize = true;
            this.chkTimeStamp.Checked = true;
            this.chkTimeStamp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTimeStamp.Location = new System.Drawing.Point(120, 422);
            this.chkTimeStamp.Name = "chkTimeStamp";
            this.chkTimeStamp.Size = new System.Drawing.Size(80, 17);
            this.chkTimeStamp.TabIndex = 11;
            this.chkTimeStamp.Text = "Time stamp";
            this.chkTimeStamp.UseVisualStyleBackColor = true;
            // 
            // PlanetaryMoons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chkTimeStamp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnStellariumClear);
            this.Controls.Add(this.btnStellariumPlot);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSharpCapDSA);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDSAFormat);
            this.Controls.Add(this.btnAddToAP);
            this.Controls.Add(this.dgvMoons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlanetaryMoons";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Planetary Moons";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMoons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMoons;
        private System.Windows.Forms.Button btnAddToAP;
        private System.Windows.Forms.ComboBox cbDSAFormat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSharpCapDSA;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnStellariumPlot;
        private System.Windows.Forms.Button btnStellariumClear;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTimeStamp;
    }
}