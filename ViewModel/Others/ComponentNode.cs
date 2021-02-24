using DRSysCtrlDisplay.Models;
using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    //构件节点
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ComponentNode : BaseNode
    {
        [Category("\t基本信息"), Description("节点序号"), ReadOnly(true)]
        public int NodeId { get; private set; }                //Node节点对应的序号

        [Category("节点信息"), Description("节点属性信息"), ReadOnly(true)]
        public CmpNode NodeObject { get; private set; }         //Node节点对应的实例

        public ComponentNode(int nodeNum, CmpNode nodeObject)
            : base(nodeObject.NodeType, nodeObject.Name)
        {
            this.NodeId = nodeNum;
            this.NodeObject = nodeObject;
        }
        public override void DrawNode(Graphics graph, Rectangle rect)
        {
            BaseDrawerCore coreView = base.GetBaseDrawerCore(NodeObject.Obj,rect);
            coreView.DrawView(graph);
        }

        public override void DrawChoosedNode(Graphics graph, Rectangle rect)
        {
            BaseDrawerCore coreView = base.GetBaseDrawerCore(NodeObject.Obj, rect);
            coreView.ChoosedDrawView(graph);
        }
    }
}
