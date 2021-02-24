using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.Models
{
    /// <summary>
    /// 计算颗粒节点定义
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CmpNode : XMLManager.IXmlTransformByPath
    {
        [Category("基本属性"), Description("节点类型"), ReadOnly(true)]
        public EndType NodeType { get; set; }                      //节点对应的芯片类型

        [Category("基本属性"), Description("属性信息"), ReadOnly(true)]
        public ModelBaseCore Obj { get; set; }            //对应的芯片实例

        [Category("基本属性"), Description("节点名称"), ReadOnly(true)]
        public string Name { get { return Obj.Name; } }

        //Todo:显示资源相关信息
        [Category("资源信息"), Description("以太网发布资源"), ReadOnly(true)]
        public EthSource[] EthPbSourceArray { get; set; }

        [Category("资源信息"), Description("以太网订阅资源"), ReadOnly(true)]
        public EthSource[] EthSubSourceArray { get; set; }

        [Category("资源信息"), Description("RapidIO发布资源"), ReadOnly(true)]
        public RioSource[] RioPbSourceArray { get; set; }

        [Category("资源信息"), Description("RapidIO订阅资源"), ReadOnly(true)]
        public RioSource[] RioSubSourceArray { get; set; }

        [Browsable(false)]
        public List<EthSource> EthPbSources { get; set; }  //以太网发布的资源

        [Browsable(false)]
        public List<EthSource> EthSubSources { get; set; } //以太网订阅的资源

        [Browsable(false)]
        public List<RioSource> RioPbSources { get; set; } //rio网订阅的资源

        [Browsable(false)]
        public List<RioSource> RioSubSources { get; set; } //rio网订阅的资源

        public CmpNode(EndType nodeType, ModelBaseCore obj)
        {
            EthPbSources = new List<EthSource>();
            EthSubSources = new List<EthSource>();
            RioPbSources = new List<RioSource>();
            RioSubSources = new List<RioSource>();

            NodeType = nodeType;
            Obj = obj;
        }

        public ModelBaseCore CreateObjectByPath(string objectFilePath)
        {
            return Obj.CreateByPath(objectFilePath);
            //Todo:扩展内容以记录资源
        }

        public void SaveXmlByPath(string xmlFilePath)
        {
            Obj.SaveByPath(xmlFilePath);
        }
    }

}
