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

        public PPCViewModel(PPC ppc, Graphics g, Rectangle rect)
            : base(g, rect)
        {
            _ppc = ppc;
        }

        public PPCViewModel(PPC ppc)
        {
            _ppc = ppc;
        }

        public override void DrawView()
        {
            base._graph.DrawRectangle(ComputeNodeColor.Pen_PPC, base._rect);
            base._graph.FillRectangle(ComputeNodeColor.Brushes_PPC, base._rect);
            base.AddSentence("PPC");
        }

        public override void DrawView(Pen pen, Brush brush)
        {
            base._graph.DrawRectangle(pen, base._rect);
            base._graph.FillRectangle(brush, base._rect);
            base.AddSentence("PPC");
        }

        public override void DrawView(string name)
        {
            base._graph.DrawRectangle(Princeple.ComputeNodeColor.Pen_PPC, base._rect);
            base._graph.FillRectangle(Princeple.ComputeNodeColor.Brushes_PPC, base._rect);
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
