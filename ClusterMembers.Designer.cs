namespace EAACtrl
{
    partial class ClusterMembers
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
            this.components = new System.ComponentModel.Container();
            this.dgvSearchResults = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMemberCount = new System.Windows.Forms.Label();
            this.chkMarkersOnly = new System.Windows.Forms.CheckBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.txtMagnitude = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProbability = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblProperMotionDEC = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblProperMotionRA = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblRadius = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtType = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDistance = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStarCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNames = new System.Windows.Forms.TextBox();
            this.lblCapNames = new System.Windows.Forms.Label();
            this.btnSharpCapDSA = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SearchContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCentre = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCDSByName = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCDSByPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCopyRow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopyCell = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClearPlot = new System.Windows.Forms.Button();
            this.btnAPFront = new System.Windows.Forms.Button();
            this.btnPlFront = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnFilterReset = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFilterByID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SearchContextMenu.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSearchResults
            // 
            this.dgvSearchResults.AllowUserToAddRows = false;
            this.dgvSearchResults.AllowUserToDeleteRows = false;
            this.dgvSearchResults.AllowUserToOrderColumns = true;
            this.dgvSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchResults.Location = new System.Drawing.Point(12, 184);
            this.dgvSearchResults.Name = "dgvSearchResults";
            this.dgvSearchResults.ReadOnly = true;
            this.dgvSearchResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchResults.Size = new System.Drawing.Size(999, 319);
            this.dgvSearchResults.TabIndex = 1;
            this.dgvSearchResults.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSearchResults_CellMouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMemberCount);
            this.groupBox1.Controls.Add(this.chkMarkersOnly);
            this.groupBox1.Controls.Add(this.btnShow);
            this.groupBox1.Controls.Add(this.txtMagnitude);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtProbability);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 115);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 58);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Member Filtering";
            // 
            // lblMemberCount
            // 
            this.lblMemberCount.AutoSize = true;
            this.lblMemberCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMemberCount.Location = new System.Drawing.Point(440, 26);
            this.lblMemberCount.Name = "lblMemberCount";
            this.lblMemberCount.Size = new System.Drawing.Size(103, 13);
            this.lblMemberCount.TabIndex = 36;
            this.lblMemberCount.Text = "0 members found";
            // 
            // chkMarkersOnly
            // 
            this.chkMarkersOnly.AutoSize = true;
            this.chkMarkersOnly.Location = new System.Drawing.Point(246, 25);
            this.chkMarkersOnly.Name = "chkMarkersOnly";
            this.chkMarkersOnly.Size = new System.Drawing.Size(88, 17);
            this.chkMarkersOnly.TabIndex = 35;
            this.chkMarkersOnly.Text = "Markers Only";
            this.chkMarkersOnly.UseVisualStyleBackColor = true;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(344, 21);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 34;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtMagnitude
            // 
            this.txtMagnitude.Location = new System.Drawing.Point(193, 23);
            this.txtMagnitude.Name = "txtMagnitude";
            this.txtMagnitude.Size = new System.Drawing.Size(35, 20);
            this.txtMagnitude.TabIndex = 33;
            this.txtMagnitude.Text = "20";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(121, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Magnitude <";
            // 
            // txtProbability
            // 
            this.txtProbability.Location = new System.Drawing.Point(80, 24);
            this.txtProbability.Name = "txtProbability";
            this.txtProbability.Size = new System.Drawing.Size(35, 20);
            this.txtProbability.TabIndex = 31;
            this.txtProbability.Text = "70";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Probability %:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblProperMotionDEC);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lblProperMotionRA);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lblRadius);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtType);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lblDistance);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblStarCount);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtNames);
            this.groupBox2.Controls.Add(this.lblCapNames);
            this.groupBox2.Location = new System.Drawing.Point(12, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(997, 99);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cluster Information";
            // 
            // lblProperMotionDEC
            // 
            this.lblProperMotionDEC.AutoSize = true;
            this.lblProperMotionDEC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProperMotionDEC.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProperMotionDEC.Location = new System.Drawing.Point(365, 68);
            this.lblProperMotionDEC.Name = "lblProperMotionDEC";
            this.lblProperMotionDEC.Size = new System.Drawing.Size(60, 13);
            this.lblProperMotionDEC.TabIndex = 38;
            this.lblProperMotionDEC.Text = "Unknown";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(261, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Proper Motion DEC:";
            // 
            // lblProperMotionRA
            // 
            this.lblProperMotionRA.AutoSize = true;
            this.lblProperMotionRA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProperMotionRA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProperMotionRA.Location = new System.Drawing.Point(111, 68);
            this.lblProperMotionRA.Name = "lblProperMotionRA";
            this.lblProperMotionRA.Size = new System.Drawing.Size(60, 13);
            this.lblProperMotionRA.TabIndex = 36;
            this.lblProperMotionRA.Text = "Unknown";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(7, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Proper Motion RA:";
            // 
            // lblRadius
            // 
            this.lblRadius.AutoSize = true;
            this.lblRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRadius.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblRadius.Location = new System.Drawing.Point(628, 40);
            this.lblRadius.Name = "lblRadius";
            this.lblRadius.Size = new System.Drawing.Size(60, 13);
            this.lblRadius.TabIndex = 34;
            this.lblRadius.Text = "Unknown";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(499, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Radius (50th Percentile):";
            // 
            // txtType
            // 
            this.txtType.AutoSize = true;
            this.txtType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtType.Location = new System.Drawing.Point(50, 40);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(60, 13);
            this.txtType.TabIndex = 32;
            this.txtType.Text = "Unknown";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(7, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Type:";
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDistance.Location = new System.Drawing.Point(425, 40);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(60, 13);
            this.lblDistance.TabIndex = 30;
            this.lblDistance.Text = "Unknown";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(291, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Distance (50th Percentile):";
            // 
            // lblStarCount
            // 
            this.lblStarCount.AutoSize = true;
            this.lblStarCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStarCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblStarCount.Location = new System.Drawing.Point(230, 40);
            this.lblStarCount.Name = "lblStarCount";
            this.lblStarCount.Size = new System.Drawing.Size(60, 13);
            this.lblStarCount.TabIndex = 28;
            this.lblStarCount.Text = "Unknown";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(169, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Star Count:";
            // 
            // txtNames
            // 
            this.txtNames.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNames.Location = new System.Drawing.Point(55, 20);
            this.txtNames.Name = "txtNames";
            this.txtNames.ReadOnly = true;
            this.txtNames.Size = new System.Drawing.Size(936, 13);
            this.txtNames.TabIndex = 26;
            // 
            // lblCapNames
            // 
            this.lblCapNames.AutoSize = true;
            this.lblCapNames.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCapNames.Location = new System.Drawing.Point(6, 20);
            this.lblCapNames.Name = "lblCapNames";
            this.lblCapNames.Size = new System.Drawing.Size(43, 13);
            this.lblCapNames.TabIndex = 25;
            this.lblCapNames.Text = "Names:";
            // 
            // btnSharpCapDSA
            // 
            this.btnSharpCapDSA.Location = new System.Drawing.Point(12, 512);
            this.btnSharpCapDSA.Name = "btnSharpCapDSA";
            this.btnSharpCapDSA.Size = new System.Drawing.Size(101, 23);
            this.btnSharpCapDSA.TabIndex = 35;
            this.btnSharpCapDSA.Text = "SharpCap DSA";
            this.btnSharpCapDSA.UseVisualStyleBackColor = true;
            this.btnSharpCapDSA.Click += new System.EventHandler(this.btnSharpCapDSA_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(936, 512);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 36;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SearchContextMenu
            // 
            this.SearchContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCentre,
            this.toolStripSeparator1,
            this.tsmiCDSByName,
            this.tsmiCDSByPosition,
            this.toolStripSeparator2,
            this.tsmiCopyRow,
            this.tsmiCopyCell});
            this.SearchContextMenu.Name = "SearchContextMenu";
            this.SearchContextMenu.Size = new System.Drawing.Size(159, 126);
            this.SearchContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.SearchContextMenu_Opening);
            // 
            // tsmiCentre
            // 
            this.tsmiCentre.Name = "tsmiCentre";
            this.tsmiCentre.Size = new System.Drawing.Size(158, 22);
            this.tsmiCentre.Text = "Centre";
            this.tsmiCentre.Click += new System.EventHandler(this.tsmiCentre_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(155, 6);
            // 
            // tsmiCDSByName
            // 
            this.tsmiCDSByName.Name = "tsmiCDSByName";
            this.tsmiCDSByName.Size = new System.Drawing.Size(158, 22);
            this.tsmiCDSByName.Text = "CDS by Name";
            this.tsmiCDSByName.Click += new System.EventHandler(this.tsmiCDSByName_Click);
            // 
            // tsmiCDSByPosition
            // 
            this.tsmiCDSByPosition.Name = "tsmiCDSByPosition";
            this.tsmiCDSByPosition.Size = new System.Drawing.Size(158, 22);
            this.tsmiCDSByPosition.Text = "CDS by Position";
            this.tsmiCDSByPosition.Click += new System.EventHandler(this.tsmiCDSByPosition_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(155, 6);
            // 
            // tsmiCopyRow
            // 
            this.tsmiCopyRow.Name = "tsmiCopyRow";
            this.tsmiCopyRow.Size = new System.Drawing.Size(158, 22);
            this.tsmiCopyRow.Text = "Copy Rows";
            this.tsmiCopyRow.Click += new System.EventHandler(this.tsmiCopyRow_Click);
            // 
            // tsmiCopyCell
            // 
            this.tsmiCopyCell.Name = "tsmiCopyCell";
            this.tsmiCopyCell.Size = new System.Drawing.Size(158, 22);
            this.tsmiCopyCell.Text = "Copy Cell";
            this.tsmiCopyCell.Click += new System.EventHandler(this.tsmiCopyCell_Click);
            // 
            // btnClearPlot
            // 
            this.btnClearPlot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClearPlot.Location = new System.Drawing.Point(126, 512);
            this.btnClearPlot.Name = "btnClearPlot";
            this.btnClearPlot.Size = new System.Drawing.Size(75, 23);
            this.btnClearPlot.TabIndex = 37;
            this.btnClearPlot.Text = "Clear Plot";
            this.btnClearPlot.UseVisualStyleBackColor = true;
            this.btnClearPlot.Click += new System.EventHandler(this.btnClearPlot_Click);
            // 
            // btnAPFront
            // 
            this.btnAPFront.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnAPFront.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAPFront.Location = new System.Drawing.Point(213, 512);
            this.btnAPFront.Name = "btnAPFront";
            this.btnAPFront.Size = new System.Drawing.Size(41, 23);
            this.btnAPFront.TabIndex = 54;
            this.btnAPFront.Text = "AP";
            this.btnAPFront.UseVisualStyleBackColor = false;
            this.btnAPFront.Click += new System.EventHandler(this.btnAPFront_Click);
            // 
            // btnPlFront
            // 
            this.btnPlFront.BackColor = System.Drawing.Color.Silver;
            this.btnPlFront.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPlFront.Location = new System.Drawing.Point(259, 512);
            this.btnPlFront.Name = "btnPlFront";
            this.btnPlFront.Size = new System.Drawing.Size(41, 23);
            this.btnPlFront.TabIndex = 53;
            this.btnPlFront.Text = "Pl";
            this.btnPlFront.UseVisualStyleBackColor = false;
            this.btnPlFront.Click += new System.EventHandler(this.btnPlFront_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnFilterReset);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtFilterByID);
            this.groupBox3.Location = new System.Drawing.Point(588, 115);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(421, 58);
            this.groupBox3.TabIndex = 59;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filter";
            // 
            // btnFilterReset
            // 
            this.btnFilterReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFilterReset.Location = new System.Drawing.Point(323, 18);
            this.btnFilterReset.Name = "btnFilterReset";
            this.btnFilterReset.Size = new System.Drawing.Size(46, 23);
            this.btnFilterReset.TabIndex = 61;
            this.btnFilterReset.Text = "Reset";
            this.btnFilterReset.UseVisualStyleBackColor = true;
            this.btnFilterReset.Click += new System.EventHandler(this.btnFilterReset_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(7, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "Filter by Gaia ID";
            // 
            // txtFilterByID
            // 
            this.txtFilterByID.Location = new System.Drawing.Point(96, 20);
            this.txtFilterByID.Name = "txtFilterByID";
            this.txtFilterByID.Size = new System.Drawing.Size(223, 20);
            this.txtFilterByID.TabIndex = 59;
            this.txtFilterByID.TextChanged += new System.EventHandler(this.txtFilterByID_TextChanged);
            // 
            // ClusterMembers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 542);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnAPFront);
            this.Controls.Add(this.btnPlFront);
            this.Controls.Add(this.btnClearPlot);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSharpCapDSA);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvSearchResults);
            this.Controls.Add(this.groupBox2);
            this.Name = "ClusterMembers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open Cluster Census III - Members";
            this.Load += new System.EventHandler(this.ClusterMembers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.SearchContextMenu.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSearchResults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.TextBox txtMagnitude;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProbability;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStarCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNames;
        private System.Windows.Forms.Label lblCapNames;
        private System.Windows.Forms.Button btnSharpCapDSA;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label txtType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblRadius;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblProperMotionDEC;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblProperMotionRA;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkMarkersOnly;
        private System.Windows.Forms.Label lblMemberCount;
        private System.Windows.Forms.ContextMenuStrip SearchContextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiCentre;
        private System.Windows.Forms.ToolStripMenuItem tsmiCDSByName;
        private System.Windows.Forms.ToolStripMenuItem tsmiCDSByPosition;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyCell;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnClearPlot;
        private System.Windows.Forms.Button btnAPFront;
        private System.Windows.Forms.Button btnPlFront;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnFilterReset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFilterByID;
    }
}