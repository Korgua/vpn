﻿using System.Drawing;
using System.Drawing.Drawing2D;

namespace VPN_Connection {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
<<<<<<< HEAD
       /*protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
=======
        /*protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        { 
>>>>>>> vpn_test
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
        private void InitializeComponent() {
            this.vpnConnect = new System.Windows.Forms.Button();
            this.vpmDisconnect = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
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
            // vpmDisconnect
            // 
            this.vpmDisconnect.Location = new System.Drawing.Point(2, 22);
            this.vpmDisconnect.Name = "vpmDisconnect";
            this.vpmDisconnect.Size = new System.Drawing.Size(98, 23);
            this.vpmDisconnect.TabIndex = 1;
            this.vpmDisconnect.Text = "vpnDisconnect";
            this.vpmDisconnect.UseVisualStyleBackColor = true;
            this.vpmDisconnect.Click += new System.EventHandler(this.vpmDisconnect_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(96, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 222);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vpmDisconnect);
            this.Controls.Add(this.vpnConnect);
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button vpnConnect;
        private System.Windows.Forms.Button vpmDisconnect;
        private System.Windows.Forms.Button button1;
    }
}

