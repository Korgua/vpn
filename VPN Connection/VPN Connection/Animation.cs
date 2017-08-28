using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
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

        private System.Windows.Forms.Timer AnimationTimer = new System.Windows.Forms.Timer();
        private BackgroundWorker BW;
        private Form formOriginal;
        private PictureBox pBoxOriginal;
        private Label formOriginalNotification;
        private Label formOriginalNotificationExtend;
        private double formMaxOpacity = 0.80;
        public int formOriginalWidth = 250;
        public int formOriginalHeight = 40;
        private int formPictureBoxDimension = 32;
        private int formInterval = 10;

        public animation(Form form,int width = 250, int height = 40, double opacity = 0.80, int interval = 5000) {
            formOriginalHeight = height;
            formOriginalWidth = width;
            formMaxOpacity = opacity > 0.7 ? 0.7 : opacity;
            formInterval = interval;
            foreach(Control control in form.Controls) {
                if(control.Name == "notificationText") {
                    formOriginalNotification = (Label)control;
                }
                else if(control.Name == "notificationTextExtend") {
                    formOriginalNotificationExtend = (Label)control;
                }
                else if(control.Name == "notificationStatusIcon") {
                    pBoxOriginal = (PictureBox)control;
                }
                Console.WriteLine(control.Name);
            }
            form.Opacity = formMaxOpacity;
            formOriginal = form;
            formPictureBoxDimension = pBoxOriginal.Width + pBoxOriginal.Padding.All * 2;
        }

        public void changeNotification(int type = 1) {
            switch(type) {
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
                case 4:
                    notificationIcon = VPN_Connection.Properties.Resources.error;
                    notificationFontColor = System.Drawing.Color.Firebrick;
                    notificationFormColor = System.Drawing.Color.DarkSalmon;
                    notificationText = "A kapcsolat megszakadt";
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
            logging.writeToLog(data, null, 3);
            data = null;
        }

        /// <summary>
        ///  Activating the notification
        /// </summary>
        /// <param name="form">Current/active form</param>
        /// <param name="vpnStatus">Used by the applicable notification</param>
        /// <param name="error">Showed by as extra information in notification</param>
        public void activateNotification(Form form, int vpnStatus=0, string error = "") {
            logging.writeToLog(null, String.Format("[activateNotification] Begin"), 3);
            form.Invoke(new MethodInvoker(delegate {
                formOriginal.Width = formOriginalWidth;
                formOriginal.Height = formOriginalHeight;
                AnimationTimer.Stop();
                AnimationTimer.Dispose();
                AnimationTimer.Interval = formInterval;

                changeNotification(vpnStatus);

                formOriginal.BackColor = notificationFormColor;
                formOriginalNotification.ForeColor = notificationFontColor;
                formOriginalNotification.BackColor = notificationFormColor;
                formOriginalNotification.Text = notificationText;
                formOriginalNotificationExtend.ForeColor = notificationFontColor;
                formOriginalNotificationExtend.BackColor = notificationFormColor;
                formOriginalNotificationExtend.Text = error;
                pBoxOriginal.Image = notificationIcon;
                form = formOriginal;
            }));
            BW = new BackgroundWorker();
            BW.WorkerSupportsCancellation = true;
            BW.WorkerReportsProgress = true;
            BW.DoWork += (sender, args) => Shrink(form);
            BW.RunWorkerCompleted += (_sender, args) => {
                BW.DoWork -= (sender_, args_) => Shrink(form);
                BW.CancelAsync();
                BW.Dispose();
            };
            BW.RunWorkerAsync();
            logging.writeToLog(null, String.Format("[activateNotification] End"), 3);
        }

        public void Stretch(object sender, DoWorkEventArgs args) {
            try {
                formOriginal.Invoke(new MethodInvoker(delegate {
                    while(formOriginal.Width < formOriginalWidth) {
                        Thread.Sleep(10);
                        if(formOriginalWidth > formOriginal.Width) {
                            formOriginal.Width += 10;
                        }
                    }
                    formOriginal.Height = formOriginalHeight;
                    formOriginal.Width = formOriginalWidth;
                }));
            }
            catch(Exception ex) {
                logging.writeToLog(null, String.Format("[BWShrink]Exception: {0}", ex.Message));
            }
        }

        private void Shrink(Form form) {
            Thread.Sleep(3000);
            try {
                form.Invoke(new MethodInvoker(delegate {
                    while(formOriginal.Height > 40 || formOriginal.Width > 40) {
                        Thread.Sleep(10);
                        if(formOriginal.Height > 40) {
                            formOriginal.Height -= 10;
                        }
                        if(formOriginal.Width > 40) {
                            formOriginal.Width -= 10;
                        }
                    }
                    form.Height = 40;
                    form.Width = 40;
                }));
            }
            catch(Exception ex) {
                logging.writeToLog(null, String.Format("[BWShrink]Exception: {0}", ex.Message));
            }
        }

    }
}