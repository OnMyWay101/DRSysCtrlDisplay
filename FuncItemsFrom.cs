using System;
using System.Collections;
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
using System.Diagnostics;
using DynamicNode = DRSysCtrlDisplay.DynamicTopo.DynamicNode;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;
using DRSysCtrlDisplay.Models;

namespace DRSysCtrlDisplay
{
    public partial class FuncItemsForm : DockContent
    {
        private delegate void ThreadReconfigProc(TreeNode tNode);//其他线程使用该委托来更新UI控件
        private static FuncItemsForm uniqueInstance;
        public List<DynamicNode> AppMatchTopoNode { get; private set; }
        public List<TargetNode> TargetNodes { get; private set; }
        Timer _pTimer = new Timer();     //计时器用来更新过程
        long _timeTicks;               //花费的等待停止应用的时间,单位：一百纳秒

        private FuncItemsForm()
        {
            AppMatchTopoNode = null;
            TargetNodes = new List<TargetNode>();
            InitializeComponent();
            //AddTargetNode(null);//添加示例节点

            //TreeView相关的事件
            this._srTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SrTreeView_AfterSelect);
            this._eqTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.EqTreeView_AfterSelect);
            this._cpTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CpTreeView_AfterSelect);
            this._eqTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this._eqTreeView_MouseClick);
            this._srTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this._srTreeView_MouseClick);
            this._cpTreeView.MouseClick += new MouseEventHandler(_cpTreeView_MouseClick);
            this.MouseClick += new MouseEventHandler(FuncItemsForm_MouseClick);

            //上下文菜单选项触发的事件
            this._addCMSAddItem.Click += new System.EventHandler(this.ContextMSAdd_Click);
            this._editCMSDeleteItem.Click += new System.EventHandler(this.ContextMSDelete_Click);
            this._editCMSModifyItem.Click += new System.EventHandler(this.ContextMSModify_Click);
            this._srcCMS_AddInfo.Click += new EventHandler(_srcCMS_AddInfo_Click);
            MainForm.GetInstance().AddSource += new Action<TreeNode>(AddSource);
            MainForm.GetInstance().ClearSource += new Action<TreeNode>(ClearSource);


            this.Load += new System.EventHandler(this.FuncItems_Load);
            this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
            TcpManager.Instance.TargetStatusChange += new Action<TcpManager.TargetTcpClient>(OnTargetStatusChange);
            TargetConnectForm.Instance.TargetInfoComed += new Action<Dictionary<IPAddress, TargetListener.MultiCastPacket>>(OnTargetInfoComed);
            this._pTimer.Tick += new EventHandler(On_pTimer_Tick);
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
            TreeNode selectedNode = ((TreeView)sender).SelectedNode;
            try
            {
                if (selectedNode.Level == 1)
                {//当选中的节点为IP下面的属性节点的时候

                        switch (selectedNode.Index)
                        {
                            case 0:
                                ((NodeInfo)selectedNode.Tag)._formType = Princeple.FormType.TOPO;
                                break;
                            case 1:
                                ((NodeInfo)selectedNode.Tag)._formType = Princeple.FormType.APP;
                                break;
                            case 2:
                                ((NodeInfo)selectedNode.Tag)._formType = Princeple.FormType.STATUS;
                                break;
                            default:
                                return;
                        }
                }
                else if (selectedNode.Level == 2)
                {
                    ((NodeInfo)selectedNode.Tag)._formType = Princeple.FormType.COMPONENT;
                }
                else
                {
                    return;
                }

                SelectLibNodeToShow(selectedNode);
            }
            catch (System.Exception ep)
            {
                MessageBox.Show(ep.StackTrace);
            }
        }

        /*设备库节点响应*/
        private void EqTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = ((TreeView)sender).SelectedNode;

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
            TreeNode selectedNode = ((TreeView)sender).SelectedNode;
            if (selectedNode.Level == 1)
            {
                SelectLibNodeToShow(selectedNode);
            }
        }

        //弹出右键菜单
        void FuncItemsForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _addCMSAddItem.Enabled = false;
                _addCMS.Show(this, e.Location);
            }
        }

        /*弹出节点右键菜单*/
        private void _eqTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //1.设置当前右键节点为TreeView的selectedNode
                TreeView tree = (TreeView)sender;
                TreeNode clickNode = tree.GetNodeAt(e.Location);
                tree.SelectedNode = clickNode;

                if (tree.SelectedNode.Level == 0)
                {
                    if (tree.SelectedNode.Index != 0)
                    {//板卡库、背板库、机箱库节点才弹出添加菜单
                        _addCMSAddItem.Enabled = true;
                        _addCMS.Show(this, e.Location);
                    }
                }
                else if (tree.SelectedNode.Level == 1)
                {
                    if (tree.SelectedNode.Parent.Index == 0)
                    {//PPC、FPGA、ZYNQ节点才弹出添加菜单
                        _addCMSAddItem.Enabled = true;
                        _addCMS.Show(this, e.Location);
                    }
                    else
                    {//板卡库、背板库、机箱库节点下的成员节点才弹出编辑菜单
                        _editCMS.Show(this, e.Location);
                    }
                }
                else if (tree.SelectedNode.Level == 2)
                {//PPC、FPGA、ZYNQ节点下的成员节点才弹出编辑菜单
                    _editCMS.Show(this, e.Location);
                }
            }
            //TODO:美化弹出效果
        }

        private void _cpTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //1.设置当前右键节点为TreeView的selectedNode
                TreeView tree = (TreeView)sender;
                TreeNode clickNode = tree.GetNodeAt(e.Location);
                tree.SelectedNode = clickNode;

                if (clickNode.Level == 0)
                {
                    //构件库、应用库才弹出添加菜单
                    _addCMSAddItem.Enabled = true;
                    _addCMS.Show(this, e.Location);
                }
                else
                {
                    _editCMS.Show(this, e.Location);

                }
            }
        }

        //资源池界面点击右键显示的上下文菜单
        private void _srTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //1.设置当前右键节点为TreeView的selectedNode
                TreeView tree = sender as TreeView;
                TreeNode clickNode = tree.GetNodeAt(e.Location);
                tree.SelectedNode = clickNode;

                try
                {
                    //恢复所有选项
                    _srcCMS_AddInfo.Enabled = true;
                    _srcCMS_ClearInfo.Enabled = true;
                    _srcCMS_MatchApp.Enabled = true;
                    _srcCMS_Upload.Enabled = true;
                    _srcCMS_Recrt.Enabled = true;

                    if (clickNode.Level == 0)//目标节点
                    {
                        _srcCMS_MatchApp.Enabled = false;
                        _srcCMS_Upload.Enabled = false;
                        _srcCMS_Recrt.Enabled = false;
                        //构件库、应用库才弹出添加菜单
                        _srcCMS.Show(this, e.Location);
                    }
                    else if (clickNode.Level == 1 && clickNode.Index == 1)//应用节点
                    {
                        _srcCMS_AddInfo.Enabled = false;
                        _srcCMS_ClearInfo.Enabled = false;
                        //构件库、应用库才弹出添加菜单
                        _srcCMS.Show(this, e.Location);
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        void _srcCMS_AddInfo_Click(object sender, EventArgs e)
        {
            AddSource(_srTreeView.SelectedNode);
        }

        private void SrcCMS_ClearInfo_Click(object sender, EventArgs e)
        {
            ClearSource(_srTreeView.SelectedNode);
        }

        private void _srcCMS_MatchApp_Click(object sender, EventArgs e)
        {
            var treeNode = _srTreeView.SelectedNode;
            TargetNode targetNode = TargetNodes[treeNode.Parent.Index];//treeNode对应的目标节点
            NodeInfo info = treeNode.Tag as NodeInfo;
            try
            {
                var topo = (DynamicTopo)(info._form.showViewPanel1.ShowView);
                Boolean autoReconfigFlag = targetNode.AutoReconfigFlag;
                if (topo.ChooseStrategy(ref autoReconfigFlag))
                {
                    topo.TriggerRedrawRequst();
                    AppMatchTopoNode = topo.GetMatchedNodes();
                    targetNode.AutoReconfigFlag = autoReconfigFlag;
                }
            }
            catch (NullReferenceException ne)
            {
                Console.WriteLine("_srcCMS_MatchApp_Click" + ne.Message);
                MessageBox.Show("请添加/显示节点对应构件");
            }
            catch (Exception ex)
            {
                MessageBox.Show("_srcCMS_MatchApp_Click:" + ex.Message);
            }
        }

        private void _srcCMS_Upload_Click(object sender, EventArgs e)
        {
            var targetNode = _srTreeView.SelectedNode;//选中的treeNode
            TargetNode tNode = TargetNodes[targetNode.Parent.Index];//treeNode对应的目标节点
            if (AppMatchTopoNode != null)
            {
                //显示加载执行文件的窗体
                var upLoadFileForm = new ExeFileForm(AppMatchTopoNode, tNode.ExeFile);
                upLoadFileForm.ShowDialog();
                if (upLoadFileForm.DialogResult == DialogResult.Yes)
                {
                    tNode.ExeFile = upLoadFileForm.GetFilesHt();
                }
                upLoadFileForm.Dispose();
            }
            else
            {
                MessageBox.Show("当前无匹配结果，不能上传文件");
            }
        }

        //点击“重构”的相关处理
        private void _srcCMS_Recrt_Click(object sender, EventArgs e)
        {
            var treeNode = _srTreeView.SelectedNode;
            TargetNode targetNode = TargetNodes[treeNode.Parent.Index];//treeNode对应的目标节点
            NodeInfo info = treeNode.Tag as NodeInfo;
            try
            {
                var topo = (DynamicTopo)(info._form.showViewPanel1.ShowView);
                Boolean autoReconfigFlag = targetNode.AutoReconfigFlag;
                if (topo.ChooseStrategy(ref autoReconfigFlag))
                {
                    topo.TriggerRedrawRequst();
                    AppMatchTopoNode = topo.GetMatchedNodes();
                    targetNode.AutoReconfigFlag = autoReconfigFlag;
                    ReconfigProcess(treeNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("_srcCMS_MatchApp_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// 添加设备或构件的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMSAdd_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0://资源池

                    break;
                case 1://设备库
                    EqTreeViewAdd();
                    break;
                default://构件库
                    CpTreeViewAdd();
                    break;
            }
        }

        /*组件删除*/
        private void ContextMSDelete_Click(object sender, EventArgs e)
        {
            TreeView selectedTree;  //被选中的树
            string filePath;        //该树对应的配置文件路径
            if (tabControl1.SelectedIndex == 1)
            {
                selectedTree = _eqTreeView;
                filePath = XMLManager.PathManager.GetEqLibFilePath();
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                selectedTree = _cpTreeView;
                filePath = XMLManager.PathManager.GetCmpLibFilePath();
            }
            else
            {
                return;
            }
            NodeInfo.DeleteTreeNode(selectedTree.SelectedNode);
            XMLManager.HandleTreeView.ReadTreeViewToXML(selectedTree, filePath);
        }

        /*组件修改*/
        private void ContextMSModify_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = _eqTreeView.SelectedNode;
            string oldNodeName = selectedNode.Text;

            switch (selectedNode.Parent.Text)
            {
                case "PPCs":
                    //通过读取XML文件初始化PPC,然后用一个PPC实例初始化PPCInitForm
                    PPC ppc = ModelFactory<PPC>.CreateByName(selectedNode.Text);
                    PPCInitForm ppcInitForm = new PPCInitForm(ppc);
                    ppcInitForm.ShowDialog();

                    if (ppcInitForm.DialogResult == DialogResult.Yes)
                    {
                        if (ppcInitForm.GetObjectName() != oldNodeName)
                        {//改了名字的话，要删除老文件
                            selectedNode.Text = ppcInitForm.GetObjectName();
                            var filePath = string.Format(@"{0}\{1}.xml", XMLManager.PathManager.GetPPCPath(), oldNodeName);
                            File.Delete(filePath);
                        }
                    }
                    ppcInitForm.Dispose();
                    break;
                case "FPGAs":
                    FPGA fpga = ModelFactory<FPGA>.CreateByName(selectedNode.Text);
                    FPGAInitForm fpgaInitForm = new FPGAInitForm(fpga);
                    fpgaInitForm.ShowDialog();

                    if (fpgaInitForm.DialogResult == DialogResult.Yes)
                    {
                        if (fpgaInitForm.GetObjectName() != oldNodeName)
                        {
                            selectedNode.Text = fpgaInitForm.GetObjectName();
                            var filePath = string.Format(@"{0}\{1}.xml", XMLManager.PathManager.GetFPGAPath(), oldNodeName);
                            File.Delete(filePath);
                        }
                    }
                    fpgaInitForm.Dispose();
                    break;
                case "ZYNQs":
                    ZYNQ zynq = ModelFactory<ZYNQ>.CreateByName(selectedNode.Text);
                    ZYNQInitForm zynqInitForm = new ZYNQInitForm(zynq);
                    zynqInitForm.ShowDialog();

                    if (zynqInitForm.DialogResult == DialogResult.Yes)
                    {
                        if (zynqInitForm.GetObjectName() != oldNodeName)
                        {
                            selectedNode.Text = zynqInitForm.GetObjectName();
                            var filePath = string.Format(@"{0}\{1}.xml", XMLManager.PathManager.GetZYNQPath(), oldNodeName);
                            File.Delete(filePath);
                        }
                    }
                    zynqInitForm.Dispose();
                    break;
                case "板卡库":
                    Board board = ModelFactory<Board>.CreateByName(selectedNode.Text);
                    BoardInitForm boardInitForm = new BoardInitForm(board);
                    boardInitForm.ShowDialog();

                    if (boardInitForm.DialogResult == DialogResult.Yes)
                    {
                        if (boardInitForm.GetObjectName() != oldNodeName)
                        {
                            selectedNode.Text = boardInitForm.GetObjectName();
                            var filePath = string.Format(@"{0}\{1}.xml", XMLManager.PathManager.GetBoardPath(), oldNodeName);
                            File.Delete(filePath);
                        }
                    }
                    boardInitForm.Dispose();
                    break;
                case "背板库":
                    BackPlane bp = ModelFactory<BackPlane>.CreateByName(selectedNode.Text);
                    BackPlaneInitForm bpInitForm = new BackPlaneInitForm(bp);
                    bpInitForm.ShowDialog();

                    if (bpInitForm.DialogResult == DialogResult.Yes)
                    {
                        if (bpInitForm.GetObjectName() != oldNodeName)
                        {
                            selectedNode.Text = bpInitForm.GetObjectName();
                            var filePath = string.Format(@"{0}\{1}.xml", XMLManager.PathManager.GetBackPlanePath(), oldNodeName);
                            File.Delete(filePath);
                        }
                    }
                    bpInitForm.Dispose();
                    break;
                case "机箱库":
                    Models.Container ctnView = ModelFactory<Models.Container>.CreateByName(selectedNode.Text); ;
                    ContainerInitForm ctnInitForm = new ContainerInitForm(ctnView);
                    ctnInitForm.ShowDialog();

                    if (ctnInitForm.DialogResult == DialogResult.Yes)
                    {
                        if (ctnInitForm.GetObjectName() != oldNodeName)
                        {
                            selectedNode.Text = ctnInitForm.GetObjectName();
                            var filePath = string.Format(@"{0}\{1}.xml", XMLManager.PathManager.GetContainerPath(), oldNodeName);
                            File.Delete(filePath);
                        }
                    }
                    ctnInitForm.Dispose();
                    break;
                default:
                    break;
            }
            //无论有无文件内容更改都把TreeView的内容读入对应xml文件内
            XMLManager.HandleTreeView.ReadTreeViewToXML(_eqTreeView, XMLManager.PathManager.GetEqLibFilePath());
        }

        //窗体加载事件处理器
        private void FuncItems_Load(object sender, EventArgs e)
        {
            //1.初始化所有TreeView的NodeInfo
            RefreshTreeViews();

            //2.主界面菜单选项打勾
            MainForm mainForm = MainForm.GetInstance();
            mainForm.FuncToolStripMenuItem.Checked = true;
        }

        //关闭窗口的事件处理函数改写
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm mainForm = MainForm.GetInstance();
            mainForm.FuncToolStripMenuItem.Checked = false;
            this.Hide();
            e.Cancel = true;
        }

        //目标机状态改变的事件处理（主要是Tcp相关的：连接新目标）
        private void OnTargetStatusChange(TcpManager.TargetTcpClient targetClient)
        {
            AddTargetNode(targetClient);
        }

        //选中的目标机的广播包信息的事件处理函数
        private void OnTargetInfoComed(Dictionary<IPAddress, TargetListener.MultiCastPacket> infosDir)
        {
            //更新TargetNode的节点信息
            foreach (var tNode in TargetNodes)
            {
                var client = tNode.TcpClt;
                if (client == null) //示例节点的client为null
                {
                    continue;
                }
                if (infosDir.ContainsKey(client.IpAddr))
                {
                    tNode.MultiPacket = infosDir[client.IpAddr];
                    var index = TargetNodes.IndexOf(tNode);     //TreeNode的序号即为目标节点的序号
                    ChildrenNodeInfoChange(index, tNode);
                }
            }
        }

        private void On_pTimer_Tick(object sender, EventArgs e)
        {
            var targetNode = _srTreeView.SelectedNode;//选中的treeNode
            TargetNode tNode = TargetNodes[targetNode.Parent.Index];//treeNode对应的目标节点
            var ip = targetNode.Parent.Text;

            //统算时间,判断是否超时
            var nowTicks = DateTime.Now.Ticks;
            TimeSpan ts = new TimeSpan(nowTicks - _timeTicks);
            if (ts.TotalMilliseconds > 20000)//500ms之内需应用停止成功
            {
                _pTimer.Stop();
                MainForm.SetOutPutText("应用停止失败!用时超过500ms");
                return;
            }
            MainForm.SetOutPutText(string.Format("OnTick!时间{0}ms", ts.TotalMilliseconds));
            //获取回复命令
            var recvTcpCmd = TcpManager.Instance.RecvOneCmd(ip, CmdCode.StopApp);
            if (recvTcpCmd != null)
            {
                _pTimer.Stop();
                MainForm.SetOutPutText(string.Format("应用停止成功!时间{0}ms", ts.TotalMilliseconds));

                var upLoadFileForm = new ExeFileForm(AppMatchTopoNode, tNode.ExeFile);
                if (tNode.AutoReconfigFlag)//自动匹配
                {
                    upLoadFileForm.DeployFiles();
                }
                else
                {
                    //进入部署界面
                    upLoadFileForm.ShowDialog();
                    if (upLoadFileForm.DialogResult == DialogResult.Yes)
                    {
                        tNode.ExeFile = upLoadFileForm.GetFilesHt();
                    }
                }
                upLoadFileForm.Dispose();
            }
        }

        #endregion 事件处理函数

        //_srcTreeView的目标节点下面的信息有改变
        private void ChildrenNodeInfoChange(int index, TargetNode tNode)
        {
            var nodes = _srTreeView.Nodes[index].Nodes;
            foreach (TreeNode node in nodes)
            {
                try
                {
                    NodeInfo info = node.Tag as NodeInfo;
                    if (info._formFlag) //  有对应窗体显示,才触发该事件
                    {
                        info.TriggerNodeInfoChanged(tNode);
                        //若当前视图为应用视图那么记录匹配的节点
                        if (info._formType == Princeple.FormType.APP)
                        {
                            var topo = (DynamicTopo)(info._form.showViewPanel1.ShowView);
                            AppMatchTopoNode = topo.GetMatchedNodes();
                            if (topo._reconfigFlag && AppMatchTopoNode != null)
                            {
                                ReconfigProcess(node);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MainForm.SetOutPutText(e.Message + "\n" + e.StackTrace);
                }
            }
        }

        /*显示被选中的元件节点*/
        private void SelectLibNodeToShow(TreeNode node)
        {
            ShowViewForm viewForm = null;
            NodeInfo info = (NodeInfo)node.Tag;
            CloseExistForm(node);

            try
            {
                if (info._formType == Princeple.FormType.APP || info._formType == Princeple.FormType.STATUS)
                {
                    TargetNode target = GetTargetNode(node);
                    ShowViewForm destForm = (info._formType == Princeple.FormType.APP ? target.DynamicTopoForm : target.StatusForm);
                    if (destForm != null)
                    {
                        viewForm = destForm;
                    }
                    else
                    {
                        viewForm = new ShowViewForm(node);
                        if (info._formType == Princeple.FormType.APP)
                        {
                            target.DynamicTopoForm = viewForm;
                        }
                        else
                        {
                            target.StatusForm = viewForm;
                        }
                    }
                }
                else
                {
                    viewForm = new ShowViewForm(node);
                }

                //1.显示窗体
                viewForm.Text = node.Text;
                WeifenLuo.WinFormsUI.Docking.DockPanel panel = MainForm.GetPanel();
                viewForm.Show(panel);

                //2.通过view来初始化propertyGrid
                PropertyForm.Show(viewForm.showViewPanel1.ShowView.GetModelInstance());

                //3.配置树节点的NodeInfo
                info._form = viewForm;
                info._formFlag = true;
            }
            catch (Exception e)
            {
                MainForm.SetOutPutText(e.StackTrace);
            }
        }

        //显示目标机对应的窗体
        private TargetNode GetTargetNode(TreeNode node)
        {
            NodeInfo info = (NodeInfo)node.Tag;
            TreeNode rootNode = node;
            //循环访问到根节点
            while (rootNode.Parent != null)
            {
                rootNode = rootNode.Parent;
            }
            return TargetNodes[rootNode.Index];
        }

        private void ReconfigProcessInner(TreeNode tNode)
        {
            _srTreeView.SelectedNode = tNode;
            //发送停止命令
            var ip = tNode.Parent.Text;
            TcpManager.Instance.SendOneCmd(ip, new TcpCommand_StopApp());
            //等待回复
            MainForm.SetOutPutText("应用停止中...");
            _pTimer.Interval = 5;//5ms一次的检查
            _timeTicks = DateTime.Now.Ticks;
            _pTimer.Start();
        }

        private void ReconfigProcess(TreeNode tNode)
        {
            if (this.InvokeRequired)
            {
                var threadWork = new ThreadReconfigProc(ReconfigProcessInner);
                this.Invoke(threadWork, tNode);
            }
            else
            {
                ReconfigProcessInner(tNode);
            }
        }

        /// <summary>
        /// 选择资源节点去显示
        /// </summary>
        /// <param name="node"></param>
        private void SelectSourceNodeToShow(TreeNode node)
        {
            NodeInfo info = (NodeInfo)node.Tag;
            String nodeName = node.Text;

            CloseExistForm(node);

            //显示一个view 
            ShowViewForm viewForm = new ShowViewForm(node);
            viewForm.Text = nodeName;
            WeifenLuo.WinFormsUI.Docking.DockPanel panel = MainForm.GetPanel();
            viewForm.Show(panel);
        }

        /*判断对应窗口口是否已经存在:存在，关闭；不存在，跳过；*/
        private void CloseExistForm(TreeNode node)
        {
            if (NodeInfo.FormShowed(node))
            {
                NodeInfo info = (NodeInfo)node.Tag;
                ShowViewForm form = info._form;
                if (form != null)
                {
                    if (info._formType == Princeple.FormType.APP ||
                        info._formType == Princeple.FormType.STATUS)
                    {
                        form.Hide();
                    }
                    else
                    {
                        form.Close();
                    }
                }
            }
        }

        //添加一个目标节点
        private void AddTargetNode(TcpManager.TargetTcpClient targetClient)
        {
            TargetNode tNode = new TargetNode(targetClient);
            TargetNodes.Add(tNode);
            if (targetClient == null)
            {
                AddSrcTreeNode("示例:X.X.X.X");
            }
            else
            {
                AddSrcTreeNode(targetClient.IpAddr.ToString());
            }
        }

        private void AddSrcTreeNode(string nodeName)
        {
            TreeView tv = this._srTreeView;
            TreeNode newNode = new TreeNode(nodeName);
            tv.Nodes.Add(newNode);

            NodeInfo.AddChildrenNode(newNode, "资源:", Princeple.FormType.TOPO);
            NodeInfo.AddChildrenNode(newNode, "应用集合", Princeple.FormType.APP);
            NodeInfo.AddChildrenNode(newNode, "机箱状态", Princeple.FormType.STATUS);
        }

        //重新更新TreeView的内容
        private void RefreshTreeViews()
        {
            NodeInfo.InitTreeNodeInfo(_srTreeView);

            XMLManager.HandleTreeView.SynchronizeTreeViewXML();

            _eqTreeView.Nodes.Clear();
            _cpTreeView.Nodes.Clear();
            String eqFilePath = XMLManager.PathManager.GetEqLibFilePath();
            String cpFilePath = XMLManager.PathManager.GetCmpLibFilePath();

            XMLManager.HandleTreeView.ReadXMLToTreeView(_eqTreeView, eqFilePath);
            XMLManager.HandleTreeView.ReadXMLToTreeView(_cpTreeView, cpFilePath);
        }


        private void EqTreeViewAdd()
        {
            TreeNode selectedNode = _eqTreeView.SelectedNode;

            switch (selectedNode.Text)
            {
                //显示属性初始化窗口
                case "PPCs":
                    PPCInitForm ppcInitForm = new PPCInitForm();
                    ppcInitForm.ShowDialog();
                    if (ppcInitForm.DialogResult == DialogResult.Yes)
                    {
                        NodeInfo.AddChildrenNode(selectedNode, ppcInitForm.GetObjectName(), Princeple.FormType.PPC);
                    }
                    ppcInitForm.Dispose();
                    break;
                case "FPGAs":
                    FPGAInitForm fpgaInitForm = new FPGAInitForm();
                    fpgaInitForm.ShowDialog();
                    if (fpgaInitForm.DialogResult == DialogResult.Yes)
                    {
                        NodeInfo.AddChildrenNode(selectedNode, fpgaInitForm.GetObjectName(), Princeple.FormType.FPGA);
                    }
                    fpgaInitForm.Dispose();
                    break;
                case "ZYNQs":
                    ZYNQInitForm zynqInitForm = new ZYNQInitForm();
                    zynqInitForm.ShowDialog();
                    if (zynqInitForm.DialogResult == DialogResult.Yes)
                    {
                        NodeInfo.AddChildrenNode(selectedNode, zynqInitForm.GetObjectName(), Princeple.FormType.ZYNQ);
                    }
                    zynqInitForm.Dispose();
                    break;
                case "板卡库":
                    BoardInitForm boardInitForm = new BoardInitForm();
                    boardInitForm.ShowDialog();

                    if (boardInitForm.DialogResult == DialogResult.Yes)
                    {
                        NodeInfo.AddChildrenNode(selectedNode, boardInitForm.BoardNodeName, Princeple.FormType.BOARD);
                    }
                    boardInitForm.Dispose();
                    break;
                case "背板库":
                    BackPlaneInitForm bpInitForm = new BackPlaneInitForm();
                    bpInitForm.ShowDialog();
                    if (bpInitForm.DialogResult == DialogResult.Yes)
                    {
                        NodeInfo.AddChildrenNode(selectedNode, bpInitForm.GetObjectName(), Princeple.FormType.BACKPLANE);
                    }
                    bpInitForm.Dispose();
                    break;
                case "机箱库":
                    ContainerInitForm ctnInitForm = new ContainerInitForm();
                    ctnInitForm.ShowDialog();
                    if (ctnInitForm.DialogResult == DialogResult.Yes)
                    {
                        NodeInfo.AddChildrenNode(selectedNode, ctnInitForm.GetObjectName(), Princeple.FormType.CONTIANER);
                        ctnInitForm.Dispose();
                    }
                    break;
                case "系统库":
                    SystemStruInitForm sysInitForm = new SystemStruInitForm();
                    sysInitForm.ShowDialog();
                    if (sysInitForm.DialogResult == DialogResult.Yes)
                    {
                        NodeInfo.AddChildrenNode(selectedNode, sysInitForm.GetObjectName(), Princeple.FormType.SYSTEM);
                        sysInitForm.Dispose();
                    }
                    break;
                default:
                    break;
            }
            //无论有无文件内容更改都把TreeView的内容读入对应xml文件内
            XMLManager.HandleTreeView.ReadTreeViewToXML(_eqTreeView, XMLManager.PathManager.GetEqLibFilePath());
        }

        private void CpTreeViewAdd()
        {
            TreeNode selectedNode = _cpTreeView.SelectedNode;

            switch (selectedNode.Text)
            {
                //显示属性初始化窗口
                case PathManager.CompsDir:
                    var initForm = new ComponentInitForm();
                    initForm.ShowDialog();
                    if (initForm.DialogResult == DialogResult.Yes)
                    {
                        NodeInfo.AddChildrenNode(selectedNode, initForm.GetObjectName(), Princeple.FormType.COMPONENT);
                        initForm.Dispose();
                    }
                    break;
                case PathManager.AppsDir:
                    break;
                default:
                    break;
            }
            //无论有无文件内容更改都把TreeView的内容读入对应xml文件内
            XMLManager.HandleTreeView.ReadTreeViewToXML(_cpTreeView, XMLManager.PathManager.GetCmpLibFilePath());
        }

        private void AddSource(TreeNode node)
        {
            //找到指定的node
            if (node == null)
            {
                if (_srTreeView.Nodes.Count == 0)
                {
                    MessageBox.Show("没有指定的目标机！");
                    return;
                }
                node = _srTreeView.Nodes[0];
            }

            var initForm = new NodeInfoAddForm();
            initForm.StartPosition = FormStartPosition.CenterParent;
            initForm.ShowDialog();
            if (initForm.DialogResult == DialogResult.Yes)
            {
                //先清除信息，再设置信息
                ClearSource(node);
                var nodeCntName = node.Nodes[0].Text;   //机箱的名字
                var cntFrontName = nodeCntName.Substring(0, nodeCntName.IndexOf(':') + 1);
                node.Nodes[0].Text = string.Format("{0}{1}", cntFrontName, initForm.GetContainerName());

                //逐个添加选中的构件
                foreach (var cmpName in initForm.GetComponentNames())
                {
                    NodeInfo.AddChildrenNode(node.Nodes[1], cmpName, Princeple.FormType.TOPO);
                }
            }
            initForm.Dispose();
        }

        private void ClearSource(TreeNode node)
        {
            if (node == null)
            {
                if (_srTreeView.Nodes.Count == 0)
                {
                    MessageBox.Show("没有指定的目标机！");
                    return;
                }
                node = _srTreeView.Nodes[0];
            }
            var nodeSysName = node.Nodes[0].Text;   //系统的名字
            var cntFrontName = nodeSysName.Substring(0, nodeSysName.IndexOf(':') + 1);
            //清除系统名,应用名
            node.Nodes[0].Text = cntFrontName;
            node.Nodes[1].Nodes.Clear();

            //清除对应目标机的相关信息
            TargetNode target = GetTargetNode(node);
            target.StatusForm = null;
            target.DynamicTopoForm = null;
        }

        //获取相关类型节点的名称集合
        public string[] GetEqSetNames(Princeple.FormType type)
        {
            List<string> nameInfos = new List<string>();
            TreeNode rootNode = null;       //相关类型的根节点
            switch (type)
            {
                case Princeple.FormType.PPC:
                    rootNode = _eqTreeView.Nodes[0].Nodes[0];   //_eqTreeView的节点0的节点0就是ppc库的根节点
                    break;
                case Princeple.FormType.FPGA:
                    rootNode = _eqTreeView.Nodes[0].Nodes[1];   //_eqTreeView的节点0的节点1就是fpga库的根节点
                    break;
                case Princeple.FormType.ZYNQ:
                    rootNode = _eqTreeView.Nodes[0].Nodes[2];   //_eqTreeView的节点0的节点2就是zynq库的根节点
                    break;
                case Princeple.FormType.BOARD:
                    rootNode = _eqTreeView.Nodes[1];            //_eqTreeView的节点1就是板卡库的根节点
                    break;
                case Princeple.FormType.BACKPLANE:
                    rootNode = _eqTreeView.Nodes[2];            //_eqTreeView的节点2就是背板库的根节点
                    break;
                case Princeple.FormType.CONTIANER:
                    rootNode = _eqTreeView.Nodes[3];            //_eqTreeView的节点3就是机箱库的根节点
                    break;
                case Princeple.FormType.SYSTEM:
                    rootNode = _eqTreeView.Nodes[4];            //_eqTreeView的节点4就是系统库的根节点
                    break;
                case Princeple.FormType.COMPONENT:
                    rootNode = _cpTreeView.Nodes[0];            //_cpTreeView的节点0就是构件库的根节点
                    break;
                default:
                    MessageBox.Show("GetInfos:未指定正确的节点类型");
                    return null;
            }
            //遍历目标根节点的子节点获取节点名
            nameInfos.Add("无");
            foreach (TreeNode node in rootNode.Nodes)
            {
                nameInfos.Add(node.Text);
            }
            return nameInfos.ToArray();
        }
    }

    //描述一个连接的目标节点
    public class TargetNode
    {
        public ShowViewForm StatusForm { get; set; }                               //状态对应的窗体
        public ShowViewForm DynamicTopoForm { get; set; }                          //动态topo对应的窗体
        public TargetListener.MultiCastPacket MultiPacket { get; set; }     //目标节点对应的组播包
        public TcpManager.TargetTcpClient TcpClt { get; private set; }      //目标节点对应的客户端
        public Hashtable ExeFile { get; set; }                              //节点对应的文件
        public Boolean AutoReconfigFlag { get; set; }                       //自动重构的标志

        public TargetNode(TcpManager.TargetTcpClient tcpClient)
        {
            TcpClt = tcpClient;
            ExeFile = null;
        }
    }
}
