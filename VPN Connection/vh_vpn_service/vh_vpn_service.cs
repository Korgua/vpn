using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;

namespace vh_vpn {
    public partial class vh_vpn :ServiceBase { 
        private EventLog eventLog;
        private int eventId = 1;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private logging logging = new logging();
        private vpn_connector vpn_connector = new vpn_connector();
        public ServiceStatus serviceStatus;

        protected override void OnStart(string[] args) {
            serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog.Source = CONSTANTS.EVENT_LOG_SOURCE;
            logging.writeToLog(null, String.Format("A szolgáltatás elindult"),0);
            eventLog.WriteEntry("A VPN szolgáltatás elindult");

            /*Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();*/

            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            /*BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, arg) => {*/
            new manage_vpn();
            /*};
            bw.RunWorkerAsync();*/
        }

        protected override void OnStop() {
            logging.writeToLog(null, String.Format("A szolgáltatás leállt"),0);
            eventLog.WriteEntry("A VPN szolgáltatás leállt");
            vpn_connector.disconnectPPTP();
        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args) {
            eventLog.WriteEntry("A szolgáltás állapota", EventLogEntryType.Information, eventId++);
        }
        protected override void OnContinue() {
            eventLog.WriteEntry("A VPN szolgáltatás fut");
            logging.writeToLog(null, String.Format("A szolgáltatás fut"),0);
        }
        public enum ServiceState {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

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
