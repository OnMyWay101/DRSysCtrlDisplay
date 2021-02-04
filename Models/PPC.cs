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
    public class PPC : ModelBase, XMLManager.IXmlTransformByName, XMLManager.IXmlTransformByPath
    {
        #region PPC的属性

        [Category("基本属性"), Description("类型")]
        public String Type { get; set; }

        [Category("基本属性"), Description("主频:单位为MHZ")]
        public int Frequency { get; set; }

        [Category("基本属性"), Description("核心数")]
        public int CoreNum { get; set; }

        [Category("基本属性"), Description("矢量引擎")]
        public bool VectorEngin { get; set; }

        [Category("基本属性"), Description("内存:单位为MB")]
        public int Memory { get; set; }

        [Category("基本属性"), Description("文件系统:单位为MB")]
        public int FileSystem { get; set; }

        #endregion

        public ModelBase CreateObjectByPath(string objectFilePath)
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

            ppc.VectorEngin = bool.Parse(rt.Element("VectorEngin").Value);
            ppc.Memory = int.Parse(rt.Element("Memory").Value);
            ppc.FileSystem = int.Parse(rt.Element("FileSystem").Value);
            return ppc;
        }

        public void SaveXmlByPath(string xmlFilePath)
        {
            //先判断一些文件是否存在
            if (!XMLManager.PathManager.CheckFile(xmlFilePath))
            {
                return;
            }
            //保存XML文件
            var xd = new XDocument(
                new XElement("PPC",
                    new XElement("Name", base.Name),
                    new XElement("Type", this.Type),
                    new XElement("Frequency", this.Frequency.ToString()),
                    new XElement("CoreNum", this.CoreNum.ToString()),
                    new XElement("VectorEngin", this.VectorEngin.ToString()),
                    new XElement("Memory", this.Memory.ToString()),
                    new XElement("FileSystem", this.FileSystem.ToString())
                    ));
            xd.Save(xmlFilePath);
        }
        public ModelBase CreateObjectByName(string objectName)
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetPPCPath(), objectName);
            return CreateObjectByPath(xmlPath);
        }

        public void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetPPCPath(), this.Name);
            SaveXmlByPath(xmlPath);
        }
    }
}
