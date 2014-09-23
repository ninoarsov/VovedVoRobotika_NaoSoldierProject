using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Collections.Generic;

namespace NaoWindowsForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 

    
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.mouseButton = new System.Windows.Forms.ToolStripButton();
            this.connectButton = new System.Windows.Forms.ToolStripButton();
            this.textBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mouseButton,
            this.connectButton,
            this.textBox1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(910, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.toolStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // mouseButton
            // 
            this.mouseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mouseButton.Image = ((System.Drawing.Image)(resources.GetObject("mouseButton.Image")));
            this.mouseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mouseButton.Name = "mouseButton";
            this.mouseButton.Size = new System.Drawing.Size(85, 22);
            this.mouseButton.Text = "Enable Mouse";
            this.mouseButton.Click += new System.EventHandler(this.mouseButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.connectButton.Enabled = false;
            this.connectButton.Image = ((System.Drawing.Image)(resources.GetObject("connectButton.Image")));
            this.connectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectButton.Name = "connectButton"; 
            this.connectButton.Size = new System.Drawing.Size(99, 22);
            this.connectButton.Text = "Connect to NAO";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 25);
            this.textBox1.Text = "IsNoGood.local";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 642);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

      

        
    }
}

