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
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).BeginInit();
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
    }
}