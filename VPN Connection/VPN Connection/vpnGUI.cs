using System;
using System.Windows.Forms;

namespace VPN_Connection {
    public partial class vpnGUI :Form {
        private vpn vpn = new vpn();
        private vpnData vpnData = new vpnData();
        private int vpnPreviousState = 0; //0:inactive, 1: connecting, 2: connected, 3:failed
        public vpnGUI() {
            AppDomain.CurrentDomain.ProcessExit += (AppDomainSender, AppDomainArgs) => {
                trayIcon.BalloonTipClosed += (sender, args) => {
                    var trayIcon = (NotifyIcon)sender;
                    trayIcon.Visible = false;
                    trayIcon.Icon = null;
                    trayIcon.Dispose();
                };
            };
            InitializeComponent();
            Timer timer = new Timer();
            timer.Interval = vpnData.stateInterval;
            int attempts = 0;
            timer.Tick += (sender, args) => {
                Console.WriteLine("Before -- Attempts: {0}/{2}, vpnPreviousState: {1}", attempts, vpnPreviousState,vpnData.maxAttempt);
                if (attempts == vpnData.maxAttempt && vpnPreviousState == 1) {
                    timer.Stop();
                    vpnPreviousState = 3;
                }
                else if (vpnPreviousState < 2) {
                    ++attempts;
                }
                else if (vpnPreviousState > 1) {
                    attempts = 0;
                }
                connectToVpn();
                Console.WriteLine("After -- Attempts: {0}, vpnPreviousState: {1}", attempts, vpnPreviousState);
            };
            timer.Start();
            trayIconContextItemState.Enabled = false;
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles
        }

        public void createBalloonMessage(int _interval, string _text, string _title, ToolTipIcon _tti) {
            trayIcon.BalloonTipIcon = _tti;
            trayIcon.BalloonTipText = _text;
            trayIcon.BalloonTipTitle = _title;
            trayIcon.Visible = true;
            if (_interval > 5000) _interval = 5000;
            trayIcon.ShowBalloonTip(_interval);
        }

        public void connectToVpn() {
            if (!vpn.getConnectionStatus()) {
                if (vpnPreviousState == 3) {
                    Console.WriteLine("connection failed");
                    trayIconContextItemState.Text = "Csatlakozás sikertelen";
                    vpnPreviousState = 3;
                    createBalloonMessage(vpnData.notificationLength, "A központhoz való csatlakozás meghiúsult", vpnData.host, ToolTipIcon.Error);
                    MessageBox.Show("A központhoz való csatlakozás meghiúsult", vpnData.host, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    Console.WriteLine("!vpn.getConnectionStatus()");
                    Console.WriteLine("Csatlakozás");
                    if (vpnPreviousState != 1)
                        createBalloonMessage(vpnData.notificationLength, "Csatlakozás...", vpnData.host, ToolTipIcon.Info);
                    vpnPreviousState = 1;
                    trayIconContextItemState.Text = "Csatlakozás";
                    vpn.connectPPTP();
                    if (vpn.getConnectionStatus()) {
                        createBalloonMessage(vpnData.notificationLength, "Csatlakoztatva", vpnData.host, ToolTipIcon.Info);
                        trayIconContextItemState.Text = "Csatlakoztatva";
                        vpnPreviousState = 2;
                    }
                }
            }
            else if (vpn.getConnectionStatus() && vpnPreviousState == 0) {
                createBalloonMessage(vpnData.notificationLength, "Csatlakoztatva", vpnData.host, ToolTipIcon.Info);
                vpnPreviousState = 2;
            }
        }

        private void vpnConnect_Click(object sender, EventArgs e) {
            Console.WriteLine("connect_click");
            vpn.connectPPTP();
        }

        private void vpnDisconnect_Click(object sender, EventArgs e) {
            Console.WriteLine("alma");
            vpn.disconnectPPTP();
        }
    }
}