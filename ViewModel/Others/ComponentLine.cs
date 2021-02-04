using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    //构件连接
    public class ComponentLine : BaseLine
    {
        public ComponentLine(LinkType type, int firstId, int secondId, LinkLanes lanesNum)
            : base(type, firstId, secondId, lanesNum)
        { }

        public override void DrawLine(Graphics graph, List<Point> line)
        {
            //点集为空则不画
            if (line == null)
            {
                return;
            }
            Pen linePen;
            switch (base.LinkType)
            {
                case LinkType.EtherNet:
                    linePen = ConnectAreaColor.Pen_EtherNet;
                    break;
                case LinkType.RapidIO:
                    linePen = ConnectAreaColor.Pen_RapidIO;
                    break;
                case LinkType.GTX:
                    linePen = ConnectAreaColor.Pen_GTX;
                    break;
                default: //LinkType.LVDS
                    linePen = ConnectAreaColor.Pen_LVDS;
                    break;
            }
            graph.DrawLines(linePen, line.ToArray());
        }
    }
}
