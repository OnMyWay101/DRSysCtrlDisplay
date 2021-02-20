using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using DRSysCtrlDisplay.Models;

namespace DRSysCtrlDisplay
{
    public class ShowViewPanel : Panel
    {
        public BaseDrawer ShowView { get; private set; }   //图像类的基类，用于画图
        public Princeple.FormType FormType { get; private set; }
        public float ZoomFactor { get; private set; }
        private TreeNode _treeNode;
        private PointF _viewOffset;
        public const int ViewMargin = 100;//边距100 

        public ShowViewPanel(TreeNode node)
        {
            //初始化成员变量
            this.DoubleBuffered = true;//重绘双缓冲开启，防止界面闪烁
            base.AutoScroll = true;
            _treeNode = node;
            NodeInfo info = (NodeInfo)node.Tag;
            FormType = info._formType;
            ZoomFactor = 1;
            _viewOffset = new PointF();

            switch (FormType)
            {
                case Princeple.FormType.PPC:
                    PPC ppc = ModelFactory<PPC>.CreateByName(node.Text);
                    ShowView = new PPCViewModel(ppc);
                    break;
                case Princeple.FormType.FPGA:
                    FPGA fpga = ModelFactory<FPGA>.CreateByName(node.Text);
                    ShowView = new FPGAViewModel(fpga);
                    break;
                case Princeple.FormType.ZYNQ:
                    ZYNQ zynq = ModelFactory<ZYNQ>.CreateByName(node.Text);
                    ShowView = new ZYNQViewModel(zynq);
                    break;
                case Princeple.FormType.BOARD:
                    Board board = ModelFactory<Board>.CreateByName(node.Text);
                    ShowView = new BoardViewModel(board);
                    break;
                case Princeple.FormType.BACKPLANE:
                    BackPlane bp = ModelFactory<BackPlane>.CreateByName(node.Text);
                    ShowView = new BackPlaneViewModel(bp);
                    break;
                case Princeple.FormType.CONTIANER:
                    Container ctn = ModelFactory<Container>.CreateByName(node.Text);
                    ShowView = new ContainerViewModel(ctn);
                    break;
                case Princeple.FormType.COMPONENT:
                    Component cmp = ModelFactory<Component>.CreateByName(node.Text);
                    ShowView = new ComponentViewModel(cmp);
                    break;
                case Princeple.FormType.SYSTEM:
                    SystemStru sys = ModelFactory<SystemStru>.CreateByName(node.Text);
                    ShowView = new SystemStruViewModel(sys);
                    break;
                case Princeple.FormType.TOPO:
                    var sysName = node.Text.Substring(node.Text.IndexOf(':') + 1);
                    SystemStru sys2 = ModelFactory<SystemStru>.CreateByName(sysName);
                    ShowView = new StaticTopo(sys2);
                    break;
                case Princeple.FormType.APP:
                    var sysNode = node.Parent.Nodes[0];
                    var sysSName = sysNode.Text.Substring(sysNode.Text.IndexOf(':') + 1);
                    SystemStru sys3 = ModelFactory<SystemStru>.CreateByName(sysSName);
                    var sysStatic = new StaticTopo(sys3);
                    ShowView = new DynamicTopo(sysStatic);

                    ((DynamicTopo)ShowView).MatchApps(GetNodeCmps(node));
                    break;
                case Princeple.FormType.STATUS:
                    ShowView = new Status();
                    break;
                default://无对应界面类型退出
                    return;
            }
            SetViewSize();

            //绑定界面点击事件的处理
            if(ShowView as IDrawerChoosed != null) 
                this.MouseClick += new MouseEventHandler(((IDrawerChoosed)ShowView).MouseEventHandler);
            if(ShowView as IDrawerNotify != null)
                info.NodeInfoChanged += new Action<TargetNode>(((IDrawerNotify)ShowView).OnNodeInfoChanged);
            ShowView.RedrawRequst += new Action(OnShowViewRedrawRequst);
            this.Scroll += new ScrollEventHandler(ShowViewPanel_Scroll);
            this.MouseWheel += new MouseEventHandler(ShowViewPanel_MouseWheel);
        }

        private Component[] GetNodeCmps(TreeNode tNode)
        {
            List<Component> cmps = new List<Component>();
            foreach (TreeNode node in tNode.Nodes)
            {
                var cmp = ModelFactory<Component>.CreateByName(node.Text);
                cmps.Add(cmp);
            }
            return cmps.ToArray();
        }

        private void OnShowViewRedrawRequst()
        {
            this.Invalidate();
        }

        //设置显示区域，方便产生滚动条
        private void SetViewSize()
        {
            var size = ShowView.GetViewSize();
            this.AutoScrollMinSize = new Size(size.Width + 2 * ViewMargin, size.Height + 2 * ViewMargin);
        }

        #region 事件处理函数

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.ScaleTransform(ZoomFactor, ZoomFactor);
            g.TranslateTransform(_viewOffset.X, _viewOffset.Y);

            var size = ShowView.GetViewSize();
            ShowView.DrawView();
        }

        void ShowViewPanel_Scroll(object sender, ScrollEventArgs e)
        {
            _viewOffset.X = -1 * this.HorizontalScroll.Value;
            _viewOffset.Y = -1 * this.VerticalScroll.Value;
            OnShowViewRedrawRequst();
        }

        void ShowViewPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            _viewOffset.X = -1 * this.HorizontalScroll.Value;
            _viewOffset.Y = -1 * this.VerticalScroll.Value;
            OnShowViewRedrawRequst();
        }

        #endregion 事件处理函数
    }

}
