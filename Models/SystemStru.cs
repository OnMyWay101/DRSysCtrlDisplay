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
    public class SystemStru : ModelBase
    {
        #region SystemStru的基本属性
        public string Type { get; set; }                                //系统的型号
        public int CntsNum { get; set; }                                //系统包含机箱的个数
        public string[] CntNames { get; set; }                          //机箱名称数组
        public List<SystemStruLink>[] LinksArray { get; set; }          //各机箱的连接信息
        #endregion SystemStru的基本属性

        public SystemStru(int cntsNum)
        {
            CntsNum = cntsNum;
            CntNames = new string[CntsNum];
            LinksArray = new List<SystemStruLink>[CntsNum];
        }

        public override void SaveXmlByName()
        {
            List<SystemStruLink> savedLinks = new List<SystemStruLink>(); //已经存入的连接
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetSysPath(), this.Name);

            //先判断一些文件是否存在
            if (!PathManager.CheckFile(xmlPath))
            {
                return;
            }
            //保存XML文件
            XDocument xd = new XDocument(
                new XElement("SystemStru",
                    new XAttribute("Name", this.Name),
                    new XAttribute("Type", this.Type),
                    new XAttribute("CntsNum", this.CntsNum),
                    new XElement("CntNames"),
                    new XElement("Links")
                    )
                );
            XElement rt = xd.Element("SystemStru"); //xml文件根节点

            //录入机箱集合
            XElement Cnts = rt.Element("CntNames"); //机箱名根节点
            for (int i = 0; i < CntNames.Length; i++)
            {
                Cnts.Add(new XElement("Container",
                    new XAttribute("CntSn", i.ToString()),
                    new XAttribute("CntName", CntNames[i].ToString())));
            }

            //录入连接集合
            XElement links = rt.Element("Links");   //连接根节点
            foreach (var linkList in this.LinksArray)
            {
                if (linkList == null)
                {
                    continue;
                }
                foreach (var link in linkList)
                {
                    int equalNum = savedLinks.Where(lnk => SystemStruLink.IsEqual(link, lnk)).Count();
                    if (equalNum == 0)//该条连接的等效连接没有被访问过
                    {
                        links.Add(new XElement("Link",
                            new XAttribute("FirstEndId", link.FirstEndId.ToString()),
                            new XAttribute("FirstEndPos", link.FirstEndPostion.ToString()),
                            new XAttribute("SecondEndId", link.SecondEndId.ToString()),
                            new XAttribute("SecondEndPos", link.SecondEndPostion.ToString()),
                            new XAttribute("Type", link.LinkType.ToString()),
                            new XAttribute("DataWidth", link.LanesNum.ToString())
                            ));
                        savedLinks.Add(link);
                    }
                }
            }
            xd.Save(xmlPath);
        }

        public override ModelBase CreateObjectByName(string objectName)
        {
            SystemStru sys;
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetSysPath(), objectName);
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("CreateObject_SystemStru:没有该SystemStru对应的XML文件！");
                return null;
            }

            XDocument xd = XDocument.Load(xmlPath);
            //根元素的Attribute
            XElement rt = xd.Element("SystemStru");
            int cntsNum = int.Parse(rt.Attribute("CntsNum").Value);
            sys = new SystemStru(cntsNum);
            sys.Name = rt.Attribute("Name").Value;
            sys.Type = rt.Attribute("Type").Value;

            //取CntNames的值赋值到CntsName
            XElement cntNames = rt.Element("CntNames");
            foreach (var e in cntNames.Elements())
            {
                int cntSn = int.Parse(e.Attribute("CntSn").Value);
                string cntName = e.Attribute("CntName").Value;
                sys.CntNames[cntSn] = cntName;
            }

            //取links赋值到backPlane.linkDir
            XElement links = rt.Element("Links");
            for (int i = 0; i < cntsNum; i++)
            {
                List<SystemStruLink> linksList = new List<SystemStruLink>();
                //找到同一槽位的links，然后添加到list
                var slotLinks = from link in links.Elements()
                                where int.Parse(link.Attribute("FirstEndId").Value) == i
                                select link;
                foreach (var link in slotLinks)
                {
                    LinkType type = (LinkType)Enum.Parse(typeof(LinkType), link.Attribute("Type").Value);
                    LinkLanes laneNum = (LinkLanes)Enum.Parse(typeof(LinkLanes), link.Attribute("DataWidth").Value);

                    var tempLink = new SystemStruLink(i
                        , int.Parse(link.Attribute("FirstEndPos").Value)
                        , int.Parse(link.Attribute("SecondEndId").Value)
                        , int.Parse(link.Attribute("SecondEndPos").Value)
                        , type
                        , laneNum);
                    linksList.Add(tempLink);
                }
                sys.LinksArray[i] = linksList;
            }
            return sys;
        }
    }
}
