using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace DRSysCtrlDisplay
{
    class TreeXML
    {
        static private String XMLNameEq = "Equipments";
        static private String XMLNameCp = "Components";
        static public String XMLDirectory = @"configFile/";
        static public String EqconfigXMLPath = XMLDirectory + XMLNameEq + ".xml";
        static public String CpconfigXMLPath = XMLDirectory + XMLNameCp + ".xml";

        static private void writeXmlAttr(XmlTextWriter writer, String name, Object tag)
        {
            NodeInfo nodeInfo;
            try
            {
                nodeInfo = (NodeInfo)tag;
            }
            catch
            {
                return;
            }
            String xmlPath = nodeInfo._xmlPath;
            writer.WriteStartAttribute(name);
            writer.WriteValue(xmlPath);
            writer.WriteEndAttribute();
        }

        static private void saveEqTree(TreeView tree, XmlTextWriter writer)
        {
            TreeNodeCollection rootNodes = tree.Nodes;
            writer.WriteStartElement(XMLNameEq);
            for (int i = 0; i < rootNodes.Count; i++)
            {
                TreeNode childNode = rootNodes[i];
                saveNode(childNode,writer);
            }
            writer.WriteEndElement();//"Equipment"
        }

        static private void saveNode(TreeNode node, XmlTextWriter writer)
        {
            TreeNodeCollection nodes = node.Nodes;
            //定义边界条件（节点为属性节点）
            if (nodes.Count <= 0)
            {
                writer.WriteStartElement(node.Text);                
                if (node.Tag != null)
                {
                    writeXmlAttr(writer, "XmlPath", node.Tag);                    
                }
                writer.WriteEndElement();
                return;
            }
            //节点为元素节点
            writer.WriteStartElement(node.Text);
            for (int i = 0; i < nodes.Count; i++)
            {
                TreeNode childNode = nodes[i];
                saveNode(childNode, writer);
            }
            writer.WriteEndElement();
        }

        static private void saveCpTree(TreeView tree, XmlTextWriter writer)
        {
            TreeNodeCollection rootNodes = tree.Nodes;
            writer.WriteStartElement(XMLNameCp);
            for (int i = 0; i < rootNodes.Count; i++)
            {
                TreeNode childNode = rootNodes[i];
                saveNode(childNode, writer);
            }
            writer.WriteEndElement();//"Components"
        }

        static public void SaveTree(TreeView tree, String filepath, String dir)
        {
            if (!File.Exists(filepath))
            {
                XMLManager.CreateFile(dir, filepath);
            }
            XmlTextWriter writer = new XmlTextWriter(filepath, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();

            if (String.Compare(tree.Name,"_eqTreeView",false) == 0)
            {
                saveEqTree(tree, writer);
            }
            else if (String.Compare(tree.Name, "_cpTreeView", false) == 0)
            {
                saveCpTree(tree, writer);
            }

            writer.WriteEndDocument();
            writer.Close();
        }


        static private void eqTreeInit(TreeView eqTree)
        {
            TreeNode chipLibNode = new TreeNode("芯片库");
            TreeNode boardLibNode = new TreeNode("板卡库");
            TreeNode slotsLibNode = new TreeNode("背板库");
            TreeNode containerLibNode = new TreeNode("机箱库");
            eqTree.Nodes.AddRange(new TreeNode[] { chipLibNode, boardLibNode, slotsLibNode, containerLibNode });

            TreeNode ppcNode = new TreeNode("PPC");
            TreeNode fpgaNode = new TreeNode("FPGA");
            TreeNode zynqNode = new TreeNode("ZYNQ");

            eqTree.Nodes[0].Nodes.AddRange(new TreeNode[] { ppcNode, fpgaNode, zynqNode });           
        }

        static private void xmlToTreeNode(TreeNode libNode, XmlNode xmlNode, PPCInitForm.PropertyFormType formType)
        {
            //Name属性就是是当初加进去的属性(attribute)名称
            String name = xmlNode.Name;
            String xmlPath = xmlNode.Attributes["XmlPath"].Value;

            TreeNode node = new TreeNode(name);
            NodeInfo info = new NodeInfo();
            info._xmlPath = xmlPath;
            info._formType = formType;

            node.Tag = info;

            libNode.Nodes.Add(node);
        }

        static private Boolean readEqXML(TreeView tree, XmlDocument doc)
        {
            XmlNodeList roots = doc.GetElementsByTagName(XMLNameEq);
            XmlNode eqNode = roots[0];

            eqTreeInit(tree);

            XmlNode chipLibNode = eqNode.SelectSingleNode("芯片库");
            XmlNode ppcNode = chipLibNode.SelectSingleNode("PPC");
            XmlNodeList ppcChildren = ppcNode.ChildNodes;
            TreeNode libNode = tree.Nodes[0].Nodes[0];
            for (int i = 0; i < ppcChildren.Count; i++)
            {
                xmlToTreeNode(libNode, ppcChildren[i],PPCInitForm.PropertyFormType.PPC);
            }

            XmlNode fpgaNode = chipLibNode.SelectSingleNode("FPGA");
            XmlNodeList fpgaChildren = fpgaNode.ChildNodes;
            libNode = tree.Nodes[0].Nodes[1];
            for (int i = 0; i < fpgaChildren.Count; i++)
            {
                xmlToTreeNode(libNode, fpgaChildren[i], PPCInitForm.PropertyFormType.FPAG);
            }

            XmlNode zynqNode = chipLibNode.SelectSingleNode("ZYNQ");
            XmlNodeList zynqChildren = zynqNode.ChildNodes;
            libNode = tree.Nodes[0].Nodes[2];
            for (int i = 0; i < zynqChildren.Count; i++)
            {
                xmlToTreeNode(libNode, zynqChildren[i], PPCInitForm.PropertyFormType.ZYNQ);
            }

            XmlNode boardLibNode = eqNode.SelectSingleNode("板卡库");
            XmlNodeList boardChildren = boardLibNode.ChildNodes;
            libNode = tree.Nodes[1];
            for (int i = 0; i < boardChildren.Count; i++)
            {
                xmlToTreeNode(libNode, boardChildren[i],PPCInitForm.PropertyFormType.BOARD);
            }

            XmlNode slotsLibNode = eqNode.SelectSingleNode("背板库");
            XmlNodeList slotsChildren = slotsLibNode.ChildNodes;
            libNode = tree.Nodes[2];
            for (int i = 0; i < slotsChildren.Count; i++)
            {
                xmlToTreeNode(libNode, slotsChildren[i],PPCInitForm.PropertyFormType.SLOTS);
            }

            XmlNode containerLibNode = eqNode.SelectSingleNode("机箱库");
            XmlNodeList containerChildren = containerLibNode.ChildNodes;
            libNode = tree.Nodes[3];
            for (int i = 0; i < containerChildren.Count; i++)
            {
                xmlToTreeNode(libNode, containerChildren[i],PPCInitForm.PropertyFormType.CONTIANER);
            }
            return true;
        }

        static private void cpTreeInit(TreeView eqTree)
        {
            TreeNode componentLibNode = new TreeNode("构件库");
            TreeNode appLibNode = new TreeNode("应用库");
            eqTree.Nodes.AddRange(new TreeNode[] { componentLibNode ,appLibNode});
        }

        static private Boolean readCpXML(TreeView tree, XmlDocument doc)
        {
            cpTreeInit(tree);
            XmlNodeList roots = doc.GetElementsByTagName(XMLNameCp);
            XmlNode cpNode = roots[0];

            XmlNode compLibNode = cpNode.SelectSingleNode("构件库");
            XmlNodeList compChildren = compLibNode.ChildNodes;
            TreeNode libNode = tree.Nodes[0];
            for (int i = 0; i < compChildren.Count; i++)
            {
                xmlToTreeNode(libNode, compChildren[i], PPCInitForm.PropertyFormType.COMPONENT);
            }

            XmlNode appLibNode = cpNode.SelectSingleNode("应用库");
            XmlNodeList appChildren = appLibNode.ChildNodes;
            libNode = tree.Nodes[1];
            for (int i = 0; i < appChildren.Count; i++)
            {
                xmlToTreeNode(libNode, appChildren[i], PPCInitForm.PropertyFormType.APPLICATION);
            }
            return true;
        }

        static public Boolean ReadXML(TreeView tree, string filepath)
        {
            XmlDocument doc;
            Boolean ret = false;
            try
            {
                doc = new XmlDocument();
                doc.Load(filepath);
            }
            catch
            {
                MessageBox.Show("Error:XML文件（" + filepath +")打开错误");
                return false;
            }
            if (String.Compare(tree.Name, "_eqTreeView", false) == 0)
            {
                ret = readEqXML(tree, doc);
            }
            else if (String.Compare(tree.Name, "_cpTreeView", false) == 0)
            {
                ret = readCpXML(tree, doc);
            }
            return ret;
        }
    }

    public class NodeInfo
    {
        public Boolean _formFlag;
        public ShowViewForm _form;
        public PPCInitForm.PropertyFormType _formType;
        public String _xmlPath;
        public object _info;

        public NodeInfo()
        {
            _formFlag = false;
            _form = null;
            _xmlPath = String.Empty;
            _info = null;
            _formType = 0;
        }

        /*初始化一棵树的所有节点的Tag属性为NodeInfo*/
        public static void InitTreeNodeInfo(TreeView tree)
        {
            Queue nodeQue = new Queue();
            foreach (TreeNode tempNode in tree.Nodes)//注意：空树是否异常
            {
                nodeQue.Enqueue(tempNode);
            }
            while (nodeQue.Count != 0)
            {
                TreeNode tempNode = (TreeNode)nodeQue.Dequeue();
                /*添加子节点到队列*/
                foreach (TreeNode childNode in tempNode.Nodes)//注意：空树是否异常
                {
                    nodeQue.Enqueue(childNode);
                }
                /*本节点Tag属性设置*/
                tempNode.Tag = new NodeInfo();
            }
        }

        /*判断节点对应的窗体是否显示*/
        public static Boolean FormShowed(TreeNode node)
        {
            NodeInfo info;
            try
            {
                info = (NodeInfo)node.Tag;
                if (info._formFlag)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        ///*判断节点对应的窗体是否显示*/
        //public static Form GetForm(TreeNode node)
        //{
        //    NodeInfo info;
        //    try
        //    {
        //        info = (NodeInfo)node.Tag;
        //        return info._form;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /*设置节点绑定对应的窗体*/
        public static Boolean AttachForm(TreeNode node, ShowViewForm form)
        {
            NodeInfo info;
            try
            {
                info = (NodeInfo)node.Tag;
                info._formFlag = true;
                info._form = form;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /*设置节点分离对应的窗体*/
        public static Boolean DetachForm(TreeNode node)
        {
            NodeInfo info;
            try
            {
                info = (NodeInfo)node.Tag;
                info._formFlag = false;
                info._form = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public static void InitSrTreeFormTyp()
        //{
        //}
    }


}
