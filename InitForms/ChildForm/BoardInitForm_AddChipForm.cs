using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace DRSysCtrlDisplay
{
    public partial class BoardInitForm_AddChipForm : Form
    {
        public String Type { get; set; }
        public String Model { get; set; }
        public BoardInitForm_AddChipForm()
        {
            InitializeComponent();
        }
        //类型选择框内容改变响应
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            switch(comboBox1.Text)
            {
                case "PPC":
                    string[] ppcChipNode = FuncItemsForm.GetInstance().GetEqSetNames(Princeple.FormType.PPC); ;
                    ComboBox2_AddItems(ppcChipNode);
                    break;
                case "FPGA":
                    string[] fpgaChipNode = FuncItemsForm.GetInstance().GetEqSetNames(Princeple.FormType.FPGA); ;
                    ComboBox2_AddItems(fpgaChipNode);
                    break;
                case "ZYNQ":
                    string[] zynqChipNode = FuncItemsForm.GetInstance().GetEqSetNames(Princeple.FormType.ZYNQ); ;
                    ComboBox2_AddItems(zynqChipNode);
                    break;
                default:
                    break;
            }
        }
        //获取指定类型芯片集合
        private string[] ReadChipNodeXml(String path)
        {
            string[] chipNodes = {};
            List<string> _list = new List<string>(chipNodes);
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            chipNodes = new string[dirInfo.GetFiles().Length];
            foreach (FileInfo f in dirInfo.GetFiles())
            {
                _list.Add(f.Name.Replace(".xml", ""));
            }
            chipNodes = _list.ToArray();
            return chipNodes;
        }
        //芯片型号动态变化供选择
        private void ComboBox2_AddItems(string[] chipNode)
        {
            foreach (string strs in chipNode)
            {
                comboBox2.Items.Add(strs);
            }
        }

        private void BoardInitForm_AddChipForm_Load(object sender, EventArgs e)
        {
            comboBox1.Text = String.Empty;
            comboBox2.Text = String.Empty;
        }
        
        private void YesButton_Click(object sender, EventArgs e)
        {
            if (false == CompleteJudgment())
            {
                return;
            }
            Type = comboBox1.Text;
            Model = comboBox2.Text;
            this.DialogResult = DialogResult.Yes;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        //芯片选择完整性判断
        private Boolean CompleteJudgment()
        {
            if (comboBox1.Text == String.Empty || comboBox2.Text == String.Empty)
            { 
                MessageBox.Show("请录入完整芯片信息");
                return false;
            }
            return true;
        }




    }
}
