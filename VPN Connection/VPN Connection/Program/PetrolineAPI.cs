using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VPN_Connection {
    /*
    http://localhost:7699/vpnstatus&state=0&nexttry=2000
    http://localhost:7699/vpnstatus&state=1
     */

    class PetrolineAPI {
        private static string   URL         = "http://localhost:7699/vpnstatus",
                                STATE       = "&state=",
                                NEXT_TRY    = "&nexttry=";
       public string fire(int _state = 0, int _nextTry = 2000) {
            int state   = _state,
                nextTry = _nextTry;
            string url = URL + STATE + state + NEXT_TRY + nextTry;
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            
            return null;
       }
    }
}
