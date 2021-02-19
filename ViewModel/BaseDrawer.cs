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
        protected Graphics _graph;          //对应的画布
        protected Rectangle _rect;          //图像的边框

        protected BaseDrawer(Graphics g, Rectangle rect)
        {
            Init(g, rect);
        }

        public void Init(Graphics g, Rectangle rect)
        {
            _graph = g;
            _rect = rect;
        }

        public abstract void DrawView();
        public virtual void DrawView(Pen pen, Brush brush) { }
        public virtual void ChoosedDrawView() { }

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
        protected BaseDrawerCore(Graphics g, Rectangle rect) : base(g, rect) { }

        protected const int _fontScale = 5;//字体与图形外接边框Width的比列
        public override void DrawView() { }
        public abstract void DrawView(string name);
        public abstract void ChoosedDrawView(string name);
        public override Size GetViewSize()
        {
            return new Size(100, 100);
        }
        protected void AddSentence(string Sentence)
        {
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            base._graph.DrawString(Sentence, new Font("Arial", base._rect.Width / _fontScale), Brushes.Black, base._rect, drawFormat);
        }
    }

    //画图类节点信息有变化的通知接口
    public interface IDrawerNotify
    {
        //当前baseview对应的节点信息改变的事件处理
        void OnNodeInfoChanged(TargetNode tNode);
    }

    //画图类鼠标操作选中图元的接口
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
