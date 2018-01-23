using System;
//using System.Web;
using System.IO;
using System.Net;
using System.Text;

namespace vh_vpn {
    /*
    http://localhost:7699/vpnstatus&state=0&nexttry=2000
    http://localhost:7699/vpnstatus&state=1
     */

    class PetrolineAPI {
        private logging logging = new logging();
        private static string URL = "http://localhost:7698/vpnstatus",
                                STATE = "&state=",
                                NEXT_TRY = "&nexttry=",
                                RSS = "&rss={rnd}"; //&rss=0|1|2;blablabla


        public string SendStatus(int _state = 0, int _nextTry = -1, string _rss = "") {
            if (_nextTry >= 1000) {
                _nextTry /= 1000;
            }
            string url = URL + STATE + _state;
            if (_nextTry != -1) {
                url += (NEXT_TRY + _nextTry);
            }
            if (_rss != "") {
                _rss = "2;" + _rss;
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;
                byte[] utfBytes = utf8.GetBytes(_rss);
                byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                string msg = iso.GetString(isoBytes);
                _rss = WebUtility.UrlEncode(_rss);
                url += (RSS + _rss);
            }
            logging.writeToLog(null, String.Format("[SendStatus] Sending status: {0}", url), 3);
            try {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Timeout = 2500;
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream streamResponse = httpWebResponse.GetResponseStream();
                using (StreamReader streamReader = new StreamReader(streamResponse)) {
                    string a = streamReader.ReadToEnd();
                    httpWebResponse.Close();
                    return a;
                }
            }
            catch (Exception e) {
                logging.writeToLog(null, String.Format("[SendStatus] Exception error: {0} --> {1}", e.Message, url), 1);
            }
            return null;
        }
    }
}
