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
        private int formOriginalWidth = 250;
        private int formOriginalHeight = 40;
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
                Console.WriteLine("d.height: {0}/{1}, d.width:{2}/{3}", d.Height, height, d.Width, width);
            }
            Console.WriteLine("d.height: {0}, d.width: {1}, height: {2}, width: {3}", d.Height, d.Width, height, width);
            d.Width = width;
            d.Height = height;
            logging.writeToLog(null, String.Format("[Shrink] End"));
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

        public async void MoveRight(Form d, int interval = 0, int distance = 250) {
            int currentLeftPosition = d.Location.X;
            int movingRightIdx = 1;
            Console.WriteLine("Screen.FromPoint(this.Location).WorkingArea.Right: {0}",Screen.FromPoint(d.Location).WorkingArea.Right);
            while (distance > movingRightIdx) {
                await Task.Delay(interval);
                --distance;
                d.SetDesktopLocation(currentLeftPosition+movingRightIdx, 0);
                Console.WriteLine("currentLeftPosition+movingRightIdx: {0}", currentLeftPosition + movingRightIdx);
                movingRightIdx += 8;
            }
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
        public void activateNotification(Form form, PictureBox pBox, Label label, string style,int vpnStatus, int interval) {
            logging.writeToLog(null, String.Format("[activateNotification] Begin"));
            changeNotification(vpnStatus);
            form.BackColor = notificationFormColor;
            label.ForeColor = notificationFontColor;
            label.BackColor = notificationFormColor;
            label.Text = notificationText;
            pBox.Image = notificationIcon;
            switch (style) {
                case "fade":
                    FadeIn(form, 20);
                    break;
                case "stretch":
                    Stretch(form, 10, formOriginalHeight, formOriginalWidth);
                    Console.WriteLine("pBox.Padding: {0}", pBox.Width + pBox.Padding.All);
                    break;
            }
            Timer AnimationControl = new Timer();
            Console.WriteLine("Interval: {0}", interval);
            AnimationControl.Interval = interval;
            AnimationControl.Tick += (sender, args) => {
                logging.writeToLog(null, String.Format("[activateNotification][FadeOutTicker] Tick"));
                //FadeOut(form, 20);
                switch (style) {
                    case "fade":
                        FadeOut(form, 20);
                        break;
                    case "stretch":
                        Shrink(form, 10, form.Height, pBox.Width + pBox.Padding.All * 2);
                        Console.WriteLine("pBox.Padding: {0}", pBox.Width + pBox.Padding.All);
                        break;
                }
                AnimationControl.Stop();
                logging.writeToLog(null, String.Format("[activateNotification][FadeOutTicker] End"));
            };
            AnimationControl.Start();
            logging.writeToLog(null, String.Format("[activateNotification][Ticker] Start"));
            logging.writeToLog(null, String.Format("[activateNotification] End"));
        }
    }
}
