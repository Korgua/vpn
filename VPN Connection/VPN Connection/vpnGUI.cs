using System;
using System.Drawing;
using System.Windows.Forms;

namespace VPN_Connection {
    public partial class vpnGUI :Form {
        private bool ToggleMove;
        private int MValX;
        private int MValY;

        private vpn vpn = new vpn();
        //private vpnData vpnData = new vpnData();
        private logging logging = new logging();
        private animation Anim = new animation();
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
            this.Location = new Point(Screen.FromPoint(this.Location).WorkingArea.Right - this.Width, 0);
            logging.writeToLog(null, String.Format("[Program] Begin"));
            int attempts = 0;
            //connectToVpn();
            Timer timer = new Timer();
            timer.Interval = 1;
            timer.Tick += (sender, args) => {
                if (attempts == vpn.vpnData.maxAttempt && vpnPreviousState == 1) {
                    timer.Stop();
                    vpnPreviousState = 3;
                    logging.writeToLog(null, String.Format("[Ticker] Reached max attempts({0})! Timer stopped",vpn.vpnData.maxAttempt));
                    connectToVpn();
                }
                else if (vpnPreviousState < 2) {
                    attempts++;
                    logging.writeToLog(null, String.Format("[Ticker] Attempts: {0}/{1}", attempts, vpn.vpnData.maxAttempt));
                    connectToVpn();
                }
                else if (vpnPreviousState == 2) {
                    attempts = 0;
                    if (!connectionStatus()) {
                        connectToVpn();
                    }
                }
                if (timer.Interval == 1) {
                    timer.Interval = vpn.vpnData.stateInterval;
                }
            };
            timer.Start();
            trayIconContextItemState.Enabled = false;
            //this.Opacity = 100;
            //Anim.Shrink(this, 5, 32, 128);
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles
        }
        private void FormDragStart(object sender, MouseEventArgs e) {
            ToggleMove = true;
            MValX = e.X;
            MValY = e.Y;
        }
        private void FormDragEnd(object sender, MouseEventArgs e) {
            ToggleMove = false;
        }
        private void FormDrag(object sender, MouseEventArgs e) {
            if(ToggleMove) {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        public void connectToVpn() {
            logging.writeToLog(null, String.Format("[connectToVpn] Begin"));
            logging.writeToLog(null, String.Format("[connectToVpn] vpnPreviousState: {0}",vpnPreviousState));
            if (vpn.getConnectionStatus()==null) {
                logging.writeToLog(null, String.Format("[ConnectionStatus] Not connected"));
                if (vpnPreviousState == 3) {
                    logging.writeToLog(null, String.Format("[connectToVpn] Failed to connect"));
                    vpnPreviousState = 3;
                    Anim.activateNotification(this, notificationIcon, notificationText, 3, vpn.vpnData.notificationLength);
                    //MessageBox.Show("Csatlakozás sikertelen", vpnData.host, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    if (vpnPreviousState != 1) {
                        Anim.activateNotification(this, notificationIcon, notificationText, 1, vpn.vpnData.notificationLength);
                        trayIconContextItemState.Text = "Csatlakozás";
                        logging.writeToLog(null, String.Format("[connectToVpn] Connecting"));
                    }
                    vpnPreviousState = 1;
                    vpn.connectPPTP();
                    if (vpn.getConnectionStatus() != null) {
                        logging.writeToLog(null, String.Format("[ConnectionStatus] Connected"));
                        Anim.activateNotification(this, notificationIcon, notificationText, 2, vpn.vpnData.notificationLength);
                        trayIconContextItemState.Text = "Csatlakoztatva";
                        vpnPreviousState = 2;
                    }
                }
            }
            else if (vpn.getConnectionStatus()!=null && vpnPreviousState == 0) {
                logging.writeToLog(null, String.Format("[ConnectionStatus] Already connected"));
                vpnPreviousState = 2;
                Anim.activateNotification(this, notificationIcon, notificationText, 2, vpn.vpnData.notificationLength);
            }
        }

        private bool connectionStatus() {
            if (vpn.getConnectionStatus()!=null) {
                logging.writeToLog(null, String.Format("[connectionStatus] VPN connection is active"));
                return true;
            }
            else {
                if (vpnPreviousState == 2) {
                    vpnPreviousState = 0;
                }
            }
            logging.writeToLog(null, String.Format("[connectionStatus] VPN connection is not active"));
            Console.WriteLine(String.Format("[connectionStatus] VPN connection is not active"));
            return false;
        }

        private void trayIconContextItemDisconnect_Click(object sender, EventArgs e) {
            vpn.disconnectPPTP();
        }

        private void kilépésToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}