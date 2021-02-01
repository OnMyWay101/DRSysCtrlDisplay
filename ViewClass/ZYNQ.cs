using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    [TypeConverter(typeof(BaseViewCoreTypeConverter))]
    public class ZYNQ : BaseViewCore
    {
        #region ZYNQ的属性

        /*添加ZYNQ属性*/
        [Category("PS"), Description("PS类型")]
        public String PSType { get; set; }

        [Category("PS"), Description("核心数量")]
        public int CoreNum { get; set; }

        [Category("PS"), Description("主频")]
        public String MainClock { get; set; }

        [Category("PS"), Description("芯片内存")]
        public String Memory { get; set; }

        [Category("PS"), Description("扩展内存")]
        public String ExpandMemory { get; set; }

        [Category("PL"), Description("PL类型")]
        public String PLType { get; set; }

        [Category("PL"), Description("逻辑单元数")]
        public int LogicNum { get; set; }

        [Category("PL"), Description("查找表")]
        public int LUT { get; set; }

        [Category("PL"), Description("触发器")]
        public int Flip_Flops { get; set; }

        [Category("PL"), Description("Block ARM")]
        public int Block_ARM { get; set; }

        [Category("PL"), Description("DSP Slice")]
        public String DSP_Slice { get; set; }

        [Category("PL"), Description("有无AD")]
        public String AD { get; set; }

        #endregion

        public ZYNQ() { }

        public override void DrawView(Graphics g) {}

        public override void DrawView(Graphics g, Rectangle rect)
        {
            g.DrawRectangle(Princeple.ComputeNodeColor.Pen_PL, rect);
            g.FillRectangle(Princeple.ComputeNodeColor.Brushes_PL, rect);
            base.AddSentence(g, rect, "ZYNQ");
        }

        public override void DrawView(Graphics g, Rectangle rect, Pen pen, Brush brush)
        {
            g.DrawRectangle(pen, rect);
            g.FillRectangle(brush, rect);
            base.AddSentence(g, rect, "ZYNQ");
        }

        public override void DrawView(Graphics g, Rectangle rect, string name)
        {
            g.DrawRectangle(Princeple.ComputeNodeColor.Pen_PL, rect);
            g.FillRectangle(Princeple.ComputeNodeColor.Brushes_PL, rect);
            base.AddSentence(g, rect, name);
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
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetZYNQPath(), this.Name);
            SaveXmlByPath(xmlPath);
        }

        public override void SaveXmlByPath(string xmlFilePath)
        {
            //先判断一些文件是否存在
            if (!PathManager.CheckFile(xmlFilePath))
            {
                return;
            }
            //保存XML文件
            XDocument xd = new XDocument(
                new XElement("ZYNQ",
                    new XAttribute("Name", this.Name),
                    new XElement("PS",
                        new XAttribute("Type", this.PSType),
                        new XAttribute("CoreNum", this.CoreNum.ToString()),
                        new XAttribute("MainClock", this.MainClock),
                        new XAttribute("Memory", this.Memory),
                        new XAttribute("ExpandMemory", this.ExpandMemory)
                        ),
                    new XElement("PL",
                        new XAttribute("Type", this.PLType),
                        new XAttribute("LogicNum", this.LogicNum.ToString()),
                        new XAttribute("LUT", this.LUT.ToString()),
                        new XAttribute("Flip_Flops", this.Flip_Flops.ToString()),
                        new XAttribute("Block_ARM", this.Block_ARM.ToString()),
                        new XAttribute("DSP_Slice", this.DSP_Slice),
                        new XAttribute("AD", this.AD)
                        )
                    )
                );
            xd.Save(xmlFilePath);
        }

        public override BaseView CreateObjectByName(string objectName)
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetZYNQPath(), objectName);
            return CreateObjectByPath(xmlPath);
        }

        public override BaseView CreateObjectByPath(string objectFilePath)
        {
            ZYNQ zynq = new ZYNQ();
            if (!File.Exists(objectFilePath))
            {
                MessageBox.Show("CreateObject_ZYNQ:没有该ZYNQ芯片对应的XML文件！");
                return zynq;
            }
            XDocument xd = XDocument.Load(objectFilePath);
            //根元素的Attribute
            XElement rt = xd.Element("ZYNQ");
            zynq.Name = rt.Attribute("Name").Value;
            //ps元素的Attribute
            XElement ps = rt.Element("PS");
            zynq.PSType = ps.Attribute("Type").Value;
            zynq.CoreNum = int.Parse(ps.Attribute("CoreNum").Value);
            zynq.MainClock = ps.Attribute("MainClock").Value;
            zynq.Memory = ps.Attribute("Memory").Value;
            zynq.ExpandMemory = ps.Attribute("ExpandMemory").Value;
            //pl元素的Attribute
            XElement pl = rt.Element("PL");
            zynq.PLType = pl.Attribute("Type").Value;
            zynq.LogicNum = int.Parse(pl.Attribute("LogicNum").Value);
            zynq.LUT = int.Parse(pl.Attribute("LUT").Value);
            zynq.Flip_Flops = int.Parse(pl.Attribute("Flip_Flops").Value);
            zynq.Block_ARM = int.Parse(pl.Attribute("Block_ARM").Value);
            zynq.DSP_Slice = pl.Attribute("DSP_Slice").Value;
            zynq.AD = pl.Attribute("AD").Value;

            return zynq;
        }
    }

}
