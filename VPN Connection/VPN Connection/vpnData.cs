﻿using System;
using System.IO;
using System.Xml;

namespace VPN_Connection {
    public class vpnData {
        public string password { get; set; }
        public string username { get; set; }
        public string host { get; set; }
        public string entryName { get; set; }
        public int notification { get; set; }
        public int notificationLength { get; set; }
        public int maxAttempt { get; set; }
        public int wait { get; set; }
        public int stateInterval { get; set; }
        private logging log = new logging();
        public vpnData() {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\config.xml";
            try {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    try {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(fs);
                        XmlElement root = xmlDoc.DocumentElement;
                        XmlNodeList nodes = root.SelectNodes("vpn_connection");
                        try {
                            foreach (XmlNode xnode in nodes) {
                                username = xnode["vpn_username"].InnerText;
                                password = xnode["vpn_password"].InnerText;
                                host = xnode["vpn_host"].InnerText;
                                entryName = xnode["vpn_entry_name"].InnerText;

                            }
                            nodes = root.SelectNodes("customize");
                            foreach (XmlNode xnode in nodes) {
                                notification = int.Parse(xnode["notification"].InnerText);
                                notificationLength = int.Parse(xnode["notification_length"].InnerText) * 1000;
                                maxAttempt = int.Parse(xnode["max_attempt_to_reconnect"].InnerText);
                                stateInterval = int.Parse(xnode["checking_state_interval"].InnerText) * 1000;
                            }
                            Console.WriteLine(String.Format("NotificationLength: {0}", notificationLength));
                        }
                        catch (Exception e) {
                            log.writeToLog(null, String.Format("An exception was found while store data from  {0} --> {1}", path, e.Message));
                            Console.WriteLine(String.Format("An exception was found while store data from  {0} --> {1}", path, e.Message));
                        }
                    }
                    catch (Exception e) {
                        log.writeToLog(null, String.Format("An exception was found while parsing {0} --> {1}", path, e.Message));
                        Console.WriteLine(String.Format("An exception was found while parsing { 0} --> {1}", path, e.Message));
                    }
                }
            }
            catch (Exception e) {
                log.writeToLog(null, String.Format("An exception was found while open {0} --> {1}", path, e.Message));
                Console.WriteLine(String.Format("An exception was found while open {0} --> {1}", path, e.Message));
            }
        }
    }
}
