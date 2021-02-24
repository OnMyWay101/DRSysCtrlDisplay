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
    public class Container : ModelBase
    {
        #region Container的基本属性

        [Category("\t基本信息"), Description("机箱类型"), ReadOnly(true)]
        public string Type { get; set; }            //机箱的型号

        [Category("\t基本信息"), Description("背板名称"), ReadOnly(true)]
        public string BackPlaneName { get; set; }   //背板的名字

        [Category("板卡集信息"), Description("板卡集信息")]
        public String[] BoardNames
        {
            get
            {
                var bp = ModelFactory<BackPlane>.CreateByName(BackPlaneName);
                String[] ret = new string[bp.SlotsNum];
                foreach(var namePair in BoardNameDir)
                {
                    ret[namePair.Key] = namePair.Value;
                }
                return ret;
            }
        }

        public Dictionary<int, string> BoardNameDir = new Dictionary<int, string>();   //key:槽位号；value:板卡名称

        #endregion Container的基本属性

        public Container() { }

        public override void SaveXmlByName()
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

        public override ModelBase CreateObjectByName(string objectName)
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
                container.BoardNameDir.Add(slotNum, boardName);
            }
            return container;
        }

        //该槽位没放板卡,以后要优化判断
        public bool IsContainBoard(int slotNum)
        {
            try
            {
                return IsContainBoard(BoardNameDir[slotNum]);
            }
            catch (KeyNotFoundException ex)
            {
                return false;
            }
        }

        public bool IsContainBoard(string boardName)
        {
            if ((boardName == "-请输入-") || (boardName == "无"))
            {
                return false;
            }
            return true;
        }
    }
}
