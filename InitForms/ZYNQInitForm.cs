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
    public class ZYNQInitForm : Form
    {
        #region Designer生成的控件
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _coreNumTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _psTypeCB;
        private System.Windows.Forms.ComboBox _expanfMemoryCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox _memoryCB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _mainClockCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox _logicNumTB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox _plTypeCB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox _adCB;
        private System.Windows.Forms.TextBox _dspSliceTB;
        private System.Windows.Forms.TextBox _bolckArmTB;
        private System.Windows.Forms.TextBox _flipFlopsTB;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox _lutTB;
        private System.Windows.Forms.Button nextButton;
        protected System.Windows.Forms.TextBox _nameTB;
        private System.Windows.Forms.Label label13;
        #endregion

        public ZYNQInitForm()
        {
            InitializeComponent();
            //设置窗体显示位置
            this.StartPosition = FormStartPosition.CenterParent;
        }
        public ZYNQInitForm(ZYNQ zynq)
        {
            InitializeComponent();

            _nameTB.Text = zynq.Name;
            //PS相关的参数
            _psTypeCB.SelectedItem = zynq.PSType;
            _coreNumTB.Text = zynq.CoreNum.ToString();
            _mainClockCB.SelectedItem = zynq.MainClock;
            _memoryCB.SelectedItem = zynq.Memory;
            _expanfMemoryCB.SelectedItem = zynq.ExpandMemory;

            //PL相关的参数
            _plTypeCB.SelectedItem = zynq.PLType;
            _logicNumTB.Text = zynq.LogicNum.ToString();
            _lutTB.Text = zynq.LUT.ToString();
            _flipFlopsTB.Text = zynq.Flip_Flops.ToString();
            _bolckArmTB.Text = zynq.Block_ARM.ToString();
            _dspSliceTB.Text = zynq.DSP_Slice;
            _adCB.Text = zynq.AD;
        }

        //Designer生成的初始化代码
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._expanfMemoryCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this._memoryCB = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this._mainClockCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._coreNumTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._psTypeCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._adCB = new System.Windows.Forms.ComboBox();
            this._dspSliceTB = new System.Windows.Forms.TextBox();
            this._bolckArmTB = new System.Windows.Forms.TextBox();
            this._flipFlopsTB = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this._lutTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._logicNumTB = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this._plTypeCB = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.yesButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this._nameTB = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 49);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(279, 394);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._expanfMemoryCB);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this._memoryCB);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this._mainClockCB);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this._coreNumTB);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this._psTypeCB);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage1.Size = new System.Drawing.Size(271, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "PS";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _expanfMemoryCB
            // 
            this._expanfMemoryCB.FormattingEnabled = true;
            this._expanfMemoryCB.Items.AddRange(new object[] {
            "256MB",
            "512MB",
            "1000MB",
            "2000MB"});
            this._expanfMemoryCB.Location = new System.Drawing.Point(90, 155);
            this._expanfMemoryCB.Name = "_expanfMemoryCB";
            this._expanfMemoryCB.Size = new System.Drawing.Size(139, 20);
            this._expanfMemoryCB.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "扩展内存:";
            // 
            // _memoryCB
            // 
            this._memoryCB.FormattingEnabled = true;
            this._memoryCB.Items.AddRange(new object[] {
            "256KB"});
            this._memoryCB.Location = new System.Drawing.Point(90, 116);
            this._memoryCB.Name = "_memoryCB";
            this._memoryCB.Size = new System.Drawing.Size(139, 20);
            this._memoryCB.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "芯片内存:";
            // 
            // _mainClockCB
            // 
            this._mainClockCB.FormattingEnabled = true;
            this._mainClockCB.Items.AddRange(new object[] {
            "667MHZ",
            "800MHZ",
            "1000MHZ"});
            this._mainClockCB.Location = new System.Drawing.Point(90, 76);
            this._mainClockCB.Name = "_mainClockCB";
            this._mainClockCB.Size = new System.Drawing.Size(139, 20);
            this._mainClockCB.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "主频:";
            // 
            // _coreNumTB
            // 
            this._coreNumTB.Location = new System.Drawing.Point(90, 38);
            this._coreNumTB.Name = "_coreNumTB";
            this._coreNumTB.Size = new System.Drawing.Size(139, 21);
            this._coreNumTB.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "核心数:";
            // 
            // _psTypeCB
            // 
            this._psTypeCB.FormattingEnabled = true;
            this._psTypeCB.Items.AddRange(new object[] {
            "Z-7045"});
            this._psTypeCB.Location = new System.Drawing.Point(90, 7);
            this._psTypeCB.Name = "_psTypeCB";
            this._psTypeCB.Size = new System.Drawing.Size(139, 20);
            this._psTypeCB.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "类型:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this._adCB);
            this.tabPage2.Controls.Add(this._dspSliceTB);
            this.tabPage2.Controls.Add(this._bolckArmTB);
            this.tabPage2.Controls.Add(this._flipFlopsTB);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this._lutTB);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this._logicNumTB);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this._plTypeCB);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage2.Size = new System.Drawing.Size(271, 368);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "PL";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // _adCB
            // 
            this._adCB.FormattingEnabled = true;
            this._adCB.Items.AddRange(new object[] {
            "有",
            "无"});
            this._adCB.Location = new System.Drawing.Point(83, 214);
            this._adCB.Name = "_adCB";
            this._adCB.Size = new System.Drawing.Size(139, 20);
            this._adCB.TabIndex = 26;
            // 
            // _dspSliceTB
            // 
            this._dspSliceTB.Location = new System.Drawing.Point(83, 183);
            this._dspSliceTB.Name = "_dspSliceTB";
            this._dspSliceTB.Size = new System.Drawing.Size(139, 21);
            this._dspSliceTB.TabIndex = 25;
            // 
            // _bolckArmTB
            // 
            this._bolckArmTB.Location = new System.Drawing.Point(83, 155);
            this._bolckArmTB.Name = "_bolckArmTB";
            this._bolckArmTB.Size = new System.Drawing.Size(139, 21);
            this._bolckArmTB.TabIndex = 24;
            // 
            // _flipFlopsTB
            // 
            this._flipFlopsTB.Location = new System.Drawing.Point(83, 119);
            this._flipFlopsTB.Name = "_flipFlopsTB";
            this._flipFlopsTB.Size = new System.Drawing.Size(139, 21);
            this._flipFlopsTB.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 217);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 12);
            this.label12.TabIndex = 22;
            this.label12.Text = "AD:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 186);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 21;
            this.label11.Text = "DSP Slice:";
            // 
            // _lutTB
            // 
            this._lutTB.Location = new System.Drawing.Point(83, 76);
            this._lutTB.Name = "_lutTB";
            this._lutTB.Size = new System.Drawing.Size(139, 21);
            this._lutTB.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "Block RAM:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "Flip-Flops:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "LUT:";
            // 
            // _logicNumTB
            // 
            this._logicNumTB.Location = new System.Drawing.Point(83, 38);
            this._logicNumTB.Name = "_logicNumTB";
            this._logicNumTB.Size = new System.Drawing.Size(139, 21);
            this._logicNumTB.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "逻辑单元数:";
            // 
            // _plTypeCB
            // 
            this._plTypeCB.FormattingEnabled = true;
            this._plTypeCB.Items.AddRange(new object[] {
            "Kintex-7",
            "Artix-7",
            "Virtex-7",
            "Spartan-6"});
            this._plTypeCB.Location = new System.Drawing.Point(83, 7);
            this._plTypeCB.Name = "_plTypeCB";
            this._plTypeCB.Size = new System.Drawing.Size(139, 20);
            this._plTypeCB.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "类型:";
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(94, 410);
            this.yesButton.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 1;
            this.yesButton.Text = "确定";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Click += new System.EventHandler(this.YesButton_Clicked);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(192, 410);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Clicked);
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(5, 410);
            this.nextButton.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 3;
            this.nextButton.Text = "下一步";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.NextButton_Clicked);
            // 
            // _nameTB
            // 
            this._nameTB.Location = new System.Drawing.Point(79, 10);
            this._nameTB.Name = "_nameTB";
            this._nameTB.Size = new System.Drawing.Size(139, 21);
            this._nameTB.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 13);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 12);
            this.label13.TabIndex = 4;
            this.label13.Text = "名称:";
            // 
            // ZYNQInitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 443);
            this.Controls.Add(this._nameTB);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "ZYNQInitForm";
            this.Text = "ZYNQInitForm";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region 事件处理函数
        //确定按钮按下事件响应
        protected virtual void YesButton_Clicked(object sender, EventArgs e)
        {
            if (false == CompleteJudgment())
            {
                return;
            }
            ZYNQ zynq = new ZYNQ();
            RefreshZYNQ(zynq);
            zynq.SaveXmlByName();

            this.DialogResult = DialogResult.Yes;
        }

        //取消按钮按下事件响应
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;   
        }

        //"下一步"按钮按下事件响应
        private void NextButton_Clicked(object sender, EventArgs e)
        {
            //TabControl.TabPageCollection pages = tabControl1.TabPages;
            if (tabControl1.SelectedTab == tabPage1)
            {
                tabControl1.SelectedTab = tabPage2;
                this.nextButton.Enabled = false;
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                this.nextButton.Enabled = true;
            }
            else
            {
                this.nextButton.Enabled = false;
            }
        }

        #endregion

        /// <summary>
        /// 通过ZYNQ控件的属性初始化一个ZYNQ实例对象的属性值
        /// </summary>
        /// <param name="zynq"></param>
        protected void RefreshZYNQ(ZYNQ zynq)
        {
            try
            {
                zynq.Name = _nameTB.Text;
                //PS相关参数
                zynq.PSType = _psTypeCB.Text;
                zynq.CoreNum = int.Parse(_coreNumTB.Text);
                zynq.MainClock = _mainClockCB.Text;
                zynq.Memory = _memoryCB.Text;
                zynq.ExpandMemory = _expanfMemoryCB.Text;
                //PL相关的参数
                zynq.PLType = _plTypeCB.Text;
                zynq.LogicNum = int.Parse(_logicNumTB.Text);
                zynq.LUT = int.Parse(_lutTB.Text);
                zynq.Flip_Flops = int.Parse(_flipFlopsTB.Text);
                zynq.Block_ARM = int.Parse(_bolckArmTB.Text);
                zynq.DSP_Slice = _dspSliceTB.Text;
                zynq.AD = _adCB.Text;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "填入值异常");
            }
        }

        public string GetObjectName()
        {
            return new string(_nameTB.Text.ToCharArray());
        }

        //ZYNQ描述信息XML写入前完整性判断
        private Boolean CompleteJudgment()
        {
            if (String.Empty == _psTypeCB.Text || String.Empty == _coreNumTB.Text || String.Empty == _mainClockCB.Text ||
                String.Empty == _memoryCB.Text || String.Empty == _expanfMemoryCB.Text)
            {
                MessageBox.Show("PS属性不能为空!");
                return false;
            }
            if (String.Empty == _plTypeCB.Text || String.Empty == _logicNumTB.Text || String.Empty == _lutTB.Text ||
                String.Empty == _flipFlopsTB.Text || String.Empty == _bolckArmTB.Text || String.Empty == _dspSliceTB.Text ||
                String.Empty == _adCB.Text)
            {
                MessageBox.Show("PL属性不能为空!");
                return false;
            }
            return true;
        }
    }
}
