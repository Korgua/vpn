using System;
//using System.Web;
using System.IO;
using System.Net;

namespace VPN_Connection {
    /*
    http://localhost:7699/vpnstatus&state=0&nexttry=2000
    http://localhost:7699/vpnstatus&state=1
     */
    
    class PetrolineAPI {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private logging logging = new logging();
        private static string   URL         = "http://localhost:7699/vpnstatus",
                                STATE       = "&state=",
                                NEXT_TRY    = "&nexttry=",
                                RSS         = "&rss="; //&rss=0|1|2;blablabla

        
       public string sendStatus(int _state = 0, int _nextTry = 2000, string _rss = "") {
            Console.WriteLine(String.Format("_state: {0}, _nextTry: {1}, _rss: {2}",_state, _nextTry, _rss));
            _rss = WebUtility.UrlEncode(_rss);
            if(_nextTry >= 1000) {
                _nextTry /= 1000;
            }
            string url = URL + STATE + _state + NEXT_TRY + _nextTry + RSS + _rss;
            try {
                logging.writeToLog(null, String.Format("[sendStatus] Sending status: {0}", url), 3);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Timeout = 1000;
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream streamResponse = httpWebResponse.GetResponseStream();
                httpWebResponse.Close();
                using(StreamReader streamReader = new StreamReader(streamResponse)) {
                    return streamReader.ReadToEnd();
                }
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[sendStatus] Exception error: {0} --> {1}",e.Message, url), 1);
            }
            return null;
       }
    private void sendNextTry() {

    }
    }
}
