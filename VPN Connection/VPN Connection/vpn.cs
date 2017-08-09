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
        public string error;

        public bool connectPPTP() {
            testInternetConnection();
            string ip = null;
            try {
                Uri uri = new Uri(vpnData.host);
                logging.writeToLog(null, String.Format("[ConnectToPPTP][VPN_HOST] {0}", vpnData.host));
                try {
                    ip = Dns.GetHostAddresses(uri.Host)[0].ToString();
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][GetHostAddresses] {0}", ip));
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][GetHostAddresses] {0} --> Exception found: {1}", vpnData.host, e.Message));
                    Console.WriteLine(e.Message);
                    error = "A VPN szerver nem érhető el";
                }
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][VPN_HOST] {0} --> Exception found: {1}", vpnData.host, e.Message));
                Console.WriteLine(e.Message);
                return false;
            }
            String path = AppDomain.CurrentDomain.BaseDirectory + @".\\vpn.pbk";
            if(System.IO.File.Exists(path)) {
                try {
                    System.IO.File.Delete(path);
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Remove old phonebook] success"));
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Remove old phonebook] Exception found: {0}", e.Message));
                    return false;
                }
            }
            try {
                using(FileStream fs = System.IO.File.Create(path)) {
                    fs.Close();
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][Create Phonebook] success"));
                }
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Create Phonebook] Exception found: {0}", e.Message));
                Console.WriteLine(e.Message);
                return false;
            }

            RasPhoneBook book = new RasPhoneBook();
            try {
                book.Open(path); //Define book path
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Open Phonebook] Success"));
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Open Phonebook] Exception found: {0}", e.Message));
                Console.WriteLine(e.Message);
                return false;
            }
            RasDevice device = RasDevice.GetDevices().Where(o => o.Name.Contains("PPTP")).FirstOrDefault();
            if(device == null) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Device] Useable device for VPN not found"));
                Console.WriteLine(String.Format("[ConnectToPPTP][Device] Useable device for VPN not found"));

            }
            RasEntry entry = RasEntry.CreateVpnEntry(vpnData.entryName, ip, RasVpnStrategy.PptpFirst, device);
            try {
                book.Entries.Add(entry);
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Write to Phonebook] Success"));
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Add phonebook entry] Exception found: {0}", e.Message));
                Console.WriteLine(String.Format("[ConnectToPPTP][Add phonebook entry] Exception found: {0}", e.Message));
                return false;
            }

            RasDialer dialer = new RasDialer();
            dialer.PhoneBookPath = book.Path;
            dialer.Credentials = new NetworkCredential(vpnData.username, vpnData.password);
            dialer.EntryName = vpnData.entryName;
            try {
                //dialer.Dial();
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Dial] Success"));
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[ConnectToPPTP][Dial] Exception found: {0}", e.Message));
                Console.WriteLine(String.Format("[ConnectToPPTP][Dial] Exception found: {0}", e.Message));
                return false;
            }
            return true;
        }

        public void disconnectPPTP() {
            RasConnection conn = getConnectionStatus();
            if(conn != null) {
                conn.HangUp();
                logging.writeToLog(null, String.Format("[disconnectPPTP] Disconnect Success"));
            }
            logging.writeToLog(null, String.Format("[disconnectPPTP] No active VPN connection"));
            Console.WriteLine(String.Format("[disconnectPPTP] No active VPN connection"));
        }

        public RasConnection getConnectionStatus() {
            RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == vpnData.entryName).FirstOrDefault();
            if(conn != null) {
                return conn;
            }
            return null;
        }

        public IPAddress resolveIP(string host) {
            try {
                using(Ping Ping = new Ping()) {
                    PingReply PingReply = Ping.Send(host);
                    if(PingReply.Status == IPStatus.Success)
                        return PingReply.Address;
                }
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[testInternetConnection][Exception] {0} is unreachable: {1}", host, e.Message));
            }
            return null;
        }

        public bool testInternetConnection() {
            logging.writeToLog(null, String.Format("[testInternetConnection] Begin"));
            bool pingable = false;
            Ping Ping = null;
            PingReply PingReply = null;
            try {
                string PingAddress = "google-public-dns-a.google.com";
                Ping = new Ping();
                PingReply = Ping.Send(PingAddress);
                pingable = (PingReply.Status == IPStatus.Success);
                logging.writeToLog(null, String.Format("[testInternetConnection] {0} is alive: {1}", PingAddress, PingReply.Address));
                try {
                    Ping = new Ping();
                    PingReply = Ping.Send(vpnData.host);
                    pingable = (PingReply.Status == IPStatus.Success);
                    logging.writeToLog(null, String.Format("[testInternetConnection] {0} is alive: {1}",vpnData.host, PingReply.Address));
                }
                catch(Exception e) {
                    logging.writeToLog(null, String.Format("[testInternetConnection] Petrolcard is unreachable"));
                    error = "VPN szerver nem érhető el";
                }
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[testInternetConnection] Google is unreachable"));
                error = "Nincs internetkapcsolat";
            }
            return pingable;
        }
    }
}