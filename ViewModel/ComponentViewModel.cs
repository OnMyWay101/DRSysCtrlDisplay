using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.Models;
using DRSysCtrlDisplay.ViewModel.Others;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    public class ComponentViewModel : BaseDrawer, IDrawerChoosed
    {
        private Models.Component _component;        //构件的实例
        private TopoNetView<ComponentNode, ComponentLine> _topoView;
        public BaseDrawer ChoosedBv { get; set; }


        public ComponentViewModel(Models.Component cmp, Graphics g, Rectangle rect)
            : base(g, rect)
        {
            _component = cmp;
            _topoView = new TopoNetView<ComponentNode, ComponentLine>(base._graph, base._rect, _component.CmpTopoNet);
        }

        public ComponentViewModel(Models.Component cmp)
        {
            _component = cmp;
        }

        public override void Init(Graphics g, Rectangle rect)
        {
            base.Init(g, rect);
            _topoView = new TopoNetView<ComponentNode, ComponentLine>(base._graph, base._rect, _component.CmpTopoNet);
        }

        #region 重载虚函数
        public override void DrawView()
        {
            _topoView.DrawView();
        }
        public override Size GetViewSize()
        {
            return new Size(800, 400);
        }
        #endregion 重载虚函数

        #region 实现接口
        public BaseDrawer GetChoosedBaseView(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseEventHandler(object sender, MouseEventArgs e)
        {
            //处理鼠标事件放在TopoNetView中实现
            _topoView.MouseEventHandler(sender, e);
            //Todo:切换相关属性的显示
            base.TriggerRedrawRequst();
        }
        #endregion 实现接口

    }
}
