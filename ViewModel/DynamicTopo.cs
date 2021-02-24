using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.ViewModel.Others;
using DRSysCtrlDisplay.Models;
using SystemInformation = DRSysCtrlDisplay.TargetListener.MultiCastPacket.MultiCastPacketInfo.SystemInformation;
using StaticNode = DRSysCtrlDisplay.StaticTopo.StaticNode;
using StaticLine = DRSysCtrlDisplay.StaticTopo.StaticLine;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// 画一个系统的动态topo图，包含计算颗粒(PPC,ZYNQ,FPGA)和连接关系(EtherNet,RapidIO,GTX,LVDS )
    /// </summary>
    public class DynamicTopo : BaseDrawer, IDrawerChoosed, IDrawerNotify
    {
        private StaticTopo _sTopo = null;                                       //该动态Topo图对应的静态topo图
        public TopoNet<DynamicNode, DynamicLine> _topoNet { get; private set; } //计算颗粒包含的topo图
        private TopoNetView<DynamicNode, DynamicLine> _topoView;                //Topo图的画图类
        public BaseDrawer ChoosedBv { get; set; }
        public Models.Component[] Components { get; set; }                      //该topo对应的应用集
        List<List<DynamicNode>> _appMatchedTopoList;                            //应用匹配的节点集合topo的集合
        int _choosedMatchedListNum = -1;                                        //选择的匹配节点集合topo的序号

        List<Boolean>[] _onLineFlags = null;                                    //各机箱槽位板卡在线信息
        public Boolean _reconfigFlag { get; set; }                              //该应用是否发生了重构

        public DynamicTopo(StaticTopo sTopo, Rectangle rect)
        {
            _sTopo = sTopo;
            Init(rect);
        }

        public DynamicTopo(StaticTopo sTopo)
        {
            _sTopo = sTopo;
        }

        public override void Init(Rectangle rect)
        {
            base.Init(rect);
            _sTopo.Init(rect);

            InitTopoNet();
            _appMatchedTopoList = new List<List<DynamicNode>>();
            MatchApps();

            _topoView = new TopoNetView<DynamicNode, DynamicLine>(base._rect, _topoNet);
            InitOnlineFlags();
        }

        /// <summary>
        /// 初始化成员变量_topoNet（TopoNet<DynamicNode, DynamicLine>）
        /// </summary>
        private void InitTopoNet()
        {
            _topoNet = new TopoNet<DynamicNode, DynamicLine>(_sTopo.ComputeNodeNum);
            //初始化节点
            foreach (StaticNode sNode in _sTopo._topoNet.NodeArray)
            {
                DynamicNode dNode = new DynamicNode(sNode);
                //默认除了机箱0都设置为掉线
                if (sNode.FrameId == 0)
                    dNode.Status = NodeStatus.OnLine;
                _topoNet.SetNodeValue(sNode.UrlId, dNode);
            }
            //连接初始化
            InitTopoLink(_sTopo._topoNet.EthLinks);
            InitTopoLink(_sTopo._topoNet.RioLinks);
            InitTopoLink(_sTopo._topoNet.GTXLinks.Cast<StaticLine>());
            InitTopoLink(_sTopo._topoNet.LVDSLinks.Cast<StaticLine>());
        }

        private void InitTopoLink(IEnumerable<StaticLine> sLines)
        {
            foreach (StaticLine sLine in sLines)
            {
                if (sLine != null)
                {
                    DynamicLine dLine = new DynamicLine(sLine);
                    _topoNet.SetLinkValue(dLine);
                }
            }
        }

        //初始化槽位的在线状态，目前只默认存在机箱0在线，其余板卡不在线
        private void InitOnlineFlags()
        {
            _onLineFlags = new List<Boolean>[_sTopo.System.CntNames.Length];
            for (int i = 0; i < _onLineFlags.Length; i++)
            {
                var ctn = ModelFactory<Models.Container>.CreateByName(_sTopo.System.CntNames[i]);
                var bp = ModelFactory<Models.BackPlane>.CreateByName(ctn.BackPlaneName);
                _onLineFlags[i] = new List<bool>();     //该机箱板卡对应在线情况
                for (int j = 0; j < bp.SlotsNum; j++)
                {
                    if (i == 0)//判断是否为机箱0
                    {
                        _onLineFlags[i].Add(true);
                    }
                    else
                    {
                        _onLineFlags[i].Add(false);
                    }
                }
            }
        }

        //获取该应用匹配的结果
        public List<DynamicNode> GetMatchedNodes()
        {
            if (_appMatchedTopoList.Count > 0 && _choosedMatchedListNum >= 0)
            {
                return _appMatchedTopoList[_choosedMatchedListNum];
            }
            return null;
        }

        public void OnNodeInfoChanged(TargetNode tNode)
        {
            Boolean nodeDropFlag = false;   //是否有点接掉线
            _reconfigFlag = false;

            //处理信息，信息只表示是一个机箱
            var cnt0Flags = _onLineFlags[0];
            for (int i = 0; i < cnt0Flags.Count; i++)
            {
                Boolean newFlag = IsBoardOnLine(i, tNode.MultiPacket.SysInfo);
                if (!newFlag && cnt0Flags[i])    //判断有无新节点掉线
                {
                    MainForm.SetOutPutText(string.Format("槽位{0}板卡节点掉线", i + 1));
                    nodeDropFlag = true;
                }
                cnt0Flags[i] = newFlag;//更新各槽位的在线情况
                var dNodes = _topoNet.NodeArray.Where(dNode => dNode.SNode.FrameId == 0 && dNode.SNode.SlotId == i);
                foreach (var dNode in dNodes)
                {
                    if (newFlag)
                    {
                        if (dNode.Status == NodeStatus.OffLine)//新上线
                        {
                            dNode.Status = NodeStatus.OnLine;
                        }
                    }
                    else
                    {
                        dNode.Status = NodeStatus.OffLine;
                    }
                }
            }

            if (nodeDropFlag)
            {
                Boolean autoReconfigFlag = tNode.AutoReconfigFlag;
                ProcessNodeDrop(ref autoReconfigFlag);
                tNode.AutoReconfigFlag = autoReconfigFlag;
                base.TriggerRedrawRequst();
            }
        }

        private bool IsBoardOnLine(int slotSn, SystemInformation sysInfo)
        {
            try
            {
                var slotInfo = sysInfo._boardsInfo[slotSn];
                return slotInfo._isOnline == 1;
            }
            catch (IndexOutOfRangeException e)
            {
                MainForm.SetOutPutText(e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        //处理节点掉线的任务
        private void ProcessNodeDrop(ref Boolean autoReconfigFlag)
        {
            //有匹配的结果才进行处理
            if (_appMatchedTopoList.Count > 0 && _choosedMatchedListNum >= 0)
            {
                bool needReconfigFlag = false;  //是否需要进行重构的标志

                var chooseList = _appMatchedTopoList[_choosedMatchedListNum];
                foreach (var node in chooseList)
                {
                    if (!_onLineFlags[node.SNode.FrameId][node.SNode.SlotId])
                    {
                        needReconfigFlag = true;
                        break;
                    }
                }
                if (needReconfigFlag)
                {
                    if (autoReconfigFlag)//判断是否为自动重构
                    {
                        MainForm.SetOutPutText("进行自动匹配。。。");
                        ReconfigForm recnfForm = new ReconfigForm(this, _appMatchedTopoList, _onLineFlags);
                        _choosedMatchedListNum = recnfForm.FindFirstValidStrategy();
                        if (_choosedMatchedListNum < 0)
                        {
                            _reconfigFlag = false;
                            MessageBox.Show("无有效匹配方案!");
                            MainForm.SetOutPutText("无有效匹配方案!");
                        }
                        else
                        {
                            SetMatchedTopo(_choosedMatchedListNum);
                            _reconfigFlag = true;
                            MainForm.SetOutPutText("自动匹配成功！选择方案" + (_choosedMatchedListNum + 1));
                        }
                    }
                    else
                    {
                        _reconfigFlag = ChooseStrategy(ref autoReconfigFlag);
                    }
                }
            }
        }

        /// <summary>
        /// 策略选择
        /// </summary>
        /// <returns>是否为有效选择</returns>
        public bool ChooseStrategy(ref Boolean autoReconfigFlag)
        {
            bool flag = false;
            //显示重构窗体
            ReconfigForm recnfForm = new ReconfigForm(this, _appMatchedTopoList, _onLineFlags);
            recnfForm.AutoReconfigFlag = autoReconfigFlag;
            recnfForm.ShowDialog();
            if (recnfForm.DialogResult == DialogResult.Yes)
            {
                _choosedMatchedListNum = recnfForm.ChoosedIndex;
                if (_choosedMatchedListNum >= 0)//有效的选择项
                {
                    SetMatchedTopo(_choosedMatchedListNum);
                    autoReconfigFlag = recnfForm.AutoReconfigFlag;
                    flag = true;
                }
            }
            recnfForm.Dispose();
            return flag;
        }

        //设置该应用匹配的结果使用的topo序号
        public void SetMatchedTopo(int index)
        {
            _choosedMatchedListNum = index;
            try
            {
                if (_appMatchedTopoList.Count > index && _choosedMatchedListNum >= 0)
                {
                    //先把使用了的节点重至为Online
                    foreach (var node in _topoNet.NodeArray)
                    {
                        if (node.Status == NodeStatus.Used)
                        {
                            node.CNode = null;
                            node.ComName = string.Empty;
                            node.Status = NodeStatus.OnLine;
                        }
                    }
                    //再把匹配上的节点至为Used
                    for (int i = 0; i < _appMatchedTopoList[_choosedMatchedListNum].Count; i++)
                    {
                        var node = _appMatchedTopoList[_choosedMatchedListNum][i];
                        node.CNode = GetCmpNode(i);
                        node.ComName = GetCmpName(node.CNode);
                        node.Status = NodeStatus.Used;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SetMatchedTopo:" + ex.Message);
            }
        }

        private ComponentNode GetCmpNode(int matchIndex)
        {
            var cmpNodeList = new List<ComponentNode>();
            foreach (var cmp in Components)
            {
                foreach (var node in cmp.CmpTopoNet.NodeArray)
                {
                    cmpNodeList.Add(node);
                }
            }
            return cmpNodeList[matchIndex];
        }

        private string GetCmpName(ComponentNode cNode)
        {
            return Components.Where(cmp => cmp.CmpTopoNet.NodeArray.Contains(cNode)).Select(cmp => cmp.Name).FirstOrDefault();
        }

        #region 实现接口
        public void MouseEventHandler(object sender, MouseEventArgs e)
        {
            //处理鼠标事件放在TopoNetView中实现
            _topoView.MouseEventHandler(sender, e);
            if (_topoView.ChoosedBv != null)
            {
                var chooseNode = _topoView.ChoosedBv as DynamicNode;
                PropertyForm.Show(chooseNode);
            }
            else
            {
                PropertyForm.Show(this.GetModelInstance());
            }
            base.TriggerRedrawRequst();
        }

        public BaseDrawer GetChoosedBaseView(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion 实现接口


        #region 重载虚函数
        public override void DrawView(Graphics g)
        {
            _topoView.DrawView(g);
        }

        public override Size GetViewSize()
        {
            //计算颗粒的个数每5个计算颗粒对应800宽度
            return new Size(this._sTopo.ComputeNodeNum * 800 / 5, 400);
        }

        public override object GetModelInstance()
        {
            return this._topoNet;
        }
        #endregion 重载虚函数

        #region 匹配应用相关算法

        private void MatchApps()
        {
            var resultTopoList = new List<List<DynamicNode>>();//记录各cmp匹配完的结果

            foreach (Models.Component cmp in Components)
            {
                var matchedTopoList = resultTopoList;           //记录已匹配的节点
                resultTopoList = new List<List<DynamicNode>>();   //重置结果

                if (matchedTopoList.Count == 0)//第一个应用匹配
                {
                    MatchOneApp(cmp, new List<DynamicNode>());
                    resultTopoList = _appMatchedTopoList;
                }
                else
                {
                    foreach (var usedtopo in matchedTopoList)
                    {
                        MatchOneApp(cmp, usedtopo);
                        //组合新的topolist和usedTopolist到resultTopoList
                        UnionTopoList(_appMatchedTopoList, usedtopo, ref resultTopoList);
                    }
                }
                if (resultTopoList.Count == 0)
                {
                    MessageBox.Show(string.Format("应用{0}匹配失败", Components.ToList().IndexOf(cmp)));
                    return;
                }
            }
            _appMatchedTopoList = resultTopoList;
        }

        private void UnionTopoList(List<List<DynamicNode>> newTopoList, List<DynamicNode> usedTopo, ref List<List<DynamicNode>> resultTopoList)
        {
            foreach (var newTopo in newTopoList)
            {
                var resultTopo = new List<DynamicNode>();
                resultTopo.AddRange(usedTopo);
                resultTopo.AddRange(newTopo);
                resultTopoList.Add(resultTopo);
            }
        }

        private void MatchOneApp(Models.Component cmp, List<DynamicNode> usedtopo)
        {
            var sNodeArray = _topoNet.NodeArray;                         //当前所包含的所有资源节点
            var cNodeArray = cmp.CmpTopoNet.NodeArray;                  //需要匹配的应用节点集合
            Stack<DynamicNode> selectedNode = new Stack<DynamicNode>(); //用一个堆栈来存储选择了的节点
            _appMatchedTopoList = new List<List<DynamicNode>>();        //把已匹配的节点设置为空

            var curCNode = cNodeArray[selectedNode.Count];
            try
            {
                foreach (var node in sNodeArray)
                {
                    //目标节点不在已经使用了的节点中，并且匹配成功
                    if (!usedtopo.Contains(node) && NodeMatched(curCNode, node))
                    {
                        selectedNode.Push(node);
                        //递归访问下一个
                        DFS_MatchNode(cmp, selectedNode, usedtopo);
                        selectedNode.Pop();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("StaticTopo.MatchApp:" + e.Message);
            }
        }

        private void DFS_MatchNode(Models.Component cmp, Stack<DynamicNode> selectedNode, List<DynamicNode> usedtopo)
        {
            var sNodeArray = _topoNet.NodeArray;
            var cNodeArray = cmp.CmpTopoNet.NodeArray;
            int selectedNum = selectedNode.Count;

            if (selectedNum == cNodeArray.Length)//退出条件
            {
                _appMatchedTopoList.Add(selectedNode.Reverse().ToList());
                return;
            }

            var curCNode = cNodeArray[selectedNum];
            foreach (var node in sNodeArray)
            {
                if (!usedtopo.Contains(node) && !selectedNode.Contains(node) && NodeMatched(curCNode, node))
                {
                    //判断当前node与selectedNode的连接关系，是否满足curCNode与已匹配的cNode节点的连接情况；
                    if (LinesMatched(cmp, curCNode, selectedNode, node))
                    {
                        selectedNode.Push(node);
                        DFS_MatchNode(cmp, selectedNode, usedtopo);
                        selectedNode.Pop();
                    }
                }
            }
        }

        private bool NodeMatched(ComponentNode cNode, DynamicNode dNode)
        {
            //判断节点的状态是否满足，只有当节点状态既不是OnLine同时也不是Used的时候，匹配失败；
            if (dNode.Status != NodeStatus.OnLine && dNode.Status != NodeStatus.Used) return false;
            if (dNode.SNode.NodeType != cNode.NodeType) return false;
            //Todo:比较具体计算颗粒属性是否满足
            return true;
        }

        //判断该节点连接是否满足构件连接
        private bool LineMatched(ComponentLine cLine, DynamicLine dLine)
        {
            if (cLine == null)//构件连接不存在，则能满足
            {
                return true;
            }
            else if (dLine != null)//构件,节点连接都存在
            {
                //Todo:比较具体连接属性是否满足
                return true;
            }
            return false;
        }

        //判断cNode与cmp当中的各个构件的连接关系，是否gNode与selectedNode各个节点也满足
        private bool LinesMatched(Models.Component cmp, ComponentNode cNode, Stack<DynamicNode> selectedNode, DynamicNode dNode)
        {
            for (int i = 0; i < cmp.NodeNum; i++)
            {
                var curCNode = cmp.CmpTopoNet.NodeArray[i];//当前的构件节点
                if (curCNode == cNode)
                {
                    break;
                    //continue;
                }
                var curDNode = selectedNode.Reverse().ToList()[curCNode.NodeId];   //当前的动态节点
                //GTX连接判断
                var cGTXLine = cmp.CmpTopoNet.GTXLinks[cNode.NodeId, curCNode.NodeId];
                var dGtxLine = _topoNet.GTXLinks[dNode.SNode.UrlId, curDNode.SNode.UrlId];
                if (!LineMatched(cGTXLine, dGtxLine))
                {
                    return false;
                }
                //LVDS连接判断
                var cLVDSLine = cmp.CmpTopoNet.LVDSLinks[cNode.NodeId, curCNode.NodeId];
                var dLVDSLine = _topoNet.LVDSLinks[dNode.SNode.UrlId, curDNode.SNode.UrlId];
                if (!LineMatched(cLVDSLine, dLVDSLine))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion 匹配应用相关算法

        /// <summary>
        /// 动态topo的节点
        /// </summary>
        public class DynamicNode : BaseNode
        {
            [Category("静态属性"), Description("节点对应的静态节点属性"), ReadOnly(true)]
            public StaticNode SNode { get; private set; }       //对应的静态节点

            [Category("构件属性"), Description("节点对应的构件节点属性"), ReadOnly(true)]
            public ComponentNode CNode { get; set; }            //对应的构件组件

            [Category("其他信息"), Description("节点所在应用名称"), ReadOnly(true)]
            public string ComName { get; set; }                 //该节点对应的应用名

            [Category("其他信息"), Description("节点状态"), ReadOnly(true)]
            public NodeStatus Status { get; set; }              //节点对应的状态

            [Category("其他信息"), Description("节点是否有对应下载文件"), ReadOnly(true)]
            public bool IsAssigned { get; set; }                //是否被分配了文件

            [Category("其他信息"), Description("节点是对应下载文件名"), ReadOnly(true)]
            public string FileName { get; set; }                //对应的下载文件

            public DynamicNode(StaticNode sNode)
                : base(sNode.NodeType, sNode.Name)
            {
                SNode = sNode;
                CNode = null;
                Status = NodeStatus.OffLine; //默认状态为掉线
                IsAssigned = false;
                FileName = string.Empty;
                ComName = string.Empty;
            }

            public override void DrawNode(Graphics graph, Rectangle rect)
            {
                var coreView = base.GetBaseDrawerCore(SNode.NodeObject, rect);
                if (Status == NodeStatus.OnLine)
                {
                    coreView.DrawView(graph);
                }
                else if (Status == NodeStatus.Used)
                {
                    coreView.DrawView(graph, CNode.Name);
                }
                else
                {
                    coreView.DrawView(graph, Pens.Gray, Brushes.Gray);
                }
            }
            public override void DrawChoosedNode(Graphics graph, Rectangle rect)
            {
                var coreView = base.GetBaseDrawerCore(SNode.NodeObject,rect);
                if (Status == NodeStatus.OnLine)
                {
                    coreView.ChoosedDrawView(graph);
                }
                else if (Status == NodeStatus.Used)
                {
                    coreView.ChoosedDrawView(graph, CNode.Name);
                }
                else
                {
                    coreView.DrawView(graph, Pens.Gray, Brushes.Gray);
                }
            }
        }

        /// <summary>
        /// 动态topo的节点
        /// </summary>
        public class DynamicLine : BaseLine
        {
            public StaticLine SLink { get; private set; }
            public DynamicLine(StaticLine sLink)
                : base(sLink.LinkType, sLink.FirstEndId, sLink.SecondEndId, sLink.LanesNum)
            {
                SLink = sLink;
            }

            public override void DrawLine(Graphics graph, List<Point> line)
            {
                SLink.DrawLine(graph, line);
            }
        }
    }
}
