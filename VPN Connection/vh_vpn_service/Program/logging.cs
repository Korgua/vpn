using System;
using System.Collections.Generic;
using System.IO;

namespace vh_vpn {

    public class logging {

        List<string> loggingExc = new List<string>();
        private string logPath = AppDomain.CurrentDomain.BaseDirectory + @"\\log"; //@".\\log\vpn_";
        private DateTimeOffset logDateStart;
        //private string actualFileName;
        private bool logInConsole = true;
        private int logDepth = 3; //0: Only exceptions, 1: function fails, 2: function succes, 3: everything
        public logging() {
            //createLogFile();
        }

        public string createLogFile(string prefix) {
            logDateStart = new DateTimeOffset(DateTime.Now);
            String actualFileName = String.Empty;
            if (prefix != null) {
                actualFileName = "\\" + prefix+ "_" + logDateStart.ToString("yyyy.MM.dd_HH") + "_" + Environment.UserName + ".txt";
            }
            else actualFileName = "\\" + logDateStart.ToString("yyyy.MM.dd_HH") + "_" + Environment.UserName + ".txt";
            try {
                Directory.CreateDirectory(logPath);
                FileStream fs = null;
                try {
                    Console.WriteLine(String.Format("Actual Filename: {0}", actualFileName));
                    if (!File.Exists(logPath + actualFileName)) {
                        try {
                            fs = File.Create(logPath + actualFileName);
                            fs.Close();
                            try {
                                StreamWriter log = new StreamWriter(logPath + actualFileName, true);
                                log.WriteLine(logDateStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t------------   Log Start   ------------"));
                                if (logInConsole) {
                                    Console.WriteLine(logDateStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t------------   Log Start   ------------"));
                                }
                                log.Close();
                                return actualFileName;
                            }
                            catch (Exception e__) {
                                Console.WriteLine(String.Format("Logging exception while insert Log start: {0}", e__.Message));
                                loggingExc.Add(e__.Message);
                            }
                        }
                        catch (Exception e___) {
                            Console.WriteLine(String.Format("Logging exception while create log file: {0}", e___.Message));
                            loggingExc.Add(e___.Message);
                        }
                    }
                    else {
                        return actualFileName;
                    }
                }
                catch (Exception e_) {
                    Console.WriteLine(String.Format("Logging exception while checking log file existence: {0}", e_.Message));
                    loggingExc.Add(e_.Message);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                foreach (string s in loggingExc) {
                    Console.WriteLine(s);
                }
            }
            return null;
        }
        public void writeToLog(List<string> multiline, string line, int depth = 0) {
            string actualFileName = String.Empty;
            if (depth <= 1) {
                actualFileName = createLogFile("Error");
                if (multiline != null) {
                    writeToLog(multiline, null, 3);
                }
                else {
                    writeToLog(null, line, 3);
                }
            }
            else {
                actualFileName = createLogFile(null);
            }
            try {
                StreamWriter log = new StreamWriter(logPath + actualFileName, true);
                DateTimeOffset logStart = new DateTimeOffset(DateTime.Now);
                if (multiline != null && depth <= logDepth) {
                    bool isFirst = true;
                    foreach (string s in multiline) {
                        if (isFirst) {
                            log.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + s));
                            if (logInConsole) {
                                Console.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + s));
                            }
                            isFirst = false;
                        }
                        else {
                            log.WriteLine(logStart.ToString("                       ") + ("\t" + s));
                            if (logInConsole) {
                                Console.WriteLine(logStart.ToString("                       ") + ("\t" + s));
                            }
                        }
                    }
                }
                else if (line != null && depth <= logDepth) {
                    log.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + line));
                    if (logInConsole) {
                        Console.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + line));
                    }
                }
                log.Close();
            }
            catch (Exception e) {
                loggingExc.Add("Writing to log file exception: " + e.Message + " --> " + line);
            }

            foreach (string s in loggingExc) {
                Console.WriteLine(s);
            }
        }
    }
}
