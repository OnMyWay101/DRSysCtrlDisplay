using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    using Princeple;
    using System.ComponentModel;

    public enum SwitchCategory
    {
        EtherNetSw,
        RioSw
    }

    /// <summary>
    /// 描述板卡上面交换机的类
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SwitchDevice : BaseDrawer
    {
        [Category("基本属性"), Description("类型")]
        public SwitchCategory Category { get; set; }    //交换的种类，这里只包含EtherNet交换机和Rio交换机
        [Category("基本属性"), Description("型号")]
        public string Type { get; set; }                //交换机的型号

        public SwitchDevice(SwitchCategory category, string type)
            : base(new Rectangle())
        {
            Category = category;
            Type = type;
        }

        public void SetDrawingRect(Rectangle rect)
        {
            base._rect = rect;
        }

        public override void DrawView(Graphics g)
        {
            var pen = (Category == SwitchCategory.EtherNetSw ? ConnectAreaColor.Pen_EtherNet : ConnectAreaColor.Pen_RapidIO);
            var brush = (Category == SwitchCategory.EtherNetSw ? ConnectAreaColor.Brushes_EtherNet : ConnectAreaColor.Brushes_RapidIO);
            g.DrawEllipse(pen, base._rect);
            g.FillEllipse(brush, base._rect);

            //给交换机加上“SW”字样
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            g.DrawString("SW", new Font("Arial", 16), Brushes.Black, base._rect, drawFormat);
        }

        public override void ChoosedDrawView(Graphics g)
        {
            Rectangle marginRect = base.GetMarginRect();
            DrawView(g);
            g.DrawEllipse(Pens.Red, marginRect);
        }

        public override object GetModelInstance()
        {
            return this;
        }
    }
}
