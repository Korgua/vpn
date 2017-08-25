using System;
using System.Collections.Generic;
using System.IO;

namespace VPN_Connection {

    public class logging {

        List<string> loggingExc = new List<string>();
        private string logPath = AppDomain.CurrentDomain.BaseDirectory + @"\\log"; //@".\\log\vpn_";
        private string actualFileName;
        private bool logInConsole = true;
        private int logDepth = 3; //0: Only exceptions, 1: function fails, 2: function succes, 3: everything
        public logging() {
            createLogFile();
        }

        /*public void deleteLog() {
            try {
                File.Delete(logPath);
            }
            catch(Exception e) {
                loggingExc.Add(e.Message);
            }
        }*/

        public bool createLogFile() {
            
            try {
                Directory.CreateDirectory(logPath);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            FileStream fs = null;
            DateTimeOffset logDateStart = new DateTimeOffset(DateTime.Now);
            actualFileName = "\\vpn_" + logDateStart.ToString("yyyy.MM.dd_HH") + ".txt";
            try {
                if(!File.Exists(logPath + actualFileName)) {
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
                        catch(Exception e) {
                            loggingExc.Add(e.Message);
                        }
                    }
                    catch(Exception e) {
                        loggingExc.Add(e.Message);
                    }
                }
                else {
                    return true;
                }
            }
            catch(Exception e) {
                loggingExc.Add(e.Message);
            }
            foreach(string s in loggingExc) {
                Console.WriteLine(s);
            }
            return false;
        }
        public void writeToLog(List<string> multiline, string line, int depth = 0) {
            if (createLogFile()) {
                try {
                    StreamWriter log = new StreamWriter(logPath+actualFileName, true);
                    DateTimeOffset logStart = new DateTimeOffset(DateTime.Now);
                    if (multiline != null && depth<=logDepth) {
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
                    try {
                        log.Close();
                    }
                    catch (Exception e) {
                        loggingExc.Add("Closing log file exception: "+e.Message);
                    }
                }
                catch(Exception e) {
                    loggingExc.Add("Writing to log file exception: "+e.Message+" --> "+line);
                }
            }
            foreach (string s in loggingExc) {
                Console.WriteLine(s);
            }
        }
    }
}
