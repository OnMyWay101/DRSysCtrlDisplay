using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing.Design;
using System.Xml.Linq;
using System.IO;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.OtherView;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    public class BackPlane : BaseView
    {
        [Category("\t基本信息"), Description("背板类型")]
        public String Type { get; set; }

        [BrowsableAttribute(false)]
        public int VirtualSlotsNum { get; private set; }

        [Category("\t基本信息"), Description("背板槽位数")]
        public int SlotsNum { get; private set; }                       //可以插板卡的槽位数

        [Category("连接信息"), Description("各槽位的连接信息")]
        public List<BackPlaneLink>[] LinksArray { get; set; }

        //包含的子视图，比槽位多2个，其中：n-1为连接示意区；n-2为外接口区
        [Category("槽位信息"), Description("机箱的各槽位的信息")]
        public VpxEndView[] VpxEndViewArray { get; set; }

        [BrowsableAttribute(false)]
        public DrawBackPlane _drawBackPlane { get; private set; }

        [BrowsableAttribute(false)]
        public BaseView ChoosedBv { get; private set; }    //当前视图被选中的图元


        public BackPlane() { }

        public BackPlane(int slotNum)
        {
            SlotsNum = slotNum;
            VirtualSlotsNum = slotNum + 2;
            LinksArray = new List<BackPlaneLink>[VirtualSlotsNum];
            InitVpxEndViews();
        }

        public override void DrawView(Graphics g){ }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            InitDrawBackPlane(g, rect);
            _drawBackPlane.Draw();
        }

        public void DrawViewNoIndicate(Graphics g, Rectangle rect)
        {
            InitDrawBackPlane(g, rect);
            _drawBackPlane.NoIndicate = true;
            _drawBackPlane.Draw();
        }

        public override Size GetViewSize()
        {
            return new Size(800, 400);
        }

        public override void SaveXmlByName()
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

        public override BaseView CreateObjectByName(string objectName)
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
                List<BackPlane.BackPlaneLink> linksList = new List<BackPlane.BackPlaneLink>();
                //找到同一槽位的links，然后添加到list
                var slotLinks = from link in links.Elements()
                                where int.Parse(link.Attribute("FirstEndId").Value) == i
                                select link;
                foreach (var link in slotLinks)
                {
                    LinkType type = (LinkType)Enum.Parse(typeof(LinkType), link.Attribute("Type").Value);

                    var tempLink = new BackPlane.BackPlaneLink(i, int.Parse(link.Attribute("FirstEndPos").Value)
                        , int.Parse(link.Attribute("SecondEndId").Value), int.Parse(link.Attribute("SecondEndPos").Value), type);
                    linksList.Add(tempLink);
                }
                backPlane.LinksArray[i] = linksList;
            }
            return backPlane;
        }

        public override void MouseEventHandler(object sender, MouseEventArgs e)
        {
            ChoosedBv = _drawBackPlane.GetChoosedBaseView(e);
            if (ChoosedBv != null)
            {
                PropertyForm.Show(ChoosedBv);
            }
            else
            {
                ChoosedBv = this;
            }
            base.TriggerRedrawRequst();
        }

        private void InitVpxEndViews()
        {
            VpxEndViewArray = new VpxEndView[VirtualSlotsNum];
            for (int i = 0; i < VirtualSlotsNum; i++)
            {
                if (i == VirtualSlotsNum - 1)//第n-1个为连接示意区
                {
                    VpxEndViewArray[i] = new IndicateAreaVpx("连接示意区");
                }
                else if (i == VirtualSlotsNum - 2)//第n-2个为外连口区
                {
                    VpxEndViewArray[i] = new BackPlaneVpx("外连口区");
                }
                else
                {
                    VpxEndViewArray[i] = new BackPlaneVpx("槽位" + (i + 1));
                }
            }
        }

        public Point GetInterfacePoint(LinkType type)
        {
            try
            {
                var vpxEndView = this.VpxEndViewArray[VirtualSlotsNum - 2];
                //找到对应示意区的矩形
                var areaRect = (type == LinkType.EtherNet) ? vpxEndView._infoAreaRects[0] : vpxEndView._infoAreaRects[1];
                int pointX = areaRect.X + areaRect.Width / 2;
                int pointY = areaRect.Y + areaRect.Height / 2;
                return new Point(pointX, pointY);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("GetInterfacePoint:" + ex.Message);
                return new Point(0, 0);
            }
        }

        /// <summary>
        /// 判断该虚拟槽位是否为连接区
        /// </summary>
        /// <param name="virtualSlotId"></param>
        /// <returns></returns>
        public bool IsConnetctAreaSlot(int virtualSlotId)
        {
            return virtualSlotId == (this.VirtualSlotsNum - 2);
        }

        public void InitDrawBackPlane(Graphics g, Rectangle rect)
        {
            _drawBackPlane = new DrawBackPlane(this, g, rect);
        }



        /// <summary>
        /// 用于背板显示的画图类
        /// </summary>
        public class DrawBackPlane : IDrawer
        {
            BackPlane _backPlane;                       //需要画图的背板
            protected Graphics _graph;                  //背板对应的画布
            Rectangle _bpRect;                          //背板的边框
            public Rectangle[] SlotRects { get; private set; }  		                //包含的槽位图像矩形位置集合；
            public Dictionary<BackPlaneLink, Point[]> LinkDir { get; private set; }     //包含的连接及对应的点
            public Boolean NoIndicate { get; set; }     //不画连接示意区标志

            public DrawBackPlane(BackPlane bp, Graphics g, Rectangle r)
            {
                _backPlane = bp;
                _graph = g;
                NoIndicate = false;
                int newRectWidth = r.Width * _backPlane.VirtualSlotsNum / 12;
                _bpRect = new Rectangle(r.X + (r.Width - newRectWidth) / 2, r.Y, newRectWidth, r.Height);
                SlotRects = new Rectangle[_backPlane.VpxEndViewArray.Length];
                AssignSloteRects();
                LinkDir = new Dictionary<BackPlaneLink, Point[]>();
                InitLinks();
            }

            public BaseView GetChoosedBaseView(MouseEventArgs e)
            {
                for (int i = 0; i < SlotRects.Length; i++)
                {
                    //矩形是否包含点
                    if (SlotRects[i].Contains(e.Location))
                    {
                        return _backPlane.VpxEndViewArray[i].GetChoosedBaseView(e);
                    }
                }
                return null;
            }
            public Rectangle GetBaseViewRect(BaseView baseView, ref bool isFind)
            {
                for (int i = 0; i < SlotRects.Length; i++)
                {
                    BaseView bv = _backPlane.VpxEndViewArray[i];
                    if (baseView != bv)
                    {
                        isFind = true;
                        return SlotRects[i];
                    }
                }
                isFind = false;
                return new Rectangle(0, 0, 0, 0);
            }

            public void Draw()
            {
                var choosedBv = _backPlane.ChoosedBv;

                DrawBoundary();

                //画槽位
                for (int i = 0; i < SlotRects.Length; i++)
                {
                    if (!NoIndicate || (i != SlotRects.Length - 1))//判断是否该画槽位，连接示意区在最后一个
                    {
                        BaseView bv = _backPlane.VpxEndViewArray[i];
                        bv.DrawView(_graph, SlotRects[i]);
                    }
                }
                //稍后画选中的图元
                if (choosedBv != null)
                {
                    bool isFind = false;
                    var choosedRect = GetBaseViewRect(choosedBv, ref isFind);
                    if (isFind)
                    {
                        choosedBv.ChoosedDrawView(_graph, choosedRect);
                    }
                }

                //画Links
                foreach (var linePair in LinkDir)
                {
                    var link = linePair.Key;
                    link.EndRadius = SlotRects[0].Width / 20;
                    link.DrawLine(_graph, linePair.Value.ToList());
                }
            }

            protected void DrawBoundary()
            {
                int margin = 10;
                _graph.DrawRectangle(Pens.Black, new Rectangle(_bpRect.Location.X - margin,
                    _bpRect.Location.Y - margin, _bpRect.Size.Width + 2 * margin, _bpRect.Size.Height + 2 * margin));
            }

            protected void DrawLine(Point p1, Point p2)
            {
                _graph.DrawLine(Pens.Black, p1, p2);
                int radius = SlotRects[0].Width / 20;   //半径值取区域矩形的宽度的1/20
                _graph.DrawEllipse(Pens.Black, p1.X - radius, p1.Y - radius, 2 * radius, 2 * radius);
                _graph.FillEllipse(Brushes.Black, p1.X - radius, p1.Y - radius, 2 * radius, 2 * radius);
                _graph.DrawEllipse(Pens.Black, p2.X - radius, p2.Y - radius, 2 * radius, 2 * radius);
                _graph.FillEllipse(Brushes.Black, p2.X - radius, p2.Y - radius, 2 * radius, 2 * radius);
            }

            protected void DrawLineWithColor(Point p1, Point p2, Pen pen, Brush brush)
            {
                _graph.DrawLine(pen, p1, p2);
                int radius = SlotRects[0].Width / 20;   //半径值取区域矩形的宽度的1/20
                _graph.DrawEllipse(pen, p1.X - radius, p1.Y - radius, 2 * radius, 2 * radius);
                _graph.FillEllipse(brush, p1.X - radius, p1.Y - radius, 2 * radius, 2 * radius);
                _graph.DrawEllipse(pen, p2.X - radius, p2.Y - radius, 2 * radius, 2 * radius);
                _graph.FillEllipse(brush, p2.X - radius, p2.Y - radius, 2 * radius, 2 * radius);
            }

            private void AssignSloteRects()
            {
                int num = SlotRects.Length;
                //把_bpRect均分为num个矩形框
                int rectHeight = _bpRect.Size.Height;
                int rectWidth = _bpRect.Size.Width / num;
                int rectY = _bpRect.Location.Y;
                int rectX = 0;
                for (int i = 0; i < num; i++)
                {
                    rectX = _bpRect.Location.X + rectWidth * i;
                    SlotRects[i] = new Rectangle(rectX, rectY, rectWidth, rectHeight);
                    _backPlane.VpxEndViewArray[i].AssignRect(SlotRects[i]);
                }
            }

            private void InitLinks()
            {
                //1.把背板对应的所有link放在一起
                List<BackPlaneLink> allLinks = new List<BackPlaneLink>();
                foreach (var linkList in _backPlane.LinksArray)
                {
                    if (linkList != null)
                    {
                        foreach (BackPlaneLink link in linkList)
                        {
                            allLinks.Add(link);
                        }
                    }
                }
                //2.给各个link分配相应的点
                PushLinksPoints(LinkDir, allLinks, LinkType.EtherNet);
                PushLinksPoints(LinkDir, allLinks, LinkType.RapidIO);
                PushLinksPoints(LinkDir, allLinks, LinkType.GTX);
                PushLinksPoints(LinkDir, allLinks, LinkType.LVDS);
            }

            /// <summary>
            /// 把背板Link集合当中的某种类型Link转化为points加入到Dir
            /// </summary>
            /// <param name="linkDir"></param>
            /// <param name="allLink"></param>
            /// <param name="type"></param>
            private void PushLinksPoints(Dictionary<BackPlaneLink, Point[]> linkDir, List<BackPlaneLink> allLink, LinkType type)
            {
                //1.找到相同类型link的个数
                int num = Enumerable.Count(allLink, (lnk) => lnk.LinkType == type);
                double linkYoffSetScale = 0;    //Link的Y刚对于起始位置偏移的比例（与区域Height）

                //2.把相同类型的link按照槽位号来分组，组的顺序是槽位号递增的
                var sameTypelinks = from lnk in allLink
                                    where lnk.LinkType == type
                                    orderby lnk.FirstEndId ascending
                                    group lnk by lnk.FirstEndId;    //得到一系列按照link.FirstEndId递增的group(同组的LinkEndId一样)
                //3.挨个处理每组里面的link，得到points
                foreach (var grp in sameTypelinks)
                {
                    //同组的link按照link.SecondEndId递增排序
                    var sloteLinks = from link in grp
                                     orderby link.SecondEndId ascending
                                     select link;
                    foreach (BackPlaneLink link in sloteLinks)
                    {
                        linkYoffSetScale += 1.0 / (num + 1);
                        linkDir.Add(link, GetLinkPoints(link, linkYoffSetScale));
                    }
                }
            }

            private Point[] GetLinkPoints(BackPlaneLink link, double linkYoffSetScale)
            {
                VpxEndView vev1 = _backPlane.VpxEndViewArray[link.FirstEndId];
                VpxEndView vev2 = _backPlane.VpxEndViewArray[link.SecondEndId];
                Rectangle r1;
                Rectangle r2;
                switch (link.LinkType)
                {
                    case LinkType.EtherNet:
                        r1 = vev1._infoAreaRects[0];
                        r2 = vev2._infoAreaRects[0];
                        break;
                    case LinkType.RapidIO:
                        r1 = vev1._infoAreaRects[1];
                        r2 = vev2._infoAreaRects[1];
                        break;
                    case LinkType.GTX:
                        r1 = vev1._infoAreaRects[2];
                        r2 = vev2._infoAreaRects[2];
                        break;
                    default://LVDS
                        r1 = vev1._infoAreaRects[3];
                        r2 = vev2._infoAreaRects[3];
                        break;
                }
                Point p1 = new Point(r1.Location.X + r1.Size.Width / 2, (int)(r1.Location.Y + r1.Size.Height - linkYoffSetScale * r1.Size.Height));
                Point p2 = new Point(r2.Location.X + r2.Size.Width / 2, p1.Y);
                return new Point[] { p1, p2 };
            }
        }
    }
}
