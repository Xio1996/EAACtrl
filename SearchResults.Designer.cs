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
            // SearchResults
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Load += new System.EventHandler(this.SearchResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSearchResults;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAddToAP;
        private System.Windows.Forms.Button btnDrawSelection;
        private System.Windows.Forms.Button btnPlotAll;
        private System.Windows.Forms.Button btnOptions;
    }
}