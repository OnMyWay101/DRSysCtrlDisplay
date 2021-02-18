using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.OtherView;
using DRSysCtrlDisplay.ViewModel.Others;
using DRSysCtrlDisplay.Models;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    public class BackPlaneViewModel : BaseDrawer, IDrawerChoosed
    {
        public BackPlane _bp;                                                       //对应的背板实体
        public Rectangle[] SlotRects { get; private set; }                          //包含的槽位图像矩形位置集合；
        public PlaneVpx[] PlaneVpxArray { get; set; }                           //包含的子视图，比槽位多2个，其中：n-1为连接示意区；n-2为外接口区
        public Dictionary<BackPlaneLink, Point[]> LinkDir { get; private set; }     //包含的连接及对应的点
        public BaseDrawer ChoosedBv { get; set; }                                   //当前视图被选中的图元
        public BackPlaneViewModel(BackPlane bp, Graphics g, Rectangle rect)
            : base(g, rect)
        {
            //初始化基类的矩形
            int newRectWidth = rect.Width * bp.VirtualSlotsNum / 12;
            base._rect = new Rectangle(rect.X + (rect.Width - newRectWidth) / 2, rect.Y, newRectWidth, rect.Height);
            //槽位矩形的分配
            SlotRects = new Rectangle[bp.VirtualSlotsNum];
            AssignSloteRects();

            InitVpxEndViews();
            LinkDir = new Dictionary<BackPlaneLink, Point[]>();
            InitLinks();
        }

        #region 重载虚函数
        public override void DrawView() 
        {
            DrawBoundary();
            //画槽位
            for (int i = 0; i < SlotRects.Length; i++)
            {
                BaseDrawer bv = PlaneVpxArray[i];
                bv.DrawView();
            }
            //稍后画选中的图元
            if (ChoosedBv != null)
            {
                ChoosedBv.ChoosedDrawView();
            }

            //画Links
            foreach (var linePair in LinkDir)
            {
                var link = linePair.Key;
                link.EndRadius = SlotRects[0].Width / 20;
                link.DrawLine(_graph, linePair.Value.ToList());
            }
        }

        public override Size GetViewSize()
        {
            return new Size(800, 400);
        }
        #endregion 重载虚函数

        #region 实现接口
        public void MouseEventHandler(object sender, MouseEventArgs e)
        {
            ChoosedBv = GetChoosedBaseView(e);
            if (ChoosedBv != null)
            {
                PropertyForm.Show(ChoosedBv);
            }
            else
            {
                PropertyForm.Show(this);
            }
            base.TriggerRedrawRequst();
        }

        public BaseDrawer GetChoosedBaseView(MouseEventArgs e)
        {
            for (int i = 0; i < SlotRects.Length; i++)
            {
                //矩形是否包含点
                if (SlotRects[i].Contains(e.Location))
                {
                    return PlaneVpxArray[i];
                }
            }
            return null;
        }
        #endregion 实现接口

        private void InitVpxEndViews()
        {
            PlaneVpxArray = new PlaneVpx[_bp.VirtualSlotsNum];
            for (int i = 0; i < _bp.VirtualSlotsNum; i++)
            {
                if (i == _bp.VirtualSlotsNum - 1)//第n-1个为连接示意区
                {
                    PlaneVpxArray[i] = new IndicateAreaVpx(base._graph, SlotRects[i], "连接示意区");
                }
                else if (i == _bp.VirtualSlotsNum - 2)//第n-2个为外连口区
                {
                    PlaneVpxArray[i] = new BackPlaneVpx(base._graph, SlotRects[i], "外连口区");
                }
                else
                {
                    PlaneVpxArray[i] = new BackPlaneVpx(base._graph, SlotRects[i], "槽位" + (i + 1));
                }
            }
        }

        public Point GetInterfacePoint(LinkType type)
        {
            try
            {
                var vpxEndView = this.PlaneVpxArray[_bp.VirtualSlotsNum - 2];
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
        /// 画背板最外面的边框
        /// </summary>
        protected void DrawBoundary()
        {
            int margin = 10;
            _graph.DrawRectangle(Pens.Black, new Rectangle(base._rect.Location.X - margin,
                base._rect.Location.Y - margin, base._rect.Size.Width + 2 * margin, base._rect.Size.Height + 2 * margin));
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
            int rectHeight = base._rect.Size.Height;
            int rectWidth = base._rect.Size.Width / num;
            int rectY = base._rect.Location.Y;
            int rectX = 0;
            for (int i = 0; i < num; i++)
            {
                rectX = base._rect.Location.X + rectWidth * i;
                SlotRects[i] = new Rectangle(rectX, rectY, rectWidth, rectHeight);
            }
        }

        private void InitLinks()
        {
            //1.把背板对应的所有link放在一起
            List<BackPlaneLink> allLinks = new List<BackPlaneLink>();
            foreach (var linkList in _bp.LinksArray)
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
            PlaneVpx vev1 = PlaneVpxArray[link.FirstEndId];
            PlaneVpx vev2 = PlaneVpxArray[link.SecondEndId];
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
