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
            this.trayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayIconContextItemState = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIconContextItemDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIconContextItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.notificationText = new System.Windows.Forms.Label();
            this.notificationIcon = new System.Windows.Forms.PictureBox();
            this.trayContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notificationIcon)).BeginInit();
            this.SuspendLayout();
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
            this.trayIconContextItemDisconnect,
            this.trayIconContextItemExit});
            this.trayContextMenu.Name = "trayContextMenu";
            this.trayContextMenu.Size = new System.Drawing.Size(152, 70);
            // 
            // trayIconContextItemState
            // 
            this.trayIconContextItemState.Enabled = false;
            this.trayIconContextItemState.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.trayIconContextItemState.Name = "trayIconContextItemState";
            this.trayIconContextItemState.Size = new System.Drawing.Size(151, 22);
            this.trayIconContextItemState.Text = "Várakozás";
            // 
            // trayIconContextItemDisconnect
            // 
            this.trayIconContextItemDisconnect.Name = "trayIconContextItemDisconnect";
            this.trayIconContextItemDisconnect.Size = new System.Drawing.Size(151, 22);
            this.trayIconContextItemDisconnect.Text = "Lekapcsolódás";
            this.trayIconContextItemDisconnect.Click += new System.EventHandler(this.trayIconContextItemDisconnect_Click);
            // 
            // trayIconContextItemExit
            // 
            this.trayIconContextItemExit.Name = "trayIconContextItemExit";
            this.trayIconContextItemExit.Size = new System.Drawing.Size(151, 22);
            this.trayIconContextItemExit.Text = "Kilépés";
            this.trayIconContextItemExit.Click += new System.EventHandler(this.kilépésToolStripMenuItem_Click);
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
            // notificationIcon
            // 
            this.notificationIcon.BackColor = System.Drawing.Color.Transparent;
            this.notificationIcon.Location = new System.Drawing.Point(5, 4);
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
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(283, 42);
            this.Controls.Add(this.notificationText);
            this.Controls.Add(this.notificationIcon);
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
            this.trayContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.notificationIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.NotifyIcon trayIcon;
        public System.Windows.Forms.ContextMenuStrip trayContextMenu;
        public System.Windows.Forms.ToolStripMenuItem trayIconContextItemState;
        private System.Windows.Forms.ToolStripMenuItem trayIconContextItemDisconnect;
        private System.Windows.Forms.PictureBox notificationIcon;
        private System.Windows.Forms.Label notificationText;
        private ToolStripMenuItem trayIconContextItemExit;
    }
}

