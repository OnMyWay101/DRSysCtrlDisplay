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
        public ComputeNodeType _nodeType;               //节点对应的芯片类型
        public ModelBaseCore _object = null;            //对应的芯片实例

        public List<EthSource> _ethPbSources = new List<EthSource>();   //以太网发布的资源
        public List<EthSource> _ethSubSources = new List<EthSource>();  //以太网订阅的资源
        public List<RioSource> _rioPbSources = new List<RioSource>();   //rio网订阅的资源
        public List<RioSource> _rioSubSources = new List<RioSource>();  //rio网订阅的资源

        public CmpNode(ComputeNodeType nodeType, ModelBaseCore obj)
        {
            _nodeType = nodeType;
            _object = obj;
        }

        public ModelBase CreateObjectByPath(string objectFilePath)
        {
            return _object.CreateByPath(objectFilePath);
        }

        public void SaveXmlByPath(string xmlFilePath)
        {
            _object.SaveByPath(xmlFilePath);
        }
    }

}
