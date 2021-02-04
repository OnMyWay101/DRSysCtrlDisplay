using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    /// <summary>
    /// 描述背板的一条连接
    /// </summary>
    public class BackPlaneLink : BaseLine
    {
        private const LinkLanes DefaultLanesNum = LinkLanes.X1;
        public int EndRadius { get; set; }
        public int FirstEndPostion { get; private set; }
        public int SecondEndPostion { get; private set; }

        public BackPlaneLink(int end1Id, int end1Pos, int end2Id, int end2Pos, LinkType type)
            : base(type, end1Id, end2Id, DefaultLanesNum)
        {
            FirstEndPostion = end1Pos;
            SecondEndPostion = end2Pos;
        }
        //用来判定两条link是否为相等
        public static bool IsEqual(BackPlaneLink link1, BackPlaneLink link2)
        {
            //1.同一条Link
            if (link1 == link2)
            {
                return true;
            }
            //2.不同Link但是end1,end2相反,视为equal
            if (link1.LinkType == link2.LinkType)
            {
                if (link1.FirstEndId == link2.SecondEndId
                    && link1.FirstEndPostion == link2.SecondEndPostion    //link1的end1与link2的end2相等
                    && link1.SecondEndId == link2.FirstEndId
                    && link1.SecondEndPostion == link2.FirstEndPostion)   //link1的end2与link2的end1相等
                {
                    return true;
                }
            }
            return false;
        }

        public override void DrawLine(Graphics graph, List<Point> line)
        {
            DrawLineColor(graph, line, Pens.Black, Brushes.Black);
        }

        protected void DrawLineColor(Graphics graph, List<Point> line, Pen pen, Brush brush)
        {
            Point p0 = line[0];
            Point p1 = line[1];
            graph.DrawLine(pen, p0, p1);
            graph.DrawEllipse(pen, p0.X - EndRadius, p0.Y - EndRadius, 2 * EndRadius, 2 * EndRadius);
            graph.FillEllipse(brush, p0.X - EndRadius, p0.Y - EndRadius, 2 * EndRadius, 2 * EndRadius);
            graph.DrawEllipse(pen, p1.X - EndRadius, p1.Y - EndRadius, 2 * EndRadius, 2 * EndRadius);
            graph.FillEllipse(brush, p1.X - EndRadius, p1.Y - EndRadius, 2 * EndRadius, 2 * EndRadius);
        }
    }
}
