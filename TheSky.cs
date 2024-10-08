﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class TheSky
    {
        private string sTSScriptHeader = "/* Java Script */\r\n/* Socket Start Packet */\r\n";
        private string sSetTSTargetPosition = "\r\n\r\nvar Out=\"\";\r\nvar ObjectFound = false;\r\ntry {\r\n\tsky6StarChart.Find(ObjectName);\r\n\tTheSkyXAction.execute(\"TARGET_CENTER\");\r\n\tObjectFound=true;\r\n}\r\ncatch(err){\tOut = err.message;}\r\n\r\ntry {\r\n\tif (!ObjectFound) {\r\n\t\tsky6StarChart.RightAscension = RA;\r\n\t\tsky6StarChart.Declination = Dec;\r\n\t\tObjectFound = true;\r\n\t}\r\n}\r\ncatch (err) { Out += err.message; }\r\n\r\nif (ObjectFound) {\r\n\tif (FOV > 0) {\r\n\t\tsky6StarChart.FieldOfView = FOV;\r\n\t}\r\n}\r\n\r\n/* Socket End Packet */";
        private string sMsg = "";

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        private string TCPMessage(String server, Int32 port, String message)
        {
            // String to store the response ASCII representation.
            String responseData = String.Empty;

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                TcpClient client = new TcpClient(server, port);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Translate the passed message into ASCII and store it as a Byte array.
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                // Receive the server response.

                // Buffer to store the response bytes.
                data = new Byte[1024];

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);

                stream.Close();
                client.Close();

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                sMsg = "TCP Message (" + elapsedTime + ")\r\n";

            }
            catch (Exception e)
            {
                sMsg = "TCP Exception: " + e.Message + "\r\n";
            }

            return responseData;
        }

        // Connects to Rotator. Calculates the PA and sets rotator angle to PA. This is reflected in TheSky FOVI as long as the FOVI is set as linked to the camera's rotator.
        public void AltAzFOVICorrection()
        {
            try
            {
                string sAltAzFOVICorr = "/* Java Script */\r\n/* Socket Start Packet */\r\nvar Out;\r\n/* Find centre of chart and calculate PA for rotator */\r\ntry {\r\n\tif (ccdsoftCamera.rotatorIsConnected() == 0) {\r\n\t\tccdsoftCamera.rotatorConnect();\r\n\t\tOut = \"Rotator Connected,\"\r\n\t}\r\n\r\n\tif (ccdsoftCamera.rotatorIsConnected() > 0) {\r\n\t\tccdsoftCamera.rotatorGotoPositionAngle(sky6StarChart.Rotation);\r\n\t\tOut = \"  PA:\" + sky6StarChart.Rotation.toFixed(3);\r\n\t}\r\n\telse {\r\n\t\tOut = \"-2\";\r\n\t}\r\n}\r\ncatch (err) { Out = \"-1\"; }\r\n/* Socket End Packet */";
                string sPA = TCPMessage("127.0.0.1", 3040, sAltAzFOVICorr);
                sMsg = "TS AltAzFOVI" + sPA.Substring(0, sPA.LastIndexOf("|")) + "\r\n";
            }
            catch (Exception) { sMsg = "TS AltAzFOVI failed!\r\n"; }
        }

        public void SlewToTarget(string RA, string Dec)
        {
            string sSlewScript = "/* Java Script */\r\n/* Socket Start Packet */\r\nif (sky6RASCOMTele.IsConnected==0)//Connect failed for some reason\r\n{\r\n\tOut = \"-1\"\r\n}\r\nelse\r\n{\r\n\tsky6RASCOMTele.Asynchronous = true;\r\n\tsky6RASCOMTele.SlewToRaDec(\"" + RA + "\", \"" + Dec + "\",\"\");\r\n\tOut  = \"0\";\r\n}\r\n/* Socket End Packet */";
            TCPMessage("127.0.0.1", 3040, sSlewScript);
        }

        public void SetTSTargetPosition(string id, string RA, string Dec, double FOV)
        {
            try
            {
                string sScriptParams = "var ObjectName = \"" + id + "\";var RA = " + RA + ";var Dec = " + Dec + ";var FOV = " + FOV.ToString() + ";";
                TCPMessage("127.0.0.1", 3040, sTSScriptHeader + sScriptParams + sSetTSTargetPosition);
                sMsg = "TS Target Id=" + id + " RA:" + RA.ToString() + " Dec:" + Dec.ToString() + "\r\n";
            }
            catch (Exception)
            {
                sMsg = "TS Target fail!\r\n";
            }
        }

        public Dictionary<string, string> GetTSObject()
        {
            Dictionary<string, string> ObjectParams = new Dictionary<string, string>();
            try
            {
                sMsg = "GetTSObject\r\n";
                string sCode = "/* Java Script */\r\n/* Socket Start Packet */\r\nvar Out;sky6ObjectInformation.Property(56);Out = \"RAJ2000d: \" + sky6ObjectInformation.ObjInfoPropOut + \"|\";sky6ObjectInformation.Property(57);Out = Out +  \"DecJ2000d: \" + sky6ObjectInformation.ObjInfoPropOut + \"|\";sky6ObjectInformation.Property(64);Out = Out +  \"PAd: \" + sky6ObjectInformation.ObjInfoPropOut + \"|\";sky6ObjectInformation.Property(11);Out = Out + sky6ObjectInformation.ObjInfoPropOut.replace(/(\\r\\n|\\n|\\r)/gm, \"|\");\r\n/* Socket End Packet */";

                string sOut = TCPMessage("127.0.0.1", 3040, sCode);
                sMsg = sOut + "\r\n";

                // Split the parameters into a handy dictionary for processing
                var sParams = sOut.Split('|');
                foreach (string sParam in sParams)
                {
                    var sPair = sParam.Split(':');
                    //UCAC catalog uses a ':' in the name.
                    if (sPair.Length == 2 || sPair.Length == 3)
                    {
                        if (sPair.Length == 3)
                        {
                            sPair[1] = sPair[1].Trim() + sPair[2];
                        }
                        ObjectParams.Add(sPair[0], sPair[1]);
                    }
                }

                return ObjectParams;

            }
            catch (Exception)
            {
                sMsg = "TS Target fail!\r\n";
            }

            return null;
        }
    }
}