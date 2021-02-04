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
    public partial class BoardInitForm_AddInterChangeForm : Form
    {
        //交换机类型和型号的属性
        public String Type { get; set; }
        public String Model { get; set; }
        public BoardInitForm_AddInterChangeForm()
        {
            InitializeComponent();
        }
        private void BoardInitForm_AddChipForm_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "以太网交换机";
            textBox1.Text = String.Empty;
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            if (false == CompleteJudgment())
            {
                return;
            }
            Type = comboBox1.Text;
            Model = textBox1.Text;
            this.DialogResult = DialogResult.Yes;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        //芯片选择完整性判断
        private Boolean CompleteJudgment()
        {
            if (comboBox1.Text == String.Empty || textBox1.Text == String.Empty)
            { 
                MessageBox.Show("请录入交换机完整信息");
                return false;
            }
            return true;
        }




    }
}
