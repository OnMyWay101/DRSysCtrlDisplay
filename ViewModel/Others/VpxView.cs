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
    public class VpxEndAreaView : BaseDrawer
    {
        public LinkType LinkType { get; private set; }              //该vpx连接区对应的连接类型
        public VpxCategory Type { get; private set; }               //该vpx的种类

        public VpxEndAreaView(Graphics g, Rectangle rect, LinkType linkType, VpxCategory type)
            : base(g, rect)
        {
            LinkType = linkType;
            Type = type;
        }

        public override void DrawView()
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
            base._graph.DrawRectangle(pen, base._rect);
            base._graph.FillRectangle(brush, base._rect);
        }

        public void DrawViewColor(Pen pen, Brush brush)
        {
            base._graph.DrawRectangle(pen, base._rect);
            base._graph.FillRectangle(brush, base._rect);
        }
    }

    #region Vpx连接端相关图像类

    /// <summary>
    /// 描述整个VPX区域
    /// </summary>
    [TypeConverter(typeof(VpxEndViewTypeConverter))]
    public abstract class VpxEndView : BaseDrawer
    {
        [BrowsableAttribute(false)]
        protected const int InfoAreasNum = 4;

        [BrowsableAttribute(false)]
        protected Pen DefaultPen = Pens.Gray;

        [BrowsableAttribute(false)]
        protected Brush DefaultBrush = Brushes.Gray;

        [Category("\t基本信息"), Description("名称")]
        public string Name { get; private set; }

        [Category("\t基本信息"), Description("类型")]
        public VpxCategory Type { get; private set; }               //该vpx的种类

        [Category("以太网信息"), Description("以太网区域信息")]
        public VpxEndAreaView EthArea { get; protected set; }

        [Category("RapidIO信息"), Description("RapidIO区域信息")]
        public VpxEndAreaView RioArea { get; protected set; }

        [Category("GTX信息"), Description("GTX区域信息")]
        public VpxEndAreaView GtxArea { get; protected set; }

        [Category("LVDS信息"), Description("LVDS区域信息")]
        public VpxEndAreaView LvdsArea { get; protected set; }

        protected VpxEndView(Graphics g, Rectangle rect, string name, VpxCategory type)
            : base(g, rect)
        {
            Name = name;
            this.Type = type;
        }

        //画出该View的图像
        public override void DrawView() { }

        //获取该view对应的信息区域的矩形
        protected virtual Rectangle GetWholeInfoAreaRect() { return new Rectangle(0, 0, 0, 0); }
    }

    public class VpxEndViewTypeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            VpxEndView vev = value as VpxEndView;

            return vev.Name;
        }
    }

    //面板(机箱，背板)上的Vpx区域
    public class PlaneVpx : VpxEndView
    {
        public Rectangle _planeAreaRect;
        public Rectangle _nameAreaRect;
        public Rectangle[] _infoAreaRects;

        public PlaneVpx(Graphics g, Rectangle rect, string name, VpxCategory type)
            : base(g, rect, name, type) 
        {
            AssignRect();
            base.EthArea = new VpxEndAreaView(g, _infoAreaRects[0], LinkType.EtherNet, type);
            base.RioArea = new VpxEndAreaView(g, _infoAreaRects[1], LinkType.RapidIO, type);
            base.GtxArea = new VpxEndAreaView(g, _infoAreaRects[2], LinkType.GTX, type);
            base.LvdsArea = new VpxEndAreaView(g, _infoAreaRects[3], LinkType.LVDS, type);
        }

        public override void DrawView()
        {
            //画通信区域的图像
            base.EthArea.DrawView();
            base.RioArea.DrawView();
            base.GtxArea.DrawView();
            base.LvdsArea.DrawView();

            //画底板和名称区域的图像
            base._graph.DrawRectangle(base.DefaultPen, this._planeAreaRect);
            base._graph.FillRectangle(base.DefaultBrush, this._planeAreaRect);

            base._graph.DrawRectangle(base.DefaultPen, this._nameAreaRect);
            base._graph.FillRectangle(base.DefaultBrush, this._nameAreaRect);
        }

        public override void ChoosedDrawView()
        {
            DrawView();
            base._graph.DrawRectangle(Pens.Red, _rect);
        }

        public void AssignRect()
        {
            int rectY = 0;
            var rect = base._rect;

            _infoAreaRects = new Rectangle[VpxEndView.InfoAreasNum];
            //给信息区分配子矩形
            int rectX = rect.Location.X + rect.Size.Width / InfoAreasNum;
            int rectWidth = rect.Size.Width / 2;
            int rectHeight = rect.Size.Height / (InfoAreasNum + 1);
            for (int i = 0; i < InfoAreasNum; i++)
            {
                rectY = rect.Location.Y + rectHeight * i;
                _infoAreaRects[i] = new Rectangle(rectX, rectY, rectWidth, rectHeight);
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
    }

    /// <summary>
    /// 背板槽位图像类(也包含外接口区)
    /// </summary>
    public class BackPlaneVpx : PlaneVpx
    {
        public BackPlaneVpx(Graphics g, Rectangle rect, string name)
            : base(g, rect, name, VpxCategory.BackPlane)
        { }
    }

    /// <summary>
    /// 机箱里的空槽位图像
    /// </summary>
    public class EmptySlotVpx : PlaneVpx
    {
        public EmptySlotVpx(Graphics g, Rectangle rect, string name)
            : base(g, rect, name, VpxCategory.EmptySlot)
        { }

        public override void DrawView()
        {
            //画通信区域的图像
            base.EthArea.DrawViewColor(base.DefaultPen, base.DefaultBrush);
            base.RioArea.DrawViewColor(base.DefaultPen, base.DefaultBrush);
            base.GtxArea.DrawViewColor(base.DefaultPen, base.DefaultBrush);
            base.LvdsArea.DrawViewColor(base.DefaultPen, base.DefaultBrush);

            //画底板和名称区域的图像
            base._graph.DrawRectangle(base.DefaultPen, _planeAreaRect);
            base._graph.FillRectangle(base.DefaultBrush, _planeAreaRect);

            base._graph.DrawRectangle(base.DefaultPen, _nameAreaRect);
            base._graph.FillRectangle(base.DefaultBrush, _nameAreaRect);
        }
    }

    /// <summary>
    /// 机箱里的板卡图像
    /// </summary>
    public class BoardVpx : PlaneVpx
    {
        public BoardVpx(Graphics g, Rectangle rect, string name)
            : base(g, rect, name, VpxCategory.Board)
        { }

        public override void ChoosedDrawView()
        {
            DrawView();
            base._graph.DrawRectangle(Pens.Red, base.GetWholeInfoAreaRect());
        }
    }

    /// <summary>
    /// 背板示意区图像类
    /// </summary>
    public class IndicateAreaVpx : PlaneVpx
    {
        public IndicateAreaVpx(Graphics g, Rectangle rect, string name) 
            : base(g, rect, name, VpxCategory.IndicateArea) 
        { }

        public override void DrawView()
        {
            //画通信区域的图像
            EthArea.DrawView();
            BaseDrawer.AddDirctionSentence(base._graph, _infoAreaRects[0], "EtherNet", false);

            RioArea.DrawView();
            BaseDrawer.AddDirctionSentence(base._graph, _infoAreaRects[1], "RapidIO", false);

            GtxArea.DrawView();
            BaseDrawer.AddDirctionSentence(base._graph, _infoAreaRects[2], "GTX", false);

            LvdsArea.DrawView();
            BaseDrawer.AddDirctionSentence(base._graph, _infoAreaRects[3], "LVDS", false);

        }
    }

    #endregion Vpx连接端相关图像类

}
