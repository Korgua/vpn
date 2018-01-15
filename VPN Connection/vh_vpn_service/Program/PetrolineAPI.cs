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
        private static string URL = "http://localhost:7699/vpnstatus",
                                STATE = "&state=",
                                NEXT_TRY = "&nexttry=",
                                RSS = "&rss="; //&rss=0|1|2;blablabla


        public string SendStatus(int _state = 0, int _nextTry = 0, string _rss = "") {
            if (_nextTry >= 1000) {
                _nextTry /= 1000;
            }
            string url = URL + STATE + _state;
            if (_nextTry != 0) {
                url += (NEXT_TRY + _nextTry);
            }
            if (_rss != "") {
                /*byte[] bytes = Encoding.Default.GetBytes(_rss);
                _rss = Encoding.UTF8.GetString(bytes);*/
                _rss = WebUtility.UrlEncode(_rss);
                url += (RSS + _rss);
            }
            logging.writeToLog(null, String.Format("[SendStatus] Sending status: {0}", url), 3);
            try {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.KeepAlive = true;
                //httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Timeout = 2000;
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream streamResponse = httpWebResponse.GetResponseStream();
                httpWebResponse.Close();
                using (StreamReader streamReader = new StreamReader(streamResponse)) {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception e) {
                logging.writeToLog(null, String.Format("[SendStatus] Exception error: {0} --> {1}", e.Message, url), 1);
            }
            return null;
        }
    }
}
