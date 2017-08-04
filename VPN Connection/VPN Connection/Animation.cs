using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPN_Connection {
    public class animation {
        public Color fontColor { get; set; }
        public Color formColor { get; set; }
        public Image Icon { get; set; }

        public async void FadeIn(Form o, int interval = 80) {
            while(o.Opacity < 1.0) {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = 1;    
        }

        public async void FadeOut(Form o, int interval = 80) {
            while(o.Opacity > 0.0) {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }
            o.Opacity = 0;   
        }

        public void changeIcon(Form o,int type=1) {
            switch(type) {
                case 1:
                    Icon = VPN_Connection.Properties.Resources.info;
                    fontColor = System.Drawing.Color.CornflowerBlue;
                    formColor = System.Drawing.Color.PowderBlue;
                    break;
                case 2:
                    Icon = VPN_Connection.Properties.Resources.tick;
                    fontColor = System.Drawing.Color.ForestGreen;
                    formColor = System.Drawing.Color.PaleTurquoise;
                    break;
                case 3:
                    Icon = VPN_Connection.Properties.Resources.error;
                    fontColor = System.Drawing.Color.Firebrick;
                    formColor = System.Drawing.Color.DarkSalmon;
                    break;
                default:
                    Icon = VPN_Connection.Properties.Resources.info;
                    fontColor = System.Drawing.Color.CornflowerBlue;
                    formColor = System.Drawing.Color.PowderBlue;
                    break;
            }
        }
    }
}
