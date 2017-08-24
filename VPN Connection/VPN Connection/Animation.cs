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
        private double formMaxOpacity = 0.70;
        private int formOriginalWidth = 250;
        private int formOriginalHeight = 40;
        private int formPictureBoxDimension = 32;
        private int formInterval = 10;

        public animation(Form form, PictureBox pBox, int width = 250, int height = 40, double opacity = 0.7, int interval = 5000) {
            formOriginalHeight = height;
            formOriginalWidth = width;
            formOriginal = form;
            formMaxOpacity = opacity;
            formInterval = interval;
            formPictureBoxDimension = pBox.Width + pBox.Padding.All * 2;
            pBoxOriginal = pBox;
        }

        public void BWShrink(Form d, int interval = 80, int height = 40, int width = 40) {
            logging.writeToLog(null, String.Format("[BWShrink] Begin"));
            /*while (d.Height > height || d.Width > width) {
                Thread.Sleep(10);
                if (d.Height > height) {
                    d.Height -= 10;
                }
                if (d.Width > width) {
                    d.Width -= 10;
                }
            }
            d.Width = width;
            d.Height = height;*/
            logging.writeToLog(null, String.Format("[BWShrink] End"));
        }

        public async void Shrink(Form d, int interval = 80, int height = 40, int width = 40) {
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

        public void testing(Object obj, EventArgs args) {
            logging.writeToLog(null, String.Format("[activateNotification][Ticker] Tick"));
            Shrink(formOriginal, 10, 40,40);
            Console.WriteLine("testing...");
            AnimationTimer.Stop();
            AnimationTimer.Tick -= testing;
            logging.writeToLog(null, String.Format("[activateNotification][Ticker] End"));
        }


        public void activateNotification(Form form,Label NotificationText,int vpnStatus) {
            logging.writeToLog(null, String.Format("[activateNotification] Begin"));
            Console.WriteLine(String.Format("[activateNotification] Begin"));

            form.Invoke(new MethodInvoker(delegate {
                Console.WriteLine("inside form invoke, vpnstatus: {0}",vpnStatus);
                formOriginal.Width = formOriginalWidth;
            formOriginal.Height = formOriginalHeight;
            AnimationTimer.Stop();
            AnimationTimer.Dispose();
            AnimationTimer.Interval = formInterval;

            changeNotification(vpnStatus);

            formOriginal.BackColor = notificationFormColor;
            NotificationText.ForeColor = notificationFontColor;
            NotificationText.BackColor = notificationFormColor;
            NotificationText.Text = notificationText;
            /*
                        NotificationTextExtend.ForeColor = notificationFontColor;
                        NotificationTextExtend.BackColor = notificationFormColor;
                        NotificationTextExtend.Text = notificationTextExtend;
                        */
             
            pBoxOriginal.Image = notificationIcon;
            }));
            BW = new BackgroundWorker();
            BW.WorkerSupportsCancellation = true;
            BW.WorkerReportsProgress = true;
            BW.DoWork += (sender,args) => BW_DoWork(form);
            BW.RunWorkerCompleted += (_sender, args) => {
                BW.DoWork -= (sender_, args_) => BW_DoWork(form);
                BW.CancelAsync();
                BW.Dispose();
                Console.WriteLine("Backworker done!!");
            };
            if(!BW.IsBusy) BW.RunWorkerAsync();

                //AnimationTimer.Tick += testing;
                /*AnimationTimer.Tick += (sender, args) => {
                     logging.writeToLog(null, String.Format("[activateNotification][Ticker] Tick"));
                     Console.WriteLine("Ticker: {0}", tick++);
                     Shrink(form, 10);
                     AnimationTimer.Stop();
                     logging.writeToLog(null, String.Format("[activateNotification][Ticker] End"));
                 };*/

                //AnimationTimer.Start();
                Console.WriteLine(String.Format("[activateNotification] End"));
            logging.writeToLog(null, String.Format("[activateNotification][Ticker] Start"));
            logging.writeToLog(null, String.Format("[activateNotification] End"));
        }

        private void BW_DoWork(Form form) {
            Thread.Sleep(3000);
            //BWShrink(formOriginal, 10, 40, 40);
            try {
                /*while (formOriginal.Height > 40 || formOriginal.Width > 40) {
                    Thread.Sleep(10);
                    if (formOriginal.Height > 40) {
                        formOriginal.Height -= 10;
                    }
                    if (formOriginal.Width > 40) {
                        formOriginal.Width -= 10;
                    }
                }
                                form.Height = 40;
                form.Width = 40;
                 */
                form.Invoke(new MethodInvoker(delegate {
                    while (formOriginal.Height > 40 || formOriginal.Width > 40) {
                        Thread.Sleep(10);
                        if (formOriginal.Height > 40) {
                            formOriginal.Height -= 10;
                        }
                        if (formOriginal.Width > 40) {
                            formOriginal.Width -= 10;
                        }
                    }
                    form.Height = 40;
                    form.Width = 40;
                }));
            }
            catch (Exception ex) {
                logging.writeToLog(null, String.Format("[BWShrink]Exception: {0}", ex.Message));
            }
        }
    }
}
