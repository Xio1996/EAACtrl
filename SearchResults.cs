using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
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



        private void SearchResults_Load(object sender, EventArgs e)
        {
            Stellarium.ScriptFolder = Properties.Settings.Default.StScriptFolder;

            if (ResultsList != null || ResultsTable != null)
            {
                dt.Columns.Add("ID");
                dt.Columns.Add("Names");
                dt.Columns.Add("Type");
                dt.Columns.Add("Magnitude", typeof(double));
                dt.Columns.Add("Distance Mpc", typeof(double));
                dt.Columns.Add("Galaxy Type");
                dt.Columns.Add("Catalogue");
                dt.Columns.Add("RA");
                dt.Columns.Add("Dec");
                dt.Columns.Add("dRA");
                dt.Columns.Add("dDec");
                dt.Columns.Add("Size");
                dt.Columns.Add("PosAngle");
                dt.Columns.Add("Constellation");

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
                            row["Magnitude"] = Mag;
                        }
                        else row["Magnitude"] = DBNull.Value;

                        if (double.TryParse(APObject[5], out double Dist))
                        {
                            row["Distance Mpc"] = Math.Round(Dist, 2);
                        }
                        else row["Distance Mpc"] = DBNull.Value;

                        row["Galaxy Type"] = APObject[4];
                        row["Catalogue"] = APObject[6];
                        row["RA"] = APObject[7];
                        row["Dec"] = APObject[8];
                        row["dRA"] = APObject[9];
                        row["dDec"] = APObject[10];
                        row["Size"] = APObject[11];
                        row["PosAngle"] = APObject[12];
                        row["Constellation"] = APObject[13];

                        dt.Rows.Add(row);

                    }
                }
                
                dgvSearchResults.DataSource = dt;
                dgvSearchResults.Columns["dRA"].Visible = false;
                dgvSearchResults.Columns["dDec"].Visible = false;
                dgvSearchResults.Columns["Size"].Visible = false;
                dgvSearchResults.Columns["PosAngle"].Visible = false;
                dgvSearchResults.Columns["Constellation"].Visible = false;

                dgvSearchResults.Sort(dgvSearchResults.Columns["Magnitude"], System.ComponentModel.ListSortDirection.Ascending);

                this.Text = "Search Results (" + dt.Rows.Count.ToString() + ")";
            }
        }

        private void btnDrawSelection_Click(object sender, EventArgs e)
        {
            DataTable Selected = new DataTable();

            Selected.Columns.Add("ID");
            Selected.Columns.Add("Names");
            Selected.Columns.Add("Type");
            Selected.Columns.Add("Magnitude", typeof(double));
            Selected.Columns.Add("Distance Mpc", typeof(double));
            Selected.Columns.Add("Galaxy Type");
            Selected.Columns.Add("Catalogue");
            Selected.Columns.Add("RA");
            Selected.Columns.Add("Dec");
            Selected.Columns.Add("dRA");
            Selected.Columns.Add("dDec");
            Selected.Columns.Add("Size");
            Selected.Columns.Add("PosAngle");
            Selected.Columns.Add("Constellation");

            foreach (DataGridViewRow row in dgvSearchResults.SelectedRows)
            {
                DataRow SelectedRow = Selected.NewRow();
                SelectedRow["ID"] = row.Cells["ID"].Value;
                SelectedRow["Names"] = row.Cells["Names"].Value;
                SelectedRow["Type"] = row.Cells["Type"].Value;

                if (double.TryParse(row.Cells["Magnitude"].Value.ToString(), out double Mag))
                {
                    SelectedRow["Magnitude"] = Mag;
                }
                else SelectedRow["Magnitude"] = DBNull.Value;

                if (double.TryParse(row.Cells["Distance Mpc"].Value.ToString(), out double Dist))
                {
                    SelectedRow["Distance Mpc"] = Math.Round(Dist, 2);
                }
                else SelectedRow["Distance Mpc"] = DBNull.Value;

                SelectedRow["Galaxy Type"] = row.Cells["Galaxy Type"].Value;
                SelectedRow["Catalogue"] = row.Cells["Catalogue"].Value;
                SelectedRow["RA"] = row.Cells["RA"].Value;
                SelectedRow["Dec"] = row.Cells["Dec"].Value;
                SelectedRow["dRA"] = row.Cells["dRA"].Value;
                SelectedRow["dDec"] = row.Cells["dDec"].Value;
                SelectedRow["Size"] = row.Cells["Size"].Value;
                SelectedRow["PosAngle"] = row.Cells["PosAngle"].Value;
                SelectedRow["Constellation"] = row.Cells["Constellation"].Value;

                Selected.Rows.Add(SelectedRow);
            }

            Stellarium.DrawObjects(Selected);
        }

        private void btnPlotAll_Click(object sender, EventArgs e)
        {
            Stellarium.DrawObjects(dt);
        }

        private void btnAddToAP_Click(object sender, EventArgs e)
        {
            List < APCmdObject > apObjects = new List < APCmdObject >();

            foreach (DataGridViewRow row in dgvSearchResults.SelectedRows)
            {
                APCmdObject obj = new APCmdObject();
                obj.ID = row.Cells["ID"].Value.ToString();
                obj.Name = row.Cells["Names"].Value.ToString();
                obj.Type = row.Cells["Type"].Value.ToString();
                obj.RA2000 = double.Parse(row.Cells["dRA"].Value.ToString());
                obj.Dec2000 = double.Parse(row.Cells["dDec"].Value.ToString());
                obj.Catalogue = row.Cells["Catalogue"].Value.ToString();
                obj.Distance = row.Cells["Distance Mpc"].Value.ToString();
                obj.GalaxyType = row.Cells["Galaxy Type"].Value.ToString();
                obj.Size = row.Cells["Size"].Value.ToString();
                obj.PosAngle = int.Parse(row.Cells["PosAngle"].Value.ToString());
                obj.Constellation = row.Cells["Constellation"].Value.ToString();

                if (double.TryParse(row.Cells["Magnitude"].Value.ToString(), out double Magnitude))
                {
                    obj.Magnitude = Magnitude;
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
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            using (StelFOVOptions frmOpt = new StelFOVOptions())
            {
                frmOpt.Mode = 1;
                frmOpt.TopMost = true;
                if (frmOpt.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }
    }
}
