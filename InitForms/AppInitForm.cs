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
    public partial class AppInitForm :InitFormBase
    {
        private TextBox textBox1;
        private TabPage tabPage2;
        private ListView etherLV;
        private TabPage tabPage3;
        private ListView rioLV;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private ListView componentLV;
        private ListView gtxLV;
        private ListView lvdsLV;
        private Label label1;
    
        public AppInitForm()
        {
            InitializeComponent();
            ListViewInit();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.etherLV = new System.Windows.Forms.ListView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rioLV = new System.Windows.Forms.ListView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.gtxLV = new System.Windows.Forms.ListView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lvdsLV = new System.Windows.Forms.ListView();
            this.componentLV = new System.Windows.Forms.ListView();
            this._btnPanel.SuspendLayout();
            this._infoPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._collectionPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.SetChildIndex(this.tabPage5, 0);
            this.tabControl1.Controls.SetChildIndex(this.tabPage4, 0);
            this.tabControl1.Controls.SetChildIndex(this.tabPage3, 0);
            this.tabControl1.Controls.SetChildIndex(this.tabPage2, 0);
            this.tabControl1.Controls.SetChildIndex(this.tabPage1, 0);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.componentLV);
            this.tabPage1.Text = "构件集合";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "型号：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(60, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.etherLV);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(608, 313);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "EtherNet连接";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // etherLV
            // 
            this.etherLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.etherLV.Location = new System.Drawing.Point(3, 3);
            this.etherLV.Name = "etherLV";
            this.etherLV.Size = new System.Drawing.Size(602, 307);
            this.etherLV.TabIndex = 0;
            this.etherLV.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rioLV);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(608, 313);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "RapidIO连接";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rioLV
            // 
            this.rioLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rioLV.Location = new System.Drawing.Point(3, 3);
            this.rioLV.Name = "rioLV";
            this.rioLV.Size = new System.Drawing.Size(602, 307);
            this.rioLV.TabIndex = 0;
            this.rioLV.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.gtxLV);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(608, 313);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "GTX连接";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // gtxLV
            // 
            this.gtxLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gtxLV.Location = new System.Drawing.Point(3, 3);
            this.gtxLV.Name = "gtxLV";
            this.gtxLV.Size = new System.Drawing.Size(602, 307);
            this.gtxLV.TabIndex = 0;
            this.gtxLV.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.lvdsLV);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(608, 313);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "LVDS连接";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // lvdsLV
            // 
            this.lvdsLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvdsLV.Location = new System.Drawing.Point(3, 3);
            this.lvdsLV.Name = "lvdsLV";
            this.lvdsLV.Size = new System.Drawing.Size(602, 307);
            this.lvdsLV.TabIndex = 0;
            this.lvdsLV.UseCompatibleStateImageBehavior = false;
            // 
            // componentLV
            // 
            this.componentLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.componentLV.Location = new System.Drawing.Point(3, 3);
            this.componentLV.Name = "componentLV";
            this.componentLV.Size = new System.Drawing.Size(602, 307);
            this.componentLV.TabIndex = 0;
            this.componentLV.UseCompatibleStateImageBehavior = false;
            // 
            // AppInitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(697, 464);
            this.Name = "AppInitForm";
            this._btnPanel.ResumeLayout(false);
            this._infoPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._collectionPanel.ResumeLayout(false);
            this._collectionPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ListViewInit()
        {
            List<ListView> linkLV = new List<ListView> { etherLV, rioLV, gtxLV,lvdsLV};

            //初始化构件集合
            this.componentLV.BeginUpdate();
            this.componentLV.View = View.Details;
            this.componentLV.GridLines = true;
            this.componentLV.Columns.Add("序号", 100, HorizontalAlignment.Left);
            this.componentLV.Columns.Add("构件类型", 100, HorizontalAlignment.Left);
            this.componentLV.EndUpdate();

            //初始化连接关系的ListView
            foreach (ListView lv in linkLV)
            {
                lv.BeginUpdate();
                lv.View = View.Details;
                lv.GridLines = true;
                lv.Columns.Add("序号", 100, HorizontalAlignment.Left);
                lv.Columns.Add("端1构件", 100, HorizontalAlignment.Left);
                lv.Columns.Add("端2构件", 100, HorizontalAlignment.Left);
                lv.Columns.Add("详细信息", 100, HorizontalAlignment.Left);
                lv.EndUpdate();
            }
        }

    }
}
