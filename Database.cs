using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace EAACtrl
{
    internal class Database
    {
        public string ConnectionString = "Server=127.0.0.1;Port=5432;Database=Astro;User Id=postgres;Password=Forrest;";

        private APHelper APHelper = new APHelper();

        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("Names");
            dt.Columns.Add("Type");
            dt.Columns.Add("Mag", typeof(double));
            dt.Columns.Add("Mag2", typeof(double));
            dt.Columns.Add("Period", typeof(double));
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
            dt.Columns.Add("_ID");
            dt.Columns.Add("_Epoch", typeof(double));
            return dt;
        }

        private void ProcessObjects(ref NpgsqlDataReader reader, ref DataTable dt)
        {
            while (reader.Read())
            {
                List<string> Namelist = new List<string>();
                var ID = "";
                var Names = "";

                var GladeID = reader.GetInt32(0);

                var GWGC = reader.GetString(2).Trim();
                if (GWGC != "-")
                {
                    Namelist.Add(GWGC);
                }

                var PGC = "";
                if (!reader.IsDBNull(1))
                {
                    PGC = reader.GetString(1).Trim();
                    Namelist.Add("PGC " + PGC);
                }

                var HyperLEDA = reader.GetString(3).Trim();
                if (HyperLEDA != "-")
                {
                    if (int.TryParse(HyperLEDA, out int iHyperLEDA))
                    {
                        Namelist.Add("LEDA " + iHyperLEDA.ToString());
                    }
                    else
                    {
                        Namelist.Add(HyperLEDA);
                    }
                }

                var TwoMass = reader.GetString(4).Trim();
                if (TwoMass != "-")
                {
                    Namelist.Add("2MASS " + TwoMass);
                }

                var Wise = reader.GetString(5).Trim();
                if (Wise != "-")
                {
                    Namelist.Add("WISEA " + Wise);
                }

                var SDSS = reader.GetString(6).Trim();
                if (SDSS != "-")
                {
                    Namelist.Add("SDSS " + SDSS);
                }

                bool bFirst = true;
                foreach (var name in Namelist)
                {
                    if (bFirst)
                    {
                        ID = name;
                        bFirst = false;
                    }
                    else
                    {
                        Names += name + " ";
                    }
                }


                var Type = reader.GetChar(7);
                string sType = "";
                if (Type == 'G')
                {
                    sType = "Galaxy";
                }
                else if (Type == 'Q')
                {
                    sType = "Quasar";
                }

                var RAd = reader.GetDouble(8) / 15.0;
                var RA = APHelper.RADecimalHoursToHMS(RAd, @"hh\hmm\mss\.ff\s");
                var Decd = reader.GetDouble(9);
                var Dec = APHelper.DecDecimalToDMS(Decd);
                var Bmag = Math.Round(reader.GetDouble(10), 2);

                double DistanceMpc = 0;
                if (!reader.IsDBNull(12))
                {
                    DistanceMpc = Math.Round(reader.GetDouble(12) * 299792.458 / Properties.Settings.Default.Hubble, 0); // Convert to Mpc
                }

                dt.Rows.Add(ID, Names, sType, Bmag, 0, 0, DistanceMpc, "", "", "", 0.0, 0.0, RA, Dec, RAd, Decd, "", "Glade+");
            }
        }

        private void ProcessREGALADEObjects(ref NpgsqlDataReader reader, ref DataTable dt)
        {
            while (reader.Read())
            {
                List<string> Namelist = new List<string>();
                var Names = "";

                var ID = reader.GetString(1).Trim();

                var RAd = reader.GetDouble(2) / 15.0;
                var RA = APHelper.RADecimalHoursToHMS(RAd, @"hh\hmm\mss\.ff\s");
                var Decd = reader.GetDouble(3);
                var Dec = APHelper.DecDecimalToDMS(Decd);
                
                var gmag = 0.0;
                if (!reader.IsDBNull(10))
                    gmag = Math.Round(reader.GetDouble(10), 2);

                double DistanceMpc = 0;
                if (!reader.IsDBNull(4))
                {
                    DistanceMpc = Math.Round(reader.GetDouble(4));
                }

                double PA = 0.0;
                if (!reader.IsDBNull(8))
                {
                    PA = Math.Round(reader.GetDouble(8), 2);
                }

                var objSize = "";
                // Both size entries available
                if (!reader.IsDBNull(6) && !reader.IsDBNull(7))
                {
                    objSize = Math.Round(reader.GetDouble(6), 2).ToString() + " x " + Math.Round(reader.GetDouble(7), 2).ToString();
                }

                dt.Rows.Add(ID, Names, "Galaxy", gmag, 0,0, DistanceMpc, "", objSize, "", PA, 0.0, RA, Dec, RAd, Decd, "", "REGALADE");
            }
        }

        private void ProcessAAVSO_VSXObjects(ref NpgsqlDataReader reader, ref DataTable dt)
        {
            while (reader.Read())
            {
                List<string> Namelist = new List<string>();
                var Names = "";

                var ID = reader.GetString(1).Trim();

                var RAd = reader.GetDouble(3) / 15.0;
                var RA = APHelper.RADecimalHoursToHMS(RAd, @"hh\hmm\mss\.ff\s");
                var Decd = reader.GetDouble(4);
                var Dec = APHelper.DecDecimalToDMS(Decd);

                var maxmag = 0.0;
                if (!reader.IsDBNull(7))
                    maxmag = Math.Round(reader.GetDouble(7), 2);

                var minmag = 0.0;
                if (!reader.IsDBNull(12))
                    minmag = Math.Round(reader.GetDouble(12), 2);

                var Period = 0.0;
                if (!reader.IsDBNull(18))
                    Period = Math.Round(reader.GetDouble(18), 2);

                var varType = "";
                if (!reader.IsDBNull(5))
                {
                    varType = reader.GetString(5).Trim();
                    varType = varType.Replace("|", ",");
                }

                var _ID = "";
                if (!reader.IsDBNull(0))
                {
                    _ID = reader.GetInt32(0).ToString();
                }

                var _Epoch = 0.0;
                if (!reader.IsDBNull(15))
                {
                    _Epoch = reader.GetDouble(15);
                }


                dt.Rows.Add(ID, Names, "Var Star - " + varType, maxmag, minmag, Period, 0, "", 0, "", 0, 0.0, RA, Dec, RAd, Decd, "", "AAVSO VSX", _ID, _Epoch);
            }
        }

        public DataTable GladeBoundingSearch(List<(double, double)> BoundingPolygon, double MagnitudeLimit, string ObjectType)
        {
            DataTable dt = CreateTable();
            
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);

            conn.Open();

            string Query = "SELECT * FROM \"Glade\" WHERE ST_Contains(ST_GEOMFROMTEXT('POLYGON((";
            bool bFirst = true;
            foreach (var polygon in BoundingPolygon)
            {
                double RA = polygon.Item1;
                double Dec = polygon.Item2;

                if (!bFirst)
                {
                    Query += ", ";
                }
                Query += RA.ToString() + " " + Dec.ToString();
                bFirst = false;
            }
            
            //Query += "5 5, 6 5, 6 6, 5 6, 5 5"; // Polygon shape
            Query += "))',4326), \"geom\")";
            Query += "AND \"Bmag\" < " + MagnitudeLimit.ToString();
            if (ObjectType != "All")
            {
                Query += " AND \"Type\" = " + "'" + ObjectType + "'";
            }
            Query += ";";

            var cmd = new NpgsqlCommand(Query, conn);
            var reader = cmd.ExecuteReader();

            ProcessObjects(ref reader, ref dt);

            conn.Close();

            return dt;
        }

        public DataTable GladeConeSearch(double CentreRA, double CentreDec, double Radius, double MagnitudeLimit, string ObjectType)
        {
 
            DataTable dt = CreateTable();

            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
          
            conn.Open();

            string Query = "SELECT * FROM \"Glade\"";
            Query += "WHERE ST_DWithin(geom, ST_SetSRID(ST_MakePoint(" + CentreRA.ToString() + "," + CentreDec.ToString() + "), 4326)," + Radius.ToString() + ") ";
            Query += "AND \"Bmag\" < " + MagnitudeLimit.ToString();
            if (ObjectType != "All")
            {
                Query += " AND \"Type\" = " + "'" + ObjectType + "'";
            }
            Query += ";";           
           
            var cmd = new NpgsqlCommand(Query, conn);
            var reader = cmd.ExecuteReader();
            
            ProcessObjects(ref reader, ref dt);

            conn.Close();

            return dt;
        }

        public DataTable REGALADEConeSearch(double CentreRA, double CentreDec, double Radius, double MagnitudeLimit)
        {

            DataTable dt = CreateTable();

            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);

            conn.Open();

            string Query = "SELECT * FROM \"Regalade\"";
            Query += "WHERE ST_DWithin(geom, ST_SetSRID(ST_MakePoint(" + CentreRA.ToString() + "," + CentreDec.ToString() + "), 4326)," + Radius.ToString() + ") ";
            Query += "AND \"gmag\" < " + MagnitudeLimit.ToString();
            Query += ";";

            var cmd = new NpgsqlCommand(Query, conn);
            var reader = cmd.ExecuteReader();

            ProcessREGALADEObjects(ref reader, ref dt);

            conn.Close();

            return dt;
        }

        public DataTable AAVSO_VSX_ConeSearch(double CentreRA, double CentreDec, double Radius, double MagnitudeLimit)
        {

            DataTable dt = CreateTable();

            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);

            conn.Open();

            string Query = "SELECT * FROM \"AAVSO_VSX\"";
            Query += "WHERE ST_DWithin(geom, ST_SetSRID(ST_MakePoint(" + CentreRA.ToString() + "," + CentreDec.ToString() + "), 4326)," + Radius.ToString() + ") ";
            Query += "AND \"max\" < " + MagnitudeLimit.ToString();
            Query += ";";

            var cmd = new NpgsqlCommand(Query, conn);
            var reader = cmd.ExecuteReader();

            ProcessAAVSO_VSXObjects(ref reader, ref dt);

            conn.Close();

            return dt;
        }
    }
}
