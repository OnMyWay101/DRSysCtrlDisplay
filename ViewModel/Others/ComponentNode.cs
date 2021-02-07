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
        public int NodeNum { get; private set; }                //Node节点对应的序号
        [BrowsableAttribute(false)]
        public BaseViewCore NodeObject { get; private set; }    //Node节点对应的实例

        public ComponentNode(EndType nodeType, string nodeName, int nodeNum, BaseViewCore nodeObject)
            : base(nodeType, nodeName)
        {
            this.NodeNum = nodeNum;
            this.NodeObject = nodeObject;
        }
        public override void DrawNode(Graphics graph, Rectangle rect)
        {
            NodeObject.DrawView(graph, rect, base.Name);
        }

        public override void DrawChoosedNode(Graphics graph, Rectangle rect)
        {
            NodeObject.ChoosedDrawView(graph, rect, base.Name);
        }
    }
}
