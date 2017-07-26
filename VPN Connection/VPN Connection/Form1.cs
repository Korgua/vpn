using DotRas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace VPN_Connection {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            /*foreach(RasDevice RasDevice in RasDevice.GetDevices()) {
                Console.WriteLine(RasDevice.Name);
                RasEntry entry = RasEntry.CreateVpnEntry("My VPN Client", "petrolcard.hu", RasVpnStrategy.PptpFirst, RasDevice); //Prepare server details
                Console.WriteLine(entry.Device.Name);
            }*/
        }


            RasDialer dialer = new RasDialer();
            void connectPPTP(string ip, string username, string password){
            FileStream fs = null;
            String path = @".\\vpn.pbk";
            if(System.IO.File.Exists(path)) {
                System.IO.File.Delete(path);
            }
                fs = System.IO.File.Create(path);
                fs.Close();

            
                        //File.WriteAllText("vpn.pbk", ""); //Clean & create vpn.pbk
                        RasPhoneBook book = new RasPhoneBook();
                book.Open("vpn.pbk"); //Define book path
            RasDevice device = RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn); //PPTP interface in Windows
            RasEntry entry = RasEntry.CreateVpnEntry("My VPN Client", ip, RasVpnStrategy.PptpFirst, device); //Prepare server details
                book.Entries.Add(entry); //Write to vpn.pbk

    //Writing server information is done, now we will connect to it.

                
                dialer = new RasDialer();
                            dialer.PhoneBookPath = book.Path; //Read server list from vpn.pbk
                dialer.Credentials = new NetworkCredential(username, password); //Define username and password
                dialer.EntryName = "My VPN Client"; //Get server named My VPN Client
                dialer.Dial(); //Connect
            }

void disconnectPPTP()
{
                RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == "My VPN Client").FirstOrDefault(); //Used LINQ, to get connection named My VPN Client
                if(conn != null) //If connection is found
                {
                    conn.HangUp(); //You know what this does
                }
            }

            bool getConnectionStatus()
{
                RasConnection conn = RasConnection.GetActiveConnections().Where(o => o.EntryName == "My VPN Client").FirstOrDefault(); //Used LINQ, to get connection named My VPN Client
                if(conn != null) //If connection is found
                {
                    return true;
                }
                return false;
            }
    }
}
