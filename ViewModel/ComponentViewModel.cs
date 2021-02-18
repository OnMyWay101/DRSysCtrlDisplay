using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;
using DRSysCtrlDisplay.Princeple;

namespace DRSysCtrlDisplay
{
    public class ComponentViewModel : BaseDrawer
    {
        public TopoNet<ComponentNode, ComponentLine> CmpTopoNet { get; private set; }
        private TopoNetView<ComponentNode, ComponentLine> _topoView;
        public int NodeNum { get; private set; }

        public ComponentViewModel() { }

        public void InitTopo(int nodeNum)
        {
            NodeNum = nodeNum;
            CmpTopoNet = new TopoNet<ComponentNode, ComponentLine>(nodeNum);
        }


        public override void DrawView(Graphics g){ }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            _topoView = new TopoNetView<ComponentNode, ComponentLine>(g, rect, CmpTopoNet);
            _topoView.DrawView();
        }

        public override Size GetViewSize()
        {
            return new Size(800, 400);
        }

        public override void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetComponentPath(), this.Name);
            //先判断一些文件是否存在
            if (!PathManager.CheckFile(xmlPath))
            {
                return;
            }
            Directory.CreateDirectory(string.Format(@"{0}\{1}", PathManager.GetComponentPath(), this.Name));
            //保存XML文件
            XDocument xd = new XDocument(
                new XElement("Component",
                    new XAttribute("Name", this.Name),
                    new XAttribute("NodeNum", this.NodeNum),
                    new XElement("Nodes"),
                    new XElement("Links",
                        new XElement("EthLinks"),
                        new XElement("RioLinks"),
                        new XElement("GTXLinks"),
                        new XElement("LVDSLinks")
                        )
                    )
                );
            //找到根节点
            XElement rt = xd.Element("Component");
            //添加节点
            XElement nodes = rt.Element("Nodes");
            foreach (var nodeInfo in this.CmpTopoNet.NodeArray)
            {
                nodes.Add(new XElement("Node",
                    new XAttribute("NodeNum", nodeInfo.NodeNum),
                    new XAttribute("NodeType", nodeInfo.NodeType),
                    new XAttribute("NodeName", nodeInfo.Name)
                    ));
                //生成一个构件节点相关文件
                Component_GenNodeFile(this.Name, nodeInfo);
            }
            //添加连接关系
            XElement links = rt.Element("Links");
            //以太网
            XElement entLinks = links.Element("EthLinks");
            Component_AddXmlLinks(entLinks, this.CmpTopoNet.EthLinks, "EthLink");
            //RapidIO
            XElement rioLinks = links.Element("RioLinks");
            Component_AddXmlLinks(rioLinks, this.CmpTopoNet.RioLinks, "RioLink");
            //GTX
            XElement gtxLinks = links.Element("GTXLinks");
            Component_AddXmlLinks(gtxLinks, Component_LinksArrayTranse(this.NodeNum, this.CmpTopoNet.GTXLinks), "GTXLink");
            //LVDS
            XElement lvdsLinks = links.Element("LVDSLinks");
            Component_AddXmlLinks(lvdsLinks, Component_LinksArrayTranse(this.NodeNum, this.CmpTopoNet.LVDSLinks), "LVDSLink");

            xd.Save(xmlPath);
        }

        //转化二维数组到一维数组
        private ComponentLine[] Component_LinksArrayTranse(int Num, ComponentLine[,] line2DArray)
        {
            ComponentLine[] retArray = new ComponentLine[Num * Num];
            for (int i = 0; i < Num; i++)
            {
                for (int j = 0; j < Num; j++)
                {
                    retArray[i * Num + j] = line2DArray[i, j];
                }
            }
            return retArray;
        }

        //添加一条Link信息到XElement节点
        private void Component_AddXmlLinks(XElement LinksElement, IEnumerable<ComponentLine> links
            , string linkName)
        {
            foreach (var linkInfo in links)
            {
                if (linkInfo != null)
                {
                    LinksElement.Add(new XElement(linkName,
                        new XAttribute("LinkType", linkInfo.LinkType),
                        new XAttribute("FirstEndId", linkInfo.FirstEndId),
                        new XAttribute("SecondEndId", linkInfo.SecondEndId),
                        new XAttribute("LanesNum", linkInfo.LanesNum)
                        ));
                }
            }
        }

        public override BaseDrawer CreateObjectByName(string objectName)
        {
            ComponentViewModel component = new ComponentViewModel();
            string xmlPathDir = Path.Combine(PathManager.GetComponentPath(), objectName);
            string xmlPath = string.Format("{0}.xml", xmlPathDir);
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("CreateObject_Component:没有该Component对应的XML文件！");
                return null;
            }
            XDocument xd = XDocument.Load(xmlPath);
            //根元素的Attribute
            XElement rt = xd.Element("Component");
            component.Name = rt.Attribute("Name").Value;
            component.InitTopo(int.Parse(rt.Attribute("NodeNum").Value));

            var nodesElement = rt.Element("Nodes");
            foreach (var e in nodesElement.Elements())
            {
                var nodeNum = int.Parse(e.Attribute("NodeNum").Value);
                var nodeType = (EndType)(Enum.Parse(typeof(ComputeNodeType), e.Attribute("NodeType").Value));
                var nodeName = e.Attribute("NodeName").Value;
                var nodeObject = Component_GenNodeObj(nodeType, string.Format(@"{0}\{1}.xml", xmlPathDir, nodeName));

                var cmpNode = new ComponentViewModel.ComponentNode(nodeType, nodeName, nodeNum, nodeObject);
                component.CmpTopoNet.SetNodeValue(nodeNum, cmpNode);
            }

            var linksElement = rt.Element("Links");

            var ethLinksElement = linksElement.Element("EthLinks");
            foreach (var eLine in ethLinksElement.Elements())
            {
                component.CmpTopoNet.SetLinkValue(Component_TransXmlLink(eLine));
            }
            var rioLinksElement = linksElement.Element("RioLinks");
            foreach (var eLine in rioLinksElement.Elements())
            {
                component.CmpTopoNet.SetLinkValue(Component_TransXmlLink(eLine));
            }
            var gtxLinksElement = linksElement.Element("GTXLinks");
            foreach (var eLine in gtxLinksElement.Elements())
            {
                component.CmpTopoNet.SetLinkValue(Component_TransXmlLink(eLine));
            }
            var lvdsLinksElement = linksElement.Element("LVDSLinks");
            foreach (var eLine in lvdsLinksElement.Elements())
            {
                component.CmpTopoNet.SetLinkValue(Component_TransXmlLink(eLine));
            }
            return component;
        }

        //把一个连接的XElement转化为一个ComponentLine实体
        private ComponentLine Component_TransXmlLink(XElement LinkElement)
        {
            int nodeId1 = int.Parse(LinkElement.Attribute("FirstEndId").Value);
            int nodeId2 = int.Parse(LinkElement.Attribute("SecondEndId").Value);
            var linkType = (LinkType)(Enum.Parse(typeof(LinkType), LinkElement.Attribute("LinkType").Value));
            var lanes = (LinkLanes)(Enum.Parse(typeof(LinkLanes), LinkElement.Attribute("LanesNum").Value));

            return new ComponentLine(linkType, nodeId1, nodeId2, lanes);
        }

        //通过一个xml文件来创建一个节点的BaseViewCore
        private BaseDrawerCore Component_GenNodeObj(Princeple.EndType type, string xmlPath)
        {
            Type objType = TypeConvert.GetEndType(type);
            Type FactoryType = typeof(BaseViewCoreFactory<>);
            FactoryType = FactoryType.MakeGenericType(objType);
            return (BaseDrawerCore)(FactoryType.InvokeMember("CreateByPath"
                , BindingFlags.Default | BindingFlags.InvokeMethod, null, null, new object[] { xmlPath }));
        }

        public override void MouseEventHandler(object sender, MouseEventArgs e)
        {
            CmpTopoNet.ChoosedNode = _topoView.GetChoosedNodeView(e);
            if (CmpTopoNet.ChoosedNode != null)
            {
                //PropertyForm.Show(CmpTopoNet.ChoosedNode.NodeObject);
                PropertyForm.Show(CmpTopoNet.ChoosedNode);
            }
            else
            {
                PropertyForm.Show(this);
            }
            base.TriggerRedrawRequst();
        }



    }
}
