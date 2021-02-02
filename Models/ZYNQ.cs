﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay.Models
{
    public class ZYNQ : ModelBase, XMLManager.IXmlTransformByName, XMLManager.IXmlTransformByPath
    {
        public String PSType { get; set; }
        public int CoreNum { get; set; }
        public String MainClock { get; set; }
        public String Memory { get; set; }
        public String ExpandMemory { get; set; }
        public String PLType { get; set; }
        public int LogicNum { get; set; }
        public int LUT { get; set; }
        public int Flip_Flops { get; set; }
        public int Block_ARM { get; set; }
        public String DSP_Slice { get; set; }
        public String AD { get; set; }

        public ModelBase CreateObjectByName(string objectName)
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetZYNQPath(), objectName);
            return CreateObjectByPath(xmlPath);
        }

        public void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetZYNQPath(), this.Name);
            SaveXmlByPath(xmlPath);
        }

        public ModelBase CreateObjectByPath(string objectFilePath)
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


        public void SaveXmlByPath(string xmlFilePath)
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
    }
}
