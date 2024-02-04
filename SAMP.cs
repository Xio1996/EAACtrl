using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EAACtrl
{
    internal class SAMP
    {
        public bool StandardProfile = false;
        public bool SAMPConnected = false;
        public string SAMP_PrivateKey = "";
        public string Samp_hub_url = @"http://127.0.0.1:21012"; //Web Profile

        private string sMsg = "";

        public string Message
        {
            get
            {
                return sMsg;
            }
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

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(Samp_hub_url, "POST", sSAMPRegister);

                if (result != "")
                {
                    int iStart = 0, iEnd = 0, iPos = 0;
                    iPos = result.IndexOf("samp.private-key", 0);
                    iStart = result.IndexOf("<value>", iPos) + 7;
                    iEnd = result.IndexOf("</value>", iStart);

                    result = result.Substring(iStart, iEnd - iStart);

                    SAMP_PrivateKey = result;

                    TimeSpan ts = stopwatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    sMsg = "SampRegister: key=" + result + " (" + elapsedTime + ")\r\n";

                    SAMPConnected = true;

                }
                else
                {
                    sMsg = "SampRegister: Can't Connect!\r\n";
                }
            }
            catch (Exception e)
            {
                sMsg = "SampRegister ERROR " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient?.Dispose();
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

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(Samp_hub_url, "POST", sSAMPDisconnect);

                SAMPConnected = false;

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SampDisconnect (" + elapsedTime + ")\r\n";
            }
            catch (Exception e)
            {
                sMsg = "SetAction ERROR " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
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

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(Samp_hub_url, "POST", sSAMPMetaData);

                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SampMetaData (" + elapsedTime + ")\r\n";

            }
            catch (Exception e)
            {
                sMsg = "SampMetaData " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
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

            WebClient lwebClient = new WebClient();

            if (!StandardProfile)
            {
                sSAMPCoordPointAt = sSAMPCoordPointAt.Replace("samp.hub.notifyAll", "samp.webhub.notifyAll");
            }

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(Samp_hub_url, "POST", sSAMPCoordPointAt);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SampCoordPointAt " + RA + "," + Dec + " (" + elapsedTime + ")\r\n";

            }
            catch (Exception e)
            {
                sMsg = "SampCoordPointAt " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        public string Samp_script_aladin_send(string sScriptCmd)
        {

            if (!SAMPConnected)
            { return "NOTCONNECTED"; }

            string result = "";
            string sSAMPCoordPointAt = "<?xml version='1.0'?><methodCall><methodName>samp.hub.notifyAll</methodName><params>";
            sSAMPCoordPointAt += "<param><value>" + SAMP_PrivateKey + "</value></param>";
            sSAMPCoordPointAt += "<param><value><struct><member><name>samp.mtype</name><value>script.aladin.send</value></member><member><name>samp.params</name><value><struct>";
            sSAMPCoordPointAt += "<member><name>script</name><value>" + sScriptCmd + "</value></member>";
            sSAMPCoordPointAt += "</struct></value></member></struct></value></param></params></methodCall>";

            WebClient lwebClient = new WebClient();

            if (!StandardProfile)
            {
                sSAMPCoordPointAt = sSAMPCoordPointAt.Replace("samp.hub.notifyAll", "samp.webhub.notifyAll");
            }

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(Samp_hub_url, "POST", sSAMPCoordPointAt);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg ="SampAladinCmd " + sScriptCmd + " (" + elapsedTime + ")\r\n";

            }
            catch (Exception e)
            {
                sMsg = "SampCoordPointAt " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }

            return result;
        }

        private string Samp_getRegisteredClients()
        {
            string result = "";
            //string sXML = "<?xml version='1.0'?><methodCall><methodName>samp.webhub.register</methodName><params><param><value><struct><member><name>samp.name</name><value><string>EAACtrl</string></value></member></struct></value></param></params></methodCall>";
            string sSAMPDisconnect = "<?xml version='1.0'?><methodCall><methodName>samp.hub.getRegisteredClients</methodName><params><param><value>" + SAMP_PrivateKey + "</value></param></params></methodCall>";

            if (!StandardProfile)
            {
                sSAMPDisconnect = sSAMPDisconnect.Replace("samp.hub.unregister", "samp.webhub.unregister");
            }

            WebClient lwebClient = new WebClient();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(Samp_hub_url, "POST", sSAMPDisconnect);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                sMsg = "SampDisconnect (" + elapsedTime + ")\r\n";
            }
            catch (Exception e)
            {
                sMsg = "SetAction ERROR " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

        private string Samp_coord_get_sky()
        {

            if (!SAMPConnected)
            { return "NOTCONNECTED"; }

            string result = "";
            string sSAMPCoordPointAt = "<?xml version='1.0'?><methodCall><methodName>samp.hub.notify</methodName><params>";
            sSAMPCoordPointAt += "<param><value>" + SAMP_PrivateKey + "</value></param><param><value>c1</value></param>";
            sSAMPCoordPointAt += "<param><value><struct><member><name>samp.mtype</name><value>coord.get.sky</value></member>";
            sSAMPCoordPointAt += "</struct></value></param></params></methodCall>";

            WebClient lwebClient = new WebClient();

            if (!StandardProfile)
            {
                sSAMPCoordPointAt = sSAMPCoordPointAt.Replace("samp.hub.notifyAll", "samp.webhub.notifyAll");
            }

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                lwebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                result = lwebClient.UploadString(Samp_hub_url, "POST", sSAMPCoordPointAt);

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                //sMsg = "SampAladinCmd " + sScriptCmd + " (" + elapsedTime + ")\r\n";
            }
            catch (Exception e)
            {
                sMsg = "SampCoordPointAt " + e.Message + "\r\n";
                result = "exception";
            }
            finally
            {
                lwebClient.Dispose();
            }
            return result;
        }

    }
}
