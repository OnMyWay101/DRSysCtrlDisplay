using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DRSysCtrlDisplay
{
    public partial class PPCInitForm : Form
    {
        /*库组件生成名称及路径定义*/
        static private String XmlPath_Eq = @"Lib/Equipments/";
        static private String XmlPath_Cp = @"Lib/Components/";
        private String XmlPath_Enter;
        private String XmlPath_Full;

        private NodeInfo _nodeInfo;
        private object _view;
        public PropertyFormType _formType;

        public PPCInitForm(TreeNode node)
        {
            InitializeComponent();
            switch (node.Text)
            {
                case "PPC":
                    _formType = PropertyFormType.PPC;
                    XmlPath_Enter = node.Parent.Text + "/" + node.Text + "/";
                    break;
                case "FPGA":

                    break;

                default:
                    break;
            }
            
        }

        public void OnLoad(object sender, EventArgs e)
        {
            switch(_formType)
            {
                case PropertyFormType.PPC:
                    _view = new PPC();
                    PPC ppc = (PPC)_view;
                    propertyGrid1.SelectedObject = ppc;
                    propertyGrid1.PropertySort = PropertySort.Categorized;
                    break;

                case PropertyFormType.FPAG:

                    break;

                case PropertyFormType.ZYNQ:

                    break;

                case PropertyFormType.BOARD:

                    break;

                case PropertyFormType.SLOTS:

                    break;

                case PropertyFormType.CONTIANER:

                    break;
                    
                default:

                    break;
             }   
        }

        public enum PropertyFormType
        {
            PPC = 1,
            FPAG = 2,
            ZYNQ = 3,
            BOARD = 4,
            SLOTS = 5,      /*背板*/
            CONTIANER = 6,   /*机箱*/
            COMPONENT = 7,
            APPLICATION = 8,

            TOPO = 9,   /*资源池拓扑图*/
            STATU = 10, /*资源池状态图*/
            APP         /*资源池应用*/
        };

        private void button2_Click(object sender, EventArgs e)
        {
            //注意：可以再加一些输入内容正确性与完整性判断
            CreateInfo();
            this.DialogResult = DialogResult.Yes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public String GetName()
        {
            String name;
            switch(_formType)
            {
                case PropertyFormType.PPC:
                    name = ((PPC)_view).Name;
                    break;

                default:
                    name = String.Empty;
                    break;
            }

            return name;
        }

        public NodeInfo GetInfo()
        {
            return _nodeInfo;
        }
        public void CreateInfo()
        {
            CreateXML();
            _nodeInfo = new NodeInfo();
            _nodeInfo._xmlPath = XmlPath_Full;
            _nodeInfo._formType = _formType;
        }
        public void UpdateInfo()
        {

        }
        public String UpdateXML()
        {
            return String.Empty;
        }

        public String CreateXML()
        {
            String XmlFileName = GetName() + ".xml";
            String XmlDirectory = XmlPath_Eq + XmlPath_Enter;
            XmlPath_Full = XmlDirectory + XmlFileName;
            //创建相关路径及文件
            XMLManager.CreateFile(XmlDirectory, XmlPath_Full);

            XmlTextWriter writer = new XmlTextWriter(XmlPath_Full, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();

            switch (_formType)
            {
                case PropertyFormType.PPC:
                    CreatePPCXml(writer);
                    break;

                default:
                    
                    break;
            }

            writer.WriteEndDocument();
            writer.Close();

            return XmlPath_Full;
        }

        public Boolean CreatePPCXml(XmlTextWriter writer)
        {
            PPC ppc = (PPC)_view;
            writer.WriteStartElement(ppc.Name);
            XMLManager.XmlWriteAttr(writer, "Type", ppc.Type);
            XMLManager.XmlWriteAttr(writer, "Frequency", ppc.Frequency.ToString());
            XMLManager.XmlWriteAttr(writer, "CoreNum", ppc.CoreNum.ToString());
            XMLManager.XmlWriteAttr(writer, "VectorEngin", ppc.VectorEngin.ToString());
            XMLManager.XmlWriteAttr(writer, "Memory", ppc.Memory.ToString());
            XMLManager.XmlWriteAttr(writer, "FileSystem", ppc.FileSystem.ToString());
            writer.WriteEndElement();
            return true;
        }
    }
}
