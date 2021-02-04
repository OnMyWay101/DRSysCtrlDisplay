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
    public class BackPlane : ModelBase, XMLManager.IXmlTransformByName
    {
        public BackPlane(int slotNum)
        {
            SlotsNum = slotNum;
            VirtualSlotsNum = slotNum + 2;
            LinksArray = new List<BackPlaneLink>[VirtualSlotsNum];
        }

        #region Board的基本属性
        [Category("\t基本信息"), Description("背板类型")]
        public String Type { get; set; }

        [BrowsableAttribute(false)]
        public int VirtualSlotsNum { get; private set; }

        [Category("\t基本信息"), Description("背板槽位数")]
        public int SlotsNum { get; private set; }                       //可以插板卡的槽位数

        [Category("连接信息"), Description("各槽位的连接信息")]
        public List<BackPlaneLink>[] LinksArray { get; set; }

        #endregion

        public void SaveXmlByName()
        {
            List<BackPlaneLink> savedLinks = new List<BackPlaneLink>(); //已经存入的连接
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetBackPlanePath(), this.Name);
            //先判断一些文件是否存在
            if (!PathManager.CheckFile(xmlPath))
            {
                return;
            }
            //保存XML文件
            XDocument xd = new XDocument(
                new XElement("BackPlane",
                    new XAttribute("Name", this.Name),
                    new XAttribute("Type", this.Type),
                    new XAttribute("SlotsNum", this.SlotsNum),
                    new XElement("Links")
                    )
                );
            XElement rt = xd.Element("BackPlane");
            XElement links = rt.Element("Links");
            foreach (var linkList in this.LinksArray)
            {
                if (linkList == null)
                {
                    continue;
                }
                foreach (var link in linkList)
                {
                    int equalNum = savedLinks.Where(lnk => BackPlaneLink.IsEqual(link, lnk)).Count();
                    if (equalNum == 0)//该条连接的等效连接没有被访问过
                    {
                        links.Add(new XElement("Link",
                            new XAttribute("FirstEndId", link.FirstEndId.ToString()),
                            new XAttribute("FirstEndPos", link.FirstEndPostion.ToString()),
                            new XAttribute("SecondEndId", link.SecondEndId.ToString()),
                            new XAttribute("SecondEndPos", link.SecondEndPostion.ToString()),
                            new XAttribute("Type", link.LinkType.ToString())
                            ));
                        savedLinks.Add(link);
                    }
                }
            }
            xd.Save(xmlPath);
        }

        public ModelBase CreateObjectByName(string objectName)
        {
            BackPlane backPlane;
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetBackPlanePath(), objectName);
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("CreateObject_BackPlane:没有该BackPlane对应的XML文件！");
                return null;
            }

            XDocument xd = XDocument.Load(xmlPath);
            //根元素的Attribute
            XElement rt = xd.Element("BackPlane");
            int slotsNum = int.Parse(rt.Attribute("SlotsNum").Value);
            backPlane = new BackPlane(slotsNum);
            backPlane.Name = rt.Attribute("Name").Value;
            backPlane.Type = rt.Attribute("Type").Value;

            //取links赋值到backPlane.linkDir
            XElement links = rt.Element("Links");
            for (int i = 0; i < backPlane.VirtualSlotsNum; i++)
            {
                List<BackPlaneLink> linksList = new List<BackPlaneLink>();
                //找到同一槽位的links，然后添加到list
                var slotLinks = from link in links.Elements()
                                where int.Parse(link.Attribute("FirstEndId").Value) == i
                                select link;
                foreach (var link in slotLinks)
                {
                    LinkType type = (LinkType)Enum.Parse(typeof(LinkType), link.Attribute("Type").Value);

                    var tempLink = new BackPlaneLink(i, int.Parse(link.Attribute("FirstEndPos").Value)
                        , int.Parse(link.Attribute("SecondEndId").Value), int.Parse(link.Attribute("SecondEndPos").Value), type);
                    linksList.Add(tempLink);
                }
                backPlane.LinksArray[i] = linksList;
            }
            return backPlane;
        }
    }
}
