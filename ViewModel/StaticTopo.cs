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
    /// 静态topo图
    /// </summary>
    public class StaticTopo : BaseDrawer, IDrawerChoosed
    {
        public SystemStru System { get; set; }                                  //对应的机箱

        public RawTopo _rawTopo { get; private set; }                           //机箱对应的原生topo图

        public TopoNet<StaticNode, StaticLine> _topoNet { get; private set; }   //计算颗粒包含的topo图

        private TopoNetView<StaticNode, StaticLine> _topoView;                  //Topo图的画图类

        public int EndNodeNum { get; private set; }                             //端点(vpx,sw,ppc,fpga,zynq)的总数量

        public int ComputeNodeNum { get; private set; }                         //计算颗粒(ppc,fpga,zynq)的数量

        public BaseDrawer ChoosedBv { get; set; }

        public StaticTopo(SystemStru sys, Rectangle rect)
        {
            System = sys;
            InitNodeNum();
            Init(rect);
        }
        public StaticTopo(SystemStru sys)
        {
            System = sys;
            InitNodeNum();
        }

        public override void Init(Rectangle rect)
        {
            base.Init(rect);
            InitRawTopo();
            InitTopoNet();
            _topoView = new TopoNetView<StaticNode, StaticLine>(base._rect, _topoNet);
        }

        //初始化端点的总数和计算颗粒的总数
        private void InitNodeNum()
        {
            foreach (var ctnName in System.CntNames)//遍历机箱数组
            {
                var ctn = ModelFactory<Models.Container>.CreateByName(ctnName);
                var bp = ModelFactory<Models.BackPlane>.CreateByName(ctn.BackPlaneName);

                EndNodeNum += bp.VirtualSlotsNum;  //添加背板包含端点(vpx)的数量到总数量

                //累加板卡包含端点(PPC,FPGA,ZYNQ,SW )的数量
                foreach (var boardInfoPair in ctn.BoardNameDir)
                {
                    string boardName = boardInfoPair.Value;
                    if (ctn.IsContainBoard(boardName))
                    {
                        var board = ModelFactory<Board>.CreateByName(boardName);
                        //累计计算颗粒得到计算颗粒总数
                        ComputeNodeNum += (board.PPCList.Count + board.FPGAList.Count + board.ZYNQList.Count);
                        EndNodeNum += board.SwitchList.Count;//累计交换机数量到总数量
                    }
                }
            }
            EndNodeNum += ComputeNodeNum;     //添加计算颗粒数量到总数量
        }

        //初始化原生topo图
        private void InitRawTopo()
        {
            _rawTopo = new RawTopo(EndNodeNum);
            int curUrlId = 0;   //当前的urlId

            for (int i = 0; i < System.CntNames.Length; i++)//遍历机箱数组
            {
                var ctn = ModelFactory<Models.Container>.CreateByName(System.CntNames[i]);
                var bp = ModelFactory<Models.BackPlane>.CreateByName(ctn.BackPlaneName);
                AddBackPlane(bp, i, ref curUrlId);
                foreach (var boardPair in ctn.BoardNameDir)
                {
                    if (!ctn.IsContainBoard(boardPair.Value))
                    {
                        continue;
                    }
                    Board board = ModelFactory<Board>.CreateByName(boardPair.Value);
                    AddBoard(board, i, boardPair.Key, ref curUrlId);
                }
            }
            AddContainerLinks();
            _rawTopo.OptimizeVpxConnection();
        }

        //添加一个背板的描述到_rawTopo里面
        private void AddBackPlane(BackPlane bp, int frameId, ref int curUrlId)
        {
            //添加节点(槽位，包含虚拟槽位)
            for (int i = 0; i < bp.VirtualSlotsNum; i++)
            {
                RawTopo.RawNode bpNode = new RawTopo.RawNode(EndType.VPX, null, curUrlId++);
                bpNode.FrameId = frameId;
                bpNode.SlotId = i;
                _rawTopo.SetNodeValue(bpNode.UrlId, bpNode);
            }

            //加入link
            foreach (var links in bp.LinksArray)
            {
                foreach (var link in links)
                {
                    var subLink = new RawTopo.RawSubLink(link.FirstEndPostion, link.SecondEndPostion, link.LanesNum);
                    int urlId1 = _rawTopo.NodeArray
                        .Where(node => ((node != null) && (node.SlotId == link.FirstEndId) && (node.Type == EndType.VPX) && (node.FrameId == frameId)))
                        .Select(node => node.UrlId)
                        .FirstOrDefault();
                    int urlId2 = _rawTopo.NodeArray
                        .Where(node => ((node != null) && (node.SlotId == link.SecondEndId) && (node.Type == EndType.VPX) && (node.FrameId == frameId)))
                        .Select(node => node.UrlId)
                        .FirstOrDefault();
                    _rawTopo.AddSubLink(link.LinkType, urlId1, urlId2, subLink);
                }
            }
        }

        //添加一个板卡的描述到图里面
        private void AddBoard(Board board, int frameId, int slotId, ref int curUrlId)
        {
            //创建一个字典用于保存GraphNode的endId与UrlId的映射关系
            Dictionary<int, int> endId2UrlId = new Dictionary<int, int>();

            //1.添加节点到_nodeList
            #region
            //添加ppc节点到顶点集合
            foreach (var ppc in board.PPCList)
            {
                var node = new RawTopo.RawNode(EndType.PPC, ppc.Name, curUrlId++);
                node.FrameId = frameId;
                node.SlotId = slotId;
                node.GenerateEndId(board.PPCList.IndexOf(ppc));

                endId2UrlId.Add(node.EndId, node.UrlId);
                _rawTopo.SetNodeValue(node.UrlId, node);
            }
            //添加fpga节点到顶点集合
            foreach (var fpga in board.FPGAList)
            {
                var node = new RawTopo.RawNode(EndType.FPGA, fpga.Name, curUrlId++);
                node.FrameId = frameId;
                node.SlotId = slotId;
                node.GenerateEndId(board.FPGAList.IndexOf(fpga));

                endId2UrlId.Add(node.EndId, node.UrlId);
                _rawTopo.SetNodeValue(node.UrlId, node);
            }
            //添加zynq节点到顶点集合
            foreach (var zynq in board.ZYNQList)
            {
                var node = new RawTopo.RawNode(EndType.ZYNQ, zynq.Name, curUrlId++);
                node.FrameId = frameId;
                node.SlotId = slotId;
                node.GenerateEndId(board.ZYNQList.IndexOf(zynq));

                endId2UrlId.Add(node.EndId, node.UrlId);
                _rawTopo.SetNodeValue(node.UrlId, node);
            }
            //添加sw节点到顶点集合
            foreach (var sw in board.SwitchList)
            {
                //这里的交换机的名字用Type代替
                var node = new RawTopo.RawNode(EndType.SW, sw.Type, curUrlId++);
                node.FrameId = frameId;
                node.SlotId = slotId;
                node.GenerateEndId(board.SwitchList.IndexOf(sw));

                endId2UrlId.Add(node.EndId, node.UrlId);
                _rawTopo.SetNodeValue(node.UrlId, node);
            }
            //不用添加板卡上的vpx节点到顶点集合，因为该vpx一定对应了一个backPlane的slot
            #endregion
            //2.添加link到对应的matrix
            foreach (var link in board.LinkList)
            {
                //把每一条link都加入到matrix里面
                var subLink = new RawTopo.RawSubLink(link.FirstEndPositionList[0], link.SecondEndPositionList[0]);
                //端1一般为板卡上的芯片不为vpx,端2才可能会包含vpx类型
                int urlId1 = endId2UrlId[RawTopo.RawNode.CalculateEndId(link.FirstEndType, link.FirstEndId)];
                int urlId2 = 0;
                if (link.SecondEndType == EndType.VPX)
                {
                    //直接去找vpx插在相应槽位的urlId
                    urlId2 = _rawTopo.NodeArray
                        .Where(node => ((node != null) && (node.SlotId == slotId) && (node.Type == EndType.VPX) && (node.FrameId == frameId)))
                        .Select(node => node.UrlId)
                        .FirstOrDefault();
                }
                else
                {
                    urlId2 = endId2UrlId[RawTopo.RawNode.CalculateEndId(link.SecondEndType, link.SecondEndId)];
                }
                _rawTopo.AddSubLink(link.LinkType, urlId1, urlId2, subLink);
            }
        }

        //添加机箱之间的连接
        private void AddContainerLinks()
        {
            for (int i = 0; i < System.LinksArray.Length; i++)
            {
                var ctn = ModelFactory<Models.Container>.CreateByName(System.CntNames[i]);
                var bp = ModelFactory<Models.BackPlane>.CreateByName(ctn.BackPlaneName);
                int slot1 = bp.VirtualSlotsNum - 2;//外接口的槽位号
                var links = System.LinksArray[i];
                foreach (SystemStruLink link in links)
                {
                    //创建一天原生子链接
                    var subLink = new RawTopo.RawSubLink(link.FirstEndPostion, link.SecondEndPostion);

                    //找到在_rawTopo里面端点1对应的urlId
                    int urlId1 = _rawTopo.NodeArray.Where(rNode => rNode.FrameId == link.FirstEndId
                        && rNode.Type == EndType.VPX && rNode.SlotId == slot1)
                        .Select(rNode => rNode.UrlId).FirstOrDefault();

                    //找到在_rawTopo里面端点1对应的urlId
                    var ctn2 = ModelFactory<Models.Container>.CreateByName(System.CntNames[i]);
                    var bp2 = ModelFactory<Models.BackPlane>.CreateByName(ctn.BackPlaneName);
                    int slot2 = bp2.VirtualSlotsNum - 2;
                    int urlId2 = _rawTopo.NodeArray.Where(rNode => rNode.FrameId == link.SecondEndId
                        && rNode.Type == EndType.VPX && rNode.SlotId == slot2)
                        .Select(rNode => rNode.UrlId).FirstOrDefault();
                    _rawTopo.AddSubLink(link.LinkType, urlId1, urlId2, subLink);
                }
            }
        }

        //初始化TopoNet
        private void InitTopoNet()
        {
            int curUrlId = 0;
            _topoNet = new TopoNet<StaticNode, StaticLine>(ComputeNodeNum);

            //得到一个集合，是按照板卡为单位来分的NodeArray(PPC,ZYNQ,FPGA),且按照一定顺序
            var groups = from rNode in _rawTopo.NodeArray
                         where rNode.Type != EndType.SW && rNode.Type != EndType.VPX
                         orderby rNode.FrameId ascending, rNode.SlotId ascending
                         group rNode by new { rNode.FrameId, rNode.SlotId };
            try
            {
                //分组访问把其中的rNode录入TopoNet中
                foreach (var group in groups)
                {
                    foreach (var rNode in group)
                    {
                        var sNode = new StaticNode(rNode.Type, rNode.Name, curUrlId++);
                        sNode.FrameId = rNode.FrameId;
                        sNode.SlotId = rNode.SlotId;
                        sNode.EndId = rNode.EndId;
                        sNode.NodeObject = StaticTopo_GenNodeObj(sNode.NodeType, sNode.Name);
                        _topoNet.SetNodeValue(sNode.UrlId, sNode);
                    }
                }

                //把rNode的相关连接录入TopoNet中
                AddLinksToTopoNet();
            }
            catch (Exception e)
            {
                MessageBox.Show("InitTopoNet:" + e.Message);
            }

        }

        //通过一个xml文件来创建一个节点的BaseViewCore
        private ModelBaseCore StaticTopo_GenNodeObj(Princeple.EndType type, string xmlName)
        {
            Type objType = TypeConvert.GetEndType(type);
            var core = Activator.CreateInstance(objType) as ModelBaseCore;
            return (ModelBaseCore)(core.CreateObjectByName(xmlName));
        }

        //添加所有连接到TopoNet里面
        private void AddLinksToTopoNet()
        {
            //处理EtherNet连接
            AddNetLinks(LinkType.EtherNet);
            //处理RapidIONet连接
            AddNetLinks(LinkType.RapidIO);
            //处理GtxNet连接
            AddSignalLinks(LinkType.GTX);
            //处理LvdsNet连接
            AddSignalLinks(LinkType.LVDS);
        }

        //添加EtherNet或者RapidIO连接到TopoNet里面
        private void AddNetLinks(LinkType type)
        {
            var linkNet = _rawTopo.GetMaxTopo(type);
            foreach (var rNode in linkNet)
            {
                if (rNode.Type == EndType.SW)
                {
                    continue;
                }
                var staticNode = RNodeToSNode(rNode);
                _topoNet.SetLinkValue(new StaticLine(type, staticNode.UrlId, 0));
            }
        }

        //添加Gtx或者Lvds连接到TopoNet里面
        private void AddSignalLinks(LinkType lType)
        {
            foreach (var sNode in _topoNet.NodeArray)
            {
                var rNode = SNodeToRNode(sNode);
                var linkArray = _rawTopo.GetConnectedLink(lType, rNode);
                foreach (var rLink in linkArray)
                {
                    if (rLink != null)
                    {
                        var oppersiteRNode = _rawTopo.NodeArray[rLink.End2Id];
                        var oppersiteSNode = RNodeToSNode(oppersiteRNode);
                        _topoNet.SetLinkValue(new StaticLine(lType, sNode.UrlId, oppersiteSNode.UrlId));
                    }
                }
            }
        }

        //转化RawNode到与之对应的StaticNode
        private StaticNode RNodeToSNode(RawTopo.RawNode rNode)
        {
            var staticNode = _topoNet.NodeArray.Where(sNode => rNode.Type == sNode.NodeType
                && rNode.FrameId == sNode.FrameId
                && rNode.SlotId == sNode.SlotId
                && rNode.EndId == sNode.EndId).FirstOrDefault();
            return staticNode;
        }

        //转化StaticNode到与之对应的RawNode
        private RawTopo.RawNode SNodeToRNode(StaticNode sNode)
        {
            var rawNode = _rawTopo.NodeArray.Where(rNode => rNode.Type == sNode.NodeType
                && rNode.FrameId == sNode.FrameId
                && rNode.SlotId == sNode.SlotId
                && rNode.EndId == sNode.EndId).FirstOrDefault();
            return rawNode;
        }
        #region 实现接口
        public void MouseEventHandler(object sender, MouseEventArgs e)
        {
            //处理鼠标事件放在TopoNetView中实现
            _topoView.MouseEventHandler(sender, e);

            if (_topoView.ChoosedBv != null)
            {
                var chooseNode = _topoView.ChoosedBv as StaticNode;
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
        public override Size GetViewSize()
        {
            //计算颗粒的个数每5个计算颗粒对应800宽度
            return new Size(ComputeNodeNum * 800 / 5, 400);
        }

        public override void DrawView(Graphics g)
        {
            _topoView.DrawView(g);
        }

        public override object GetModelInstance()
        {
            return this._topoNet;
        }
        #endregion 重载虚函数

        /// <summary>
        /// 静态topo的节点
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class StaticNode : BaseNode
        {
            [Category("位置信息"), Description("节点全局资源ID"), ReadOnly(true)]
            public int UrlId { get; private set; }     

            [Category("位置信息"), Description("机箱号ID"), ReadOnly(true)]
            public int FrameId { get; set; }           

            [Category("位置信息"), Description("槽位号ID"), ReadOnly(true)]
            public int SlotId { get; set; }           

            [Category("位置信息"), Description("端点在板内的ID号"), ReadOnly(true)]
            public int EndId { get; set; }              //端点号ID

            [Category("节点属性"), Description("节点属性详细信息"), ReadOnly(true)]
            public ModelBaseCore NodeObject { get; set; }
            public StaticNode(EndType type, string name, int urlId)
                : base(type, name)
            {
                UrlId = urlId;
            }

            public override void DrawNode(Graphics graph, Rectangle rect)
            {
                BaseDrawerCore coreView = base.GetBaseDrawerCore(NodeObject, rect);
                coreView.DrawView(graph);
            }
            public override void DrawChoosedNode(Graphics graph, Rectangle rect)
            {
                BaseDrawerCore coreView = base.GetBaseDrawerCore(NodeObject, rect);
                coreView.ChoosedDrawView(graph);
            }
        }

        /// <summary>
        /// 静态topo的连接线
        /// </summary>
        public class StaticLine : BaseLine
        {
            public StaticLine(LinkType type, int firstId, int secondId)
                : base(type, firstId, secondId, LinkLanes.None)
            { }

            public override void DrawLine(Graphics graph, List<Point> line)
            {
                //点集为空则不画
                if (line == null)
                {
                    return;
                }
                Pen linePen;
                switch (base.LinkType)
                {
                    case LinkType.EtherNet:
                        linePen = ConnectAreaColor.Pen_EtherNet;
                        break;
                    case LinkType.RapidIO:
                        linePen = ConnectAreaColor.Pen_RapidIO;
                        break;
                    case LinkType.GTX:
                        linePen = ConnectAreaColor.Pen_GTX;
                        break;
                    default: //LinkType.LVDS
                        linePen = ConnectAreaColor.Pen_LVDS;
                        break;
                }
                graph.DrawLines(linePen, line.ToArray());
            }
        }
    }
}
