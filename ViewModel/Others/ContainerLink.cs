using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    public class ContainerLink : BackPlaneLink
    {
        public bool IsConnectValid { get; private set; }      //该连接是否有效

        public ContainerLink(BackPlaneLink bpLink, bool isValid)
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
}
