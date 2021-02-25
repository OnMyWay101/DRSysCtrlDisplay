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
    public class FPGA : ModelBaseCore
    {
        #region FPGA的属性

        [Category("\t基本属性"), Description("类型"), ReadOnly(true)]
        public String Type { get; set; }

        [Category("\t基本属性"), Description("是否支持AD"), ReadOnly(true)]
        public String AD { get; set; }

        [Category("\t基本属性"), Description("时钟：单位MHz"), ReadOnly(true)]
        public int Clock { get; set; }

        [Category("逻辑资源"), Description("Slices"), ReadOnly(true)]
        public int Slices { get; set; }

        [Category("逻辑资源"), Description("GLB"), ReadOnly(true)]
        public int GLB { get; set; }

        [Category("逻辑资源"), Description("LogicCells"), ReadOnly(true)]
        public int LogicCells { get; set; }

        [Category("逻辑资源"), Description("查找表"), ReadOnly(true)]
        public int LUT { get; set; }

        [Category("IO信息"), Description("普通IO"), ReadOnly(true)]
        public int DifferentialIO { get; set; }

        [Category("IO信息"), Description("信号IO"), ReadOnly(true)]
        public int SingalIO { get; set; }

        [Category("内存资源"), Description("TotalBlock"), ReadOnly(true)]
        public int TotalBlock { get; set; }

        [Category("内存资源"), Description("Block"), ReadOnly(true)]
        public int Block { get; set; }

        [Category("内存资源"), Description("MaxDistributed"), ReadOnly(true)]
        public int MaxDistributed { get; set; }

        [Category("速度等级"), Description("IndustrialTemp"), ReadOnly(true)]
        public int IndustrialTemp { get; set; }

        [Category("速度等级"), Description("ExtenedTemp"), ReadOnly(true)]
        public int ExtenedTemp { get; set; }

        [Category("速度等级"), Description("Commercial"), ReadOnly(true)]
        public int Commercial { get; set; }

        [Category("IPCore资源"), Description("GTP"), ReadOnly(true)]
        public int GTP { get; set; }

        [Category("IPCore资源"), Description("AES"), ReadOnly(true)]
        public int AES { get; set; }

        [Category("IPCore资源"), Description("AMS"), ReadOnly(true)]
        public int AMS { get; set; }

        [Category("IPCore资源"), Description("PCIE"), ReadOnly(true)]
        public int PCIE { get; set; }

        [Category("IPCore资源"), Description("DSP"), ReadOnly(true)]
        public String DSP { get; set; }

        #endregion FPGA的属性

        public override ModelBaseCore CreateByPath(string objectFilePath)
        {
            FPGA fpga = new FPGA();
            if (!File.Exists(objectFilePath))
            {
                MessageBox.Show("CreateObject_FPGA:没有该FPGA芯片对应的XML文件！");
                return fpga;
            }

            XDocument xd = XDocument.Load(objectFilePath);
            //根元素的Attribute
            XElement rt = xd.Element("FPGA");
            fpga.Name = rt.Attribute("Name").Value;
            fpga.Type = rt.Attribute("Type").Value;
            fpga.AD = rt.Attribute("AD").Value;
            fpga.Clock = int.Parse(rt.Attribute("Clock").Value);
            //Logic元素的Attribute
            XElement logic = rt.Element("Logic");
            fpga.Slices = int.Parse(logic.Attribute("Slices").Value);
            fpga.GLB = int.Parse(logic.Attribute("GLB").Value);
            fpga.LogicCells = int.Parse(logic.Attribute("LogicCells").Value);
            fpga.LUT = int.Parse(logic.Attribute("LUT").Value);
            //IO元素的Attribute
            XElement io = rt.Element("IO");
            fpga.DifferentialIO = int.Parse(io.Attribute("DifferentialIO").Value);
            fpga.SingalIO = int.Parse(io.Attribute("SingalIO").Value);
            //Memory元素的Attribute
            XElement memory = rt.Element("Memory");
            fpga.TotalBlock = int.Parse(memory.Attribute("TotalBlock").Value);
            fpga.Block = int.Parse(memory.Attribute("Block").Value);
            fpga.MaxDistributed = int.Parse(memory.Attribute("MaxDistributed").Value);
            //SpeedLevel元素的Attribute
            XElement sl = rt.Element("SpeedLevel");
            fpga.IndustrialTemp = int.Parse(sl.Attribute("IndustrialTemp").Value);
            fpga.ExtenedTemp = int.Parse(sl.Attribute("ExtenedTemp").Value);
            fpga.Commercial = int.Parse(sl.Attribute("Commercial").Value);
            //IPResources元素的Attribute
            XElement ip = rt.Element("IPResources");
            fpga.GTP = int.Parse(ip.Attribute("GTP").Value);
            fpga.AES = int.Parse(ip.Attribute("AES").Value);
            fpga.AMS = int.Parse(ip.Attribute("AMS").Value);
            fpga.PCIE = int.Parse(ip.Attribute("PCIE").Value);
            fpga.DSP = ip.Attribute("DSP").Value;

            return fpga;
        }

        public override void SaveByPath(string xmlFilePath)
        {
            //先判断一些文件是否存在
            if (!PathManager.CheckFile(xmlFilePath))
            {
                return;
            }
            //保存XML文件
            var xd = new XDocument(
                new XElement("FPGA",
                    new XAttribute("Name", this.Name),
                    new XAttribute("Type", this.Type),
                    new XAttribute("AD", this.AD),
                    new XAttribute("Clock", this.Clock.ToString()),
                    new XElement("Logic",
                        new XAttribute("Slices", this.Slices.ToString()),
                        new XAttribute("GLB", this.GLB.ToString()),
                        new XAttribute("LogicCells", this.LogicCells.ToString()),
                        new XAttribute("LUT", this.LUT.ToString())
                        ),
                    new XElement("IO",
                        new XAttribute("DifferentialIO", this.DifferentialIO.ToString()),
                        new XAttribute("SingalIO", this.SingalIO.ToString())
                        ),
                    new XElement("Memory",
                        new XAttribute("TotalBlock", this.TotalBlock.ToString()),
                        new XAttribute("Block", this.Block.ToString()),
                        new XAttribute("MaxDistributed", this.MaxDistributed.ToString())
                        ),
                    new XElement("SpeedLevel",
                        new XAttribute("IndustrialTemp", this.IndustrialTemp.ToString()),
                        new XAttribute("ExtenedTemp", this.ExtenedTemp.ToString()),
                        new XAttribute("Commercial", this.Commercial.ToString())
                        ),
                    new XElement("IPResources",
                        new XAttribute("GTP", this.GTP.ToString()),
                        new XAttribute("AES", this.AES.ToString()),
                        new XAttribute("AMS", this.AMS.ToString()),
                        new XAttribute("PCIE", this.PCIE.ToString()),
                        new XAttribute("DSP", this.DSP.ToString())
                        )
                    ));
            xd.Save(xmlFilePath);
        }

        public override ModelBase CreateObjectByName(string objectName)
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetFPGAPath(), objectName);
            return CreateByPath(xmlPath);
        }

        public override void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetFPGAPath(), this.Name);
            SaveByPath(xmlPath);
        }
    }
}
