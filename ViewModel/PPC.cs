using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.IO;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// PPC类，包含画图接口
    /// </summary>
    [TypeConverter(typeof(BaseViewCoreTypeConverter))]
    public class PPC : BaseViewCore
    {
        

        public PPC()
        {
            this.Name = string.Empty;
            this.Type = string.Empty;
            this.Frequency = 0;
            this.CoreNum = 0;
            this.VectorEngin = "false";
            this.Memory = 0;
            this.FileSystem = 0;
        }

        public override void DrawView(Graphics g)
        { }

        public override void DrawView(Graphics g, Rectangle rect)
        {
            g.DrawRectangle(Princeple.ComputeNodeColor.Pen_PPC, rect);
            g.FillRectangle(Princeple.ComputeNodeColor.Brushes_PPC, rect);
            AddSentence(g, rect, "PPC");
        }

        public override void DrawView(Graphics g, Rectangle rect, Pen pen, Brush brush)
        {
            g.DrawRectangle(pen, rect);
            g.FillRectangle(brush, rect);
            base.AddSentence(g, rect, "PPC");
        }

        public override void DrawView(Graphics g, Rectangle rect, string name)
        {
            g.DrawRectangle(Princeple.ComputeNodeColor.Pen_PPC, rect);
            g.FillRectangle(Princeple.ComputeNodeColor.Brushes_PPC, rect);
            AddSentence(g, rect, name);
        }

        public override void ChoosedDrawView(Graphics g, Rectangle rect, string name)
        {
            Rectangle marginRect = base.GetMarginRect(rect);

            DrawView(g, rect, name);
            g.DrawRectangle(Pens.Red, marginRect);
        }

        public override void ChoosedDrawView(Graphics g, Rectangle rect)
        {
            Rectangle marginRect = base.GetMarginRect(rect);

            DrawView(g, rect);
            g.DrawRectangle(Pens.Red, marginRect);
        }

        public override Size GetViewSize()
        {
            return new Size(100, 100);
        }

        public override void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetPPCPath(), this.Name);
            SaveXmlByPath(xmlPath);
        }

        public override void SaveXmlByPath(string xmlFilePath)
        {
            //先判断一些文件是否存在
            if (!XMLManager.PathManager.CheckFile(xmlFilePath))
            {
                return;
            }
            //保存XML文件
            var xd = new XDocument(
                new XElement("PPC",
                    new XElement("Name", this.Name),
                    new XElement("Type", this.Type),
                    new XElement("Frequency", this.Frequency.ToString()),
                    new XElement("CoreNum", this.CoreNum.ToString()),
                    new XElement("VectorEngin", this.VectorEngin),
                    new XElement("Memory", this.Memory.ToString()),
                    new XElement("FileSystem", this.FileSystem.ToString())
                    ));
            xd.Save(xmlFilePath);
        }

        public override BaseView CreateObjectByName(string objectName)
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetPPCPath(), objectName);
            return CreateObjectByPath(xmlPath);
        }

        public override BaseView CreateObjectByPath(string objectFilePath)
        {
            PPC ppc = new PPC();
            if (!File.Exists(objectFilePath))
            {
                MessageBox.Show("CreateObjectByPath:没有该PPC芯片对应的XML文件！");
                return ppc;
            }
            XDocument xd = XDocument.Load(objectFilePath);
            XElement rt = xd.Element("PPC");
            ppc.Name = rt.Element("Name").Value;
            ppc.Type = rt.Element("Type").Value;
            ppc.Frequency = int.Parse(rt.Element("Frequency").Value);
            ppc.CoreNum = int.Parse(rt.Element("CoreNum").Value);

            ppc.VectorEngin = rt.Element("VectorEngin").Value;
            ppc.Memory = int.Parse(rt.Element("Memory").Value);
            ppc.FileSystem = int.Parse(rt.Element("FileSystem").Value);
            return ppc;
        }
    }
}
