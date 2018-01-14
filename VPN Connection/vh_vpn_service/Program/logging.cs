using System;
using System.Collections.Generic;
using System.IO;

namespace vh_vpn {

    public class logging {

        List<string> loggingExc = new List<string>();
        private string logPath = AppDomain.CurrentDomain.BaseDirectory + @"\\log"; //@".\\log\vpn_";
        private DateTimeOffset logDateStart;
        private string actualFileName;
        private bool logInConsole = true;
        private int logDepth = 3; //0: Only exceptions, 1: function fails, 2: function succes, 3: everything
        public logging() {
            //createLogFile();
        }

        public bool createLogFile() {
            logDateStart = new DateTimeOffset(DateTime.Now);
            actualFileName = "\\vpn_" + logDateStart.ToString("yyyy.MM.dd_HH") + "_" + Environment.UserName + ".txt";
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
                                return true;
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
                        return true;
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
            return false;
        }
        public void writeToLog(List<string> multiline, string line, int depth = 0) {
            createLogFile();
            try {
                StreamWriter log = new StreamWriter(logPath + actualFileName, true);
                DateTimeOffset logStart = new DateTimeOffset(DateTime.Now);
                if (multiline != null && (depth <= logDepth || depth == 10)) {
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
                else if (line != null && (depth <= logDepth || depth == 10)) {
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
