using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace EAACtrl
{
    internal class StarryNight
    {
        private string sMsg = "";
        public string StarryNightMsgPath= @"C:\StarryNightMsg\snmsg.txt";
        public string StarryNightReplyPath = @"C:\StarryNightMsg\snrcout.txt";

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        public void SetSNTargetPosition(string id, string RA, string Dec, double FOV)
        {
            sMsg = "SetSNTarPos " + id + ", " + RA + ", " + Dec + ", " + FOV.ToString() + "\r\n";

            File.WriteAllText(StarryNightMsgPath, "target|" + id + "|" + RA + "|" + Dec + "|" + FOV.ToString());
        }

        public void SetSNFOV(double FOV)
        {
            sMsg = "SetSNFOV " + FOV.ToString() + "\r\n";
            File.WriteAllText(StarryNightMsgPath, "fov|" + FOV.ToString());
        }

        public void SetSNAltAz(double Alt, double Az, double FOV)
        {
            sMsg = "SetSNAltAz " + Az.ToString() + ", " + Alt.ToString() + "\r\n";
            File.WriteAllText(StarryNightMsgPath, "altaz|" + Alt.ToString() + "|" + Az.ToString() + "|" + FOV.ToString());
        }

        public APCmdObject GetSNSelectedObject()
        {
            sMsg = "GetSNSelectedObject \r\n";
            string sOut = "";

            APCmdObject apObject = new APCmdObject();

            File.WriteAllText(StarryNightMsgPath, "getselobj");

            DateTime startTime = DateTime.Now;
            TimeSpan timeout = TimeSpan.FromSeconds(10);
            try
            {
                while (!File.Exists(StarryNightReplyPath))
                {
                    if (DateTime.Now - startTime > timeout)
                    {
                        throw new TimeoutException($"The file '{StarryNightReplyPath}' did not appear within the timeout period.");
                    }

                    Thread.Sleep(500); // Sleep for 500 milliseconds before checking again
                }

                sOut = File.ReadAllText(StarryNightReplyPath);
                if (sOut!= "")
                {
                    string[] sOutArray = sOut.Split('|');
                    apObject.ID = sOutArray[0];
                    apObject.Name = sOutArray[0];
                    apObject.Type = sOutArray[1];
                    apObject.RA2000 = double.Parse(sOutArray[2]);
                    apObject.Dec2000 = double.Parse(sOutArray[3]);

                    double Mag = sOutArray[8] == "" ? 0 : double.Parse(sOutArray[8]);
                    if (Mag == -9999.0)
                    {
                        Mag = 999;
                    }
                    apObject.Magnitude = Mag;
                    
                    double Size = sOutArray[9] == "" ? 0 : double.Parse(sOutArray[9]);
                    if (Size<0)
                    {
                        Size = 0;
                    }
                    apObject.Size = (Size*60.0).ToString(); // Convert to arcminutes
                }
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                File.Delete(StarryNightReplyPath);
            }

            return apObject;
        }
    }
}
