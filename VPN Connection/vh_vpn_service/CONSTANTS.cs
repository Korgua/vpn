using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace vh_vpn {
    class CONSTANTS {

        private Encryption encryption = new Encryption();
        private logging logging = new logging();

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
        public static string CONFIG_FILE_PATH = AppDomain.CurrentDomain.BaseDirectory + @"\\vh_vpn.exe.config";


        public CONSTANTS(string whichClass) {
            logging.writeToLog(null, String.Format("[CONSTANTS][{0}] Reading up the constants",whichClass),3);
            /*checkConfigFile();
            EncryptConfigFile();*/
            try {
                host = encryption.Decrypt(vpn.Default.vpn_host);
                username = encryption.Decrypt(vpn.Default.vpn_username);
                password = encryption.Decrypt(vpn.Default.vpn_password);
                test_ip = encryption.Decrypt(vpn.Default.vpn_test_ip);
            }
            catch(Exception e) {
                logging.writeToLog(null, String.Format("[CONSTANTS][{0}]Exception while decrypting: {1}",whichClass,e.Message),0);
            }
            entryName = vpn.Default.vpn_entry_name;
            maxAttempt = vpn.Default.max_attempt_to_reconnect;
            stateInterval = vpn.Default.checking_state_interval * 1000;
            backupDir = vpn.Default.BackupDirectory;
            wait = vpn.Default.wait_after_failed_connection * 1000;
        }

        public void checkConfigFile() {
            logging.writeToLog(null, String.Format("[checkConfigFile]Start"),3);
            if (!File.Exists(CONFIG_FILE_PATH)) {
                logging.writeToLog(null, String.Format("[checkConfigFile]There is no config file. Trying to create it"),3);
                try {
                    using (FileStream fs = File.Create(CONFIG_FILE_PATH)) {
                        fs.Close();
                        try {
                            StreamWriter CONFIG_FILE = new StreamWriter(CONFIG_FILE_PATH, true);
                            CONFIG_FILE.Write(
"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
"<configuration>\n" +
"   <configSections>\n" +
"       <sectionGroup name=\"applicationSettings\" type=\"System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" >\n" +
"           <section name=\"vh_vpn.vpn\" type=\"System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" requirePermission=\"false\" />\n" +
"       </sectionGroup>\n" +
"   </configSections>\n" +
"   <startup> " +
"       <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.5\"/>\n" +
"   </startup>\n" +
"   <applicationSettings>\n" +
"       <vh_vpn.vpn>\n" +
"           <setting name=\"vpn_host\" serializeAs=\"String\">\n" +
"               <value>www.petrolcard.hu</value>\n" +
"           </setting>\n" +
"           <setting name=\"vpn_password\" serializeAs=\"String\">\n" +
"               <value>jundjkft</value>\n" +
"           </setting>\n" +
"           <setting name=\"vpn_username\" serializeAs=\"String\">\n" +
"               <value>jundjkft</value>\n" +
"           </setting>\n" +
"           <setting name=\"vpn_entry_name\" serializeAs=\"String\">\n" +
"               <value>vh-vpn</value>\n" +
"           </setting>\n" +
"           <setting name=\"max_attempt_to_reconnect\" serializeAs=\"String\">\n" +
"               <value>5</value>\n" +
"           </setting>\n" +
"           <setting name=\"checking_state_interval\" serializeAs=\"String\">\n" +
"               <value>5</value>\n" +
"           </setting>\n" +
"           <setting name=\"vpn_test_ip\" serializeAs=\"String\">\n" +
"               <value>192.168.75.8</value>\n" +
"           </setting>\n" +
"           <setting name=\"BackupDirectory\" serializeAs=\"String\">\n" +
"               <value>DefaultFolder</value>\n" +
"           </setting>\n" +
"           <setting name=\"wait_after_failed_connection\" serializeAs=\"String\">\n" +
"               <value>60</value>\n" +
"           </setting>\n" +
"       </vh_vpn.vpn>\n" +
"   </applicationSettings>\n" +
"</configuration>"
                                    );
                            CONFIG_FILE.Close();
                            logging.writeToLog(null, String.Format("[checkConfigFile]Config file created"),2);
                        }
                        catch (Exception e__) {
                            logging.writeToLog(null,String.Format("[ConfigFile]Exception while insert into {0}: {1}", CONFIG_FILE_PATH, e__.Message),0);
                        }
                    }
                }
                catch (Exception e___) {
                    logging.writeToLog(null, String.Format("[ConfigFile]Exception while create log file: {0}  --> {1}", CONFIG_FILE_PATH, e___.Message),0);
                }
            }
            else {
                logging.writeToLog(null, String.Format("[checkConfigFile]Config file is available"),3);
            }
            logging.writeToLog(null, String.Format("[checkConfigFile]End"),3);
        }

        public void EncryptConfigFile() {
            logging.writeToLog(null, string.Format("[EncryptConfigFile]Start"),3);
            XmlDocument xmlDoc = new XmlDocument();
            try {
                xmlDoc.Load(CONFIG_FILE_PATH);
                XmlNodeList elemList;
                int i = 0;
                try {
                    elemList = xmlDoc.GetElementsByTagName("setting");
                    for (i = 0;i <= elemList.Count - 1;i++) {
                        if (elemList[i].Attributes["name"].Value == "vpn_host") {
                            string temp = elemList[i].InnerText;
                            if (!temp.Contains("@")) {
                                logging.writeToLog(null, string.Format("[EncryptConfigFile]vpn host name is plain text! Encrypting..."),2);
                                elemList[i].InnerXml = "<value>@" + encryption.Encrypt(temp) + "@</value>";
                                xmlDoc.Save(CONFIG_FILE_PATH);
                                logging.writeToLog(null, string.Format("[EncryptConfigFile]Encryption finished"),2);
                            }
                        }
                        else if (elemList[i].Attributes["name"].Value == "vpn_password") {
                            string temp = elemList[i].InnerText;
                            if (!temp.Contains("@")) {
                                logging.writeToLog(null, string.Format("[EncryptConfigFile]vpn password is plain text! Encrypting..."),2);
                                elemList[i].InnerXml = "<value>@" + encryption.Encrypt(temp) + "@</value>";
                                xmlDoc.Save(CONFIG_FILE_PATH);
                                logging.writeToLog(null, string.Format("[EncryptConfigFile]Encryption finished"),2);
                            }
                        }
                        else if (elemList[i].Attributes["name"].Value == "vpn_username") {
                            string temp = elemList[i].InnerText;
                            if (!temp.Contains("@")) {
                                logging.writeToLog(null, string.Format("[EncryptConfigFile]vpn username is plain text! Encrypting..."),2);
                                elemList[i].InnerXml = "<value>@" + encryption.Encrypt(temp) + "@</value>";
                                xmlDoc.Save(CONFIG_FILE_PATH);
                                logging.writeToLog(null, string.Format("[EncryptConfigFile]Encryption finished"),2);
                            }
                        }
                        else if (elemList[i].Attributes["name"].Value == "vpn_test_ip") {
                            string temp = elemList[i].InnerText;
                            if (!temp.Contains("@")) {
                                logging.writeToLog(null, string.Format("[EncryptConfigFile]vpn test ip is plain text! Encrypting..."),2);
                                elemList[i].InnerXml = "<value>@" + encryption.Encrypt(temp) + "@</value>";
                                xmlDoc.Save(CONFIG_FILE_PATH);
                                logging.writeToLog(null, string.Format("[EncryptConfigFile]Encryption finished"),2);
                            }
                        }
                    }
                }
                catch (Exception e) {
                    logging.writeToLog(null, String.Format("[EncryptConfigFile] Exception found while parsing the config file: {0}",e.Message),0);
                }
            }
            catch (Exception e) {
                logging.writeToLog(null, String.Format("[EncryptConfigFile]Exception while open the config file at: {0} --> {1}", CONFIG_FILE_PATH, e.Message),0);
            }
            logging.writeToLog(null, string.Format("[EncryptConfigFile]End"),3);
        }
    }
}
