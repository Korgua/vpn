using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace VPN_Connection {
    public partial class vpnGUI :Form {
        private System.Windows.Forms.Timer connectionTesting = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer statusUpdate = new System.Windows.Forms.Timer();
        private PetrolineAPI papi = new PetrolineAPI();

        private bool ToggleMove;
        private int MValX;
        private int MValY;
        private int counter = 0;

        private vpn vpn = new vpn();
        private logging logging = new logging();
        private int vpnPreviousState = 1; //0:not connected, 1: connecting, 2: connected, 3: vpn connection failed, 4: network error
        private int attempts = 0;
        public vpnGUI() {
            InitializeComponent();
            //vpn.disconnectPPTP();
            this.Location = new Point(Screen.FromPoint(this.Location).WorkingArea.Right - this.Width, 0);
            logging.writeToLog(null, String.Format("[Program] Begin"), 1);

            statusUpdate.Interval = 1000;

            connectionTesting.Tick += establishConnection;
            connectionTesting.Interval = 1;
            connectionTesting.Start();
        }


        private void establishConnection(object sender, EventArgs args) {
            logging.writeToLog(null, String.Format("[establishConnection] Begin"), 3);
            statusUpdate.Stop();
            statusUpdate.Tick -= updateStatus;
            counter = 0;
            if(connectionTesting.Interval != vpn.vpnData.stateInterval) {
                connectionTesting.Interval = vpn.vpnData.stateInterval;
                logging.writeToLog(null, String.Format("[establishConnection] Set timeout back to {0}", vpn.vpnData.stateInterval), 3);
            }
            if (vpn.getConnectionStatus() == null) {
                if (attempts <= vpn.vpnData.maxAttempt) {
                    if (vpn.testInternetConnection(false)) {
                        if (vpnPreviousState != 1) {
                            papi.sendStatus(0, connectionTesting.Interval);
                        }
                        vpnPreviousState = 1;
                        vpn.Dialer();
                    }
                    else{
                        if (vpnPreviousState != 4){
                            papi.sendStatus(0, connectionTesting.Interval);
                        }
                        vpnPreviousState = 4;
                    }
                }
                else {
                    vpnPreviousState = 3;
                    papi.sendStatus(0, connectionTesting.Interval);
                    connectionTesting.Interval = vpn.vpnData.stateInterval * 2;
                    attempts = 0;
                    logging.writeToLog(null, String.Format("[establishConnection] Error: {0}", vpn.error), 10);
                    papi.sendStatus(0, connectionTesting.Interval, "2;Próbálkozások száma elérte a maximumot: " + vpn.error);
                    logging.writeToLog(null, String.Format("[establishConnection] Max attempt reached without vpn connection! Increasing timeout to: {0}", connectionTesting.Interval), 10);
                }
            }
            else {
                if (!vpn.testInternetConnection(true)) {
                    if (attempts == vpn.vpnData.maxAttempt) {
                        vpnPreviousState = 4;
                        //vpn.disconnectPPTP();
                        attempts = 0;
                        connectionTesting.Interval *= 2;
                        papi.sendStatus(0, connectionTesting.Interval, "2;Próbálkozások száma elérte a maximumot: " + vpn.error);
                        logging.writeToLog(null, String.Format("[establishConnection] Max attempt achieved with vpn connection! Increasing timeout to: {0}", connectionTesting.Interval), 10);
                    }
                    else {
                        vpnPreviousState = 1;
                        papi.sendStatus(0, connectionTesting.Interval);
                        logging.writeToLog(null, String.Format("[establishConnection] Error: {0}",vpn.error), 10);
                    }
                }
                else {
                    if (vpnPreviousState == 0 || vpnPreviousState == 1) {
                        vpnPreviousState = 2;
                    }
                    papi.sendStatus(1, 0);
                    logging.writeToLog(null, String.Format("[establishConnection] Vpn connection is alive"), 10);
                    attempts = 0;
                }
            }
            statusUpdate.Tick += updateStatus;
            statusUpdate.Start();
            logging.writeToLog(null, String.Format("[establishConnection] Attempts: {0}/{1}, current interval: {2}", attempts++, vpn.vpnData.maxAttempt,connectionTesting.Interval),3);
        }


        private void updateStatus(object sender, EventArgs args) {
            papi.sendStatus(vpnPreviousState==2?1:0, connectionTesting.Interval  - (++counter*1000));
        }


        private void FormDragStart(object sender, MouseEventArgs e) {
            ToggleMove = true;
            MValX = e.X;
            MValY = e.Y;
        }
        private void StatusIconDragStart(object sender, MouseEventArgs e) {
            ToggleMove = true;
            MValX = e.X + notificationStatusIcon.Padding.Left + SystemInformation.BorderSize.Width;
            MValY = e.Y + notificationStatusIcon.Padding.Top + SystemInformation.BorderSize.Width;
        }
        private void FormDragEnd(object sender, MouseEventArgs e) {
            ToggleMove = false;
        }
        private void FormDrag(object sender, MouseEventArgs e) {
            if (ToggleMove) {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        private void statusIconContextReconnect_Click(object sender, EventArgs e) {
            vpnPreviousState = 0;
            connectionTesting.Interval = 1;
            connectionTesting.Start();
        }

        private void notificationStatusIcon_Click(object sender, EventArgs e) {
            if (vpnPreviousState == 0 || vpnPreviousState >= 3) {
                statusIconContextReconnect.Enabled = true;
                statusIconContextHangUp.Enabled = false;
            }
            else if (vpnPreviousState == 2) {
                statusIconContextHangUp.Enabled = true;
                statusIconContextReconnect.Enabled = false;
            }
            else if (vpnPreviousState == 1) {
                statusIconContextReconnect.Enabled = false;
                statusIconContextHangUp.Enabled = false;
                if (ModifierKeys.HasFlag(Keys.Control)) {
                    statusIconContextHangUp.Enabled = true;
                }
            }
            MouseEventArgs MEA = (MouseEventArgs)e;
            if (MEA.Button.ToString() == "Right") {
                statusIconContextExit.Enabled = true;
            }
            else {
                statusIconContextExit.Enabled = false;
            }
            StatusIconContextMenu.Show(Cursor.Position);
        }

        private void statusIconContextHangUp_Click(object sender, EventArgs e) {
            vpn.disconnectPPTP();
            connectionTesting.Stop();
            vpnPreviousState = 0;
        }

        private void statusIconContextExit_Click(object sender, EventArgs e) {
            System.Windows.Forms.Timer waitBeforeExit = new System.Windows.Forms.Timer();
            waitBeforeExit.Interval = 100;
            waitBeforeExit.Tick += (_sender, _args) => {
                if(!vpn.vpn_connected) {
                    logging.writeToLog(null, String.Format("[Program] End"), 1);
                    Application.Exit();
                }
            };
            waitBeforeExit.Start();
            vpn.disconnectPPTP();
            connectionTesting.Stop();
        }
    }
}