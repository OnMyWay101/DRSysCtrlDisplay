using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml;

namespace DRSysCtrlDisplay
{
    public partial class FuncItemsForm : DockContent
    {
        private static FuncItemsForm uniqueInstance;

        private FuncItemsForm()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Form_FormClosed);
            this.FormClosing += new FormClosingEventHandler(Form_FormClosing); 
        }
        public static FuncItemsForm GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new FuncItemsForm();
            }
            return uniqueInstance;
        }
#region 事件处理函数

        /*资源池节点响应*/
        private void SrTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView selectedTree = (TreeView)sender;
            TreeNode selectedNode = selectedTree.SelectedNode;
            if (selectedNode.Level == 1)
                {
                    try
                    {
                        IPAddress addr = IPAddress.Parse(selectedNode.Parent.Text);
                        if (addr != null)
                        {
                            switch (selectedNode.Index)
                            {
                                case 0:
                                    ((NodeInfo)selectedNode.Tag)._formType = PPCInitForm.PropertyFormType.TOPO;
                                    break;
                                case 1:
                                    ((NodeInfo)selectedNode.Tag)._formType = PPCInitForm.PropertyFormType.STATU;
                                    break;
                                case 2:
                                    ((NodeInfo)selectedNode.Tag)._formType = PPCInitForm.PropertyFormType.APP;
                                    break;
                                default:
                                    return;
                                   
                            }
                            SelectSourceNodeToShow(selectedNode);
                        //    switch (selectedNode.Index)
                        //    {
                        //        case 0:
                        //            if (NodeInfo.FormShowed(selectedNode))
                        //            {
                        //                //关闭以前的窗口，新建一个
                        //                Form form = NodeInfo.GetForm(selectedNode);
                        //                if (form != null)
                        //                {
                        //                    form.Close();
                        //                    ConnectTopoView(addr, selectedNode);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                //显示资源池拓扑图dockform
                        //                ConnectTopoView(addr, selectedNode);
                        //            }
                        //            break;
                        //        case 1:
                        //            if (NodeInfo.FormShowed(selectedNode))
                        //            {
                        //                //关闭以前的窗口，新建一个
                        //                Form form = NodeInfo.GetForm(selectedNode);
                        //                if (form != null)
                        //                {
                        //                    form.Close();
                        //                    ConnectStatuView(addr, selectedNode);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                //显示状态视图dockform
                        //                ConnectStatuView(addr, selectedNode);
                        //            }
                        //            break;
                        //        case 2:
                        //            if (NodeInfo.FormShowed(selectedNode))
                        //            {
                        //                //关闭以前的窗口，新建一个
                        //                Form form = NodeInfo.GetForm(selectedNode);
                        //                if (form != null)
                        //                {
                        //                    form.Close();
                        //                    ConnectAppView(addr, selectedNode);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                //显示应用视图dockform
                        //                ConnectAppView(addr, selectedNode);
                        //            }
                        //            break;

                        //        default:
                        //            break;
                        //    }
                        }
                    }
                    catch (System.Exception)
                    { }
                }
        }

        /*设备库节点响应*/
        private void EqTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView selectedTree = (TreeView)sender;
            TreeNode selectedNode = selectedTree.SelectedNode;
            /*芯片库节点操作*/
            if (selectedNode.Level == 2)
            {
                SelectLibNodeToShow(selectedNode);
            }
            /*板卡库、背板库、机箱库节点操作*/
            else if (selectedNode.Level == 1)
            {
                if (selectedNode.Parent.Index != 0)
                {
                    SelectLibNodeToShow(selectedNode);
                    
                }
            }
        }

        /*构件库节点响应*/
        private void CpTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView selectedTree = (TreeView)sender;
            TreeNode selectedNode = selectedTree.SelectedNode;

        }

        /*弹出节点右键菜单*/
        private void LibContextMS_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {         
                //设置当前右键节点为TreeView的selectedNode
                TreeView tree = (TreeView)sender;
                TreeNode clickNode = tree.GetNodeAt(e.Location);
                tree.SelectedNode = clickNode;

                if (tree.SelectedNode.Level == 0)
                {
                    if (tree.SelectedNode.Index != 0)
                    {
                        _libCMS.Show(this, e.Location);
                    }
                }
                else if (tree.SelectedNode.Level == 1)
                {
                    if (tree.SelectedNode.Parent.Index == 0)
                    {
                        _libCMS.Show(this, e.Location);
                    }
                    else
                    {
                        _cpCMS.Show(this, e.Location);
                    }
                }
                else if (tree.SelectedNode.Level == 2)
                {
                    _cpCMS.Show(this, e.Location);
                }
            }
            //注意;美化弹出效果
        }

        /*组件添加窗口*/
        private void ContextMSAdd_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _eqTreeView.SelectedNode;
            Form initForm;
            PPCInitForm ppcInitForm;

            /*@todo:初始窗口不用分类，可以放在protInitForm加载的时候分类*/
            switch (selectedNode.Text)
            {
                //显示属性初始化窗口
                case "PPC":
                    ppcInitForm = new PPCInitForm(selectedNode);
                    ppcInitForm.ShowDialog();
                    if (ppcInitForm.DialogResult == DialogResult.Yes)
                    {
                        //创建对应的TreeNode
                        XMLManager.PropertyToNode(ppcInitForm.GetName(), selectedNode, ppcInitForm.GetInfo());
                        ppcInitForm.Dispose();
                    }
                    break;
                case "FPGA":
                    initForm = new FPGAInitForm();
                    initForm.ShowDialog();
                    if (initForm.DialogResult == DialogResult.Yes)
                    {
                        initForm.Dispose();
                    }
                    break;
                case "ZYNQ":
                    initForm = new ZYNQInitForm();
                    initForm.ShowDialog();
                    if (initForm.DialogResult == DialogResult.Yes)
                    {
                        initForm.Dispose();
                    }
                    break;
                case "板卡库":
                    initForm = new BoardInitForm();
                    initForm.ShowDialog();
                    if (initForm.DialogResult == DialogResult.Yes)
                    {
                        initForm.Dispose();
                    }
                    break;
                default:

                    break;
            }
        }

        /*组件删除*/
        private void ContextMSDelete_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _eqTreeView.SelectedNode;
            DeleteNode(selectedNode);
            
        }
        /*组件修改*/
        private void ContextMSModify_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _eqTreeView.SelectedNode;
            ModifyNody(selectedNode);
        }

        //窗体加载事件处理器
        private void FuncItems_Load(object sender, EventArgs e)
        {
            NodeInfo.InitTreeNodeInfo(_srTreeView);
            String eqFilePath = TreeXML.EqconfigXMLPath;
            String cpFilePath = TreeXML.CpconfigXMLPath;

            MainForm mainForm = MainForm.GetInstance();
            mainForm.FuncToolStripMenuItem.Checked = true;
            try
            {
                TreeXML.ReadXML(_eqTreeView, eqFilePath);
            }
            catch
            {
                MessageBox.Show("FuncItems:库XML文件" + eqFilePath + "读取失败！");
            }
            try
            {
                TreeXML.ReadXML(_cpTreeView, cpFilePath);
            }
            catch
            {
                MessageBox.Show("FuncItems:库XML文件" + cpFilePath + "读取失败！");
            }
            return;
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            String eqFilePath = TreeXML.EqconfigXMLPath;
            String cpFilePath = TreeXML.CpconfigXMLPath;
            String fileDirectory = TreeXML.XMLDirectory;
            try
            {
                TreeXML.SaveTree(_eqTreeView, eqFilePath, fileDirectory);
            }
            catch
            {
                MessageBox.Show("FuncItems:库XML文件" + eqFilePath + "保存失败！");
            }
            try
            {
                TreeXML.SaveTree(_cpTreeView, cpFilePath, fileDirectory);
            }
            catch
            {
                MessageBox.Show("FuncItems:库XML文件" + cpFilePath + "保存失败！");
            }
        }


        //关闭窗口的事件处理函数改写
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm mainForm = MainForm.GetInstance();
            mainForm.FuncToolStripMenuItem.Checked = false;
            this.Hide();
            e.Cancel = true;
        }

#endregion 事件处理函数

        private void ConnectTopoView(IPAddress addr, TreeNode node)
        {
            //TopoView topoView = new TopoView(node);
            //WeifenLuo.WinFormsUI.Docking.DockPanel panel = _mainForm.GetPanel();
            //topoView.Show(panel);

        }

        private void ConnectStatuView(IPAddress addr, TreeNode node)
        {
            //StatuView statuView = new StatuView(node);
            //WeifenLuo.WinFormsUI.Docking.DockPanel panel = _mainForm.GetPanel();
            //statuView.Show(panel);
        }

        private void ConnectAppView(IPAddress addr, TreeNode node)
        {
            //AppView appView = new AppView(node);
            //WeifenLuo.WinFormsUI.Docking.DockPanel panel = _mainForm.GetPanel();
            //appView.Show(panel);
        }

        private void DeleteNode(TreeNode node)
        {
            NodeInfo info = (NodeInfo)(node.Tag);
            try
            {
                File.Delete(info._xmlPath);
                node.Remove();
            }
            catch
            {
                MessageBox.Show("FuncItem:删除节点Xml文件失败!");
            }            
        }

        private void ModifyNody(TreeNode node)
        {

        }

        /*显示被选中的元件节点*/
        private void SelectLibNodeToShow(TreeNode node)
        {
            PropertyGrid pGrid;
            NodeInfo info = (NodeInfo)node.Tag;

            CloseExistForm(node);

            //1.显示一个view 
            ShowViewForm viewForm = new ShowViewForm(node);
            viewForm.Text = node.Text;
            WeifenLuo.WinFormsUI.Docking.DockPanel panel = MainForm.GetPanel();
            viewForm.Show(panel);

            //2.通过view来初始化propertyGrid
            pGrid =PropertyForm.GetInstance().GetGrid();
            pGrid.SelectedObject = viewForm.showViewPanel1.ShowView;
            //switch (info._formType)
            //{
            //    case PropertyInit.PropertyFormType.PPC:
                    
            //        break;
            //    case PropertyInit.PropertyFormType.FPAG:
            //        return;
            //        break;
            //    default:
            //        return;
            //        break;
            //}

            //3.配置树节点的NodeInfo
            info._form = viewForm;
            info._formFlag = true;
        }

        private void SelectSourceNodeToShow(TreeNode node)
        {
            NodeInfo info = (NodeInfo)node.Tag;
            String nodeName = node.Text;

            CloseExistForm(node);

            //1.显示一个view 
            ShowViewForm viewForm = new ShowViewForm(node);
            viewForm.Text = nodeName;
            WeifenLuo.WinFormsUI.Docking.DockPanel panel = MainForm.GetPanel();

            //2.配置树节点的NodeInfo
            info._form = viewForm;
            info._formFlag = true;

            viewForm.Show(panel);

        }

        /*判断对应窗口口是否已经存在:存在，关闭；不存在，跳过；*/
        private void CloseExistForm(TreeNode node)
        {
            if (NodeInfo.FormShowed(node))
            {
                NodeInfo info = (NodeInfo)node.Tag;
                ShowViewForm form = info._form;
                if(form != null)
                {
                    form.Close();
                }
            }
        }

    

    }
}
