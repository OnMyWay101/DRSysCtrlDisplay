using System.Drawing;
using DRSysCtrlDisplay.Models;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    using Princeple;
    public class ZYNQViewModel : BaseDrawerCore
    {
        ZYNQ _zynq;

        public ZYNQViewModel(ZYNQ zynq, Rectangle rect)
            : base(rect)
        {
            _zynq = zynq;
        }
        public ZYNQViewModel(ZYNQ zynq)
        {
            _zynq = zynq;
        }

        public override void DrawView(Graphics g)
        {
            g.DrawRectangle(ComputeNodeColor.Pen_PL, base._rect);
            g.FillRectangle(ComputeNodeColor.Brushes_PL, base._rect);
            base.AddSentence(g, "ZYNQ");
        }

        public override void DrawView(Graphics g, Pen pen, Brush brush)
        {
            g.DrawRectangle(pen, base._rect);
            g.FillRectangle(brush, base._rect);
            base.AddSentence(g, "ZYNQ");
        }

        public override void DrawView(Graphics g, string name)
        {
            g.DrawRectangle(ComputeNodeColor.Pen_PL, base._rect);
            g.FillRectangle(ComputeNodeColor.Brushes_PL, base._rect);
            base.AddSentence(g, name);
        }

        public override void ChoosedDrawView(Graphics g, string name)
        {
            Rectangle marginRect = base.GetMarginRect();
            DrawView(g, name);
            g.DrawRectangle(Pens.Red, marginRect);
        }

        public override void ChoosedDrawView(Graphics g)
        {
            Rectangle marginRect = base.GetMarginRect();
            DrawView(g);
            g.DrawRectangle(Pens.Red, marginRect);
        }

        public override object GetModelInstance()
        {
            return _zynq;
        }
    }
}
