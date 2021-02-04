using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace DRSysCtrlDisplay
{
    public partial class BoardInitForm_AddConnect : Form
    {
        public String Port1_Type { get; set; }
        public String Port1_SN { get; set; }
        public String Port1_Num { get; set; }
        public String Port2_Type { get; set; }
        public String Port2_SN { get; set; }
        public String Port2_Num { get; set; }
        public String Speed { get; set; }
        public String Channel { get; set; }
        public String _linkType { get; set; }


        public BoardInitForm_AddConnect(string linkType)
        {
            InitializeComponent();
            _linkType = linkType;
        }

        /* 描述：确定按钮按下响应函数
         * 参数：略
         * 返回值：void
         */
        private void YesButton_Click(object sender, EventArgs e)
        {
            if (false == CompleteJudgment())
            {
                return;
            }
            Port1_Type = Port1TypeBox.Text;
            Port1_SN = Model1Box.SelectedIndex.ToString();            
            Port1_Num = textBox1.Text;
            Port2_Type = Port2TypeBox.Text;
            Port2_SN = Model2Box.SelectedIndex.ToString();
            Port2_Num = textBox2.Text;
            Speed = textBox3.Text;
            Channel = textBox4.Text;
            this.DialogResult = DialogResult.Yes;
        }
        /* 描述：取消按钮响应函数
         * 参数：略
         * 返回值：void
         */
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        /* 描述：端口1芯片选择类型变化响应函数
         * 参数：略
         * 返回值：void
         */
        private void Port1TypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Model1Box.Items.Clear();
            string[] typeModel = GetTypeModel(Port1TypeBox.Text);
            ModelBoxAddItem(this.Model1Box, typeModel);

        }
        /* 描述：端口2芯片选择类型变化响应函数
         * 参数：略
         * 返回值：void
         */
        private void Port2TypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Model2Box.Items.Clear();
            string[] typeModel = GetTypeModel(Port2TypeBox.Text);
            ModelBoxAddItem(this.Model2Box, typeModel);
        }
        /* 描述：型号下拉框添加项函数
         * 参数：
         * cb----需要操作的ComboBox实例
         * strs----存放Item文本的字符串数组
         * 返回值：void
         */
        private void ModelBoxAddItem(ComboBox cb, string[] strs)
        {
            foreach (string str in strs)
            {
                cb.Items.Add(str);
            }
        }
        /* 描述：获取芯片集已有选中类型芯片型号列表函数
         * 参数：type----芯片类型，FPGA、PPC、ZYNQ
         * 返回值：对应类型的型号字符串数组
         */
        private string[] GetTypeModel(string type)
        {
            BoardInitForm bif = (BoardInitForm)this.Owner;
            ListView lv = bif.ChipLV;
            ListView swlv = bif.SWLV;
            string[] models = { };
            List<string> _list = new List<string>(models);
            models = new string[lv.Items.Count];
            if (type == "EtherSW" || type == "RapidIOSW")
            {
                for (int i = 0; i < swlv.Items.Count; i++)
                {
                    if (type == swlv.Items[i].SubItems[1].Text)
                    {
                        _list.Add(swlv.Items[i].SubItems[2].Text);
                    }

                }
                models = _list.ToArray();
                return models;
            }
            if (type == "VPX")
            {
                _list.Add(_linkType);
                models = _list.ToArray();
                return models;
            }

            for (int i = 0; i < lv.Items.Count; i++)
            {

                if (type == lv.Items[i].SubItems[1].Text)
                {
                    _list.Add(lv.Items[i].SubItems[2].Text);
                }
                else
                {
                    continue;
                }
            }
            models = _list.ToArray();
            return models;
        }

        private Boolean CompleteJudgment()
        {
            if (String.Empty == Port1TypeBox.Text || String.Empty == Model1Box.Text || String.Empty == textBox1.Text)
            {
                MessageBox.Show("请填写端1完整信息！");
                return false;
            }
            else if (String.Empty == Port2TypeBox.Text || String.Empty == Model2Box.Text || String.Empty == textBox2.Text)
            {
                MessageBox.Show("请填写端2完整信息！");
                return false;
            }
            else if (String.Empty == textBox3.Text || String.Empty == textBox4.Text)
            {
                MessageBox.Show("请填写详细信息！");
                return false;
            }
            return true;
        }

    }
}
