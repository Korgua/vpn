using DotRas;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace VPN_Connection {
    public class vpn {
        public vpnData vpnData = new vpnData();
        private logging logging = new logging();
        private string path = AppDomain.CurrentDomain.BaseDirectory + @".\\vpn.pbk";
        public string error { get; set; }

        private RasPhoneBook createPhoneBook() {
            string ip = resolveIP(vpnData.host).ToString();
            if(File.Exists(path)) {
                try {
                    System.IO.File.Delete(path);
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Remove old phonebook] success"));
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Remove old phonebook] Exception found: {0}", e.Message));
                    return null;
                }
            }
            try {
                using(FileStream fs = File.Create(path)) {
                    fs.Close();
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Create Phonebook] success"));
                }
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Create Phonebook] Exception found: {0}", e.Message));
                return null;
            }

            RasPhoneBook book = new RasPhoneBook();
            try {
                book.Open(path); //Define book path
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Open Phonebook] Success"));
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Open Phonebook] Exception found: {0}", e.Message));
                return null;
            }
            RasDevice device = RasDevice.GetDevices().Where(o => o.Name.Contains("PPTP")).FirstOrDefault();
            if(device == null) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Device] Useable device for VPN not found"));

            }
            RasEntry entry = RasEntry.CreateVpnEntry(vpnData.entryName, ip, RasVpnStrategy.PptpFirst, device);
            try {
                book.Entries.Add(entry);
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Write to Phonebook] Success"));
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Add phonebook entry] Exception found: {0}", e.Message));
                return null;
            }
            return book;
        }

        public void Dialer() {
            logging.writeToLog(null, String.Format("Dialer] Begin"));
            RasPhoneBook book;
            if(testInternetConnection() && (book = createPhoneBook()) != null) {
                string ip = resolveIP(vpnData.host).ToString();
                RasDialer dialer = new RasDialer();
                dialer.PhoneBookPath = book.Path;
                dialer.Credentials = new NetworkCredential(vpnData.username, vpnData.password);
                dialer.EntryName = vpnData.entryName;
                try {
                    //dialer.Dial();
                    dialer.DialAsync();
                    logging.writeToLog(null, String.Format("Dialer] Success"));
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[Dialer][Exception] {0}", e.Message));
                }
            }
            book = null;
            logging.writeToLog(null, String.Format("Dialer] End"));
        }

        public void disconnectPPTP() {
            RasConnection conn;
            if((conn = getConnectionStatus()) != null) {
                conn.HangUp(true);
                logging.writeToLog(null, String.Format("[disconnectPPTP] Disconnect Success"));
            }
            conn = null;
        }

        public RasConnection getConnectionStatus() {
            RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == vpnData.entryName).FirstOrDefault();
            if(conn != null) {
                logging.writeToLog(null, String.Format("[getConnectionStatus] VPN connection is active"));
                return conn;
            }
            logging.writeToLog(null, String.Format("[getConnectionStatus] No active VPN connection"));
            return null;
        }

        public IPAddress resolveIP(string host) {
            logging.writeToLog(null, String.Format("[resolveIP] Host: {0}",host));
            try {
                using(Ping Ping = new Ping()) {
                    PingReply PingReply = Ping.Send(host);
                    if(PingReply.Status == IPStatus.Success) {
                        logging.writeToLog(null, String.Format("[resolveIP] Host found, IP address: {0}",PingReply.Address));
                        return PingReply.Address;
                    }
                }
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[resolveIP][Exception] {0} is unreachable: {1}", host, e.Message));
            }
            return null;
        }

        public bool testInternetConnection(bool checkInnerNetwork = false) {
            logging.writeToLog(null, String.Format("[testInternetConnection] Begin"));
            bool testIC = true;
            string testingError = "";
            if(resolveIP("google-public-dns-a.google.com")==null) {
                testingError = "Nincs internetkapcsolat";
                testIC = false;
            }
            if(resolveIP(vpnData.host) == null) {
                testingError = "VPN szerver nem érhető el";
                testIC = false;
            }
            if(checkInnerNetwork) {
                if (resolveIP(vpnData.test_ip) == null) {
                    testingError = "Belső hálózat nem érhető el";
                    testIC = false;
                }
            }
            error = testingError;
            logging.writeToLog(null, String.Format("[testInternetConnection] End"));
            return testIC;
        }
    }
}