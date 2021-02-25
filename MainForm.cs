using System;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Net;
using DRSysCtrlDisplay.Properties;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// 该类为单例模式
    /// </summary>
    public partial class MainForm : Form
    {
        private delegate void ThreadAddText(string text);   //其他线程使用该委托来更新UI控件
        private static MainForm _uniqueInstance;            //主界面的实例   
        public event Action<TreeNode> AddSourceEvent;       //添加资源的事件
        public event Action<TreeNode> ClearSourceEvent;     //清除资源的事件
        public event Action<TreeNode> MatchAppEvent;        //匹配应用的事件
        public event Action<TreeNode> LoadFilesEvent;       //部署文件的事件
        public event Action<TreeNode> RecfgEvent;           //重构应用的事件

        private MainForm()
        {
            InitializeComponent();
            Initialize();
            FuncToolStripMenuItem.Click += new EventHandler(FuncToolStripMenuItem_Click);
            PropToolStripMenuItem.Click += new EventHandler(PropToolStripMenuItem_Click);
            OutPutToolStripMenuItem.Click += new EventHandler(OutPutToolStripMenuItem_Click);
            Connect_ToolStripMenuItem.Click += new System.EventHandler(this.Connect_ToolStripMenuItem_Click);
            _connectTSBtn.Click += new EventHandler(Connect_ToolStripMenuItem_Click);
            this.Load += new System.EventHandler(this.MainForm_Load);
        }

        public static MainForm GetInstance()
        {
            if (_uniqueInstance == null)
            {
                _uniqueInstance = new MainForm();
            }
            return _uniqueInstance;
        }

        public static DockPanel GetPanel()
        {
            return _uniqueInstance.dockPanel1;
        }

        /// <summary>
        /// 主程序一开始运行的时候进行的初始化
        /// </summary>
        private void Initialize()
        {
            this._connectTSBtn.Image = ((System.Drawing.Image)(Resources.ConnectImage));
            //初始化所有的XML相关操作的文件夹
            XMLManager.PathManager.CreateXmlDictorys();
        }

        private void SetStatusTextInner(string text)
        {
            if (this.statusStrip1.InvokeRequired)
            {
                var threadWork = new ThreadAddText(SetStatusTextInner);
                this.Invoke(threadWork, text);
            }
            else
            {
                this.toolStripStatusLabel1.Text = text;
            }
        }

        public static void SetStatusText(string text)
        {
            MainForm.GetInstance().SetStatusTextInner(text);
        }

        private void SetOutPutTextInner(string text)
        {
            if (OutPutForm.GetInstacne().InvokeRequired)
            {
                var threadWork = new ThreadAddText(OutPutForm.Log);
                this.Invoke(threadWork, text);
            }
            else
            {
                OutPutForm.Log(text);
            }
        }

        public static void SetOutPutText(string text)
        {
            MainForm.GetInstance().SetOutPutTextInner(text);
        }

        #region 事件处理函数

        private void MainForm_Load(object sender, EventArgs e)
        {
            //显示FuncItem            
            FuncItemsForm fItemsWnd = FuncItemsForm.GetInstance();
            fItemsWnd.Show(dockPanel1, DockState.DockLeft);

            //显示Property
            PropertyForm propertyWnd = PropertyForm.GetInstance();
            propertyWnd.Show(dockPanel1, DockState.DockRight);

            //显示OutPutForm
            OutPutForm outPutForm = OutPutForm.GetInstacne();
            outPutForm.Show(dockPanel1, DockState.DockBottom);
        }

        //点击菜单，显示功能区子窗体
        private void FuncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FuncItemsForm fItemsWnd = FuncItemsForm.GetInstance();
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            item.Checked = !item.Checked;
            if (item.Checked)
            {
                fItemsWnd.Show(dockPanel1, DockState.DockLeft);
            }
            else
            {
                fItemsWnd.Hide();
            }
        }

        //点击菜单，显示属性区子窗体
        private void PropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyForm propertyWnd = PropertyForm.GetInstance();
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            item.Checked = !item.Checked;
            if (item.Checked)
            {
                propertyWnd.Show(dockPanel1, DockState.DockRight);
            }
            else
            {
                propertyWnd.Hide();
            }
        }

        //点击菜单，显示输出区子窗体
        private void OutPutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutPutForm outPutForm = OutPutForm.GetInstacne();
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            item.Checked = !item.Checked;
            if (item.Checked)
            {
                outPutForm.Show(dockPanel1, DockState.DockBottom);
            }
            else
            {
                outPutForm.Hide();
            }
        }

        //点击连接目标机菜单选项的事件处理函数
        private void Connect_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var connectForm = TargetConnectForm.Instance;
            connectForm.ShowForm();
            if (connectForm.DialogResult == DialogResult.Yes)
            {
                connectForm.CloseForm();

                //开启Tcp通信管理服务
                var tcpManager = TcpManager.Instance;
                tcpManager.RegAutoCmdProc(new TcpAutoCmdProcer_Log());
                tcpManager.StartWork();

                //连接选中的目标机
                if (connectForm.ChoosedTargets.Count <= 0)
                {
                    return;
                }
                string choosedIp = ((IPAddress)(connectForm.ChoosedTargets[0])).ToString();

                MainForm.SetOutPutText(string.Format("连接{0}中...", choosedIp));
                if (tcpManager.AddClient(choosedIp))
                {
                    SetStatusTextInner(string.Format("连接{0}成功!", choosedIp));
                    MainForm.SetOutPutText(string.Format("连接{0}成功!", choosedIp));
                }
                else
                {
                    SetStatusTextInner(string.Format("连接{0}失败!", choosedIp));
                    MainForm.SetOutPutText(string.Format("连接{0}失败!", choosedIp));
                }
            }
            else
            {
                connectForm.CloseForm();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            TargetConnectForm.Instance.EndListen();//退出程序要关掉广播监听的线程
            base.OnClosing(e);
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
            aboutForm.Dispose();
        }

        private void _addSrcTSBtn_Click(object sender, EventArgs e)
        {
            if (AddSourceEvent != null)
            {
                AddSourceEvent(null);
            }
        }

        private void _delSrcTSBtn_Click(object sender, EventArgs e)
        {
            if (ClearSourceEvent != null)
            {
                ClearSourceEvent(null);
            }
        }

        private void _matchAppTSBtn_Click(object sender, EventArgs e)
        {
            if (MatchAppEvent != null)
            {
                MatchAppEvent(null);
            }
        }

        private void _loadFilesTSBtn_Click(object sender, EventArgs e)
        {
            if (LoadFilesEvent != null)
            {
                LoadFilesEvent(null);
            }
        }

        private void _recfgTSBtn_Click(object sender, EventArgs e)
        {
            if (RecfgEvent != null)
            {
                RecfgEvent(null);
            }
        }

        private void 添加资源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AddSourceEvent != null)
            {
                AddSourceEvent(null);
            }
        }

        private void 清除资源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClearSourceEvent != null)
            {
                ClearSourceEvent(null);
            }
        }

        private void 匹配ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MatchAppEvent != null)
            {
                MatchAppEvent(null);
            }
        }

        private void 部署文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadFilesEvent != null)
            {
                LoadFilesEvent(null);
            }
        }

        private void 重构ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RecfgEvent != null)
            {
                RecfgEvent(null);
            }
        }

        #endregion 事件处理函数

    }
}
