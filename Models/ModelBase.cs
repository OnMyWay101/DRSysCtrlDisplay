using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.Models
{
    public class ModelBase
    {
        [Category("\t基本信息"), Description("名称")]
        public String Name { get; set; }
    }
}
