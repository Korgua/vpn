using System;
using System.Timers;

namespace vh_vpn {
    public class manage_vpn {
        private Timer connectionTesting = new Timer();
        private Timer statusUpdate = new Timer();
        private PetrolineAPI papi = new PetrolineAPI();
        private CONSTANTS CONSTANTS = new CONSTANTS();

        private int counter = 0;
        private string vpnPreviousError;
        private vpn_connector vpn = new vpn_connector();
        private logging logging = new logging();
        private int vpnPreviousState = 1; //0:not connected, 1: connecting, 2: connected, 3: vpn connection failed, 4: network error
        private int attempts = 0;
        public manage_vpn() {
            logging.writeToLog(null, String.Format("[Program] Begin"), 1);
            statusUpdate.Interval = 1;
            connectionTesting.Elapsed += establishConnection;
            connectionTesting.Interval = 1;
            connectionTesting.Start();
        }


        private void establishConnection(object sender, EventArgs args) {
            logging.writeToLog(null, String.Format("[establishConnection] Begin"), 3);
            statusUpdate.Stop();
            statusUpdate.Elapsed -= updateStatus;
            counter = 0;
            statusUpdate.Elapsed += updateStatus;
            statusUpdate.Start();

            if (Convert.ToInt32(connectionTesting.Interval) != CONSTANTS.stateInterval) {
                connectionTesting.Interval = CONSTANTS.stateInterval;
                logging.writeToLog(null, String.Format("[establishConnection] Set timeout back to {0}", CONSTANTS.stateInterval), 3);
            }
            int connTest = Convert.ToInt32(connectionTesting.Interval);
            if (vpn.getConnectionStatus() == null) {
                if (attempts <= CONSTANTS.maxAttempt) {
                    if (vpn.testInternetConnection(false)) {
                        vpnPreviousState = 1;
                        vpn.Dialer();
                    }
                    else {
                        if (vpnPreviousState != 4) {
                            papi.sendStatus(0, connTest);
                        }
                        vpnPreviousState = 4;
                    }
                }
                else {
                    vpnPreviousState = 3;
                    connectionTesting.Interval = CONSTANTS.wait;
                    attempts = 0;
                    logging.writeToLog(null, String.Format("[establishConnection] Error: {0}", vpn.error), 10);
                    if (vpnPreviousError != vpn.error) {
                        papi.sendStatus(0, connTest, "Max attempt reached without vpn connection: " + vpn.error);
                        vpnPreviousError = vpn.error;
                    }
                    else {
                        papi.sendStatus(0, connTest);
                    }
                    logging.writeToLog(null, String.Format("[establishConnection] Max attempt reached without vpn connection! Increasing timeout to: {0}", connectionTesting.Interval), 10);
                }
            }
            else {
                if (!vpn.testInternetConnection(true)) {
                    if (attempts == CONSTANTS.maxAttempt) {
                        vpnPreviousState = 4;
                        attempts = 0;
                        connectionTesting.Interval = CONSTANTS.wait;
                        if (vpnPreviousError != vpn.error) {
                            papi.sendStatus(0, connTest, "Max attempt reached with vpn connection: " + vpn.error);
                            vpnPreviousError = vpn.error;
                        }
                        else {
                            papi.sendStatus(0, connTest);
                        }
                        logging.writeToLog(null, String.Format("[establishConnection] Max attempt achieved with vpn connection! Increasing timeout to: {0}", connectionTesting.Interval), 10);
                    }
                    else {
                        vpnPreviousState = 1;
                        logging.writeToLog(null, String.Format("[establishConnection] Error: {0}", vpn.error), 10);
                    }
                }
                else {
                    if (vpnPreviousState == 0 || vpnPreviousState == 1) {
                        vpnPreviousState = 2;
                    }
                    logging.writeToLog(null, String.Format("[establishConnection] Vpn connection is alive"), 10);
                    attempts = 0;
                }
            }
            logging.writeToLog(null, String.Format("[establishConnection] Attempts: {0}/{1}, current interval: {2}", attempts++, CONSTANTS.maxAttempt, connectionTesting.Interval), 3);
        }


        private void updateStatus(object sender, EventArgs args) {
            if (statusUpdate.Interval == 1) {
                statusUpdate.Interval = 1000;
            }
            
                papi.sendStatus(vpnPreviousState==1?1:0, Convert.ToInt32(connectionTesting.Interval) - (++counter * 1000));


        }
    }
}