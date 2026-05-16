using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Globalization;

namespace EAACtrl
{
    internal class KStars
    {
        private string sMsg = "";
        private string sDBusSend = @"C:\Users\peter\AppData\Local\Programs\KStars Desktop Planetarium\bin\dbus-send.exe";
        private string sDBUSParams = "--system --print-reply --dest=org.kde.kstars /KStars org.kde.kstars.";

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        public bool SetTracking(bool Tracking)
        {

            string KStarsParams = sDBUSParams + "setTracking " + "boolean:" + Tracking.ToString().ToLower();

            // Create a new process start info
            ProcessStartInfo startInfo = new ProcessStartInfo(sDBusSend, KStarsParams);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            // Start the process
            Process process = Process.Start(startInfo);

            // Optionally, wait for the process to exit
            process.WaitForExit();

            int ec = process.ExitCode;
            return true;
        }

        public bool SyncAltAz(double Altitude, double Azimuth)
        {
            // Make sure tracking is disabled before setting Alt/Az, otherwise KStars will ignore the command
            SetTracking(false);

            string KStarsParams = sDBUSParams + "setAltAz " + "double:" + Altitude.ToString() + " double:" + Azimuth.ToString() + " boolean:true";
            // Create a new process start info
            ProcessStartInfo startInfo = new ProcessStartInfo(sDBusSend, KStarsParams);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            // Start the process
            Process process = Process.Start(startInfo);

            // Optionally, wait for the process to exit
            process.WaitForExit();

            int ec = process.ExitCode;
            return true;
        }

        public bool SyncObject(string ObjectName, string Catalogue)
        {
            // Make sure tracking is disabled before setting Alt/Az, otherwise KStars will ignore the command
            SetTracking(false);

            string formattedObjectName = FormatCatalogName(ObjectName, Catalogue);

            // Escape any double-quotes in the name, then wrap the argument in double-quotes
            string escaped = formattedObjectName.Replace("\"", "\\\"");
            string KStarsParams = sDBUSParams + "lookTowards string:\"" + escaped + "\"";

            var startInfo = new ProcessStartInfo(sDBusSend, KStarsParams)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process == null)
                {
                    sMsg = "Failed to start dbus-send.";
                    return false;
                }

                // Read output/error and wait for exit
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                sMsg = (output + Environment.NewLine + error).Trim();
                return process.ExitCode == 0;
            }
        }

        public bool ViewDirection(string direction)
        {
            string KStarsParams = sDBUSParams + "activateAction string:" + direction;
            ProcessStartInfo startInfo = new ProcessStartInfo(sDBusSend, KStarsParams);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            int ec = process.ExitCode;
            return ec == 0;
        }

        public bool Zoom(string fov)
        {
            string KStarsParams = sDBUSParams + "setApproxFOV double:" + fov;
            ProcessStartInfo startInfo = new ProcessStartInfo(sDBusSend, KStarsParams);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            int ec = process.ExitCode;
            return ec == 0;
        }

        public FocusInfo GetFocussedObject()
        {

            string KStarsParams = sDBUSParams + "getFocusInformationXML";

            var startInfo = new ProcessStartInfo(sDBusSend, KStarsParams)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process == null)
                {
                    sMsg = "Failed to start dbus-send.";
                    return null;
                }

                // Read output/error and wait for exit
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                
                sMsg = (output + Environment.NewLine + error).Trim();
                return ParseFocusXml(output);
            }
        }

        public string FormatCatalogName(string name, string catalogue)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            if (string.IsNullOrWhiteSpace(catalogue)) return name;

            string trimmed = name.Trim();
            string cat = catalogue.Trim().ToLowerInvariant();

            if (cat == "messier")
            {
                // Accept "M2", "m002", "2", "M 2" etc. -> "M 2"
                var m = Regex.Match(trimmed, @"^(?:M)?\s*0*(\d+)$", RegexOptions.IgnoreCase);
                if (m.Success) return $"M {m.Groups[1].Value}";
                return trimmed;
            }

            if (cat == "ngc")
            {
                // Accept "NGC1234", "ngc01234", "1234", "NGC 1234" -> "NGC 1234"
                var m = Regex.Match(trimmed, @"^(?:NGC)?\s*0*(\d+)$", RegexOptions.IgnoreCase);
                if (m.Success) return $"NGC {m.Groups[1].Value}";
                return trimmed;
            }

            if (cat == "ic")
            {
                // Accept "IC10", "ic010", "10", "IC 10" -> "IC 10"
                var m = Regex.Match(trimmed, @"^(?:IC)?\s*0*(\d+)$", RegexOptions.IgnoreCase);
                if (m.Success) return $"IC {m.Groups[1].Value}";
                return trimmed;
            }

            // Sharpless: accept "Sh2-265", "Sh 2-265", "Sh2 265", "Sh2265" etc. -> "Sh2 265"
            if (cat == "sharpless" || cat == "sh" || cat == "sh2")
            {
                // Match forms with optional spaces/dash between "Sh" and the number, and optional "2" token
                var m = Regex.Match(trimmed, @"^Sh\s*2?\s*[-\s]?\s*0*(\d+)$", RegexOptions.IgnoreCase);
                if (m.Success) return $"Sh2 {m.Groups[1].Value}";
                // Fallback: replace first '-' after a Sh/Sh2 prefix with a space
                var replaced = Regex.Replace(trimmed, @"^(Sh(?:2)?)[-]\s*(\d+)$", "$1 $2", RegexOptions.IgnoreCase);
                if (!string.Equals(replaced, trimmed, StringComparison.Ordinal)) return replaced;
                return trimmed;
            }

            // not a handled catalogue — return the trimmed input unchanged
            return trimmed;
        }

        public bool IsSupportedCatalogue(string catalogue)
        {
            if (string.IsNullOrWhiteSpace(catalogue)) return false;

            string cat = catalogue.Trim().ToLowerInvariant();
            switch (cat)
            {
                case "messier":
                case "ngc":
                case "ic":
                case "sharpless":
                case "sh":
                case "sh2":
                    return true;
                default:
                    return false;
            }
        }

        public class FocusInfo
        {
            public double? FOV_Degrees { get; set; }
            public double? RA_JNow_Degrees { get; set; }
            public double? Dec_JNow_Degrees { get; set; }
            public string RA_JNow_HMS { get; set; }
            public string Dec_JNow_DMS { get; set; }
            public double? Altitude_Degrees { get; set; }
            public double? Azimuth_Degrees { get; set; }
            public string Altitude_DMS { get; set; }
            public string Azimuth_DMS { get; set; }
            public string Focused_Object { get; set; }
            public string RawXml { get; set; }
        }

        public FocusInfo ParseFocusXml(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;
            try
            {
                // Strip any leading/trailing non-XML text: keep from first '<' to last '>' inclusive.
                int firstLt = xml.IndexOf('<');
                int lastGt = xml.LastIndexOf('>');
                if (firstLt < 0 || lastGt < 0 || lastGt < firstLt)
                {
                    sMsg = "ParseFocusXml: no XML found in input.";
                    return null;
                }

                string inner = xml.Substring(firstLt, lastGt - firstLt + 1);

                // Remove surrounding quotes if dbus wrapped the XML in quotes
                if ((inner.StartsWith("\"") && inner.EndsWith("\"")) || (inner.StartsWith("'") && inner.EndsWith("'")))
                    inner = inner.Substring(1, inner.Length - 2);
                // Some dbus / encoding paths insert stray control or non-breaking characters (e.g. "Â").
                // Normalize common artifacts before XML parse so XDocument does not choke on odd characters.
                // Clean common encoding artifacts before parsing
                string cleaned = inner.Trim().Replace("\u00A0", " ").Replace("Â", "");

                var doc = XDocument.Parse(cleaned);
                var root = doc.Root;
                if (root == null) return null;

                string get(string name)
                {
                    var el = root.Element(name) ?? root.Descendants(name).FirstOrDefault();
                    return el?.Value?.Trim();
                }

                double? parseDouble(string s)
                {
                    if (string.IsNullOrWhiteSpace(s)) return null;
                    if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out double v))
                        return v;
                    // try replace comma decimal separator fallback
                    s = s.Replace(',', '.');
                    if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out v))
                        return v;
                    return null;
                }

                var f = new FocusInfo
                {
                    RawXml = cleaned,
                    FOV_Degrees = parseDouble(get("FOV_Degrees")),
                    RA_JNow_Degrees = parseDouble(get("RA_JNow_Degrees")),
                    Dec_JNow_Degrees = parseDouble(get("Dec_JNow_Degrees")),
                    RA_JNow_HMS = get("RA_JNow_HMS"),
                    Dec_JNow_DMS = (get("Dec_JNow_DMS") ?? "").Replace("Â", "").Replace("&quot;", "\"").Trim(),
                    Altitude_Degrees = parseDouble(get("Altitude_Degrees")),
                    Azimuth_Degrees = parseDouble(get("Azimuth_Degrees")),
                    Altitude_DMS = (get("Altitude_DMS") ?? "").Replace("Â", "").Replace("&quot;", "\"").Trim(),
                    Azimuth_DMS = (get("Azimuth_DMS") ?? "").Replace("Â", "").Replace("&quot;", "\"").Trim(),
                    Focused_Object = get("Focused_Object")
                };

                return f;
            }
            catch (Exception ex)
            {
                // preserve last error message for diagnostics
                sMsg = "ParseFocusXml: " + ex.Message;
                return null;
            }
        }
    }
}
