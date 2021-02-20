using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.ViewModel.Others;
using DRSysCtrlDisplay.Models;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    public class BoardViewModel : BaseDrawer, IDrawerChoosed
    {
        Board _board;                                           //对应的板卡实体

        AssignRectangle _assignRect;                            //矩形分配器
        List<Rectangle> _ppcsPositions;                         //PPC图像集合,与_board的PPCList成员一一对应
        List<Rectangle> _fpgasPositions;                        //FPGA图像集合,与_board的FPGAList成员一一对应
        List<Rectangle> _zynqsPositions;                        //ZYNQ图像集合,与_board的ZYNQList成员一一对应
        List<Rectangle> _swsPositions;                          //SW图像集合,与_board的SwitchList成员一一对应
        Dictionary<List<Point>, LinkType> _linksPositions;      //Link图像集合,与_board的LinkList成员一一对应
        List<Rectangle> _vpxsPositions;                         //VPX的图像位置集合;一共4个成员：EtherNet(0),RapidIO(1),GTX(2),LVDS(3)
        public BaseDrawer ChoosedBv { get; set; }               //当前视图被选中的图元

        public BoardViewModel(Board board, Rectangle rect)
        {
            _board = board;
            Init(rect);
        }

        public BoardViewModel(Board board)
        {
            _board = board;
        }

        public override void Init( Rectangle rect)
        {
            base.Init(rect);
            _assignRect = new AssignRectangle(base._rect);
        }

        #region 重载虚函数
        public override void DrawView(Graphics g)
        {
            //画板卡对应的方框
            g.DrawRectangle(Pens.Black, base._rect);
            DrawVPXs(g);

            DrawCores(g);
            if (ChoosedBv != null)
            {
                ChoosedBv.ChoosedDrawView(g);
            }
            DrawLinks(g, _board.LinkList);
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
            //访问PPC所在的空间
            if (_ppcsPositions != null)
            {
                for (int i = 0; i < _ppcsPositions.Count; i++)
                {
                    if (_ppcsPositions[i].Contains(e.Location))
                    {
                        return new PPCViewModel(_board.PPCList[i], _ppcsPositions[i]);
                    }
                }
            }
            //访问FPGA所在的空间
            if (_fpgasPositions != null)
            {
                for (int i = 0; i < _fpgasPositions.Count; i++)
                {
                    if (_fpgasPositions[i].Contains(e.Location))
                    {
                        return new FPGAViewModel(_board.FPGAList[i], _fpgasPositions[i]);
                    }
                }
            }
            //访问ZYNQ所在的空间
            if (_zynqsPositions != null)
            {
                for (int i = 0; i < _zynqsPositions.Count; i++)
                {
                    if (_zynqsPositions[i].Contains(e.Location))
                    {
                        return new ZYNQViewModel(_board.ZYNQList[i], _zynqsPositions[i]);
                    }
                }
            }
            //访问SW所在的空间
            if (_swsPositions != null)
            {
                for (int i = 0; i < _swsPositions.Count; i++)
                {
                    if (_swsPositions[i].Contains(e.Location))
                    {
                        var category = i == 0 ? SwitchCategory.EtherNetSw : SwitchCategory.RioSw;//i == 0：以太网交换机；//i == 1，RapidIO交换机
                        return _board.SwitchList.Where(sw => sw.Category == category).FirstOrDefault();
                    }
                }
            }
            return null;
        }

        #endregion 实现接口

        private void DrawVPXs(Graphics g)
        {
            _vpxsPositions = GetVPXsPositions();
            //画vpx连接区的EtherNet区域
            g.DrawRectangle(ConnectAreaColor.Pen_EtherNet, _vpxsPositions[0]);
            g.FillRectangle(ConnectAreaColor.Brushes_EtherNet, _vpxsPositions[0]);
            BaseDrawer.AddDirctionSentence(g, _vpxsPositions[0], "EtherNet", true);

            //画vpx连接区的RapidIO区域
            g.DrawRectangle(Princeple.ConnectAreaColor.Pen_RapidIO, _vpxsPositions[1]);
            g.FillRectangle(Princeple.ConnectAreaColor.Brushes_RapidIO, _vpxsPositions[1]);
            BaseDrawer.AddDirctionSentence(g, _vpxsPositions[1], "RapidIO", true);

            //画vpx连接区的GTX区域
            g.DrawRectangle(Princeple.ConnectAreaColor.Pen_GTX, _vpxsPositions[2]);
            g.FillRectangle(Princeple.ConnectAreaColor.Brushes_GTX, _vpxsPositions[2]);
            BaseDrawer.AddDirctionSentence(g, _vpxsPositions[2], "GTX", true);

            //画vpx连接区的LVDS区域
            g.DrawRectangle(Princeple.ConnectAreaColor.Pen_LVDS, _vpxsPositions[3]);
            g.FillRectangle(Princeple.ConnectAreaColor.Brushes_LVDS, _vpxsPositions[3]);
            BaseDrawer.AddDirctionSentence(g, _vpxsPositions[3], "LVDS", true);

        }

        /// <summary>
        /// 画所有的芯片：PPC集合，FPGA集合，ZYNQ集合，SwitchDevice集合
        /// </summary>
        private void DrawCores(Graphics g)
        {
            //PPC集合
            _ppcsPositions = GetPPCsPositions(_board.PPCList.Count);
            for (int i = 0; i < _board.PPCList.Count; i++)
            {
                var ppcDrawer = new PPCViewModel(_board.PPCList[i], _ppcsPositions[i]);
                ppcDrawer.DrawView(g);
            }
            //FPGA集合
            _fpgasPositions = GetFPGAsPositions(_board.FPGAList.Count);
            for (int i = 0; i < _board.FPGAList.Count; i++)
            {
                var fpgaDrawer = new FPGAViewModel(_board.FPGAList[i], _fpgasPositions[i]);
                fpgaDrawer.DrawView(g);
            }
            //ZYNQ集合
            _zynqsPositions = GetZYNQsPositions(_board.ZYNQList.Count);
            for (int i = 0; i < _board.ZYNQList.Count; i++)
            {
                var zynqDrawer = new ZYNQViewModel(_board.ZYNQList[i], _zynqsPositions[i]);
                zynqDrawer.DrawView(g);
            }
            //SwitchDevice集合
            _swsPositions = GetSwsPositions();
            foreach (SwitchDevice sw in _board.SwitchList)
            {
                if (sw.Category == SwitchCategory.EtherNetSw)//以太网交换机
                {
                    sw.SetDrawingRect(_swsPositions[0]);
                    sw.DrawView(g);
                    DrawSw2Vpx(g, _swsPositions[0], _vpxsPositions[0], ConnectAreaColor.Pen_EtherNet);
                }
                else//Rio交换机
                {
                    sw.SetDrawingRect(_swsPositions[1]);
                    sw.DrawView(g);
                    DrawSw2Vpx(g, _swsPositions[1], _vpxsPositions[1], ConnectAreaColor.Pen_RapidIO);
                }
            }
        }
        private void DrawLinks(Graphics g, List<BoardLink> links)
        {
            //把所有的links转化为,连接位置关系的集合
            _linksPositions = GetLinksPositions(links);

            foreach (var linkPositions in _linksPositions)
            {
                Pen pen;
                switch (linkPositions.Value)
                {
                    case LinkType.EtherNet:
                        pen = ConnectAreaColor.Pen_EtherNet;
                        break;
                    case LinkType.RapidIO:
                        pen = ConnectAreaColor.Pen_RapidIO;
                        break;
                    case LinkType.GTX:
                        pen = ConnectAreaColor.Pen_GTX;
                        break;
                    default:
                        pen = ConnectAreaColor.Pen_LVDS;
                        break;
                }
                //把一个List转化为Array
                Point[] points = new Point[linkPositions.Key.Count];
                linkPositions.Key.CopyTo(points);
                g.DrawLines(pen, points);
            }
        }

        //画交换机到vpx的连线；
        //(注意：有提升空间，现在只是表示连接关系画了一条虚拟的线，没细化到有具体多少根，和每根对应的sw端口及vpx位置)
        private void DrawSw2Vpx(Graphics g, Rectangle sw, Rectangle vpx, Pen pen)
        {
            var point1 = new Point(sw.Location.X + sw.Size.Width / 2, sw.Location.Y + sw.Size.Height);
            var point2 = new Point(point1.X, vpx.Location.Y);
            g.DrawLine(pen, point1, point2);
        }

        private List<Rectangle> GetPPCsPositions(int num)
        {
            Rectangle ppcsRect = _assignRect.GetPPCsRectangle();
            return GetcoresPositions(ppcsRect, num);
        }

        private List<Rectangle> GetZYNQsPositions(int num)
        {
            Rectangle zynqsRect = _assignRect.GetZYNQsRectangle();
            return GetcoresPositions(zynqsRect, num);
        }

        private List<Rectangle> GetFPGAsPositions(int num)
        {
            Rectangle fpgasRect = _assignRect.GetFPGAsRectangle();
            return GetcoresPositions(fpgasRect, num);
        }

        /// <summary>
        /// 获取芯片集合对应的矩形集合
        /// </summary>
        /// <param name="rect">芯片矩形集合对应的外部矩形</param>
        /// <param name="num">芯片的个数</param>
        /// <returns></returns>
        private List<Rectangle> GetcoresPositions(Rectangle rect, int num)
        {
            if (num == 0)
            {
                return null;
            }
            List<Rectangle> coresPositions = new List<Rectangle>();
            double sizeW = rect.Size.Height / 2 / num;                            //固定值
            double sizeH = sizeW;                                                   //固定值
            double pointX = rect.Location.X + rect.Size.Width / 2 - sizeW / 2;      //固定值
            double pointY_First = rect.Location.Y + sizeH / 2;              //第1个对应矩形框的Y的值
            double pointY_Offset = 2 * sizeH;                                           //两个相邻矩形框的Y值偏移

            for (int i = 0; i < num; i++)
            {
                coresPositions.Add(new Rectangle(new Point((int)pointX, (int)(pointY_First + pointY_Offset * i)),
                    new Size((int)sizeW, (int)sizeH)));
            }
            return coresPositions;
        }

        /// <summary>
        /// 返回两个Sw对应的矩形区域，第1个是以太网Sw,第2个是RapidIO的Sw;
        /// </summary>
        /// <returns></returns>
        private List<Rectangle> GetSwsPositions()
        {
            const double size_Scale = 8.0 / 10;     //交换机长或者宽与区域矩形高的比例
            const double pointYoff_Scale = 1.0 / 10;   //交换机Y起点相对于区域矩形起点的偏移与区域矩形高的比例

            List<Rectangle> swsPositions = new List<Rectangle>();
            Rectangle swsRect = _assignRect.GetSwsRectangle();

            Size s = new Size((int)(swsRect.Height * size_Scale), (int)(swsRect.Height * size_Scale));

            double ethPointX = swsRect.Location.X + swsRect.Size.Width / 6 - s.Width / 2;
            double ethPointY = swsRect.Location.Y + s.Height * pointYoff_Scale;
            double rioPointX = swsRect.Location.X + swsRect.Size.Width / 2 - s.Width / 2;
            double rioPointY = ethPointY;

            swsPositions.Add(new Rectangle(new Point((int)ethPointX, (int)ethPointY), s));
            swsPositions.Add(new Rectangle(new Point((int)rioPointX, (int)rioPointY), s));
            return swsPositions;
        }

        /// <summary>
        /// 获取VPX对应的矩形集合;VPX有4个区域:EtherNet,RapidIO,GTX,LVDX,分别对应List的第0个，第1个，第2个，第3个Item
        /// </summary>
        /// <returns></returns>
        private List<Rectangle> GetVPXsPositions()
        {
            double allPointY_Scale = 0;         //所有矩形的Point的y值都一样；
            double allSizeH_Scale = 1;          //所有矩形的Size的Height都一样,都为外部矩形高度;

            double etherNetPointX_Scale = 0;    //连接器以太网矩形区域Point的x值相对于外部矩形宽度(Width)的比例
            double etherNetSizeW_Scale = 1.0 / 3;   //连接器以太网矩形区域Size的Width值相对于外部矩形宽度(Width)的比例

            double rapidIOPointX_Scale = etherNetPointX_Scale + etherNetSizeW_Scale;
            double rapidIOSizeW_Scale = etherNetSizeW_Scale;

            double gtxPointX_Scale = rapidIOPointX_Scale + rapidIOSizeW_Scale;
            double gtxSizeW_Scale = (1 - etherNetSizeW_Scale - etherNetSizeW_Scale) / 2;

            double lvdsPointX_Scale = gtxPointX_Scale + gtxSizeW_Scale;
            double lvdsSizeW_Scale = gtxSizeW_Scale;

            List<Rectangle> vpxsPositions = new List<Rectangle>();
            Rectangle vpxsRect = _assignRect.GetVPXsRectangle();

            //加入VPX的EtherNet区域对应的矩形
            vpxsPositions.Add(AssignRectangle.GetRectangle(
                vpxsRect, etherNetPointX_Scale, allPointY_Scale, etherNetSizeW_Scale, allSizeH_Scale));
            //加入VPX的RapidIO区域对应的矩形
            vpxsPositions.Add(AssignRectangle.GetRectangle(
                vpxsRect, rapidIOPointX_Scale, allPointY_Scale, rapidIOSizeW_Scale, allSizeH_Scale));
            //加入VPX的GTX区域对应的矩形
            vpxsPositions.Add(AssignRectangle.GetRectangle(
                vpxsRect, gtxPointX_Scale, allPointY_Scale, gtxSizeW_Scale, allSizeH_Scale));
            //加入VPX的LVDS区域对应的矩形
            vpxsPositions.Add(AssignRectangle.GetRectangle(
                vpxsRect, lvdsPointX_Scale, allPointY_Scale, lvdsSizeW_Scale, allSizeH_Scale));

            return vpxsPositions;
        }


        private Dictionary<List<Point>, LinkType> GetLinksPositions(List<BoardLink> links)
        {
            var linksDir = new Dictionary<List<Point>, LinkType>();

            //1.获取各布局器所包含link对应的点
            //添加芯片相关连接线对应的点
            var ppcsLinksDir = GetCoreLinksDir(links, EndType.PPC);
            var zynqsLinksDir = GetCoreLinksDir(links, EndType.ZYNQ);
            var fpgasLinksDir = GetCoreLinksDir(links, EndType.FPGA);

            //添加Sw相关连接线对应的点
            var ethSwLayout = new Layouters.SwLayout(_swsPositions[0], LinkType.EtherNet);
            var ethSwPoint = ethSwLayout.GetPoint();
            var rioSwLayout = new Layouters.SwLayout(_swsPositions[1], LinkType.RapidIO);
            var rioSwPoint = rioSwLayout.GetPoint();

            //添加Vpx相关连接线对应的点
            var ethVpxLinksDir = GetVPXLinksDir(links, LinkType.EtherNet);
            var rioVpxLinksDir = GetVPXLinksDir(links, LinkType.RapidIO);
            var gtxVpxLinksDir = GetVPXLinksDir(links, LinkType.GTX);
            var lvdsVpxLinksDir = GetVPXLinksDir(links, LinkType.LVDS);

            //2.拼接连接到交换机的所有Link
            spliceSw(ref linksDir, ppcsLinksDir, ethSwPoint, rioSwPoint);
            spliceSw(ref linksDir, zynqsLinksDir, ethSwPoint, rioSwPoint);
            spliceSw(ref linksDir, fpgasLinksDir, ethSwPoint, rioSwPoint);

            //3.拼接连接到VPX的所有Link
            //ppc到vpx
            spliceVpx(ref linksDir, ppcsLinksDir, ethVpxLinksDir);
            spliceVpx(ref linksDir, ppcsLinksDir, rioVpxLinksDir);
            //zynq到vpx
            spliceVpx(ref linksDir, zynqsLinksDir, ethVpxLinksDir);
            spliceVpx(ref linksDir, zynqsLinksDir, rioVpxLinksDir);
            spliceVpx(ref linksDir, zynqsLinksDir, gtxVpxLinksDir);
            spliceVpx(ref linksDir, zynqsLinksDir, lvdsVpxLinksDir);
            //fpga到vpx
            spliceVpx(ref linksDir, fpgasLinksDir, rioVpxLinksDir);
            spliceVpx(ref linksDir, fpgasLinksDir, gtxVpxLinksDir);
            spliceVpx(ref linksDir, fpgasLinksDir, lvdsVpxLinksDir);

            //4.拼接ZYNQ与FPGA之间的连接
            spliceZF(ref linksDir, zynqsLinksDir, fpgasLinksDir);

            return linksDir;
        }

        #region 下列方法都是连接link转为点集合的辅助方法，仅被GetLinksPositions调用

        private Dictionary<BoardLink, List<Point>> GetCoreLinksDir(List<BoardLink> allLinks, EndType endType)
        {
            Layouters.CoresLayouterBase coresLayouter = null;
            //获取连在了同一类型芯片上面的link
            var coresLinks = from lnk in allLinks
                                where lnk.FirstEndType == endType || lnk.SecondEndType == endType
                                select lnk;
            var coresLinksList = coresLinks.ToList();
            if (coresLinksList.Count == 0)
            {
                return null;
            }
            //创建相关的布局器
            switch (endType)
            {
                case EndType.PPC:
                    coresLayouter = new Layouters.PPCsLayouter(coresLinksList, _assignRect.GetPPCsRectangle(), _ppcsPositions);
                    break;
                case EndType.FPGA:
                    coresLayouter = new Layouters.FPGAsLayouter(coresLinksList, _assignRect.GetFPGAsRectangle(), _fpgasPositions);
                    break;
                default://EndType.ZYNQ
                    coresLayouter = new Layouters.ZYNQsLayouter(coresLinksList, _assignRect.GetZYNQsRectangle(), _zynqsPositions);
                    break;
            }
            return coresLayouter.GetLinksPoints();
        }

        private Dictionary<BoardLink, List<Point>> GetVPXLinksDir(List<BoardLink> allLinks, LinkType linkType)
        {
            Layouters.VpxLayout vpxLayout = null;
            //找到同类型的link
            var vpxLinks = from lnk in allLinks
                            where lnk.SecondEndType == EndType.VPX && lnk.LinkType == linkType
                            select lnk;
            var vpxLinksList = vpxLinks.ToList();
            if (vpxLinksList.Count == 0)
            {
                return null;
            }
            switch (linkType)
            {
                case LinkType.EtherNet:
                    vpxLayout = new Layouters.VpxLayout(_vpxsPositions[0], vpxLinksList, linkType
                        , _assignRect.GetEthsRectangle().Height, _assignRect.GetEthsRectangle().Y);
                    break;
                case LinkType.RapidIO:
                    vpxLayout = new Layouters.VpxLayout(_vpxsPositions[1], vpxLinksList, linkType
                        , _assignRect.GetRiosRectangle().Height, _assignRect.GetRiosRectangle().Y);
                    break;
                case LinkType.GTX:
                    vpxLayout = new Layouters.VpxLayout(_vpxsPositions[2], vpxLinksList, linkType
                        , _assignRect.GetGtxsRectangle().Height, _assignRect.GetGtxsRectangle().Y);
                    break;
                default://LinkType.LVDS
                    vpxLayout = new Layouters.VpxLayout(_vpxsPositions[3], vpxLinksList, linkType
                        , _assignRect.GetLvdssRectangle().Height, _assignRect.GetLvdssRectangle().Y);
                    break;
            }
            return vpxLayout.GetLinksPoints();
        }

        private void spliceSw(ref Dictionary<List<Point>, LinkType> linksDir,
            Dictionary<BoardLink, List<Point>> coresPointDir, Point ethSwPoint, Point rioSwPoint)
        {
            if (coresPointDir == null)
            {
                return;
            }
            foreach (var link in coresPointDir)
            {
                if (link.Key.SecondEndType == EndType.SW)
                {
                    Point point4 = (link.Key.LinkType == LinkType.EtherNet ? ethSwPoint : rioSwPoint);
                    Point point3 = new Point(link.Value[1].X, point4.Y);
                    var pointList = new List<Point> { link.Value[0], link.Value[1], point3, point4 };
                    linksDir.Add(pointList, link.Key.LinkType);
                }
            }
        }

        private void spliceVpx(ref Dictionary<List<Point>, LinkType> linksDir,
            Dictionary<BoardLink, List<Point>> coresPointDir, Dictionary<BoardLink, List<Point>> vpxPointDir)
        {
            List<Point> pointList = null;
            if (coresPointDir == null || vpxPointDir == null)
            {
                return;
            }
            foreach (var linkPair in coresPointDir)
            {
                if (linkPair.Key.SecondEndType == EndType.VPX)
                {
                    if (vpxPointDir.Keys.Contains(linkPair.Key) == false)
                    {
                        continue;
                    }
                    var vpxList = vpxPointDir[linkPair.Key];
                    if (vpxList == null)
                    {//直接连到连接器
                        int point3Y = _assignRect.GetVPXsRectangle().Location.Y;
                        Point point3 = new Point(linkPair.Value[1].X, point3Y);
                        pointList = new List<Point> { linkPair.Value[0], linkPair.Value[1], point3 };
                        linksDir.Add(pointList, linkPair.Key.LinkType);
                    }
                    else
                    {
                        Point point3 = new Point(linkPair.Value[1].X, vpxList[1].Y);
                        pointList = new List<Point> { linkPair.Value[0], linkPair.Value[1], point3, vpxList[1], vpxList[0] };
                        linksDir.Add(pointList, linkPair.Key.LinkType);
                    }
                }
            }
        }

        /// <summary>
        /// 拼接ZYNQ和FPGA的之间的连线(gtx,lvds)
        /// </summary>
        /// <param name="linksDir"></param>
        /// <param name="zynqsPointDir"></param>
        /// <param name="fpgasPointDir"></param>
        private void spliceZF(ref Dictionary<List<Point>, LinkType> linksDir,
            Dictionary<BoardLink, List<Point>> zynqsPointDir, Dictionary<BoardLink, List<Point>> fpgasPointDir)
        {
            List<Point> pointList = null;
            if (zynqsPointDir == null || fpgasPointDir == null)
            {
                return;
            }
            foreach (var linkPair in zynqsPointDir)
            {
                if (linkPair.Key.SecondEndType == EndType.FPGA)
                {
                    if (fpgasPointDir.Keys.Contains(linkPair.Key) == false)
                    {
                        continue;
                    }
                    var fpgaList = fpgasPointDir[linkPair.Key];
                    pointList = new List<Point> { linkPair.Value[0], fpgaList[0] };
                    linksDir.Add(pointList, linkPair.Key.LinkType);
                }
            }
        }
        #endregion

        /// <summary>
        /// 分配矩形位置的类
        /// </summary>
        public class AssignRectangle
        {
            private Rectangle _rect;    //一个用来分配的矩形
            //PPCs集合区域
            private static double _ppcPointX_Scale = 0;         //子矩形起点位置x值相对于原矩形宽度(Width)的比例
            private static double _ppcPointY_Scale = 0;         //子矩形起点位置y值相对于原矩形高度(Height)的比例
            private static double _ppcSizeW_Scale = 1.0 / 3;      //子矩形的宽度相对于原矩形宽度(Width)的比例
            private static double _ppcSizeH_Scale = 5.0 / 10;     //子矩形的高度相对于原矩形高度(Height)的比例
            //ZYNQs集合区域
            private static double _zynqPointX_Scale = _ppcPointX_Scale + _ppcSizeW_Scale;
            private static double _zynqPointY_Scale = 0;
            private static double _zynqSizeW_Scale = _ppcSizeW_Scale;
            private static double _zynqSizeH_Scale = _ppcSizeH_Scale;
            //FPGAs集合区域
            private static double _fpgaPointX_Scale = _zynqPointX_Scale + _zynqSizeW_Scale;
            private static double _fpgaPointY_Scale = 0;
            private static double _fpgaSizeW_Scale = _ppcSizeW_Scale;
            private static double _fpgaSizeH_Scale = _ppcSizeH_Scale;
            //SWs集合区域
            private static double _swPointX_Scale = 0;
            private static double _swPointY_Scale = _ppcPointY_Scale + _ppcSizeH_Scale;
            private static double _swSizeW_Scale = 1;
            private static double _swSizeH_Scale = 1.5 / 10;
            //EtherNet到VPX线的对应区域
            private static double _ethPointX_Scale = 0;
            private static double _ethPointY_Scale = _swPointY_Scale + _swSizeH_Scale;
            private static double _ethSizeW_Scale = 1;
            private static double _ethSizeH_Scale = (1 - _ppcSizeH_Scale - _swSizeH_Scale) / 5;
            //RapidIO到VPX线的对应区域
            private static double _rioPointX_Scale = 0;
            private static double _rioPointY_Scale = _ethPointY_Scale + _ethSizeH_Scale;
            private static double _rioSizeW_Scale = 1;
            private static double _rioSizeH_Scale = _ethSizeH_Scale;
            //GTX到VPX线的对应区域
            private static double _gtxPointX_Scale = 0;
            private static double _gtxPointY_Scale = _rioPointY_Scale + _rioSizeH_Scale;
            private static double _gtxSizeW_Scale = 1;
            private static double _gtxSizeH_Scale = _ethSizeH_Scale;
            //LVDS到VPX线的对应区域
            private static double _lvdsPointX_Scale = 0;
            private static double _lvdsPointY_Scale = _gtxPointY_Scale + _gtxSizeH_Scale;
            private static double _lvdsSizeW_Scale = 1;
            private static double _lvdsSizeH_Scale = _ethSizeH_Scale;
            //VPXs集合区域
            private static double _vpxPointX_Scale = 0;
            private static double _vpxPointY_Scale = _lvdsPointY_Scale + _lvdsSizeH_Scale;
            private static double _vpxSizeW_Scale = 1;
            private static double _vpxSizeH_Scale = _ethSizeH_Scale;

            public AssignRectangle(Rectangle r)
            {
                _rect = r;
            }
            public Rectangle GetPPCsRectangle()
            {
                return GetRectangle(_rect, _ppcPointX_Scale, _ppcPointY_Scale, _ppcSizeW_Scale, _ppcSizeH_Scale);
            }
            public Rectangle GetZYNQsRectangle()
            {
                return GetRectangle(_rect, _zynqPointX_Scale, _zynqPointY_Scale, _zynqSizeW_Scale, _zynqSizeH_Scale);
            }
            public Rectangle GetFPGAsRectangle()
            {
                return GetRectangle(_rect, _fpgaPointX_Scale, _fpgaPointY_Scale, _fpgaSizeW_Scale, _fpgaSizeH_Scale);
            }
            public Rectangle GetSwsRectangle()
            {
                return GetRectangle(_rect, _swPointX_Scale, _swPointY_Scale, _swSizeW_Scale, _swSizeH_Scale);
            }
            public Rectangle GetEthsRectangle()
            {
                return GetRectangle(_rect, _ethPointX_Scale, _ethPointY_Scale, _ethSizeW_Scale, _ethSizeH_Scale);
            }
            public Rectangle GetRiosRectangle()
            {
                return GetRectangle(_rect, _rioPointX_Scale, _rioPointY_Scale, _rioSizeW_Scale, _rioSizeH_Scale);
            }
            public Rectangle GetGtxsRectangle()
            {
                return GetRectangle(_rect, _gtxPointX_Scale, _gtxPointY_Scale, _gtxSizeW_Scale, _gtxSizeH_Scale);
            }
            public Rectangle GetLvdssRectangle()
            {
                return GetRectangle(_rect, _lvdsPointX_Scale, _lvdsPointY_Scale, _lvdsSizeW_Scale, _lvdsSizeH_Scale);
            }
            public Rectangle GetVPXsRectangle()
            {
                return GetRectangle(_rect, _vpxPointX_Scale, _vpxPointY_Scale, _vpxSizeW_Scale, _vpxSizeH_Scale);
            }

            /// <summary>
            /// 通过起点位置比例，和大小位子比例获取一个子方形区域
            /// </summary>
            /// <param name="xScale">子矩形起点位置x值相对于原矩形宽度(Width)的比例</param>
            /// <param name="yScale">子矩形起点位置y值相对于原矩形高度(Height)的比例</param>
            /// <param name="wSacle">子矩形的宽度相对于原矩形宽度(Width)的比例</param>
            /// <param name="hScale">子矩形的高度相对于原矩形高度(Height)的比例</param>
            /// <returns>新的矩形的引用</returns>
            public static Rectangle GetRectangle(Rectangle rect, double xScale, double yScale, double wSacle, double hScale)
            {
                var p = new Point((int)(rect.Location.X + xScale * rect.Size.Width),
                    (int)(rect.Location.Y + yScale * rect.Size.Height));
                var s = new Size((int)(wSacle * rect.Size.Width), (int)(hScale * rect.Size.Height));
                return new Rectangle(p, s);
            }
        }

        /// <summary>
        /// 存放布局器的类
        /// </summary>
        public class Layouters
        {
            private static bool SameLinkJudge(BoardLink link1, BoardLink link2)
            {
                if ((link1.LinkType == link2.LinkType) && (link1.SecondEndType == link2.SecondEndType))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 作为所有芯片集区域布局器的基类
            /// </summary>
            public class CoresLayouterBase
            {
                protected List<BoardLink> _coresLinks;      //该种芯片集对应的所有连接
                protected Rectangle _coresRect;        //该种芯片集对应的矩形区域
                protected List<CoreLayoutBase> _coreLayouts;  //各个芯片对应的矩形区域的集合

                protected CoresLayouterBase(List<BoardLink> coreslinks, Rectangle coresRect, List<Rectangle> coreRects)
                {
                    _coresLinks = coreslinks;
                    _coresRect = coresRect;
                    InitCoreLayouts(coreRects);
                }

                /// <summary>
                /// 初始化_coreLayouts成员
                /// </summary>
                /// <param name="coreRects"></param>
                private void InitCoreLayouts(List<Rectangle> coreRects)
                {
                    _coreLayouts = new List<CoreLayoutBase>();
                    var linkGroups = from link in _coresLinks
                                        group link by link.FirstEndId;
                    foreach (var linkGroup in linkGroups)
                    {
                        var links = linkGroup.ToList();
                        _coreLayouts.Add(CreateCoreLayout(links, coreRects[links[0].FirstEndId]));
                    }
                }

                /// <summary>
                /// 创建一个CoreLayout子类的实例，子类必需重载；
                /// </summary>
                /// <param name="coreLinks"></param>
                /// <param name="coreRect"></param>
                /// <returns></returns> 
                protected virtual CoreLayoutBase CreateCoreLayout(List<BoardLink> coreLinks, Rectangle coreRect)
                {
                    return new CoreLayoutBase(coreLinks, coreRect);
                }

                /// <summary>
                /// 获取link在这个芯片集区域里面对应的2个点
                /// </summary>
                /// <param name="link"></param>
                /// <returns></returns>
                private List<Point> GetPoints(BoardLink link)
                {
                    List<Point> points = new List<Point>();
                    //找到对应的coreLayout
                    var coreLos = from lo in _coreLayouts
                                    where lo.Contain(link)
                                    select lo;
                    CoreLayoutBase coreLo = coreLos.First();
                    //从该CoreLayout里面取出Link对应的点
                    points.Add(coreLo.GetLinkPoint(link));
                    //找到第二个点//Todo:确定基类能否调用子类重载的方法；
                    var point2 = new Point(GetLinkPointX(link), points[0].Y);
                    points.Add(point2);
                    return points;
                }

                /// <summary>
                /// 获取某个连接对应的分配点的X值；子类必需重载；
                /// </summary>
                /// <param name="link"></param>
                /// <returns></returns>
                protected virtual int GetLinkPointX(BoardLink link)
                {
                    return 0;
                }

                /// <summary>
                /// 获取link在当前区域所包含的同类link当中所占的比例，要排序
                /// </summary>
                /// <param name="link"></param>
                /// <returns></returns>
                protected double GetLinkOffsetScaleSqu(BoardLink link, Func<BoardLink, BoardLink, bool> func_Judge)
                {
                    //获取通类型的link
                    var sideLinks = from lnk in _coresLinks
                                    where func_Judge(link, lnk)
                                    orderby lnk.FirstEndId ascending
                                    select lnk;
                    var linksList = sideLinks.ToList();
                    int num = linksList.IndexOf(link);//确定该link是这个sideLinks里面的第几根,从0开始
                    return (num + 1.0) / (linksList.Count() + 1);
                }

                /// <summary>
                /// 获取该芯片集区域所有对应的Link所对应点集合(2个点)
                /// </summary>
                /// <returns></returns>
                public Dictionary<BoardLink, List<Point>> GetLinksPoints()
                {
                    Dictionary<BoardLink, List<Point>> linksPoints = new Dictionary<BoardLink, List<Point>>();
                    foreach (var link in _coresLinks)
                    {
                        linksPoints.Add(link, GetPoints(link));
                    }
                    return linksPoints;
                }
            }

            /// <summary>
            /// 芯片布局器的基类
            /// </summary>
            public class CoreLayoutBase
            {
                protected List<BoardLink> _coreLinks;  //芯片对应的所有连接
                protected Rectangle _coreRect;    //芯片对应的矩形区域

                public CoreLayoutBase(List<BoardLink> coreLinks, Rectangle coreRect)
                {
                    _coreLinks = coreLinks;
                    _coreRect = coreRect;
                }

                public bool Contain(BoardLink link)
                {
                    return _coreLinks.Contains(link);
                }

                public int GetWidth()
                {
                    return _coreRect.Width;
                }

                /// <summary>
                /// 获取link在当前区域所包含的同类link当中所占的比例，不排序
                /// </summary>
                /// <param name="link"></param>
                /// <returns></returns>
                protected double GetLinkOffsetScale(BoardLink link, Func<BoardLink, BoardLink, bool> func_Judge)
                {
                    var sideLinks = from lnk in _coreLinks
                                    where func_Judge(link, lnk)
                                    select lnk;
                    var linksList = sideLinks.ToList();
                    int num = linksList.IndexOf(link);//确定该link是这个sideLinks里面的第几根,从0开始
                    return (num + 1.0) / (linksList.Count() + 1);
                }

                /// <summary>
                /// 获取一个Link在矩形区域上面的一个点，也是一条Link的第一个点
                /// </summary>
                /// <param name="link"></param>
                /// <returns></returns>
                public virtual Point GetLinkPoint(BoardLink link)
                {
                    return new Point(0, 0);
                }
            }

            /// <summary>
            /// PPCs对应区域的布局器，布局器用来分配一个点，属于一条Link(EtherNet、RapidIO)的第二个点；
            /// </summary>
            public class PPCsLayouter : CoresLayouterBase
            {
                public PPCsLayouter(List<BoardLink> ppcslinks, Rectangle ppcsRect, List<Rectangle> ppcRects)
                    : base(ppcslinks, ppcsRect, ppcRects)
                {

                }

                protected override CoreLayoutBase CreateCoreLayout(List<BoardLink> coreLinks, Rectangle coreRect)
                {
                    return new PPCLayout(coreLinks, coreRect);
                }

                /// <summary>
                /// 获取某个连接对应的分配点的X值
                /// </summary>
                /// <param name="link"></param>
                /// <returns></returns>
                protected override int GetLinkPointX(BoardLink link)
                {
                    int x = 0;  //结果的X值
                    int ppcGap = (base._coresRect.Width - base._coreLayouts[0].GetWidth()) / 4;
                    double offsetScale = 0;  //点对应的X的偏移相对于PPCs矩形起点的偏移比例（为偏移值与ppcGap的比例）
                    offsetScale = GetLinkOffsetScaleSqu(link, SameLinkJudge);

                    if (link.LinkType == LinkType.EtherNet)
                    {
                        if (link.SecondEndType == EndType.SW)
                        {
                            x = base._coresRect.Location.X + (int)(ppcGap * offsetScale);
                        }
                        else//EndType.VPX
                        {
                            x = base._coresRect.Location.X + ppcGap + (int)(ppcGap * offsetScale);
                        }
                    }
                    else//LinkType.RapidIO
                    {
                        if (link.SecondEndType == EndType.SW)
                        {
                            x = base._coresRect.Location.X + base._coresRect.Width - (int)(ppcGap * offsetScale);
                        }
                        else//EndType.VPX
                        {
                            x = base._coresRect.Location.X + base._coresRect.Width - ppcGap
                                - (int)(ppcGap * offsetScale);
                        }
                    }
                    return x;
                }

                /// <summary>
                /// 单个PPC对应的布局器
                /// </summary>
                private class PPCLayout : CoreLayoutBase
                {
                    public PPCLayout(List<BoardLink> ppcLinks, Rectangle ppcRect)
                        : base(ppcLinks, ppcRect)
                    {

                    }
                    /// <summary>
                    /// 获取一个Link在矩形区域上面的一个点，也是一条Link(EtherNet、RapidIO)的第一个点
                    /// </summary>
                    /// <param name="link"></param>
                    /// <returns></returns>
                    public override Point GetLinkPoint(BoardLink link)
                    {
                        int pointX_Offset = 0;          //点对应的X的相对于PPC矩形起点的偏移（为绝对值）
                        int pointY_Offset = 0;          //点对应的Y的相对于PPC矩形起点的偏移（为绝对值）
                        double pointY_OffsetScale = 0;  //点对应的Y的偏移相对于起点的偏移比例（为偏移值与Height/2的比例）

                        //LinkType为EtherNet或者RapidIO
                        pointX_Offset = (link.LinkType == LinkType.EtherNet ? 0 : base._coreRect.Width);
                        //EndType为SW或者VPX
                        pointY_Offset = (link.SecondEndType == EndType.SW ? 0 : base._coreRect.Height / 2);

                        pointY_OffsetScale = GetLinkOffsetScale(link, SameLinkJudge);

                        var point = new Point(base._coreRect.Location.X + pointX_Offset,
                            (int)(base._coreRect.Location.Y + pointY_Offset + pointY_OffsetScale * base._coreRect.Height / 2));
                        return point;
                    }
                }

            }

            /// <summary>
            /// ZYNQs对应区域的布局器，布局器用来分配一个点，属于一条Link(EtherNet、RapidIO、GTX、LVDS)的第二个点；
            /// </summary>
            public class ZYNQsLayouter : CoresLayouterBase
            {
                private const int _leftLinksKind = 4;   //左侧link的种类：eth->sw;eth->vpx;rio->sw;rio->vpx
                private const int _rightLinksKind = 4;  //右侧link的种树：gtx->vpx;gtx->fpga;lvds->vpx;lvds->fpga;
                //单侧link种类最大值
                private const int _sideLinksKindMax = _leftLinksKind > _rightLinksKind ? _leftLinksKind : _rightLinksKind;

                private readonly int _ethSwStartPointX; //连接EtherNet的Link到交换机的起点X坐标
                private readonly int _ethVpxStartPointX; //连接EtherNet的Link的到VPX起点X坐标

                private readonly int _rioSwStartPointX; //连接RapidIO的Link到交换机的起点X坐标
                private readonly int _rioVpxStartPointX; //连接RapidIO的Link到Vpx的起点X坐标

                private readonly int _gtxStartPointX; //连接GTX的Link的起点X坐标
                private readonly int _lvdsStartPointX; //连接LVDS的Link的起点X坐标
                private readonly int _areaWidth;        //每一种连接区域的宽度;GTX,LVDS要乘以2倍

                public ZYNQsLayouter(List<BoardLink> zynqslinks, Rectangle zynqsRect, List<Rectangle> zynqRects)
                    : base(zynqslinks, zynqsRect, zynqRects)
                {
                    _areaWidth = (base._coresRect.Width - base._coreLayouts[0].GetWidth()) / 2 / _sideLinksKindMax;
                    _ethSwStartPointX = zynqsRect.Location.X;
                    _ethVpxStartPointX = _ethSwStartPointX + _areaWidth;
                    _rioSwStartPointX = _ethVpxStartPointX + _areaWidth;
                    _rioVpxStartPointX = _rioSwStartPointX + _areaWidth;
                    _lvdsStartPointX = zynqsRect.Location.X + zynqsRect.Size.Width;
                    _gtxStartPointX = _lvdsStartPointX - _areaWidth * 2;
                }

                protected override CoreLayoutBase CreateCoreLayout(List<BoardLink> coreLinks, Rectangle coreRect)
                {
                    return new ZYNQLayout(coreLinks, coreRect);
                }

                /// <summary>
                /// 获取某个连接对应的分配点的X值
                /// </summary>
                /// <param name="link"></param>
                /// <returns></returns>
                protected override int GetLinkPointX(BoardLink link)
                {
                    double offsetScale = GetLinkOffsetScaleSqu(link, SameLinkJudge);
                    double x = 0;
                    switch (link.LinkType)
                    {
                        case LinkType.EtherNet:
                            x = (link.SecondEndType == EndType.SW ? _ethSwStartPointX : _ethVpxStartPointX)
                                + offsetScale * _areaWidth;
                            break;
                        case LinkType.RapidIO:
                            x = (link.SecondEndType == EndType.SW ? _rioSwStartPointX : _rioVpxStartPointX)
                                + offsetScale * _areaWidth;
                            break;
                        case LinkType.GTX:
                            x = _gtxStartPointX - offsetScale * _areaWidth * 2;//GTX间距为一般间距的两倍
                            break;
                        case LinkType.LVDS:
                            x = _lvdsStartPointX - offsetScale * _areaWidth * 2;//LVDS间距为一般间距的两倍
                            break;
                        default:
                            break;
                    }
                    return (int)x;
                }
                /// <summary>
                /// 单个ZYNQ对应的布局器
                /// </summary>
                public class ZYNQLayout : CoreLayoutBase
                {
                    public ZYNQLayout(List<BoardLink> zynqLinks, Rectangle zynqRect)
                        : base(zynqLinks, zynqRect)
                    {

                    }

                    /// <summary>
                    /// 获取一个Link在矩形区域上面的一个点，也是一条Link(EtherNet、RapidIO、GTX、LVDS)的第一个点
                    /// </summary>
                    /// <param name="link"></param>
                    /// <returns></returns>
                    public override Point GetLinkPoint(BoardLink link)
                    {
                        int pointX = 0;
                        int pointY_Start = 0;     //点对应的区域起点的Y坐标
                        double pointY_Scale = 0;  //点对应的Y的偏移相对于FPGA矩形高度1/4的偏移比例
                        int rectRelativeHeight = base._coreRect.Size.Height / _sideLinksKindMax; //FPGA矩形高度1/4

                        pointY_Scale = GetLinkOffsetScale(link, SameLinkJudge);
                        switch (link.LinkType)
                        {
                            case LinkType.EtherNet:
                                pointX = base._coreRect.Location.X;
                                pointY_Start = base._coreRect.Location.Y
                                    + (link.SecondEndType == EndType.SW ? 0 : rectRelativeHeight);
                                break;
                            case LinkType.RapidIO:
                                pointX = base._coreRect.Location.X;
                                pointY_Start = base._coreRect.Location.Y + base._coreRect.Size.Height / 2
                                    + (link.SecondEndType == EndType.SW ? 0 : rectRelativeHeight);
                                break;
                            case LinkType.GTX:
                                pointX = base._coreRect.Location.X + base._coreRect.Size.Width;
                                pointY_Start = base._coreRect.Location.Y + rectRelativeHeight
                                    + (link.SecondEndType == EndType.FPGA ? 0 : (base._coreRect.Size.Height / 2));
                                break;
                            case LinkType.LVDS:
                                pointX = base._coreRect.Location.X + base._coreRect.Size.Width;
                                pointY_Start = base._coreRect.Location.Y
                                    + (link.SecondEndType == EndType.FPGA ? 0 : (base._coreRect.Size.Height / 2));
                                break;
                            default:
                                break;
                        }
                        return new Point(pointX, (int)(pointY_Start + pointY_Scale * rectRelativeHeight));
                    }
                }
            }

            /// <summary>
            /// FPGAs对应区域的布局器，布局器用来分配一个点，属于一条Link(RapidIO、GTX、LVDS)的第二个点；
            /// </summary>
            public class FPGAsLayouter : CoresLayouterBase
            {
                private const int _leftLinksKind = 5;   //左侧link的种类：rio->sw;rio->vpx;gtx->vpx;gtx->fpga;lvds->fpga
                private const int _rightLinksKind = 1;  //右侧link的种树：lvds->vpx;
                //单侧link种类最大值
                private const int _sideLinksKindMax = _leftLinksKind > _rightLinksKind ? _leftLinksKind : _rightLinksKind;

                private readonly int _rioSwStartPointX; //连接RapidIO的Link到交换机的起点X坐标
                private readonly int _rioVpxStartPointX; //连接RapidIO的Link到VPX的起点X坐标
                private readonly int _gtxStartPointX; //连接GTX的Link的起点X坐标
                private readonly int _lvdsStartPointX;  //连接LVDS的Link的起点X坐标
                private readonly int _areaWidth;        //连接区域的宽度

                public FPGAsLayouter(List<BoardLink> fpgaslinks, Rectangle fpgasRect, List<Rectangle> fpgaRects)
                    : base(fpgaslinks, fpgasRect, fpgaRects)
                {
                    _areaWidth = (base._coresRect.Width - base._coreLayouts[0].GetWidth()) / 2 / _sideLinksKindMax;
                    _rioSwStartPointX = fpgasRect.Location.X;
                    _rioVpxStartPointX = _rioSwStartPointX + _areaWidth;
                    _gtxStartPointX = _rioVpxStartPointX + _areaWidth * 2;
                    _lvdsStartPointX = fpgasRect.Location.X + fpgasRect.Size.Width;
                }

                protected override CoreLayoutBase CreateCoreLayout(List<BoardLink> coreLinks, Rectangle coreRect)
                {
                    return new FPGALayout(coreLinks, coreRect);
                }

                /// <summary>
                /// 获取某个连接对应的分配点的X值
                /// </summary>
                /// <param name="link"></param>
                /// <returns></returns>
                protected override int GetLinkPointX(BoardLink link)
                {
                    double offsetScale = GetLinkOffsetScaleSqu(link, SameLinkJudge);
                    double x = 0;
                    switch (link.LinkType)
                    {
                        case LinkType.RapidIO:
                            x = (link.SecondEndType == EndType.SW ? _rioSwStartPointX : _rioVpxStartPointX)
                                + offsetScale * _areaWidth;
                            break;
                        case LinkType.GTX:
                            x = _gtxStartPointX + offsetScale * _areaWidth;
                            break;
                        case LinkType.LVDS:
                            x = _lvdsStartPointX - offsetScale * _areaWidth * 3;//LVDS间距扩大3倍
                            break;
                        default:
                            break;
                    }
                    return (int)x;
                }
                /// <summary>
                /// 单个FPGA对应的布局器
                /// </summary>
                public class FPGALayout : CoreLayoutBase
                {
                    public FPGALayout(List<BoardLink> fpgaLinks, Rectangle fpgaRect)
                        : base(fpgaLinks, fpgaRect)
                    {

                    }

                    /// <summary>
                    /// 获取一个Link在矩形区域上面的一个点，也是一条Link(RapidIO、GTX、LVDS)的第一个点
                    /// </summary>
                    /// <param name="link"></param>
                    /// <returns></returns>
                    public override Point GetLinkPoint(BoardLink link)
                    {
                        int pointX = 0;
                        int pointY_Start = 0;     //点对应的区域起点的Y坐标
                        double pointY_Scale = 0;  //点对应的Y的偏移相对于FPGA矩形高度1/4的偏移比例

                        int rectRelativeHeight = base._coreRect.Size.Height / (_leftLinksKind - 1); //FPGA矩形高度1/4

                        pointY_Scale = GetLinkOffsetScale(link, SameLinkJudge);
                        switch (link.LinkType)
                        {
                            case LinkType.RapidIO:
                                pointX = base._coreRect.Location.X;
                                pointY_Start = base._coreRect.Location.Y + base._coreRect.Size.Height / 2
                                    + (link.SecondEndType == EndType.SW ? 0 : rectRelativeHeight / 2);
                                pointY_Scale = pointY_Scale / 2;//RapidIO高度缩短为1/2
                                break;
                            case LinkType.GTX:
                                pointX = base._coreRect.Location.X;
                                if (link.SecondEndType == EndType.VPX)
                                {
                                    pointY_Start = base._coreRect.Location.Y + base._coreRect.Size.Height / 2 + rectRelativeHeight;
                                }
                                else
                                {
                                    pointY_Start = base._coreRect.Location.Y + rectRelativeHeight;
                                }
                                break;
                            case LinkType.LVDS:
                                if (link.SecondEndType == EndType.VPX)
                                {
                                    pointX = base._coreRect.Location.X + base._coreRect.Size.Width;
                                    pointY_Start = base._coreRect.Location.Y;
                                    pointY_Scale = pointY_Scale * 4;//LVDS高度扩展为4倍
                                }
                                else//link.SecondEndType == EndType.ZYNQ
                                {
                                    pointX = base._coreRect.Location.X;
                                    pointY_Start = base._coreRect.Location.Y;
                                }
                                break;
                            default:
                                break;
                        }
                        return new Point(pointX, (int)(pointY_Start + pointY_Scale * rectRelativeHeight));
                    }
                }
            }

            /// <summary>
            /// Sw布局器，用来分配一个点，属于一条Link(EtherNet、RapidIO)的第四个点(最后一个)
            /// </summary>
            public class SwLayout
            {
                Rectangle _swRect;
                LinkType _swType;
                static double _ethPointY_Scale = 1.0 / 4;//以太网Sw点对应的Y的偏移相对于Sw矩形起点的偏移比例（为偏移值与Height的比例）
                static double _rioPointY_Scale = 3.0 / 4;

                public SwLayout(Rectangle swRect, LinkType type)
                {
                    _swRect = swRect;
                    _swType = type;
                }

                public Point GetPoint()
                {
                    int pointX_Offset = _swRect.Location.X + _swRect.Size.Width / 2;
                    double pointY_Scale = ((_swType == LinkType.EtherNet) ? _ethPointY_Scale : _rioPointY_Scale);

                    var point = new Point(pointX_Offset, (int)(_swRect.Location.Y + _swRect.Size.Height * pointY_Scale));
                    return point;
                }
            }

            /// <summary>
            /// VPX布局器,目前只做了GTX和LVDS区域的分配，用来分配芯片在连往VPX的两个点（第4和第5点）；
            /// </summary>
            public class VpxLayout
            {
                private Rectangle _vpxRect;     //该VPX区域对应的矩形区域
                private List<BoardLink> _vpxLinks;   //该VPX区域连接的Link集合
                private LinkType _linkType;    //区域的类型：EtherNet,RapidIO,GTX,LVDS
                private int _point2Hight;           //第二个点所在区域的高度
                private int _point2OriginY;         //第二个点对应的起始Y偏移


                public VpxLayout(Rectangle vpxRect, List<BoardLink> links, LinkType type, int height, int originY)
                {
                    _vpxRect = vpxRect;
                    _vpxLinks = links;
                    _linkType = type;
                    _point2Hight = height;
                    _point2OriginY = originY;
                }

                public Dictionary<BoardLink, List<Point>> GetLinksPoints()
                {
                    var dic = new Dictionary<BoardLink, List<Point>>();
                    foreach (BoardLink link in _vpxLinks)
                    {
                        switch (_linkType)
                        {
                            case LinkType.EtherNet:
                                dic.Add(link, EthGetPoints(link));
                                break;
                            case LinkType.RapidIO:
                                dic.Add(link, RioGetPoints(link));
                                break;
                            case LinkType.GTX:
                                dic.Add(link, GtxGetPoints(link));
                                break;
                            default://LinkType.LVDS
                                dic.Add(link, LvdsGetPoints(link));
                                break;
                        }
                    }
                    return dic;
                }

                /// <summary>
                /// 获取link在当前区域所包含的同类link当中所占的比例，排序
                /// </summary>
                private double GetLinkOffsetScaleSqu(BoardLink link)
                {
                    var links = from lnk in _vpxLinks
                                where link.LinkType == lnk.LinkType && link.FirstEndType == lnk.FirstEndType
                                orderby lnk.FirstEndId ascending
                                select lnk;
                    var linksList = links.ToList();
                    int seqNum = linksList.IndexOf(link);
                    return (seqNum + 1.0) / (linksList.Count + 1);
                }

                //获取以太网VPX上面的连接连接线的点集（2个）,分布point的算法
                private List<Point> EthGetPoints(BoardLink link)
                {
                    double offScale = 0;//该条link的相对同类link中的占比
                    var pointList = new List<Point>();
                    double pointX = 0;  //点的X坐标

                    int areaWidth = _vpxRect.Size.Width / 3;//区域宽度
                    if (link.FirstEndType == EndType.PPC)
                    {
                        //link的端1连接到PPC就没有点集
                        return null;
                    }

                    //获取点的X的坐标
                    offScale = GetLinkOffsetScaleSqu(link);
                    pointX = _vpxRect.Location.X + 2 * areaWidth + (offScale * areaWidth);

                    pointList.Add(new Point((int)pointX, _vpxRect.Location.Y));

                    double point2Y = _point2OriginY - offScale * _point2Hight;
                    pointList.Add(new Point((int)pointX, (int)point2Y));
                    return pointList;
                }

                private List<Point> RioGetPoints(BoardLink link)
                {
                    double offScale = 0;//该条link的相对同类link中的占比
                    var pointList = new List<Point>();
                    double pointX = 0;  //点的X坐标

                    int areaWidth = _vpxRect.Size.Width / 4;//区域宽度
                    if (link.FirstEndType == EndType.ZYNQ)
                    {
                        //link的端1连接到ZYNQ就没有点集
                        return null;
                    }

                    //获取点的X的坐标
                    offScale = GetLinkOffsetScaleSqu(link);
                    pointX = (link.FirstEndType == EndType.PPC ? 0 : (2 * areaWidth))
                        + _vpxRect.Location.X + (offScale * areaWidth);

                    pointList.Add(new Point((int)pointX, _vpxRect.Location.Y));

                    double point2Y = _point2OriginY - offScale * _point2Hight;
                    pointList.Add(new Point((int)pointX, (int)point2Y));
                    return pointList;
                }

                private List<Point> GtxGetPoints(BoardLink link)
                {
                    double offScale = 0;//该条link的相对同类link中的占比
                    var pointList = new List<Point>();
                    double pointX = 0;  //点的X坐标

                    int areaWidth = _vpxRect.Size.Width / 3;//区域宽度
                    if (link.FirstEndType == EndType.FPGA)
                    {
                        //link的端1连接到FPGA就没有点集
                        return null;
                    }

                    //获取点的X的坐标
                    offScale = GetLinkOffsetScaleSqu(link);
                    pointX = _vpxRect.Location.X + areaWidth - (offScale * areaWidth);

                    pointList.Add(new Point((int)pointX, _vpxRect.Location.Y));

                    double point2Y = _point2OriginY - offScale * _point2Hight;
                    pointList.Add(new Point((int)pointX, (int)point2Y));
                    return pointList;
                }

                private List<Point> LvdsGetPoints(BoardLink link)
                {
                    //Lvds的Link分布点的算法与Gtx的是一致的
                    return GtxGetPoints(link);
                }
            }
        }
    }
}
