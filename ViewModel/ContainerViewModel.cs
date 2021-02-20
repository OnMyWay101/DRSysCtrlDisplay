using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.IO;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.OtherView;
using System.ComponentModel;
using DRSysCtrlDisplay.ViewModel.Others;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;
using DRSysCtrlDisplay.Models;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// 机箱类
    /// </summary>
    public class ContainerViewModel : BaseDrawer, IDrawerChoosed
    {
        private Models.Container _container;                        //包含的机箱实例

        public BackPlaneViewModel _bpView;                         //包含的背板图像
        private Rectangle[] _boardRects;                            //板卡集对应的矩形集合
        public PlaneVpx[] _boardViews;                              //包含的板卡视图集
        private Dictionary<ContainerLink, Point[]> _links;          //包含的连接及对应的点
        public BaseDrawer ChoosedBv { get; set; }                   //当前视图被选中的图元

        public ContainerViewModel(Models.Container container, Rectangle rect)
        {
            _container = container;
            Init(rect);
        }

        public ContainerViewModel(Models.Container container)
        {
            _container = container;
        }

        public override void Init(Rectangle rect)
        {
            base.Init(rect);
            Init();
        }

        #region 重载虚函数
        public override void DrawView(Graphics g)
        {
            //画背板
            _bpView.DrawView(g);

            //画Boards
            for (int i = 0; i < _boardViews.Length; i++)
            {
                _boardViews[i].DrawView(g);
            }

            //稍后画选中的图元
            if (ChoosedBv != null)
            {
                ChoosedBv.ChoosedDrawView(g);
            }

            //画Links
            foreach (var linePair in _links)
            {
                linePair.Key.EndRadius = _boardRects[0].Width / 20;
                linePair.Key.DrawLine(g, linePair.Value.ToList());
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
            //先查鼠标位置是否在板卡里
            for (int i = 0; i < _boardViews.Length; i++)
            {
                if (_boardRects[i].Contains(e.Location))
                {
                    return _boardViews[i];
                }
            }
            //检查是否在背板上
            return _bpView.GetChoosedBaseView(e);
        }

        #endregion 实现接口

        private void Init()
        {
            //初始化背板画图对象
            var bp = ModelFactory<BackPlane>.CreateByName(_container.BackPlaneName);
            _boardViews = new PlaneVpx[bp.SlotsNum];
            _bpView = new BackPlaneViewModel(bp, base._rect);
            _boardRects = _bpView.SlotRects;

            //初始化_boardViews
            foreach (var pair in _container.BoardNameDir)
            {
                var rect = _boardRects[pair.Key];
                if(_container.IsContainBoard(pair.Value))
                {
                    _boardViews[pair.Key] = new BoardVpx(rect, pair.Value);
                }
                else
                {
                    _boardViews[pair.Key] = new EmptySlotVpx(rect, pair.Value);
                }
            }

            //分配连接
            _links = new Dictionary<ContainerLink, Point[]>();
            foreach (var linkPair in _bpView.LinkDir)
            {
                BackPlaneLink link = linkPair.Key;
                _links.Add(new ContainerLink(link, IsValidLine(link)), linkPair.Value);
            }
        }

        /// <summary>
        /// 判断一条Link的两端是否有效地连接了板卡的相关位置；
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private bool IsValidLine(BackPlaneLink link)
        {
            //排除无效的槽位
            if (!(_container.IsContainBoard(link.FirstEndId) && _container.IsContainBoard(link.SecondEndId)))
            {
                return false;
            }

            //获取Link的端点1连接的板卡名字
            if (!_bpView._bp.IsConnetctAreaSlot(link.FirstEndId))
            {
                Board end1Board = ModelFactory<Board>.CreateByName(_container.BoardNameDir[link.FirstEndId]);
                if (!end1Board.IsLinkValidConnected(link, 1))
                {
                    return false;
                }
            }

            //获取Link的端点2连接的板卡名字
            if (!_bpView._bp.IsConnetctAreaSlot(link.FirstEndId))
            {
                Board end2Board = ModelFactory<Board>.CreateByName(_container.BoardNameDir[link.SecondEndId]);
                if (!end2Board.IsLinkValidConnected(link, 2))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
