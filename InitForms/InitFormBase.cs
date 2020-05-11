using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DRSysCtrlDisplay
{
    public class InitFormBase : Form
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel propPanel;
        protected System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel btnPanel;
        protected System.Windows.Forms.Panel InfoPanel;
        protected System.Windows.Forms.Panel ViewPanel;   /*该显示的Panel现在占时使用，先放在这里*/

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.propPanel = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnPanel = new System.Windows.Forms.Panel();
            this.InfoPanel = new System.Windows.Forms.Panel();
            this.ViewPanel = new System.Windows.Forms.Panel();
            this.propPanel.SuspendLayout();
            this.btnPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // propPanel
            // 
            this.propPanel.Controls.Add(this.propertyGrid1);
            this.propPanel.Location = new System.Drawing.Point(0, 0);
            this.propPanel.Name = "propPanel";
            this.propPanel.Size = new System.Drawing.Size(281, 221);
            this.propPanel.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(281, 221);
            this.propertyGrid1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(197, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnPanel
            // 
            this.btnPanel.Controls.Add(this.button2);
            this.btnPanel.Controls.Add(this.button1);
            this.btnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPanel.Location = new System.Drawing.Point(0, 418);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(697, 46);
            this.btnPanel.TabIndex = 3;
            // 
            // InfoPanel
            // 
            this.InfoPanel.Location = new System.Drawing.Point(2, 233);
            this.InfoPanel.Name = "InfoPanel";
            this.InfoPanel.Size = new System.Drawing.Size(281, 178);
            this.InfoPanel.TabIndex = 4;
            // 
            // ViewPanel
            // 
            this.ViewPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ViewPanel.Location = new System.Drawing.Point(289, 0);
            this.ViewPanel.Name = "ViewPanel";
            this.ViewPanel.Size = new System.Drawing.Size(408, 418);
            this.ViewPanel.TabIndex = 5;
            // 
            // InitFormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 464);
            this.Controls.Add(this.ViewPanel);
            this.Controls.Add(this.InfoPanel);
            this.Controls.Add(this.btnPanel);
            this.Controls.Add(this.propPanel);
            this.Name = "InitFormBase";
            this.Text = "InitFormBase";
            this.propPanel.ResumeLayout(false);
            this.btnPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public InitFormBase()
        {
            InitializeComponent();
        }

        /*brief:只显示属性和按钮
         */
        protected void OnlyShowProperty()
        {
            this.propPanel.SuspendLayout();
            this.btnPanel.SuspendLayout();
            this.SuspendLayout();
            //不显示InfoPanel和ViewPanel
            this.Controls.Remove(this.InfoPanel);
            this.Controls.Remove(this.ViewPanel);
            this.Size = new System.Drawing.Size(281, 500);
            this.propPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.propPanel.Size = new System.Drawing.Size(281, 420);
            this.btnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propPanel.ResumeLayout();
            this.btnPanel.ResumeLayout();
            this.ResumeLayout();
        }
    }
}
