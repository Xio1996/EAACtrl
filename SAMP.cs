using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Net.Http;

namespace EAACtrl
{
    internal class SAMP
    {
        public bool StandardProfile = false;
        public bool SAMPConnected = false;
        public string SAMP_PrivateKey = "";
        public string Samp_hub_url = @"http://127.0.0.1:21012"; //Web Profile
        private static readonly HttpClient httpClient = new HttpClient();

        private string sMsg = "";

        public string Message
        {
            get
            {
                return sMsg;
            }
        }

        public string SampMessage(string url, string message)
        {
            string result = "";

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var content = new StringContent(message, Encoding.UTF8, "application/xml");
                HttpResponseMessage response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();
                result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = $"SAMP Request to {url} {elapsedTime}";
            }
            catch (HttpRequestException e)
            {
                sMsg = $"SAMP Request {url} ERROR {e.Message}";
                result = "exception";
            }

            return result; 
        }

        public string SampRegister()
        {
            string result = "";
            //string sXML = "<?xml version='1.0'?><methodCall><methodName>samp.hub.register</methodName><params><param><value><struct><member><name>samp.name</name><value><string>EAACtrl</string></value></member></struct></value></param></params></methodCall>";
            string sSAMPRegister = "<?xml version='1.0'?><methodCall><methodName>samp.hub.register</methodName><params><param><value>[SECRETorNAME]</value></param></params></methodCall>";

            if (StandardProfile)
            {
                // Locate home folder and read the lock file
                string sHomeFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                if (File.Exists(sHomeFolder + @"\.samp"))
                {
                    // Read the lock file
                    string sSAMPLockFileContent = File.ReadAllText(sHomeFolder + @"\.samp");

                    int ipos = sSAMPLockFileContent.IndexOf("samp.secret=");
                    int ilastpos = sSAMPLockFileContent.IndexOf(Environment.NewLine, ipos);

                    string sSamp_secret = sSAMPLockFileContent.Substring(ipos + 12, ilastpos - (ipos + 12));

                    sSAMPRegister = sSAMPRegister.Replace("[SECRETorNAME]", sSamp_secret);

                    ipos = sSAMPLockFileContent.IndexOf("samp.hub.xmlrpc.url=");
                    ilastpos = sSAMPLockFileContent.IndexOf(Environment.NewLine, ipos);

                    Samp_hub_url = sSAMPLockFileContent.Substring(ipos + 20, ilastpos - (ipos + 20));

                    sMsg ="SampRegister: Standard Profile, " + "Secret=" + sSamp_secret + ", " + "URL=" + Samp_hub_url + "\r\n";
                }
                else { return ("nolockfile"); }
            }
            else
            {
                StandardProfile = false;

                sSAMPRegister = sSAMPRegister.Replace("[SECRETorNAME]", "EAACtrl");
                sSAMPRegister = sSAMPRegister.Replace("samp.hub.register", "samp.webhub.register");
                sMsg = "SampRegister: Web Profile, " + "URL=" + Samp_hub_url + "\r\n";
            }

            try
            {
                result = SampMessage(Samp_hub_url, sSAMPRegister);

                if (result != "")
                {
                    int iStart = 0, iEnd = 0, iPos = 0;
                    iPos = result.IndexOf("samp.private-key", 0);
                    iStart = result.IndexOf("<value>", iPos) + 7;
                    iEnd = result.IndexOf("</value>", iStart);

                    result = result.Substring(iStart, iEnd - iStart);

                    SAMP_PrivateKey = result;

                    sMsg = $"SampRegister: key={result}, {sMsg}\r\n";

                    SAMPConnected = true;

                }
                else
                {
                    sMsg = $"SampRegister: Can't Connect!, {sMsg}\r\n";
                }
            }
            catch (Exception e)
            {
                sMsg = $"SampRegister ERROR {e.Message}\r\n";
                result = "exception";
            }

            return result;
        }

        public string SampDisconnect()
        {
            string result = "";
            //string sXML = "<?xml version='1.0'?><methodCall><methodName>samp.webhub.register</methodName><params><param><value><struct><member><name>samp.name</name><value><string>EAACtrl</string></value></member></struct></value></param></params></methodCall>";
            string sSAMPDisconnect = "<?xml version='1.0'?><methodCall><methodName>samp.hub.unregister</methodName><params><param><value>" + SAMP_PrivateKey + "</value></param></params></methodCall>";

            if (!StandardProfile)
            {
                sSAMPDisconnect = sSAMPDisconnect.Replace("samp.hub.unregister", "samp.webhub.unregister");
            }

            try
            {
                result = SampMessage(Samp_hub_url, sSAMPDisconnect);

                SAMPConnected = false;

                sMsg = $"SampDisconnect, {sMsg}\r\n";
            }
            catch (Exception e)
            {
                sMsg = $"SampDisconnect ERROR {e.Message}\r\n";
                result = "exception";
            }

            return result;
        }

        public string SampMetaData()
        {
            string result = "";
            string sSAMPMetaData = "<?xml version='1.0'?><methodCall><methodName>samp.hub.declareMetadata</methodName><params><param><value><string>[PRIVATEKEY]</string></value></param>" +
                "<param><value><struct><member><name>samp.name</name><value><string>EAACtrl</string></value></member><member><name>samp.description</name>" +
                "<value><string>Manges EAA workflow by coordinating astronomy apps such as AstroPlanner, SharpCap, Stellarium etc.</string></value></member>" +
                "<member><name>samp.author</name><value><string>Pete Gallop</string></value></member></struct></value></param></params></methodCall>";

            sSAMPMetaData = sSAMPMetaData.Replace("[PRIVATEKEY]", SAMP_PrivateKey);

            if (!StandardProfile)
            {
                sSAMPMetaData = sSAMPMetaData.Replace("samp.hub.declareMetadata", "samp.webhub.declareMetadata");
            }

            try
            {
                result = SampMessage(Samp_hub_url, sSAMPMetaData);
                sMsg = $"SampMetaData, {sMsg}\r\n";
            }
            catch (Exception e)
            {
                sMsg = $"SampMetaData ERROR {e.Message}\r\n";
                result = "exception";
            }

            return result;
        }

        public string Samp_coord_pointAt_sky(string RA, string Dec)
        {

            if (!SAMPConnected)
            { return "NOTCONNECTED"; }

            string result = "";
            string sSAMPCoordPointAt = "<?xml version='1.0'?><methodCall><methodName>samp.hub.notifyAll</methodName><params>";
            sSAMPCoordPointAt += "<param><value>" + SAMP_PrivateKey + "</value></param>";
            sSAMPCoordPointAt += "<param><value><struct><member><name>samp.mtype</name><value>coord.pointAt.sky</value></member><member><name>samp.params</name><value><struct>";
            sSAMPCoordPointAt += "<member><name>ra</name><value>" + RA + "</value></member>";
            sSAMPCoordPointAt += "<member><name>dec</name><value>" + Dec + "</value></member>";
            sSAMPCoordPointAt += "</struct></value></member></struct></value></param></params></methodCall>";

            if (!StandardProfile)
            {
                sSAMPCoordPointAt = sSAMPCoordPointAt.Replace("samp.hub.notifyAll", "samp.webhub.notifyAll");
            }

            try
            {
                result = SampMessage(Samp_hub_url, sSAMPCoordPointAt);
                sMsg = $"SampCoordPointAt {RA}, {Dec}, {sMsg} \r\n";
            }
            catch (Exception e)
            {
                sMsg = $"SampCoordPointAt ERROR {e.Message}\r\n";
                result = "exception";
            }
            return result;
        }

        public string Samp_script_aladin_send(string sScriptCmd)
        {

            if (!SAMPConnected)
            { return "NOTCONNECTED"; }

            string result = "";
            string sSAMPScript = "<?xml version='1.0'?><methodCall><methodName>samp.hub.notifyAll</methodName><params>";
            sSAMPScript += "<param><value>" + SAMP_PrivateKey + "</value></param>";
            sSAMPScript += "<param><value><struct><member><name>samp.mtype</name><value>script.aladin.send</value></member><member><name>samp.params</name><value><struct>";
            sSAMPScript += "<member><name>script</name><value>" + sScriptCmd + "</value></member>";
            sSAMPScript += "</struct></value></member></struct></value></param></params></methodCall>";

            if (!StandardProfile)
            {
                sSAMPScript = sSAMPScript.Replace("samp.hub.notifyAll", "samp.webhub.notifyAll");
            }

            try
            {
                result = SampMessage(Samp_hub_url, sSAMPScript);
                sMsg = $"SampAladinCmd {sScriptCmd}, {sMsg}\r\n";
            }
            catch (Exception e)
            {
                sMsg = $"SampScript ERROR {e.Message}\r\n";
                result = "exception";
            }
            return result;
        }

        private string Samp_getRegisteredClients()
        {
            string result = "";
            //string sXML = "<?xml version='1.0'?><methodCall><methodName>samp.webhub.register</methodName><params><param><value><struct><member><name>samp.name</name><value><string>EAACtrl</string></value></member></struct></value></param></params></methodCall>";
            string sSAMPGetRegClients = "<?xml version='1.0'?><methodCall><methodName>samp.hub.getRegisteredClients</methodName><params><param><value>" + SAMP_PrivateKey + "</value></param></params></methodCall>";

            if (!StandardProfile)
            {
                sSAMPGetRegClients = sSAMPGetRegClients.Replace("samp.hub.unregister", "samp.webhub.unregister");
            }

            try
            {
                result = SampMessage(Samp_hub_url, sSAMPGetRegClients);
                sMsg = $"SampGetRegClients, {sMsg}\r\n";
            }
            catch (Exception e)
            {
                sMsg = $"SAMPGetRegClients ERROR {e.Message}\r\n";
                result = "exception";
            }

            return result;
        }

        private string Samp_coord_get_sky()
        {

            if (!SAMPConnected)
            { return "NOTCONNECTED"; }

            string result = "";
            string sSAMPGetCoordPoint = "<?xml version='1.0'?><methodCall><methodName>samp.hub.notify</methodName><params>";
            sSAMPGetCoordPoint += "<param><value>" + SAMP_PrivateKey + "</value></param><param><value>c1</value></param>";
            sSAMPGetCoordPoint += "<param><value><struct><member><name>samp.mtype</name><value>coord.get.sky</value></member>";
            sSAMPGetCoordPoint += "</struct></value></param></params></methodCall>";

            if (!StandardProfile)
            {
                sSAMPGetCoordPoint = sSAMPGetCoordPoint.Replace("samp.hub.notifyAll", "samp.webhub.notifyAll");
            }

            try
            {
                result = SampMessage(Samp_hub_url, sSAMPGetCoordPoint);
                sMsg = $"SampGetCoordPoint, {sMsg}\r\n";
            }
            catch (Exception e)
            {
                sMsg = $"SampGetCoordPoint ERROR {e.Message}\r\n";
                result = "exception";
            }

            return result;
        }

    }
}
