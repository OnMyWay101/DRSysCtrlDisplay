using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    public abstract class BaseNode
    {
        public EndType NodeType { get; private set; }
        public string Name { get; private set; }

        public BaseNode(EndType type, string name)
        {
            this.NodeType = type;
            this.Name = name;
        }

        public abstract void DrawNode(Graphics graph, Rectangle rect);
        public abstract void DrawChoosedNode(Graphics graph, Rectangle rect);
    }

    public abstract class BaseLine
    {
        [Description("连接类型")]
        public LinkType LinkType { get; set; }

        [Description("端1Id")]
        public int FirstEndId { get; set; }

        [Description("端2Id")]
        public int SecondEndId { get; set; }

        [Description("连接带宽")]
        public LinkLanes LanesNum { get; set; }

        public BaseLine() { }

        public BaseLine(LinkType type, int firstId, int secondId, LinkLanes lanesNum)
        {
            LinkType = type;
            FirstEndId = firstId;
            SecondEndId = secondId;
            LanesNum = lanesNum;
        }
        public abstract void DrawLine(Graphics graph, List<Point> line);
    }

    public class TopoNet<TNode, TLine>
        where TNode : BaseNode
        where TLine : BaseLine
    {
        public TNode ChoosedNode { get; set; }  //被选中的节点
        public TNode[] NodeArray { get; set; }
        public TLine[] EthLinks { get; set; }    //节点是否接入局域网(以太网)
        public TLine[] RioLinks { get; set; }    //节点是否接入topo网(RapidIO网络)
        public TLine[,] GTXLinks { get; set; }      //某两个节点之间是否有GTX连接
        public TLine[,] LVDSLinks { get; set; }      //某两个节点之间是否有LVDS连接

        public TopoNet(int nodeNum)
        {
            NodeArray = new TNode[nodeNum];
            EthLinks = new TLine[nodeNum];
            RioLinks = new TLine[nodeNum];
            GTXLinks = new TLine[nodeNum, nodeNum];
            LVDSLinks = new TLine[nodeNum, nodeNum];
        }

        public void SetNodeValue(int nodeId, TNode value)
        {
            NodeArray[nodeId] = value;
        }

        public void SetLinkValue(TLine value)
        {
            LinkType type = value.LinkType;
            int endId1 = value.FirstEndId;
            int endId2 = value.SecondEndId;

            switch (type)
            {
                case LinkType.EtherNet:
                    EthLinks[endId1] = value;
                    break;
                case LinkType.RapidIO:
                    RioLinks[endId1] = value;
                    break;
                case LinkType.GTX:
                    GTXLinks[endId1, endId2] = value;
                    GTXLinks[endId2, endId1] = value;
                    break;
                default://LinkType.LVDS
                    LVDSLinks[endId1, endId2] = value;
                    LVDSLinks[endId2, endId1] = value;
                    break;
            }
        }

        public List<int> GetConnectedNodes(int nodeId, LinkType type)
        {
            List<int> retNodesList = new List<int>();
            TLine[,] lineMatrix = null;
            switch (type)
            {
                case LinkType.EtherNet:
                case LinkType.RapidIO:
                    break;
                case LinkType.GTX:
                    lineMatrix = GTXLinks;
                    break;
                case LinkType.LVDS:
                    lineMatrix = LVDSLinks;
                    break;
                default:
                    MessageBox.Show("TopoNet.GetConnectedNodes invalid type!");
                    break;
            }
            for (int i = 0; i < NodeArray.Length; i++)
            {
                if (lineMatrix[nodeId, i] != null)
                {
                    retNodesList.Add(i);
                }
            }
            return retNodesList;
        }

    }

}
