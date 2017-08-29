using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace VPN_Connection {
    public class vpnData {
        public string password { get;  }
        public string username { get;  }
        public string host { get;  }
        public string entryName { get;  }
        public string test_ip { get;  }
        public int notificationLength { get;  }
        public int maxAttempt { get;  }
        public int stateInterval { get;  }
        public TimeSpan silentmode_start { get;  }
        public TimeSpan silentmode_end { get;  }
        public vpnData() {
            host = vpn_connection.Default.vpn_host;
            username = vpn_connection.Default.vpn_username;
            password = vpn_connection.Default.vpn_password;
            entryName = vpn_connection.Default.vpn_entry_name;
            test_ip = vpn_connection.Default.vpn_test_ip;
            notificationLength = vpn_connection.Default.notification_length * 1000;
            maxAttempt = vpn_connection.Default.max_attempt_to_reconnect;
            stateInterval = vpn_connection.Default.checking_state_interval * 1000;
            silentmode_start = vpn_connection.Default.silent_mode_start;
            silentmode_end = vpn_connection.Default.silent_mode_end;
        }
    }
}
