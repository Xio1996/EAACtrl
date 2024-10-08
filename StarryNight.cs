using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EAACtrl
{
    internal class StarryNight
    {
        private string sMsg = "";
        public string StarryNightMsgPath= @"C:\StarryNightMsg\snmsg.txt";

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
    }
}
