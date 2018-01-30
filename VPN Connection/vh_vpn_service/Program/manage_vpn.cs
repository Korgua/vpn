using System;
using System.Diagnostics;
using System.Timers;

namespace vh_vpn {
    public class Manage_vpn {
        private Timer connectionTesting = new Timer();
        private Timer statusUpdate = new Timer();
        private Timer logDelete = new Timer();

        private PetrolineAPI papi = new PetrolineAPI();
        private CONSTANTS CONSTANTS = new CONSTANTS("@manage_vpn");
        private vh_vpn vh_Vpn;

        private VPN_connector vpn = new VPN_connector();
        private logging logging = new logging();

        private string vpnPreviousResolveError, vpnPreviousConnectError;
        private string papiRss, papiRssAccepted;
        private int vpnPreviousState = 1; //0:not connected, 1: connecting, 2: connected, 3: vpn connection failed, 4: network error
        private int attempts = 0, counter = 0;

        public Manage_vpn(vh_vpn vh_Vpn) {
            this.vh_Vpn = vh_Vpn;
            logging.writeToLog(null, String.Format("[Program] Begin"), 3);
            statusUpdate.Interval = 1;

            logDelete.Interval = 1;
            logDelete.Elapsed += deleteLogFiles;
            logDelete.Start();

            connectionTesting.Elapsed += EstablishConnection;
            connectionTesting.Interval = 1;
            connectionTesting.Start();

            statusUpdate.Elapsed += UpdateStatus;
            statusUpdate.Start();
        }


        private void EstablishConnection(object sender, EventArgs args) {
            logging.writeToLog(null, String.Format("[EstablishConnection] Begin"), 3);
            counter = 0;
            /*statusUpdate.Stop();
            statusUpdate.Elapsed -= UpdateStatus;
            
            statusUpdate.Elapsed += UpdateStatus;
            statusUpdate.Start();*/

            if (Convert.ToInt32(connectionTesting.Interval) != CONSTANTS.stateInterval) {
                connectionTesting.Interval = CONSTANTS.stateInterval;
                logging.writeToLog(null, String.Format("[EstablishConnection] Set timeout back to {0}", CONSTANTS.stateInterval), 3);
            }
            int connTest = Convert.ToInt32(connectionTesting.Interval)/1000;
            if (vpn.getConnectionStatus() == null) {
                if (attempts <= CONSTANTS.maxAttempt) {
                    if (vpn.testInternetConnection(false)) {
                        vpnPreviousState = 1;
                        vpn.Dialer();
                    }
                    else {
                        /*if (vpnPreviousState != 4) {
                            papi.SendStatus(0, connTest);
                        }*/
                        vpnPreviousState = 4;
                    }
                }
                else {
                    vpnPreviousState = 3;
                    connectionTesting.Interval = CONSTANTS.wait;
                    attempts = 0;
                    if (vpn.resolveError != null)
                        logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.resolveError), 1);
                    if (vpn.connectError != null) {
                        logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.connectError), 1);
                    }
                    if (vpnPreviousResolveError != vpn.resolveError) {
                        papiRss = "Max attempt reached without vpn connection: " + vpn.resolveError;
                        /*string papiResp = papi.SendStatus(0, CONSTANTS.wait, "Max attempt reached without vpn connection: " + vpn.resolveError);
                        if (papiResp.Contains("RSSAccepted=0")) {
                            papiRssAccepted = "Max attempt reached without vpn connection: " + vpn.resolveError;
                        }
                        else {
                            papiRssAccepted = null;
                        }*/
                        vpnPreviousResolveError = vpn.resolveError;
                    }
                    else if (vpnPreviousConnectError != vpn.connectError) {
                        papiRss = "Max attempt reached without vpn connection: " + vpn.connectError;
                        /*string papiResp = papi.SendStatus(0, CONSTANTS.wait, "Max attempt reached without vpn connection: " + vpn.connectError);
                        if (papiResp.Contains("RSSAccepted=0")) {
                            papiRssAccepted = "Max attempt reached without vpn connection: " + vpn.connectError;
                        }
                        else {
                            papiRssAccepted = null;
                        }*/
                        vpnPreviousConnectError = vpn.connectError;
                    }
                    /*else {
                        papi.SendStatus(0, connTest - counter);
                    }*/
                    logging.writeToLog(null, String.Format("[EstablishConnection] Max attempt reached without vpn connection! Increasing timeout to: {0}", connectionTesting.Interval), 1);
                }
            }
            else {
                if (!vpn.testInternetConnection(true)) {
                    if (attempts == CONSTANTS.maxAttempt) {
                        vpnPreviousState = 4;
                        attempts = 0;
                        connectionTesting.Interval = CONSTANTS.wait;
                        if (vpn.resolveError != null)
                            logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.resolveError), 1);
                        if (vpn.connectError != null)
                            logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.connectError), 1);
                        if (vpnPreviousResolveError != vpn.resolveError) {
                            //papi.SendStatus(0, CONSTANTS.wait, "Max attempt reached with vpn connection: " + vpn.resolveError);
                            vpnPreviousResolveError = vpn.resolveError;
                        }
                        else if (vpnPreviousConnectError != vpn.connectError) {
                            //papi.SendStatus(0, CONSTANTS.wait, "Max attempt reached without vpn connection: " + vpn.connectError);
                            vpnPreviousConnectError = vpn.connectError;
                        }
                        /*else {
                            papi.SendStatus(0, connTest - counter);
                        }*/
                        logging.writeToLog(null, String.Format("[EstablishConnection] Max attempt achieved with vpn connection! Increasing timeout to: {0}", connectionTesting.Interval), 1);
                    }
                    else {
                        vpnPreviousState = 1;
                        logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.resolveError), 1);
                    }
                }
                else {
                    vpnPreviousState = 2;
                    logging.writeToLog(null, String.Format("[EstablishConnection] Vpn connection is alive"), 2);
                    attempts = 0;
                }
            }
            logging.writeToLog(null, String.Format("[EstablishConnection][End] Attempts: {0}/{1}, current interval: {2}", attempts++, CONSTANTS.maxAttempt, connectionTesting.Interval), 3);
            if (vpn.connectError.Contains("denied")) {
                connectionTesting.Stop();
                statusUpdate.Stop();
                papi.SendStatus(0, connTest - counter, vpn.connectError);
                vh_Vpn.ExitCode = 0;
                vh_Vpn.Stop();
            }
        }


        private void UpdateStatus(object sender, EventArgs args) {
            logging.writeToLog(null, String.Format("[UpdateStatus] Begin"), 3);
            if (statusUpdate.Interval == 1) {
                statusUpdate.Interval = 1000;
            }
            if (vpnPreviousState == 2) {
                papi.SendStatus(1);
            }
            else {
                string papiResp = String.Empty;
                if (papiRssAccepted == null) {
                    papiResp = papi.SendStatus(0, Convert.ToInt32(connectionTesting.Interval) - (++counter * 1000));
                }
                else {
                    papiResp = papi.SendStatus(0, Convert.ToInt32(connectionTesting.Interval) - (++counter * 1000), papiRssAccepted);
                    if (papiResp.Contains("RSSAccepted=1")) {
                        papiRssAccepted = null;
                    }
                    papiResp = null;
                }
                if (papiResp.Contains("NeedConnect=1")) {
                    connectionTesting.Interval = 1;
                }
            }
        }

        private void deleteLogFiles(object sender, EventArgs args) {
            if(logDelete.Interval == 1) {
                logDelete.Interval = 1000 * 60 * 60 * 24;
            }
            logging.deleteOldFiles();
        }
    }
}