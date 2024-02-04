using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EAACtrl
{
    internal class SkyChart
    {
        private string sMsg = "";

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        public string IPAddress = "127.0.0.1";
        public string Port = "3292";

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

        public void SkychartRedraw()
        {
            string sAddr = IPAddress;
            int iPort = Int32.Parse(Port);
            TCPMessage(sAddr, iPort, "REDRAW\r\n");
        }

        public void SkychartFOV(double FOV, bool Redraw = true)
        {
            string sAddr = IPAddress;
            int iPort = Int32.Parse(Port);

            string sFOVCmd = "SETFOV " + FOV.ToString() + "\r\n";

            TCPMessage(sAddr, iPort, sFOVCmd);

            if (Redraw)
            {
                SkychartRedraw();
            }
        }

        public void SkychartViewDirection(string Direction, bool ReDraw = true)
        {
            string sAddr = IPAddress;
            int iPort = Int32.Parse(Port);

            string sCmd = Direction + "\r\n";
            TCPMessage(sAddr, iPort, sCmd);

            if (ReDraw)
            {
                SkychartRedraw();
            }
        }

        public void SkychartTargetPosition(string id, string RA, string Dec, double FOV)
        {
            try
            {
                string sSearchCmd = "SEARCH \"" + id + "\" LOCK\r\n";

                string sResult = "";

                string sAddr = IPAddress;
                int iPort = Int32.Parse(Port);

                sResult = TCPMessage(sAddr, iPort, sSearchCmd);
                if (sResult.Contains("Not found!"))
                {
                    string sRADec = "SETRA " + RA + "\r\n";
                    sResult = TCPMessage(sAddr, iPort, sRADec);
                    sRADec = "SETDEC " + Dec + "\r\n";
                    sResult = TCPMessage(sAddr, iPort, sRADec);
                }

                if (FOV > 0)
                {
                    SkychartFOV(FOV, false);
                }

                SkychartRedraw();
                sMsg = "CdC Target Id=" + id + " RA:" + RA.ToString() + " Dec:" + Dec.ToString() + " FOV:" + FOV.ToString() + "\r\n";
            }
            catch (Exception)
            {
                sMsg = "CdC Target fail!\r\n";
            }
        }
    }
}
