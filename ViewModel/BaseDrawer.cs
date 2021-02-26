using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DRSysCtrlDisplay.Princeple;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// 设备库所有设备的基类
    /// </summary>
    public abstract class BaseDrawer
    {
        public event Action RedrawRequst;   //需要重绘事件
        protected Rectangle _rect;          //图像的边框

        protected BaseDrawer(Rectangle rect)
        {
            Init(rect);
        }
        protected BaseDrawer() { }

        public virtual void Init(Rectangle rect)
        {
            _rect = rect;
        }

        public abstract void DrawView(Graphics g);
        public virtual void DrawView(Graphics g, Pen pen, Brush brush) { }
        public virtual void ChoosedDrawView(Graphics g) { }

        //获取该图像显示的区域大小
        public virtual Size GetViewSize() { return new Size(0, 0); }

        //通知界面重绘
        public void TriggerRedrawRequst()
        {
            if (RedrawRequst != null)
            {
                RedrawRequst();
            }
        }

        //获取一个矩形的外接矩形
        protected Rectangle GetMarginRect()
        {
            int margin = 5;     //显示一个选中的外框离选中矩形的间距
            int pointX = ((_rect.X - margin) >= 0) ? _rect.X - margin : 0;
            int pointY = ((_rect.Y - margin) >= 0) ? _rect.Y - margin : 0;
            int width = _rect.Width + margin * 2;
            int height = _rect.Height + margin * 2;

            return new Rectangle(pointX, pointY, width, height);
        }

        public virtual object GetModelInstance()
        {
            return null;
        }

        //往一个矩形区域添加字符字段
        public static void AddDirctionSentence(Graphics g, Rectangle rect, string Sentence, bool IsHorizontal)
        {
            StringFormat drawFormat;    //字体模板
            float fontSize;             //字体大小
            if (IsHorizontal)//是否为水平布局
            {
                drawFormat = new StringFormat();
                fontSize = rect.Height / 2;
            }
            else
            {
                drawFormat = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft);
                fontSize = rect.Width / 4;
            }
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            g.DrawString(Sentence, new Font("Arial", fontSize), Brushes.Black, rect, drawFormat);
        }
    }

    /// <summary>
    /// 设备库中计算颗粒对应的设备的基类
    /// </summary>
    public abstract class BaseDrawerCore : BaseDrawer
    {
        protected BaseDrawerCore() { }
        protected BaseDrawerCore(Rectangle rect) : base(rect) { }

        protected const int _fontScale = 5;//字体与图形外接边框Width的比列
        public override void DrawView(Graphics g) { }
        public abstract void DrawView(Graphics g, string name);
        public abstract void ChoosedDrawView(Graphics g, string name);
        public virtual void ChoosedDrawView(Graphics g, Pen pen, Brush brush)
        {
            ChoosedDrawView(g);
            DrawView(g, pen, brush);
        }
        public override Size GetViewSize()
        {
            return new Size(100, 100);
        }
        protected void AddSentence(Graphics g, string Sentence)
        {
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            g.DrawString(Sentence, new Font("Arial", base._rect.Width / _fontScale), Brushes.Black, base._rect, drawFormat);
        }
    }

    //画图类节点信息有变化的通知接口
    public interface IDrawerNotify
    {
        //当前baseview对应的节点信息改变的事件处理
        void OnNodeInfoChanged(TargetNode tNode);
    }

    //画图类鼠标操作选中图元的接口;该接口还可以优化拆分;
    public interface IDrawerChoosed
    {
        BaseDrawer ChoosedBv { get; set; }
        void MouseEventHandler(object sender, MouseEventArgs e);

        //获取该对象中对应点击位置的子对象
        BaseDrawer GetChoosedBaseView(MouseEventArgs e);
    }

    //画图类鼠标操作选中图元的接口
    public interface IDrawerChoosed<TNode>
    {
        TNode ChoosedBv { get; set; }
        void MouseEventHandler(object sender, MouseEventArgs e);

        //获取该对象中对应点击位置的子对象
        TNode GetChoosedNodeView(MouseEventArgs e);
    }
}
