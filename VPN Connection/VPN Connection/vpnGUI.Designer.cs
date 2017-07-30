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
            wantedshape.AddEllipse(25, 25, 25,25);
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
            this.trayContextMenu.SuspendLayout();
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
            this.vpnDisconnect.Location = new System.Drawing.Point(80, 89);
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
            this.trayIconContextItemState.Size = new System.Drawing.Size(152, 22);
            this.trayIconContextItemState.Text = "ChangeState";
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.statusToolStripMenuItem.Text = "Status";
            // 
            // vpnGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 220);
            this.Controls.Add(this.vpnDisconnect);
            this.Controls.Add(this.vpnConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "vpnGUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VH VPN";
            this.trayContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button vpnConnect;
        public System.Windows.Forms.Button vpnDisconnect;
        public System.Windows.Forms.NotifyIcon trayIcon;
        public System.Windows.Forms.ContextMenuStrip trayContextMenu;
        public System.Windows.Forms.ToolStripMenuItem trayIconContextItemState;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
    }
}

