using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.Models
{
    public static class ModelFactory<T>
    where T : ModelBase, new()
    {
        public static T CreateRaw()
        {
            return new T();
        }

        public static T CreateByName(string name)
        {
            T t = new T();
            return (T)t.CreateObjectByName(name);
        }
    }

}
