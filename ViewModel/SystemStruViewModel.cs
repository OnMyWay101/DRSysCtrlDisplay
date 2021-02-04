using System;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DRSysCtrlDisplay.Princeple;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    public class SystemStruViewModel : BaseView
    {
        public string Type { get; set; }                                //系统的型号
        public int CntsNum { get; set; }                                //系统包含机箱的个数
        public string[] CntNames { get; set; }                          //机箱名称数组
        public List<SystemStruLink>[] LinksArray { get; set; }          //各机箱的连接信息
        public ContainerViewModel[] CntsArray;                                   //机箱数组

        public DrawSystemStru _drawer;                                  //系统画图类

        public SystemStruViewModel() { }

        public SystemStruViewModel(int cntsNum)
        {
            CntsNum = cntsNum;
            CntNames = new string[CntsNum];
            LinksArray = new List<SystemStruLink>[CntsNum];
            CntsArray = new ContainerViewModel[cntsNum];
        }

        public override void DrawView(Graphics g) {}

        public override void DrawView(Graphics g, Rectangle rect)
        {
            _drawer = new DrawSystemStru(this, g, rect);
            _drawer.Draw();
        }

        public override Size GetViewSize()
        {
            int width = 800;    //宽度为固定值800
            int height = 400 * CntsNum;//高度为一个机箱400，机箱数*400
            return new Size(width, height);
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

        public override BaseView CreateObjectByName(string objectName)
        {
            SystemStruViewModel sys;
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
            sys = new SystemStruViewModel(cntsNum);
            sys.Name = rt.Attribute("Name").Value;
            sys.Type = rt.Attribute("Type").Value;

            //取CntNames的值赋值到CntsName,CntsArray
            XElement cntNames = rt.Element("CntNames");
            foreach (var e in cntNames.Elements())
            {
                int cntSn = int.Parse(e.Attribute("CntSn").Value);
                string cntName = e.Attribute("CntName").Value;
                sys.CntNames[cntSn] = cntName;
                sys.CntsArray[cntSn] = BaseViewFactory<ContainerViewModel>.CreateByName(sys.CntNames[cntSn]);
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

       

        /// <summary>
        /// 用于系统显示的画图类
        /// </summary>
        public class DrawSystemStru : IDrawer
        {
            SystemStruViewModel _sys;                        //需要画图的系统
            protected Graphics _graph;              //系统对应的画布
            Rectangle _sysRect;                     //系统的边框
            public Rectangle[] CntRects { get; private set; }  		                    //包含的机箱图像矩形位置集合；
            public Dictionary<SystemStruLink, Point[]> LinkDir { get; private set; }    //包含的连接及对应的点
            const double _cntScale = 9.0 / 10;      //机箱长度占单个矩形块的长宽比例

            public DrawSystemStru(SystemStruViewModel sys, Graphics g, Rectangle r)
            {
                _sys = sys;
                _graph = g;
                _sysRect = r;

                CntRects = new Rectangle[_sys.CntsNum];
                AssignCntRects();

                LinkDir = new Dictionary<SystemStruLink, Point[]>();
                InitLinks();
            }

            /// <summary>
            /// 通过_sysRect来给该图形的图元分配矩形容器
            /// 分配规则：
            /// 1)_sysRect是该系统对应的矩形外边框；该系统对应的机箱数量为CntsNum;
            /// 2)首先：机箱的排列方向按照垂直方向排列；按照垂直方向把_sysRext分为CntsNum份；每一份为_sysRectChild;
            /// 3）其次：一个_sysRectChild的包含一个cntRect,其中宽度占9/10；
            /// </summary>
            private void AssignCntRects()
            {
                int cntsNum = _sys.CntsNum;     //机箱的数量
                int childHeight = _sysRect.Height / cntsNum;

                //cntRect的高，宽，X坐标都恒定，Y会有变化
                int height = (int)(childHeight * _cntScale);
                int width = (int)(_sysRect.Width * _cntScale);
                int pointX = _sysRect.X;
                int pointY = _sysRect.Y + (int)(childHeight * (1 - _cntScale));

                for (int i = 0; i < cntsNum; i++)
                {
                    CntRects[i] = new Rectangle(pointX, pointY, width, height);
                    _sys.CntsArray[i].InitDrawContainer(_graph, CntRects[i]);//创建对应机箱的画图器
                    pointY += childHeight;
                }
            }

            /// <summary>
            /// 分配点的规则说明：
            /// 1）分配的link这里分为两大类：EtherNet，RapidIO;
            /// 2) 这些links在list的里面的排布是按照机箱号来递增的；
            /// 3）一条link对应有4个点：机1外连接区-》机1垂直走线区-》机2垂直走线区-》机2外连接区
            /// </summary>
            /// <param name="type"></param>
            /// <param name="links"></param>
            private void AssignLinksPoints(LinkType type, List<SystemStruLink> links)
            {
                int linksNum = links.Count;//连接的数量
                int pointXWidth = (int)(_sysRect.Width * (1 - _cntScale) / 2);      //pointX所在区域的宽度
                int areaBase = (int)(_sysRect.X + _sysRect.Width * _cntScale);      //走线区的起始位置
                int pointXBase = (type == LinkType.EtherNet ? areaBase : areaBase + pointXWidth);    //pointX的起始位置

                foreach (var link in links)
                {
                    ContainerViewModel cnt1 = _sys.CntsArray[link.FirstEndId];
                    ContainerViewModel cnt2 = _sys.CntsArray[link.SecondEndId];
                    double scale = (links.IndexOf(link) + 1.0) / (linksNum + 1.0);

                    Point p1 = cnt1._backPlane.GetInterfacePoint(type);
                    Point p4 = cnt2._backPlane.GetInterfacePoint(type);
                    Point p2 = new Point((int)(pointXBase + pointXWidth * scale), p1.Y);
                    Point p3 = new Point(p2.X, p4.Y);
                    LinkDir.Add(link, new Point[] { p1, p2, p3, p4});
                }
            }

            private void InitLinks()
            {
                //这里连接只用管网络连接：Rio,EtherNet。
                var linksArray = _sys.LinksArray;

                var ethLinks = new List<SystemStruLink>();
                var rioLinks = new List<SystemStruLink>();
                for (int i = 0; i < linksArray.Length; i++)
                {
                    var cntLinks = linksArray[i];
                    foreach (var link in cntLinks)
                    {
                        if (link.LinkType == LinkType.EtherNet)
                        {
                            ethLinks.Add(link);
                        }
                        else if (link.LinkType == LinkType.RapidIO)
                        {
                            rioLinks.Add(link);
                        }
                    }
                }
                AssignLinksPoints(LinkType.EtherNet, ethLinks);
                AssignLinksPoints(LinkType.RapidIO, rioLinks);
            }

            public void Draw()
            {
                //画机箱
                for (int i = 0; i < _sys.CntsNum; i++)
                {
                    var curRect = CntRects[i];
                    var curCnt = _sys.CntsArray[i];
                    curCnt.DrawViewNoIndicate(_graph, curRect);
                }
                //填入机箱名
                AddCntNames();
                //画连接线
                foreach (var linkPair in LinkDir)
                {
                    var link = linkPair.Key;
                    var points = linkPair.Value.ToList();
                    link.DrawLine(_graph, points);
                }
            }

            private void AddCntNames()
            {
                for (int i = 0; i < CntRects.Length; i++)
                {
                    var cntRect = CntRects[i];//当前的机箱视图
                    int nameRectWidth = cntRect.Width / 10;
                    var nameRect = new Rectangle(cntRect.X - nameRectWidth, cntRect.Y, nameRectWidth, cntRect.Height);
                    BaseView.AddDircSentence(_graph, nameRect, _sys.CntNames[i], false);
                }
            }

            public BaseView GetChoosedBaseView(MouseEventArgs e)
            {
                return null;
            }

            public Rectangle GetBaseViewRect(BaseView baseView, ref bool isFind)
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }
    }
}
