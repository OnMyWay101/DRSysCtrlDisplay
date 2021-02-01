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
    //属性显示的子窗口；单例模式；
    public partial class PropertyForm : DockContent
    {
        private static PropertyForm uniqueInstance;

        private PropertyForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
        }

        public static PropertyForm GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new PropertyForm();
            }
            return uniqueInstance;
        }

#region 事件处理函数

        //窗体加载事件处理器
        protected override void OnLoad(EventArgs e)
        {
            MainForm mainForm = MainForm.GetInstance();
            mainForm.PropToolStripMenuItem.Checked = true;
            this.propertyGrid1.PropertySort = PropertySort.CategorizedAlphabetical;
            base.OnLoad(e);
        }

        //关闭窗口的事件处理函数改写
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm mainForm = MainForm.GetInstance();
            mainForm.PropToolStripMenuItem.Checked = false;
            this.Hide();
            e.Cancel = true;
        }

#endregion 事件处理函数

        public PropertyGrid GetGrid()
        {
            return uniqueInstance.propertyGrid1;
        }

        public static void Show(object o)
        {
            PropertyForm.GetInstance().GetGrid().SelectedObject = o;
        }
    }
}
