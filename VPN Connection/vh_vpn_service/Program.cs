using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace vh_vpn {
    class Program {
        private static logging logging = new logging();

        static void Main(String[] args) {
            logging.writeToLog(null, "A szolgáltatás elindítása folyamatban", 0);
            ServiceBase serviceBase = new vh_vpn();
            serviceBase.CanShutdown = true;
            serviceBase.CanStop = true;
            ServiceBase.Run(new vh_vpn());
        }
    }
}
