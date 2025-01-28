using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EAACtrl
{
    public partial class StelFOVOptions : Form
    {
        public StelFOVOptions()
        {
            InitializeComponent();
        }

        public int Mode = 0;

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.soID = chkID.Checked;
            Properties.Settings.Default.soMag = chkMagnitude.Checked;
            Properties.Settings.Default.soNames = chkNames.Checked;
            Properties.Settings.Default.soType = chkType.Checked;
            Properties.Settings.Default.soGalaxyType = chkGalaxyType.Checked;
            Properties.Settings.Default.soDistance = chkDistance.Checked;
            Properties.Settings.Default.soCatalogue = chkCatalogue.Checked;
            Properties.Settings.Default.Hubble = double.Parse(txtHubble.Text.Trim());
            Properties.Settings.Default.ShowRV = chkRadialVelocity.Checked;
            Properties.Settings.Default.DistanceUnit = cbDistanceUnit.SelectedIndex;

            Properties.Settings.Default.sfMagnitude = txtMagnitude.Text.Trim();
            Properties.Settings.Default.sfDistMin = txtDistMin.Text.Trim();
            Properties.Settings.Default.sfDistMax = txtDistMax.Text.Trim(); 
            Properties.Settings.Default.sfAll = rbAll.Checked;
            Properties.Settings.Default.sfStars = rbStars.Checked;
            Properties.Settings.Default.sfGalaxies = rbGalaxies.Checked;
            Properties.Settings.Default.sfQuasars = rbQuasars.Checked;
            Properties.Settings.Default.sfDouble = rbDouble.Checked;
            Properties.Settings.Default.sfVariable = rbVariable.Checked;
            Properties.Settings.Default.sfGlobulars = rbGlobulars.Checked;
            Properties.Settings.Default.sfNebulae = rbNebulae.Checked;
            Properties.Settings.Default.sfDatasource = cbDatasource.SelectedIndex;
            Properties.Settings.Default.SearchArea = cbSearchAreas.SelectedIndex;
            Properties.Settings.Default.sfAstroPlanner = rbAstroPlanner.Checked;
            Properties.Settings.Default.sfPlanetarium = rbPlanetarium.Checked;

            if (double.TryParse(txtSearchRadius.Text.Trim(), out double sr))
            {
                Properties.Settings.Default.SearchRadius = sr;
            }
            else 
            {
                Properties.Settings.Default.SearchRadius = 0.5;
            }

            Properties.Settings.Default.sfNoMag = cbNoMag.Checked;

            Properties.Settings.Default.sSharpCapDSA = chkSharpCapDSA.Checked;
            Properties.Settings.Default.sResultsList = chkResultsList.Checked;

            Properties.Settings.Default.StFontSize = txtStFontSize.Text.Trim();
            Properties.Settings.Default.StFontColour = lblStFontColour.BackColor;
            Properties.Settings.Default.StGraphicSize = txtGraphicSize.Text.Trim(); 
            Properties.Settings.Default.StGraphicColour = lblStGraphicColour.BackColor;
            Properties.Settings.Default.StGraphic = cbStGraphic.SelectedItem.ToString();
            Properties.Settings.Default.StLabelPosition = cbLabelPosition.SelectedItem.ToString();

            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void StelFOVOptions_Load(object sender, EventArgs e)
        {
            chkID.Checked = Properties.Settings.Default.soID;
            chkMagnitude.Checked = Properties.Settings.Default.soMag;
            chkNames.Checked = Properties.Settings.Default.soNames;
            chkType.Checked = Properties.Settings.Default.soType;
            chkGalaxyType.Checked = Properties.Settings.Default.soGalaxyType;
            chkDistance.Checked = Properties.Settings.Default.soDistance;
            chkCatalogue.Checked = Properties.Settings.Default.soCatalogue;
            txtHubble.Text = Properties.Settings.Default.Hubble.ToString();
            chkRadialVelocity.Checked = Properties.Settings.Default.ShowRV;
            cbDistanceUnit.SelectedIndex = Properties.Settings.Default.DistanceUnit;

            txtMagnitude.Text = Properties.Settings.Default.sfMagnitude;
            txtDistMin.Text = Properties.Settings.Default.sfDistMin;
            txtDistMax.Text= Properties.Settings.Default.sfDistMax;
            rbAll.Checked = Properties.Settings.Default.sfAll;
            rbStars.Checked = Properties.Settings.Default.sfStars;
            rbGalaxies.Checked = Properties.Settings.Default.sfGalaxies;
            rbQuasars.Checked = Properties.Settings.Default.sfQuasars;
            rbDouble.Checked = Properties.Settings.Default.sfDouble;
            rbVariable.Checked = Properties.Settings.Default.sfVariable;
            rbGlobulars.Checked = Properties.Settings.Default.sfGlobulars;
            rbNebulae.Checked = Properties.Settings.Default.sfNebulae;
            cbDatasource.SelectedIndex = Properties.Settings.Default.sfDatasource;
            cbSearchAreas.SelectedIndex = Properties.Settings.Default.SearchArea;
            txtSearchRadius.Text = Properties.Settings.Default.SearchRadius.ToString();
            cbNoMag.Checked = Properties.Settings.Default.sfNoMag;
            rbAstroPlanner.Checked = Properties.Settings.Default.sfAstroPlanner;
            rbPlanetarium.Checked = Properties.Settings.Default.sfPlanetarium;

            chkSharpCapDSA.Checked = Properties.Settings.Default.sSharpCapDSA;
            chkResultsList.Checked = Properties.Settings.Default.sResultsList;

            txtStFontSize.Text = Properties.Settings.Default.StFontSize;
            lblStFontColour.BackColor = Properties.Settings.Default.StFontColour;
            txtGraphicSize.Text = Properties.Settings.Default.StGraphicSize;
            lblStGraphicColour.BackColor = Properties.Settings.Default.StGraphicColour;
            cbStGraphic.SelectedItem = Properties.Settings.Default.StGraphic;
            cbLabelPosition.SelectedItem = Properties.Settings.Default.StLabelPosition;
            txtLabelDistance.Text = Properties.Settings.Default.StLabelDistance.ToString();
            
            if (int.TryParse(txtLabelDistance.Text.Trim(), out int ld))
            {
                Properties.Settings.Default.StLabelDistance = ld;
            }
            else
            {
                Properties.Settings.Default.StLabelDistance = 12;
            }

            if (Mode == 1)
            {
                gbFilter.Enabled = false;
                gbOtherOptions.Enabled = false;
                gbDatasource.Enabled = false;
                gbSearchArea.Enabled = false;
                txtHubble.Enabled = false;
                
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rbAll.Checked = true;
            txtDistMax.Text = "";
            txtDistMin.Text = "";
            txtMagnitude.Text = "";
        }

        private void btnFontColour_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblStFontColour.BackColor = colorDialog.Color;
            }
        }

        private void btnGraphicColour_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
                  
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblStGraphicColour.BackColor = colorDialog.Color;
            }
        }

        private void lblStFontColour_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblStFontColour.BackColor = colorDialog.Color;
            }
        }

        private void lblStGraphicColour_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblStGraphicColour.BackColor = colorDialog.Color;
            }
        }
    }
}
