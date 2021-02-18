using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.ViewModel.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay.Models
{
    public class Board : ModelBase, XMLManager.IXmlTransformByName
    {
        #region Board的基本属性

        [Category("\t基本信息"), Description("板卡类型")]
        public string Type { get; set; }

        [Category("\t基本信息"), Description("版本号")]
        public string Version { get; set; }

        [Category("芯片信息"), Description("PowerPC集合信息")]
        public PPC[] PPCArray { get { return PPCList.ToArray(); } }

        [Category("芯片信息"), Description("FPGA集合信息")]
        public FPGA[] FPGAArray { get { return FPGAList.ToArray(); } }

        [Category("芯片信息"), Description("ZYNQ集合信息")]
        public ZYNQ[] ZYNQArray { get { return ZYNQList.ToArray(); } }

        [Category("芯片信息"), Description("Switch集合信息")]
        public SwitchDevice[] SwitchDeviceArray { get { return SwitchList.ToArray(); } }

        //@Todo:List的ToArray方法返回数组是新建的吗？
        //（里面的对象肯定都是引用，就是这个数组是不是新建的，List的内部实现起始也是一个数组，看是不是用的这个）
        [Category("连接信息"), Description("Link集合信息")]
        public BoardLink[] LinkArray { get { return LinkList.ToArray(); } }

        [BrowsableAttribute(false)]
        public List<PPC> PPCList { get; set; }              //板上的PPC芯片集合

        [BrowsableAttribute(false)]
        public List<FPGA> FPGAList { get; set; }            //板上的FPGA芯片集合

        [BrowsableAttribute(false)]
        public List<ZYNQ> ZYNQList { get; set; }            //板上的ZYNQ芯片集合

        [BrowsableAttribute(false)]
        public List<SwitchDevice> SwitchList { get; set; }  //板上的交换机芯片集合

        [BrowsableAttribute(false)]
        public List<BoardLink> LinkList { get; set; }       //板卡上连接关系的集合

        #endregion Board的基本属性

        public Board()
        {
            PPCList = new List<PPC>();
            FPGAList = new List<FPGA>();
            ZYNQList = new List<ZYNQ>();
            SwitchList = new List<SwitchDevice>();
            LinkList = new List<BoardLink>();
        }

        public void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetBoardPath(), this.Name);
            //先判断一些文件是否存在
            if (!PathManager.CheckFile(xmlPath))
            {
                return;
            }
            //保存XML文件
            XDocument xd = new XDocument(
                new XElement("Board",
                    new XAttribute("Name", base.Name),
                    new XAttribute("Type", this.Type),
                    new XAttribute("Version", this.Version),
                    new XElement("Devices",
                        new XElement("PPCs"),
                        new XElement("FPGAs"),
                        new XElement("ZYNQs"),
                        new XElement("Sws")
                        ),
                    new XElement("Links")
                    )
                );
            XElement rt = xd.Element("Board");
            //找到PPCs元素，添加PPC集合到该元素下面
            XElement ppcs = rt.Element("Devices").Element("PPCs");
            foreach (PPC p in this.PPCList)
            {
                ppcs.Add(new XElement("PPC", new XAttribute("Name", p.Name)));
            }
            //找到FPGAs元素,添加FPGA集合到该元素下面
            XElement fpgas = rt.Element("Devices").Element("FPGAs");
            foreach (FPGA f in this.FPGAList)
            {
                fpgas.Add(new XElement("FPGA", new XAttribute("Name", f.Name)));
            }
            //找到ZYNQs元素,添加ZYNQ集合到该元素下面
            XElement zynqs = rt.Element("Devices").Element("ZYNQs");
            foreach (ZYNQ z in this.ZYNQList)
            {
                zynqs.Add(new XElement("ZYNQ", new XAttribute("Name", z.Name)));
            }
            //找到Sws元素,添加Sw集合到该元素下面
            XElement sws = rt.Element("Devices").Element("Sws");
            foreach (var sw in this.SwitchList)
            {
                sws.Add(new XElement("Sw",
                    new XAttribute("Category", sw.Category),
                    new XAttribute("Type", sw.Type)
                    ));
            }
            //找到Links元素,添加Link集合到该元素下面
            XElement links = rt.Element("Links");
            foreach (var link in this.LinkList)
            {
                links.Add(new XElement("Link",
                    new XAttribute("Type", link.LinkType.ToString()),
                    new XAttribute("FirstEndType", link.FirstEndType.ToString()),
                    new XAttribute("FirstEndId", link.FirstEndId.ToString()),
                    new XAttribute("SecondEndType", link.SecondEndType.ToString()),
                    new XAttribute("SecondEndId", link.SecondEndId.ToString()),
                    new XElement("FirstEndPositionList"),
                    new XElement("SecondEndPositionList")
                    ));
                //找到FirstEndPositionList元素，在其下添加Position的集合
                XElement fpositionList = links.Elements("Link").Last().Element("FirstEndPositionList");
                foreach (var p in link.FirstEndPositionList)
                {
                    fpositionList.Add(new XElement("Position", p.ToString()));
                }
                //找到SecondEndPositionList元素，在其下添加Position的集合
                XElement spositionList = links.Elements("Link").Last().Element("SecondEndPositionList");
                foreach (var p in link.SecondEndPositionList)
                {
                    spositionList.Add(new XElement("Position", p.ToString()));
                }
            }
            xd.Save(xmlPath);
        }

        public ModelBase CreateObjectByName(string objectName)
        {
            Board retBoard = new Board();
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetBoardPath(), objectName);
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("CreateObject_Board:没有该Board对应的XML文件！");
                return null;
            }
            XDocument xd = XDocument.Load(xmlPath);
            //根元素的Attribute
            XElement rt = xd.Element("Board");
            retBoard.Name = rt.Attribute("Name").Value;
            retBoard.Type = rt.Attribute("Type").Value;
            retBoard.Version = rt.Attribute("Version").Value;
            //找到Devices元素下的PPCs元素，添加PPC集合到类
            XElement ppcs = rt.Element("Devices").Element("PPCs");
            foreach (var element in ppcs.Elements())
            {
                retBoard.PPCList.Add(ModelFactory<PPC>.CreateByName(element.Attribute("Name").Value));
            }
            //找到Devices元素下的FPGAs元素，添加FPGA集合到类
            XElement fpgas = rt.Element("Devices").Element("FPGAs");
            foreach (var element in fpgas.Elements())
            {
                retBoard.FPGAList.Add(ModelFactory<FPGA>.CreateByName(element.Attribute("Name").Value));
            }
            //找到Devices元素下的ZYNQs元素，添加ZYNQ集合到类
            XElement zynqs = rt.Element("Devices").Element("ZYNQs");
            foreach (var element in zynqs.Elements())
            {
                retBoard.ZYNQList.Add(ModelFactory<ZYNQ>.CreateByName(element.Attribute("Name").Value));
            }
            //找到Devices元素下的Sws元素，添加Sw集合到类
            XElement sws = rt.Element("Devices").Element("Sws");
            foreach (var element in sws.Elements())
            {
                var category = (SwitchCategory)Enum.Parse(typeof(SwitchCategory), element.Attribute("Category").Value);
                var sw = new SwitchDevice(category, element.Attribute("Type").Value);
                retBoard.SwitchList.Add(sw);
            }
            //找到Links元素,添加Link集合到类
            XElement links = rt.Element("Links");
            foreach (var element in links.Elements())
            {
                var link = new BoardLink();
                link.LinkType = (LinkType)Enum.Parse(typeof(LinkType), element.Attribute("Type").Value);
                link.FirstEndType = (EndType)Enum.Parse(typeof(EndType), element.Attribute("FirstEndType").Value);
                link.FirstEndId = int.Parse(element.Attribute("FirstEndId").Value);
                link.SecondEndType = (EndType)Enum.Parse(typeof(EndType), element.Attribute("SecondEndType").Value);
                link.SecondEndId = int.Parse(element.Attribute("SecondEndId").Value);
                foreach (var fPosition in element.Element("FirstEndPositionList").Elements())
                {
                    link.FirstEndPositionList.Add(int.Parse(fPosition.Value));
                }
                foreach (var sPosition in element.Element("SecondEndPositionList").Elements())
                {
                    link.SecondEndPositionList.Add(int.Parse(sPosition.Value));
                }
                retBoard.LinkList.Add(link);
            }
            return retBoard;
        }

        /// <summary>
        /// 判断一条底板连接的Link是否有效地连接到了板子的VPX的有效位置及区域类型
        /// </summary>
        /// <param name="bLink"></param>
        /// <param name="endPosition">需判断的Link连接的端点位置：1，2</param>
        /// <returns></returns>
        public bool IsLinkValidConnected(BackPlaneLink bLink, int endPosition)
        {
            int linkPostion = ((endPosition == 1) ? bLink.FirstEndPostion : bLink.SecondEndPostion);
            var validLinks = from link in LinkList
                             where link.LinkType == bLink.LinkType &&  //类型相同
                             ((link.FirstEndType == EndType.VPX && link.FirstEndPositionList.Contains(linkPostion))//第1端为vpx且包含相关位置；
                             || (link.SecondEndType == EndType.VPX && link.SecondEndPositionList.Contains(linkPostion)))//第2端为vpx且包含相关位置；
                             select link;
            if (validLinks.ToList().Count == 0)
            {
                return false;
            }
            return true;
        }

    }
}
