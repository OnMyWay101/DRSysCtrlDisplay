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
    public abstract class BaseView : XMLManager.IXmlTransformByName
    {
        public event Action RedrawRequst;

        [Category("\t\t名称"), Description("名称")]
        public String Name { get; set; }

        protected BaseView()
        {
            this.Name = string.Empty;
        }

        public abstract void DrawView(Graphics g);
        public abstract void DrawView(Graphics g, Rectangle rect);
        public virtual void DrawView(Graphics g, Rectangle rect, Pen pen, Brush brush) { }
        public virtual void ChoosedDrawView(Graphics g, Rectangle rect) { }

        //获取该图像显示的区域大小
        public virtual Size GetViewSize() { return new Size(0, 0); }

        public virtual void SaveXmlByName() { }
        public virtual BaseView CreateObjectByName(string objectName) { return null; }

        //当前baseview对应的节点信息改变的事件处理
        public virtual void OnNodeInfoChanged(TargetNode tNode) { }

        //通知界面重绘
        public virtual void MouseEventHandler(object sender, MouseEventArgs e) { }

        public void TriggerRedrawRequst()
        {
            if (RedrawRequst != null)
            {
                RedrawRequst();
            }
        }

        //获取一个矩形的外接矩形
        protected Rectangle GetMarginRect(Rectangle rect)
        {
            int margin = 5;     //显示一个选中的外框离选中矩形的间距
            int pointX = ((rect.X - margin) >= 0) ? rect.X - margin : 0;
            int pointY = ((rect.Y - margin) >= 0) ? rect.Y - margin : 0;
            int width = rect.Width + margin * 2;
            int height = rect.Height + margin * 2;

            return new Rectangle(pointX, pointY, width, height);
        }

        //往一个矩形区域添加字符字段
        public static void AddDircSentence(Graphics g, Rectangle rect, string Sentence, bool IsHorizontal)
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
    public abstract class BaseViewCore : BaseView, XMLManager.IXmlTransformByPath
    {
        protected const int _fontScale = 5;//字体与图形外接边框Width的比列

        public override void DrawView(Graphics g) { }
        public override void DrawView(Graphics g, Rectangle rect) { }
        public abstract void DrawView(Graphics g, Rectangle rect, string name);

        public abstract void ChoosedDrawView(Graphics g, Rectangle rect, string name);

        public virtual void SaveXmlByPath(string xmlFilePath) { }
        public virtual BaseView CreateObjectByPath(string objectFilePath) { return null; }

        protected void AddSentence(Graphics g, Rectangle rect, string Sentence)
        {
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            g.DrawString(Sentence, new Font("Arial", rect.Width / _fontScale), Brushes.Black, rect, drawFormat);
        }
    }

    public class BaseViewCoreTypeConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            BaseViewCore bvCore = value as BaseViewCore;

            return bvCore.Name;
        }
    }

    public static class BaseViewFactory<T>
        where T : BaseView, new()
    {
        public static T CreateRaw()
        {
            return new T();
        }

        public static T CreateByName(string name)
        {
            T t = new T();
            return (T)t.CreateObjectByName(name);
        }
    }

    public static class BaseViewCoreFactory<T>
        where T : BaseViewCore, new()
    {
        public static T CreateRaw()
        {
            return new T();
        }

        public static T CreateByName(string name)
        {
            T t = new T();
            return (T)t.CreateObjectByName(name);
        }

        public static T CreateByPath(string objectFilePath)
        {
            T t = new T();
            return (T)t.CreateObjectByPath(objectFilePath);
        }
    }

    //画图类方法接口
    public interface IDrawer
    {
        //获取该对象中对应点击位置的子对象
        BaseView GetChoosedBaseView(MouseEventArgs e);

        //获取一个BaseView对象在该对象里面显示的矩形区域
        Rectangle GetBaseViewRect(BaseView baseView, ref bool isFind);
    }

    //画图类方法接口
    public interface IGenericDrawer<TNode>
    {
        //获取该对象中对应点击位置的子对象
        TNode GetChoosedNodeView(MouseEventArgs e);

        //获取一个BaseView对象在该对象里面显示的矩形区域
        Rectangle GetBaseViewRect(TNode nodeView, ref bool isFind);
    }
}
