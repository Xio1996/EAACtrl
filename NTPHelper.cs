using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class NTPHelper
    {
        public static string ExecuteNtpStripchartCommand(string computerName)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "w32tm.exe";
                    process.StartInfo.Arguments = $"/stripchart /computer:{computerName} /dataonly /samples:1";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return output;
                }
            }
            catch (Exception ex)
            {
                return $"Error executing command: {ex.Message}";
            }
        }
    }
}
