using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DRSysCtrlDisplay
{
    public partial class OutPutForm : DockContent
    {
        private static OutPutForm uniqueInstance;

        private OutPutForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
        }

        public static OutPutForm GetInstacne()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new OutPutForm();
            }
            return uniqueInstance;
        }

#region 事件处理函数

        protected override void OnLoad(EventArgs e)
        {
            MainForm mainForm = MainForm.GetInstance();
            mainForm.OutPutToolStripMenuItem.Checked = true;
            base.OnLoad(e);
        }

        //关闭窗口的事件处理函数改写
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm mainForm = MainForm.GetInstance();
            mainForm.OutPutToolStripMenuItem.Checked = false;
            this.Hide();
            e.Cancel = true;
        }

#endregion 事件处理函数
    }
}
