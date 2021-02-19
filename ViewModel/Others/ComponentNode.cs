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
    public class ComponentNode : BaseNode
    {
        public int NodeId { get; private set; }                //Node节点对应的序号
        [BrowsableAttribute(false)]
        public CmpNode NodeObject { get; private set; }         //Node节点对应的实例

        public ComponentNode(int nodeNum, CmpNode nodeObject)
            : base(nodeObject._nodeType, nodeObject.Name)
        {
            this.NodeId = nodeNum;
            this.NodeObject = nodeObject;
        }
        public override void DrawNode(Graphics graph, Rectangle rect)
        {
            BaseDrawerCore coreView = base.GetBaseDrawerCore(NodeObject._object, graph, rect);
            coreView.DrawView();
        }

        public override void DrawChoosedNode(Graphics graph, Rectangle rect)
        {
            BaseDrawerCore coreView = base.GetBaseDrawerCore(NodeObject._object, graph, rect);
            coreView.ChoosedDrawView();
        }
    }
}
