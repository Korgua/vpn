using DotRas;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace vh_vpn {
    public class VPN_connector {
        private CONSTANTS CONSTANTS = new CONSTANTS("@VPN_connector");
        private Logging logging = new Logging();
        private string path = AppDomain.CurrentDomain.BaseDirectory + @".\\vpn.pbk";
        public string ResolveError { get; set; }
        public string ConnectError { get; set; }


        /// <summary>
        /// Creating a RasDial phonebook to dial it
        /// </summary>
        private RasPhoneBook CreatePhoneBook() {
            string ip;
            if ((ip = ResolveIP(CONSTANTS.Host,"vpn server")) == null) {
                logging.WriteToLog(null, String.Format("[createPhoneBook] {0} not found", ip), 1);
                return null;
            }
            if (ip.Contains("http://")) {
                logging.WriteToLog(null, String.Format("[createPhoneBook] {0} is not valid vpn hostname", ip), 1);
                ip = ip.Replace("http://", "");
            }
            if (File.Exists(path)) {
                try {
                    System.IO.File.Delete(path);
                    logging.WriteToLog(null, String.Format("[createPhoneBook][Remove old phonebook] success"), 3);
                }
                catch (Exception e) {
                    logging.WriteToLog(null, String.Format("[createPhoneBook][Remove old phonebook] Exception found: {0}", e.Message), 0);
                    return null;
                }
            }
            try {
                using (FileStream fs = File.Create(path)) {
                    fs.Close();
                    logging.WriteToLog(null, String.Format("[createPhoneBook] success", 2));
                }
            }
            catch (Exception e) {
                ConnectError = "Can't create phonebook at" + path;
                logging.WriteToLog(null, String.Format("[createPhoneBook] Exception found: {0}", e.Message), 0);
                return null;
            }

            RasPhoneBook book = new RasPhoneBook();
            try {
                book.Open(path);
                logging.WriteToLog(null, String.Format("[createPhoneBook][Open Phonebook] Success"), 2);
            }
            catch (Exception e) {
                ConnectError = "Can't open phonebook at" + path;
                logging.WriteToLog(null, String.Format("[createPhoneBook][Open Phonebook] Exception found: {0}", e.Message), 0);
                return null;
            }
            RasDevice device = RasDevice.GetDevices().Where(o => o.Name.Contains("PPTP")).FirstOrDefault();
            if (device == null) {
                ConnectError = "Useable device for VPN not found";
                logging.WriteToLog(null, String.Format("[createPhoneBook][Device] Useable device for VPN not found"), 1);
                return null;

            }
            RasEntry entry = RasEntry.CreateVpnEntry(CONSTANTS.EntryName, ip, RasVpnStrategy.PptpFirst, device);
            try {
                book.Entries.Add(entry);
                logging.WriteToLog(null, String.Format("[createPhoneBook][Write to Phonebook] Success"), 2);
            }
            catch (Exception e) {
                ConnectError = String.Format("Can't add the following entry to the phonebook: {0}", entry);
                logging.WriteToLog(null, String.Format("[createPhoneBook][Add phonebook entry] Exception found: {0}", e.Message), 0);
                return null;
            }
            return book;
        }


        /// <summary>
        /// Dialing the vpn connection with specified parameters
        /// </summary>
        public void Dialer() {
            logging.WriteToLog(null, String.Format("[Dialer] Begin"), 3);
            RasPhoneBook book;
            if ((book = CreatePhoneBook()) != null) {
                RasDialer dialer;
                try {
                    dialer = new RasDialer {
                        PhoneBookPath = book.Path,
                        Credentials = new NetworkCredential(CONSTANTS.Username, CONSTANTS.Password),
                        EntryName = CONSTANTS.EntryName,
                        Timeout = 5000
                    };
                    dialer.DialCompleted += (sender, args) => {
                        logging.WriteToLog(null, String.Format("[Dialer] DialCompleted"), 2);
                    };
                    dialer.Error += (sender, args) => {
                        logging.WriteToLog(null, String.Format("[Dialer] DialError"), 2);
                    };
                    dialer.StateChanged += (sender, args) => {
                        logging.WriteToLog(null, String.Format("[Dialer] dialer.StateChanged: {0}", args.State), 1);
                    };
                    dialer.Dial();
                    logging.WriteToLog(null, String.Format("[Dialer] Success"), 2);
                }
                catch (Exception e) {
                    ConnectError = String.Format("Can't connect because: {0}", e.Message);
                    logging.WriteToLog(null, String.Format("[Dialer][Exception] {0}", e.Message), 0);
                }
            }
            book = null;
            if (ConnectError != null) {
                logging.WriteToLog(null, String.Format("[Dialer] There was an error while dialing: {0}", ConnectError), 1);
            }
            logging.WriteToLog(null, String.Format("[Dialer] End"), 3);
        }

        /// <summary>
        /// If there is an active vpn connection this will hang up
        /// </summary>
        public void DisconnectPPTP() {
            RasConnection conn;
            if ((conn = GetConnectionStatus()) != null) {
                conn.HangUp();
                logging.WriteToLog(null, String.Format("[disconnectPPTP] Disconnect Success"), 1);
                conn = null;
            }
        }

        /// <summary>
        /// If vpn connection is active
        /// This will return with it
        /// Else it will return as null
        /// </summary>
        public RasConnection GetConnectionStatus() {
            RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == CONSTANTS.EntryName).FirstOrDefault();
            if (conn != null) {
                ConnectError = null;
                logging.WriteToLog(null, String.Format("[getConnectionStatus] VPN connection is active"), 2);
                return conn;
            }
            logging.WriteToLog(null, String.Format("[getConnectionStatus] No active VPN connection"), 1);
            return null;
        }

        /// <summary>
        /// Testing the remote hosts and servers via socket
        /// </summary>
        /// <param name="host">Which host/server need to be checked</param>
        /// <param name="mask">Don't write the host/server address to log file, mask it</param>
        /// <param name="port">With which port need to be checked?</param>
        public string ResolveIP(string host, string mask,int port = 80) {
            using (TcpClient client = new TcpClient()) {
                logging.WriteToLog(null, String.Format("[resolveIP] Trying to resolve {0} ", mask), 3);
                IAsyncResult asyncResult = client.BeginConnect(host, port, null, null);
                System.Threading.WaitHandle WaitHandle = asyncResult.AsyncWaitHandle;
                try {
                    if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2), false)) {
                        client.Close();
                        throw new Exception();
                    }
                    client.EndConnect(asyncResult);
                    logging.WriteToLog(null, String.Format("[resolveIP] {0} found", mask), 2);
                    return host;
                }
                catch (Exception e) {
                    logging.WriteToLog(null, String.Format("[resolveIP][Exception] {0}: is unreachable: {1}", mask,e.Message), 0);
                }
                finally {
                    WaitHandle.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Testing the remote hosts and servers
        /// </summary>
        /// <param name="checkInnerNetwork">Need to check the inner network? Default is false</param>
        public bool TestInternetConnection(bool checkInnerNetwork = false) {
            logging.WriteToLog(null, String.Format("[testInternetConnection] Begin"), 3);
            if (ResolveIP("google.com","google.com") == null) {
                if (ResolveIP("facebook.com","facebook.com") == null) {
                    ResolveError = "Nincs internetkapcsolat";
                    return false;
                }
            }
            if (ResolveIP(CONSTANTS.Host,"vpn server") == null) {
                ResolveError = "VPN szerver nem érhető el";
                return false;
            }
            if (checkInnerNetwork) {
                if (ResolveIP(CONSTANTS.Test_ip,"inner network", 22) == null) {
                    ResolveError = "Belső hálózat nem érhető el";
                    return false;
                }
            }
            logging.WriteToLog(null, String.Format("[testInternetConnection] End"), 3);
            ResolveError = null;
            return true;
        }
    }
}