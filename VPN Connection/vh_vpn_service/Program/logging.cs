using System;
using System.Collections.Generic;
using System.IO;

namespace vh_vpn {

    public class logging {

        List<string> loggingExc = new List<string>();
        private string logPath = AppDomain.CurrentDomain.BaseDirectory + @"\\log"; //@".\\log\vpn_";
        private DateTimeOffset logDateStart;
        private bool logInConsole = true;
        private int logDepth = 3; //0: Only exceptions, 1: function fails, 2: function succes, 3: everything
        private string Prefix = String.Empty;
        public logging(string _Prefix = "vpn") {
            this.Prefix = _Prefix;
        }

        public string createLogFile() {
            logDateStart = new DateTimeOffset(DateTime.Now);
            String actualFileName = String.Empty;
            if (Prefix != null && Prefix != "" ) {
                actualFileName = "\\" + Prefix+ "_" + logDateStart.ToString("yyyy.MM.dd") + ".txt";
            }
            else actualFileName = "\\" + logDateStart.ToString("yyyy.MM.dd") + ".txt";
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
            /*string prevPrefix = this.Prefix;
            if (depth <= 1) {
                this.Prefix = "Error";
                actualFileName = createLogFile();
                if (multiline != null) {
                    writeToLog(multiline, null, 3);
                }
                else {
                    writeToLog(null, line, 3);
                }
                this.Prefix = prevPrefix;
            }
            else {
                //this.Prefix = "";
                actualFileName = createLogFile();
            }
            /*/actualFileName = createLogFile();
            using(StreamWriter log = new StreamWriter(logPath + actualFileName, true)) {
                try {
                    DateTimeOffset logStart = new DateTimeOffset(DateTime.Now);
                    if(multiline != null && depth <= logDepth) {
                        bool isFirst = true;
                        foreach(string s in multiline) {
                            if(isFirst) {
                                log.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + s));
                                if(logInConsole) {
                                    Console.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + s));
                                }
                                isFirst = false;
                            }
                            else {
                                log.WriteLine(logStart.ToString("                       ") + ("\t" + s));
                                if(logInConsole) {
                                    Console.WriteLine(logStart.ToString("                       ") + ("\t" + s));
                                }
                            }
                        }
                    }
                    else if(line != null && depth <= logDepth) {
                        log.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + line));
                        if(logInConsole) {
                            Console.WriteLine(logStart.ToString("yyyy.MM.dd HH:mm:ss:fff") + ("\t" + line));
                        }
                    }
                }
                catch(Exception e) {
                    loggingExc.Add("Writing to log file exception: " + e.Message + " --> " + line);
                }
                finally {
                    log.Close();
                }
            }
            foreach (string s in loggingExc) {
                Console.WriteLine(s);
            }
        }

        public void deleteOldFiles() {
            String[] files = Directory.GetFiles(logPath);
            List<String> _files = new List<string>();
            DateTime dt = DateTime.Now;
            bool delete = false;
            foreach (var i in files) {
                TimeSpan diff = dt.Subtract(File.GetLastWriteTime(i));
                if(diff.Days > 5) {
                    if (!delete) {
                        delete = true;
                        writeToLog(null, String.Format("[deleteOldFiles] Files that older than 5 day are deleting now..."), 3);
                    }
                    try {
                        writeToLog(null, String.Format("[deleteOldFiles] These files should be deleted right now: {0}",i), 3);
                        File.Delete(i);
                    }
                    catch(Exception e) {
                        writeToLog(null, String.Format("[deleteOldFiles][Exception] The following file can not be deleted: {0} because {1}",i,e.Message), 0);
                    }
                }
            }
            if (delete) {
                writeToLog(null, String.Format("[deleteOldFiles] Files deleted"), 3);
            }
        }
    }
}
