using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;

namespace DRSysCtrlDisplay
{
    /**
    *类型选择框
    **/
    public class TypeItems : StringConverter
    {
        //1.支持下拉框标准值
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        //2.获取标准值
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new String[]{"P2020", "8640","8640D"});
        }
        //3.不允许编辑
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    /**
     *核心数选择框
     **/
    public class CoreNumItems : Int32Converter
    {
        public CoreNumItems()
        {

        }
        //1.支持下拉框标准值
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        //2.获取标准值
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new int[]{1,2});
        }
        //3.不允许编辑
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
    /*CheckboxVE*/
    public class CheckboxVE : System.Drawing.Design.UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            ControlPaint.DrawCheckBox(e.Graphics,e.Bounds,ButtonState.Inactive);
        }
    }

    class PPC : BaseView
    {
        [Category("基本属性"), Description("类型"), TypeConverter(typeof(TypeItems))]
        public String Type { get; set; }

        [Category("基本属性"), Description("主频:单位为MHZ")]
        public int Frequency { get; set; }

        [Category("基本属性"), Description("核心数"), TypeConverter(typeof(CoreNumItems))]
        public int CoreNum { get; set; }

        [Category("基本属性"), Description("矢量引擎")]
        public Boolean VectorEngin { get; set; }

        [Category("基本属性"), Description("内存:单位为MB")]
        public int Memory { get; set; }

        [Category("基本属性"), Description("文件系统:单位为MB")]
        public int FileSystem { get; set; }

        public PPC()
        {
        }
        public PPC(String xmlPath)
        {

        }

        public PPC(XmlDocument xmlDoc, String nodeName)
        {
            //解析XML文件
            XmlNodeList nodes = xmlDoc.GetElementsByTagName(nodeName);
            XmlNode node = nodes[0];
            Name = nodeName;
            try
            {
                Type = node.Attributes["Type"].Value;
                Frequency = int.Parse(node.Attributes["Frequency"].Value);
                CoreNum = int.Parse(node.Attributes["CoreNum"].Value);
                VectorEngin = Boolean.Parse(node.Attributes["VectorEngin"].Value);
                Memory = int.Parse(node.Attributes["Memory"].Value);
                FileSystem = int.Parse(node.Attributes["FileSystem"].Value);
            }
            catch
            {
                MessageBox.Show("读取XML文件属性异常！");
            }

        }

        public override void DrawView(Graphics g)
        {
            Rectangle r = new Rectangle(100, 100, 100, 100);
            g.DrawRectangle(Pens.Blue, r);
            g.FillRectangle(Brushes.Blue,r);
        }

    }
}
