using System;
using System.Diagnostics;
using System.Timers;

namespace vh_vpn {
    public class Manage_vpn {
        //Sending vpn status and deleting old log files with Timers
        private Timer statusUpdate = new Timer();
        private Timer logDelete = new Timer();


        private PetrolineAPI papi = new PetrolineAPI();
        private CONSTANTS CONSTANTS = new CONSTANTS("@manage_vpn");
        private vh_vpn vh_Vpn;

        public VPN_connector vpn = new VPN_connector();
        private logging logging = new logging();

        //If there was an error with connecting to vpn server or resolving somthing else (there is no internet, vpn server is unreachable
        private string vpnPreviousResolveError, vpnPreviousConnectError;

        //P_API_rss: Sending error to PetroLine to make it as an RSS
        //P_API_rss_accepted: Last accepted error message (as RSS)
        private string P_API_rss = null, P_API_rss_accepted = null;

        //0:not connected, 1: connecting, 2: connected, 3: vpn connection failed, 4: network error
        private int vpnPreviousState = 1;

        //attempts: counting the vpn connection attempts
        //counter: Waiting N sec before trying to reconnect (usually 5 or 60)
        private int attempts = 0, counter = 1;


        public Manage_vpn(vh_vpn vh_Vpn) {
            this.vh_Vpn = vh_Vpn;
            logging.writeToLog(null, String.Format("[Program] Begin"), 3);
            if (!CONSTANTS.checkConfigFile()) {
                papi.SendStatus(0, 0, "The VPN configuration file is missing, or parsing error!");
                vh_Vpn.Stop();
            }
            if (!CONSTANTS.EncryptConfigFile()) {
                P_API_rss = "There was an error while encrypting sensitive data in vpn config file!";
            }

            //Deleting log files
            logDelete.Interval = 1;
            logDelete.Elapsed += deleteLogFiles;
            logDelete.Start();

            //starting the connection
            statusUpdate.Interval = 1;
            statusUpdate.Elapsed += UpdateStatus;
            statusUpdate.Start();
        }


        private void EstablishConnection() {
            //If there is no vpn connection
            if(vpn.getConnectionStatus() == null) {
                //but the maximum tries is not achieved
                if(attempts <= CONSTANTS.maxAttempt) {
                    if(vpn.testInternetConnection(false)) {
                        vpnPreviousState = 1;
                        //Trying to dialing the vpn connection
                        vpn.Dialer();
                    }
                    else {
                        vpnPreviousState = 4;
                    }
                }
                else {
                    //At this point we reached the maximum attempts
                    //Now, we will waiting n sec before trying to dialing again
                    vpnPreviousState = 3;
                    counter = CONSTANTS.wait;
                    attempts = 0;
                    //Maybe, there was an error with internet connection
                    if (vpn.resolveError != null) {
                        logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.resolveError), 1);
                        if (vpnPreviousResolveError != vpn.resolveError) {
                            P_API_rss = "Max attempt reached without vpn connection: " + vpn.resolveError;
                            vpnPreviousResolveError = vpn.resolveError;
                        }
                    }
                    //Maybe there was an error with RasDial phonebook, or with the dialing
                    else if (vpn.connectError != null) {
                        logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.connectError), 1);
                        if (vpnPreviousConnectError != vpn.connectError) {
                            P_API_rss = "Max attempt reached without vpn connection: " + vpn.connectError;
                            vpnPreviousConnectError = vpn.connectError;
                        }
                    }
                    logging.writeToLog(null, String.Format("[EstablishConnection] Max attempt reached without vpn connection! Increasing timeout to: {0}", counter), 1);
                }
            }
            else {
                //Now, we have vpn connection
                if(!vpn.testInternetConnection(true)) {
                    //But something can goes wrong
                    if(attempts == CONSTANTS.maxAttempt) {
                        vpnPreviousState = 4;
                        attempts = 0;
                        counter = CONSTANTS.wait;
                        //For example, the internet went down
                        if(vpn.resolveError != null) {
                            if(vpnPreviousResolveError != vpn.resolveError) {
                                P_API_rss = "Max attempt reached without vpn connection: " + vpn.resolveError;
                                vpnPreviousResolveError = vpn.resolveError;
                            }
                            logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.resolveError), 1);
                        }
                        //or just the vpn server
                        if(vpn.connectError != null) {
                            if(vpnPreviousConnectError != vpn.connectError) {
                                P_API_rss = "Max attempt reached without vpn connection: " + vpn.connectError;
                                vpnPreviousConnectError = vpn.connectError;
                            }
                            logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.connectError), 1);
                        }
                        logging.writeToLog(null, String.Format("[EstablishConnection] Max attempt achieved with vpn connection! Increasing timeout to: {0}", counter), 1);
                    }
                    else {
                        logging.writeToLog(null, String.Format("[EstablishConnection] Error: {0}", vpn.resolveError), 1);
                    }
                }
                else {
                    //If we reached this point, we are good, everything works
                    vpnPreviousState = 2;
                    logging.writeToLog(null, String.Format("[EstablishConnection] Vpn connection is alive"), 2);
                    vpnPreviousConnectError = null;
                    vpnPreviousResolveError = null;
                    P_API_rss = null;
                    P_API_rss_accepted = null;
                    attempts = 0;
                }
            }
            logging.writeToLog(null, String.Format("[EstablishConnection][End] Attempts: {0}/{1}", ++attempts, CONSTANTS.maxAttempt), 3);
            //If we are here, the username or password is wrong
            //No need to trying to reconect
            if(vpn.connectError.Contains("denied") || vpn.connectError.Contains("tilt")) {
                statusUpdate.Stop();
                papi.SendStatus(0, 0,vpn.connectError);
                vh_Vpn.ExitCode = 0;
                vh_Vpn.Stop();
            }
        }


        private void UpdateStatus(object sender, EventArgs args) {
            counter--;
            string P_API_resp = null;
            //The first start, increasing the interval
            if (statusUpdate.Interval == 1) {
                statusUpdate.Interval = 1000;
            }
            //we are waiting for next reconnect or the status checking
            if (counter == 0) {
                counter = CONSTANTS.stateInterval;
                EstablishConnection();
            }
            //Looks good! Keep it up!
            if (vpnPreviousState == 2) {
                //Sending an "OK" message to PetroLine
                papi.SendStatus(1);
            }
            //Looks bad
            else {
                //If PetroLine accepted my last rss, i won't send it again
                if(P_API_rss_accepted == P_API_rss) {
                    P_API_rss = null;
                }
                //We reached the maximum attempts, we have to wait much longer
                //Of course, we need to inform PetroLine
                if (attempts == CONSTANTS.maxAttempt) {
                    if(vpnPreviousConnectError != null) {
                        P_API_rss = "Maximum attempt reached with or without vpn connection: "+vpnPreviousConnectError;
                    }
                    else if(vpnPreviousResolveError != null) {
                        P_API_rss = "Maximum attempt reached with or without vpn connection: " + vpnPreviousResolveError;
                    }
                    P_API_resp = papi.SendStatus(0, counter, P_API_rss);
                }
                else {
                    P_API_resp = papi.SendStatus(0, counter, P_API_rss);
                }
            }
            //Petroline is accepted my message
            if (P_API_resp.Contains("RSSAccepted=1")) {
                P_API_rss_accepted = P_API_rss;
                P_API_rss = null;
            }
            //if i have to wait, because i reached the maximum attempt
            //but PetroLine informing me: "You need to reconnect right now!"
            if (P_API_resp.Contains("NeedConnect=1")) {
                counter = 0;
            }
            P_API_resp = null;
        }


        private void deleteLogFiles(object sender, EventArgs args) {
            //If i deleted the old log files
            if (logDelete.Interval == 1) {
                //i will continue to delete the old ones every day
                logDelete.Interval = 1000 * 60 * 60 * 24;
            }
            logging.deleteOldFiles();
        }
    }
}