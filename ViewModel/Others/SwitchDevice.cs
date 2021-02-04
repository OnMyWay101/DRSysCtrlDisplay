using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    /// <summary>
    /// 描述板卡上面交换机的类
    /// </summary>
    public class SwitchDevice : BaseView
    {
        public SwitchCategory Category { get; set; }    //交换的种类，这里只包含EtherNet交换机和Rio交换机
        public string Type { get; set; }                //交换机的型号

        public SwitchDevice(SwitchCategory category, string type)
        {
            base.Name = type;
            Category = category;
            Type = type;
        }

        public override void DrawView(Graphics g) { }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            var pen = (Category == SwitchCategory.EtherNetSw ? Princeple.ConnectAreaColor.Pen_EtherNet : Princeple.ConnectAreaColor.Pen_RapidIO);
            var brush = (Category == SwitchCategory.EtherNetSw ? Princeple.ConnectAreaColor.Brushes_EtherNet : Princeple.ConnectAreaColor.Brushes_RapidIO);
            g.DrawEllipse(pen, rect);
            g.FillEllipse(brush, rect);
            //给交换机加上“SW”字样
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            g.DrawString("SW", new Font("Arial", 16), Brushes.Black, rect, drawFormat);
        }

        public override void ChoosedDrawView(Graphics g, Rectangle rect)
        {
            Rectangle marginRect = base.GetMarginRect(rect);
            DrawView(g, rect);
            g.DrawEllipse(Pens.Red, marginRect);
        }
    }

    public enum SwitchCategory
    {
        EtherNetSw,
        RioSw
    }
}
