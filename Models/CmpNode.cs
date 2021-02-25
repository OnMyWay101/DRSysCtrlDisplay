using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DRSysCtrlDisplay.Models
{
    /// <summary>
    /// 计算颗粒节点定义
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CmpNode : XMLManager.IXmlTransformByPath
    {
        #region 基本属性
        [Category("基本属性"), Description("节点类型"), ReadOnly(true)]
        public EndType NodeType { get; set; }                      //节点对应的芯片类型

        [Category("基本属性"), Description("属性信息"), ReadOnly(true)]
        public ModelBaseCore Obj { get; set; }            //对应的芯片实例

        [Category("基本属性"), Description("节点名称"), ReadOnly(true)]
        public string Name { get { return Obj.Name; } }

        //Todo:显示资源相关信息
        [Category("资源信息"), Description("以太网发布资源"), ReadOnly(true)]
        public EthSource[] EthPbSourceArray { get { return EthPbSources.ToArray(); } }

        [Category("资源信息"), Description("以太网订阅资源"), ReadOnly(true)]
        public EthSource[] EthSubSourceArray { get { return EthSubSources.ToArray(); } }

        [Category("资源信息"), Description("RapidIO发布资源"), ReadOnly(true)]
        public RioSource[] RioPbSourceArray { get { return RioPbSources.ToArray(); } }

        [Category("资源信息"), Description("RapidIO订阅资源"), ReadOnly(true)]
        public RioSource[] RioSubSourceArray { get { return RioSubSources.ToArray(); } }

        [Browsable(false)]
        public List<EthSource> EthPbSources { get; set; }  //以太网发布的资源

        [Browsable(false)]
        public List<EthSource> EthSubSources { get; set; } //以太网订阅的资源

        [Browsable(false)]
        public List<RioSource> RioPbSources { get; set; } //rio网订阅的资源

        [Browsable(false)]
        public List<RioSource> RioSubSources { get; set; } //rio网订阅的资源
        #endregion 基本属性

        public CmpNode(EndType nodeType, ModelBaseCore obj)
        {
            EthPbSources = new List<EthSource>();
            EthSubSources = new List<EthSource>();
            RioPbSources = new List<RioSource>();
            RioSubSources = new List<RioSource>();

            NodeType = nodeType;
            Obj = obj;
        }

        public ModelBaseCore CreateObjectByPath(string objectFilePath)
        {
            var core = Obj.CreateByPath(objectFilePath);

            try
            {
                XDocument xd = XDocument.Load(objectFilePath);
                XElement sourceNode = xd.Root.Element("SourceDefine");

                PullEthSources(sourceNode.Element("EthPub"), EthPbSources);
                PullEthSources(sourceNode.Element("EthSub"), EthSubSources);

                PullRioSources(sourceNode.Element("RioPub"), RioPbSources);
                PullRioSources(sourceNode.Element("RioSub"), RioSubSources);
                xd.Save(objectFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
            return core;
        }

        public void SaveXmlByPath(string xmlFilePath)
        {
            Obj.SaveByPath(xmlFilePath);
            XDocument xd = XDocument.Load(xmlFilePath);
            XElement rt = xd.Root;
            rt.Add(new XElement("SourceDefine",
                new XElement("EthPub"),
                new XElement("EthSub"),
                new XElement("RioPub"),
                new XElement("RioSub")
                ));

            PushEthSources(rt.Element("SourceDefine").Element("EthPub"), EthPbSources);
            PushEthSources(rt.Element("SourceDefine").Element("EthSub"), EthSubSources);

            PushRioSources(rt.Element("SourceDefine").Element("RioPub"), RioPbSources);
            PushRioSources(rt.Element("SourceDefine").Element("RioSub"), RioSubSources);
            xd.Save(xmlFilePath);
        }

        private void PushEthSources(XElement node, List<EthSource> eSrcs)
        {
            foreach (var eSrc in eSrcs)
            {
                node.Add(new XElement("Source",
                    new XAttribute("Sn", eSrc.Sn),
                    new XAttribute("NodeName", eSrc.NodeName),
                    new XAttribute("SourceName", eSrc.SourceName)
                    ));
            }
        }

        private void PushRioSources(XElement node, List<RioSource> rSrcs)
        {
            foreach (var rSrc in rSrcs)
            {
                node.Add(new XElement("Source",
                    new XAttribute("Sn", rSrc.Sn),
                    new XAttribute("NodeName", rSrc.NodeName),
                    new XAttribute("SourceName", rSrc.SourceName),
                    new XAttribute("PackMaxLen", rSrc.PackMaxLen),
                    new XAttribute("BufSize", rSrc.BufSize)
                    ));
            }
        }

        private void PullEthSources(XElement node, List<EthSource> eSrcs)
        {
            foreach(var sNode in node.Elements())
            {
                var eSrc = new EthSource();
                eSrc.Sn = int.Parse(sNode.Attribute("Sn").Value);
                eSrc.NodeName = sNode.Attribute("NodeName").Value;
                eSrc.SourceName = sNode.Attribute("SourceName").Value;
                eSrcs.Add(eSrc);
            }
        }

        private void PullRioSources(XElement node, List<RioSource> rSrcs)
        {
            foreach (var sNode in node.Elements())
            {
                var rSrc = new RioSource();
                rSrc.Sn = int.Parse(sNode.Attribute("Sn").Value);
                rSrc.NodeName = sNode.Attribute("NodeName").Value;
                rSrc.SourceName = sNode.Attribute("SourceName").Value;
                rSrc.PackMaxLen = int.Parse(sNode.Attribute("PackMaxLen").Value);
                rSrc.BufSize = int.Parse(sNode.Attribute("BufSize").Value);
                rSrcs.Add(rSrc);
            }
        }

    }

}
