using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.ViewModel.Others;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay.Models
{
    class Component : ModelBase, XMLManager.IXmlTransformByName
    {
        public Component() { }

        public TopoNet<ComponentNode, ComponentLine> CmpTopoNet { get; private set; }
        public int NodeNum { get; private set; }

        private void InitTopo(int nodeNum)
        {
            NodeNum = nodeNum;
            CmpTopoNet = new TopoNet<ComponentNode, ComponentLine>(nodeNum);
        }

        public void SaveXmlByName()
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

        //生成一个构件节点对应的Xml文件
        private void Component_GenNodeFile(string componentName, ComponentNode nodeInfo)
        {
            string xmlPath = string.Format(@"{0}\{1}\{2}.xml", PathManager.GetComponentPath(), componentName, nodeInfo.Name);
            nodeInfo.NodeObject.SaveXmlByPath(xmlPath);
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

        public ModelBase CreateObjectByName(string objectName)
        {
            Component component = new Component();
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

                var cmpNode = new Component.ComponentNode(nodeType, nodeName, nodeNum, nodeObject);
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

        //通过一个xml文件来创建一个节点的BaseViewCore
        private BaseViewCore Component_GenNodeObj(Princeple.EndType type, string xmlPath)
        {
            Type objType = TypeConvert.GetEndType(type);
            Type FactoryType = typeof(BaseViewCoreFactory<>);
            FactoryType = FactoryType.MakeGenericType(objType);
            return (BaseViewCore)(FactoryType.InvokeMember("CreateByPath"
                , BindingFlags.Default | BindingFlags.InvokeMethod, null, null, new object[] { xmlPath }));
        }
    }
}
