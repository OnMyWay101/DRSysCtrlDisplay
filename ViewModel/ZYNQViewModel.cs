using System.Drawing;
using DRSysCtrlDisplay.Models;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    using Princeple;
    public class ZYNQViewModel : BaseDrawerCore
    {
        ZYNQ _zynq;

        public ZYNQViewModel(ZYNQ zynq, Graphics g, Rectangle rect)
            : base(g, rect)
        {
            _zynq = zynq;
        }
        public ZYNQViewModel(ZYNQ zynq)
        {
            _zynq = zynq;
        }

        public override void DrawView()
        {
            base._graph.DrawRectangle(ComputeNodeColor.Pen_PL, base._rect);
            base._graph.FillRectangle(ComputeNodeColor.Brushes_PL, base._rect);
            base.AddSentence("ZYNQ");
        }

        public override void DrawView(Pen pen, Brush brush)
        {
            base._graph.DrawRectangle(pen, base._rect);
            base._graph.FillRectangle(brush, base._rect);
            base.AddSentence("ZYNQ");
        }

        public override void DrawView(string name)
        {
            base._graph.DrawRectangle(ComputeNodeColor.Pen_PL, base._rect);
            base._graph.FillRectangle(ComputeNodeColor.Brushes_PL, base._rect);
            base.AddSentence(name);
        }

        public override void ChoosedDrawView(string name)
        {
            Rectangle marginRect = base.GetMarginRect();
            DrawView(name);
            base._graph.DrawRectangle(Pens.Red, marginRect);
        }

        public override void ChoosedDrawView()
        {
            Rectangle marginRect = base.GetMarginRect();
            DrawView();
            base._graph.DrawRectangle(Pens.Red, marginRect);
        }
    }
}
