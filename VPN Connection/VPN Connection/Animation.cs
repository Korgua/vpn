using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPN_Connection {
    public class animation {
        private logging logging = new logging();
        public Color notificationFontColor { get; set; }
        public Color notificationFormColor { get; set; }
        public Image notificationIcon { get; set; }
        public string notificationText { get; set; }
        private double maxOpacity = 0.70;
        public async void FadeIn(Form o, int interval = 80) {
            logging.writeToLog(null, String.Format("[FadeIn] Begin"));
            while (o.Opacity < maxOpacity) {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = maxOpacity;
            logging.writeToLog(null, String.Format("[FadeIn] End"));
        }

        public async void FadeOut(Form o, int interval = 80) {
            logging.writeToLog(null, String.Format("[FadeOut] Begin"));
            while (o.Opacity > 0.0) {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }
            o.Opacity = 0;
            logging.writeToLog(null, String.Format("[FadeOut] End"));
        }

        public async void Shrink(Form d, int interval = 80, int height = 128, int width = 128) {
            logging.writeToLog(null, String.Format("[Shrink] Begin"));
            while (d.Height > height || d.Width > width) {
                await Task.Delay(interval);
                if (d.Height > height) {
                    d.Height -= 10;
                }
                if (d.Width > width) {
                    d.Width -= 10;
                }
            }
            Console.WriteLine("Before sleep");
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("After sleep");
            d.Width = width;
            d.Height = height;
            logging.writeToLog(null, String.Format("[Shrink] End"));
            Stretch(d, 10, 32, 256);
        }

        public async void Stretch(Form d, int interval = 80, int height=500, int width=500) {
            logging.writeToLog(null, String.Format("[Stretch] Begin"));
            while (d.Height < height || d.Width< width) {
                logging.writeToLog(null, String.Format("[Stretch] d.height: {0}/{1}, d.width:{2}/{3}", d.Height, height, d.Width, width)); ;
                await Task.Delay(interval);
                if (d.Height < height) {
                    d.Height += 10;
                }
                if (d.Width < width) {
                    d.Width += 10;
                }
            }
            d.Width = width;
            d.Height = height;
            logging.writeToLog(null, String.Format("[Stretch] End"));
        }

        public void changeNotification(int type = 1) {
            switch (type) {
                case 0:
                    notificationIcon = VPN_Connection.Properties.Resources.error;
                    notificationFontColor = System.Drawing.Color.Firebrick;
                    notificationFormColor = System.Drawing.Color.DarkSalmon;
                    notificationText = "Sikeresen lekapcsolódva";
                    break;
                case 1:
                    notificationIcon = VPN_Connection.Properties.Resources.info;
                    notificationFontColor = System.Drawing.Color.CornflowerBlue;
                    notificationFormColor = System.Drawing.Color.PowderBlue;
                    notificationText = "Csatlakozás folyamatban...";
                    break;
                case 2:
                    notificationIcon = VPN_Connection.Properties.Resources.tick;
                    notificationFontColor = System.Drawing.Color.ForestGreen;
                    notificationFormColor = System.Drawing.Color.PaleTurquoise;
                    notificationText = "Csatlakoztatva";
                    break;
                case 3:
                    notificationIcon = VPN_Connection.Properties.Resources.error;
                    notificationFontColor = System.Drawing.Color.Firebrick;
                    notificationFormColor = System.Drawing.Color.DarkSalmon;
                    notificationText = "Csatlakozás sikertelen";
                    break;
                default:
                    notificationIcon = VPN_Connection.Properties.Resources.info;
                    notificationFontColor = System.Drawing.Color.CornflowerBlue;
                    notificationFormColor = System.Drawing.Color.PowderBlue;
                    notificationText = "Csatlakozás folyamatban...";
                    break;
            }
            List<String> data = new List<string>();
            data.Add(String.Format("notificationIcon: {0}", notificationIcon));
            data.Add(String.Format("notificationFontColor: {0}", notificationFontColor));
            data.Add(String.Format("notificationFormColor: {0}", notificationFormColor));
            data.Add(String.Format("notificationText: {0}", notificationText));
            logging.writeToLog(data, null);
            data = null;
        }
        public void activateNotification(Form form, PictureBox pBox, Label label, int type, int interval) {
            logging.writeToLog(null, String.Format("[activateNotification] Begin"));
            changeNotification(type);
            form.BackColor = notificationFormColor;
            label.ForeColor = notificationFontColor;
            label.BackColor = notificationFormColor;
            label.Text = notificationText;
            pBox.Image = notificationIcon;
            FadeIn(form, 20);
            Timer fadeOut = new Timer();
            Console.WriteLine("Interval: {0}", interval);
            fadeOut.Interval = interval;
            fadeOut.Tick += (sender, args) => {
                logging.writeToLog(null, String.Format("[activateNotification][FadeOutTicker] Tick"));
                //FadeOut(form, 20);
                fadeOut.Stop();
                logging.writeToLog(null, String.Format("[activateNotification][FadeOutTicker] End"));
            };
            fadeOut.Start();
            logging.writeToLog(null, String.Format("[activateNotification][Ticker] Start"));
            logging.writeToLog(null, String.Format("[activateNotification] End"));
        }
    }
}
