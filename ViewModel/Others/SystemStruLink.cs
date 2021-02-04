using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    public class SystemStruLink : BackPlaneLink
    {
        public SystemStruLink(int end1Id, int end1Pos, int end2Id, int end2Pos, LinkType type, LinkLanes lanes)
            : base(end1Id, end1Pos, end2Id, end2Pos, type)
        {
            base.LanesNum = lanes;
            base.EndRadius = 3;
        }

        public override void DrawLine(Graphics graph, List<Point> line)
        {
            graph.DrawLines(Pens.Black, line.ToArray());

            graph.DrawEllipse(Pens.Black, line[0].X - EndRadius, line[0].Y - EndRadius, 2 * EndRadius, 2 * EndRadius);
            graph.FillEllipse(Brushes.Black, line[0].X - EndRadius, line[0].Y - EndRadius, 2 * EndRadius, 2 * EndRadius);
            graph.DrawEllipse(Pens.Black, line[3].X - EndRadius, line[3].Y - EndRadius, 2 * EndRadius, 2 * EndRadius);
            graph.FillEllipse(Brushes.Black, line[3].X - EndRadius, line[3].Y - EndRadius, 2 * EndRadius, 2 * EndRadius);
        }
    }
}
