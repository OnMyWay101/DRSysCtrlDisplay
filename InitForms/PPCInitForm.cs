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
using DRSysCtrlDisplay.Models;

namespace DRSysCtrlDisplay
{
    public class PPCInitForm : Form
    {
        #region Designer生成的控件
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.Button _yesBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        protected System.Windows.Forms.TextBox _nameTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox _coreNumCB;
        private System.Windows.Forms.TextBox _mainFrequencyTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _fileSystemTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox _memoryTB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox _typeCB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox _vectorEnginCB;
        #endregion

        //Designer初始化函数
        private void InitializeComponent()
        {
            this._cancelBtn = new System.Windows.Forms.Button();
            this._yesBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._nameTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._vectorEnginCB = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this._memoryTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this._typeCB = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._fileSystemTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._coreNumCB = new System.Windows.Forms.ComboBox();
            this._mainFrequencyTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this._cancelBtn.Location = new System.Drawing.Point(30, 337);
            this._cancelBtn.Name = "button1";
            this._cancelBtn.Size = new System.Drawing.Size(75, 23);
            this._cancelBtn.TabIndex = 1;
            this._cancelBtn.Text = "取消";
            this._cancelBtn.UseVisualStyleBackColor = true;
            this._cancelBtn.Click += new System.EventHandler(this._cancelBtn_Click);
            // 
            // button2
            // 
            this._yesBtn.Location = new System.Drawing.Point(159, 337);
            this._yesBtn.Name = "button2";
            this._yesBtn.Size = new System.Drawing.Size(75, 23);
            this._yesBtn.TabIndex = 2;
            this._yesBtn.Text = "确定";
            this._yesBtn.UseVisualStyleBackColor = true;
            this._yesBtn.Click += new System.EventHandler(this._yesBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._nameTB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 64);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基础信息";
            // 
            // textBox1
            // 
            this._nameTB.Location = new System.Drawing.Point(80, 30);
            this._nameTB.Name = "textBox1";
            this._nameTB.Size = new System.Drawing.Size(121, 21);
            this._nameTB.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._vectorEnginCB);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this._memoryTB);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this._typeCB);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this._fileSystemTB);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this._coreNumCB);
            this.groupBox2.Controls.Add(this._mainFrequencyTB);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(1, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 260);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "芯片参数";
            // 
            // comboBox3
            // 
            this._vectorEnginCB.FormattingEnabled = true;
            this._vectorEnginCB.Items.AddRange(new object[] {
            "true",
            "false"});
            this._vectorEnginCB.Location = new System.Drawing.Point(80, 201);
            this._vectorEnginCB.Name = "comboBox3";
            this._vectorEnginCB.Size = new System.Drawing.Size(121, 20);
            this._vectorEnginCB.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 204);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 13;
            this.label10.Text = "矢量引擎";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(210, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "MB";
            // 
            // textBox4
            // 
            this._memoryTB.Location = new System.Drawing.Point(80, 169);
            this._memoryTB.Name = "textBox4";
            this._memoryTB.Size = new System.Drawing.Size(121, 21);
            this._memoryTB.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 172);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "内存";
            // 
            // comboBox2
            // 
            this._typeCB.FormattingEnabled = true;
            this._typeCB.Items.AddRange(new object[] {
            "P2020",
            "8640",
            "8640D"});
            this._typeCB.Location = new System.Drawing.Point(80, 63);
            this._typeCB.Name = "comboBox2";
            this._typeCB.Size = new System.Drawing.Size(121, 20);
            this._typeCB.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "类型";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(210, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "MB";
            // 
            // textBox3
            // 
            this._fileSystemTB.Location = new System.Drawing.Point(80, 133);
            this._fileSystemTB.Name = "textBox3";
            this._fileSystemTB.Size = new System.Drawing.Size(121, 21);
            this._fileSystemTB.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "文件系统";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(210, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "MHZ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "主频";
            // 
            // comboBox1
            // 
            this._coreNumCB.FormattingEnabled = true;
            this._coreNumCB.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8"});
            this._coreNumCB.Location = new System.Drawing.Point(80, 25);
            this._coreNumCB.Name = "comboBox1";
            this._coreNumCB.Size = new System.Drawing.Size(121, 20);
            this._coreNumCB.TabIndex = 2;
            // 
            // textBox2
            // 
            this._mainFrequencyTB.Location = new System.Drawing.Point(80, 99);
            this._mainFrequencyTB.Name = "textBox2";
            this._mainFrequencyTB.Size = new System.Drawing.Size(121, 21);
            this._mainFrequencyTB.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "核心数量";
            // 
            // PPCInitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 372);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._yesBtn);
            this.Controls.Add(this._cancelBtn);
            this.Name = "PPCInitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PropertyInit";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 修改节点时，使用PPC实例初始化PPCInitForm中各项的值
        /// </summary>
        /// <param name="ppc"></param>
        public PPCInitForm(PPC ppc)
        {
            InitializeComponent();
            _nameTB.Text = ppc.Name;
            _coreNumCB.SelectedItem = ppc.CoreNum.ToString();
            _typeCB.SelectedItem = ppc.Type;
            _mainFrequencyTB.Text = ppc.Frequency.ToString();
            _fileSystemTB.Text = ppc.FileSystem.ToString();
            _memoryTB.Text = ppc.Memory.ToString();
            _vectorEnginCB.SelectedItem = ppc.VectorEngin;
        }

        public PPCInitForm()
        {
            InitializeComponent();
            //设置窗体显示位置
            this.StartPosition = FormStartPosition.CenterParent;

            //_nameTB.Text = "输入名称";
            //_coreNumCB.Text = "0";
            //_typeCB.Text = "NoType";
            //_mainFrequencyTB.Text = "0";
            //_fileSystemTB.Text = "0";
            //_memoryTB.Text = "0";
            //_vectorEnginCB.Text = "无";
        }

        /// <summary>
        /// 获取初始化对象的名字
        /// </summary>
        /// <returns></returns>
        public string GetObjectName()
        {
            return new string(_nameTB.Text.ToCharArray());
        }

        protected virtual void _yesBtn_Click(object sender, EventArgs e)
        {
            if (!CompleteJudgment())
            {
                MessageBox.Show("数据不完整");
                return;
            }
            PPC ppc = new PPC();
            RefreshPPC(ppc);
            ppc.SaveXmlByName();
            this.DialogResult = DialogResult.Yes;
        }

        private void _cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        protected void RefreshPPC(PPC ppc)
        {
            ppc.Name = _nameTB.Text;
            ppc.Type = _typeCB.Text;
            ppc.Frequency = int.Parse(_mainFrequencyTB.Text);
            ppc.CoreNum = int.Parse(_coreNumCB.Text);
            ppc.VectorEngin = bool.Parse(_vectorEnginCB.Text);
            ppc.Memory = int.Parse(_memoryTB.Text);
            ppc.FileSystem = int.Parse(_fileSystemTB.Text);
        }

        /// <summary>
        /// 填入内容完整性的判断
        /// </summary>
        /// <returns></returns>
        private Boolean CompleteJudgment()
        {
            if (String.Empty == _nameTB.Text)
            {
                MessageBox.Show("请输入名称！");
                return false;
            }
            if (String.Empty == _mainFrequencyTB.Text 
                    || String.Empty == _fileSystemTB.Text 
                    || String.Empty == _memoryTB.Text 
                    || String.Empty == _coreNumCB.Text
                    || String.Empty == _typeCB.Text
                    || String.Empty == _vectorEnginCB.Text)
            {
                MessageBox.Show("芯片参数不能为空！");
                return false;
            }
            return true;
        }
    }
}
