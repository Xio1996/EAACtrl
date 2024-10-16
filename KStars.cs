using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;

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

        public bool SyncObject(double RA, double Dec)
        {

            string KStarsParams = sDBUSParams + "setRaDecJ2000 " + "double:" + RA.ToString() + " double:" + Dec.ToString();
            //string KStarsParams = sDBUSParams + "lookTowards string:'M 11'";
            // Create a new process start info
            ProcessStartInfo startInfo = new ProcessStartInfo(sDBusSend, KStarsParams);
            startInfo.UseShellExecute = false;
            // Start the process
            Process process = Process.Start(startInfo);

            // Optionally, wait for the process to exit
            process.WaitForExit();

            int ec = process.ExitCode;
            return true;
        }
    }
}
