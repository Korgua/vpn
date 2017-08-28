using DotRas;
using System;
using System.ComponentModel;
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
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Remove old phonebook] success",3));
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Remove old phonebook] Exception found: {0}", e.Message));
                    return null;
                }
            }
            try {
                using(FileStream fs = File.Create(path)) {
                    fs.Close();
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Create Phonebook] success",2));
                }
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Create Phonebook] Exception found: {0}", e.Message));
                return null;
            }

            RasPhoneBook book = new RasPhoneBook();
            try {
                book.Open(path); //Define book path
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Open Phonebook] Success"),2);
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Open Phonebook] Exception found: {0}", e.Message));
                return null;
            }
            RasDevice device = RasDevice.GetDevices().Where(o => o.Name.Contains("PPTP")).FirstOrDefault();
            if(device == null) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Device] Useable device for VPN not found"),1);

            }
            RasEntry entry = RasEntry.CreateVpnEntry(vpnData.entryName, ip, RasVpnStrategy.PptpFirst, device);
            try {
                book.Entries.Add(entry);
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Write to Phonebook] Success"),2);
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Add phonebook entry] Exception found: {0}", e.Message));
                return null;
            }
            return book;
        }

        public void Dialer() {
            logging.writeToLog(null, String.Format("Dialer] Begin"),3);

            RasPhoneBook book;
            if((book = createPhoneBook()) != null) {
                try {
                    string ip = resolveIP(vpnData.host).ToString();
                RasDialer dialer = new RasDialer();
                dialer.PhoneBookPath = book.Path;
                dialer.Credentials = new NetworkCredential(vpnData.username, vpnData.password);
                dialer.EntryName = vpnData.entryName;
                
                    dialer.Dial();        
                    //dialer.DialAsync();            
                    logging.writeToLog(null, String.Format("[Dialer] Success"),2);
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[Dialer][Exception] {0}", e.Message));
                }
            }
            book = null;
            logging.writeToLog(null, String.Format("[Dialer] End"),3);
        }

        public void disconnectPPTP() {
            BackgroundWorker BW = new BackgroundWorker();
            BW.DoWork += (_sender, _args) => {
                RasConnection conn;
                if((conn = getConnectionStatus()) != null) {
                    conn.HangUp(true);
                    logging.writeToLog(null, String.Format("[disconnectPPTP] Disconnect Success"), 2);
                }
                conn = null;
            };
            BW.RunWorkerCompleted += (_sender, _args) => {
                BW.Dispose();
            };
            BW.RunWorkerAsync();
        }

        public RasConnection getConnectionStatus() {
            RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == vpnData.entryName).FirstOrDefault();
            if(conn != null) {
                logging.writeToLog(null, String.Format("[getConnectionStatus] VPN connection is active"),2);
                return conn;
            }
            logging.writeToLog(null, String.Format("[getConnectionStatus] No active VPN connection"),1);
            return null;
        }

        public IPAddress resolveIP(string host) {
            try {
                using(Ping Ping = new Ping()) {
                    PingReply PingReply = Ping.Send(host);
                    if(PingReply.Status == IPStatus.Success) {
                        logging.writeToLog(null, String.Format("[resolveIP] {0} found, IP address: {1}", host, PingReply.Address), 2);
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
            logging.writeToLog(null, String.Format("[testInternetConnection] Begin"),3);
            if(resolveIP("google-public-dns-a.google.com")==null) {
                error = "Nincs internetkapcsolat";
                return false;
            }
            if(resolveIP(vpnData.host) == null) {
                error = "VPN szerver nem érhető el";
                return false;
            }
            if(checkInnerNetwork) {
                if (resolveIP(vpnData.test_ip) == null) {
                    error = "Belső hálózat nem érhető el";
                    return false;
                }
            }
            logging.writeToLog(null, String.Format("[testInternetConnection] End"),3);
            error = "";
            return true;
        }
    }
}