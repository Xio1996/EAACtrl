﻿using System;
using System.Collections.Generic;

namespace EAACtrl
{
    internal class APHelper
    {
        public struct SizeInfo 
        {
            public double MajorAxis;
            public double MinorAxis;
        }

        public struct SizeInfoString
        {
            public string MajorAxis;
            public string MinorAxis;
        }

        private Dictionary<string, string> ConstellationMappings = new Dictionary<string, string>()
        {
            {"and", "Andromeda"}, {"ant", "Antlia"}, {"aps", "Apus"}, {"aqr", "Aquarius"},
            {"aql", "Aquila"}, {"ara", "Ara"}, {"ari","Aries"},{"aur","Auriga"},{"boo","Bootes"},
            {"cae", "Caelum"},{"cam", "Camelopardalis"},{"cnc", "Cancer"},{"cvn", "Canes Venatici"},
            {"cma", "Canis Major"},{"cmi", "Canis Minor"},{"cap", "Capricornus"},{"car", "Carina"},
            {"cas", "Cassiopeia"}, {"cen", "Centaurus"}, {"cep", "Cepheus"}, {"cet", "Cetus"},
            {"cha", "Chamaeleon"}, {"cir", "Circinus"}, {"col", "Columba"}, {"com", "Coma Berenices"},
            {"cra", "Corona Australis"}, {"crb", "Corona Borealis"}, {"crv", "Corvus"}, {"crt", "Crater"},
            {"cru", "Crux"}, {"cyg", "Cygnus"}, {"del", "Delphinus"}, {"dor", "Dorado"}, {"dra", "Draco"},
            {"equ", "Equuleus"}, {"eri", "Eridanus"}, {"for", "Fornax"}, {"gem", "Gemini"}, {"gru", "Grus"},
            {"her", "Hercules"}, {"hor", "Horologium"}, {"hya", "Hydra"}, {"hyi", "Hydrus"}, {"ind", "Indus"},
            {"lac", "Lacerta"}, {"leo", "Leo"}, {"lmi", "Leo Minor"}, {"lib", "Libra"}, {"lup", "Lupus"},
            {"lyn", "Lynx"}, {"lyr", "Lyra"}, {"men", "Mensa"}, {"mic", "Microscopium"}, {"mon", "Monoceros"},
            {"mus", "Musca"}, {"nor", "Norma"}, {"oct", "Octans"}, {"oph", "Ophiuchus"}, {"ori", "Orion"},
            {"pav", "Pavo"}, {"peg", "Pegasus"}, {"per", "Perseus"}, {"phe", "Phoenix"}, {"pic", "Pictor"},
            {"psc", "Pisces"}, {"psa", "Piscis Austrinus"}, {"pup", "Puppis"}, {"pyx", "Pyxis"}, {"ret", "Reticulum"},
            {"sge", "Sagitta"}, {"sgr", "Sagittarius"},{"sco", "Scorpius"}, {"scl", "Sculptor"}, {"sct", "Scutum"},
            {"ser", "Serpens"}, {"sex", "Sextans"}, {"tau", "Taurus"}, {"tel", "Telescopium"}, {"tri", "Triangulum"},
            {"tra", "Triangulum Australe"}, {"tuc", "Tucana"}, {"uma", "Ursa Major"}, {"umi", "Ursa Minor"}, 
            {"vel", "Vela"}, {"vir", "Virgo"}, {"vol", "Volans"}, {"vul", "Vulpecula"}
        };
        public string ConstellationFullName(string ConstellationAbreviation)
        {
            try
            {
                if (ConstellationAbreviation != null || ConstellationAbreviation != "")
                {
                    return ConstellationMappings[ConstellationAbreviation.ToLower().Trim()];
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return ConstellationAbreviation;
            }

            return "";
        }

        private Dictionary<string, string> APTypeMappings = new Dictionary<string, string>()
        {
            {"P Neb","Planetary Nebula"}, {"Open","Open Cluster"}, {"Open+D Neb","Open Cluster + Dark Nebula"},
            {"Neb","Nebula"}, {"SNR","Supernova Remnant"}, {"D Neb","Dark Nebula"}, {"Open+Asterism","Open Cluster + Asterism"},
            {"Dbl+Asterism","Double Star + Asterism"}, {"GalClus","Galaxy Cluster"}, {"E Neb","Emission Nebula"},
            {"Mult","Multiple Star"}
        };

        public string DisplayTypeFromAPType(string ObjectType)
        {
            try
            {
                if (ObjectType != null || ObjectType != "")
                {
                    return APTypeMappings[ObjectType];
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return ObjectType;
            }

            return "";
        }

        public string RADecimalHoursToHMS(double RA, string format)
        {
            TimeSpan time = TimeSpan.FromHours(RA * 15 * 24 / 360);
            return time.ToString(format);
        }

        public string DecDecimalToDMS(double Dec )
        {
            int degrees = Math.Abs((int)Dec);
            double minutes = (Math.Abs((Dec)) - degrees) * 60;
            double seconds = (minutes - (int)minutes) * 60;
            string dms = $"{(Dec < 0 ? "-" : "")}{degrees}d{((int)minutes).ToString("D2")}m{seconds.ToString("0.00s")}";
            return dms;
        }

        public string DecDecimalToDMSSp(double Dec)
        {
            int degrees = Math.Abs((int)Dec);
            double minutes = (Math.Abs((Dec)) - degrees) * 60;
            double seconds = (minutes - (int)minutes) * 60;
            string dms = $"{(Dec < 0 ? "-" : "+")}{degrees} {((int)minutes).ToString("D2")} {seconds.ToString("0.00")}";
            return dms;
        }

        public double DMSToDecDegrees(string hours, string minutes, string seconds)
        {
            if (string.IsNullOrEmpty(hours) || string.IsNullOrEmpty(minutes) || string.IsNullOrEmpty(seconds))
            {
                return -1; // Indicate invalid input
            }

            if (!double.TryParse(hours, out double hourValue))
            {
                return -1;
            }

            if (!double.TryParse(minutes, out double minuteValue))
            {
                return -1;
            }

            if (!double.TryParse(seconds, out double secondValue))
            {
                return -1;
            }

            return hourValue + (minuteValue / 60.0) + (secondValue / 3600.0);
        }

        public string TargetDisplay(APCmdObject apObj)
        {
            // How many characters of the objects name to display.
            const int MaxDisplayLength = 50;
            string sInfo = ""; string sName = "";
            if (apObj != null)
            {
                // Add the object ID to the information string
                sInfo = apObj.ID;
                // Decide which name field to use
                if ((apObj.Name.Length) > 0)
                {
                    if (apObj.Name.Contains(","))
                    {
                        // The default name field often contains multiple synonyms seperated by commas.
                        // Check and only display 2 names at most.
                        string[] sNames = apObj.Name.Split(',');

                        // If the length of using two names exceeds maximum specifed then use first name only
                        if (sNames[0].Length + sNames[1].Length + 1 > MaxDisplayLength)
                        {
                            sName = sNames[0];
                        }
                        else
                        {
                            sName = sNames[0] + ", " + sNames[1];
                        }
                    }
                    else
                    {
                        sName = apObj.Name;
                    }

                    // Constrain the name's length to the required characters
                    if (sName.Length > MaxDisplayLength)
                    {
                        sName = sName.Substring(0, MaxDisplayLength);
                    }
                    sInfo += ", " + sName;
                }

                sInfo += " {";

                // If a distance field exists then add to information
                if (apObj.Distance.Length > 0 && apObj.Distance != "???")
                {
                    sInfo += apObj.Distance + " - ";
                }

                // Get the Objects constellation (abbreviation) and lookup the full name
                sInfo += ConstellationFullName(apObj.Constellation);

                // Add object type information
                if (apObj.Type.Length > 0)
                {
                    sInfo += " - " + DisplayTypeFromAPType(apObj.Type) + "}";
                }
                else 
                {
                    sInfo += "}";
                }
            }
            return sInfo;
        }

        public SizeInfo ObjectSize(string apSize)
        {
            SizeInfo sizeInfo = new SizeInfo();
            sizeInfo.MajorAxis = 0; sizeInfo.MinorAxis = 0; 

            var Sizes = apSize.Split('x');
            if (Sizes.Length == 2)
            {
                sizeInfo.MajorAxis = double.Parse(Sizes[0].Trim());
                sizeInfo.MinorAxis = double.Parse(Sizes[1].Trim());
            }
            else 
            {
                sizeInfo.MajorAxis = double.Parse(apSize.Trim());
                sizeInfo.MinorAxis = sizeInfo.MajorAxis;
            }

            return sizeInfo;
        }

        public SizeInfoString ObjectSizeString(string apSize)
        {
            SizeInfoString sizeInfoString = new SizeInfoString();
            sizeInfoString.MajorAxis = ""; sizeInfoString.MinorAxis = "";

            if (apSize.Trim().Length > 0)
            {
                SizeInfo sizeInfo = ObjectSize(apSize);
                if (sizeInfo.MajorAxis > 0)
                {
                    sizeInfoString.MajorAxis = sizeInfo.MajorAxis.ToString();
                    sizeInfoString.MinorAxis = sizeInfo.MinorAxis.ToString();
                }
            }

            return sizeInfoString;
        }
    }
}
