using System.ComponentModel;
using System.Drawing;
using DRSysCtrlDisplay.Models;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    using Princeple;
    /// <summary>
    /// PPC类，包含画图接口
    /// </summary>
    public class PPCViewModel : BaseDrawerCore
    {
        PPC _ppc;

        public PPCViewModel(PPC ppc, Rectangle rect)
            : base(rect)
        {
            _ppc = ppc;
        }

        public PPCViewModel(PPC ppc)
        {
            _ppc = ppc;
        }

        public override void DrawView(Graphics g)
        {
            g.DrawRectangle(ComputeNodeColor.Pen_PPC, base._rect);
            g.FillRectangle(ComputeNodeColor.Brushes_PPC, base._rect);
            base.AddSentence(g, "PPC");
        }

        public override void DrawView(Graphics g, Pen pen, Brush brush)
        {
            g.DrawRectangle(pen, base._rect);
            g.FillRectangle(brush, base._rect);
            base.AddSentence(g, "PPC");
        }

        public override void DrawView(Graphics g, string name)
        {
            g.DrawRectangle(Princeple.ComputeNodeColor.Pen_PPC, base._rect);
            g.FillRectangle(Princeple.ComputeNodeColor.Brushes_PPC, base._rect);
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
            return _ppc;
        }
    }
}
