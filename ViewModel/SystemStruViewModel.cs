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
using DRSysCtrlDisplay.Models;
using DRSysCtrlDisplay.ViewModel.Others;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    public class SystemStruViewModel : BaseDrawer
    {
        const double _cntScale = 9.0 / 10;                                          //机箱长度占单个矩形块的长宽比例
        private SystemStru _sys;                                                    //对应的系统实例
        public ContainerViewModel[] CntsArray;                                      //机箱数组
        public Rectangle[] CntRects { get; private set; }                           //包含的机箱图像矩形位置集合；
        public Dictionary<SystemStruLink, Point[]> LinkDir { get; private set; }    //包含的连接及对应的点

        public SystemStruViewModel(SystemStru sys, Graphics g, Rectangle rect)
            : base(g, rect)
        {
            _sys = sys;
        }

        public SystemStruViewModel(SystemStru sys)
        {
            _sys = sys;
        }

        #region 重载虚函数

        public override void DrawView()
        {
            //画机箱
            for (int i = 0; i < _sys.CntsNum; i++)
            {
                var curCnt = CntsArray[i];
                curCnt.DrawView();
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

        public override Size GetViewSize()
        {
            int width = 800;    //宽度为固定值800
            int height = 400 * _sys.CntsNum;//高度为一个机箱400，机箱数*400
            return new Size(width, height);
        }

        #endregion 重载虚函数

        public void Init()
        {
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
            int childHeight = base._rect.Height / cntsNum;

            //cntRect的高，宽，X坐标都恒定，Y会有变化
            int height = (int)(childHeight * _cntScale);
            int width = (int)(base._rect.Width * _cntScale);
            int pointX = base._rect.X;
            int pointY = base._rect.Y + (int)(childHeight * (1 - _cntScale));

            for (int i = 0; i < cntsNum; i++)
            {
                CntRects[i] = new Rectangle(pointX, pointY, width, height);
                var cnt = ModelFactory<Container>.CreateByName(_sys.CntNames[i]);
                CntsArray[i] = new ContainerViewModel(cnt, base._graph, CntRects[i]);//创建对应机箱的画图器
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
            int pointXWidth = (int)(base._rect.Width * (1 - _cntScale) / 2);      //pointX所在区域的宽度
            int areaBase = (int)(base._rect.X + base._rect.Width * _cntScale);      //走线区的起始位置
            int pointXBase = (type == LinkType.EtherNet ? areaBase : areaBase + pointXWidth);    //pointX的起始位置

            foreach (var link in links)
            {
                ContainerViewModel cnt1 = CntsArray[link.FirstEndId];
                ContainerViewModel cnt2 = CntsArray[link.SecondEndId];
                double scale = (links.IndexOf(link) + 1.0) / (linksNum + 1.0);

                Point p1 = cnt1._bpView.GetInterfacePoint(type);
                Point p4 = cnt2._bpView.GetInterfacePoint(type);
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

        private void AddCntNames()
        {
            for (int i = 0; i < CntRects.Length; i++)
            {
                var cntRect = CntRects[i];//当前的机箱视图
                int nameRectWidth = cntRect.Width / 10;
                var nameRect = new Rectangle(cntRect.X - nameRectWidth, cntRect.Y, nameRectWidth, cntRect.Height);
                BaseDrawer.AddDirctionSentence(_graph, nameRect, _sys.CntNames[i], false);
            }
        }
    }
}
