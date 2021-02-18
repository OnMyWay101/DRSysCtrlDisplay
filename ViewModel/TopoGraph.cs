using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.ViewModel.Others;

namespace DRSysCtrlDisplay
{
    
    public class TopoNetView<TNode, TLine> : IDrawerChoosed<TNode>
        where TNode : BaseNode
        where TLine : BaseLine
    {
        readonly TopoNet<TNode, TLine> _topoNet;
        readonly Graphics _graph;
        readonly int _gOffsetX;
        readonly int _gOffsetY;
        readonly Rectangle _rect;
        readonly int _nodeNum;

        Rectangle[] _nodeRects;             //顶点对应矩形集合:下标为urlId
        List<Point>[] _ethLines;            //以太网连接线的集合:下标为urlId;
        List<Point>[] _rioLines;            //rio网连接线的集合 
        List<Point>[,] _gtxLines;           //gtx连接线的集合,两个节点的urlId决定了一条gtxline
        List<Point>[,] _lvdsLines;          //lvds连接线的集合

        private const double _ethNetBarScale = 0.5 / 10;    //以太网bar高度与矩形框Height的比例
        private const double _rioNetBarScale = 0.5 / 10;    //以riobar高度与矩形框Height的比例
        private const double _nodeHeightScale = 1.5 / 10;    //节点图像的高度与矩形框高度的比例
        private const double _nodeWidthScale = 1.5 / 10;    //节点图像的宽度与矩形框高度的比例
        private const double _lineRangeScale = 2.0 / 10;      //线所包含范围的宽度与矩形框高度的比例

        public TopoNetView(Graphics g, Rectangle r, TopoNet<TNode, TLine> topo)
        {
            _graph = g;
            _gOffsetX = (int)g.Transform.OffsetX;
            _gOffsetY = (int)g.Transform.OffsetY;
            _rect = r;
            _topoNet = topo;
            _nodeNum = topo.NodeArray.Length;
            //初始成员变量

            _nodeRects = new Rectangle[_nodeNum];
            _ethLines = new List<Point>[_nodeNum];
            _rioLines = new List<Point>[_nodeNum];

            _gtxLines = new List<Point>[_nodeNum, _nodeNum];
            _lvdsLines = new List<Point>[_nodeNum, _nodeNum];
        }

        private void DrawEthNetBar()
        {
            int barHeight = (int)(_rect.Height * _ethNetBarScale);
            var ethNetBar = new Rectangle(_rect.X, _rect.Y, _rect.Width, barHeight);
            _graph.DrawRectangle(ConnectAreaColor.Pen_EtherNet, ethNetBar);
            _graph.FillRectangle(ConnectAreaColor.Brushes_EtherNet, ethNetBar);
        }

        private void DrawRioNetBar()
        {
            int barHeight = (int)(_rect.Height * _rioNetBarScale);
            int barY = _rect.Y + _rect.Height - barHeight;
            var rioNetBar = new Rectangle(_rect.X, barY, _rect.Width, barHeight);
            _graph.DrawRectangle(ConnectAreaColor.Pen_RapidIO, rioNetBar);
            _graph.FillRectangle(ConnectAreaColor.Brushes_RapidIO, rioNetBar);
        }

        private void DrawNodes()
        {
            //所有顶点矩形的大小和Y坐标是不变的
            int rectHeight = (int)(_nodeHeightScale * _rect.Height);
            int rectWidth = (int)(_nodeWidthScale * _rect.Height);
            int rectY = _rect.Y + _rect.Height / 2 - rectHeight / 2;

            int nodeOffset = (int)(_nodeWidthScale * _rect.Height);
            for (int i = 0; i < _nodeNum; i++)
            {
                int rectX = _rect.X + nodeOffset;
                _nodeRects[i] = new Rectangle(rectX, rectY, rectWidth, rectHeight);
                nodeOffset += 2 * rectWidth;
                _topoNet.NodeArray[i].DrawNode(_graph, _nodeRects[i]);
            }
        }

        //画出所有连接的line集合（etherNet,rioNet）
        private void DrawLinesList(LinkType type)
        {
            var lineViewList = (type == LinkType.EtherNet ? _ethLines : _rioLines);
            var linkList = (type == LinkType.EtherNet ? _topoNet.EthLinks : _topoNet.RioLinks);

            for (int i = 0; i < _nodeNum; i++)
            {
                if (linkList[i] != null)
                {
                    Rectangle curRect = _nodeRects[i];

                    //分配point1和point2的X,Y坐标
                    int pointX = curRect.X + curRect.Width / 2;
                    int point1Y = type == LinkType.EtherNet ? curRect.Y : (curRect.Y + curRect.Height);
                    int point2Y = type == LinkType.EtherNet ? (_rect.Y + (int)(_ethNetBarScale * _rect.Height))
                         : (_rect.Y + _rect.Height - (int)(_rioNetBarScale * _rect.Height));

                    lineViewList[i] = new List<Point>();
                    lineViewList[i].Add(new Point(pointX, point1Y));
                    lineViewList[i].Add(new Point(pointX, point2Y));
                    linkList[i].DrawLine(_graph, lineViewList[i]);
                }
            }
        }

        //画出所有连接的line集合（gtx,lvds）
        private void DrawLinesMatrix(LinkType type)
        {
            List<Point>[,] lineMatrix = (type == LinkType.GTX ? _gtxLines : _lvdsLines);
            var linkList = (type == LinkType.GTX ? _topoNet.GTXLinks : _topoNet.LVDSLinks);

            for (int i = 0; i < _nodeNum; i++)
            {
                var nodeIdList = _topoNet.GetConnectedNodes(i, type);
                AssignNodeRectPoint1(i, nodeIdList, type);
            }
            AssignNodeRectPoint2(type);

            //连接一条line的4个节点
            for (int i = 0; i < _nodeNum; i++)
            {
                //只用访问矩阵的右上半部分
                for (int j = i + 1; j < _nodeNum; j++)
                {
                    var pointList1 = lineMatrix[i, j];
                    var pointList2 = lineMatrix[j, i];
                    if (pointList1 != null && pointList2 != null)
                    {
                        pointList1.Add(pointList2[1]);
                        pointList1.Add(pointList2[0]);
                        linkList[i, j].DrawLine(_graph, pointList1);
                    }
                }
            }
        }

        #region 连接线点布局的方法
        //分配一个节点rect上面的的line的point1
        private void AssignNodeRectPoint1(int nodeId, List<int> cNodeIdList, LinkType type)
        {
            var rect = _nodeRects[nodeId];
            List<Point>[,] lineMatrix = (type == LinkType.GTX ? _gtxLines : _lvdsLines);

            //得到左，右边的nodeId连接列表
            var leftNodeIds = cNodeIdList.Where(cNodeId => cNodeId <= nodeId).Select(cNodeId => cNodeId).ToList();
            var rightNodeIds = cNodeIdList.Where(cNodeId => cNodeId > nodeId).Select(cNodeId => cNodeId).ToList();

            //轮询处理相连的节点
            foreach (var cnId in cNodeIdList)
            {
                List<int> cnIdList = (cnId <= nodeId ? leftNodeIds : rightNodeIds);
                double offsetXScale = GetScale(cnId, cnIdList, cnId > nodeId);

                //计算point1对应的X,Y坐标
                int pointX = (cnId <= nodeId ? (rect.X + (int)(offsetXScale * rect.Width / 2))
                    : (rect.X + rect.Width - (int)(offsetXScale * rect.Width / 2)));
                int pointY = (type == LinkType.GTX ? rect.Y : (rect.Y + rect.Height));

                //创建相关line的pointList,然后加入相关的点
                List<Point> linePoints = new List<Point>();
                linePoints.Add(new Point(pointX, pointY));
                lineMatrix[nodeId, cnId] = linePoints;
            }
        }

        //获取一个节点在当前节点列表中所占的序列比例.注意：suqFlag=true为升序排列，反之降序；
        private double GetScale(int cnId, List<int> cnIdList, bool suqFlag)
        {
            List<int> squList;
            int total = cnIdList.Count();

            if (suqFlag)
            {
                var squUids = from uid in cnIdList
                              orderby uid ascending
                              select uid;
                squList = squUids.ToList();
            }
            else
            {
                var squUids = from uid in cnIdList
                              orderby uid descending
                              select uid;
                squList = squUids.ToList();
            }

            int index = squList.IndexOf(cnId);
            return (index + 1.0) / (total + 1);
        }

        //分配节点rect上面的的line的point2
        private void AssignNodeRectPoint2(LinkType type)
        {
            //获取一个高度排列的优先级列表集合:Point的x和y代表的是两个矩形的序号；List的下标表示优先级
            List<List<Point>> priorityList = new List<List<Point>>();
            List<Point>[,] lineMatrix = (type == LinkType.GTX ? _gtxLines : _lvdsLines);

            //初始化优先级列表集合,i为跨度
            for (int i = 1; i < _nodeNum; i++)
            {
                var spanPointList = new List<Point>();
                for (int j = i, k = 0; j < _nodeNum; j++, k++)
                {
                    if (lineMatrix[k, j] != null)
                    {
                        spanPointList.Add(new Point(k, j));
                    }
                }
                //把子列表加入到列表集合里面去
                JoinSubList(priorityList, spanPointList, i);
            }

            //访问每一条连接
            for (int i = 0; i < _nodeNum; i++)
            {
                for (int j = 0; j < _nodeNum; j++)
                {
                    var pointList = lineMatrix[i, j];
                    if (pointList != null)
                    {
                        double heightScale = GetHeightScale(priorityList, new Point(i, j));
                        int point2X = pointList[0].X;
                        int point2Y = pointList[0].Y + (int)(heightScale * _lineRangeScale * _rect.Height)
                            * (type == LinkType.GTX ? -1 : 1);
                        pointList.Add(new Point(point2X, point2Y));
                    }
                }
            }
        }

        /// <summary>
        /// 把子列表经过调整加入到列表集合里面去
        /// </summary>
        /// <param name="totalList"></param>
        /// <param name="subList"></param>
        /// <param name="span">跨度，用作调整子列表</param>
        private void JoinSubList(List<List<Point>> totalList, List<Point> subList, int span)
        {
            //一个扩展的List<Point>数组，要来存放处理subList过后的List<Point>
            var spanLists = new List<Point>[span];
            for (int i = 0; i < spanLists.Length; i++)
            {
                spanLists[i] = new List<Point>();
            }

            //逐个访问subList里面的point,把它放入相应spanList里面；
            foreach (var point in subList)
            {
                //spanLists从index=0的list开始访问，比较该list的最后一个节点的X,与待加入point的X的差是否小于span；
                foreach (var joinList in spanLists)
                {
                    if (joinList.Count == 0)
                    {
                        joinList.Add(point);
                        break;
                    }
                    var lastPoint = joinList[joinList.Count - 1];
                    if (point.X - lastPoint.X >= span)
                    {
                        joinList.Add(point);
                    }
                }
            }
            foreach (var spanList in spanLists)
            {
                if (spanList.Count > 0)
                {
                    totalList.Add(spanList);
                }
            }
        }

        /// <summary>
        /// 获取一个point在列表集合里面的相对排序，返回比例
        /// </summary>
        /// <param name="totalList"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private double GetHeightScale(List<List<Point>> totalList, Point p)
        {
            int totalLevel = totalList.Count;   //列表集合对应的level的总和
            int squNum = 0;                     //当前point所在list，在totalList的序列号（index）
            foreach (var pList in totalList)
            {
                //对点的X,Y取反
                var opPoint = new Point(p.Y, p.X);
                if (pList.Contains(p) || pList.Contains(opPoint))
                {
                    squNum = totalList.IndexOf(pList);
                    break;
                }
            }
            return (squNum + 1.0) / (totalLevel + 1);
        }

        #endregion

        public void DrawView()
        {
            TNode choosedNode = _topoNet.ChoosedNode;

            DrawEthNetBar();
            DrawRioNetBar();
            DrawNodes();
            DrawLinesList(LinkType.EtherNet);
            DrawLinesList(LinkType.RapidIO);
            DrawLinesMatrix(LinkType.GTX);
            DrawLinesMatrix(LinkType.LVDS);
            //画被选中的图像
            if (choosedNode != null)
            {
                bool isFind = false;
                Rectangle choosedRect = GetBaseViewRect(choosedNode, ref isFind);
                if (isFind)
                {
                    choosedNode.DrawChoosedNode(_graph, choosedRect);
                }
            }
        }

        public TNode GetChoosedNodeView(MouseEventArgs e)
        {
            for (int i = 0; i < _nodeRects.Length; i++)
            {
                //把e相对于panel的坐标系转换为，该位置在初始化时g中的坐标系。
                if (_nodeRects[i].Contains(e.Location.X - _gOffsetX, e.Location.Y - _gOffsetY))
                {
                    return _topoNet.NodeArray[i];
                }
            }
            return null;
        }

        public Rectangle GetBaseViewRect(TNode nodeView, ref bool isFind)
        {
            Rectangle defaultRect = new Rectangle(0, 0, 0, 0);
            var nodeViewArray = _topoNet.NodeArray;

            for (int i = 0; i < nodeViewArray.Length; i++)
            {
                if (nodeViewArray[i] == nodeView)
                {
                    isFind = true;
                    return _nodeRects[i];
                }
            }
            isFind = false;
            return defaultRect;
        }
    }
}
