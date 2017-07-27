using DotRas;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace VPN_Connection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += (sender, args) => getConnectionStatus(sender, args);
            timer.Start();

            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles
        }


        RasDialer dialer = new RasDialer();
        void connectPPTP(string ip, string username, string password)
        {
            Uri uri = new Uri(ip);
            ip = Dns.GetHostAddresses(uri.Host)[0].ToString();
            FileStream fs = null;
            String path = @".\\vpn.pbk";
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            fs = System.IO.File.Create(path);
            fs.Close();


            //File.WriteAllText("vpn.pbk", ""); //Clean & create vpn.pbk
            RasPhoneBook book = new RasPhoneBook();
            book.Open(path); //Define book path
            bool first = true;
            RasDevice device = null;
            foreach (RasDevice _device in RasDevice.GetDevices())
            {
                Console.WriteLine(_device.Name+" "+_device.DeviceType);
                if (_device.Name.Contains("PPTP") && _device.DeviceType.ToString().ToLower() == "vpn" && first)
                {

                    device = _device;
                    first = false;
                }
            }
            /*RasDevice device = RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn); //PPTP interface in Windows*/
            RasEntry entry = RasEntry.CreateVpnEntry("Petrolcard.hu", ip, RasVpnStrategy.PptpFirst, device); //Prepare server details
            book.Entries.Add(entry); //Write to vpn.pbk

            //Writing server information is done, now we will connect to it.


            dialer = new RasDialer();
            dialer.PhoneBookPath = book.Path; //Read server list from vpn.pbk
            dialer.Credentials = new NetworkCredential(username, password); //Define username and password
            dialer.EntryName = "Petrolcard.hu"; //Get server named My VPN Client
            dialer.Dial(); //Connect
        }

        void disconnectPPTP()
        {
            RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == "Petrolcard.hu").FirstOrDefault(); //Used LINQ, to get connection named My VPN Client
            if (conn != null) //If connection is found
            {
                conn.HangUp(); //You know what this does
            }
        }

        void getConnectionStatus(object sender, EventArgs args)
        {
            RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == "Petrolcard.hu").FirstOrDefault(); //Used LINQ, to get connection named My VPN Client
            if (conn != null) //If connection is found
            {
                Console.WriteLine("VPN aktív");
                this.BackColor = System.Drawing.Color.Green;
            }
            else Console.WriteLine("VPN inaktív"); ;
        }

        private void vpnConnect_Click(object sender, EventArgs e)
        {
            connectPPTP("http://www.petrolcard.hu", "jundjkft", "jundjkft");
        }

        private void vpmDisconnect_Click(object sender, EventArgs e)
        {
            disconnectPPTP();
        }

        private void button1_Click(object sender, EventArgs e) {
            VPNManager.VPN vpn = new VPNManager.VPN("petrolcard.hu", "37.221.209.92");
            vpn.StartManaging();
        }
    }
}