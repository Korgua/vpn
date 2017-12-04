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
        public bool shrinkEnded = false;
        public bool stretchEnded = false;

        private BackgroundWorker BW;
        private Form formOriginal;
        private PictureBox pBoxOriginal;
        private Label formOriginalNotification;
        private Label formOriginalNotificationExtend;
        private double formMaxOpacity = 0.80;
        public int formOriginalWidth = 250;
        public int formOriginalHeight = 40;
        private int formPictureBoxDimension = 32;
        private int formInterval = 5000;

        public animation(Form form,int width = 250, int height = 40, double opacity = 0.80, int interval = 5000) {
            formOriginalHeight = height;
            formOriginalWidth = width;
            formMaxOpacity = opacity >= 0.8 ? 0.8 : opacity;
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
            }
            form.Opacity = formMaxOpacity;
            formOriginal = form;
            formPictureBoxDimension = pBoxOriginal.Width + pBoxOriginal.Padding.All * 2;
        }

        public void changeNotification(int type) {
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
                /*default:
                    notificationIcon = VPN_Connection.Properties.Resources.info;
                    notificationFontColor = System.Drawing.Color.CornflowerBlue;
                    notificationFormColor = System.Drawing.Color.PowderBlue;
                    notificationText = "Csatlakozás folyamatban...";
                    break;*/
            }
            List<String> data = new List<string>();
            data.Add(String.Format("vpnStatus: {0}", type));
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
        public void activateNotification(Form form, int vpnStatus, string error = "") {
            logging.writeToLog(null, String.Format("[activateNotification] Begin"), 3);
            logging.writeToLog(null, String.Format("[activateNotification] vpnStatus: {0}",vpnStatus), 3);
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


            BW = new BackgroundWorker();
            BW.WorkerSupportsCancellation = true;
            BW.WorkerReportsProgress = true;
            BW.DoWork += Stretch;
            BW.RunWorkerCompleted += (_sender, args) => {
                BW.DoWork -= Stretch;
                BW.Dispose();
            };
            BW.RunWorkerAsync();

            BW = new BackgroundWorker();
            BW.WorkerSupportsCancellation = true;
            BW.WorkerReportsProgress = true;
            BW.DoWork += Shrink;
            BW.RunWorkerCompleted += (_sender, args) => {
                BW.DoWork -= Shrink;
                BW.Dispose();
                if(vpnStatus>=3) {
                    activateNotification(form, 1);
                }
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

        public void Shrink(object sender, DoWorkEventArgs args) {
            Thread.Sleep(formInterval);
            try {
                formOriginal.Invoke(new MethodInvoker(delegate {
                    while(formOriginal.Height > formPictureBoxDimension || formOriginal.Width > formPictureBoxDimension) {
                        Thread.Sleep(10);
                        if(formOriginal.Height > formPictureBoxDimension) {
                            formOriginal.Height -= 10;
                        }
                        if(formOriginal.Width > formPictureBoxDimension) {
                            formOriginal.Width -= 10;
                        }
                    }
                    formOriginal.Height = formPictureBoxDimension;
                    formOriginal.Width = formPictureBoxDimension;
                }));
            }
            catch (Exception ex) {
                logging.writeToLog(null, String.Format("[BWShrink]Exception: {0}", ex.Message));
            }
        }

    }
}