﻿using System.Drawing;
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
            this.notificationText = new System.Windows.Forms.Label();
            this.notificationStatusIcon = new System.Windows.Forms.PictureBox();
            this.StatusIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusIconContextReconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.statusIconContextHangUp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusIconContextExit = new System.Windows.Forms.ToolStripMenuItem();
            this.notificationTextExtend = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.notificationStatusIcon)).BeginInit();
            this.StatusIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notificationText
            // 
            this.notificationText.AutoSize = true;
            this.notificationText.BackColor = System.Drawing.Color.Transparent;
            this.notificationText.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold);
            this.notificationText.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.notificationText.Location = new System.Drawing.Point(38, 5);
            this.notificationText.Name = "notificationText";
            this.notificationText.Size = new System.Drawing.Size(157, 15);
            this.notificationText.TabIndex = 6;
            this.notificationText.Text = "Sikeresen lekapcsolódva";
            this.notificationText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDragStart);
            this.notificationText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.notificationText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDragEnd);
            // 
            // notificationStatusIcon
            // 
            this.notificationStatusIcon.BackColor = System.Drawing.Color.Transparent;
            this.notificationStatusIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.notificationStatusIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.notificationStatusIcon.ErrorImage = null;
            this.notificationStatusIcon.Image = global::VPN_Connection.Properties.Resources.info;
            this.notificationStatusIcon.InitialImage = null;
            this.notificationStatusIcon.Location = new System.Drawing.Point(3, 3);
            this.notificationStatusIcon.Margin = new System.Windows.Forms.Padding(0);
            this.notificationStatusIcon.Name = "notificationStatusIcon";
            this.notificationStatusIcon.Padding = new System.Windows.Forms.Padding(3);
            this.notificationStatusIcon.Size = new System.Drawing.Size(34, 34);
            this.notificationStatusIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.notificationStatusIcon.TabIndex = 5;
            this.notificationStatusIcon.TabStop = false;
            this.notificationStatusIcon.Click += new System.EventHandler(this.notificationStatusIcon_Click);
            this.notificationStatusIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StatusIconDragStart);
            this.notificationStatusIcon.MouseLeave += new System.EventHandler(this.statusIconContext_Leave);
            this.notificationStatusIcon.MouseHover += new System.EventHandler(this.statusIconContext_Hover);
            this.notificationStatusIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.notificationStatusIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDragEnd);
            // 
            // StatusIconContextMenu
            // 
            this.StatusIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusIconContextReconnect,
            this.statusIconContextHangUp,
            this.toolStripSeparator1,
            this.statusIconContextExit});
            this.StatusIconContextMenu.Name = "StatusIconContextMenu";
            this.StatusIconContextMenu.Size = new System.Drawing.Size(155, 76);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // statusIconContextExit
            // 
            this.statusIconContextExit.Enabled = false;
            this.statusIconContextExit.Name = "statusIconContextExit";
            this.statusIconContextExit.Size = new System.Drawing.Size(154, 22);
            this.statusIconContextExit.Text = "Kilépés";
            this.statusIconContextExit.Click += new System.EventHandler(this.statusIconContextExit_Click);
            // 
            // notificationTextExtend
            // 
            this.notificationTextExtend.AutoSize = true;
            this.notificationTextExtend.Enabled = false;
            this.notificationTextExtend.Location = new System.Drawing.Point(40, 24);
            this.notificationTextExtend.Name = "notificationTextExtend";
            this.notificationTextExtend.Size = new System.Drawing.Size(0, 13);
            this.notificationTextExtend.TabIndex = 7;
            // 
            // vpnGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(200, 40);
            this.Controls.Add(this.notificationTextExtend);
            this.Controls.Add(this.notificationText);
            this.Controls.Add(this.notificationStatusIcon);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "vpnGUI";
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
        private System.Windows.Forms.PictureBox notificationStatusIcon;
        private System.Windows.Forms.Label notificationText;
        private ContextMenuStrip StatusIconContextMenu;
        private ToolStripMenuItem statusIconContextReconnect;
        private ToolStripMenuItem statusIconContextHangUp;
        private Label notificationTextExtend;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem statusIconContextExit;
    }
}

