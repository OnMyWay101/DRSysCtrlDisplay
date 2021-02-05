using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay.Models
{
    public class Container : ModelBase, XMLManager.IXmlTransformByName
    {
        #region Container的基本属性

        [Category("\t基本信息"), Description("机箱类型")]
        public string Type { get; set; }            //机箱的型号
        public string BackPlaneName { get; set; }   //背板的名字
        public Dictionary<int, string> BoardNameDir = new Dictionary<int, string>();   //key:槽位号；value:板卡名称

        #endregion Container的基本属性

        public Container() { }

        public void SaveXmlByName()
        {
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetContainerPath(), this.Name);
            //先判断一些文件是否存在
            if (!PathManager.CheckFile(xmlPath))
            {
                return;
            }
            //保存XML文件
            XDocument xd = new XDocument(
                new XElement("Container",
                    new XAttribute("Name", base.Name),
                    new XAttribute("Type", this.Type),
                    new XAttribute("BackPlaneName", this.BackPlaneName),
                    new XElement("Boards")
                    )
                );
            //找到根节点
            XElement rt = xd.Element("Container");
            XElement boards = rt.Element("Boards");
            foreach (var soltInfo in this.BoardNameDir)
            {
                boards.Add(new XElement("Board",
                    new XAttribute("slotNum", soltInfo.Key),
                    new XAttribute("boardName", soltInfo.Value)
                    ));
            }
            xd.Save(xmlPath);
        }

        public ModelBase CreateObjectByName(string objectName)
        {
            Container container = new Container();
            string xmlPath = string.Format(@"{0}\{1}.xml", PathManager.GetContainerPath(), objectName);
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("CreateObject_Container:没有该Container对应的XML文件！");
                return null;
            }

            XDocument xd = XDocument.Load(xmlPath);
            //根元素的Attribute
            XElement rt = xd.Element("Container");
            container.Name = rt.Attribute("Name").Value;
            container.Type = rt.Attribute("Type").Value;
            container.BackPlaneName = rt.Attribute("BackPlaneName").Value;

            XElement boards = rt.Element("Boards");
            foreach (var board in boards.Elements())
            {
                int slotNum = int.Parse(board.Attribute("slotNum").Value);
                string boardName = board.Attribute("boardName").Value;
                BoardNameDir.Add(slotNum, boardName);
            }
            return container;
        }

    }
}
