using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

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

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
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
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notificationText = new System.Windows.Forms.Label();
            this.notificationStatusIcon = new System.Windows.Forms.PictureBox();
            this.StatusIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusIconContextReconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.statusIconContextHangUp = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.notificationStatusIcon)).BeginInit();
            this.StatusIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "trayIcon";
            this.trayIcon.Visible = true;
            // 
            // notificationText
            // 
            this.notificationText.AutoSize = true;
            this.notificationText.BackColor = System.Drawing.Color.Transparent;
            this.notificationText.Font = new System.Drawing.Font("Microsoft PhagsPa", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationText.ForeColor = System.Drawing.Color.Firebrick;
            this.notificationText.Location = new System.Drawing.Point(50, 11);
            this.notificationText.Name = "notificationText";
            this.notificationText.Size = new System.Drawing.Size(0, 21);
            this.notificationText.TabIndex = 6;
            this.notificationText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDragStart);
            this.notificationText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.notificationText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDragEnd);
            // 
            // notificationStatusIcon
            // 
            this.notificationStatusIcon.BackColor = System.Drawing.Color.Transparent;
            this.notificationStatusIcon.Location = new System.Drawing.Point(5, 4);
            this.notificationStatusIcon.Name = "notificationStatusIcon";
            this.notificationStatusIcon.Size = new System.Drawing.Size(35, 35);
            this.notificationStatusIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.notificationStatusIcon.TabIndex = 5;
            this.notificationStatusIcon.TabStop = false;
            this.notificationStatusIcon.Click += new System.EventHandler(this.notificationStatusIcon_Click);
            // 
            // StatusIconContextMenu
            // 
            this.StatusIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusIconContextReconnect,
            this.statusIconContextHangUp});
            this.StatusIconContextMenu.Name = "StatusIconContextMenu";
            this.StatusIconContextMenu.Size = new System.Drawing.Size(155, 70);
            // 
            // statusIconContextReconnect
            // 
            this.statusIconContextReconnect.Name = "statusIconContextReconnect";
            this.statusIconContextReconnect.Size = new System.Drawing.Size(154, 22);
            this.statusIconContextReconnect.Text = "Újracsatlakozás";
            this.statusIconContextReconnect.Click += new System.EventHandler(this.statusIconContextReconnect_Click);
            // 
            // statusIconContextHangUp
            // 
            this.statusIconContextHangUp.Name = "statusIconContextHangUp";
            this.statusIconContextHangUp.Size = new System.Drawing.Size(154, 22);
            this.statusIconContextHangUp.Text = "Lekapcsolódás";
            this.statusIconContextHangUp.Click += new System.EventHandler(this.statusIconContextHangUp_Click);
            // 
            // vpnGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(283, 42);
            this.Controls.Add(this.notificationText);
            this.Controls.Add(this.notificationStatusIcon);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "vpnGUI";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "VH VPN";
            this.TopMost = true;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDragStart);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDragEnd);
            ((System.ComponentModel.ISupportInitialize)(this.notificationStatusIcon)).EndInit();
            this.StatusIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.PictureBox notificationStatusIcon;
        private System.Windows.Forms.Label notificationText;
        private ContextMenuStrip StatusIconContextMenu;
        private ToolStripMenuItem statusIconContextReconnect;
        private ToolStripMenuItem statusIconContextHangUp;
    }
}

