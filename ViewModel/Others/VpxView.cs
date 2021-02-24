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
using DRSysCtrlDisplay.ViewModel.Others;
using DRSysCtrlDisplay.Models;

namespace DRSysCtrlDisplay.OtherView
{
    /// <summary>
    /// 一个Vpx上的通信区的描述
    /// </summary>
    public class VpxEndAreaView : BaseDrawer
    {
        public LinkType LinkType { get; private set; }              //该vpx连接区对应的连接类型
        public VpxCategory Type { get; private set; }               //该vpx的种类

        public VpxEndAreaView(Rectangle rect, LinkType linkType, VpxCategory type)
            : base(rect)
        {
            LinkType = linkType;
            Type = type;
        }

        public override void DrawView(Graphics g)
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
            g.DrawRectangle(pen, base._rect);
            g.FillRectangle(brush, base._rect);
        }

        public void DrawViewColor(Graphics g, Pen pen, Brush brush)
        {
            g.DrawRectangle(pen, base._rect);
            g.FillRectangle(brush, base._rect);
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

        [BrowsableAttribute(false)]
        public VpxEndAreaView EthArea { get; protected set; }

        [BrowsableAttribute(false)]
        public VpxEndAreaView RioArea { get; protected set; }

        [BrowsableAttribute(false)]
        public VpxEndAreaView GtxArea { get; protected set; }

        [BrowsableAttribute(false)]
        public VpxEndAreaView LvdsArea { get; protected set; }

        protected VpxEndView(Rectangle rect, string name, VpxCategory type)
            : base( rect)
        {
            Name = name;
            this.Type = type;
        }

        //画出该View的图像
        public override void DrawView(Graphics g) { }

        public override object GetModelInstance()
        {
            return this;
        }

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
    public class PlaneVpx : VpxEndView , IDrawerChoosed
    {
        public Rectangle _planeAreaRect;
        public Rectangle _nameAreaRect;
        public Rectangle[] _infoAreaRects;

        [Browsable(false)]
        public BaseDrawer ChoosedBv { get; set; }

        public PlaneVpx(Rectangle rect, string name, VpxCategory type)
            : base(rect, name, type) 
        {
            AssignRect();
            base.EthArea = new VpxEndAreaView(_infoAreaRects[0], LinkType.EtherNet, type);
            base.RioArea = new VpxEndAreaView(_infoAreaRects[1], LinkType.RapidIO, type);
            base.GtxArea = new VpxEndAreaView(_infoAreaRects[2], LinkType.GTX, type);
            base.LvdsArea = new VpxEndAreaView(_infoAreaRects[3], LinkType.LVDS, type);
        }

        public override void DrawView(Graphics g)
        {
            //画通信区域的图像
            base.EthArea.DrawView(g);
            base.RioArea.DrawView(g);
            base.GtxArea.DrawView(g);
            base.LvdsArea.DrawView(g);

            //画底板和名称区域的图像
            g.DrawRectangle(base.DefaultPen, this._planeAreaRect);
            g.FillRectangle(base.DefaultBrush, this._planeAreaRect);

            g.DrawRectangle(base.DefaultPen, this._nameAreaRect);
            g.FillRectangle(base.DefaultBrush, this._nameAreaRect);
        }

        public override void ChoosedDrawView(Graphics g)
        {
            DrawView(g);
            g.DrawRectangle(Pens.Red, _rect);
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

        public void MouseEventHandler(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public BaseDrawer GetChoosedBaseView(MouseEventArgs e)
        {
            if(GetWholeInfoAreaRect().Contains(e.Location))
            {
                return this;
            }
            return null;
        }
    }

    /// <summary>
    /// 背板槽位图像类(也包含外接口区)
    /// </summary>
    public class BackPlaneVpx : PlaneVpx
    {
        [Category("连接信息"), Description("以太网连接集合")]
        public BackPlaneLink[] EthLinks { get; private set; }

        [Category("连接信息"), Description("RapidIO连接集合")]
        public BackPlaneLink[] RioLinks { get; private set; }

        [Category("连接信息"), Description("GTX连接集合")]
        public BackPlaneLink[] GtxLinks { get; private set; }

        [Category("连接信息"), Description("LVDS连接集合")]
        public BackPlaneLink[] LvdsLinks { get; private set; }

        public BackPlaneVpx(Rectangle rect, string name, List<BackPlaneLink> links)
            : base(rect, name, VpxCategory.BackPlane)
        {
            EthLinks = links.Where(link => link.LinkType == LinkType.EtherNet).ToArray();
            RioLinks = links.Where(link => link.LinkType == LinkType.RapidIO).ToArray();
            GtxLinks = links.Where(link => link.LinkType == LinkType.GTX).ToArray();
            LvdsLinks = links.Where(link => link.LinkType == LinkType.LVDS).ToArray();
        }
    }

    /// <summary>
    /// 机箱里的空槽位图像
    /// </summary>
    public class EmptySlotVpx : PlaneVpx
    {
        public EmptySlotVpx(Rectangle rect, string name)
            : base(rect, name, VpxCategory.EmptySlot)
        { }

        public override void DrawView(Graphics g)
        {
            //画通信区域的图像
            base.EthArea.DrawViewColor(g, base.DefaultPen, base.DefaultBrush);
            base.RioArea.DrawViewColor(g, base.DefaultPen, base.DefaultBrush);
            base.GtxArea.DrawViewColor(g, base.DefaultPen, base.DefaultBrush);
            base.LvdsArea.DrawViewColor(g, base.DefaultPen, base.DefaultBrush);

            //画底板和名称区域的图像
            g.DrawRectangle(base.DefaultPen, _planeAreaRect);
            g.FillRectangle(base.DefaultBrush, _planeAreaRect);

            g.DrawRectangle(base.DefaultPen, _nameAreaRect);
            g.FillRectangle(base.DefaultBrush, _nameAreaRect);
        }

        public override void ChoosedDrawView(Graphics g)
        {
            DrawView(g);
            g.DrawRectangle(Pens.Red, base.GetWholeInfoAreaRect());
        }
    }

    /// <summary>
    /// 机箱里的板卡图像
    /// </summary>
    public class BoardVpx : PlaneVpx
    {
        public BoardVpx(Rectangle rect, string name)
            : base(rect, name, VpxCategory.Board)
        { }

        public override void ChoosedDrawView(Graphics g)
        {
            DrawView(g);
            g.DrawRectangle(Pens.Red, base.GetWholeInfoAreaRect());
        }

        public override object GetModelInstance()
        {
            return ModelFactory<Board>.CreateByName(base.Name);
        }
    }

    /// <summary>
    /// 背板示意区图像类
    /// </summary>
    public class IndicateAreaVpx : PlaneVpx
    {
        public IndicateAreaVpx(Rectangle rect, string name) 
            : base(rect, name, VpxCategory.IndicateArea) 
        { }

        public override void DrawView(Graphics g)
        {
            //画通信区域的图像
            EthArea.DrawView(g);
            BaseDrawer.AddDirctionSentence(g, _infoAreaRects[0], "EtherNet", false);

            RioArea.DrawView(g);
            BaseDrawer.AddDirctionSentence(g, _infoAreaRects[1], "RapidIO", false);

            GtxArea.DrawView(g);
            BaseDrawer.AddDirctionSentence(g, _infoAreaRects[2], "GTX", false);

            LvdsArea.DrawView(g);
            BaseDrawer.AddDirctionSentence(g, _infoAreaRects[3], "LVDS", false);

        }
    }

    #endregion Vpx连接端相关图像类

}
