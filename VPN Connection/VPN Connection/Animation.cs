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
        public string notificationTextExtend { get; set; }

        private Timer AnimationTimer = new Timer();
        private double maxOpacity = 0.70;
        private int formOriginalWidth = 250;
        private int formOriginalHeight = 40;
        private int pictureBoxDimension = 32;

        public animation(int width = 250, int height = 40, double opacity = 0.7) {
            formOriginalHeight = height;
            formOriginalWidth = width;
            maxOpacity = opacity;
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
            d.Width = width;
            d.Height = height;
            logging.writeToLog(null, String.Format("[Shrink] End"));
        }
        public async void Stretch(Form d, int interval = 80, int height=500, int width=500) {
            logging.writeToLog(null, String.Format("[Stretch] Begin"));
            while (d.Height < height || d.Width< width) {
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
            while (distance > movingRightIdx) {
                await Task.Delay(interval);
                --distance;
                d.SetDesktopLocation(currentLeftPosition+movingRightIdx, 0);
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
        public void activateNotification(Form form, PictureBox pBox, Label NotificationText, /*Label NotificationTextExtend,*/ string style,int vpnStatus, int interval) {
            logging.writeToLog(null, String.Format("[activateNotification] Begin"));

            AnimationTimer.Stop();
            AnimationTimer.Interval = interval;

            changeNotification(vpnStatus);
            form.BackColor = notificationFormColor;
            form.Width = formOriginalWidth;
            form.Height = formOriginalHeight;

            NotificationText.ForeColor = notificationFontColor;
            NotificationText.BackColor = notificationFormColor;
            NotificationText.Text = notificationText;
/*
            NotificationTextExtend.ForeColor = notificationFontColor;
            NotificationTextExtend.BackColor = notificationFormColor;
            NotificationTextExtend.Text = notificationTextExtend;
            */
            pBox.Image = notificationIcon;
            pictureBoxDimension = pBox.Width + pBox.Padding.All * 2;
            form.Opacity = maxOpacity;

            AnimationTimer.Tick += (sender, args) => {
                logging.writeToLog(null, String.Format("[activateNotification][Ticker] Tick"));
                Shrink(form, 10, form.Height, pictureBoxDimension);
                AnimationTimer.Stop();
                logging.writeToLog(null, String.Format("[activateNotification][Ticker] End"));
            };

            AnimationTimer.Start();
            logging.writeToLog(null, String.Format("[activateNotification][Ticker] Start"));
            logging.writeToLog(null, String.Format("[activateNotification] End"));
        }
    }
}
