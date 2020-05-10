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
    public partial class MainForm : Form
    {
        private static MainForm uniqueInstance;

        private MainForm()
        {
            InitializeComponent();
            FuncToolStripMenuItem.Click += new EventHandler(FuncToolStripMenuItem_Click);
            PropToolStripMenuItem.Click += new EventHandler(PropToolStripMenuItem_Click);
            OutPutToolStripMenuItem.Click += new EventHandler(OutPutToolStripMenuItem_Click);
        }

        public static MainForm GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new MainForm();
            }
            return uniqueInstance;
        }

       
#region 事件处理函数

        #region 主窗体

        public void MainForm_Load(object sender, EventArgs e)
        {
            //显示FuncItem            
            FuncItemsForm fItemsWnd = FuncItemsForm.GetInstance();
            fItemsWnd.Show(dockPanel1, DockState.DockLeft);

            //显示Property
            PropertyForm propertyWnd = PropertyForm.GetInstance();
            propertyWnd.Show(dockPanel1, DockState.DockRight);

            //显示OutPutForm
            OutPutForm outPutForm = OutPutForm.GetInstacne();
            outPutForm.Show(dockPanel1, DockState.DockBottom);
        }

        #endregion 主窗体

        #region 菜单
        public void FuncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FuncItemsForm fItemsWnd = FuncItemsForm.GetInstance();
            ToolStripMenuItem item = (ToolStripMenuItem) sender;

            item.Checked = !item.Checked;
            if (item.Checked)
            {
                fItemsWnd.Show(dockPanel1, DockState.DockLeft);
            }
            else
            {
                fItemsWnd.Hide();
            }
        }
        public void PropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyForm propertyWnd = PropertyForm.GetInstance();
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            item.Checked = !item.Checked;
            if (item.Checked)
            {
                propertyWnd.Show(dockPanel1, DockState.DockRight);
            }
            else
            {
                propertyWnd.Hide();
            }
        }
        public void OutPutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutPutForm outPutForm = OutPutForm.GetInstacne();
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            item.Checked = !item.Checked;
            if (item.Checked)
            {
                outPutForm.Show(dockPanel1, DockState.DockBottom);
            }
            else
            {
                outPutForm.Hide();
            }
        }

        #endregion 菜单

#endregion 事件处理函数

        public static DockPanel GetPanel()
        {
            return uniqueInstance.dockPanel1;
        }
    }
}
