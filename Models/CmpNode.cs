using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.Models
{
    /// <summary>
    /// 计算颗粒节点定义
    /// </summary>
    public class CmpNode : XMLManager.IXmlTransformByPath
    {
        public EndType _nodeType;               //节点对应的芯片类型
        public ModelBaseCore _object = null;            //对应的芯片实例

        public List<EthSource> EthPbSources { get; set; }  //以太网发布的资源
        public List<EthSource>  EthSubSources { get; set; } //以太网订阅的资源
        public List<RioSource>  RioPbSources { get; set; } //rio网订阅的资源
        public List<RioSource>  RioSubSources { get; set; } //rio网订阅的资源
        public string Name { get { return _object.Name; } }

        public CmpNode(EndType nodeType, ModelBaseCore obj)
        {
            EthPbSources = new List<EthSource>();
            EthSubSources = new List<EthSource>();
            RioPbSources = new List<RioSource>();
            RioSubSources = new List<RioSource>();

            _nodeType = nodeType;
            _object = obj;
        }

        public ModelBase CreateObjectByPath(string objectFilePath)
        {
            return _object.CreateByPath(objectFilePath);
            //Todo:扩展内容以记录资源
        }

        public void SaveXmlByPath(string xmlFilePath)
        {
            _object.SaveByPath(xmlFilePath);
        }
    }

}
