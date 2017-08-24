﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace VPN_Connection {
    public partial class vpnGUI : Form {
        private System.Windows.Forms.Timer connectionTesting = new System.Windows.Forms.Timer();

        private bool ToggleMove;
        private int MValX;
        private int MValY;

        private vpn vpn = new vpn();
        private logging logging = new logging();
        private animation Anim;
        private int vpnPreviousState = 0; //0:not connected, 1: connecting, 2: connected, 3:failed
        private int attempts = 0;
        public vpnGUI() {
            InitializeComponent();
            Anim = new animation(this, this.notificationStatusIcon, this.Width, this.Height, vpn.vpnData.stateInterval);
            this.Location = new Point(Screen.FromPoint(this.Location).WorkingArea.Right - this.Width, 0);
            logging.writeToLog(null, String.Format("[Program] Begin"));
            connectionTesting.Tick += establishConnection;
            connectionTesting.Interval = 1;
            connectionTesting.Start();
            //establishConnection();
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles
        }

        private void establishConnection(object sender, EventArgs args) {
            
                if (attempts == vpn.vpnData.maxAttempt && vpnPreviousState == 1) {
                    connectionTesting.Stop();
                    connectionTesting.Tick -= establishConnection;
                    vpnPreviousState = 3;
                attempts = 0;
                    logging.writeToLog(null, String.Format("[Ticker] Reached max attempts({0})! Timer stopped", vpn.vpnData.maxAttempt));
                    connectToVpn();
            }
                else if (vpnPreviousState < 2) {
                    attempts++;
                    logging.writeToLog(null, String.Format("[Ticker] Attempts: {0}/{1}", attempts, vpn.vpnData.maxAttempt));
                    connectToVpn();
                }
                else if (vpnPreviousState == 2) {
                    attempts = 0;
                    if(!connectionStatus()) {
                        connectToVpn();
                    }
                }
                if (connectionTesting.Interval == 1) {
                    connectionTesting.Interval = vpn.vpnData.stateInterval < 10000 ? 10000 : vpn.vpnData.stateInterval;
                }
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

        public void connectToVpn() {
            logging.writeToLog(null, String.Format("[connectToVpn] Begin"));
            logging.writeToLog(null, String.Format("[connectToVpn] vpnPreviousState: {0}", vpnPreviousState));
            if (vpn.getConnectionStatus() == null) {
                logging.writeToLog(null, String.Format("[connectToVpn][ConnectionStatus] Not connected"));
                if (vpnPreviousState == 3) {
                    logging.writeToLog(null, String.Format("[connectToVpn] Failed to connect"));
                    Anim.activateNotification(this,notificationText,3);
                }
                else {
                    if (vpnPreviousState != 1) {
                        Anim.activateNotification(this,notificationText,1);
                        logging.writeToLog(null, String.Format("[connectToVpn] Connecting"));
                    }
                    vpnPreviousState = 1;
                    vpn.Dialer();
                    if (vpn.getConnectionStatus() != null) {
                        logging.writeToLog(null, String.Format("[ConnectionStatus] Connected"));
                        Anim.activateNotification(this,notificationText, 2);
                        vpnPreviousState = 2;
                    }
                }
            }
            else if (connectionStatus()) {
                logging.writeToLog(null, String.Format("[ConnectionStatus] Already connected"));
                vpnPreviousState = 2;
                Anim.activateNotification(this,notificationText, 2);
            }
        }

        private bool connectionStatus() {
            if (vpn.getConnectionStatus() != null && vpn.testInternetConnection(true)) {
                return true;
            }
            else {
                if (vpnPreviousState == 2) {
                    vpnPreviousState = 0;
                }
                return false;
            }
        }


        private void statusIconContextReconnect_Click(object sender, EventArgs e) {
            vpnPreviousState = 0;
            connectionTesting.Stop();
            connectionTesting.Tick -= establishConnection;
            connectionTesting.Tick += establishConnection;
            connectionTesting.Interval = 1;
            connectionTesting.Start();
        }

        private void notificationStatusIcon_Click(object sender, EventArgs e) {
            if (vpnPreviousState == 0 || vpnPreviousState == 3) { 
                statusIconContextReconnect.Enabled = true;
                statusIconContextHangUp.Enabled = false;
            }
            else if(vpnPreviousState == 2){
                statusIconContextHangUp.Enabled = true;
                statusIconContextReconnect.Enabled = false;
            }
            else if(vpnPreviousState == 1) {
                statusIconContextReconnect.Enabled = false;
                statusIconContextHangUp.Enabled = false;
                if (ModifierKeys.HasFlag(Keys.Control)) {
                    statusIconContextHangUp.Enabled = true;
                }
            }
            StatusIconContextMenu.Show(Cursor.Position);
        }

        private void statusIconContextHangUp_Click(object sender, EventArgs e) {
            vpn.disconnectPPTP();
            connectionTesting.Stop();
            connectionTesting.Tick -= establishConnection;
            vpnPreviousState = 0;
            Anim.activateNotification(this,notificationText,0);
        }
    }
}