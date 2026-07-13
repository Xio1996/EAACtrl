using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAACtrl
{
    public partial class ClusterMembers : Form
    {
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
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void btnShow_Click(object sender, EventArgs e)
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

            Stellarium.DrawClusterObjects(dt, chkMarkersOnly.Checked);
            //dgvSearchResults.Sort(dgvSearchResults.Columns["Probability"], System.ComponentModel.ListSortDirection.Descending);
        }
    }
}
