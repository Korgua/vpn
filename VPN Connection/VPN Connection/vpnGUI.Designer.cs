using System.Drawing;
using System.Drawing.Drawing2D;

namespace VPN_Connection {
    partial class vpnGUI {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
       /*protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        { 
            GraphicsPath wantedshape = new GraphicsPath();
            Rectangle rect = new Rectangle(20, 20, 150, 50);
            wantedshape.AddRectangle(rect);
            this.Region = new Region(wantedshape);
        }*/
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(vpnGUI));
            this.vpnConnect = new System.Windows.Forms.Button();
            this.vpnDisconnect = new System.Windows.Forms.Button();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayIconContextItemState = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.notificationText = new System.Windows.Forms.Label();
            this.notificationIcon = new System.Windows.Forms.PictureBox();
            this.trayContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notificationIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // vpnConnect
            // 
            this.vpnConnect.Location = new System.Drawing.Point(2, 0);
            this.vpnConnect.Name = "vpnConnect";
            this.vpnConnect.Size = new System.Drawing.Size(75, 23);
            this.vpnConnect.TabIndex = 0;
            this.vpnConnect.Text = "vpnConnect";
            this.vpnConnect.UseVisualStyleBackColor = true;
            this.vpnConnect.Click += new System.EventHandler(this.vpnConnect_Click);
            // 
            // vpnDisconnect
            // 
            this.vpnDisconnect.Location = new System.Drawing.Point(83, 0);
            this.vpnDisconnect.Name = "vpnDisconnect";
            this.vpnDisconnect.Size = new System.Drawing.Size(109, 23);
            this.vpnDisconnect.TabIndex = 1;
            this.vpnDisconnect.Text = "vpnDisconnect";
            this.vpnDisconnect.UseVisualStyleBackColor = true;
            this.vpnDisconnect.Click += new System.EventHandler(this.vpnDisconnect_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayContextMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "trayIcon";
            this.trayIcon.Visible = true;
            // 
            // trayContextMenu
            // 
            this.trayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayIconContextItemState,
            this.statusToolStripMenuItem});
            this.trayContextMenu.Name = "trayContextMenu";
            this.trayContextMenu.Size = new System.Drawing.Size(142, 48);
            // 
            // trayIconContextItemState
            // 
            this.trayIconContextItemState.Enabled = false;
            this.trayIconContextItemState.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.trayIconContextItemState.Name = "trayIconContextItemState";
            this.trayIconContextItemState.Size = new System.Drawing.Size(141, 22);
            this.trayIconContextItemState.Text = "ChangeState";
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.statusToolStripMenuItem.Text = "Status";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(198, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // notificationText
            // 
            this.notificationText.AutoSize = true;
            this.notificationText.BackColor = System.Drawing.Color.Transparent;
            this.notificationText.Font = new System.Drawing.Font("Microsoft PhagsPa", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationText.ForeColor = System.Drawing.Color.Firebrick;
            this.notificationText.Location = new System.Drawing.Point(53, 36);
            this.notificationText.Name = "notificationText";
            this.notificationText.Size = new System.Drawing.Size(201, 21);
            this.notificationText.TabIndex = 6;
            this.notificationText.Text = "Csatlakozás folyamatban";
            this.notificationText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDragStart);
            this.notificationText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.notificationText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDragEnd);
            // 
            // notificationIcon
            // 
            this.notificationIcon.Image = ((System.Drawing.Image)(resources.GetObject("notificationIcon.Image")));
            this.notificationIcon.Location = new System.Drawing.Point(12, 29);
            this.notificationIcon.Name = "notificationIcon";
            this.notificationIcon.Size = new System.Drawing.Size(35, 35);
            this.notificationIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.notificationIcon.TabIndex = 5;
            this.notificationIcon.TabStop = false;
            this.notificationIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDragStart);
            this.notificationIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.notificationIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDragEnd);
            // 
            // vpnGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSalmon;
            this.ClientSize = new System.Drawing.Size(282, 70);
            this.Controls.Add(this.notificationText);
            this.Controls.Add(this.notificationIcon);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vpnDisconnect);
            this.Controls.Add(this.vpnConnect);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "vpnGUI";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VH VPN";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDragStart);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDragEnd);
            this.trayContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.notificationIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button vpnConnect;
        public System.Windows.Forms.Button vpnDisconnect;
        public System.Windows.Forms.NotifyIcon trayIcon;
        public System.Windows.Forms.ContextMenuStrip trayContextMenu;
        public System.Windows.Forms.ToolStripMenuItem trayIconContextItemState;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox notificationIcon;
        private System.Windows.Forms.Label notificationText;
    }
}

