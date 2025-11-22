namespace EAACtrl
{
    partial class SearchResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchResults));
            this.dgvSearchResults = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddToAP = new System.Windows.Forms.Button();
            this.btnDrawSelection = new System.Windows.Forms.Button();
            this.btnPlotAll = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnClearPlot = new System.Windows.Forms.Button();
            this.cbCataloguesFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCentreSelected = new System.Windows.Forms.Button();
            this.btnRecentre = new System.Windows.Forms.Button();
            this.btnDSOAll = new System.Windows.Forms.Button();
            this.btnDSOStandard = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDSOOff = new System.Windows.Forms.Button();
            this.SearchContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCentre = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCDSByName = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCDSByPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiShowStarSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWDSComponentsFiltered = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCopyRow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopyCell = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).BeginInit();
            this.SearchContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSearchResults
            // 
            this.dgvSearchResults.AllowUserToAddRows = false;
            this.dgvSearchResults.AllowUserToDeleteRows = false;
            this.dgvSearchResults.AllowUserToOrderColumns = true;
            this.dgvSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvSearchResults, "dgvSearchResults");
            this.dgvSearchResults.Name = "dgvSearchResults";
            this.dgvSearchResults.ReadOnly = true;
            this.dgvSearchResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchResults.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSearchResults_CellMouseDown);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAddToAP
            // 
            resources.ApplyResources(this.btnAddToAP, "btnAddToAP");
            this.btnAddToAP.Name = "btnAddToAP";
            this.btnAddToAP.UseVisualStyleBackColor = true;
            this.btnAddToAP.Click += new System.EventHandler(this.btnAddToAP_Click);
            // 
            // btnDrawSelection
            // 
            resources.ApplyResources(this.btnDrawSelection, "btnDrawSelection");
            this.btnDrawSelection.Name = "btnDrawSelection";
            this.btnDrawSelection.UseVisualStyleBackColor = true;
            this.btnDrawSelection.Click += new System.EventHandler(this.btnDrawSelection_Click);
            // 
            // btnPlotAll
            // 
            resources.ApplyResources(this.btnPlotAll, "btnPlotAll");
            this.btnPlotAll.Name = "btnPlotAll";
            this.btnPlotAll.UseVisualStyleBackColor = true;
            this.btnPlotAll.Click += new System.EventHandler(this.btnPlotAll_Click);
            // 
            // btnOptions
            // 
            resources.ApplyResources(this.btnOptions, "btnOptions");
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // btnClearPlot
            // 
            resources.ApplyResources(this.btnClearPlot, "btnClearPlot");
            this.btnClearPlot.Name = "btnClearPlot";
            this.btnClearPlot.UseVisualStyleBackColor = true;
            this.btnClearPlot.Click += new System.EventHandler(this.btnClearPlot_Click);
            // 
            // cbCataloguesFilter
            // 
            this.cbCataloguesFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCataloguesFilter.FormattingEnabled = true;
            resources.ApplyResources(this.cbCataloguesFilter, "cbCataloguesFilter");
            this.cbCataloguesFilter.Name = "cbCataloguesFilter";
            this.cbCataloguesFilter.SelectedIndexChanged += new System.EventHandler(this.cbCataloguesFilter_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnCentreSelected
            // 
            resources.ApplyResources(this.btnCentreSelected, "btnCentreSelected");
            this.btnCentreSelected.Name = "btnCentreSelected";
            this.btnCentreSelected.UseVisualStyleBackColor = true;
            this.btnCentreSelected.Click += new System.EventHandler(this.btnCentreSelected_Click);
            // 
            // btnRecentre
            // 
            resources.ApplyResources(this.btnRecentre, "btnRecentre");
            this.btnRecentre.Name = "btnRecentre";
            this.btnRecentre.UseVisualStyleBackColor = true;
            this.btnRecentre.Click += new System.EventHandler(this.btnRecentre_Click);
            // 
            // btnDSOAll
            // 
            resources.ApplyResources(this.btnDSOAll, "btnDSOAll");
            this.btnDSOAll.Name = "btnDSOAll";
            this.btnDSOAll.UseVisualStyleBackColor = true;
            this.btnDSOAll.Click += new System.EventHandler(this.btnDSOAll_Click);
            // 
            // btnDSOStandard
            // 
            resources.ApplyResources(this.btnDSOStandard, "btnDSOStandard");
            this.btnDSOStandard.Name = "btnDSOStandard";
            this.btnDSOStandard.UseVisualStyleBackColor = true;
            this.btnDSOStandard.Click += new System.EventHandler(this.btnDSOStandard_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnDSOOff
            // 
            resources.ApplyResources(this.btnDSOOff, "btnDSOOff");
            this.btnDSOOff.Name = "btnDSOOff";
            this.btnDSOOff.UseVisualStyleBackColor = true;
            this.btnDSOOff.Click += new System.EventHandler(this.btnDSOOff_Click);
            // 
            // SearchContextMenu
            // 
            this.SearchContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCentre,
            this.toolStripSeparator1,
            this.tsmiCDSByName,
            this.tsmiCDSByPosition,
            this.toolStripSeparator2,
            this.tsmiShowStarSystem,
            this.tsmiWDSComponentsFiltered,
            this.toolStripSeparator3,
            this.tsmiCopyRow,
            this.tsmiCopyCell});
            this.SearchContextMenu.Name = "SearchContextMenu";
            resources.ApplyResources(this.SearchContextMenu, "SearchContextMenu");
            this.SearchContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.SearchContextMenu_Opening);
            // 
            // tsmiCentre
            // 
            this.tsmiCentre.Name = "tsmiCentre";
            resources.ApplyResources(this.tsmiCentre, "tsmiCentre");
            this.tsmiCentre.Click += new System.EventHandler(this.tsmiCentre_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsmiCDSByName
            // 
            this.tsmiCDSByName.Name = "tsmiCDSByName";
            resources.ApplyResources(this.tsmiCDSByName, "tsmiCDSByName");
            this.tsmiCDSByName.Click += new System.EventHandler(this.tsmiCDSByName_Click);
            // 
            // tsmiCDSByPosition
            // 
            this.tsmiCDSByPosition.Name = "tsmiCDSByPosition";
            resources.ApplyResources(this.tsmiCDSByPosition, "tsmiCDSByPosition");
            this.tsmiCDSByPosition.Click += new System.EventHandler(this.tsmiCDSByPosition_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsmiShowStarSystem
            // 
            this.tsmiShowStarSystem.Name = "tsmiShowStarSystem";
            resources.ApplyResources(this.tsmiShowStarSystem, "tsmiShowStarSystem");
            this.tsmiShowStarSystem.Click += new System.EventHandler(this.tsmiShowStarSystem_Click);
            // 
            // tsmiWDSComponentsFiltered
            // 
            this.tsmiWDSComponentsFiltered.Name = "tsmiWDSComponentsFiltered";
            resources.ApplyResources(this.tsmiWDSComponentsFiltered, "tsmiWDSComponentsFiltered");
            this.tsmiWDSComponentsFiltered.Click += new System.EventHandler(this.tsmiWDSComponentsFiltered_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tsmiCopyRow
            // 
            this.tsmiCopyRow.Name = "tsmiCopyRow";
            resources.ApplyResources(this.tsmiCopyRow, "tsmiCopyRow");
            this.tsmiCopyRow.Click += new System.EventHandler(this.tsmiCopyRow_Click);
            // 
            // tsmiCopyCell
            // 
            this.tsmiCopyCell.Name = "tsmiCopyCell";
            resources.ApplyResources(this.tsmiCopyCell, "tsmiCopyCell");
            this.tsmiCopyCell.Click += new System.EventHandler(this.tsmiCopyCell_Click);
            // 
            // SearchResults
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDSOAll);
            this.Controls.Add(this.btnDSOStandard);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDSOOff);
            this.Controls.Add(this.btnClearPlot);
            this.Controls.Add(this.cbCataloguesFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCentreSelected);
            this.Controls.Add(this.btnRecentre);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.btnPlotAll);
            this.Controls.Add(this.btnDrawSelection);
            this.Controls.Add(this.btnAddToAP);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvSearchResults);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchResults";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchResults_FormClosing);
            this.Load += new System.EventHandler(this.SearchResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).EndInit();
            this.SearchContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSearchResults;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAddToAP;
        private System.Windows.Forms.Button btnDrawSelection;
        private System.Windows.Forms.Button btnPlotAll;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Button btnClearPlot;
        private System.Windows.Forms.ComboBox cbCataloguesFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCentreSelected;
        private System.Windows.Forms.Button btnRecentre;
        private System.Windows.Forms.Button btnDSOAll;
        private System.Windows.Forms.Button btnDSOStandard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDSOOff;
        private System.Windows.Forms.ContextMenuStrip SearchContextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiCentre;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCDSByName;
        private System.Windows.Forms.ToolStripMenuItem tsmiCDSByPosition;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowStarSystem;
        private System.Windows.Forms.ToolStripMenuItem tsmiWDSComponentsFiltered;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyRow;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyCell;
    }
}