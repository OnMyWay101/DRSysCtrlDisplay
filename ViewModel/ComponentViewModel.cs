using System;
using System.Windows.Forms;
using System.Drawing;
using DRSysCtrlDisplay.ViewModel.Others;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    public class ComponentViewModel : BaseDrawer, IDrawerChoosed
    {
        private Models.Component _component;        //构件的实例
        private TopoNetView<ComponentNode, ComponentLine> _topoView;
        public BaseDrawer ChoosedBv { get; set; }


        public ComponentViewModel(Models.Component cmp, Rectangle rect)
        {
            _component = cmp;
            Init(rect);
        }

        public ComponentViewModel(Models.Component cmp)
        {
            _component = cmp;
        }

        public override void Init( Rectangle rect)
        {
            base.Init(rect);
            _topoView = new TopoNetView<ComponentNode, ComponentLine>(base._rect, _component.CmpTopoNet);
        }

        #region 重载虚函数
        public override void DrawView(Graphics g)
        {
            _topoView.DrawView(g);
        }
        public override Size GetViewSize()
        {
            return new Size(800, 400);
        }

        public override object GetModelInstance()
        {
            return _component;
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
            //切换属性框的显示
            if (_topoView.ChoosedBv != null)
            {
                var chooseNode = _topoView.ChoosedBv as ComponentNode;
                PropertyForm.Show(chooseNode.NodeObject);
            }
            else
            {
                PropertyForm.Show(this.GetModelInstance());
            }
            base.TriggerRedrawRequst();
        }
        #endregion 实现接口

    }
}
