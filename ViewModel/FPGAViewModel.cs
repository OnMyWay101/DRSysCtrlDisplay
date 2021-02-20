using System.Drawing;
using DRSysCtrlDisplay.Models;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    using Princeple;
    public class FPGAViewModel : BaseDrawerCore
    {
        FPGA _fpga;
        public FPGAViewModel(FPGA fpga, Rectangle rect)
            : base(rect)
        {
            _fpga = fpga;
        }

        public FPGAViewModel(FPGA fpga)
        {
            _fpga = fpga;
        }

        public override void DrawView(Graphics g)
        {
            g.DrawRectangle(ComputeNodeColor.Pen_FPGA, base._rect);
            g.FillRectangle(ComputeNodeColor.Brushes_FPGA, base._rect);
            base.AddSentence(g, "FPGA");
        }

        public override void DrawView(Graphics g, Pen pen, Brush brush)
        {
            g.DrawRectangle(pen, base._rect);
            g.FillRectangle(brush, base._rect);
            base.AddSentence(g, "FPGA");
        }
        public override void DrawView(Graphics g, string name)
        {
            g.DrawRectangle(ComputeNodeColor.Pen_FPGA, base._rect);
            g.FillRectangle(ComputeNodeColor.Brushes_FPGA, base._rect);
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
    }
}
