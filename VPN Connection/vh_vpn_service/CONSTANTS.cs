using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vh_vpn {
    class CONSTANTS {

        //store from config.xml
        public string password { get; }
        public string username { get; }
        public string host { get; }
        public string entryName { get; }
        public string test_ip { get; }
        public int maxAttempt { get; }
        public int stateInterval { get; }
        public int wait { get; }
        public string backupDir { get; }

        //HardCoded constants
        public static string EVENT_LOG_SOURCE = "VH VPN event log";



        public CONSTANTS() {
            host = vpn.Default.vpn_host;
            username = vpn.Default.vpn_username;
            password = vpn.Default.vpn_password;
            entryName = vpn.Default.vpn_entry_name;
            test_ip = vpn.Default.vpn_test_ip;
            maxAttempt = vpn.Default.max_attempt_to_reconnect;
            stateInterval = vpn.Default.checking_state_interval * 1000;
            backupDir = vpn.Default.BackupDirectory;
            wait = vpn.Default.wait_after_failed_connection * 1000;
        }
    }
}
