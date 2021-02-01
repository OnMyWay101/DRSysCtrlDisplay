using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using DynamicNode = DRSysCtrlDisplay.DynamicTopo.DynamicNode;

namespace DRSysCtrlDisplay
{
    public partial class ExeFileForm : Form
    {
        private Hashtable _exeFileList = new Hashtable();   //可执行文件的列表
        List<DynamicNode> _matchNode = null;                  //应用匹配的构件集合
        const int _LvFilePathNum = 8;                         //“文件路径”在listView中的列数
        const int _LvFileTimeNum = 9;                         //“上传时间”在listView中的列数

        public ExeFileForm(List<DynamicNode> matchNode)
        {
            Init(matchNode);
        }

        public ExeFileForm(List<DynamicNode> matchNode, Hashtable exeFile)
        {
            Init(matchNode);
            if (exeFile != null)
            {
                _exeFileList = exeFile;
                foreach(ListViewItem item in ComponentLV.Items)
                {
                    if (exeFile.Contains(item.Index))
                    {
                        FileStream fs = exeFile[item.Index] as FileStream;
                        item.SubItems[_LvFilePathNum].Text = fs.Name;
                    }
                }
            }
        }

        private void Init(List<DynamicNode> matchNode)
        {
            if (matchNode == null)
            {
                MessageBox.Show("构件数量错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _matchNode = matchNode;

            InitializeComponent();
            ListViewInit();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// 根据匹配的节点信息初始化ListView
        /// </summary>
        private void ListViewInit()
        {
            ComponentLV.BeginUpdate();
            ComponentLV.View = View.Details;
            ComponentLV.GridLines = true;
            ComponentLV.FullRowSelect = true;
            ComponentLV.Columns.Add("计算颗粒编号", 90, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("应用名", 70, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("构件名", 70, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("机箱号", 70, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("槽位号", 70, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("芯片号", 70, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("芯片类型", 70, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("芯片名称", 70, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("文件路径", 350, HorizontalAlignment.Center);
            ComponentLV.Columns.Add("上传时间", 90, HorizontalAlignment.Center);

            for (int i = 0; i < _matchNode.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = i.ToString();
                lvi.SubItems.Add(_matchNode[i].ComName);
                lvi.SubItems.Add(_matchNode[i].CNode.Name);
                lvi.SubItems.Add((_matchNode[i].SNode.FrameId).ToString());
                lvi.SubItems.Add((_matchNode[i].SNode.SlotId).ToString());
                lvi.SubItems.Add(_matchNode[i].SNode.EndId.ToString());
                lvi.SubItems.Add(_matchNode[i].SNode.NodeType.ToString());
                lvi.SubItems.Add(_matchNode[i].SNode.Name.ToString());
                lvi.SubItems.Add(String.Empty);
                lvi.SubItems.Add(String.Empty);
                ComponentLV.Items.Add(lvi);
            }
            ComponentLV.EndUpdate();     
        }

        //获取该文件列表对应的Hashtable
        public Hashtable GetFilesHt()
        {
            return _exeFileList;
        }

        private void ComponentLV_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _UpdateCMS.Show(ComponentLV, e.Location);
            }
        }

        private void _UpdateItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                ListViewItem lvItem = ComponentLV.SelectedItems[0];
                //清空之前的信息
                if (_exeFileList.Contains(lvItem.Index))
                {
                    _exeFileList.Remove(lvItem.Index);
                }
                //添加现在的信息
                string path = fd.FileName;
                ComponentLV.BeginUpdate();
                lvItem.SubItems[_LvFilePathNum].Text = path;
                lvItem.SubItems[_LvFileTimeNum].Text = DateTime.Now.ToString();
                ComponentLV.EndUpdate();
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                _exeFileList.Add(ComponentLV.SelectedItems[0].Index, fs);
            }
        }
        
        private void _ClearItem_Click(object sender, EventArgs e)
        {
            ListViewItem lvItem = ComponentLV.SelectedItems[0];
            if (_exeFileList.Contains(lvItem.Index))
            {
                _exeFileList.Remove(lvItem.Index);
                lvItem.SubItems[_LvFilePathNum].Text = "";
                lvItem.SubItems[_LvFileTimeNum].Text = "";
            }
        }



        private void YesButton_Click(object sender, EventArgs e)
        {
            DeployFiles();
            this.DialogResult = DialogResult.Yes;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public void DeployFiles()
        {
            var tcpManager = TcpManager.Instance;
            try
            {
                string ip = tcpManager.ipList[0];
                //轮询发送更新文件
                for (int i = 0; i < _matchNode.Count; i++)
                {
                    var lvItem = ComponentLV.Items[i];
                    if (lvItem.SubItems[_LvFilePathNum].Text != String.Empty)       //判断是否上传了文件
                    {
                        var node = _matchNode[i];
                        var choosedFs = (FileStream)(_exeFileList[i]);
                        var fileName = choosedFs.Name.Substring(choosedFs.Name.LastIndexOf("\\") + 1);

                        var cmd = new TcpCommand_UpLoadApp(fileName, node.SNode.SlotId + 1, node.SNode.NodeType, choosedFs);
                        tcpManager.SendOneCmd(ip, cmd);
                    }
                }

                //发送开始命令
                tcpManager.SendOneCmd(ip, new TcpCommand_StartApp());
            }
            catch (Exception ex)
            {
                MessageBox.Show("ExeFileForm:" + ex.Message);
            }
        }
    }
}
