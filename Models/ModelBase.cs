using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.Models
{
    public abstract class ModelBase : XMLManager.IXmlTransformByName
    {
        [Category("\t基本信息"), Description("名称")]
        public String Name { get; set; }

        public virtual ModelBase CreateObjectByName(string objectName)
        {
            throw new NotImplementedException();
        }

        public virtual void SaveXmlByName()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ModelBaseCore : ModelBase
    {
        public virtual ModelBase CreateByPath(string objectFilePath) 
        {
            throw new NotImplementedException();
        }

        public virtual void SaveByPath(string xmlFilePath)
        {
            throw new NotImplementedException();
        }
    }

}
