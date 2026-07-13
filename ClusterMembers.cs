using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAACtrl
{
    public partial class ClusterMembers : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        public DataGridViewRow ClusterRow;
        private Stellarium Stellarium = new Stellarium();

        /// <summary>
        /// Convert an angular radius (in degrees) at a given distance (in light years)
        /// to a linear radius in light years.
        /// Uses r = distance * tan(theta) where theta is the angular radius in radians.
        /// </summary>
        /// <param name="distanceLy">Distance to the object in light years (must be >= 0)</param>
        /// <param name="angularRadiusDeg">Angular radius in degrees (0 &lt; angularRadiusDeg &lt; 90)</param>
        /// <returns>Linear radius in light years</returns>
        public static double AngularRadiusToLightYears(double distanceLy, double angularRadiusDeg)
        {
            if (distanceLy < 0)
                throw new ArgumentOutOfRangeException(nameof(distanceLy), "Distance must be non-negative.");

            if (angularRadiusDeg <= 0.0)
                return 0.0;

            if (angularRadiusDeg >= 90.0)
                throw new ArgumentOutOfRangeException(nameof(angularRadiusDeg), "Angular radius must be less than 90 degrees.");

            double radians = angularRadiusDeg * Math.PI / 180.0;
            return distanceLy * Math.Tan(radians);
        }

        /// <summary>
        /// Calculate the volume of a sphere for a given radius.
        /// V = 4/3 * π * r^3
        /// </summary>
        /// <param name="radius">Radius in the desired units (must be >= 0)</param>
        /// <returns>Volume in cubic units corresponding to the radius</returns>
        public static double SphereVolume(double radius)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be non-negative.");

            return (4.0 / 3.0) * Math.PI * Math.Pow(radius, 3);
        }

        public ClusterMembers()
        {
            InitializeComponent();
        }

        private void ClusterMembers_Load(object sender, EventArgs e)
        {
            Stellarium.ScriptFolder = Properties.Settings.Default.StScriptFolder;

            if (ClusterRow != null)
            {
                txtNames.Text = ClusterRow?.Cells["Names"]?.Value?.ToString();
                txtType.Text = ClusterRow?.Cells["Type"]?.Value?.ToString();
                lblStarCount.Text = ClusterRow?.Cells["_StarCount"]?.Value?.ToString();
                var distObj = ClusterRow?.Cells["_dist50"]?.Value;
                if (distObj != null && double.TryParse(distObj.ToString(), out double distVal))
                {
                    lblDistance.Text = distVal.ToString("F2") + " ly";
                }
                else
                {
                    lblDistance.Text = (distObj?.ToString() ?? "") + " ly";
                }
                var radius = AngularRadiusToLightYears(double.Parse(lblDistance.Text.Replace(" ly", "")), double.Parse(ClusterRow?.Cells["_r50"]?.Value?.ToString() ?? "0"));

                lblRadius.Text = ClusterRow?.Cells["_r50"]?.Value?.ToString() + " deg ( " + radius.ToString("F2") + " ly, Volume ";
                lblRadius.Text += SphereVolume(radius).ToString("F2") + " ly³ )";

                lblProperMotionRA.Text = ClusterRow?.Cells["_pmRA"]?.Value?.ToString() + " mas/yr";
                lblProperMotionDEC.Text = ClusterRow?.Cells["_pmDE"]?.Value?.ToString() + " mas/yr";
            }


            // Resize to accomodate content
            dgvSearchResults.AutoResizeColumns();

            txtProbability.Text = Properties.Settings.Default.OCMProb.ToString();
            txtMagnitude.Text = Properties.Settings.Default.OCMMag.ToString();
            chkMarkersOnly.Checked = Properties.Settings.Default.OCMMarkersOnly;

            ShowMembers();
        }
        private void CentreSelected()
        {
            if (dgvSearchResults.SelectedRows.Count > 0)
            {
                double RA = double.Parse(dgvSearchResults.SelectedRows[0].Cells["_RAd2000"].Value.ToString());
                double Dec = double.Parse(dgvSearchResults.SelectedRows[0].Cells["_Decd2000"].Value.ToString());
                Stellarium.SyncStellariumToPosition(RA, Dec);
            }
        }

        private void tsmiCentre_Click(object sender, EventArgs e)
        {
            CentreSelected();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void ShowMembers()
        {
            Properties.Settings.Default.OCMProb = double.Parse(txtProbability.Text);
            Properties.Settings.Default.OCMMag = double.Parse(txtMagnitude.Text);
            Properties.Settings.Default.OCMMarkersOnly = chkMarkersOnly.Checked;
            Properties.Settings.Default.Save();

            Database db = new Database();

            var ClusterID = Convert.ToInt32(ClusterRow?.Cells["_ID"]?.Value);

            var Probability = Convert.ToDouble(txtProbability.Text);
            var MagnitudeLimit = Convert.ToDouble(txtMagnitude.Text);

            var dt = db.Star_Cluster_MemberSearch(ClusterID, Probability, MagnitudeLimit);

            dt.DefaultView.Sort = "Probability DESC, G mag ASC";
            dgvSearchResults.DataSource = dt;

            dgvSearchResults.AutoResizeColumns();

            foreach (DataGridViewColumn col in dgvSearchResults.Columns)
            {
                if (col.Name.StartsWith("_"))
                {
                    col.Visible = false;
                }
            }

            // Removes first selection columnm
            dgvSearchResults.RowHeadersVisible = false;

            dgvSearchResults.Columns["Gaia ID"].DefaultCellStyle.BackColor = Color.LightBlue;
            dgvSearchResults.Columns["Distance ly"].DefaultCellStyle.Format = "F2";

            dgvSearchResults.AllowUserToResizeRows = false;
            dgvSearchResults.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvSearchResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            if (dgvSearchResults.Columns["Gaia ID"].Width > 350)
            {
                dgvSearchResults.Columns["Gaia ID"].Width = 350;
            }

            lblMemberCount.Text = dgvSearchResults.Rows.Count.ToString() + " members found";

            Stellarium.DrawClusterObjects(dt, chkMarkersOnly.Checked);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ShowMembers();
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

        private void tsmiCDSByName_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;
            if (dgvSearchResults.SelectedRows.Count > 0)
                row = dgvSearchResults.SelectedRows[0];
            else if (dgvSearchResults.CurrentRow != null)
                row = dgvSearchResults.CurrentRow;

            if (row == null) return;
            var id = row?.Cells["Gaia ID"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("No GaiaID available", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Build URL and open (escape the ID)
            string url = "https://cdsportal.u-strasbg.fr/?target=" + Uri.EscapeDataString(id);
            OpenUrl(url);
        }

        private void tsmiCDSByPosition_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;
            if (dgvSearchResults.SelectedRows.Count > 0)
                row = dgvSearchResults.SelectedRows[0];
            else if (dgvSearchResults.CurrentRow != null)
                row = dgvSearchResults.CurrentRow;

            if (row == null) return;
            var raObj = row?.Cells["RA2000"]?.Value.ToString();
            var decObj = row?.Cells["Dec2000"]?.Value.ToString();
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
        private void SearchContextMenu_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                tsmiCentre.Enabled = dgvSearchResults.SelectedRows.Count == 1;
                tsmiCopyCell.Enabled = dgvSearchResults.SelectedRows.Count == 1 && dgvSearchResults.CurrentCell != null && !string.IsNullOrWhiteSpace(dgvSearchResults.CurrentCell.Value.ToString());
                tsmiCopyRow.Enabled = dgvSearchResults.SelectedRows.Count >= 1;

                tsmiCDSByName.Enabled = dgvSearchResults.SelectedRows.Count == 1;
                tsmiCDSByName.Text = "CDS by Name " + (dgvSearchResults.SelectedRows.Count == 1 ? dgvSearchResults.SelectedRows[0].Cells["Gaia ID"].Value.ToString() : "");

                tsmiCDSByPosition.Enabled = dgvSearchResults.SelectedRows.Count == 1;
                string cdsPos = FormatCDSPosition(dgvSearchResults.SelectedRows[0].Cells["RA2000"].Value.ToString(), dgvSearchResults.SelectedRows[0].Cells["Dec2000"].Value.ToString());
                tsmiCDSByPosition.Text = "CDS by Position " + (dgvSearchResults.SelectedRows.Count == 1 ? cdsPos + "..." : "");
            }
            catch (Exception) { }
        }

        private void dgvSearchResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // ignore header clicks
            if (e.RowIndex < 0) return;

            // only intervene for right-click so Ctrl/Shift selection by left-click remains intact
            if (e.Button != MouseButtons.Right) return;

            var grid = dgvSearchResults;

            // If the clicked row is not already selected, make it the sole selection.
            // If it is already selected, keep the current multi-selection (Ctrl/Shift) as-is.
            if (!grid.Rows[e.RowIndex].Selected)
            {
                grid.ClearSelection();
                grid.Rows[e.RowIndex].Selected = true;
            }

            // Display the context menu at the mouse position
            SearchContextMenu.Show(Cursor.Position);
        }

        private void btnClearPlot_Click(object sender, EventArgs e)
        {
            Stellarium.ClearObjects();
        }

        private void SwitchAppToFront(string processName)
        {
            /*
            Process[] proc = Process.GetProcesses();
            foreach (var process in proc)
            {
                Console.WriteLine($"Process: {process.ProcessName}, ID: {process.Id}");
            }
            */

            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                IntPtr hWnd = processes[0].MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    SetForegroundWindow(hWnd);
                    Console.WriteLine($"{processName} brought to the front.");
                }
                else
                {
                    Console.WriteLine($"{processName} does not have a main window handle.");
                }
            }
            else
            {
                Console.WriteLine($"{processName} is not running.");
            }

        }
        private void btnAPFront_Click(object sender, EventArgs e)
        {
            SwitchAppToFront("AstroPlanner");
        }

        private void btnPlFront_Click(object sender, EventArgs e)
        {
            SwitchAppToFront("Stellarium");
        }
    }
}
