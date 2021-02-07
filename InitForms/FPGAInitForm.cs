using DRSysCtrlDisplay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace DRSysCtrlDisplay
{
    public class FPGAInitForm : InitFormBase
    {
        #region Dedigner生成的控件成员
        private ComboBox _adCB;
        private Label label20;
        private GroupBox groupBox6;
        private TextBox _commercialTB;
        private Label label17;
        private TextBox _extenedTempTB;
        private Label label18;
        private TextBox _indTempTB;
        private Label label19;
        private GroupBox groupBox4;
        private TextBox _maxDstTB;
        private Label label11;
        private TextBox _blockTB;
        private Label label10;
        private TextBox _totalBlockTB;
        private Label label9;
        private GroupBox groupBox3;
        private TextBox _singalIOTB;
        private Label label8;
        private TextBox _difIOTB;
        private Label label7;
        private TextBox _clockTB;
        private Label label6;
        private GroupBox groupBox2;
        private TextBox _lutTB;
        private TextBox _glbTB;
        private TextBox _logicCells;
        private TextBox _slicesTB;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private ComboBox _typeCB;
        private Label label1;
        private GroupBox groupBox5;
        private TextBox _dspTB;
        private Label label16;
        private TextBox _pcieTB;
        private TextBox _gtpTB;
        private TextBox _amsTB;
        private Label label15;
        private TextBox _aesTB;
        private Label label14;
        private Label label13;
        private Label label12;
        protected TextBox _nameTB;
        private Label label21;
        private Panel panel2;
        #endregion

        public FPGAInitForm()
        {
            SetFatherComponents();
            InitializeComponent();
            //设置窗体显示位置
            this.StartPosition = FormStartPosition.CenterParent;
        }
        /// <summary>
        /// 通过一FPGA实例来初始化FPGAInitForm，一般用于属性修改；
        /// </summary>
        /// <param name="fpga"></param>
        public FPGAInitForm(FPGA fpga)
        {
            SetFatherComponents();
            InitializeComponent();

            _nameTB.Text = fpga.Name;
            _typeCB.Text = fpga.Type;
            _adCB.Text = fpga.AD;
            _clockTB.Text = fpga.Clock.ToString();
            //logic
            _slicesTB.Text = fpga.Slices.ToString();
            _glbTB.Text = fpga.GLB.ToString();
            _logicCells.Text = fpga.LogicCells.ToString();
            _lutTB.Text = fpga.LUT.ToString();
            //IO
            _difIOTB.Text = fpga.DifferentialIO.ToString();
            _singalIOTB.Text = fpga.SingalIO.ToString();
            //memory
            _totalBlockTB.Text = fpga.TotalBlock.ToString();
            _blockTB.Text = fpga.Block.ToString();
            _maxDstTB.Text = fpga.MaxDistributed.ToString();
            //speedLevel
            _indTempTB.Text = fpga.IndustrialTemp.ToString();
            _extenedTempTB.Text = fpga.ExtenedTemp.ToString();
            _commercialTB.Text = fpga.Commercial.ToString();
            //IPResource
            _gtpTB.Text = fpga.GTP.ToString();
            _aesTB.Text = fpga.AES.ToString();
            _amsTB.Text = fpga.AMS.ToString();
            _pcieTB.Text = fpga.PCIE.ToString();
            _dspTB.Text = fpga.DSP;
        }

        private void InitializeComponent()
        {
            this.Controls.Remove(this._collectionPanel);
            base._infoPanel.Dock = DockStyle.Fill;

            this.panel2 = new System.Windows.Forms.Panel();
            this._nameTB = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this._dspTB = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this._pcieTB = new System.Windows.Forms.TextBox();
            this._gtpTB = new System.Windows.Forms.TextBox();
            this._amsTB = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this._aesTB = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this._adCB = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this._commercialTB = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this._extenedTempTB = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this._indTempTB = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this._maxDstTB = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this._blockTB = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this._totalBlockTB = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._singalIOTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this._difIOTB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this._clockTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._lutTB = new System.Windows.Forms.TextBox();
            this._glbTB = new System.Windows.Forms.TextBox();
            this._logicCells = new System.Windows.Forms.TextBox();
            this._slicesTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._typeCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this._btnPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Size = new System.Drawing.Size(697, 500);
            // 
            // panel1
            // 
            this._btnPanel.Location = new System.Drawing.Point(0, 495);
            // 
            // cancleBtn
            // 
            this._cancleBtn.Click += new System.EventHandler(this.CancleBtn_Click);
            // 
            // yesBtn
            // 
            this._yesBtn.Click += new System.EventHandler(this.yesBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(0, 500);
            this.tabControl1.Size = new System.Drawing.Size(616, 0);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(608, 0);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this._nameTB);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Controls.Add(this._adCB);
            this.panel2.Controls.Add(this.label20);
            this.panel2.Controls.Add(this.groupBox6);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this._clockTB);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this._typeCB);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(691, 480);
            this.panel2.TabIndex = 0;
            // 
            // _nameTB
            // 
            this._nameTB.Location = new System.Drawing.Point(114, 16);
            this._nameTB.Name = "_nameTB";
            this._nameTB.Size = new System.Drawing.Size(100, 21);
            this._nameTB.TabIndex = 36;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(13, 20);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(35, 12);
            this.label21.TabIndex = 35;
            this.label21.Text = "Name:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this._dspTB);
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this._pcieTB);
            this.groupBox5.Controls.Add(this._gtpTB);
            this.groupBox5.Controls.Add(this._amsTB);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this._aesTB);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Location = new System.Drawing.Point(9, 376);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(648, 79);
            this.groupBox5.TabIndex = 34;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "IPResources";
            // 
            // _dspTB
            // 
            this._dspTB.Location = new System.Drawing.Point(310, 49);
            this._dspTB.Name = "_dspTB";
            this._dspTB.Size = new System.Drawing.Size(100, 21);
            this._dspTB.TabIndex = 22;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(227, 49);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 21;
            this.label16.Text = "DSP:";
            // 
            // _pcieTB
            // 
            this._pcieTB.Location = new System.Drawing.Point(105, 49);
            this._pcieTB.Name = "_pcieTB";
            this._pcieTB.Size = new System.Drawing.Size(100, 21);
            this._pcieTB.TabIndex = 20;
            // 
            // _gtpTB
            // 
            this._gtpTB.Location = new System.Drawing.Point(105, 22);
            this._gtpTB.Name = "_gtpTB";
            this._gtpTB.Size = new System.Drawing.Size(100, 21);
            this._gtpTB.TabIndex = 17;
            // 
            // _amsTB
            // 
            this._amsTB.Location = new System.Drawing.Point(531, 22);
            this._amsTB.Name = "_amsTB";
            this._amsTB.Size = new System.Drawing.Size(100, 21);
            this._amsTB.TabIndex = 19;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 12);
            this.label15.TabIndex = 13;
            this.label15.Text = "GTP:";
            // 
            // _aesTB
            // 
            this._aesTB.Location = new System.Drawing.Point(310, 22);
            this._aesTB.Name = "_aesTB";
            this._aesTB.Size = new System.Drawing.Size(100, 21);
            this._aesTB.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(227, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 14;
            this.label14.Text = "AES:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(430, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 15;
            this.label13.Text = "AMS:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 16;
            this.label12.Text = "PCIE:";
            // 
            // _adCB
            // 
            this._adCB.FormattingEnabled = true;
            this._adCB.Items.AddRange(new object[] {
            "有",
            "无"});
            this._adCB.Location = new System.Drawing.Point(540, 16);
            this._adCB.Name = "_adCB";
            this._adCB.Size = new System.Drawing.Size(100, 20);
            this._adCB.TabIndex = 33;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(439, 22);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(29, 12);
            this.label20.TabIndex = 32;
            this.label20.Text = "AD：";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this._commercialTB);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this._extenedTempTB);
            this.groupBox6.Controls.Add(this.label18);
            this.groupBox6.Controls.Add(this._indTempTB);
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Location = new System.Drawing.Point(9, 307);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(648, 53);
            this.groupBox6.TabIndex = 31;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "SpeedLevel";
            // 
            // _commercialTB
            // 
            this._commercialTB.Location = new System.Drawing.Point(531, 20);
            this._commercialTB.Name = "_commercialTB";
            this._commercialTB.Size = new System.Drawing.Size(100, 21);
            this._commercialTB.TabIndex = 20;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(430, 23);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(71, 12);
            this.label17.TabIndex = 19;
            this.label17.Text = "Commercial:";
            // 
            // _extenedTempTB
            // 
            this._extenedTempTB.Location = new System.Drawing.Point(310, 20);
            this._extenedTempTB.Name = "_extenedTempTB";
            this._extenedTempTB.Size = new System.Drawing.Size(100, 21);
            this._extenedTempTB.TabIndex = 18;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(227, 23);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 12);
            this.label18.TabIndex = 17;
            this.label18.Text = "ExtenedTemp:";
            // 
            // _indTempTB
            // 
            this._indTempTB.Location = new System.Drawing.Point(105, 20);
            this._indTempTB.Name = "_indTempTB";
            this._indTempTB.Size = new System.Drawing.Size(100, 21);
            this._indTempTB.TabIndex = 16;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(5, 23);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(95, 12);
            this.label19.TabIndex = 15;
            this.label19.Text = "IndustrialTemp:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this._maxDstTB);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this._blockTB);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this._totalBlockTB);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(9, 235);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(648, 56);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Memory";
            // 
            // _maxDstTB
            // 
            this._maxDstTB.Location = new System.Drawing.Point(531, 23);
            this._maxDstTB.Name = "_maxDstTB";
            this._maxDstTB.Size = new System.Drawing.Size(100, 21);
            this._maxDstTB.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(430, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 12);
            this.label11.TabIndex = 19;
            this.label11.Text = "MaxDistributed:";
            // 
            // _blockTB
            // 
            this._blockTB.Location = new System.Drawing.Point(310, 23);
            this._blockTB.Name = "_blockTB";
            this._blockTB.Size = new System.Drawing.Size(100, 21);
            this._blockTB.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(227, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "Block:";
            // 
            // _totalBlockTB
            // 
            this._totalBlockTB.Location = new System.Drawing.Point(105, 20);
            this._totalBlockTB.Name = "_totalBlockTB";
            this._totalBlockTB.Size = new System.Drawing.Size(100, 21);
            this._totalBlockTB.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "TotalBlock:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._singalIOTB);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this._difIOTB);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(9, 164);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(648, 57);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "IO";
            // 
            // _singalIOTB
            // 
            this._singalIOTB.Location = new System.Drawing.Point(310, 20);
            this._singalIOTB.Name = "_singalIOTB";
            this._singalIOTB.Size = new System.Drawing.Size(100, 21);
            this._singalIOTB.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(227, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "SingalIO:";
            // 
            // _difIOTB
            // 
            this._difIOTB.Location = new System.Drawing.Point(105, 20);
            this._difIOTB.Name = "_difIOTB";
            this._difIOTB.Size = new System.Drawing.Size(100, 21);
            this._difIOTB.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "DifferentialIO:";
            // 
            // _clockTB
            // 
            this._clockTB.Location = new System.Drawing.Point(114, 49);
            this._clockTB.Name = "_clockTB";
            this._clockTB.Size = new System.Drawing.Size(100, 21);
            this._clockTB.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 27;
            this.label6.Text = "Clock:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._lutTB);
            this.groupBox2.Controls.Add(this._glbTB);
            this.groupBox2.Controls.Add(this._logicCells);
            this.groupBox2.Controls.Add(this._slicesTB);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(9, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(648, 76);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logic";
            // 
            // _lutTB
            // 
            this._lutTB.Location = new System.Drawing.Point(105, 46);
            this._lutTB.Name = "_lutTB";
            this._lutTB.Size = new System.Drawing.Size(101, 21);
            this._lutTB.TabIndex = 12;
            // 
            // _glbTB
            // 
            this._glbTB.Location = new System.Drawing.Point(310, 17);
            this._glbTB.Name = "_glbTB";
            this._glbTB.Size = new System.Drawing.Size(100, 21);
            this._glbTB.TabIndex = 11;
            // 
            // _logicCells
            // 
            this._logicCells.Location = new System.Drawing.Point(531, 17);
            this._logicCells.Name = "_logicCells";
            this._logicCells.Size = new System.Drawing.Size(100, 21);
            this._logicCells.TabIndex = 10;
            // 
            // _slicesTB
            // 
            this._slicesTB.Location = new System.Drawing.Point(105, 17);
            this._slicesTB.Name = "_slicesTB";
            this._slicesTB.Size = new System.Drawing.Size(101, 21);
            this._slicesTB.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "LUT:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(227, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "GLB:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(430, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "LogicCells:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Slices:";
            // 
            // _typeCB
            // 
            this._typeCB.FormattingEnabled = true;
            this._typeCB.Items.AddRange(new object[] {
            "K7",
            "V6",
            "V7"});
            this._typeCB.Location = new System.Drawing.Point(309, 16);
            this._typeCB.Name = "_typeCB";
            this._typeCB.Size = new System.Drawing.Size(101, 20);
            this._typeCB.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(233, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "Type：";
            // 
            // FPGAInitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(697, 557);
            this.Name = "FPGAInitForm";
            this.groupBox1.ResumeLayout(false);
            this._btnPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region 事件处理函数

        protected virtual void yesBtn_Click(object sender, EventArgs e)
        {
            if (false == CompleteJudgment())
            {
                return;
            }
            FPGAViewModel fpga = new FPGAViewModel();
            RefreshFPGA(fpga);
            //XMLManager.HandleType.SaveXML_FPGA(fpga);
            fpga.SaveXmlByName();

            this.DialogResult = DialogResult.Yes;
        }

        private void CancleBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        protected void RefreshFPGA(FPGA fpga)
        {
            try
            {
                fpga.Name = _nameTB.Text;
                fpga.Type = _typeCB.Text;
                fpga.AD = _adCB.Text;
                fpga.Clock = int.Parse(_clockTB.Text);
                //logic
                fpga.Slices = int.Parse(_slicesTB.Text);
                fpga.GLB = int.Parse(_glbTB.Text);
                fpga.LogicCells = int.Parse(_logicCells.Text);
                fpga.LUT = int.Parse(_lutTB.Text);
                //IO
                fpga.DifferentialIO = int.Parse(_difIOTB.Text);
                fpga.SingalIO = int.Parse(_singalIOTB.Text);
                //memory
                fpga.TotalBlock = int.Parse(_totalBlockTB.Text);
                fpga.Block = int.Parse(_blockTB.Text);
                fpga.MaxDistributed = int.Parse(_maxDstTB.Text);
                //speedLevel
                fpga.IndustrialTemp = int.Parse(_indTempTB.Text);
                fpga.ExtenedTemp = int.Parse(_extenedTempTB.Text);
                fpga.Commercial = int.Parse(_commercialTB.Text);
                //IPResource
                fpga.GTP = int.Parse(_gtpTB.Text);
                fpga.AES = int.Parse(_aesTB.Text);
                fpga.AMS = int.Parse(_amsTB.Text);
                fpga.PCIE = int.Parse(_pcieTB.Text);
                fpga.DSP = _dspTB.Text;
            }
            catch(Exception e)
            {
                MessageBox.Show("填入值异常", e.Message);
            }
        }

        protected override void SetFatherComponents()
        {
            /*清除某些不必要的显示*/
            this.Text = "FPGAInitForm";
            this.SuspendLayout();
            base.Controls.Remove(base._collectionPanel);
            base._infoPanel.Dock = DockStyle.Fill;
            this.ResumeLayout();
        }

        public override string GetObjectName()
        {
            return new string(_nameTB.Text.ToCharArray());
        }

        //FPGA描述信息XML写入前完整判断
        private Boolean CompleteJudgment()
        {
            if (String.Empty == _typeCB.Text || String.Empty == _adCB.Text || String.Empty == _clockTB.Text)
            {
                MessageBox.Show("基础属性不能为空");
                return false;
            }
            if (String.Empty == _slicesTB.Text || String.Empty == _glbTB.Text || String.Empty == _logicCells.Text ||
                String.Empty == _lutTB.Text)
            {
                MessageBox.Show("逻辑资源信息不能为空");
                return false;
            }
            if (String.Empty == _difIOTB.Text || String.Empty == _singalIOTB.Text)
            {
                MessageBox.Show("IO资源不能为空");
                return false;
            }
            if (String.Empty == _totalBlockTB.Text || String.Empty == _blockTB.Text || String.Empty == _maxDstTB.Text)
            {
                MessageBox.Show("内存信息不能为空");
                return false;
            }
            if (String.Empty == _commercialTB.Text || String.Empty == _extenedTempTB.Text || String.Empty == _indTempTB.Text)
            {
                MessageBox.Show("速度等级不能为空");
                return false;
            }
            if (String.Empty == _gtpTB.Text || String.Empty == _aesTB.Text || String.Empty == _amsTB.Text
                || String.Empty == _pcieTB.Text || String.Empty == _dspTB.Text)
            {
                MessageBox.Show("IP核信息不能为空");
                return false;
            }
            return true;
        }
    }

}
