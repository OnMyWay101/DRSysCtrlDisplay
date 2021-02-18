using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.Models;
using DRSysCtrlDisplay.ViewModel.Others;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay
{
    public class ComponentViewModel : BaseDrawer
    {
        private Models.Component _component;        //构件的实例
        private TopoNetView<ComponentNode, ComponentLine> _topoView;

        public ComponentViewModel(Models.Component cmp, Graphics g, Rectangle rect)
            : base(g, rect)
        {
            _component = cmp;
        }

        #region 重载虚函数
        public override void DrawView()
        {
            _topoView = new TopoNetView<ComponentNode, ComponentLine>(base._graph, base._rect, _component.CmpTopoNet);
            _topoView.DrawView();
        }

        public override Size GetViewSize()
        {
            return new Size(800, 400);
        }
        #endregion 重载虚函数

        //转化二维数组到一维数组
        private ComponentLine[] Component_LinksArrayTranse(int Num, ComponentLine[,] line2DArray)
        {
            ComponentLine[] retArray = new ComponentLine[Num * Num];
            for (int i = 0; i < Num; i++)
            {
                for (int j = 0; j < Num; j++)
                {
                    retArray[i * Num + j] = line2DArray[i, j];
                }
            }
            return retArray;
        }

        //添加一条Link信息到XElement节点
        private void Component_AddXmlLinks(XElement LinksElement, IEnumerable<ComponentLine> links
            , string linkName)
        {
            foreach (var linkInfo in links)
            {
                if (linkInfo != null)
                {
                    LinksElement.Add(new XElement(linkName,
                        new XAttribute("LinkType", linkInfo.LinkType),
                        new XAttribute("FirstEndId", linkInfo.FirstEndId),
                        new XAttribute("SecondEndId", linkInfo.SecondEndId),
                        new XAttribute("LanesNum", linkInfo.LanesNum)
                        ));
                }
            }
        }

        //把一个连接的XElement转化为一个ComponentLine实体
        private ComponentLine Component_TransXmlLink(XElement LinkElement)
        {
            int nodeId1 = int.Parse(LinkElement.Attribute("FirstEndId").Value);
            int nodeId2 = int.Parse(LinkElement.Attribute("SecondEndId").Value);
            var linkType = (LinkType)(Enum.Parse(typeof(LinkType), LinkElement.Attribute("LinkType").Value));
            var lanes = (LinkLanes)(Enum.Parse(typeof(LinkLanes), LinkElement.Attribute("LanesNum").Value));

            return new ComponentLine(linkType, nodeId1, nodeId2, lanes);
        }
    }
}
