using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using DRSysCtrlDisplay.Princeple;
using System.Windows.Forms;

namespace DRSysCtrlDisplay.OtherView
{
    /// <summary>
    /// 一个Vpx上的通信区的描述
    /// </summary>
    public class VpxEndAreaView : BaseView
    {
        public LinkType LinkType { get; private set; }    //该vpx连接区对应的连接类型
        public VpxCategory Type { get; private set; }               //该vpx的种类

        public VpxEndAreaView(string name, LinkType linkType, VpxCategory type)
        {
            base.Name = name;
            LinkType = linkType;
            Type = type;
        }

        public override void DrawView(Graphics g) { }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            Pen pen;
            Brush brush;
            switch (this.LinkType)
            {
                case LinkType.LVDS:
                    pen = Princeple.ConnectAreaColor.Pen_LVDS;
                    brush = Princeple.ConnectAreaColor.Brushes_LVDS;
                    break;
                case LinkType.GTX:
                    pen = Princeple.ConnectAreaColor.Pen_GTX;
                    brush = Princeple.ConnectAreaColor.Brushes_GTX;
                    break;
                case LinkType.RapidIO:
                    pen = Princeple.ConnectAreaColor.Pen_RapidIO;
                    brush = Princeple.ConnectAreaColor.Brushes_RapidIO;
                    break;
                case LinkType.EtherNet:
                    pen = Princeple.ConnectAreaColor.Pen_EtherNet;
                    brush = Princeple.ConnectAreaColor.Brushes_EtherNet;
                    break;
                default://其他类型
                    pen = Pens.Gray;
                    brush = Brushes.Gray;
                    break;
            }
            g.DrawRectangle(pen, rect);
            g.FillRectangle(brush, rect);
        }

        public void DrawViewColor(Graphics g, Rectangle rect, Pen pen, Brush brush)
        {
            g.DrawRectangle(pen, rect);
            g.FillRectangle(brush, rect);
        }
    }

    #region Vpx连接端相关图像类

    /// <summary>
    /// 描述整个VPX区域
    /// </summary>
    [TypeConverter(typeof(VpxEndViewTypeConverter))]
    public abstract class VpxEndView : BaseView, IDrawer
    {
        [BrowsableAttribute(false)]
        protected const int InfoAreasNum = 4;

        [BrowsableAttribute(false)]
        protected Pen DefaultPen = Pens.Gray;

        [BrowsableAttribute(false)]
        protected Brush DefaultBrush = Brushes.Gray;

        [BrowsableAttribute(false)]
        protected Rectangle _rect;  //所在矩形

        [BrowsableAttribute(false)]
        public Rectangle[] _infoAreaRects = new Rectangle[InfoAreasNum];

        [Category("\t基本信息"), Description("板卡类型")]
        public VpxCategory Type { get; private set; }               //该vpx的种类

        [Category("以太网信息"), Description("以太网区域信息")]
        public VpxEndAreaView EthArea { get; private set; }

        [Category("RapidIO信息"), Description("RapidIO区域信息")]
        public VpxEndAreaView RioArea { get; private set; }

        [Category("GTX信息"), Description("GTX区域信息")]
        public VpxEndAreaView GtxArea { get; private set; }

        [Category("LVDS信息"), Description("LVDS区域信息")]
        public VpxEndAreaView LvdsArea { get; private set; }

        protected VpxEndView(string name, VpxCategory type)
        {
            base.Name = name;
            this.Type = type;
            EthArea = new VpxEndAreaView("EthArea", LinkType.EtherNet, type);
            RioArea = new VpxEndAreaView("RioArea", LinkType.RapidIO, type);
            GtxArea = new VpxEndAreaView("GtxArea", LinkType.GTX, type);
            LvdsArea = new VpxEndAreaView("LvdsArea", LinkType.LVDS, type);
        }

        //画出该View的图像，不使用
        public override void DrawView(Graphics g) { }

        //画出该View的图像
        public override void DrawView(Graphics g, Rectangle rect) { }

        //画出该View被选中时候的图像
        public override void ChoosedDrawView(Graphics g, Rectangle rect) { }

        //分配该view多对应的矩形
        public virtual void AssignRect(Rectangle rect) { }

        //获取该view对应的信息区域的矩形
        protected virtual Rectangle GetWholeInfoAreaRect() { return new Rectangle(0, 0, 0, 0); }

        public virtual BaseView GetChoosedBaseView(MouseEventArgs e){ return null; }

        public virtual Rectangle GetBaseViewRect(BaseView baseView, ref bool isFind)
        {
            isFind = false;
            return new Rectangle(0,0,0,0);
        }

    }

    public class VpxEndViewTypeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            BaseView bv = value as BaseView;

            return bv.Name;
        }
    }

    //面板(机箱，背板)上的Vpx区域
    public class PlaneVpx : VpxEndView
    {
        public Rectangle _planeAreaRect;
        public Rectangle _nameAreaRect;

        public PlaneVpx(string name, VpxCategory type) : base(name, type) { }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            //画通信区域的图像
            base.EthArea.DrawView(g, base._infoAreaRects[0]);
            base.RioArea.DrawView(g, base._infoAreaRects[1]);
            base.GtxArea.DrawView(g, base._infoAreaRects[2]);
            base.LvdsArea.DrawView(g, base._infoAreaRects[3]);

            //画底板和名称区域的图像
            g.DrawRectangle(base.DefaultPen, this._planeAreaRect);
            g.FillRectangle(base.DefaultBrush, this._planeAreaRect);

            g.DrawRectangle(base.DefaultPen, this._nameAreaRect);
            g.FillRectangle(base.DefaultBrush, this._nameAreaRect);
        }

        public override void ChoosedDrawView(Graphics g, Rectangle rect)
        {
            DrawView(g, rect);
            g.DrawRectangle(Pens.Red, _rect);
        }

        public override void AssignRect(Rectangle rect)
        {
            int rectY = 0;
            base._rect = rect;

            //给信息区分配子矩形
            int rectX = rect.Location.X + rect.Size.Width / InfoAreasNum;
            int rectWidth = rect.Size.Width / 2;
            int rectHeight = rect.Size.Height / (InfoAreasNum + 1);
            for (int i = 0; i < InfoAreasNum; i++)
            {
                rectY = rect.Location.Y + rectHeight * i;
                base._infoAreaRects[i] = new Rectangle(rectX, rectY, rectWidth, rectHeight);
            }

            //分配背板区矩形
            rectX = rect.X;
            rectY = rectY + rectHeight;
            rectWidth = rect.Width;
            rectHeight = rectHeight / 2;
            this._planeAreaRect = new Rectangle(rectX, rectY, rectWidth, rectHeight);

            //分配名称区矩形
            rectX = rect.X + rect.Width * 3 / 8;
            rectY = rectY + rectHeight;
            rectWidth = rect.Width / 4;
            //rectHeight不变
            this._nameAreaRect = new Rectangle(rectX, rectY, rectWidth, rectHeight);
        }

        protected override Rectangle GetWholeInfoAreaRect()
        {
            Point p = _infoAreaRects[0].Location;
            int width = _infoAreaRects[0].Width;
            int height = _infoAreaRects[0].Height + _infoAreaRects[1].Height + _infoAreaRects[2].Height + _infoAreaRects[3].Height;
            return new Rectangle(p.X, p.Y, width, height);
        }

        public override BaseView GetChoosedBaseView(MouseEventArgs e) { return this; }

        public override Rectangle GetBaseViewRect(BaseView baseView, ref bool isFind)
        {
            return base.GetBaseViewRect(baseView, ref isFind);
        }
    }

    /// <summary>
    /// 背板槽位图像类(也包含外接口区)
    /// </summary>
    public class BackPlaneVpx : PlaneVpx
    {
        public BackPlaneVpx(string name): base(name, VpxCategory.BackPlane){}
    }

    /// <summary>
    /// 机箱里的空槽位图像
    /// </summary>
    public class EmptySlotVpx : PlaneVpx
    {
        public EmptySlotVpx(string name) : base(name, VpxCategory.EmptySlot) { }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            //画通信区域的图像
            base.EthArea.DrawViewColor(g, base._infoAreaRects[0], base.DefaultPen, base.DefaultBrush);
            base.RioArea.DrawViewColor(g, base._infoAreaRects[1], base.DefaultPen, base.DefaultBrush);
            base.GtxArea.DrawViewColor(g, base._infoAreaRects[2], base.DefaultPen, base.DefaultBrush);
            base.LvdsArea.DrawViewColor(g, base._infoAreaRects[3], base.DefaultPen, base.DefaultBrush);

            //画底板和名称区域的图像
            g.DrawRectangle(base.DefaultPen, _planeAreaRect);
            g.FillRectangle(base.DefaultBrush, _planeAreaRect);

            g.DrawRectangle(base.DefaultPen, _nameAreaRect);
            g.FillRectangle(base.DefaultBrush, _nameAreaRect);
        }
    }

    /// <summary>
    /// 机箱里的板卡图像
    /// </summary>
    public class BoardVpx : PlaneVpx
    {
        public BoardVpx(string name) : base(name, VpxCategory.Board) { }

        public override void ChoosedDrawView(Graphics g, Rectangle rect)
        {
            DrawView(g, rect);
            g.DrawRectangle(Pens.Red, base.GetWholeInfoAreaRect());
        }

        public override BaseView GetChoosedBaseView(MouseEventArgs e)
        {
            if (base.GetWholeInfoAreaRect().Contains(e.Location))
            {
                return this;
            }
            return null;
        }
    }

    /// <summary>
    /// 背板示意区图像类
    /// </summary>
    public class IndicateAreaVpx : PlaneVpx
    {
        public IndicateAreaVpx(string name) : base(name, VpxCategory.IndicateArea) { }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            //画通信区域的图像
            EthArea.DrawView(g, _infoAreaRects[0]);
            BaseView.AddDircSentence(g, _infoAreaRects[0], "EtherNet", false);

            RioArea.DrawView(g, _infoAreaRects[1]);
            BaseView.AddDircSentence(g, _infoAreaRects[1], "RapidIO", false);

            GtxArea.DrawView(g, _infoAreaRects[2]);
            BaseView.AddDircSentence(g, _infoAreaRects[2], "GTX", false);

            LvdsArea.DrawView(g, _infoAreaRects[3]);
            BaseView.AddDircSentence(g, _infoAreaRects[3], "LVDS", false);

        }
    }

#endregion

}
