using DotRas;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace VPN_Connection {
    public class vpn {
        private RasDialer dialer { get; set; }// = new RasDialer();
        private vpnData vpnData = new vpnData();
        private logging logging = new logging();

        public void connectPPTP() {
            bool error = false;
            string ip = null;
            try {
                Uri uri = new Uri(vpnData.host);
                logging.writeToLog(null, String.Format("[ConnectToPPTP][VPN_HOST] {0}",vpnData.host));
                try {
                    ip = Dns.GetHostAddresses(uri.Host)[0].ToString();
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][GetHostAddresses] {0}", ip));
                }
                catch (Exception e) {
                    error = true;
                    logging.writeToLog(null, String.Format("[ConnectToPPTP][GetHostAddresses] {0} --> Exception found: {1}", vpnData.host, e.Message));
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e) {
                error = true;
                logging.writeToLog(null, String.Format("[ConnectToPPTP][VPN_HOST] {0} --> Exception found: {1}",vpnData.host, e.Message));
                Console.WriteLine(e.Message);
            }
            FileStream fs = null;
            String path = AppDomain.CurrentDomain.BaseDirectory + @".\\vpn.pbk";
            if (System.IO.File.Exists(path)) {
                System.IO.File.Delete(path);
            }
            fs = System.IO.File.Create(path);
            fs.Close();

            RasPhoneBook book = new RasPhoneBook();
            book.Open(path); //Define book path
            bool first = true;
            RasDevice device = null;
            foreach (RasDevice _device in RasDevice.GetDevices()) {
                if (_device.Name.Contains("PPTP") && _device.DeviceType.ToString().ToLower() == "vpn" && first) {

                    device = _device;
                    first = false;
                }
            }
            try {
                RasEntry entry = RasEntry.CreateVpnEntry(vpnData.entryName, ip, RasVpnStrategy.PptpFirst, device); //Prepare server details
                book.Entries.Add(entry); //Write to vpn.pbk
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }

            //Writing server information is done, now we will connect to it.

            dialer = new RasDialer();
            dialer.PhoneBookPath = book.Path; //Read server list from vpn.pbk
            dialer.Credentials = new NetworkCredential(vpnData.username, vpnData.password); //Define username and password
            dialer.EntryName = vpnData.entryName; //Get server named {entryName}
            try {
                dialer.Dial(); //Connect
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public void disconnectPPTP() {
            RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == vpnData.entryName).FirstOrDefault(); //Used LINQ, to get connection named My VPN Client
            if (conn != null) //If connection is found
            {
                conn.HangUp(); //You know what this does
            }
        }

        public bool getConnectionStatus() {
            try {
                RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == vpnData.entryName).FirstOrDefault(); //Used LINQ, to get connection named My VPN Client
                if (conn != null){

                    return true;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return false;
        }
    }
}