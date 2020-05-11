using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DRSysCtrlDisplay
{
    public class BaseView
    {
        //public Boolean _isSlected;

        [Category("名称"),Description("名称")]
        public String Name { get; set;}

        public BaseView()
        {

        }

        public virtual void DrawView(Graphics g)
        {
        }
    }


}
