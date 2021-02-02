using System;
using System.Collections.Generic;
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

        public String Type { get; set; }

        public int Frequency { get; set; }

        public int CoreNum { get; set; }

        public bool VectorEngin { get; set; }

        public int Memory { get; set; }

        public int FileSystem { get; set; }

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
    }
}
