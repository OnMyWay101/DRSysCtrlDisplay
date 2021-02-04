using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

namespace DRSysCtrlDisplay
{
    public class ShowViewPanel : Panel
    {
        public BaseView ShowView { get; private set; }   //图像类的基类，用于画图
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
                    ShowView = BaseViewFactory<PPCViewModel>.CreateByName(node.Text);
                    break;
                case Princeple.FormType.FPGA:
                    ShowView = BaseViewFactory<FPGAViewModel>.CreateByName(node.Text);
                    break;
                case Princeple.FormType.ZYNQ:
                    ShowView = BaseViewFactory<ZYNQViewModel>.CreateByName(node.Text);
                    break;
                case Princeple.FormType.BOARD:
                    ShowView = BaseViewFactory<BoardViewModel>.CreateByName(node.Text);
                    break;
                case Princeple.FormType.BACKPLANE:
                    ShowView = BaseViewFactory<BackPlaneViewModel>.CreateByName(node.Text);
                    break;
                case Princeple.FormType.CONTIANER:
                    ShowView = BaseViewFactory<ContainerViewModel>.CreateByName(node.Text);
                    break;
                case Princeple.FormType.COMPONENT:
                    ShowView = BaseViewFactory<ComponentViewModel>.CreateByName(node.Text);
                    break;
                case Princeple.FormType.SYSTEM:
                    ShowView = BaseViewFactory<SystemStruViewModel>.CreateByName(node.Text);
                    break;
                case Princeple.FormType.TOPO:
                    var sysName = node.Text.Substring(node.Text.IndexOf(':') + 1);
                    ShowView = new StaticTopo(BaseViewFactory<SystemStruViewModel>.CreateByName(sysName));
                    break;
                case Princeple.FormType.APP:
                    var sysNode = node.Parent.Nodes[0];
                    var sysSName = sysNode.Text.Substring(sysNode.Text.IndexOf(':') + 1);
                    ShowView = new DynamicTopo(new StaticTopo(BaseViewFactory<SystemStruViewModel>.CreateByName(sysSName)));

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
            this.MouseClick += new MouseEventHandler(ShowView.MouseEventHandler);
            ShowView.RedrawRequst += new Action(OnShowViewRedrawRequst);
            info.NodeInfoChanged += new Action<TargetNode>(ShowView.OnNodeInfoChanged);
            this.Scroll += new ScrollEventHandler(ShowViewPanel_Scroll);
            this.MouseWheel += new MouseEventHandler(ShowViewPanel_MouseWheel);
        }

        private ComponentViewModel[] GetNodeCmps(TreeNode tNode)
        {
            List<ComponentViewModel> cmps = new List<ComponentViewModel>();
            foreach (TreeNode node in tNode.Nodes)
            {
                var cmp = BaseViewFactory<ComponentViewModel>.CreateByName(node.Text);
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
            ShowView.DrawView(g, new Rectangle(ViewMargin, ViewMargin, size.Width, size.Height));
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
