using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.IO;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.OtherView;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// 机箱类
    /// </summary>
    public class Container : BaseView
    {
        public string Type { get; set; }            //机箱的型号
        public string BackPlaneName { get; set; }   //背板的名字
        public BackPlane _backPlane { get; private set; }              //包含的背板
        public Dictionary<int, string> BoardNameDir = new Dictionary<int, string>();   //key:槽位号；value:板卡名称
        public VpxEndView[] _boardViews;            //包含的板卡视图集
        private DrawContainer _drawer;              //机箱的画图类
        private BaseView ChoosedBv;                 //机箱图像中被选中的图元

        public Container() { }

        public override void DrawView(Graphics g){}

        public override void DrawView(Graphics g, Rectangle rect)
        {
            _drawer = new DrawContainer(this, g, rect);
            _drawer.Draw();
        }

        //画图不画连接示意区
        public void DrawViewNoIndicate(Graphics g, Rectangle rect)
        {
            _drawer = new DrawContainer(this, g, rect);
            _drawer.NoIndicate = true;
            _drawer.Draw();
        }

        public override Size GetViewSize()
        {
            return new Size(800, 400);
        }

        //初始化背板的画图器
        public void InitDrawContainer(Graphics g, Rectangle rect)
        {
            _drawer = new DrawContainer(this, g, rect);
        }

        //该槽位没放板卡,以后要优化判断
        public bool IsContainBoard(int slotNum)
        {
            if (this._backPlane.IsConnetctAreaSlot(slotNum))//槽位号为外连接区则直接返回true
            {
                return true;
            }
            return IsContainBoard(BoardNameDir[slotNum]);
        }

        public bool IsContainBoard(string boardName)
        {
            if ((boardName == "-请输入-") || (boardName == "无"))
            {
                return false;
            }
            return true;
        }

        public override void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetContainerPath(), this.Name);
            //先判断一些文件是否存在
            if (!PathManager.CheckFile(xmlPath))
            {
                return;
            }
            //保存XML文件
            XDocument xd = new XDocument(
                new XElement("Container",
                    new XAttribute("Name", this.Name),
                    new XAttribute("Type", this.Type),
                    new XAttribute("BackPlaneName", this.BackPlaneName),
                    new XElement("Boards")
                    )
                );
            //找到根节点
            XElement rt = xd.Element("Container");
            XElement boards = rt.Element("Boards");
            foreach (var soltInfo in this.BoardNameDir)
            {
                boards.Add(new XElement("Board",
                    new XAttribute("slotNum", soltInfo.Key),
                    new XAttribute("boardName", soltInfo.Value)
                    ));
            }
            xd.Save(xmlPath);
        }

        public override BaseView CreateObjectByName(string objectName)
        {
            Container container = new Container();
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetContainerPath(), objectName);
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("CreateObject_Container:没有该Container对应的XML文件！");
                return null;
            }

            XDocument xd = XDocument.Load(xmlPath);
            //根元素的Attribute
            XElement rt = xd.Element("Container");
            container.Name = rt.Attribute("Name").Value;
            container.Type = rt.Attribute("Type").Value;
            container.BackPlaneName = rt.Attribute("BackPlaneName").Value;
            container._backPlane = BaseViewFactory<BackPlane>.CreateByName(container.BackPlaneName);
            container._boardViews = new VpxEndView[container._backPlane.SlotsNum];

            XElement boards = rt.Element("Boards");
            foreach (var board in boards.Elements())
            {
                int slotNum = int.Parse(board.Attribute("slotNum").Value);
                string boardName = board.Attribute("boardName").Value;
                container.AddOneBoard(slotNum, boardName);
            }
            return container;
        }

        private void AddOneBoard(int slotNum, string boardName)
        {
            this.BoardNameDir.Add(slotNum, boardName);

            if (IsContainBoard(slotNum))
            {
                this._boardViews[slotNum] = new BoardVpx(boardName);
            }
            else
            {
                this._boardViews[slotNum] = new EmptySlotVpx(boardName);
            }
        }

        public override void MouseEventHandler(object sender, MouseEventArgs e)
        {
            ChoosedBv = _drawer.GetChoosedBaseView(e);
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

        private class ContainerLink : BackPlane.BackPlaneLink
        {
            public bool IsConnectValid { get; private set; }      //该连接是否有效

            public ContainerLink(BackPlane.BackPlaneLink bpLink, bool isValid)
                : base(bpLink.FirstEndId, bpLink.FirstEndPostion, bpLink.SecondEndId, bpLink.SecondEndPostion, bpLink.LinkType)
            {
                IsConnectValid = isValid;
            }

            public override void DrawLine(Graphics graph, List<Point> line)
            {
                if (IsConnectValid)
                {
                    base.DrawLine(graph, line);
                }
                else
                {
                    base.DrawLineColor(graph, line, Pens.Yellow, Brushes.Yellow);
                }
            }
        }


        private class DrawContainer : IDrawer
        {
            Container _container;
            Graphics _graph;                                            //画板
            Rectangle _ctnRect;                                         //机箱对应的矩形
            Rectangle _bpRect;                                          //背板对应的矩形
            Rectangle[] _boardRects;                                    //板卡对应的矩形
            private Dictionary<ContainerLink, Point[]> _links;          //包含的连接及对应的点
            public Boolean NoIndicate { get; set; }                     //不画连接示意区标志

            public DrawContainer(Container ctn, Graphics g, Rectangle r)
            {
                _container = ctn;
                _graph = g;
                _ctnRect = r;
                _bpRect = r;
                NoIndicate = false;
                //初始化背板画图成员
                ctn._backPlane.InitDrawBackPlane(g, _bpRect);
                var drawBackPlane = ctn._backPlane._drawBackPlane;
                _boardRects = drawBackPlane.SlotRects;
                //给BoardView分配矩形区域
                for (int i = 0; i < ctn._boardViews.Length; i++)
                {
                    ctn._boardViews[i].AssignRect(_boardRects[i]);
                }
                //分配连接
                _links = new Dictionary<ContainerLink, Point[]>();
                foreach (var linkPair in drawBackPlane.LinkDir)
                {
                    BackPlane.BackPlaneLink link = linkPair.Key;
                    _links.Add(new ContainerLink(link, IsValidLine(link)), linkPair.Value);
                }
            }

            public BaseView GetChoosedBaseView(MouseEventArgs e)
            {
                BaseView resultBv;
                //先查鼠标位置是否在板卡里
                for (int i = 0; i < _container._boardViews.Length; i++)
                {
                    if (_boardRects[i].Contains(e.Location))
                    {
                        resultBv = _container._boardViews[i].GetChoosedBaseView(e);
                        if (resultBv != null)
                        {
                            return resultBv;
                        }
                    }
                }
                //检查是否在背板上
                return _container._backPlane._drawBackPlane.GetChoosedBaseView(e);
            }

            public Rectangle GetBaseViewRect(BaseView baseView, ref bool isFind)
            {
                //先在板卡里面找
                for (int i = 0; i < _container._boardViews.Length; i++)
                {
                    if (_container._boardViews[i] == baseView)
                    {
                        isFind = true;
                        return _boardRects[i];
                    }
                }

                //在背板里面找
                var drawBackPlane = _container._backPlane._drawBackPlane;
                return drawBackPlane.GetBaseViewRect(baseView, ref isFind);
            }

            //重定义一个Draw的函数
            public void Draw()
            {
                var choosedBv = _container.ChoosedBv;
                //画背板
                if (NoIndicate)
                {
                    _container._backPlane.DrawViewNoIndicate(_graph, _bpRect);
                }
                else
                {
                    _container._backPlane.DrawView(_graph, _bpRect);
                }

                //画Boards
                for (int i = 0; i < _container._boardViews.Length; i++)
                {
                    _container._boardViews[i].DrawView(_graph, _boardRects[i]);
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
                foreach (var linePair in _links)
                {
                    linePair.Key.EndRadius = _boardRects[0].Width / 20;
                    linePair.Key.DrawLine(_graph, linePair.Value.ToList());
                }
            }

            /// <summary>
            /// 判断一条Link的两端是否有效地连接了板卡的相关位置；
            /// </summary>
            /// <param name="link"></param>
            /// <returns></returns>
            private bool IsValidLine(BackPlane.BackPlaneLink link)
            {
                //排除无效的槽位
                if (!(_container.IsContainBoard(link.FirstEndId) && _container.IsContainBoard(link.SecondEndId)))
                {
                    return false;
                }

                //获取Link的端点1连接的板卡名字
                if (!_container._backPlane.IsConnetctAreaSlot(link.FirstEndId))
                {
                    Board end1Board = BaseViewFactory<Board>.CreateByName(_container.BoardNameDir[link.FirstEndId]);
                    if (!end1Board.IsLinkValidConnected(link, 1))
                    {
                        return false;
                    }
                }

                //获取Link的端点2连接的板卡名字
                if (!_container._backPlane.IsConnetctAreaSlot(link.SecondEndId))
                {
                    Board end2Board = BaseViewFactory<Board>.CreateByName(_container.BoardNameDir[link.SecondEndId]);
                    if (!end2Board.IsLinkValidConnected(link, 2))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
