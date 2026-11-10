using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAACtrl
{
    public partial class PlanetaryMoons : Form
    {
        DataTable dtMoons = new DataTable();

        private JPLHorizons jplHorizons = new JPLHorizons();
        private Stellarium Stellarium = new Stellarium();
        private int selectionAnchorRow = -1;
        private bool selectionAnchorValue;

        private double Latitude;
        private double Longitude;
        private double Altitude;

        public PlanetaryMoons()
        {
            InitializeComponent();

            // Load the site coordinates from settings
            Latitude = Properties.Settings.Default.SiteLat;
            Longitude = Properties.Settings.Default.SiteLng;
            Altitude = Properties.Settings.Default.SiteElev;

            Stellarium.ScriptFolder = Properties.Settings.Default.StScriptFolder;
            cbDSAFormat.SelectedIndex = Properties.Settings.Default.moonsDSAFormat;

            DataColumn selectedColumn = dtMoons.Columns.Add("Selected", typeof(bool));
            selectedColumn.DefaultValue = false;
            dtMoons.Columns.Add("Name");
            dtMoons.Columns.Add("JPL ID");
            dtMoons.Columns.Add("Diameter");
            dtMoons.Columns.Add("Period");
            dtMoons.Columns.Add("Description");

            dtMoons.Rows.Add(false, "Mars - Phobos", "401", "22.2", "7h 39m", "Discovered by Asaph Hall, 18th August 1877, orbits 6,000km from Mars.");
            dtMoons.Rows.Add(false, "Mars - Deimos", "402", "12.4", "30h 18m", "Discovered by Asaph Hall, 12th August 1877, orbits 23,460km from Mars.");
            dtMoons.Rows.Add(false, "Jupiter - Io", "501", "3,643", "1.76d", "Discovered by Galileo Galilei, 8th January 1610.");
            dtMoons.Rows.Add(false, "Jupiter - Europa", "502", "3, 121", "3.52d", "Discovered by Galileo Galilei, 8th January 1610.");
            dtMoons.Rows.Add(false, "Jupiter - Ganymede", "503", "5,268", "7.1d", "Discovered by Galileo Galilei, 7th January 1610.");
            dtMoons.Rows.Add(false, "Jupiter - Callisto", "504", "4,820", "16.6d", "Discovered by Galileo Galilei, 7th January 1610.");
            dtMoons.Rows.Add(false, "Jupiter - Amalthea", "505", "167", "11h 57m", "Discovered by E.E. Barnard, 9th September 1892.");
            dtMoons.Rows.Add(false, "Jupiter - Himalia", "506", "170", "249d", "Discovered by C.D. Perrine, 4th December 1904.");
            dtMoons.Rows.Add(false, "Saturn - Titan", "606", "5, 149", "16d", "Discovered by Christiaan Huygens, March 25th 1655.");
            dtMoons.Rows.Add(false, "Saturn - Rhea", "605", "1,527", "4.5d", "Discovered by G.D. Cassini, December 23rd 1672.");
            dtMoons.Rows.Add(false, "Saturn - Iapetus", "608", "1,470", "79d", "Discovered by G.D. Cassini, October 25th 1671.");
            dtMoons.Rows.Add(false, "Saturn - Dione", "604", "1,123", "2.7d", "Discovered by G.D. Cassini, March 30th 1684.");
            dtMoons.Rows.Add(false, "Saturn - Hyperion", "607", "270", "21d", "Discovered by William Bond, George Bond & William Lassell, September 16th 1848.");
            dtMoons.Rows.Add(false, "Saturn - Tethys", "603", "1,062", "1.9d", "Discovered by G.D. Cassini, March 11th 1684.");
            dtMoons.Rows.Add(false, "Saturn - Enceladus", "602", "504", "33h 36m", "Discovered by William Herschell, August 28th 1789.");
            dtMoons.Rows.Add(false, "Saturn - Mimas", "601", "396", "23h 36m", "Discovered by William Herschell, September 17th 1789.");
            dtMoons.Rows.Add(false, "Saturn - Phoebe", "609", "213", "549.41d", "Discovered by W.H Pickering, August 16th 1898.");
            dtMoons.Rows.Add(false, "Saturn - Janus", "610", "178.4", "16h 40m", "Discovered by A. Dollfus, 15th December 1966.");
            dtMoons.Rows.Add(false, "Saturn - Epimetheus", "611", "116.4", "0.7d", "Discovered by J. Fountain & S. Larson, 1977.");
            dtMoons.Rows.Add(false, "Uranus - Ariel", "701", "1,160", "2.52d", "Discovered by William Lassell, October 24th 1851.");
            dtMoons.Rows.Add(false, "Uranus - Umbriel", "702", "1,169", "4.14d", "Discovered by William Lassell, October 24th 1851.");
            dtMoons.Rows.Add(false, "Uranus - Titania", "703", "1,576", "8.73d", "Discovered by William Lassell, October 24th 1851.");
            dtMoons.Rows.Add(false, "Uranus - Oberon", "704", "1,523", "13.46d", "Discovered by William Lassell, October 24th 1851.");
            dtMoons.Rows.Add(false, "Neptune - Triton", "801", "2,706", "5.88d", "Discovered by William Lassell, October 10th 1846.");
            dtMoons.Rows.Add(false, "Neptune - Nereid", "802", "340", "360d", "Discovered by Gerard Kuiper, May 1st 1949.");
            dgvMoons.DataSource = dtMoons;

            dgvMoons.RowHeadersVisible = false;
            dgvMoons.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMoons.MultiSelect = true;
            dgvMoons.ReadOnly = true;
            dgvMoons.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvMoons.Columns["Name"].Frozen = true;

            dgvMoons.Columns["Selected"].Visible = false;
            dgvMoons.AutoResizeColumns();
            dgvMoons.Columns["Description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMoons.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvMoons.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvMoons.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvMoons.ScrollBars = ScrollBars.Both;
            dgvMoons.AllowUserToAddRows = false;
            dgvMoons.AllowUserToResizeRows = false;
            dgvMoons.CellMouseUp += dgvMoons_CellMouseUp;
            dgvMoons.Sorted += dgvMoons_Sorted;
            Shown += PlanetaryMoons_Shown;

            dgvMoons.ClearSelection();
            dgvMoons.CurrentCell = null;
            selectionAnchorRow = -1;

        }

        private void PlanetaryMoons_Shown(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                dgvMoons.ClearSelection();
                dgvMoons.CurrentCell = null;
                selectionAnchorRow = -1;
            }));
        }

        private void dgvMoons_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift && selectionAnchorRow >= 0)
            {
                int firstRow = Math.Min(selectionAnchorRow, e.RowIndex);
                int lastRow = Math.Max(selectionAnchorRow, e.RowIndex);

                for (int rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
                {
                    SetRowSelected(rowIndex, selectionAnchorValue);
                }
            }
            else
            {
                selectionAnchorValue = !GetRowSelected(e.RowIndex);

                for (int rowIndex = 0; rowIndex < dgvMoons.Rows.Count; rowIndex++)
                {
                    SetRowSelected(rowIndex, false);
                }

                if (selectionAnchorValue)
                {
                    SetRowSelected(e.RowIndex, true);
                }

                selectionAnchorRow = e.RowIndex;
            }
        }

        private bool GetRowSelected(int rowIndex)
        {
            return Convert.ToBoolean(dgvMoons.Rows[rowIndex].Cells["Selected"].Value ?? false);
        }

        private void SetRowSelected(int rowIndex, bool selected)
        {
            dgvMoons.Rows[rowIndex].Cells["Selected"].Value = selected;
            dgvMoons.Rows[rowIndex].Selected = selected;
        }

        private void dgvMoons_Sorted(object sender, EventArgs e)
        {
            for (int rowIndex = 0; rowIndex < dgvMoons.Rows.Count; rowIndex++)
            {
                dgvMoons.Rows[rowIndex].Selected = GetRowSelected(rowIndex);
            }
        }

        private string ToHms(string value)
        {
            string[] parts = value.Trim()
                                  .Split(new[] { ' ', '\t' },
                                         StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
            {
                throw new FormatException("Expected: HH MM SS.s");
            }

            return $"{parts[0].PadLeft(2, '0')}h" +
                   $"{parts[1].PadLeft(2, '0')}m" +
                   $"{parts[2]}s";
        }

        private string ToDms(string value)
        {
            string[] parts = value.Trim()
                                  .Split(new[] { ' ', '\t' },
                                         StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
            {
                throw new FormatException("Expected: DD MM SS.s");
            }

            return $"{parts[0]}d{parts[1].PadLeft(2, '0')}m{parts[2]}s";
        }

        private void StartStopTime(out string startTime, out string stopTime)
        {
            DateTime utcNow = DateTime.UtcNow;
            startTime = utcNow.ToString("yyyy-MM-dd HH:mm");
            stopTime = utcNow.AddMinutes(1).ToString("yyyy-MM-dd HH:mm");
        }

        private void btnSharpCapDSA_Click(object sender, EventArgs e)
        {
            List<string> selectedJPLIDs = new List<string>();
            string dsaData = "";

            foreach (DataGridViewRow row in dgvMoons.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Selected"].Value ?? false))
                {
                    string jplId = row.Cells["JPL ID"].Value.ToString();
                    string jplName = row.Cells["Name"].Value.ToString();
                    selectedJPLIDs.Add(jplId);

                    StartStopTime(out string startTime, out string stopTime);


                    string result = jplHorizons.QueryJPLHorizons(jplId, Longitude, Latitude, Altitude / 1000.0, startTime, stopTime, "1m");
                    if (result == "exception")
                    {
                        MessageBox.Show($"Error querying JPL Horizons for moon with JPL ID: {jplId}. Please check your internet connection and try again.", "JPL Horizons Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string[] parts;

                    jplHorizons.ProcessMoonData(jplName, row.Cells["Period"].Value.ToString(), row.Cells["Diameter"].Value.ToString(), result, cbDSAFormat.SelectedIndex, ref dsaData, out parts);

                    //Stellarium.DrawObject(jplName, ToHms(parts[3]), ToDms(parts[4]), false);
                }
            }

            if (selectedJPLIDs.Count == 0)
            {
                MessageBox.Show("Please select at least one moon to send to SharpCap DSA.", "No Moons Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Call the method to send the selected JPL IDs to SharpCap DSA
            Clipboard.SetText(dsaData);
           MessageBox.Show("Selected moons' data has been copied to the clipboard in SharpCap DSA format.", "Data Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in dtMoons.Rows)
            {
                row["Selected"] = false;
            }

            dgvMoons.ClearSelection();
            selectionAnchorRow = -1;
        }

        private void btnStellariumPlot_Click(object sender, EventArgs e)
        {
            List<string> selectedJPLIDs = new List<string>();
            string dsaData = "";

            foreach (DataGridViewRow row in dgvMoons.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Selected"].Value ?? false))
                {
                    string jplId = row.Cells["JPL ID"].Value.ToString();
                    string jplName = row.Cells["Name"].Value.ToString();
                    selectedJPLIDs.Add(jplId);
                    
                    StartStopTime(out string startTime, out string stopTime);
                    string result = jplHorizons.QueryJPLHorizons(jplId, Longitude, Latitude, Altitude / 1000.0, startTime, stopTime, "1m");
                    if (result == "exception")
                    {
                        MessageBox.Show($"Error querying JPL Horizons for moon with JPL ID: {jplId}. Please check your internet connection and try again.", "JPL Horizons Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string[] parts;

                    if (!jplHorizons.ProcessMoonData(jplName, row.Cells["Period"].Value.ToString(), row.Cells["Diameter"].Value.ToString(), result, cbDSAFormat.SelectedIndex, ref dsaData, out parts))
                    {
                        MessageBox.Show($"Error processing moon data for {jplName}.", "Moon Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Stellarium.DrawObject(jplName, ToHms(parts[3]), ToDms(parts[4]), false);
                }
            }

            if (selectedJPLIDs.Count == 0)
            {
                MessageBox.Show("Please select at least one moon to send to SharpCap DSA.", "No Moons Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnStellariumClear_Click(object sender, EventArgs e)
        {
            Stellarium.ClearObjects();
        }

        private void cbDSAFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.moonsDSAFormat = cbDSAFormat.SelectedIndex;
            Properties.Settings.Default.Save();
        }
    }
}
