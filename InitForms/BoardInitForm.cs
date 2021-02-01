using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.OtherView;

namespace DRSysCtrlDisplay
{
    public class BoardInitForm : Form
    {
        #region Designer生成的控件列表

        private StatusStrip statusStrip1;
        private Button button1;
        private Button YesButton;
        private Panel panel2;
        private Panel panel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private GroupBox groupBox1;
        private TextBox _typeTB;
        private Label label1;
        private TabPage tabPage2;
        private TabControl tabControl2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button ChipAddButton;
        private Button ChipDelButton;
        private Button ChipMoveUpButton;
        private Button ChipMoveDownButton;
        public ListView ChipLV;
        public ListView SWLV;
        private TabControl tabControl3;
        private TabPage tabPage5;
        private ListView EtherLV;
        private TabPage tabPage6;
        private ListView RioLV;
        private TabPage tabPage7;
        private ListView GtxLV;
        private TabPage tabPage8;
        private ListView LvdsLV;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button AddConnectButton;
        private Button ConnectDelButton;
        private Button button10;
        private Button button11;
        private Button _cancelButton;

        #endregion

        public String BoardNodeName;

        private int _chipNum = 0;     //芯片集序号
        private int _swNum = 0;       //交换机集序号
        private int _netNum = 0;      //以太网连接序号
        private int _rapidIONum = 0;  //RapidIO连接序号
        private int _gtxNum = 0;
        private Label label2;
        private TextBox _versionTB;	  //GTX连接序号
        private int _lvdsNum = 0;     //LVDS连接序号

        public BoardInitForm()
        {
            InitializeComponent();
            //设置窗体显示位置
            this.StartPosition = FormStartPosition.CenterParent;
            ListViewInit();
        }

        /// <summary>
        /// 使用一个Board实例来构造一个BoardInitForm
        /// </summary>
        /// <param name="board"></param>
        public BoardInitForm(Board board)
        {
            InitializeComponent();
            ListViewInit();
            _typeTB.Text = board.Name;
            _versionTB.Text = board.Version;

            //由board初始化芯片集ListView
            foreach (PPC ppc in board.PPCList)
            {
                ChipLvAddItems(null, "PPC", ppc.Name);
            }
            foreach (FPGA fpga in board.FPGAList)
            {
                ChipLvAddItems(null, "FPGA", fpga.Name);
            }
            foreach (ZYNQ zynq in board.ZYNQList)
            {
                ChipLvAddItems(null, "ZYNQ", zynq.Name);
            }
            foreach (SwitchDevice sw in board.SwitchList)
            {
                if (sw.Category == SwitchDevice.SwitchCategory.EtherNetSw)
                {
                    SWLvAddItems(null, "EtherSW", sw.Type);
                }
                else
                {
                    SWLvAddItems(null, "RapidIOSW", sw.Type);
                }
            }
            //初始化连接关系ListView
            foreach (Board.BoardLink link in board.LinkList)
            {
                switch (link.LinkType)
                {
                    case LinkType.EtherNet:
                        InitLinkFromBoard(board, link, EtherLV, ref _netNum);
                        break;
                    case LinkType.RapidIO:
                        InitLinkFromBoard(board, link, RioLV, ref _rapidIONum);
                        break;
                    case LinkType.GTX:
                        InitLinkFromBoard(board, link, GtxLV, ref _gtxNum);
                        break;
                    case LinkType.LVDS:
                        InitLinkFromBoard(board, link, LvdsLV, ref _lvdsNum);
                        break;
                    default:
                        MessageBox.Show("读取板卡配置文件错误！");
                        break;
                }
            }
        }

        public string GetObjectName()
        {
            return new string(_typeTB.Text.ToCharArray());
        }

        /*设计器自动生成的代码*/
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.button1 = new System.Windows.Forms.Button();
            this.YesButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ChipLV = new System.Windows.Forms.ListView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.SWLV = new System.Windows.Forms.ListView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ChipAddButton = new System.Windows.Forms.Button();
            this.ChipDelButton = new System.Windows.Forms.Button();
            this.ChipMoveUpButton = new System.Windows.Forms.Button();
            this.ChipMoveDownButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this._versionTB = new System.Windows.Forms.TextBox();
            this._typeTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.EtherLV = new System.Windows.Forms.ListView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.RioLV = new System.Windows.Forms.ListView();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.GtxLV = new System.Windows.Forms.ListView();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.LvdsLV = new System.Windows.Forms.ListView();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.AddConnectButton = new System.Windows.Forms.Button();
            this.ConnectDelButton = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 491);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(674, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(362, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "上一步";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // YesButton
            // 
            this.YesButton.Location = new System.Drawing.Point(473, 15);
            this.YesButton.Name = "YesButton";
            this.YesButton.Size = new System.Drawing.Size(75, 23);
            this.YesButton.TabIndex = 3;
            this.YesButton.Text = "确定";
            this.YesButton.UseVisualStyleBackColor = true;
            this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(577, 15);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 4;
            this._cancelButton.Text = "取消";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this._cancelButton);
            this.panel2.Controls.Add(this.YesButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 441);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(674, 50);
            this.panel2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(674, 441);
            this.panel1.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(674, 441);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Controls.Add(this.flowLayoutPanel1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(666, 415);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "芯片信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 70);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(579, 342);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ChipLV);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(571, 316);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "芯片集";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ChipLV
            // 
            this.ChipLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChipLV.Location = new System.Drawing.Point(3, 3);
            this.ChipLV.Name = "ChipLV";
            this.ChipLV.Size = new System.Drawing.Size(565, 310);
            this.ChipLV.TabIndex = 0;
            this.ChipLV.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.SWLV);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(571, 316);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "交换机集";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // SWLV
            // 
            this.SWLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SWLV.Location = new System.Drawing.Point(3, 3);
            this.SWLV.Name = "SWLV";
            this.SWLV.Size = new System.Drawing.Size(565, 310);
            this.SWLV.TabIndex = 0;
            this.SWLV.UseCompatibleStateImageBehavior = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.ChipAddButton);
            this.flowLayoutPanel1.Controls.Add(this.ChipDelButton);
            this.flowLayoutPanel1.Controls.Add(this.ChipMoveUpButton);
            this.flowLayoutPanel1.Controls.Add(this.ChipMoveDownButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(582, 70);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 80, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(81, 342);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // ChipAddButton
            // 
            this.ChipAddButton.Location = new System.Drawing.Point(3, 83);
            this.ChipAddButton.Name = "ChipAddButton";
            this.ChipAddButton.Size = new System.Drawing.Size(75, 23);
            this.ChipAddButton.TabIndex = 0;
            this.ChipAddButton.Text = "添加";
            this.ChipAddButton.UseVisualStyleBackColor = true;
            this.ChipAddButton.Click += new System.EventHandler(this.ChipAddButton_Click);
            // 
            // ChipDelButton
            // 
            this.ChipDelButton.Location = new System.Drawing.Point(3, 112);
            this.ChipDelButton.Name = "ChipDelButton";
            this.ChipDelButton.Size = new System.Drawing.Size(75, 23);
            this.ChipDelButton.TabIndex = 1;
            this.ChipDelButton.Text = "删除";
            this.ChipDelButton.UseVisualStyleBackColor = true;
            this.ChipDelButton.Click += new System.EventHandler(this.ChipDelButton_Click);
            // 
            // ChipMoveUpButton
            // 
            this.ChipMoveUpButton.Location = new System.Drawing.Point(3, 141);
            this.ChipMoveUpButton.Name = "ChipMoveUpButton";
            this.ChipMoveUpButton.Size = new System.Drawing.Size(75, 23);
            this.ChipMoveUpButton.TabIndex = 2;
            this.ChipMoveUpButton.Text = "上移";
            this.ChipMoveUpButton.UseVisualStyleBackColor = true;
            // 
            // ChipMoveDownButton
            // 
            this.ChipMoveDownButton.Location = new System.Drawing.Point(3, 170);
            this.ChipMoveDownButton.Name = "ChipMoveDownButton";
            this.ChipMoveDownButton.Size = new System.Drawing.Size(75, 23);
            this.ChipMoveDownButton.TabIndex = 3;
            this.ChipMoveDownButton.Text = "下移";
            this.ChipMoveDownButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this._versionTB);
            this.groupBox1.Controls.Add(this._typeTB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(660, 67);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(389, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "版本号";
            // 
            // _versionTB
            // 
            this._versionTB.Location = new System.Drawing.Point(441, 25);
            this._versionTB.Name = "_versionTB";
            this._versionTB.Size = new System.Drawing.Size(100, 21);
            this._versionTB.TabIndex = 2;
            // 
            // _typeTB
            // 
            this._typeTB.Location = new System.Drawing.Point(65, 25);
            this._typeTB.Name = "_typeTB";
            this._typeTB.Size = new System.Drawing.Size(132, 21);
            this._typeTB.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "板卡型号";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl3);
            this.tabPage2.Controls.Add(this.flowLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(666, 415);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "连接信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Controls.Add(this.tabPage7);
            this.tabControl3.Controls.Add(this.tabPage8);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(579, 409);
            this.tabControl3.TabIndex = 2;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.EtherLV);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(571, 383);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "EtherNet";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // EtherLV
            // 
            this.EtherLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EtherLV.Location = new System.Drawing.Point(3, 3);
            this.EtherLV.Name = "EtherLV";
            this.EtherLV.Size = new System.Drawing.Size(565, 377);
            this.EtherLV.TabIndex = 0;
            this.EtherLV.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.RioLV);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(571, 383);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "RapidIO";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // RioLV
            // 
            this.RioLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RioLV.Location = new System.Drawing.Point(3, 3);
            this.RioLV.Name = "RioLV";
            this.RioLV.Size = new System.Drawing.Size(565, 377);
            this.RioLV.TabIndex = 0;
            this.RioLV.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.GtxLV);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(571, 383);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "GTX";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // GtxLV
            // 
            this.GtxLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GtxLV.Location = new System.Drawing.Point(3, 3);
            this.GtxLV.Name = "GtxLV";
            this.GtxLV.Size = new System.Drawing.Size(565, 377);
            this.GtxLV.TabIndex = 0;
            this.GtxLV.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.LvdsLV);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(571, 383);
            this.tabPage8.TabIndex = 3;
            this.tabPage8.Text = "LVDS";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // LvdsLV
            // 
            this.LvdsLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LvdsLV.Location = new System.Drawing.Point(3, 3);
            this.LvdsLV.Name = "LvdsLV";
            this.LvdsLV.Size = new System.Drawing.Size(565, 377);
            this.LvdsLV.TabIndex = 0;
            this.LvdsLV.UseCompatibleStateImageBehavior = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.AddConnectButton);
            this.flowLayoutPanel2.Controls.Add(this.ConnectDelButton);
            this.flowLayoutPanel2.Controls.Add(this.button10);
            this.flowLayoutPanel2.Controls.Add(this.button11);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(582, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 100, 0, 0);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(81, 409);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // AddConnectButton
            // 
            this.AddConnectButton.Location = new System.Drawing.Point(3, 103);
            this.AddConnectButton.Name = "AddConnectButton";
            this.AddConnectButton.Size = new System.Drawing.Size(75, 23);
            this.AddConnectButton.TabIndex = 4;
            this.AddConnectButton.Text = "添加";
            this.AddConnectButton.UseVisualStyleBackColor = true;
            this.AddConnectButton.Click += new System.EventHandler(this.AddConnectButton_Click);
            // 
            // ConnectDelButton
            // 
            this.ConnectDelButton.Location = new System.Drawing.Point(3, 132);
            this.ConnectDelButton.Name = "ConnectDelButton";
            this.ConnectDelButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectDelButton.TabIndex = 5;
            this.ConnectDelButton.Text = "删除";
            this.ConnectDelButton.UseVisualStyleBackColor = true;
            this.ConnectDelButton.Click += new System.EventHandler(this.ConnectDelButton_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(3, 161);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 6;
            this.button10.Text = "上移";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(3, 190);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 7;
            this.button11.Text = "下移";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // BoardInitForm
            // 
            this.ClientSize = new System.Drawing.Size(674, 513);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Name = "BoardInitForm";
            this.Text = "BoardInitForm";
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region 界面事件响应
        /*描述：芯片信息添加按钮响应
         * 参数：略
         * 返回值：void
         */
        private void ChipAddButton_Click(object sender, EventArgs e)
        {
            //芯片集添加
            if (tabControl2.SelectedTab == tabPage3)
            {
                BoardInitForm_AddChipForm addChip = new BoardInitForm_AddChipForm();
                addChip.StartPosition = FormStartPosition.CenterScreen;
                addChip.ShowDialog();
                if (addChip.DialogResult == DialogResult.Yes)
                {
                    ChipLvAddItems(addChip, addChip.Type, addChip.Model);
                    addChip.Dispose();
                }
                return;
            }
            //交换机集添加
            if (tabControl2.SelectedTab == tabPage4)
            {
                BoardInitForm_AddInterChangeForm addInterChange = new BoardInitForm_AddInterChangeForm();
                addInterChange.StartPosition = FormStartPosition.CenterScreen;
                addInterChange.ShowDialog();
                if (addInterChange.DialogResult == DialogResult.Yes)
                {
                    SWLvAddItems(addInterChange, addInterChange.Type, addInterChange.Model);
                    addInterChange.Dispose();
                }
            }
        }


        /* 描述：芯片信息删除项按钮响应
         * 参数：略
         * 返回值：void
         */
        private void ChipDelButton_Click(object sender, EventArgs e)
        {
            //芯片集删除项
            if (tabControl2.SelectedTab == tabPage3)
            {
                if (false == ListViewDelItem(ChipLV, ref _chipNum))
                {
                    MessageBox.Show("请选择要删除的芯片！");
                    return;
                }
                return;
            }
            //交换机集删除项
            if (tabControl2.SelectedTab == tabPage4)
            {
                if (false == ListViewDelItem(SWLV, ref _swNum))
                {
                    MessageBox.Show("请选择需要删除的交换机!");
                    return;
                }
                return;
            }

        }


        /* 描述：交换机集添加项
         * 参数：
         * addSWItem----交换机集添加界面实例
         * 返回值：void
         */
        private void SWLvAddItems(BoardInitForm_AddInterChangeForm addSWItem, String str1, String str2)
        {
            _swNum++;
            SWLV.BeginUpdate();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = _swNum.ToString();
            lvi.SubItems.Add(str1);
            lvi.SubItems.Add(str2);
            SWLV.Items.Add(lvi);
            SWLV.EndUpdate();
        }
        /* 描述：芯片集、交换机集项删除函数
         * 参数：
         * lv----控件ListView实例
         * serialNum----对应的排序值
         * 返回值----失败false，成功true
         */
        private Boolean ListViewDelItem(ListView lv, ref int serialNum)
        {
            int sortNum = 1;
            if (lv.SelectedItems.Count <= 0)
            {
                return false;
            }
            foreach (ListViewItem listViewItem in lv.SelectedItems)
            {
                lv.Items.Remove(listViewItem);
                serialNum--;
            }
            //对Items重新排序
            foreach (ListViewItem lvi in lv.Items)
            {
                lvi.Text = sortNum.ToString();
                sortNum++;
            }
            return true;
        }
        /* 描述：连接信息新增按钮响应函数
         * 参数：略
         * 返回值：void
         */
        private void AddConnectButton_Click(object sender, EventArgs e)
        {
            BoardInitForm_AddConnect addConnect = new BoardInitForm_AddConnect(tabControl3.SelectedTab.Text);
            addConnect.StartPosition = FormStartPosition.CenterScreen;
            addConnect.Owner = this;
            addConnect.ShowDialog();
            if (addConnect.DialogResult == DialogResult.Yes)
            {
                switch (tabControl3.SelectedTab.Text)
                {
                    case "EtherNet":
                        ConnectAddItem(addConnect, EtherLV, ref _netNum);
                        break;
                    case "RapidIO":
                        ConnectAddItem(addConnect, RioLV, ref _rapidIONum);
                        break;
                    case "GTX":
                        ConnectAddItem(addConnect, GtxLV, ref _gtxNum);
                        break;
                    case "LVDS":
                        ConnectAddItem(addConnect, LvdsLV, ref _lvdsNum);
                        break;
                    default:
                        break;
                }
                addConnect.Dispose();
            }
        }
        /* 描述：删除节点按钮响应函数
         * 参数：略
         * 返回值：void
         */
        private void ConnectDelButton_Click(object sender, EventArgs e)
        {
            switch (tabControl3.SelectedTab.Name)
            {
                case "tabPage5":
                    if (ListViewDelItem(EtherLV, ref _netNum) == false)
                    {
                        MessageBox.Show("请选择以太网连接项！");
                        return;
                    }
                    break;
                case "tabPage6":
                    if (ListViewDelItem(RioLV, ref _rapidIONum) == false)
                    {
                        MessageBox.Show("请选择RapidIO连接项！");
                        return;
                    }
                    break;
                case "tabPage7":
                    if (ListViewDelItem(GtxLV, ref _gtxNum) == false)
                    {
                        MessageBox.Show("请选择GTX连接项！");
                        return;
                    }
                    break;
                case "tabPage8":
                    if (ListViewDelItem(LvdsLV, ref _lvdsNum) == false)
                    {
                        MessageBox.Show("请选择LVDS连接项！");
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
        /* 描述：连接信息ListView添加Item函数
         * 参数：
         * addConnect----新增连接信息录入界面实例
         * lv----四种链接信息对应的ListView实例
         * serialNum----Item排序编号，传入值为成员变量_netNum,_rapidIONum,_gtxNum,_lvdsNum
         * 返回值：void
         */
        private void ConnectAddItem(BoardInitForm_AddConnect addConnect, ListView lv, ref int serialNum)
        {
            serialNum++;
            lv.BeginUpdate();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = serialNum.ToString();
            lvi.SubItems.Add(addConnect.Port1_Type);
            lvi.SubItems.Add(addConnect.Port1_SN);
            lvi.SubItems.Add(addConnect.Port1_Num);
            lvi.SubItems.Add(addConnect.Port2_Type);
            lvi.SubItems.Add(addConnect.Port2_SN);
            lvi.SubItems.Add(addConnect.Port2_Num);

            lv.Items.Add(lvi);
            lv.EndUpdate();

        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            Board board = new Board();
            RefreshBoard(board);
            board.SaveXmlByName();
            this.DialogResult = DialogResult.Yes;
        }
        #endregion

        private void InitLinkFromBoard(Board board, Board.BoardLink link, ListView lv, ref int serialNum)
        {
            string portNum = string.Empty;
            serialNum++;
            lv.BeginUpdate();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = serialNum.ToString();
            lvi.SubItems.Add(Enum.GetName(typeof(EndType), link.FirstEndType));
            lvi.SubItems.Add(IdToName(board, link.FirstEndType, link, link.FirstEndId));
            if (link.FirstEndPositionList.Count == 1)
            {
                lvi.SubItems.Add(link.FirstEndPositionList[0].ToString());
            }
            else
            {
                portNum = string.Join(",", link.FirstEndPositionList.ToArray().Select(p => p.ToString()).ToArray());
                lvi.SubItems.Add(portNum);
            }

            lvi.SubItems.Add(Enum.GetName(typeof(EndType), link.SecondEndType));
            lvi.SubItems.Add(IdToName(board, link.SecondEndType, link, link.SecondEndId));
            if (link.SecondEndPositionList.Count == 1)
            {
                lvi.SubItems.Add(link.SecondEndPositionList[0].ToString());
            }
            else
            {
                portNum = string.Join(",", link.SecondEndPositionList.ToArray().Select(p => p.ToString()).ToArray());
                lvi.SubItems.Add(portNum);
            }

            lv.Items.Add(lvi);
            lv.EndUpdate();
        }

        private String IdToName(Board board, EndType endtype, Board.BoardLink link, int id)
        {
            String chipName = String.Empty;
            switch (endtype)
            {
                case EndType.PPC:
                    chipName = board.PPCList[id].Name;
                    break;
                case EndType.FPGA:
                    chipName = board.FPGAList[id].Name;
                    break;
                case EndType.ZYNQ:
                    chipName = board.ZYNQList[id].Name;
                    break;
                case EndType.SW:
                    chipName = board.SwitchList[id].Type;
                    break;
                case EndType.VPX:
                    chipName = Enum.GetName(typeof(LinkType), link.LinkType);
                    break;
                default:
                    break;
            }
            return chipName;
        }

        //初始化所有的listView：ChipLV、SWLV、EtherLV、RioLV、GtxLV、LvdsLV
        private void ListViewInit()
        {
            //初始化芯片集的ListView
            this.ChipLV.BeginUpdate();
            this.ChipLV.View = View.Details;
            this.ChipLV.GridLines = true;
            this.ChipLV.FullRowSelect = true;
            this.ChipLV.Columns.Add("序号", 100, HorizontalAlignment.Left);

            /*这里所指的类型是指芯片的类型：1）PPC;2)FPGA;3)ZYNQ;4)DSP*/
            this.ChipLV.Columns.Add("类型", 100, HorizontalAlignment.Left);
            this.ChipLV.Columns.Add("型号", 100, HorizontalAlignment.Left);
            this.ChipLV.EndUpdate();

            //初始化交换机集的ListView
            this.SWLV.BeginUpdate();
            this.SWLV.View = View.Details;
            this.SWLV.GridLines = true;
            this.SWLV.FullRowSelect = true;
            this.SWLV.Columns.Add("序号", 100, HorizontalAlignment.Left);
            /*这里所指的类型是指交换机的种类：1）以太网交换机；2）RapidIO交换机*/
            this.SWLV.Columns.Add("类型", 100, HorizontalAlignment.Left);
            this.SWLV.Columns.Add("型号", 100, HorizontalAlignment.Left);
            this.SWLV.EndUpdate();

            //初始化EtherNet,RapidIO,GTX,LVDS的连接关系
            List<ListView> lvTable = new List<ListView> { EtherLV, RioLV, GtxLV, LvdsLV };
            foreach (ListView lv in lvTable)
            {
                lv.BeginUpdate();
                lv.View = View.Details;
                lv.GridLines = true;
                lv.FullRowSelect = true;
                lv.Columns.Add("序号", 50, HorizontalAlignment.Left);
                lv.Columns.Add("端1类型", 90, HorizontalAlignment.Left);
                lv.Columns.Add("端1序号", 90, HorizontalAlignment.Left);
                lv.Columns.Add("端1端口号", 90, HorizontalAlignment.Left);
                lv.Columns.Add("端2类型", 90, HorizontalAlignment.Left);
                lv.Columns.Add("端2序号", 90, HorizontalAlignment.Left);
                lv.Columns.Add("端2端口号", 90, HorizontalAlignment.Left);
                /*此处的详细包括：
                 * 1）连接两端的端口号（及具体位置）；
                 * 2）速率；
                 * 3）通道数；
                 */
                //lv.Columns.Add("详细信息", 250, HorizontalAlignment.Left);
                lv.EndUpdate();
            }
        }

        private void AddLinkToList(Board board, ListView lv, LinkType linkType)
        {
            IEnumerable<ListViewItem> LinkItems = lv.Items.Cast<ListViewItem>();
            foreach (var item in LinkItems)
            {
                Board.BoardLink link = new Board.BoardLink();
                link.LinkType = linkType;
                //端1
                link.FirstEndType = ChooseEndType(item.SubItems[1].Text);
                link.FirstEndId = int.Parse(item.SubItems[2].Text);
                link.FirstEndPositionList.Add(int.Parse(item.SubItems[3].Text));
                //端2
                link.SecondEndType = ChooseEndType(item.SubItems[4].Text);
                link.SecondEndId = int.Parse(item.SubItems[5].Text);
                link.SecondEndPositionList.Add(int.Parse(item.SubItems[6].Text));
                board.LinkList.Add(link);
            }
        }

        /// <summary>
        /// 根据item中芯片的类型选择EndType枚举值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private EndType ChooseEndType(String str)
        {
            EndType endType;
            switch (str)
            {
                case "PPC":
                    endType = EndType.PPC;
                    break;
                case "FPGA":
                    endType = EndType.FPGA;
                    break;
                case "ZYNQ":
                    endType = EndType.ZYNQ;
                    break;
                case "VPX":
                    endType = EndType.VPX;
                    break;
                default:
                    endType = EndType.SW;
                    break;
            }
            return endType;
        }

        /// <summary>
        /// 返回芯片在List<>中的下标
        /// </summary>
        /// <param name="board"></param>
        /// <param name="endType"></param>
        /// <param name="chipName"></param>
        /// <returns></returns>
        private int GetChipId(Board board, EndType endType, String chipName)
        {
            int chipId = -1;
            switch (endType)
            {
                case EndType.PPC:
                    foreach (PPC ppc in board.PPCList)
                    {
                        if (ppc.Name == chipName)
                        {
                            chipId = board.PPCList.IndexOf(ppc);
                        }
                    }
                    break;
                case EndType.FPGA:
                    foreach (FPGA fpga in board.FPGAList)
                    {
                        if (fpga.Name == chipName)
                        {
                            chipId = board.FPGAList.IndexOf(fpga);
                        }
                    }
                    break;
                case EndType.ZYNQ:
                    foreach (ZYNQ zynq in board.ZYNQList)
                    {
                        if (zynq.Name == chipName)
                        {
                            chipId = board.ZYNQList.IndexOf(zynq);
                        }
                    }
                    break;
                case EndType.SW:
                    foreach (SwitchDevice sw in board.SwitchList)
                    {
                        if (sw.Type == chipName)
                        {
                            chipId = board.SwitchList.IndexOf(sw);
                        }
                    }
                    break;
                default:
                    chipId = -1;
                    break;

            }
            return chipId;
        }

        /* 描述：板卡库初始化界面信息完整性判断函数
         * 参数：
         * 返回值：信息不完整返回false，完整返回true
         */
        private Boolean CompleteJudgment()
        {
            if (String.Empty == _typeTB.Text || String.Empty == _versionTB.Text)
            {
                MessageBox.Show("请录入板卡基本信息！");
                return false;
            }

            else if (ChipLV.Items.Count <= 0)
            {
                MessageBox.Show("请录入板卡芯片集!");
                return false;
            }
            else if (EtherLV.Items.Count <= 0 && RioLV.Items.Count <= 0 && GtxLV.Items.Count <= 0
                && LvdsLV.Items.Count <= 0)
            {
                MessageBox.Show("请录入板卡芯片间连接关系信息!");
                return false;
            }
            //如果有以太网或者RapidIO连接，就必须增加交换机集
            else if ((EtherLV.Items.Count > 0 || RioLV.Items.Count > 0) && SWLV.Items.Count <= 0)
            {
                MessageBox.Show("板卡存在网络连接！请录入交换机芯片信息！");
                return false;
            }
            return true;
        }

        /* 描述：芯片集添加项
         * 参数：
         * addChipItem----芯片集添加界面实例
         * 返回值：void
         */
        private void ChipLvAddItems(BoardInitForm_AddChipForm addChipItem, String str1, String str2)
        {
            _chipNum++;
            ChipLV.BeginUpdate();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = _chipNum.ToString();
            lvi.SubItems.Add(str1);
            lvi.SubItems.Add(str2);
            ChipLV.Items.Add(lvi);
            ChipLV.EndUpdate();
        }

        /// <summary>
        /// 通过用户填的值来更新一个Board类
        /// </summary>
        /// <param name="board"></param>
        private void RefreshBoard(Board board)
        {
            BoardNodeName = _typeTB.Text;
            board.Name = _typeTB.Text;
            board.Type = _typeTB.Text;
            board.Version = _versionTB.Text;

            IEnumerable<ListViewItem> coreLVItems = ChipLV.Items.Cast<ListViewItem>();
            //添加板卡的PPC集合
            var ppcs = from e in coreLVItems
                       where e.SubItems[1].Text == "PPC"
                       select e.SubItems[2].Text;
            foreach (var ppcName in ppcs)
            {
                board.PPCList.Add(BaseViewFactory<PPC>.CreateByName(ppcName));
            }
            //添加板卡的FPGA集合
            var fpgas = from f in coreLVItems
                        where f.SubItems[1].Text == "FPGA"
                        select f.SubItems[2].Text;
            foreach (var fpgaName in fpgas)
            {
                board.FPGAList.Add(BaseViewFactory<FPGA>.CreateByName(fpgaName));
            }
            //添加板卡的ZYNQ集合
            var zynqs = from z in coreLVItems
                        where z.SubItems[1].Text == "ZYNQ"
                        select z.SubItems[2].Text;
            foreach (var zynqName in zynqs)
            {
                board.ZYNQList.Add(BaseViewFactory<ZYNQ>.CreateByName(zynqName));
            }
            //添加板卡的Sw集合
            IEnumerable<ListViewItem> swLVItems = SWLV.Items.Cast<ListViewItem>();
            foreach (var item in swLVItems)
            {
                SwitchDevice.SwitchCategory catogory = item.SubItems[1].Text == "EtherSW" ?
                    SwitchDevice.SwitchCategory.EtherNetSw : SwitchDevice.SwitchCategory.RioSw;
                board.SwitchList.Add(new SwitchDevice(catogory, item.SubItems[2].Text));
            }
            //添加板卡的Link集合
            AddLinkToList(board, EtherLV, LinkType.EtherNet);//以太网
            AddLinkToList(board, RioLV, LinkType.RapidIO);//RapidIO
            AddLinkToList(board, GtxLV, LinkType.GTX);//GTX
            AddLinkToList(board, LvdsLV, LinkType.LVDS);//LVDS
        }
    }
}
