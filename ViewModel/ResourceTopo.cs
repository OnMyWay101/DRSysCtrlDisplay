using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using DRSysCtrlDisplay.Princeple;
using SystemInformation = DRSysCtrlDisplay.TargetListener.MultiCastPacket.MultiCastPacketInfo.SystemInformation;
using StaticNode = DRSysCtrlDisplay.StaticTopo.StaticNode;
using StaticLine = DRSysCtrlDisplay.StaticTopo.StaticLine;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// 使用邻接矩阵的方式描述资源池原生topo图
    /// </summary>
    public class RawTopo
    {
        public RawNode[] NodeArray { get; private set; }             //顶点数组
        //以太网连接矩阵；注意：GraphLink_Eth[x, y]所包含的link的firstEnd对应x,secondEnd对应y；
        private RawLink[,] _ethLinksMatrix;
        private RawLink[,] _rioLinksMatrix;                          //RapidIO连接矩阵
        public RawLink[,] GtxLinksMatrix { get; private set; }       //GTX连接矩阵
        public RawLink[,] LvdsLinksMatrix { get; private set; }      //LVDS连接矩阵

        public RawTopo(int nodeNum)
        {
            NodeArray = new RawNode[nodeNum];
            _ethLinksMatrix = new RawLink[nodeNum, nodeNum];
            _rioLinksMatrix = new RawLink[nodeNum, nodeNum];
            GtxLinksMatrix = new RawLink[nodeNum, nodeNum];
            LvdsLinksMatrix = new RawLink[nodeNum, nodeNum];
        }

        public void SetNodeValue(int nodeId, RawNode value)
        {
            NodeArray[nodeId] = value;
        }

        private void SetLinkValue(RawLink value)
        {
            LinkType type = value.Type;
            int endId1 = value.End1Id;
            int endId2 = value.End2Id;

            switch (type)
            {
                case LinkType.EtherNet:
                    _ethLinksMatrix[endId1, endId2] = value;
                    break;
                case LinkType.RapidIO:
                    _rioLinksMatrix[endId1, endId2] = value;
                    break;
                case LinkType.GTX:
                    GtxLinksMatrix[endId1, endId2] = value;
                    break;
                default://LinkType.LVDS
                    LvdsLinksMatrix[endId1, endId2] = value;
                    break;
            }
        }

        //添加一个RowSubLink到相关的两个RowLink上
        public void AddSubLink(LinkType type, int urlId1, int urlId2, RawSubLink sLink)
        {
            //往链接为urlId1，urlId2的位置添加一条RowSubLink
            AddSubLinkInner(type, urlId1, urlId2, sLink);
            //往链接为urlId2，urlId1的位置添加一条RowSubLink
            RawSubLink oppositeSLink = sLink.GetOppositeSL();
            AddSubLinkInner(type, urlId2, urlId1, oppositeSLink);
        }

        //添加一个RowSubLink到相关的RowLink上
        private void AddSubLinkInner(LinkType type, int urlId1, int urlId2, RawSubLink sLink)
        {
            //获取一条RowLink
            RawLink rowLink = GetLinkValue(type, urlId1, urlId2);
            if (rowLink == null)
            {
                rowLink = new RawLink(type, urlId1, urlId2);
                SetLinkValue(rowLink);
            }
            rowLink.AddSubLine(sLink);
        }

        private RawLink GetLinkValue(LinkType type, int urlId1, int urlId2)
        {
            switch (type)
            {
                case LinkType.EtherNet:
                    return _ethLinksMatrix[urlId1, urlId2];
                case LinkType.RapidIO:
                    return _rioLinksMatrix[urlId1, urlId2];
                case LinkType.GTX:
                    return GtxLinksMatrix[urlId1, urlId2];
                default://LinkType.LVDS
                    return LvdsLinksMatrix[urlId1, urlId2];
            }
        }

        //通过vpx之间的连接关系，直接得到芯片、交换机之间的连接
        //转化关系为：板内芯片-> 板卡vpx -> 背板vpx1 -> 背板vpx2 -> 板卡vpx -> 板内芯片
        public void OptimizeVpxConnection()
        {
            List<RawSubLink> visitedEthSl = new List<RawSubLink>();   //表示被访问过的以太网sublink(其他端->vpx端)的集合;
            List<RawSubLink> visitedRioSl = new List<RawSubLink>();   //表示被访问过的rapidio的sublink(其他端->vpx端)的集合;
            List<RawSubLink> visitedGtxSl = new List<RawSubLink>();   //表示被访问过的gtx的sublink(其他端->vpx端)的集合;
            List<RawSubLink> visitedLvdsSl = new List<RawSubLink>();   //表示被访问过的lvds的sublink(其他端->vpx端)的集合;

            foreach (var node in NodeArray)
            {
                if (node.Type == EndType.VPX)
                {//vpx端点直接过掉
                    continue;
                }
                //非vpx端点访问相连接的vpx端点
                for (int i = 0; i < NodeArray.Length; i++)
                {
                    if (NodeArray[i].Type == EndType.VPX)
                    {
                        //处理以太网、rapidio、gtx、lvds的连接
                        ProcessGraphLink(LinkType.EtherNet, node.UrlId, i, visitedEthSl);
                        ProcessGraphLink(LinkType.RapidIO, node.UrlId, i, visitedRioSl);
                        ProcessGraphLink(LinkType.GTX, node.UrlId, i, visitedGtxSl);
                        ProcessGraphLink(LinkType.LVDS, node.UrlId, i, visitedLvdsSl);
                    }
                }
            }
        }

        /// <summary>
        /// 处理urlId1(非vpx节点)和urlId2(vpx节点)间的连接
        /// </summary>
        /// <param name="linkType"></param>
        /// <param name="urlId1"></param>
        /// <param name="urlId2"></param>
        /// <param name="visitedSl"></param>
        private void ProcessGraphLink(LinkType linkType, int urlId1, int urlId2, List<RawSubLink> visitedSl)
        {
            var rLink = GetLinkValue(linkType, urlId1, urlId2);
            //处理sublink的连接
            if (rLink != null)
            {
                foreach (var sl in rLink.SubList)
                {
                    if (!visitedSl.Contains(sl))
                    {
                        int oppositeUrlId = -1;
                        RawSubLink oppositeSl = null;
                        //处理与vpx链接的子链接
                        ProcessSubLink(linkType, sl, urlId1, urlId2, ref oppositeUrlId, ref oppositeSl);

                        if ((oppositeUrlId != -1) && (oppositeSl != null))
                        {//当找到对面有对应的非vpx端点和sublink的时候
                            RawSubLink newSl = new RawSubLink(sl.End1Pos, oppositeSl.End2Pos);
                            AddSubLink(linkType, urlId1, oppositeUrlId, newSl);
                            //把处理过的子链接加入visitedSl,避免2次访问
                            visitedSl.Add(sl);
                            visitedSl.Add(oppositeSl);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///处理一个子链接,得到对面的非vpx端点的urlId和对应的sublink
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="urlId1"></param>
        /// <param name="urlId2"></param>
        /// <param name="oppositeUrlId">一个输出的引用，用来记录对面的非vpx端点的urlId</param>
        /// <param name="oppositeSl">一个输出的引用，用来记录对面的非vpx端点对应的sublink</param>
        private void ProcessSubLink(LinkType linkType, RawSubLink sl, int urlId1, int urlId2, ref int oppositeUrlId, ref RawSubLink oppositeSl)
        {
            bool findFlag = false;//是否找到下一条子链接的标志，找到就可以结束循环

            //返回条件
            if (NodeArray[urlId2].Type != EndType.VPX)
            {
                oppositeUrlId = urlId2;
                oppositeSl = sl;
                return;
            }

            //访问urlId2端点与其他端点（除了urlId1）的连接
            for (int i = 0; i < NodeArray.Length; i++)
            {
                if (i != urlId1)
                {
                    //访问每个端点的子链接,按照深度来搜索
                    var rLink = GetLinkValue(linkType, urlId2, i);
                    if (rLink != null)
                    {
                        foreach (var sLink in rLink.SubList)
                        {
                            //判断子链接sl与访问的子链接sLink是否是尾(sl)首(sLink)相连
                            if (sLink.End1Pos == sl.End2Pos)
                            {
                                //!!!注意递归调用
                                ProcessSubLink(linkType, sLink, urlId2, i, ref oppositeUrlId, ref oppositeSl);
                                findFlag = true;
                            }
                            if (findFlag)
                            {
                                break;
                            }
                        }
                    }
                }
                if (findFlag)
                {
                    break;
                }
            }
        }

        //按照深度优先的方式搜索所有节点
        private void DFS_Node(LinkType linkType, RawNode node, int[] visitedNodes, List<RawNode> nodeList)
        {
            visitedNodes[node.UrlId] = 1;
            nodeList.Add(node);

            for (int i = 0; i < NodeArray.Length; i++)
            {
                //访问该顶点互联的其他顶点（不能是vpx端点，且端点没被访问过）
                if ((NodeArray[i].Type != EndType.VPX) && (visitedNodes[NodeArray[i].UrlId] == 0))
                {
                    RawLink gLink = GetLinkValue(linkType, node.UrlId, i);
                    if (gLink != null)
                    {
                        DFS_Node(linkType, NodeArray[i], visitedNodes, nodeList);
                    }
                }
            }
        }

        //获取一个最大的子网对应的点的集合（网络连接的只有：ethernet、rapidio）;
        public List<RawNode> GetMaxTopo(LinkType linkType)
        {
            List<RawNode> resultList = null;
            List<List<RawNode>> topoList = new List<List<RawNode>>();   //子网集合
            int[] visitedNodes = new int[NodeArray.Length];     //记录被访问过了的端点：0-没被访问过；1-被访问过

            //从每一个端点（除了vpx端点）出发，按照深度优先搜索以太网topo连接
            foreach (var node in NodeArray)
            {
                if ((node.Type == EndType.VPX) || (visitedNodes[node.UrlId] == 1))
                {//vpx端点直接过掉,被访问过的也过掉
                    continue;
                }
                List<RawNode> nodeList = new List<RawNode>();
                //由该GraphNode出发进行搜索有链接的GraphNode
                DFS_Node(linkType, node, visitedNodes, nodeList);
                topoList.Add(nodeList);
            }

            //找到最大的子网
            foreach (var topo in topoList)
            {
                if (resultList == null)
                {
                    resultList = topo;
                }
                else
                {
                    if (topo.Count > resultList.Count)
                    {
                        resultList = topo;
                    }
                }
            }
            return resultList;
        }

        //获取与RowNode相连的RowLink的集合,集合数组的下标为对端RowNode的urlId
        public RawLink[] GetConnectedLink(LinkType linkType, RawNode node)
        {
            RawLink[] resultLinks = new RawLink[NodeArray.Length];

            for (int i = 0; i < NodeArray.Length; i++)
            {
                if ((i != node.UrlId) && (NodeArray[i].Type != EndType.VPX))
                {//对端端点不能为自己，且不能为vpx端点
                    RawLink rLink = GetLinkValue(linkType, node.UrlId, i);
                    if (rLink != null)
                    {
                        resultLinks[i] = rLink;
                    }
                }
            }
            return resultLinks;
        }

        /// <summary>
        /// 描述一个机箱对应的图中的点：芯片、交换机、VPX
        /// </summary>
        public class RawNode
        {
            public EndType Type { get; private set; }       //点的类型
            public string Name { get; private set; }        //点的名字(当点的类型是芯片的时候，可以通过名字获取芯片的实例)
            public int UrlId { get; private set; }          //点对应的全局资源定位的ID

            public int FrameId { get; set; }                //机箱号ID
            public int SlotId { get; set; }                 //槽位号ID
            public int EndId { get; private set; }          //端点号ID（端点在板内的ID号）

            public RawNode(EndType type, string name, int urlId)
            {
                Type = type;
                Name = name;
                UrlId = urlId;

                FrameId = 0;
                SlotId = 0;
                EndId = 0;
            }

            //计算EndId
            public void GenerateEndId(int typeId)
            {
                EndId = CalculateEndId(this.Type, typeId);
            }

            /// <summary>
            /// 生成EndId,注意：这里不是直接设置EndId为参数的值，而是通过参数和该GraphNode的类型来生成的
            /// 规则：EndId = EndType * 10 + typeId；其中EndType枚举里面PPC=1,FPGA=2,ZYNQ=3,SW=4,VPX=5;
            /// </summary>
            /// <param name="endType">该端点的类型</param>
            /// <param name="typeId">该端点在板卡内属于同一类型端点集合里面的序号</param>
            public static int CalculateEndId(EndType endType, int typeId)
            {
                int endTypeNum = (int)endType;
                if (typeId == -1)
                {
                    typeId = 0;
                }
                return endTypeNum * 10 + typeId;
            }
        }

        #region GraphLink连接的相关定义区域
        /// <summary>
        /// 一个用于描述顶点连接的基类
        /// </summary>
        public class RawLink
        {
            public List<RawSubLink> SubList { get; private set; }//一条抽象连接的子链接（一条物理连接）
            public LinkType Type { get; private set; }
            public int End1Id { get; private set; }
            public int End2Id { get; private set; }

            public RawLink(LinkType type, int end1Id, int end2Id)
            {
                Type = type;
                End1Id = end1Id;
                End2Id = end2Id;
                SubList = new List<RawSubLink>();
            }

            public void AddSubLine(RawSubLink subLink)
            {
                SubList.Add(subLink);
            }
        }

        /// <summary>
        /// 一个具体的物理连接
        /// </summary>
        public class RawSubLink
        {
            //连接信息描述
            public int End1Pos { get; private set; }
            public int End2Pos { get; private set; }

            //数据宽度属性，一般一个数据宽度也对应了vpx的的列数。
            //比如说：rapidio的4x连到vpx的4列
            public LinkLanes DataWidth { get; private set; }

            public RawSubLink(int end1Pos, int end2Pos)
            {
                End1Pos = end1Pos;
                End2Pos = end2Pos;
                DataWidth = LinkLanes.X1;
            }

            public RawSubLink(int end1Pos, int end2Pos, LinkLanes dataWidth)
            {
                End1Pos = end1Pos;
                End2Pos = end2Pos;
                DataWidth = dataWidth;
            }

            public RawSubLink GetOppositeSL()
            {
                return new RawSubLink(this.End2Pos, this.End1Pos, this.DataWidth);
            }
        }

        #endregion
    }

    /// <summary>
    /// 静态topo图
    /// </summary>
    public class StaticTopo : BaseView
    {
        public RawTopo _rawTopo { get; private set; }                           //机箱对应的原生topo图
        public TopoNet<StaticNode, StaticLine> _topoNet { get; private set; }   //计算颗粒包含的topo图
        private TopoNetView<StaticNode, StaticLine> _topoView;                  //Topo图的画图类
        public SystemStruViewModel System { get; set; }                                  //对应的机箱
        public int EndNodeNum { get; private set; }                             //端点(vpx,sw,ppc,fpga,zynq)的总数量
        public int ComputeNodeNum { get; private set; }                         //计算颗粒(ppc,fpga,zynq)的数量

        public StaticTopo(SystemStruViewModel sys)
        {
            System = sys;
            EndNodeNum = 0;
            ComputeNodeNum = 0;
            InitNodeNum();
            InitRawTopo();
            InitTopoNet();
        }

        //初始化端点的总数和计算颗粒的总数
        private void InitNodeNum()
        {
            foreach (var ctn in System.CntsArray)//遍历机箱数组
            {
                EndNodeNum += ctn._backPlane.VirtualSlotsNum;  //添加背板包含端点(vpx)的数量到总数量

                //累加板卡包含端点(PPC,FPGA,ZYNQ,SW )的数量
                foreach (var boardInfoPair in ctn.BoardNameDir)
                {
                    string boardName = boardInfoPair.Value;
                    if (ctn.IsContainBoard(boardName))
                    {
                        var board = BaseViewFactory<BoardViewModel>.CreateByName(boardName);
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

            for (int i = 0; i < System.CntsArray.Length; i++)//遍历机箱数组
            {
                var ctn = System.CntsArray[i];
                AddBackPlane(ctn._backPlane, i, ref curUrlId);
                foreach (var boardPair in ctn.BoardNameDir)
                {
                    if (!ctn.IsContainBoard(boardPair.Value))
                    {
                        continue;
                    }
                    BoardViewModel board = BaseViewFactory<BoardViewModel>.CreateByName(boardPair.Value);
                    AddBoard(board, i, boardPair.Key, ref curUrlId);
                }
            }
            AddContainerLinks();
            _rawTopo.OptimizeVpxConnection();
        }

        //添加一个背板的描述到_rawTopo里面
        private void AddBackPlane(BackPlaneViewModel bp, int frameId, ref int curUrlId)
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
        private void AddBoard(BoardViewModel board, int frameId, int slotId, ref int curUrlId)
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
                int slot1 = System.CntsArray[i]._backPlane.VirtualSlotsNum - 2;//外接口的槽位号
                var links = System.LinksArray[i];
                foreach (SystemStruViewModel.SystemStruLink link in links)
                {
                    //创建一天原生子链接
                    var subLink = new RawTopo.RawSubLink(link.FirstEndPostion, link.SecondEndPostion);

                    //找到在_rawTopo里面端点1对应的urlId
                    int urlId1 = _rawTopo.NodeArray.Where(rNode => rNode.FrameId == link.FirstEndId
                        && rNode.Type == EndType.VPX && rNode.SlotId == slot1)
                        .Select(rNode => rNode.UrlId).FirstOrDefault();

                    //找到在_rawTopo里面端点1对应的urlId
                    int slot2 = System.CntsArray[link.SecondEndId]._backPlane.VirtualSlotsNum - 2;
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
        private BaseViewCore StaticTopo_GenNodeObj(Princeple.EndType type, string xmlName)
        {
            Type objType = TypeConvert.GetEndType(type);
            Type FactoryType = typeof(BaseViewCoreFactory<>);
            FactoryType = FactoryType.MakeGenericType(objType);
            return (BaseViewCore)(FactoryType.InvokeMember("CreateByName"
                , BindingFlags.Default | BindingFlags.InvokeMethod, null, null, new object[] { xmlName }));
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

        public override void MouseEventHandler(object sender, MouseEventArgs e)
        {
            _topoNet.ChoosedNode = _topoView.GetChoosedNodeView(e);
            if (_topoNet.ChoosedNode != null)
            {
                PropertyForm.Show(_topoNet.ChoosedNode);
            }
            else
            {
                PropertyForm.Show(this);
            }
            base.TriggerRedrawRequst();
        }

        public override Size GetViewSize()
        {
            //计算颗粒的个数每5个计算颗粒对应800宽度
            return new Size(_topoNet.NodeArray.Length * 800 / 5, 400);
        }

        public override void DrawView(Graphics g) { }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            _topoView = new TopoNetView<StaticNode, StaticLine>(g, rect, _topoNet);
            _topoView.DrawView();
        }

        /// <summary>
        /// 静态topo的节点
        /// </summary>
        public class StaticNode : BaseNode
        {
            public int UrlId { get; private set; }      //点对应的全局资源定位的ID
            [BrowsableAttribute(false)]
            public BaseViewCore NodeObject { get; set; }
            public int FrameId { get; set; }            //机箱号ID
            public int SlotId { get; set; }             //槽位号ID
            public int EndId { get; set; }              //端点号ID（端点在板内的ID号）

            public StaticNode(EndType type, string name, int urlId)
                : base(type, name)
            {
                UrlId = urlId;
            }

            public override void DrawNode(Graphics graph, Rectangle rect)
            {
                NodeObject.DrawView(graph, rect);
            }
            public override void DrawChoosedNode(Graphics graph, Rectangle rect)
            {
                NodeObject.ChoosedDrawView(graph, rect);
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
    /// <summary>
    /// 画一个系统的动态topo图，包含计算颗粒(PPC,ZYNQ,FPGA)和连接关系(EtherNet,RapidIO,GTX,LVDS )
    /// </summary>
    public class DynamicTopo : BaseView
    {
        private StaticTopo _sTopo = null;                                       //该动态Topo图对应的静态topo图
        public TopoNet<DynamicNode, DynamicLine> _topoNet { get; private set; } //计算颗粒包含的topo图
        private TopoNetView<DynamicNode, DynamicLine> _topoView;                //Topo图的画图类

        ComponentViewModel[] _cmps = null;                       //该topo对应的应用集
        List<List<DynamicNode>> _appMatchedTopoList;    //应用匹配的节点集合topo的集合
        int _choosedMatchedListNum = -1;                //选择的匹配节点集合topo的序号
        List<Boolean>[] _onLineFlags = null;            //各机箱槽位板卡在线信息
        public Boolean _reconfigFlag { get; set; }      //该应用是否发生了重构

        public DynamicTopo(StaticTopo sTopo)
        {
            _sTopo = sTopo;
            _appMatchedTopoList = new List<List<DynamicNode>>();
            InitTopoNet();

            InitOnlineFlags();
        }

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
            _onLineFlags = new List<Boolean>[_sTopo.System.CntsArray.Length];
            for (int i = 0; i < _onLineFlags.Length; i++)
            {
                var cnt = _sTopo.System.CntsArray[i];   //当前对应机箱
                _onLineFlags[i] = new List<bool>();     //该机箱板卡对应在线情况
                for (int j = 0; j < cnt._backPlane.SlotsNum; j++)
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

        public override void OnNodeInfoChanged(TargetNode tNode)
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
            catch (Exception e)
            {
                MessageBox.Show("IsBoardOnLine:" + e.Message);
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

        private ComponentViewModel.ComponentNode GetCmpNode(int matchIndex)
        {
            var cmpNodeList = new List<ComponentViewModel.ComponentNode>();
            foreach (var cmp in _cmps)
            {
                foreach (var node in cmp.CmpTopoNet.NodeArray)
                {
                    cmpNodeList.Add(node);
                }
            }
            return cmpNodeList[matchIndex];
        }

        private string GetCmpName(ComponentViewModel.ComponentNode cNode)
        {
            return _cmps.Where(cmp => cmp.CmpTopoNet.NodeArray.Contains(cNode)).Select(cmp => cmp.Name).FirstOrDefault();
        }

        public override void MouseEventHandler(object sender, MouseEventArgs e)
        {
            _topoNet.ChoosedNode = _topoView.GetChoosedNodeView(e);
            if (_topoNet.ChoosedNode != null)
            {
                PropertyForm.Show(_topoNet.ChoosedNode);
            }
            else
            {
                PropertyForm.Show(this);
            }
            base.TriggerRedrawRequst();
        }

        public override void DrawView(Graphics g) { }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            _topoView = new TopoNetView<DynamicNode, DynamicLine>(g, rect, _topoNet);
            _topoView.DrawView();
        }

        public override Size GetViewSize()
        {
            //计算颗粒的个数每5个计算颗粒对应800宽度
            return new Size(_topoNet.NodeArray.Length * 800 / 5, 400);
        }

        #region 匹配应用相关算法

        public void MatchApps(ComponentViewModel[] cmps)
        {
            _cmps = cmps;
            var resultTopoList = new List<List<DynamicNode>>();//记录各cmp匹配完的结果

            foreach (ComponentViewModel cmp in cmps)
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
                    MessageBox.Show(string.Format("应用{0}匹配失败", cmps.ToList().IndexOf(cmp)));
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

        private void MatchOneApp(ComponentViewModel cmp, List<DynamicNode> usedtopo)
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

        private void DFS_MatchNode(ComponentViewModel cmp, Stack<DynamicNode> selectedNode, List<DynamicNode> usedtopo)
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

        private bool NodeMatched(ComponentViewModel.ComponentNode cNode, DynamicNode dNode)
        {
            //判断节点的状态是否满足，只有当节点状态既不是OnLine同时也不是Used的时候，匹配失败；
            if (dNode.Status != NodeStatus.OnLine && dNode.Status != NodeStatus.Used) return false;
            if (dNode.SNode.NodeType != cNode.NodeType) return false;
            //Todo:比较具体计算颗粒属性是否满足
            return true;
        }

        //判断该节点连接是否满足构件连接
        private bool LineMatched(ComponentViewModel.ComponentLine cLine, DynamicLine dLine)
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
        private bool LinesMatched(ComponentViewModel cmp, ComponentViewModel.ComponentNode cNode, Stack<DynamicNode> selectedNode, DynamicNode dNode)
        {
            for (int i = 0; i < cmp.NodeNum; i++)
            {
                var curCNode = cmp.CmpTopoNet.NodeArray[i];//当前的构件节点
                if (curCNode == cNode)
                {
                    break;
                    //continue;
                }
                var curDNode = selectedNode.Reverse().ToList()[curCNode.NodeNum];   //当前的动态节点
                //GTX连接判断
                var cGTXLine = cmp.CmpTopoNet.GTXLinks[cNode.NodeNum, curCNode.NodeNum];
                var dGtxLine = _topoNet.GTXLinks[dNode.SNode.UrlId, curDNode.SNode.UrlId];
                if (!LineMatched(cGTXLine, dGtxLine))
                {
                    return false;
                }
                //LVDS连接判断
                var cLVDSLine = cmp.CmpTopoNet.LVDSLinks[cNode.NodeNum, curCNode.NodeNum];
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
            public StaticNode SNode { get; private set; }       //对应的静态节点
            public ComponentViewModel.ComponentNode CNode { get; set; }  //对应的构件组件
            public string ComName { get; set; }                 //该节点对应的应用名
            public NodeStatus Status { get; set; }              //节点对应的状态
            public bool IsAssigned { get; set; }                //是否被分配了文件
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
                if (Status == NodeStatus.OnLine)
                {
                    SNode.NodeObject.DrawView(graph, rect);
                }
                else if (Status == NodeStatus.Used)
                {
                    SNode.NodeObject.DrawView(graph, rect, CNode.Name);
                }
                else
                {
                    SNode.NodeObject.DrawView(graph, rect, Pens.Gray, Brushes.Gray);
                }
            }
            public override void DrawChoosedNode(Graphics graph, Rectangle rect)
            {
                if (Status == NodeStatus.OnLine)
                {
                    SNode.NodeObject.ChoosedDrawView(graph, rect);
                }
                else if (Status == NodeStatus.Used)
                {
                    SNode.NodeObject.ChoosedDrawView(graph, rect, CNode.Name);
                }
                else
                {
                    SNode.NodeObject.DrawView(graph, rect, Pens.Gray, Brushes.Gray);
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
