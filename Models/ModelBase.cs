using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.Models
{
    public abstract class ModelBase : XMLManager.IXmlTransformByName
    {
        [Category("\t\t基本信息"), Description("名称"), ReadOnly(true)]
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

    [TypeConverter(typeof(ModelTypeConverter))]
    public abstract class ModelBaseCore : ModelBase
    {
        public virtual ModelBaseCore CreateByPath(string objectFilePath) 
        {
            throw new NotImplementedException();
        }

        public virtual void SaveByPath(string xmlFilePath)
        {
            throw new NotImplementedException();
        }
    }

    public class ModelTypeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            var model = value as ModelBase;
            return model.Name;
        }
    }
}
