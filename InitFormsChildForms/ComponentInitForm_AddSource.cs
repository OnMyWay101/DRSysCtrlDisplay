using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DRSysCtrlDisplay
{
    //存放Eth资源的结构体
    public struct EthSource
    {
        public int _sn;             //序号      
        public string _nodeName;    //节点名称
        public string _sourceName;  //资源名称
        //public string _protocol;    //协议
    }

    //存放Rio资源结构体
    public struct RioSource
    {
        public int _sn;              //序号
        public string _nodeName;        //节点名称
        public string _sourceName;      //资源名称
        public int _packMaxLen;      //最大包长度
        public int _bufSize;    //缓存区大小
    }

    //构件初始化界面的添加资源的子界面
    public partial class ComponentInitForm_AddSource : Form
    {       
        const string _num = "序号";
        const string _nodeName = "节点名";
        const string _sourceName = "资源名";
        const string _protocol = "协议";
        const string _maxLen = "最大数据包长度";
        const string _bufSize = "缓冲区长度";

        public string NodeName { get; private set; }

        public List<EthSource> _ethPbSources = new List<EthSource>();  //以太网发布的资源
        public List<EthSource> _ethSubSources = new List<EthSource>();  //以太网订阅的资源
        public List<RioSource> _rioPbSources = new List<RioSource>();  //rio网订阅的资源
        public List<RioSource> _rioSubSources = new List<RioSource>();  //rio网订阅的资源

        public ComponentInitForm_AddSource(string nodeName)
        {
            this.NodeName = nodeName;
            InitializeComponent();

            //初始化各个DataGridView
            InitEthDgv(_ethPbDgv);
            InitEthDgv(_ethSubDgv);
            InitRioDgv(_rioPbDgv);
            InitRioDgv(_rioSubDgv);

            //绑定事假处理函数
            _ethPbAddBtn.Click += new EventHandler(_ethPbAddBtn_Click);
            _ethPbDelBtn.Click += new EventHandler(_ethPbDelBtn_Click);
            _ethSubAddBtn.Click += new EventHandler(_ethSubAddBtn_Click);
            _ethSubDelBtn.Click += new EventHandler(_ethSubDelBtn_Click);

            _rioPbAddBtn.Click += new EventHandler(_rioPbAddBtn_Click);
            _rioPbDelBtn.Click += new EventHandler(_rioPbDelBtn_Click);
            _rioSubAddBtn.Click += new EventHandler(_rioSubAddBtn_Click);
            _rioSubDelBtn.Click += new EventHandler(_rioSubDelBtn_Click);
        }

        private void InitEthDgv(DataGridView dgv)
        {
            //设置DataGridView基本属性
            dgv.Dock = DockStyle.Fill;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;

            for (int i = 0; i < 3; i++)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn());
            }

            dgv.Columns[0].HeaderText = _num;
            dgv.Columns[1].HeaderText = _nodeName;
            dgv.Columns[2].HeaderText = _sourceName;
            //dgv.Columns[3].HeaderText = _protocol;
            dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Font = new Font("宋体", 11);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        //初始化rio网的数据表
        private void InitRioDgv(DataGridView dgv)
        {
            //设置DataGridView基本属性
            dgv.Dock = DockStyle.Fill;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;

            for (int i = 0; i < 5; i++)//添加五列
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn());
            }

            dgv.Columns[0].HeaderText = _num;
            dgv.Columns[1].HeaderText = _nodeName;
            dgv.Columns[2].HeaderText = _sourceName;
            dgv.Columns[3].HeaderText = _maxLen;
            dgv.Columns[4].HeaderText = _bufSize;
            dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Font = new Font("宋体", 11);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        #region 事件处理函数

        private void _ethPbAddBtn_Click(object sender, EventArgs e)
        {           
            BtnAddRow(sender as Button);
            Debug.WriteLine("_pbEthAddBtn_Click");
        }

        private void _ethPbDelBtn_Click(object sender, EventArgs e)
        {
            BtnDelRow(sender as Button);
            Debug.WriteLine("_pbEthDelBtn_Click");
        }

        private void _ethSubAddBtn_Click(object sender, EventArgs e)
        {
            BtnAddRow(sender as Button);
            Debug.WriteLine("_subEthAddBtn_Click");
        }

        private void _ethSubDelBtn_Click(object sender, EventArgs e)
        {
            BtnDelRow(sender as Button);
            Debug.WriteLine("_subEthDelBtn_Click");
        }

        private void _rioPbAddBtn_Click(object sender, EventArgs e)
        {                      
            BtnAddRow(sender as Button);
            Debug.WriteLine("_pbRioAddBtn_Click");
        }

        private void _rioPbDelBtn_Click(object sender, EventArgs e)
        {          
            BtnDelRow(sender as Button);
            Debug.WriteLine("_pbRioDelBtn_Click");
        }

        private void _rioSubAddBtn_Click(object sender, EventArgs e)
        {
            BtnAddRow(sender as Button);
            Debug.WriteLine("_subRioAddBtn_Click");
        }

        private void _rioSubDelBtn_Click(object sender, EventArgs e)
        {
            BtnDelRow(sender as Button);
            Debug.WriteLine("_subRioDelBtn_Click");
        }

        #endregion

        private void BtnAddRow(Button btn)
        {
            DataGridView dgv = null;
            if (object.ReferenceEquals(btn, _ethPbAddBtn))
            {
                dgv = _ethPbDgv;
            }
            else if (object.ReferenceEquals(btn, _ethSubAddBtn))
            {
                dgv = _ethSubDgv;
            }
            else if (object.ReferenceEquals(btn, _rioPbAddBtn))
            {
                dgv = _rioPbDgv;
            }
            else//_rioSubAddBtn
            {
                dgv = _rioSubDgv;
            }

            int index = dgv.Rows.Add();
            dgv.Rows[index].Cells[0].Value = index;
            dgv.Rows[index].Cells[1].Value = this.NodeName;
        }

        private void BtnDelRow(Button btn)
        {
            DataGridView dgv = null;
            if (object.ReferenceEquals(btn, _ethPbDelBtn))
            {
                dgv = _ethPbDgv;
            }
            else if (object.ReferenceEquals(btn, _ethSubDelBtn))
            {
                dgv = _ethSubDgv;
            }
            else if (object.ReferenceEquals(btn, _rioPbDelBtn))
            {
                dgv = _rioPbDgv;
            }
            else
            {
                dgv = _rioSubDgv;
            }

            int row = dgv.SelectedRows.Count;
            if (row == 0)
            {
                MessageBox.Show("没有选中任何行", "Error");
                return;
            }
            else if (MessageBox.Show("确认删除选中的" + row.ToString() + "条内容吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int selectcount = dgv.SelectedRows.Count;
                while (selectcount > 0)
                {
                    if (!dgv.SelectedRows[0].IsNewRow)
                        dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index);
                    selectcount--;
                }
            }
        }

        /// <summary>
        /// 保存rio对应的相关数据
        /// </summary>
        /// <param name="dgv">存储数据的控件</param>
        /// <param name="rioSourceList">保存数据的成员变量</param>
        /// <returns>是否保存成功</returns>
        private bool SaveRioSource(DataGridView dgv, List<RioSource> rioSourceList)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var source = new RioSource();
                try
                {
                    source._sn = int.Parse(row.Cells[0].Value.ToString());
                    source._nodeName = row.Cells[1].Value.ToString();
                    source._sourceName = row.Cells[2].Value.ToString();
                    source._packMaxLen = int.Parse(row.Cells[3].Value.ToString());
                    source._bufSize = int.Parse(row.Cells[4].Value.ToString());
                    rioSourceList.Add(source);
                }
                catch(Exception e)
                {
                    MessageBox.Show("SaveRioSource:" + e.Message);
                    return false;
                }
            }
            return true;
        }

        private bool SaveEthSource(DataGridView dgv, List<EthSource> rioSourceList)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var source = new EthSource();
                try
                {
                    source._sn = int.Parse(row.Cells[0].Value.ToString());
                    source._nodeName = row.Cells[1].Value.ToString();
                    source._sourceName = row.Cells[2].Value.ToString();
                    //source._protocol = row.Cells[3].Value.ToString();
                    rioSourceList.Add(source);
                }
                catch(Exception e)
                {
                    MessageBox.Show("SaveEthSource:" + e.Message);
                    return false;
                }
            }
            return true;
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            if (!SaveRioSource(_rioPbDgv, _rioPbSources)) return;
            if (!SaveRioSource(_rioSubDgv, _rioSubSources)) return;
            if (!SaveEthSource(_ethPbDgv, _ethPbSources)) return;
            if (!SaveEthSource(_ethSubDgv, _ethSubSources)) return;
            DialogResult = DialogResult.Yes;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
