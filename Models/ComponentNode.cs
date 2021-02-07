using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.Models
{
    public class SourceBase
    {
        public EndType SourceType { get; set; }
        public string NodeName { get; set; }
        public string NourceName { get; set; }
    }

    public class ComponentPPC : PPC, XMLManager.IXmlTransformByPath
    {
        public List<SourceBase> PublishSources { get; set; }
        public List<SourceBase> SubscribeSources { get; set; }
        public ModelBase CreateObjectByPath(string objectFilePath)
        {
            return base.CreateByPath(objectFilePath);
        }

        public void SaveXmlByPath(string xmlFilePath)
        {
            base.SaveByPath(xmlFilePath);
        }
    }

    public class ComponentFPGA : FPGA, XMLManager.IXmlTransformByPath
    {
        public List<SourceBase> PublishSources { get; set; }
        public List<SourceBase> SubscribeSources { get; set; }
        public ModelBase CreateObjectByPath(string objectFilePath)
        {
            return base.CreateByPath(objectFilePath);
        }

        public void SaveXmlByPath(string xmlFilePath)
        {
            base.SaveByPath(xmlFilePath);
        }
    }

    public class ComponentZYNQ : ZYNQ, XMLManager.IXmlTransformByPath
    {
        public List<SourceBase> PublishSources { get; set; }
        public List<SourceBase> SubscribeSources { get; set; }
        public ModelBase CreateObjectByPath(string objectFilePath)
        {
            return base.CreateByPath(objectFilePath);
        }

        public void SaveXmlByPath(string xmlFilePath)
        {
            base.SaveByPath(xmlFilePath);
        }
    }
}
