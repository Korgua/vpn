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
        private RasConnectionStatus Rsc;
        private string path = AppDomain.CurrentDomain.BaseDirectory + @".\\vpn.pbk";
        public string error { get; set; }
        public bool vpn_connected = false;

        private RasPhoneBook createPhoneBook() {
            string ip;
            if ((ip = resolveIP(vpnData.host).ToString()) == null){
                logging.writeToLog(null, String.Format("[createPhoneBook] {0} not found",vpnData.host));
                return null;
            }
            if(File.Exists(path)) {
                try {
                    System.IO.File.Delete(path);
                    logging.writeToLog(null, String.Format("[createPhoneBook][Remove old phonebook] success", 3));
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[createPhoneBook][Remove old phonebook] Exception found: {0}", e.Message));
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
                logging.writeToLog(null, String.Format("[createPhoneBook] Exception found: {0}", e.Message));
                return null;
            }

            RasPhoneBook book = new RasPhoneBook();
            try {
                book.Open(path);
                logging.writeToLog(null, String.Format("[createPhoneBook][Open Phonebook] Success"),2);
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[createPhoneBook][Open Phonebook] Exception found: {0}", e.Message));
                return null;
            }
            RasDevice device = RasDevice.GetDevices().Where(o => o.Name.Contains("PPTP")).FirstOrDefault();
            if(device == null) {
                logging.writeToLog(null, String.Format("[createPhoneBook][Device] Useable device for VPN not found"),1);

            }
            RasEntry entry = RasEntry.CreateVpnEntry(vpnData.entryName, ip, RasVpnStrategy.PptpFirst, device);
            try {
                book.Entries.Add(entry);
                logging.writeToLog(null, String.Format("[createPhoneBook][Write to Phonebook] Success"),2);
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[createPhoneBook][Add phonebook entry] Exception found: {0}", e.Message));
                return null;
            }
            return book;
        }

        public void Dialer() {
            logging.writeToLog(null, String.Format("[Dialer] Begin"),3);

            RasPhoneBook book;
            if((book = createPhoneBook()) != null) {
                try {
                    string ip = resolveIP(vpnData.host).ToString();
                    RasDialer dialer = new RasDialer();
                    dialer.PhoneBookPath = book.Path;
                    dialer.Credentials = new NetworkCredential(vpnData.username, vpnData.password);
                    dialer.EntryName = vpnData.entryName;
                    dialer.DialCompleted += (sender, args) => {
                        logging.writeToLog(null, String.Format("[Dialer] DialCompleted"), 2);
                    };
                    dialer.Error += (sender, args) => {
                        logging.writeToLog(null, String.Format("[Dialer] DialError"), 2);
                    };
                    dialer.DialAsync();
                    //dialer.Dial();
                    vpn_connected = true;
                    logging.writeToLog(null, String.Format("[Dialer] Success"), 2);
                }
                catch (Exception e) {
                    logging.writeToLog(null, String.Format("[Dialer][Exception] {0}", e.Message));
                }
            }
            book = null;
            logging.writeToLog(null, String.Format("[Dialer] End"),3);
        }

        public void disconnectPPTP() {
            BackgroundWorker BW = new BackgroundWorker();
            BW.DoWork += (sender, args) => {
                RasConnection conn;
                if((conn = getConnectionStatus()) != null) {
                    conn.HangUp(true);
                    logging.writeToLog(null, String.Format("[disconnectPPTP] Disconnect Success"), 1);
                    conn = null;
                }
            };
            BW.RunWorkerCompleted += (sender, args) => {
                vpn_connected = false;
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
                    try {
                        PingReply PingReply = Ping.Send(host);
                        if (PingReply.Status == IPStatus.Success) {
                            logging.writeToLog(null, String.Format("[resolveIP] {0} found, IP address: {1}", host, PingReply.Address), 2);
                            return PingReply.Address;
                        }
                    }
                    catch(Exception e) {
                        logging.writeToLog(null, String.Format("[resolveIP][Exception] Ping {0} thrown an exception: {1}", host, e.Message));
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