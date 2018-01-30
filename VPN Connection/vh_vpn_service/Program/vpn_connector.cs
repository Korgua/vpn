using DotRas;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace vh_vpn {
    public class VPN_connector {
        private CONSTANTS CONSTANTS = new CONSTANTS("@VPN_connector");
        private logging logging = new logging();
        private string path = AppDomain.CurrentDomain.BaseDirectory + @".\\vpn.pbk";
        public string resolveError { get; set; }
        public string connectError { get; set; }
        public bool vpn_connected = false;
        ServiceController sc = new ServiceController("vh_vpn_service");

        private RasPhoneBook createPhoneBook() {
            string ip;
            if ((ip = resolveIP(CONSTANTS.host)) == null){
                logging.writeToLog(null, String.Format("[createPhoneBook] {0} not found",ip),1);
                return null;
            }
            if(ip.Contains("http://")) {
                logging.writeToLog(null, String.Format("[createPhoneBook] {0} is not valid vpn hostname", ip),1);
                ip = ip.Replace("http://", "");
            }
            if(File.Exists(path)) {
                try {
                    System.IO.File.Delete(path);
                    logging.writeToLog(null, String.Format("[createPhoneBook][Remove old phonebook] success"), 3);
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[createPhoneBook][Remove old phonebook] Exception found: {0}", e.Message),0);
                    return null;
                }
            }
            try {
                using(FileStream fs = File.Create(path)) {
                    fs.Close();
                    logging.writeToLog(null, String.Format("[createPhoneBook] success", 2));
                }
            }
            catch(Exception e) {
                connectError = "Can't create phonebook at" + path;
                logging.writeToLog(null, String.Format("[createPhoneBook] Exception found: {0}", e.Message),0);
                return null;
            }

            RasPhoneBook book = new RasPhoneBook();
            try {
                book.Open(path);
                logging.writeToLog(null, String.Format("[createPhoneBook][Open Phonebook] Success"),2);
            }
            catch(Exception e) {
                connectError = "Can't open phonebook at" + path;
                logging.writeToLog(null, String.Format("[createPhoneBook][Open Phonebook] Exception found: {0}", e.Message),0);
                return null;
            }
            RasDevice device = RasDevice.GetDevices().Where(o => o.Name.Contains("PPTP")).FirstOrDefault();
            if(device == null) {
                connectError = "Useable device for VPN not found";
                logging.writeToLog(null, String.Format("[createPhoneBook][Device] Useable device for VPN not found"),1);
                return null;

            }
            RasEntry entry = RasEntry.CreateVpnEntry(CONSTANTS.entryName, ip, RasVpnStrategy.PptpFirst, device);
            try {
                book.Entries.Add(entry);
                logging.writeToLog(null, String.Format("[createPhoneBook][Write to Phonebook] Success"),2);
            }
            catch(Exception e) {
                connectError = String.Format("Can't add the following entry to the phonebook: {0}",entry);
                logging.writeToLog(null, String.Format("[createPhoneBook][Add phonebook entry] Exception found: {0}", e.Message),0);
                return null;
            }
            return book;
        }

        public void Dialer() {
            logging.writeToLog(null, String.Format("[Dialer] Begin"),3);

            RasPhoneBook book;
            if((book = createPhoneBook()) != null ) {
                RasDialer dialer;
                try {
                     dialer = new RasDialer {
                        PhoneBookPath = book.Path,
                        Credentials = new NetworkCredential(CONSTANTS.username, CONSTANTS.password),
                        EntryName = CONSTANTS.entryName,
                        Timeout = 5000
                    };
                    dialer.DialCompleted += (sender, args) => {
                        logging.writeToLog(null, String.Format("[Dialer] DialCompleted"), 2);
                    };
                    dialer.Error += (sender, args) => {
                        logging.writeToLog(null, String.Format("[Dialer] DialError"), 2);
                    };
                    dialer.StateChanged += (sender, args) => {
                        logging.writeToLog(null, String.Format("[Dialer] dialer.StateChanged: {0}", args.State), 1);
                    };
                    dialer.Dial();
                    vpn_connected = true;
                    logging.writeToLog(null, String.Format("[Dialer] Success"), 2);
                }
                catch (Exception e) {
                    connectError = String.Format("Can't connect because: {0}", e.Message);
                    logging.writeToLog(null, String.Format("[Dialer][Exception] {0}", e.Message),0);
                }
            }
            book = null;
            if(connectError != null) {
                logging.writeToLog(null, String.Format("[Dialer] There was an error while dialing: {0}",connectError), 1);
            }
            logging.writeToLog(null, String.Format("[Dialer] End"),3);
        }

        public void disconnectPPTP() {
                RasConnection conn;
                if((conn = getConnectionStatus()) != null) {
                    conn.HangUp();
                    logging.writeToLog(null, String.Format("[disconnectPPTP] Disconnect Success"), 1);
                    conn = null;
                }
        }

        public RasConnection getConnectionStatus() {
            RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == CONSTANTS.entryName).FirstOrDefault();
            if(conn != null) {
                connectError = null;
                logging.writeToLog(null, String.Format("[getConnectionStatus] VPN connection is active"),2);
                return conn;
            }
            logging.writeToLog(null, String.Format("[getConnectionStatus] No active VPN connection"),1);
            return null;
        }

        public string resolveIP(string host) {
            if(host != CONSTANTS.host) {
                logging.writeToLog(null, String.Format("[resolveIP] Trying to resolve {0} with Ping", host), 3);
                using(Ping Ping = new Ping()) {
                    try {
                        PingReply PingReply = Ping.Send(host, 2500);
                        if (PingReply.Status == IPStatus.Success) {
                            logging.writeToLog(null, String.Format("[resolveIP] {0} found via ping, IP address: {1}", host, PingReply.Address), 2);
                            return PingReply.Address.ToString();
                        }
                        else {
                            logging.writeToLog(null, String.Format("[resolveIP] {0} is unreachable with PING", host),1);
                            return null;
                        }
                    }
                    catch(Exception e_) {
                        logging.writeToLog(null, String.Format("[resolveIP][Exception] {0} is unreachable with PING: {1}", host, e_.Message),0);
                    }
                }
            }
            else {
                try {
                    string _host = "http://" + host;
                    logging.writeToLog(null, String.Format("[resolveIP] Trying to resolve {0} with HttpWebSocket", _host), 3);
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_host);
                    httpWebRequest.Timeout = 2500;
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    if(httpWebResponse.StatusCode == HttpStatusCode.OK) {
                        logging.writeToLog(null, String.Format("[resolveIP] {0} found via HttpWebRequest", _host), 2);
                        httpWebResponse.Close();
                        return host;
                    }
                    else {
                        httpWebResponse.Close();
                        logging.writeToLog(null, String.Format("[resolveIP]{0} is unreachable with HttpWebRequest", host),1);
                        return null;
                    }
                }
                catch(Exception e_) {
                    logging.writeToLog(null, String.Format("[resolveIP][Exception] {0} is unreachable with HttpWebRequest: {1}", host, e_.Message),0);
                }
            }
            return null;
        }

        public bool testInternetConnection(bool checkInnerNetwork = false) {
            logging.writeToLog(null, String.Format("[testInternetConnection] Begin"),3);
            if(resolveIP("google-public-dns-a.google.com")==null) {
                if(resolveIP("google-public-dns-b.google.com") == null) {
                    resolveError = "Nincs internetkapcsolat";
                    return false;
                }
            }
            if(resolveIP(CONSTANTS.host) == null) {
                resolveError = "VPN szerver nem érhető el";
                return false;
            }
            if(checkInnerNetwork) {
                if (resolveIP(CONSTANTS.test_ip) == null) {
                    resolveError = "Belső hálózat nem érhető el";
                    return false;
                }
            }
            logging.writeToLog(null, String.Format("[testInternetConnection] End"),3);
            resolveError = null;
            return true;
        }
    }
}