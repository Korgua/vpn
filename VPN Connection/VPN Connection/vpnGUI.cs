using System;
using System.Windows.Forms;

namespace VPN_Connection {
    public partial class vpnGUI :Form {
        private vpn vpn = new vpn();
        private vpnData vpnData = new vpnData();
        private logging logging = new logging();
        private int vpnPreviousState = 0; //0:not connected, 1: connecting, 2: connected, 3:failed
        public vpnGUI() {
            AppDomain.CurrentDomain.ProcessExit += (AppDomainSender, AppDomainArgs) => {
                //Windows 10 - Sometimes the tray icon won't dispose after close
                //Force to remove before close the app
                trayIcon.BalloonTipClosed += (sender, args) => {
                    var trayIcon = (NotifyIcon)sender;
                    trayIcon.Visible = false;
                    trayIcon.Icon = null;
                    trayIcon.Dispose();
                };
                logging.writeToLog(null, String.Format("[Program] End"));
            };
            InitializeComponent();
            logging.writeToLog(null, String.Format("[Program] Begin"));
            int attempts = 0;
            //connectToVpn();

            Timer timer = new Timer();
            timer.Interval = vpnData.stateInterval;
            timer.Tick += (sender, args) => {
                if (attempts == vpnData.maxAttempt && vpnPreviousState == 1) {
                    timer.Stop();
                    vpnPreviousState = 3;
                    logging.writeToLog(null, String.Format("[Ticker] Reached max attempts({0})! Timer stopped",vpnData.maxAttempt));
                }
                else if (vpnPreviousState < 2) {
                    logging.writeToLog(null, String.Format("[Ticker] Attempts: {0}/{1}", ++attempts, vpnData.maxAttempt));
                    connectToVpn();
                }
                else if (vpnPreviousState == 2) {
                    connectionStatus();
                    attempts = 0;
                }
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
            logging.writeToLog(null, String.Format("[connectToVpn] Begin"));
            if (!vpn.getConnectionStatus()) {
                logging.writeToLog(null, String.Format("[ConnectionStatus] Not connected"));
                if (vpnPreviousState == 3) {
                    logging.writeToLog(null, String.Format("[connectToVpn] Failed to connect"));
                    trayIconContextItemState.Text = "Csatlakozás sikertelen";
                    vpnPreviousState = 3;
                    createBalloonMessage(vpnData.notificationLength, "A központhoz való csatlakozás meghiúsult", vpnData.host, ToolTipIcon.Error);
                    MessageBox.Show("A központhoz való csatlakozás meghiúsult", vpnData.host, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    if (vpnPreviousState != 1) {
                        createBalloonMessage(vpnData.notificationLength, "Csatlakozás...", vpnData.host, ToolTipIcon.Info);
                        trayIconContextItemState.Text = "Csatlakozás";
                        logging.writeToLog(null, String.Format("[connectToVpn] Connecting"));
                    }
                    vpnPreviousState = 1;
                    vpn.connectPPTP();
                    if (vpn.getConnectionStatus()) {
                        logging.writeToLog(null, String.Format("[ConnectionStatus] Connected"));
                        createBalloonMessage(vpnData.notificationLength, "Csatlakoztatva", vpnData.host, ToolTipIcon.Info);
                        trayIconContextItemState.Text = "Csatlakoztatva";
                        vpnPreviousState = 2;
                    }
                }
            }
            else if (vpn.getConnectionStatus() && vpnPreviousState == 0) {
                logging.writeToLog(null, String.Format("[ConnectionStatus] Already connected"));
                createBalloonMessage(vpnData.notificationLength, "Csatlakoztatva", vpnData.host, ToolTipIcon.Info);
                vpnPreviousState = 2;
            }
        }

        private bool connectionStatus() {
            if (vpn.getConnectionStatus()) {
                return true;
            }
            else {
                if (vpnPreviousState == 2) {
                    vpnPreviousState = 0;
                }
            }
            return false;
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