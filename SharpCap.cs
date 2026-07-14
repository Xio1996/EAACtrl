using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class SharpCap
    {

        static public string CreateDSAEntry(string ID, string Names, string Type, double RAHour, double Dec, double VMag, double RMax, double RMin, double PosAngle)
        {
            // IDs|Names|Type|RA(decimal hours)|Dec(degrees)|VMag|RMax(arcmin)|RMin(arcmin)|PosAngle
            string sOut = "";

            sOut = ID;

            sOut += "|" + Names + "|" + Type + "|" + RAHour.ToString() + "|" + Dec.ToString() + "|";

            if (double.IsNaN(VMag))
            {
                sOut += "|";
            }
            else
            {
                sOut += VMag.ToString() + "|";
            }

            if (double.IsNaN(RMax)) {
                sOut += "|";
            }
            else
            {
                sOut += RMax.ToString() + "|";
            }

            if (double.IsNaN(RMin))
            {
                sOut += "|";
            }
            else
            {
                sOut += RMin.ToString() + "|";
            }

            if (double.IsNaN(PosAngle))
            {
                sOut += "";
            }
            else
            {
                sOut += PosAngle.ToString();
            }

            sOut += "\r\n";

            return sOut;
        }
    }
}
