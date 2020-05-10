using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace DRSysCtrlDisplay
{
    static class XMLManager
    {
        static public Boolean PropertyToNode(String name, TreeNode parentNode, NodeInfo info)
        {
            TreeNode node = new TreeNode(name);
            node.Tag = info;
            parentNode.Nodes.Add(node);
            return true;
        }

        static public Boolean PropertyToXml(PropertyGrid grid)
        {
            return true;
        }

        static public void XmlWriteAttr(XmlTextWriter writer, string name, string value)
        {
            writer.WriteStartAttribute(name);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        static public Boolean CreateFile(String dPath, String fPath)
        {
            CreateDirectory(dPath);
            if (File.Exists(fPath))
            {
                //文件已存在，确认是否覆盖
            }
            else
            {
                File.Create(fPath).Close();
            }
            return true;
        }

        static public void CreateDirectory(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


    }
}
