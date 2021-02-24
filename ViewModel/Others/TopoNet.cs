using DRSysCtrlDisplay.Models;
using DRSysCtrlDisplay.Princeple;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class BaseNode
    {
        [Category("\t基本信息"), Description("节点类型"), ReadOnly(true)]
        public EndType NodeType { get; private set; }

        [Category("\t基本信息"), Description("节点名称"), ReadOnly(true)]
        public string Name { get; private set; }

        public BaseNode(EndType type, string name)
        {
            this.NodeType = type;
            this.Name = name;
        }

        public abstract void DrawNode(Graphics graph, Rectangle rect);
        public abstract void DrawChoosedNode(Graphics graph, Rectangle rect);

        //实现一个
        protected BaseDrawerCore GetBaseDrawerCore(ModelBaseCore obj, Rectangle rect)
        {
            if (NodeType == EndType.PPC)
            {
                PPC ppc = obj as PPC;
                return new PPCViewModel(ppc, rect);
            }
            else if (NodeType == EndType.FPGA)
            {
                FPGA fpga = obj as FPGA;
                return new FPGAViewModel(fpga, rect);
            }
            else //NodeType == EndType.ZYNQ
            {
                ZYNQ zynq = obj as ZYNQ;
                return new ZYNQViewModel(zynq,rect);
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
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

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TopoNet<TNode, TLine>
        where TNode : BaseNode
        where TLine : BaseLine
    {
        [Category("节点集合"), Description("节点集合"), ReadOnly(true)]
        public TNode[] NodeArray { get; set; }

        [Category("连接集合"), Description("以太网连接"), ReadOnly(true)]
        public TLine[] EthLinks { get; set; }    //节点是否接入局域网(以太网)

        [Category("连接集合"), Description("RapidIO连接"), ReadOnly(true)]
        public TLine[] RioLinks { get; set; }    //节点是否接入topo网(RapidIO网络)

        [Category("连接集合"), Description("GTX连接"), ReadOnly(true)]
        public TLine[] GTXLinkArray 
        { 
            get
            {
                int nodeNum = NodeArray.Length;
                TLine[] retArray = new TLine[nodeNum * nodeNum];
                for (int i = 0; i < nodeNum; i++)
                {
                    for (int j = 0; j < nodeNum; j++)
                    {
                        retArray[i * nodeNum + j] = GTXLinks[i, j];
                    }
                }
                return retArray;
            }
        }   

        [Category("连接集合"), Description("LVDS连接"), ReadOnly(true)]
        public TLine[] LVDSLinkArray 
        {
            get
            {
                int nodeNum = NodeArray.Length;
                TLine[] retArray = new TLine[nodeNum * nodeNum];
                for (int i = 0; i < nodeNum; i++)
                {
                    for (int j = 0; j < nodeNum; j++)
                    {
                        retArray[i * nodeNum + j] = LVDSLinks[i, j];
                    }
                }
                return retArray;
            }
        }  

        [Browsable(false)]
        public TLine[,] LVDSLinks { get; set; }      //某两个节点之间是否有LVDS连接

        [Browsable(false)]
        public TLine[,] GTXLinks { get; set; }      //某两个节点之间是否有GTX连接

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
