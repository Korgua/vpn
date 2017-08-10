using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace VPN_Connection {
    public class vpnData {
        public string password { get; set; }
        public string username { get; set; }
        public string host { get; set; }
        public string entryName { get; set; }
        public string test_ip { get; set; }
        public bool notification { get; set; }
        public int notificationLength { get; set; }
        public int maxAttempt { get; set; }
        public int stateInterval { get; set; }
        public vpnData() {
            host = vpn_connection.Default.vpn_host;
            username = vpn_connection.Default.vpn_username;
            password = vpn_connection.Default.vpn_password;
            entryName = vpn_connection.Default.vpn_entry_name;
            test_ip = vpn_connection.Default.vpn_test_ip;
            notification = vpn_connection.Default.notification;
            notificationLength = vpn_connection.Default.notification_length * 1000;
            maxAttempt = vpn_connection.Default.max_attempt_to_reconnect;
            stateInterval = vpn_connection.Default.checking_state_interval * 1000;
        }
    }
}
