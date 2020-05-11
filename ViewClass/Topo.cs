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
    public class Topo : BaseView
    {
        TreeView ShowTree;

        public Topo(Panel panel)
        {
            Init(panel);
        }

        private void Init(Panel panel)
        {
            ShowTree = new TreeView();
            TreeNode treeNode1 = new TreeNode("位置信息");
            TreeNode treeNode2 = new TreeNode("RapidIO");
            TreeNode treeNode3 = new TreeNode("以太网");
            TreeNode treeNode4 = new TreeNode("GTX");
            TreeNode treeNode5 = new TreeNode("LVDS");
            TreeNode treeNode6 = new TreeNode("全部显示", new TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});

            ShowTree.CheckBoxes = true;
            ShowTree.Dock = System.Windows.Forms.DockStyle.None;
            ShowTree.Location = new System.Drawing.Point(0, 0);
            ShowTree.Name = "ShowTree";
            treeNode1.Name = "treeNode1";
            treeNode1.Text = "位置信息";
            treeNode2.Name = "treeNode2";
            treeNode2.Text = "RapidIO";
            treeNode3.Name = "treeNode3";
            treeNode3.Text = "以太网";
            treeNode4.Name = "treeNode4";
            treeNode4.Text = "GTX";
            treeNode5.Name = "treeNode5";
            treeNode5.Text = "LVDS";
            treeNode6.Name = "treeNode6";
            treeNode6.Text = "全部显示";
            ShowTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            ShowTree.Size = new System.Drawing.Size(100, 100);
            ShowTree.TabIndex = 0;
            panel.Controls.Add(ShowTree);
        }

        public override void DrawView(Graphics g)
        {
            Rectangle r = new Rectangle(100, 100, 100, 100);
            g.DrawRectangle(Pens.Blue, r);
            g.FillRectangle(Brushes.Blue, r);
        }
    }
}
