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
    public partial class SlotsInitForm_AddSlot : Form
    {
        public String LocalLocation { get; set; }
        public String OppositeNum { get; set; }
        public String OppositeLoction { get; set; }
        public String LinkRelation { get; set; }
        public SlotsInitForm_AddSlot()
        {
            InitializeComponent();
        }
        //确定按钮按下事件响应
        private void button2_Click(object sender, EventArgs e)
        {
            if (false == CompleteJudgment())
            {
                return;
            }
            LocalLocation = this.textBox1.Text;
            OppositeNum = this.textBox2.Text;
            OppositeLoction = this.textBox3.Text;
            LinkRelation = this.comboBox1.Text;
            this.DialogResult = DialogResult.Yes;
        }
        //取消按钮按下事件响应
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private Boolean CompleteJudgment()
        {
            if (String.Empty == textBox1.Text || String.Empty == textBox2.Text || String.Empty == textBox3.Text
                || String.Empty == comboBox1.Text)
            {
                MessageBox.Show("请填写完整槽位信息！");
                return false;
            }
            return true;
        }
    }
}
