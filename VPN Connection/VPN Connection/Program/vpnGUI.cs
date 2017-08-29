using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace VPN_Connection {
    public partial class vpnGUI :Form {
        private System.Windows.Forms.Timer connectionTesting = new System.Windows.Forms.Timer();

        private bool ToggleMove;
        private int MValX;
        private int MValY;

        private vpn vpn = new vpn();
        private logging logging = new logging();
        private animation Anim;
        private int vpnPreviousState = 1; //0:not connected, 1: connecting, 2: connected, 3: vpn connection failed, 4: network error
        private int attempts = 0;
        public vpnGUI() {
            InitializeComponent();
            this.Location = new Point(Screen.FromPoint(this.Location).WorkingArea.Right - this.Width, 0);
            Anim = new animation(this, this.Width, this.Height, this.Opacity, vpn.vpnData.notificationLength);
            this.Load += (sender, args) => Anim.activateNotification(this, 1);
            logging.writeToLog(null, String.Format("[Program] Begin"), 1);
            connectionTesting.Tick += establishConnection;
            connectionTesting.Interval = 5000;
            connectionTesting.Start();
        }


        private void establishConnection(object sender, EventArgs args) {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            if(currentTime < vpn.vpnData.silentmode_end || currentTime > vpn.vpnData.silentmode_start) {
                logging.writeToLog(null, String.Format("Silent mode activated"), 3);
                this.Visible = false;
            }
            else {
                if(!this.Visible) {
                    logging.writeToLog(null, String.Format("Silent mode deactivated"), 3);
                    this.Visible = true;
                }
            }
            logging.writeToLog(null, String.Format("[establishConnection] Begin"), 3);
            if (vpn.getConnectionStatus() == null) {
                if (attempts <= vpn.vpnData.maxAttempt) {
                    if (vpn.testInternetConnection(false)) {
                        if (vpnPreviousState != 1) {
                            Anim.activateNotification(this, vpnPreviousState);
                        }
                        vpnPreviousState = 1;
                        vpn.Dialer();
                    }
                }
                else {
                    vpnPreviousState = 3;
                    Anim.activateNotification(this, vpnPreviousState, vpn.error);
                    connectionTesting.Interval = vpn.vpnData.stateInterval * 2;
                    attempts = 0;
                    logging.writeToLog(null, String.Format("[establishConnection] Error: {0}", vpn.error), 1);
                    logging.writeToLog(null, String.Format("[establishConnection] Max attempt achieved without vpn connection! Increasing timeout to: {0}", connectionTesting.Interval), 1);
                }
            }
            else {
                if (!vpn.testInternetConnection(true)) {
                    if (attempts == vpn.vpnData.maxAttempt) {
                        vpnPreviousState = 4;
                        Anim.activateNotification(this, 4, vpn.error);
                        vpn.disconnectPPTP();
                        attempts = 0;
                        connectionTesting.Interval = vpn.vpnData.stateInterval * 2;
                        logging.writeToLog(null, String.Format("[establishConnection] Max attempt achieved with vpn connection! Increasing timeout to: {0}", connectionTesting.Interval), 1);
                    }
                    else {
                        vpnPreviousState = 1;
                        Anim.activateNotification(this, 4, vpn.error);
                        connectionTesting.Interval = vpn.vpnData.stateInterval;
                        logging.writeToLog(null, String.Format("[establishConnection] Error: {0}",vpn.error), 1);
                    }
                }
                else {
                    if (vpnPreviousState == 0 || vpnPreviousState == 1) {
                        vpnPreviousState = 2;
                        Anim.activateNotification(this, vpnPreviousState);
                    }
                    logging.writeToLog(null, String.Format("[establishConnection] Vpn connection is alive"), 2);
                    attempts = 0;
                    if (connectionTesting.Interval != vpn.vpnData.stateInterval) {
                        connectionTesting.Interval = vpn.vpnData.stateInterval;
                        logging.writeToLog(null, String.Format("[establishConnection] Set timeout back to {0}", vpn.vpnData.stateInterval), 3);
                    }
                }
            }
            logging.writeToLog(null, String.Format("[establishConnection] Attempts: {0}/{1}, current interval: {2}", attempts++, vpn.vpnData.maxAttempt,connectionTesting.Interval),3);
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
            Anim.activateNotification(this, 1);
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
            Anim.activateNotification(this, 0);
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

        private void statusIconContext_Hover(object sender, EventArgs e) {
            BackgroundWorker BW = new BackgroundWorker();
            BW.DoWork += Anim.Stretch;
            BW.RunWorkerCompleted += (_sender, _args) => {
                BW.DoWork -= Anim.Stretch;
                BW.Dispose();
            };
            BW.RunWorkerAsync();
        }
        private void statusIconContext_Leave(object sender, EventArgs e) {
            Anim.activateNotification(this, vpnPreviousState, vpn.error);
        }
    }
}