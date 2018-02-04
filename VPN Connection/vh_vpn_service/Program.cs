using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace vh_vpn {
    class Program {
        private static Logging logging = new Logging();

        static void Main(String[] args) {
            logging.WriteToLog(null, "A szolgáltatás elindítása folyamatban", 0);
            ServiceBase serviceBase = new Vh_vpn {
                CanShutdown = true,
                CanStop = true
            };
            ServiceBase.Run(new Vh_vpn());
        }
    }
}
