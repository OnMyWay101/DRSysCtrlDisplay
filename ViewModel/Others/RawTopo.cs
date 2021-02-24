using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
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
}
