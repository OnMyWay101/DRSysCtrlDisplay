using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynamicNode = DRSysCtrlDisplay.DynamicTopo.DynamicNode;

namespace DRSysCtrlDisplay
{
    public partial class ReconfigForm : Form
    {
        private List<List<DynamicNode>> _strategyList;   //策略列表
        private List<Boolean>[] _slotOnLineFlags;        //槽位是否在线的标志
        public int ChoosedIndex { get; private set; }   //选中的策略的序号
        public DynamicTopo _dTopo;

        public Boolean AutoReconfigFlag
        {
            get { return _autoReCfgCB.Checked; }
            set { _autoReCfgCB.Checked = value; }
        }

        public ReconfigForm()
        {
            InitializeComponent();
        }

        public ReconfigForm(DynamicTopo dTopo, List<List<DynamicNode>> strategyList, List<Boolean>[] slotOnLineFlags)
        {
            //初始化成员变量
            _dTopo = dTopo;
            _slotOnLineFlags = slotOnLineFlags;
            _strategyList = strategyList;
            ChoosedIndex = -1;

            InitializeComponent();
            InitBaseListViews();
            InitStrategyLv();
            this.StartPosition = FormStartPosition.CenterParent;


            //事件处理函数
            this._yesBtn.Click += new EventHandler(On_yesBtnClick);
            this._cancelBtn.Click += new EventHandler((o, e) =>{ this.DialogResult = DialogResult.Cancel;});
            this._strategyLv.SelectedIndexChanged += new EventHandler(On_strategyLvSelectedIndexChanged);
        }

        //初始化所有的ListView
        private void InitBaseListViews()
        {
            //对_strategyLv属性进行设置
            this._strategyLv.View = View.Details;
            this._strategyLv.CheckBoxes = true;
            this._strategyLv.GridLines = true;
            this._strategyLv.FullRowSelect = true;
            this._strategyLv.Columns.Add("方案序号", -2, HorizontalAlignment.Center);
            this._strategyLv.Columns.Add("状态", -2, HorizontalAlignment.Center);

            //设置该_infoLv的属性
            _infoLv.SuspendLayout();
            _infoLv.View = View.Details;
            _infoLv.GridLines = true;
            _infoLv.FullRowSelect = true;
            _infoLv.Columns.Add("计算颗粒编号", 50, HorizontalAlignment.Center);
            _infoLv.Columns.Add("应用名", 70, HorizontalAlignment.Center);
            _infoLv.Columns.Add("构件名", 70, HorizontalAlignment.Center);
            _infoLv.Columns.Add("全局资源ID", 100, HorizontalAlignment.Center);
            _infoLv.Columns.Add("机箱号", 70, HorizontalAlignment.Center);
            _infoLv.Columns.Add("槽位号", 70, HorizontalAlignment.Center);
            _infoLv.Columns.Add("芯片号", 70, HorizontalAlignment.Center);
            _infoLv.Columns.Add("芯片类型", 70, HorizontalAlignment.Center);
            _infoLv.Columns.Add("芯片名称", 70, HorizontalAlignment.Center);
        }

        //初始化方案ListView
        private void InitStrategyLv()
        {
            //设置每行
            for (int i = 0; i < _strategyList.Count; i++ )
            {
                ListViewItem item = new ListViewItem("方案" + (i+1));
                item.UseItemStyleForSubItems = false;
                if (JudgeStrategyValid(i))
                {
                    //Todo:设置颜色
                    item.SubItems.Add("适配成功", Color.Black, Color.Green, new Font("宋体", 11));
                }
                else
                {
                    item.SubItems.Add("适配失败", Color.Black, Color.Red, new Font("宋体", 11));
                }
                _strategyLv.Items.Add(item);
            }
        }

        private void _infoLvShow(int strategyNum)
        {
            var strategy = _strategyList[strategyNum];  //选中的方案编号
            _dTopo.SetMatchedTopo(strategyNum);
            _infoLv.SuspendLayout();
            _infoLv.Items.Clear();
            for (int i = 0; i < strategy.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = i.ToString();
                lvi.SubItems.Add(strategy[i].ComName);
                lvi.SubItems.Add(strategy[i].CNode.Name);
                lvi.SubItems.Add((strategy[i].SNode.UrlId).ToString());
                lvi.SubItems.Add((strategy[i].SNode.FrameId).ToString());
                lvi.SubItems.Add((strategy[i].SNode.SlotId).ToString());
                lvi.SubItems.Add(strategy[i].SNode.EndId.ToString());
                lvi.SubItems.Add(strategy[i].SNode.NodeType.ToString());
                lvi.SubItems.Add(strategy[i].SNode.Name.ToString());
                _infoLv.Items.Add(lvi);
            }
            _infoLv.ResumeLayout();
        }

        //判断策略是否有效
        private bool JudgeStrategyValid(int strategyIndex)
        {
            var nodes = _strategyList[strategyIndex];
            foreach (var node in nodes)
            {
                //判断节点对应槽位是否为不在线
                if (!_slotOnLineFlags[node.SNode.FrameId][node.SNode.SlotId])
                {
                    return false;
                }
            }
            return true;
        }

        //从小到大找到第一个可行方案
        public int FindFirstValidStrategy()
        {
            for (int i = 0; i < _strategyList.Count; i++)
            {
                if (JudgeStrategyValid(i))
                {
                    return i;
                }
            }
            MessageBox.Show("无有效方案！");
            return -1;
        }

        private void On_yesBtnClick(object sender, EventArgs e)
        {
            ChoosedIndex = -1;
            foreach (ListViewItem item in _strategyLv.Items)
            {
                if (item.Checked)
                {
                    ChoosedIndex = _strategyLv.Items.IndexOf(item);
                }
            }
            this.DialogResult = DialogResult.Yes;
        }

        private void On_strategyLvSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_strategyLv.SelectedItems.Count > 0)
            {
                ListViewItem choosedItem = _strategyLv.SelectedItems[0];
                int choosedNum = _strategyLv.Items.IndexOf(choosedItem);
                //显示选中方案的详细信息
                _infoLvShow(choosedNum);
                //设置选中项
                foreach (ListViewItem item in _strategyLv.Items)
                {
                    item.Checked = false;
                }
                choosedItem.Checked = true;
            }

        }
    }
}
