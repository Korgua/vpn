using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace vh_vpn {
    public partial class vh_vpn :ServiceBase { 
        private EventLog eventLog;

        private logging logging = new logging();
        private Manage_vpn manage_Vpn;

        protected override void OnStart(string[] args) {
            eventLog.Source = CONSTANTS.EVENT_LOG_SOURCE;
            logging.writeToLog(null, String.Format("A szolgáltatás elindult"), 2);
            eventLog.WriteEntry("A VPN szolgáltatás elindult");
            manage_Vpn = new Manage_vpn(this);
        }

        protected override void OnStop() {
            logging.writeToLog(null, String.Format("A szolgáltatás leállt"),2);
            eventLog.WriteEntry("A VPN szolgáltatás leállt");
            manage_Vpn.vpn.disconnectPPTP();
        }

        protected override void OnContinue() {
            eventLog.WriteEntry("A VPN szolgáltatás fut");
            logging.writeToLog(null, String.Format("A szolgáltatás fut"),0);
        }
        

        public vh_vpn() {
            InitializeComponent();
            eventLog = new EventLog();
            try {
                if (!EventLog.SourceExists(CONSTANTS.EVENT_LOG_SOURCE)) {
                    EventLog.CreateEventSource(CONSTANTS.EVENT_LOG_SOURCE, CONSTANTS.EVENT_LOG_SOURCE);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
