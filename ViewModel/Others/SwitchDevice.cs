using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    using Princeple;
    public enum SwitchCategory
    {
        EtherNetSw,
        RioSw
    }

    /// <summary>
    /// 描述板卡上面交换机的类
    /// </summary>
    public class SwitchDevice : BaseDrawer
    {
        public SwitchCategory Category { get; set; }    //交换的种类，这里只包含EtherNet交换机和Rio交换机
        public string Type { get; set; }                //交换机的型号

        public SwitchDevice(SwitchCategory category, string type)
            : base(null, new Rectangle())
        {
            Category = category;
            Type = type;
        }

        public void SetDrawingTools(Graphics g, Rectangle rect)
        {
            base._graph = g;
            base._rect = rect;
        }

        public override void DrawView()
        {
            if (base._graph == null) return;

            var pen = (Category == SwitchCategory.EtherNetSw ? ConnectAreaColor.Pen_EtherNet : ConnectAreaColor.Pen_RapidIO);
            var brush = (Category == SwitchCategory.EtherNetSw ? ConnectAreaColor.Brushes_EtherNet : ConnectAreaColor.Brushes_RapidIO);
            base._graph.DrawEllipse(pen, base._rect);
            base._graph.FillEllipse(brush, base._rect);

            //给交换机加上“SW”字样
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            base._graph.DrawString("SW", new Font("Arial", 16), Brushes.Black, base._rect, drawFormat);
        }

        public override void ChoosedDrawView()
        {
            if (base._graph == null) return;

            Rectangle marginRect = base.GetMarginRect();
            DrawView();
            base._graph.DrawEllipse(Pens.Red, marginRect);
        }
    }
}
