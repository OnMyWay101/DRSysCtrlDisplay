using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay.Models
{
    public class ZYNQ : ModelBaseCore
    {
        #region ZYNQ的属性

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

        #endregion ZYNQ的属性

        public override ModelBaseCore CreateByPath(string objectFilePath)
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

        public override void SaveByPath(string xmlFilePath)
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

        public override ModelBase CreateObjectByName(string objectName)
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetZYNQPath(), objectName);
            return CreateByPath(xmlPath);
        }

        public override void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetZYNQPath(), this.Name);
            SaveByPath(xmlPath);
        }
    }
}
