using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace EAACtrl
{
    internal class Database
    {
        public string ConnectionString = "Server=127.0.0.1;Port=5432;Database=Astro;User Id=postgres;Password=Forrest;";

        //Constellations 
        public ConstellationFinder Constellations = new ConstellationFinder();

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
            dt.Columns.Add("_StarCount", typeof(System.Int32));
            dt.Columns.Add("_r50", typeof(double));
            dt.Columns.Add("_pmRA", typeof(double));
            dt.Columns.Add("_pmDE", typeof(double));
            dt.Columns.Add("_Plx", typeof(double));
            dt.Columns.Add("_dist50", typeof(double));

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

                string Constellation = Constellations.GetConstellation(RAd, Decd);

                dt.Rows.Add(ID, Names, sType, Bmag, 0, 0, DistanceMpc, "", "", "", 0.0, 0.0, RA, Dec, RAd, Decd, Constellation, "Glade+");
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

                string Constellation = Constellations.GetConstellation(RAd, Decd);

                dt.Rows.Add(ID, Names, "Galaxy", gmag, 0,0, DistanceMpc, "", objSize, "", PA, 0.0, RA, Dec, RAd, Decd, Constellation, "REGALADE");
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
                    Period = reader.GetDouble(18);

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

                string Constellation = Constellations.GetConstellation(RAd, Decd);

                
                dt.Rows.Add(ID, Names, "Var Star - " + varType, maxmag, minmag, Period, 0, "", 0, "", 0, 0.0, RA, Dec, RAd, Decd, Constellation, "AAVSO VSX", _ID, _Epoch);
            }
        }

        private string GetClusterType(string typeCode)
        {
            switch (typeCode)
            {
                case "o":
                    return "Open Cluster";
                case "g":
                    return "Globular Cluster";
                case "m":
                    return "Moving Group";
                case "d":
                    return "Too Distant";
                case "r":
                    return "Rejected";
                default:
                    return "Unknown Type";
            }
        }           
        private void ProcessStar_ClusterObjects(ref NpgsqlDataReader reader, ref DataTable dt)
        {
            while (reader.Read())
            {
                var ID = reader.GetString(2).Trim().Replace("_", " ");
                var _ID = reader.GetInt32(3);

                var RAd = reader.GetDouble(0) / 15.0;
                var RA = APHelper.RADecimalHoursToHMS(RAd, @"hh\hmm\mss\.ff\s");
                var Decd = reader.GetDouble(1);
                var Dec = APHelper.DecDecimalToDMS(Decd);

                var AllNames = reader.GetString(4).Trim().Replace("_", " ").Replace(",", ", ");
                var ObjectType = GetClusterType(reader.GetString(5).Trim());
                
                var _StarCount = reader.GetInt32(6);
                var _r50 = reader.GetDouble(7);
                
                var _pmRA = reader.GetDouble(8);
                var _pmDE = reader.GetDouble(9);
                var _Plx = reader.GetDouble(10);
                var _dist50 = reader.GetDouble(11) * 3.261564; // Convert to ly

                string Constellation = Constellations.GetConstellation(RAd, Decd);


                dt.Rows.Add(ID, AllNames, ObjectType, DBNull.Value, DBNull.Value, DBNull.Value, 0, "", 0, "", 0, 0.0, RA, Dec, RAd, Decd, Constellation, "OC Census III", _ID, 0, _StarCount, _r50, _pmRA, _pmDE, _Plx, _dist50);
            }
        }

        public static double DistanceLightYearsFromParallaxMas(double parallaxMas) 
        { 
            if (parallaxMas <= 0.0) throw new ArgumentOutOfRangeException(nameof(parallaxMas), "Parallax must be positive (mas).");
            
            double parsecs = 1000.0 / parallaxMas; 
            const double LightYearsPerParsec = 3.261563777; 
            return parsecs * LightYearsPerParsec; 
        }
        private void ProcessStar_ClusterMemberObjects(ref NpgsqlDataReader reader, ref DataTable dt)
        {
            while (reader.Read())
            {
                var GaiaID = "Gaia dr3 " + reader.GetString(4).Trim();
                var Prob = reader.GetDouble(6).ToString("F2");
                var Gmag = reader.GetDouble(10).ToString("F2");
                var distLY = DistanceLightYearsFromParallaxMas(reader.GetDouble(9));
                var Parallax = reader.GetDouble(9);
                var pmRA = reader.GetDouble(7);
                var pmDE = reader.GetDouble(8);

                var RAd = reader.GetDouble(0) / 15.0;
                var RA = APHelper.RADecimalHoursToHMS(RAd, @"hh\hmm\mss\.ff\s");
                var Decd = reader.GetDouble(1);
                var Dec = APHelper.DecDecimalToDMS(Decd);

                dt.Rows.Add(GaiaID, Prob, Gmag, distLY, Parallax, pmRA, pmDE, RA, Dec, RAd, Decd);
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

        public DataTable AAVSO_VSX_ConstellationSearch(string wktBoundary, double MagnitudeLimit)
        {
            DataTable dt = CreateTable();

            if (string.IsNullOrWhiteSpace(wktBoundary))
            {
                Console.WriteLine("Invalid WKT boundary provided.");
                return dt;
            }        

            // The SQL query utilizes ST_GeomFromText to convert the WKT parameter into a geometry.
            // 4326 represents the spatial reference system identifier (SRID) for WGS 84 / J2000 equivalents.
            string sql = @"
            SELECT * 
            FROM ""AAVSO_VSX""
            WHERE ST_Contains(ST_GeomFromText(@wkt, 4326), geom) AND ""max"" < @magnitudeLimit";

            //geom_location
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        // 1. Create the parameter and add it to the command.
                        // Specifying NpgsqlDbType.Text ensures it passes correctly as a string expression.
                        var wktParameter = new NpgsqlParameter("@wkt", NpgsqlTypes.NpgsqlDbType.Text)
                        {
                            Value = wktBoundary
                        };
                        command.Parameters.Add(wktParameter);
                        var magnitudeLimitParameter = new NpgsqlParameter("@magnitudeLimit", NpgsqlTypes.NpgsqlDbType.Double)
                        {
                            Value = MagnitudeLimit
                        };
                        command.Parameters.Add(magnitudeLimitParameter);

                        // 2. Execute and read the data
                        var reader = command.ExecuteReader();
                        
                        ProcessAAVSO_VSXObjects(ref reader, ref dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred querying the database: {ex.Message}");
            }

            return dt;
        }

        public static void NullOutPlaceholders(DataTable dt, string[] columns, double placeholder = 999.0)
        {
            if (dt == null) throw new ArgumentNullException(nameof(dt));
            if (columns == null || columns.Length == 0) return;

            foreach (var colName in columns)
            {
                if (!dt.Columns.Contains(colName)) continue;
                var colType = dt.Columns[colName].DataType;

                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    var cell = dt.Rows[r][colName];
                    if (cell == DBNull.Value) continue;
                    if (cell == null) { dt.Rows[r][colName] = DBNull.Value; continue; }

                    // handle typed numeric columns
                    if (IsNumericType(colType))
                    {
                        double d;
                        try { d = Convert.ToDouble(cell, CultureInfo.InvariantCulture); }
                        catch { continue; }
                        if (Math.Abs(d - placeholder) < 1e-9) dt.Rows[r][colName] = DBNull.Value;
                        continue;
                    }

                    // handle string or other types by parsing
                    string s = cell.ToString().Trim();
                    if (s.Length == 0) { dt.Rows[r][colName] = DBNull.Value; continue; }

                    // accept "999", "999.0", etc.
                    if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsed))
                    {
                        if (Math.Abs(parsed - placeholder) < 1e-9) dt.Rows[r][colName] = DBNull.Value;
                    }
                    else
                    {
                        // exact string match fallback
                        if (string.Equals(s, placeholder.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal))
                            dt.Rows[r][colName] = DBNull.Value;
                    }
                }
            }
        }

        private static bool IsNumericType(Type t)
        {
            return t == typeof(byte) || t == typeof(sbyte) ||
                   t == typeof(short) || t == typeof(ushort) ||
                   t == typeof(int) || t == typeof(uint) ||
                   t == typeof(long) || t == typeof(ulong) ||
                   t == typeof(float) || t == typeof(double) ||
                   t == typeof(decimal);
        }

        public DataTable Star_Cluster_ConeSearch(double CentreRA, double CentreDec, double Radius, double MagnitudeLimit)
        {

            DataTable dt = CreateTable();

            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);

            conn.Open();

            string Query = "SELECT * FROM public.\"Clusters\"";
            Query += "WHERE ST_DWithin(geom, ST_SetSRID(ST_MakePoint(" + CentreRA.ToString() + "," + CentreDec.ToString() + "), 4326)," + Radius.ToString() + ") ";
            //Query += "AND \"max\" < " + MagnitudeLimit.ToString();
            Query += ";";

            var cmd = new NpgsqlCommand(Query, conn);
            var reader = cmd.ExecuteReader();

            ProcessStar_ClusterObjects(ref reader, ref dt);

            conn.Close();

            return dt;
        }

        public DataTable CreateClusterMemberDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Gaia ID");
            dt.Columns.Add("Probability", typeof(double));
            dt.Columns.Add("G mag", typeof(double));
            dt.Columns.Add("Distance ly", typeof(double));
            dt.Columns.Add("Parallax", typeof(double));
            dt.Columns.Add("pmRA", typeof(double));
            dt.Columns.Add("pmDE", typeof(double));
            dt.Columns.Add("RA2000");
            dt.Columns.Add("Dec2000");
            
            dt.Columns.Add("_RAd2000", typeof(double));
            dt.Columns.Add("_Decd2000", typeof(double));

            return dt;
        }
        public DataTable Star_Cluster_MemberSearch(int ClusterID, double Probability, double MagnitudeLimit)
        {

            DataTable dt = CreateClusterMemberDataTable();

            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);

            conn.Open();

            string Query = "SELECT * FROM public.\"ClusterMembers\"";
            Query += "WHERE \"ID\" = " + ClusterID.ToString() + " ";
            Query += "AND \"Gmag\" < " + MagnitudeLimit.ToString() + " ";
            Query += "AND \"Prob\" >= " + (Probability*0.01).ToString() + " ";
            Query += ";";

            var cmd = new NpgsqlCommand(Query, conn);
            var reader = cmd.ExecuteReader();

            ProcessStar_ClusterMemberObjects(ref reader, ref dt);

            conn.Close();

            return dt;
        }

        public DataTable ProcessAPPlanObjects(ref DataTable dtPlan)
        {
            DataTable dtOut = CreateTable();
            for (int i = 0; i < dtPlan.Rows.Count; i++)
            {
                DataRow row = dtPlan.Rows[i];   

                var ID = row["ID"].ToString().Trim();

                var RAd = Convert.ToDouble(row["RA"]);
                var RA = APHelper.RADecimalHoursToHMS(RAd, @"hh\hmm\mss\.ff\s");
                var Decd = Convert.ToDouble(row["Dec"]);
                var Dec = APHelper.DecDecimalToDMS(Decd);

                var maxmag = 0.0;
                if (!string.IsNullOrEmpty(row["Magnitude"].ToString()))
                    maxmag = Math.Round(Convert.ToDouble(row["Magnitude"]), 2);

                var minmag = 0.0;
                if (!string.IsNullOrEmpty(row["Magnitude2"].ToString()))
                {
                        minmag = Math.Round(Convert.ToDouble(row["Magnitude2"]), 2);
                }

                var Sep = 0.0;
                if (!string.IsNullOrEmpty(row["Separation"].ToString()))
                {
                    Sep = Math.Round(Convert.ToDouble(row["Separation"]), 2);
                }

                var Period = 0.0;
                if (!string.IsNullOrEmpty(row["Period"].ToString()))
                    Period = Math.Round(Convert.ToDouble(row["Period"]), 2);

                var varType = "";
                if (!string.IsNullOrEmpty(row["Type"].ToString()))
                {
                    varType = row["Type"].ToString().Trim();
                    varType = varType.Replace("|", ",");
                }

                var _ID = "";
                var _Epoch = 0.0;
                var Catalogue = row["Catalogue"].ToString().Trim();
                var Names = row["Name"].ToString().Trim();
                var Size = row["Size"].ToString().Trim();
                var Comp = row["Comp"].ToString().Trim();

                var PA = 0;
                if (!string.IsNullOrEmpty(row["PosAngle"].ToString()))
                    PA = Convert.ToInt32(row["PosAngle"]);

                string Constellation = Constellations.GetConstellation(RAd, Decd);

                dtOut.Rows.Add(ID, Names, varType, maxmag, minmag, Period, 0, "", Size, Comp, PA, Sep, RA, Dec, RAd, Decd, Constellation, Catalogue, _ID, _Epoch);
            }

            var columnsToNull = new[] { "Mag", "Mag2" };
            NullOutPlaceholders(dtOut, columnsToNull, 999);
            columnsToNull = new[] { "PA" };
            NullOutPlaceholders(dtOut, columnsToNull, -999);
            columnsToNull = new[] { "Dist MPC", "Period", "Sep" };
            NullOutPlaceholders(dtOut, columnsToNull, 0);

            return dtOut;
        }
        public static DataTable ReadAPObjectsTable(string dbFilePath)
        {
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Database file path is required.", nameof(dbFilePath));
            if (!File.Exists(dbFilePath))
                throw new FileNotFoundException("Database file not found.", dbFilePath);

            // Quick header check for "SQLite format 3"
            using (var fs = File.OpenRead(dbFilePath))
            {
                var header = new byte[16];
                int read = fs.Read(header, 0, header.Length);
                var headerStr = Encoding.ASCII.GetString(header, 0, Math.Max(0, Math.Min(read, header.Length)));
                if (!headerStr.StartsWith("SQLite format 3"))
                    throw new InvalidDataException("File does not appear to be a SQLite format 3 database.");
            }

            var dt = new DataTable();

            // Use read-only connection string
            var connectionString = $"Data Source={dbFilePath};Read Only=True;";

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT * FROM Objects;", conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
    }
}
