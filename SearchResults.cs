using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EAACtrl
{
    public partial class SearchResults : Form
    {
        private Stellarium Stellarium = new Stellarium();
        private List<string[]> ResultsList = null;
        private DataTable ResultsTable = null;
        DataTable dt = new DataTable();
        DataTable dtUniqueCatalogues = new DataTable();
        int totalResults = 0;
        string stellariumDSOFilter;
        string displayMode = "All";

        public frmEAACP EAACP;

        public SearchResults()
        {
            InitializeComponent();
        }

        public List<string[]> Results 
        { 
            set
            {
                ResultsList = value;
            }
        }

        public DataTable ResultsDataTable
        {
            set
            {
                ResultsTable = value;
            }
        }

        public double CentreRA;
        public double CentreDec;
        public string CentreID;

        private void SearchResults_Load(object sender, EventArgs e)
        {
            Stellarium.ScriptFolder = Properties.Settings.Default.StScriptFolder;

            stellariumDSOFilter = Stellarium.GetStelProperty("NebulaMgr.catalogFilters");

            if (ResultsList != null || ResultsTable != null)
            {
                
                dt.Columns.Add("ID");
                dt.Columns.Add("Names");
                dt.Columns.Add("Type");
                dt.Columns.Add("Mag", typeof(double));
                dt.Columns.Add("Mag2", typeof(double));
                dt.Columns.Add("Dist Mpc", typeof(double));
                dt.Columns.Add("Galaxy Type");
                dt.Columns.Add("Size");
                dt.Columns.Add("Comp");
                dt.Columns.Add("PA", typeof(double));
                dt.Columns.Add("Sep", typeof(double));
                dt.Columns.Add("RA");
                dt.Columns.Add("Dec");
                dt.Columns.Add("dRA");
                dt.Columns.Add("dDec");
                dt.Columns.Add("Const");
                dt.Columns.Add("Catalogue");
                
                if (ResultsTable != null)
                {
                    dt = ResultsTable;
                }
                else
                {
                    foreach (string[] APObject in ResultsList)
                    {
                        DataRow row = dt.NewRow();
                        row["ID"] = APObject[0];
                        row["Names"] = APObject[1];
                        row["Type"] = APObject[2];

                        if (double.TryParse(APObject[3], out double Mag))
                        {
                            row["Mag"] = Mag;
                        }
                        else row["Mag"] = DBNull.Value;

                        if (double.TryParse(APObject[5], out double Dist))
                        {
                            row["Dist Mpc"] = Math.Round(Dist, 2);
                        }
                        else row["Dist Mpc"] = DBNull.Value;

                        row["Galaxy Type"] = APObject[4];
                        row["Catalogue"] = APObject[6];
                        row["RA"] = APObject[7];
                        row["Dec"] = APObject[8];
                        row["dRA"] = APObject[9];
                        row["dDec"] = APObject[10];
                        row["Size"] = APObject[11];
                        row["Const"] = APObject[13];

                        if (double.TryParse(APObject[12], out double PA))
                        {
                            if (PA == -999) row["PA"] = DBNull.Value;
                            else
                                row["PA"] = Math.Round(PA, 2);
                        }
                        else row["PA"] = DBNull.Value;

                        row["Const"] = APObject[13];
                        row["Comp"] = APObject[14];

                        if (double.TryParse(APObject[15], out double Sep))
                        {
                            if (Sep == -999) row["Sep"] = DBNull.Value;
                            else
                                row["Sep"] = Math.Round(Sep, 2);
                        }
                        else row["Sep"] = DBNull.Value;

                        if (double.TryParse(APObject[16], out double Mag2))
                        {
                            if (Mag2 == 999) row["Mag2"] = DBNull.Value;
                            else
                                row["Mag2"] = Math.Round(Mag2,2);
                        }
                        else row["Mag2"] = DBNull.Value;

                        dt.Rows.Add(row);

                    }
                }

                // Fetch and display list of unique catalogues
                cbCataloguesFilter.SelectedIndexChanged -= cbCataloguesFilter_SelectedIndexChanged;

                dtUniqueCatalogues = Stellarium.UniqueCataloguesInSearchResults(dt);
                cbCataloguesFilter.DataSource = dtUniqueCatalogues;
                cbCataloguesFilter.DisplayMember = "Catalogue";

                cbCataloguesFilter.SelectedIndexChanged += cbCataloguesFilter_SelectedIndexChanged;


                dgvSearchResults.DataSource = dt;
                dgvSearchResults.Columns["dRA"].Visible = false;
                dgvSearchResults.Columns["dDec"].Visible = false;
                dgvSearchResults.Columns["Size"].Visible = true;
                dgvSearchResults.Columns["PA"].Visible = true;
                dgvSearchResults.Columns["Const"].Visible = true;

                dgvSearchResults.Sort(dgvSearchResults.Columns["Mag"], System.ComponentModel.ListSortDirection.Ascending);

                // Removes first selection columnm
                dgvSearchResults.RowHeadersVisible = false;

                // Freeze the important columns
                dgvSearchResults.Columns["ID"].Frozen = true;
                dgvSearchResults.Columns["Names"].Frozen = true;
                dgvSearchResults.Columns["Type"].Frozen = true;
                dgvSearchResults.Columns["Mag"].Frozen = true;

                // Resize to accomodate content
                dgvSearchResults.AutoResizeColumns();

                // Set the colour of the important columns
                dgvSearchResults.Columns["ID"].DefaultCellStyle.BackColor = Color.LightBlue;
                dgvSearchResults.Columns["Names"].DefaultCellStyle.BackColor = Color.LightBlue;
                dgvSearchResults.Columns["Type"].DefaultCellStyle.BackColor = Color.LightBlue;
                dgvSearchResults.Columns["Mag"].DefaultCellStyle.BackColor = Color.LightBlue;

                dgvSearchResults.AllowUserToResizeRows = false;
                dgvSearchResults.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                dgvSearchResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                totalResults = dt.Rows.Count;
                UpdateSearchInfo(totalResults);
            }
        }

        private void UpdateSearchInfo(int viewCount)
        {
            this.Text = $"Search Results: {totalResults} objects, Current View: {viewCount} objects";
        }

        private void DrawSelectedObjects()
        {
            DataTable Selected = new DataTable();

            Selected.Columns.Add("ID");
            Selected.Columns.Add("Names");
            Selected.Columns.Add("Type");
            Selected.Columns.Add("Mag", typeof(double));
            Selected.Columns.Add("Mag2", typeof(double));
            Selected.Columns.Add("Dist Mpc", typeof(double));
            Selected.Columns.Add("Galaxy Type");
            Selected.Columns.Add("Size");
            Selected.Columns.Add("Comp");
            Selected.Columns.Add("PA", typeof(double));
            Selected.Columns.Add("Sep", typeof(double));
            Selected.Columns.Add("RA");
            Selected.Columns.Add("Dec");
            Selected.Columns.Add("dRA");
            Selected.Columns.Add("dDec");
            Selected.Columns.Add("Const");
            Selected.Columns.Add("Catalogue");


            foreach (DataGridViewRow row in dgvSearchResults.SelectedRows)
            {
                DataRow SelectedRow = Selected.NewRow();
                SelectedRow["ID"] = row.Cells["ID"].Value;
                SelectedRow["Names"] = row.Cells["Names"].Value;
                SelectedRow["Type"] = row.Cells["Type"].Value;

                if (double.TryParse(row.Cells["Mag"].Value.ToString(), out double Mag))
                {
                    SelectedRow["Mag"] = Mag;
                }
                else SelectedRow["Mag"] = DBNull.Value;

                if (double.TryParse(row.Cells["Dist Mpc"].Value.ToString(), out double Dist))
                {
                    SelectedRow["Dist Mpc"] = Math.Round(Dist, 2);
                }
                else SelectedRow["Dist Mpc"] = DBNull.Value;

                if (double.TryParse(row.Cells["Mag2"].Value.ToString(), out double Mag2))
                {
                    SelectedRow["Mag2"] = Mag2;
                }
                else SelectedRow["Mag2"] = DBNull.Value;

                if (double.TryParse(row.Cells["PA"].Value.ToString(), out double PA))
                {
                    if (PA == -999) row.Cells["PA"].Value = DBNull.Value;
                    else
                        row.Cells["PA"].Value = Math.Round(PA, 2);
                }
                else row.Cells["PA"].Value = DBNull.Value;

                if (double.TryParse(row.Cells["Sep"].Value.ToString(), out double Sep))
                {
                    if (Sep == -999) row.Cells["Sep"].Value = DBNull.Value;
                    else
                        row.Cells["Sep"].Value = Math.Round(Sep, 2);
                }
                else row.Cells["Sep"].Value = DBNull.Value;


                SelectedRow["Galaxy Type"] = row.Cells["Galaxy Type"].Value;
                SelectedRow["Catalogue"] = row.Cells["Catalogue"].Value;
                SelectedRow["RA"] = row.Cells["RA"].Value;
                SelectedRow["Dec"] = row.Cells["Dec"].Value;
                SelectedRow["dRA"] = row.Cells["dRA"].Value;
                SelectedRow["dDec"] = row.Cells["dDec"].Value;
                SelectedRow["Size"] = row.Cells["Size"].Value;
                SelectedRow["PA"] = row.Cells["PA"].Value;
                SelectedRow["Const"] = row.Cells["Const"].Value;
                SelectedRow["Comp"] = row.Cells["Comp"].Value;

                Selected.Rows.Add(SelectedRow);
            }

            Stellarium.DrawObjects(Selected);

            UpdateSearchInfo(dgvSearchResults.SelectedRows.Count);
        }

        private void btnDrawSelection_Click(object sender, EventArgs e)
        {
            DrawSelectedObjects();
        }

        private void ResetCatalogueFilter()
        {
            cbCataloguesFilter.SelectedIndexChanged -= cbCataloguesFilter_SelectedIndexChanged;
            cbCataloguesFilter.SelectedIndex = 0;
            cbCataloguesFilter.SelectedIndexChanged += cbCataloguesFilter_SelectedIndexChanged;
        }

        private void btnPlotAll_Click(object sender, EventArgs e)
        {
            displayMode = "All";
            // Stop the catalogues filter from firing
            ResetCatalogueFilter();
            
            // Remove any filtering
            dt.DefaultView.RowFilter = "";
            Stellarium.DrawObjects(dt);

            UpdateSearchInfo(totalResults);
        }

        private void btnAddToAP_Click(object sender, EventArgs e)
        {
            List < APCmdObject > apObjects = new List < APCmdObject >();
            
            DialogResult result = MessageBox.Show("Adding many objects to AstroPlanner, can take sometime and cannot be cancelled. Do you wish to continue?", "Add Objects to AstroPlanner", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                return;
            }

            if (dgvSearchResults.SelectedRows.Count == 0)
            {
                EAACP.Speak("No objects selected");
                return;
            }

            foreach (DataGridViewRow row in dgvSearchResults.SelectedRows)
            {
                APCmdObject obj = new APCmdObject();
                obj.ID = row.Cells["ID"].Value.ToString();
                obj.Name = row.Cells["Names"].Value.ToString();
                obj.Type = row.Cells["Type"].Value.ToString();
                obj.RA2000 = double.Parse(row.Cells["dRA"].Value.ToString());
                obj.Dec2000 = double.Parse(row.Cells["dDec"].Value.ToString());
                obj.Catalogue = row.Cells["Catalogue"].Value.ToString();
                obj.Distance = row.Cells["Dist Mpc"].Value.ToString();
                obj.GalaxyType = row.Cells["Galaxy Type"].Value.ToString();
                obj.Size = row.Cells["Size"].Value.ToString();
                obj.Constellation = row.Cells["Const"].Value.ToString();

                if (double.TryParse(row.Cells["Mag"].Value.ToString(), out double Mag))
                {
                    obj.Magnitude = Mag;
                }

                if (double.TryParse(row.Cells["Mag2"].Value.ToString(), out double Mag2))
                {
                    obj.Magnitude2 = Mag2;
                }

                if (double.TryParse(row.Cells["PA"].Value.ToString(), out double PA))
                {
                    obj.PosAngle = PA;
                }

                if (double.TryParse(row.Cells["Sep"].Value.ToString(), out double Sep))
                {
                    obj.Separation = Sep;
                }

                apObjects.Add(obj);
            }

            APPutCmd aPPutCmd = new APPutCmd();
            aPPutCmd.script = "EAAControl2";
            aPPutCmd.parameters = new APPutCmdParams();
            aPPutCmd.parameters.Cmd = 2;
            aPPutCmd.parameters.Option = 1;
            aPPutCmd.parameters.Objects = apObjects;

            string sOut = EAACP.APExecuteScript(Uri.EscapeDataString(JsonSerializer.Serialize<APPutCmd>(aPPutCmd)));

            EAACP.Speak(dgvSearchResults.SelectedRows.Count.ToString() + " Objects added");
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            using (StelFOVOptions frmOpt = new StelFOVOptions())
            {
                frmOpt.Mode = 1;
                frmOpt.TopMost = true;
                if (frmOpt.ShowDialog() == DialogResult.OK)
                {
                    // If the user hs changed the display attributes then update the current plot selection on ext.
                    switch (displayMode)
                    {
                        case "All":
                            Stellarium.DrawObjects(dt);
                            break;
                        case "Filtered":
                            DrawCatalogueFiltered();
                            break;
                        case "Selected":
                            DrawSelectedObjects();
                            break;
                    }
                }
            }
        }
        private void cbCataloguesFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawCatalogueFiltered();
        }

        private void btnRecentre_Click(object sender, EventArgs e)
        {
            Stellarium.SyncStellariumToAPObject(CentreID, CentreRA.ToString(), CentreDec.ToString(), "");
        }
        private void CentreSelected()
        {
            if (dgvSearchResults.SelectedRows.Count > 0)
            {
                double RA = double.Parse(dgvSearchResults.SelectedRows[0].Cells["dRA"].Value.ToString());
                double Dec = double.Parse(dgvSearchResults.SelectedRows[0].Cells["dDec"].Value.ToString());
                Stellarium.SyncStellariumToPosition(RA, Dec);
            }
        }
        private void btnCentreSelected_Click(object sender, EventArgs e)
        {
            CentreSelected();
        }
        private void DrawCatalogueFiltered()
        {
            displayMode = "Filtered";

            string apCatalogue = cbCataloguesFilter.Text;
            if (apCatalogue == "All Catalogues")
            {
                dt.DefaultView.RowFilter = "";
                Stellarium.DrawObjects(dt);
                UpdateSearchInfo(totalResults);
            }
            else
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = "Catalogue = '" + apCatalogue + "'";

                Stellarium.DrawObjects(dv.ToTable());

                UpdateSearchInfo(dv.ToTable().Rows.Count);
            }
        }

        private void btnDSOOff_Click(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "0");
        }

        private void btnDSOStandard_Click(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "7");
        }

        private void btnDSOAll_Click(object sender, EventArgs e)
        {
            Stellarium.SetStelProperty("NebulaMgr.catalogFilters", "255852279");
        }

        private void btnClearPlot_Click(object sender, EventArgs e)
        {
            displayMode = "All";
            Stellarium.ClearObjects();

            // Stop the catalogues filter from firing
            ResetCatalogueFilter();

            UpdateSearchInfo(0);
        }

        private void SearchResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stellarium.SetStelProperty("NebulaMgr.catalogFilters", stellariumDSOFilter);
        }

        private void OpenUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open browser: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string FormatCDSPosition(string ra, string dec)
        {
            if (string.IsNullOrWhiteSpace(ra) || string.IsNullOrWhiteSpace(dec))
                return "";
            // Format RA: replace h/m/s with spaces
            string cdsPos = Regex.Replace(ra, @"\s*[hms]\s*", " ", RegexOptions.IgnoreCase);
            // Format Dec: replace d/m/s with spaces, ensure leading + for positive Dec
            dec = dec.Trim() + " ";
            if (!dec.StartsWith("-"))
            {
                cdsPos += " +";
            }
            cdsPos += Regex.Replace(dec, @"\s*[dms]\s*", " ", RegexOptions.IgnoreCase);
            return cdsPos;
        }

        private bool IsDoubleWDSRow(DataGridViewRow row)
        {
            if (row == null) return false;

            var typeCell = row.Cells["Type"]?.Value;
            var catCell = row.Cells["Catalogue"]?.Value;
            if (typeCell == null || catCell == null) return false;

            string type = typeCell.ToString().Trim();
            string cat = catCell.ToString();

            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(cat)) return false;

            // Accept exact "Dbl" or values that start with "Dbl" (case-insensitive)
            if (!type.Equals("Dbl", StringComparison.OrdinalIgnoreCase) &&
                !type.StartsWith("Dbl", StringComparison.OrdinalIgnoreCase))
                return false;

            // Catalogue must contain "Washington" or "WDS" (case-insensitive)
            if (cat.IndexOf("Washington", StringComparison.OrdinalIgnoreCase) >= 0 ||
                cat.IndexOf("WDS", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            return false;
        }

        private void tsmiCentre_Click(object sender, EventArgs e)
        {
            CentreSelected();
        }

        private void tsmiCDSByName_Click(object sender, EventArgs e)
        {
            var row = dgvSearchResults.CurrentRow ?? (dgvSearchResults.SelectedRows.Count > 0 ? dgvSearchResults.SelectedRows[0] : null);
            var id = row?.Cells["ID"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("No ID available", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Build URL and open (escape the ID)
            string url = "https://cdsportal.u-strasbg.fr/?target=" + Uri.EscapeDataString(id);
            OpenUrl(url);
        }

        private void tsmiCDSByPosition_Click(object sender, EventArgs e)
        {
            var row = dgvSearchResults.CurrentRow ?? (dgvSearchResults.SelectedRows.Count > 0 ? dgvSearchResults.SelectedRows[0] : null);
            var raObj = row?.Cells["RA"]?.Value.ToString();
            var decObj = row?.Cells["Dec"]?.Value.ToString();
            if (raObj == null || decObj == null)
            {
                MessageBox.Show("No position available", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string cdsPos = FormatCDSPosition(raObj, decObj);


            // Build URL and open (format RA/Dec to 6 decimal places)
            string url = "https://cdsportal.u-strasbg.fr/?target=" + Uri.EscapeDataString(cdsPos);
            OpenUrl(url);
        }
        private void SelectRowsByIdAndCatalogue(string id, string catalogue, bool Filtered)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(catalogue))
                return;

            id = id.Trim();
            catalogue = catalogue.Trim();

            dgvSearchResults.ClearSelection();

            DataGridViewRow firstMatch = null;
            List<string> Components = new List<string>();
            int RowIndex = -1;
            foreach (DataGridViewRow row in dgvSearchResults.Rows)
            {
                if (row.IsNewRow) continue;

                var cellId = row.Cells["ID"]?.Value?.ToString();
                var cellCat = row.Cells["Catalogue"]?.Value?.ToString();
                var cellComp = row.Cells["Comp"]?.Value?.ToString();

                if (string.IsNullOrWhiteSpace(cellId) || string.IsNullOrWhiteSpace(cellCat) || string.IsNullOrWhiteSpace(cellComp))
                    continue;

                if (string.Equals(cellId.Trim(), id, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(cellCat.Trim(), catalogue, StringComparison.OrdinalIgnoreCase))
                {
                    if (Filtered && Components.Contains(cellComp[0].ToString()))
                        continue; // Skip duplicate component in filtered mode

                    row.Selected = true;
                    Components.Add(cellComp[0].ToString());
                    if (firstMatch == null) firstMatch = row;
                    if (RowIndex == -1) RowIndex = row.Index;
                }
            }

            dgvSearchResults.FirstDisplayedScrollingRowIndex = RowIndex;

            UpdateSearchInfo(dgvSearchResults.SelectedRows.Count);
        }

        void WDSComponentsSelection(bool Filtered = false)
        {
            DataGridViewRow sourceRow = null;
            if (dgvSearchResults.SelectedRows.Count > 0)
                sourceRow = dgvSearchResults.SelectedRows[0];
            else if (dgvSearchResults.CurrentRow != null)
                sourceRow = dgvSearchResults.CurrentRow;

            if (sourceRow == null) return;

            var idObj = sourceRow.Cells["ID"]?.Value;
            var typeObj = sourceRow.Cells["Type"]?.Value;
            var catObj = sourceRow.Cells["Catalogue"]?.Value;
            if (idObj == null || typeObj == null || catObj == null) return;

            string sourceID = idObj.ToString().Trim();
            string sourceType = typeObj.ToString().Trim();
            string sourceCatalogue = catObj.ToString().Trim();

            dt.DefaultView.Sort = "Catalogue ASC, ID ASC, Comp ASC";

            // Bind the grid to the sorted view (use DefaultView to keep the sort active)
            dgvSearchResults.DataSource = dt.DefaultView;

            SelectRowsByIdAndCatalogue(sourceID, sourceCatalogue, Filtered);

            DrawSelectedObjects();

            CentreSelected();
        }
        private void tsmiShowStarSystem_Click(object sender, EventArgs e)
        {
            WDSComponentsSelection(false);
        }

        private void tsmiWDSComponentsFiltered_Click(object sender, EventArgs e)
        {
            WDSComponentsSelection(true);
        }

        private void tsmiCopyRow_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            // include header row once
            var headers = dgvSearchResults.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText);
            sb.AppendLine(string.Join("\t", headers));

            // SelectedRows collection is not guaranteed to be in display order.
            // Order by row index so output is top-to-bottom.
            var selectedRows = dgvSearchResults.SelectedRows.Cast<DataGridViewRow>()
                                   .Where(r => !r.IsNewRow)
                                   .OrderBy(r => r.Index);

            foreach (DataGridViewRow row in selectedRows)
            {
                var values = row.Cells.Cast<DataGridViewCell>().Select(c => c.FormattedValue?.ToString() ?? "");
                sb.AppendLine(string.Join("\t", values));
            }

            Clipboard.SetText(sb.ToString());
        }

        private void tsmiCopyCell_Click(object sender, EventArgs e)
        {
            var cell = dgvSearchResults.CurrentCell;
            if (cell != null)
            {
                if (!string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    Clipboard.SetText(cell.FormattedValue?.ToString() ?? "");
                }
            }
        }

        private void SearchContextMenu_Opening(object sender, CancelEventArgs e)
        {
            tsmiCentre.Enabled = dgvSearchResults.SelectedRows.Count == 1;
            tsmiCopyCell.Enabled = dgvSearchResults.SelectedRows.Count == 1 && dgvSearchResults.CurrentCell != null && !string.IsNullOrWhiteSpace(dgvSearchResults.CurrentCell.Value.ToString());
            tsmiCopyRow.Enabled = dgvSearchResults.SelectedRows.Count >= 1;

            tsmiCDSByName.Enabled = dgvSearchResults.SelectedRows.Count == 1;
            tsmiCDSByName.Text = "CDS by Name " + (dgvSearchResults.SelectedRows.Count == 1 ? dgvSearchResults.SelectedRows[0].Cells["ID"].Value.ToString() : "");

            tsmiCDSByPosition.Enabled = dgvSearchResults.SelectedRows.Count == 1;
            string cdsPos = FormatCDSPosition(dgvSearchResults.SelectedRows[0].Cells["RA"].Value.ToString(), dgvSearchResults.SelectedRows[0].Cells["Dec"].Value.ToString());
            tsmiCDSByPosition.Text = "CDS by Position " + (dgvSearchResults.SelectedRows.Count == 1 ? cdsPos + "..." : "");

            // Only one row selected and it is Dbl+WDS.
            // Enable "Select as Star System" only when at least one of the selected rows (or current row if none selected)
            // matches: Type is Dbl and Catalogue contains "Washington" or "WDS"
            IEnumerable<DataGridViewRow> rowsToCheck;
            if (dgvSearchResults.SelectedRows.Count > 0)
            {
                rowsToCheck = dgvSearchResults.SelectedRows.Cast<DataGridViewRow>();
            }
            else if (dgvSearchResults.CurrentRow != null)
            {
                rowsToCheck = new[] { dgvSearchResults.CurrentRow };
            }
            else
            {
                rowsToCheck = Enumerable.Empty<DataGridViewRow>();
            }
            tsmiShowStarSystem.Enabled = dgvSearchResults.SelectedRows.Count == 1 && rowsToCheck.Any(r => IsDoubleWDSRow(r));
            tsmiWDSComponentsFiltered.Enabled = tsmiShowStarSystem.Enabled;

        }

        private void dgvSearchResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // header / empty area

            SearchContextMenu.Show(Cursor.Position);
        }
    }
}
