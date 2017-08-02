using System;
using System.Collections.Generic;
using System.IO;

namespace VPN_Connection {

    public class logging {

        List<string> loggingExc = new List<string>();
        public string logPath = AppDomain.CurrentDomain.BaseDirectory + "\\log"; //@".\\log\vpn_";
        public logging() {
            createLogFile();
        }

        public void deleteLog() {
            try {
                File.Delete(logPath);
            }
            catch(Exception e) {
                loggingExc.Add(e.Message);
            }
        }

        public bool createLogFile() {
            try {
                Directory.CreateDirectory(logPath);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            FileStream fs = null;
            DateTimeOffset logDateStart = new DateTimeOffset(DateTime.Now);
            try {
                if(!File.Exists(logPath)) {
                    try {
                        fs = File.Create(logPath+"\\vpn_"+ logDateStart.ToString("yyyy.MM.dd")+".txt");
                        fs.Close();
                        try {
                            StreamWriter log = new StreamWriter(logPath, true);
                            log.WriteLine(logDateStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t------------   Log Start   ------------"));
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
            return false;
        }
        public void writeToLog(List<string> multiline, string line) {
            if(createLogFile()) {
                try {
                    StreamWriter log = new StreamWriter(logPath, true);
                    DateTimeOffset logStart = new DateTimeOffset(DateTime.Now);
                    if(multiline != null) {
                        bool isFirst = true;
                        foreach(string s in multiline) {
                            if(isFirst) {
                                log.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + s));
                                isFirst = !isFirst;
                            }
                            else
                                log.WriteLine(logStart.ToString("                       ") + ("\t" + s));
                        }
                    }
                    else if(line != null) {
                        log.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + line));
                    }
                    else
                        log.WriteLine();
                    log.Close();
                }
                catch(Exception e) {
                    loggingExc.Add(e.Message);
                }
            }
        }
    }
}
