using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPN_Connection {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            bool result;
            var mutex = new System.Threading.Mutex(true, "VH VPN", out result);
            if(!result) {
                MessageBox.Show("Csak az egypéldányos futás engedélyezett.", "Figyelmtetés", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new vpnGUI());
            GC.KeepAlive(mutex);
        }
    }
}
