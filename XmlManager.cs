using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using System.Reflection;
using DRSysCtrlDisplay.Princeple;

namespace DRSysCtrlDisplay
{
    public class NodeInfo
    {
        public event Action<TargetNode> NodeInfoChanged;    //节点信息改变的事件
        public string _nodeType;                            //当前节点为设备节点，对应的类型；
        public Boolean _formFlag;                           //当前节点有对应的窗体显示：true-有；false-无；
        public ShowViewForm _form;                          //显示窗体的引用
        public Princeple.FormType _formType;                //显示窗体的窗体类型
        public string _xmlPath;                             //节点对应的XML文件
        public object _info;                                //其他信息

        public NodeInfo()
        {
            _formFlag = false;
            _form = null;
            _nodeType = string.Empty;
            _info = null;
            _formType = 0;
        }

        public void TriggerNodeInfoChanged(TargetNode tNode)
        {
            if (NodeInfoChanged != null)
            {
                NodeInfoChanged(tNode);
            }
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
            NodeInfo info = (NodeInfo)node.Tag;
            return info._formFlag;
        }

        /*设置节点绑定对应的窗体*/
        public static void AttachForm(TreeNode node, ShowViewForm form)
        {
            NodeInfo info = (NodeInfo)node.Tag;
            info._formFlag = true;
            info._form = form;
        }

        /*设置节点分离对应的窗体*/
        public static void DetachForm(TreeNode node)
        {
            NodeInfo info = (NodeInfo)node.Tag;
            info.NodeInfoChanged = null;
            info._formFlag = false;
            info._form = null;
        }

        /// <summary>
        /// 给一个TreeView的节点添加一个子节点
        /// </summary>
        /// <param name="fatherNode"></param>
        /// <param name="nodeName"></param>
        /// <param name="type"></param>
        public static void AddChildrenNode(TreeNode fatherNode, string nodeName, Princeple.FormType type)
        {
            var node = new TreeNode(nodeName);
            node.Tag = new NodeInfo();
            ((NodeInfo)(node.Tag))._formType = type;
            ((NodeInfo)(node.Tag))._nodeType = type.ToString();
            ((NodeInfo)(node.Tag))._xmlPath = GetXmlPath(type, nodeName);
            fatherNode.Nodes.Add(node);
        }
        /// <summary>
        /// 删除一个TreeView的节点一个子节点
        /// </summary>
        /// <param name="fatherNode"></param>
        /// <param name="nodeName"></param>
        public static void DeleteTreeNode(TreeNode node)/*删除了参数nodeName，看后面需不需要*/
        {
            try
            {
                NodeInfo info = (NodeInfo)(node.Tag);
                //删除文件，移除节点
                File.Delete(info._xmlPath);
                string dirPath = info._xmlPath.Substring(0, info._xmlPath.LastIndexOf("."));
                DeleteDir(dirPath);
                node.Remove();
            }
            catch
            {
                MessageBox.Show("DeleteTreeNode:删除节点Xml文件失败!");
            }
        }

        private static void DeleteDir(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                var filePaths = Directory.GetFiles(dirPath);
                foreach (var filePath in filePaths)
                {
                    File.Delete(filePath);
                }
                Directory.Delete(dirPath);
            }
        }

        //通过Node对应的窗体类型，返回Node的类型
        private static string GetNodeType(Princeple.FormType type)
        {
            string result;
            switch (type)
            {
                case FormType.PPC:
                    result = "PPC";
                    break;
                case FormType.FPGA:
                    result = "FPGA";
                    break;
                case FormType.ZYNQ:
                    result = "ZYNQ";
                    break;
                case FormType.BOARD:
                    result = "Board";
                    break;
                case FormType.BACKPLANE:
                    result = "BackPlane";
                    break;
                case FormType.CONTIANER:
                    result = "Container";
                    break;
                case FormType.COMPONENT:
                    result = "Component";
                    break;
                default://FormType.APPLICATION
                    result = "Application";
                    break;
            }
            return result;
        }

        private static string GetXmlPath(Princeple.FormType type, string fileName)
        {
            string resultDir;
            switch (type)
            {
                case FormType.PPC:
                    resultDir = XMLManager.PathManager.GetPPCPath();
                    break;
                case FormType.FPGA:
                    resultDir = XMLManager.PathManager.GetFPGAPath();
                    break;
                case FormType.ZYNQ:
                    resultDir = XMLManager.PathManager.GetZYNQPath();
                    break;
                case FormType.BOARD:
                    resultDir = XMLManager.PathManager.GetBoardPath();
                    break;
                case FormType.BACKPLANE:
                    resultDir = XMLManager.PathManager.GetBackPlanePath();
                    break;
                case FormType.CONTIANER:
                    resultDir = XMLManager.PathManager.GetContainerPath();
                    break;
                case FormType.COMPONENT:
                    resultDir = XMLManager.PathManager.GetComponentPath();
                    break;
                default://FormType.APPLICATION
                    resultDir = XMLManager.PathManager.GetApplicationsPath();
                    break;
            }
            return  string.Format(@"{0}\{1}.xml", resultDir, fileName);
        }
    }

    /// <summary>
    /// 一个用于XML相关事务管理的类;
    /// </summary>
    public class XMLManager
    {
        /// <summary>
        /// 用来管理XML文件及相关文件夹所对应目录的类
        /// </summary>
        public static class PathManager
        {
            private const string treeViewConfigDir = @"ConfigFile\";        //TreeView配置文件的文件夹
            private const string equipLibFile = "EquipLib.xml";             //设备库TreeView的XML配置文件
            private const string componentLibFile = "ComponentLib.xml";     //构件库TreeView的XML配置文件

            private const string libDir = @"Lib\";                          //设备库和构件库所在的文件夹
            //public const string ComsDir = "构件库";                         //构件库文件夹
            public const string ComsDir = "应用库";                         //构件库文件夹
            //public const string CompsDir = "构件库";                        //构件文件夹
            public const string CompsDir = "应用库";                        //构件文件夹
            public const string AppsDir = "应用库Ex";                         //应用文件夹

            public const string EqsDir = "设备库";                          //设备库文件夹
            public const string CoresDir = "芯片库";                        //芯片库文件夹
            public const string BoardsDir = "板卡库";                       //板卡库文件夹
            public const string BackPlanesDir = "背板库";                   //背板库文件夹
            public const string ContainersDir = "机箱库";                   //机箱库文件夹
            public const string SyssDir = "系统库";                         //系统库文件夹

            public const string PpcsDir = "PPCs";                           //PowerPC文件夹
            public const string FpgasDir = "FPGAs";                         //FPGA文件夹
            public const string ZynqsDir = "ZYNQs";                         //ZYNQ文件夹

            /// <summary>	
            /// 判断有无文件存在
            /// </summary>
            /// <returns>true:无,或者选择“覆盖”；false:不覆盖，或者其他错误</returns>
            public static bool CheckFile(string filePath)
            {
                if (File.Exists(filePath))
                {
                    if (MessageBox.Show("文件已存在，是否覆盖？", "XMLManager.HandleType.CheckFile", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }

            /// <summary>
            /// 用于程序首次在电脑里面运行的时候，搭建相关xml存放的目录；
            /// </summary>
            public static void CreateXmlDictorys()
            {
                List<string> initDirs = new List<string>(); //需要初始化的叶文件夹集合

                initDirs.Add(GetTreeViewConfigPath());

                initDirs.Add(GetComponentPath());
                //initDirs.Add(GetApplicationsPath());

                initDirs.Add(GetBoardPath());
                initDirs.Add(GetBackPlanePath());
                initDirs.Add(GetContainerPath());
                initDirs.Add(GetSysPath());

                initDirs.Add(GetPPCPath());
                initDirs.Add(GetFPGAPath());
                initDirs.Add(GetZYNQPath());

                foreach (var dic in initDirs)
                {
                    if (!Directory.Exists(dic))
                    {
                        Directory.CreateDirectory(dic);
                    }
                }
            }

            #region 获取文件夹路径

            public static string GetTreeViewConfigPath()
            {
                return treeViewConfigDir;
            }

            //获取构件库的文件夹路径
            public static string GetComponentPath()
            {
                return libDir + Path.Combine(ComsDir, CompsDir);
            }

            public static string GetApplicationsPath()
            {
                return libDir + Path.Combine(ComsDir, AppsDir);
            }

            public static string GetBoardPath()
            {
                return libDir + Path.Combine(EqsDir, BoardsDir);
            }

            public static string GetBackPlanePath()
            {
                return libDir + Path.Combine(EqsDir, BackPlanesDir);
            }

            public static string GetContainerPath()
            {
                return libDir + Path.Combine(EqsDir, ContainersDir);
            }

            public static string GetSysPath()
            {
                return libDir + Path.Combine(EqsDir, SyssDir);
            }

            public static string GetPPCPath()
            {
                return libDir + Path.Combine(Path.Combine(EqsDir, CoresDir), PpcsDir);
            }

            public static string GetFPGAPath()
            {
                return libDir + Path.Combine(Path.Combine(EqsDir, CoresDir), FpgasDir);
            }

            public static string GetZYNQPath()
            {
                return libDir + Path.Combine(Path.Combine(EqsDir, CoresDir), ZynqsDir);
            }

            #endregion

            #region 获取文件的路径

            //设备库的配置文件
            public static string GetEqLibFilePath()
            {
                return treeViewConfigDir + equipLibFile;
            }

            //构件库的配置文件
            public static string GetCmpLibFilePath()
            {
                return treeViewConfigDir + componentLibFile;
            }

            #endregion
        }

        public static class HandleTreeView    /*该类用于处理TreeView与XML文件的转化*/
        {
            public static bool ReadXMLToTreeView(TreeView tree, string filePath) /*通过XML文件初始化TreeView*/
            {
                //先判断文件名是否符合标准
                if (!(String.Compare(filePath, PathManager.GetEqLibFilePath()) == 0 ||
                      String.Compare(filePath, PathManager.GetCmpLibFilePath()) == 0))
                {
                    return false;
                }
                if (!File.Exists(filePath))
                {//创建默认的XML文件
                    CreateDefaultXML(filePath);
                }

                XElement xmlInfo = XElement.Load(filePath);
                XElementInitTreeNode(tree.Nodes, xmlInfo);
                return true;
            }

            public static bool ReadTreeViewToXML(TreeView tree, string filePath) /*通过TreeView结构生成XML文件*/
            {
                XElement xmlInfo = new XElement(GetJustFileName(filePath));
                TreeNodeInitXElement(tree.Nodes, xmlInfo);
                xmlInfo.Save(filePath);
                return true;
            }

            /// <summary>
            /// 使TreeView的配置XML与Lib下构件库与设备库对应的xml文件同步
            /// </summary>
            public static void SynchronizeTreeViewXML()
            {
                try
                {
                    var eqConfigFilePath = PathManager.GetEqLibFilePath();      //设备库配置文件
                    var cmpConfigFilePath = PathManager.GetCmpLibFilePath();    //构件库配置文件

                    //同步设备库TreeView对应的XML配置文件
                    File.Delete(eqConfigFilePath);
                    CreateDefaultXML(eqConfigFilePath);
                    XDocument eqLibXd = XDocument.Load(eqConfigFilePath);
                    XElement eqRoot = eqLibXd.Element(PathManager.EqsDir);

                    Dir2XElement(eqRoot.Element(PathManager.CoresDir).Element(PathManager.PpcsDir)
                        , PathManager.GetPPCPath(), FormType.PPC.ToString());
                    Dir2XElement(eqRoot.Element(PathManager.CoresDir).Element(PathManager.FpgasDir)
                        , PathManager.GetFPGAPath(), FormType.FPGA.ToString());
                    Dir2XElement(eqRoot.Element(PathManager.CoresDir).Element(PathManager.ZynqsDir)
                        , PathManager.GetZYNQPath(), FormType.ZYNQ.ToString());

                    Dir2XElement(eqRoot.Element(PathManager.BoardsDir)
                        , PathManager.GetBoardPath(), FormType.BOARD.ToString());
                    Dir2XElement(eqRoot.Element(PathManager.BackPlanesDir)
                        , PathManager.GetBackPlanePath(), FormType.BACKPLANE.ToString());
                    Dir2XElement(eqRoot.Element(PathManager.ContainersDir)
                        , PathManager.GetContainerPath(), FormType.CONTIANER.ToString());
                    Dir2XElement(eqRoot.Element(PathManager.SyssDir)
                        , PathManager.GetSysPath(), FormType.SYSTEM.ToString());

                    eqLibXd.Save(PathManager.GetEqLibFilePath());

                    //同步构件库TreeView对应的XML配置文件
                    File.Delete(cmpConfigFilePath);
                    CreateDefaultXML(cmpConfigFilePath);
                    XDocument comLibXd = XDocument.Load(cmpConfigFilePath);
                    XElement comRoot = comLibXd.Element(PathManager.ComsDir);

                    Dir2XElement(comRoot.Element(PathManager.CompsDir)
                        , PathManager.GetComponentPath(), FormType.COMPONENT.ToString());
                    //Dir2XElement(comRoot.Element(PathManager.AppsDir)
                    //    , PathManager.GetApplicationsPath(), FormType.APP.ToString());

                    comLibXd.Save(PathManager.GetCmpLibFilePath());
                }
                catch
                {
                    MessageBox.Show("TreeView Config XML Error!");
                }
            }

            //递归调用,按照深度搜索访问树形结构
            private static void TreeNodeInitXElement(TreeNodeCollection treeNodes, XElement element)
            {
                //定义边界条件
                if (treeNodes.Count == 0)
                {
                    return;
                }
                foreach (TreeNode node in treeNodes)
                {
                    XElement childElement;
                    //node为设备节点
                    if (((NodeInfo)(node.Tag))._nodeType != string.Empty)
                    {
                        childElement = new XElement(((NodeInfo)(node.Tag))._nodeType,
                                            new XAttribute("Name", node.Text),
                                            new XAttribute("Path", ((NodeInfo)(node.Tag))._xmlPath));
                    }
                    else
                    {
                        childElement = new XElement(node.Text);
                    }
                    element.Add(childElement);
                    TreeNodeInitXElement(node.Nodes, childElement);
                }
            }

            //递归调用,按照深度搜索访问树形结构
            private static void XElementInitTreeNode(TreeNodeCollection treeNodes, XElement element)
            {
                //定义边界条件
                if (!element.HasElements)
                {
                    return;
                }
                IEnumerable<XElement> elements = element.Elements();
                foreach (XElement e in elements)
                {
                    TreeNode node;
                    if ((!e.HasElements) && (e.Attribute("Name") != null) && (e.Attribute("Path") != null))//e为叶节点且有“Name”属性
                    {
                        node = new TreeNode(e.Attribute("Name").Value);
                        node.Tag = new NodeInfo();
                        ((NodeInfo)(node.Tag))._formType = (FormType)Enum.Parse(typeof(FormType), e.Name.LocalName);
                        ((NodeInfo)(node.Tag))._nodeType = e.Name.LocalName;
                        ((NodeInfo)(node.Tag))._xmlPath = e.Attribute("Path").Value;
                    }
                    else
                    {
                        node = new TreeNode(e.Name.LocalName);
                        node.Tag = new NodeInfo();
                    }
                    treeNodes.Add(node);
                    XElementInitTreeNode(node.Nodes, e);
                }
            }

            private static void CreateDefaultXML(string filePath)
            {
                if (filePath == PathManager.GetEqLibFilePath())
                {
                    CreatDefaultEquipLibXML();
                }
                else if (filePath == PathManager.GetCmpLibFilePath())
                {
                    CreatDefaultComponentLibXML();
                }
            }

            private static void CreatDefaultEquipLibXML()
            {
                XElement xmlTree = new XElement(PathManager.EqsDir,
                    new XElement(PathManager.CoresDir,
                        new XElement(PathManager.PpcsDir),
                        new XElement(PathManager.FpgasDir),
                        new XElement(PathManager.ZynqsDir)
                        ),
                    new XElement(PathManager.BoardsDir),
                    new XElement(PathManager.BackPlanesDir),
                    new XElement(PathManager.ContainersDir),
                    new XElement(PathManager.SyssDir)
                    );
                xmlTree.Save(PathManager.GetEqLibFilePath());
            }

            private static void CreatDefaultComponentLibXML()
            {
                XElement xmlTree = new XElement(PathManager.ComsDir
                    , new XElement(PathManager.CompsDir)
                    //, new XElement(PathManager.AppsDir)
                    );
                xmlTree.Save(PathManager.GetCmpLibFilePath());
            }

            /// <summary>
            /// 从文件的绝对路径中获取文件的名称，且不包含扩展名
            /// </summary>
            /// <param name="fileAbsolutePath"></param>
            /// <returns></returns>
            private static string GetJustFileName(string fileAbsolutePath)
            {
                var pathSlices = fileAbsolutePath.Split(new char[] { '\\', '.' });//对绝对路径的名字去取名字分段
                return pathSlices[pathSlices.Length - 2];           //倒数第一个为扩展名，倒数第二个成员才是文件名
            }

            private static string GetRelativePath(string fileAbsolutePath, string relativeDir)
            {
                var pathSlices = fileAbsolutePath.Split(new char[] { '\\' });//对绝对路径的名字去取名字分段
                return Path.Combine(relativeDir, pathSlices[pathSlices.Length - 1]);
            }

            private static void Dir2XElement(XElement xe, string dirPath, string elementName)
            {
                string[] fileNames = Directory.GetFiles(dirPath);
                foreach (string fileName in fileNames)
                {
                    xe.Add(new XElement(elementName,
                        new XAttribute("Name", GetJustFileName(fileName)),          //放入文件名
                        new XAttribute("Path", GetRelativePath(fileName, dirPath))  //放入相对路径
                        ));
                }
            }

            /// <summary>
            /// 通过名字来判断对应的窗体种类
            /// </summary>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            private static Princeple.FormType GetFormType(string name)
            {
                Princeple.FormType type = Princeple.FormType.None;
                switch (name)
                {
                    case "PPC":
                        type = Princeple.FormType.PPC;
                        break;
                    case "FPGA":
                        type = Princeple.FormType.FPGA;
                        break;
                    case "ZYNQ":
                        type = Princeple.FormType.ZYNQ;
                        break;
                    case "Board":
                        type = Princeple.FormType.BOARD;
                        break;
                    case "BackPlane":
                        type = Princeple.FormType.BACKPLANE;
                        break;
                    case "Container":
                        type = Princeple.FormType.CONTIANER;
                        break;
                    case "Component":
                        type = Princeple.FormType.COMPONENT;
                        break;
                    case "Application":
                        type = Princeple.FormType.APP;
                        break;
                    default:
                        break;
                }
                return type;
            }
        }

        public interface IXmlTransformByName
        {
            void SaveXmlByName();
            Models.ModelBase CreateObjectByName(string objectName);
        }

        public interface IXmlTransformByPath
        {
            void SaveXmlByPath(string xmlFilePath);
            Models.ModelBase CreateObjectByPath(string objectFilePath);
        }
    }
}
