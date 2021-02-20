using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using DRSysCtrlDisplay.Princeple;
using DRSysCtrlDisplay.ViewModel.Others;
using DRSysCtrlDisplay.Models;

namespace DRSysCtrlDisplay
{
    public class BackPlaneInitForm : InitFormBase
    {
        private int _slotNum = 0;
        BackPlaneInitFormDgvsOpt _dgvsOpt = new BackPlaneInitFormDgvsOpt();

        private const string _dgvColumnTitle_serialNum = "序号";
        private const string _dgvColumnTitle_linkType = "连接类型";
        private const string _dgvColumnTitle_end1SlotNum = "端1槽位号";
        private const string _dgvColumnTitle_end1PosNum = "端1位置号";
        private const string _dgvColumnTitle_end2SlotNum = "端2槽位号";
        private const string _dgvColumnTitle_end2PosNum = "端2位置号";
        private const string _dgvColumnTitle_dataWidth = "带宽";
        private const string _dgvColumnTitle_confirm = "确定";
        private const int _dgvColumnIndex_confirm = 7;

        #region Designer生成控件成员变量
        private TextBox _typeTB;
        private Label label1;
        private Button _slotAddBtn;
        private TextBox _slotAddTextB;
        private Label label2;
        #endregion

        public BackPlaneInitForm()
        {
            InitializeComponent();
            SetFatherComponents();

            this._slotAddBtn.Click += new EventHandler(SlotAddBtn_Click);
            this._yesBtn.Click += new System.EventHandler(this.yesBtn_Click);
        }

        /// <summary>
        /// 通过一个BackPlane实例来初始化BackPlaneInitForm，用于修改背板的时候
        /// </summary>
        /// <param name="bp"></param>
        public BackPlaneInitForm(BackPlane bp)
        {
            //TODO
        }

        public override string GetObjectName()
        {
            return new string(_typeTB.Text.ToCharArray());
        }

        //Designer初始化界面代码
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._typeTB = new System.Windows.Forms.TextBox();
            this._slotAddTextB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._slotAddBtn = new System.Windows.Forms.Button();
            this._btnPanel.SuspendLayout();
            this._infoPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._collectionPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _infoPanel
            // 
            this._infoPanel.Size = new System.Drawing.Size(697, 73);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._slotAddBtn);
            this.groupBox1.Controls.Add(this._slotAddTextB);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this._typeTB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Size = new System.Drawing.Size(697, 57);
            // 
            // _collectionPanel
            // 
            this._collectionPanel.Location = new System.Drawing.Point(0, 73);
            this._collectionPanel.Size = new System.Drawing.Size(697, 329);
            // 
            // tabControl1
            // 
            this.tabControl1.Size = new System.Drawing.Size(616, 329);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(608, 303);
            this.tabPage1.Text = "槽位1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "型号：";
            // 
            // _typeTB
            // 
            this._typeTB.Location = new System.Drawing.Point(60, 18);
            this._typeTB.Name = "_typeTB";
            this._typeTB.Size = new System.Drawing.Size(100, 21);
            this._typeTB.TabIndex = 1;
            // 
            // _slotAddTextB
            // 
            this._slotAddTextB.Location = new System.Drawing.Point(241, 18);
            this._slotAddTextB.Name = "_slotAddTextB";
            this._slotAddTextB.Size = new System.Drawing.Size(100, 21);
            this._slotAddTextB.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(188, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "槽位数：";
            // 
            // _slotAddBtn
            // 
            this._slotAddBtn.Location = new System.Drawing.Point(347, 18);
            this._slotAddBtn.Name = "_slotAddBtn";
            this._slotAddBtn.Size = new System.Drawing.Size(42, 23);
            this._slotAddBtn.TabIndex = 4;
            this._slotAddBtn.Text = "确定";
            this._slotAddBtn.UseVisualStyleBackColor = true;
            // 
            // BackPlaneInitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(697, 464);
            this.Name = "BackPlaneInitForm";
            this._btnPanel.ResumeLayout(false);
            this._infoPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._collectionPanel.ResumeLayout(false);
            this._collectionPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region 事件处理函数

        //槽位添加按钮按下响应
        void SlotAddBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int result = Int32.Parse(this._slotAddTextB.Text);
                if (result > 0 && result < 11)
                {
                    _slotNum = result;
                    for (int i = 0; i <= result; i++)
                    {
                        if (i == result)
                        {
                            base.AddTabPage<DataGridView>("槽位" + i + ":外接口", _dgvsOpt.DataGridViweList, InitDataGridView, i);
                        }
                        else
                        {
                            base.AddTabPage<DataGridView>("槽位" + i, _dgvsOpt.DataGridViweList, InitDataGridView, i);
                        }
                    }
                    this.TSStatus1.Text = "添加槽位成功";
                    _slotAddBtn.Enabled = false;
                }
                else
                {
                    this.TSStatus1.Text = "错误：槽位数输入异常";
                }
            }
            catch
            {
                this.TSStatus1.Text = "错误：槽位数输入异常";
            }
        }

        //页面确定按钮按下事件响应
        private void yesBtn_Click(object sender, EventArgs e)
        {
            if (false == CompleteJudgment())
            {
                return;
            }
            BackPlane bp = new BackPlane(_slotNum);
            ReFreshBackPlane(bp);
            bp.SaveXmlByName();
            this.DialogResult = DialogResult.Yes;
        }

        //对DataGridView数据的行的增，删
        protected override void addBtn_Click(object sender, EventArgs e)
        {
            DataGridViewsOpt.AddRow(_dgvsOpt, _dgvsOpt.DataGridViweList[tabControl1.SelectedIndex]);
        }

        protected override void delBtn_Click(object sender, EventArgs e)
        {
            DataGridViewsOpt.DelRow(_dgvsOpt, _dgvsOpt.DataGridViweList[tabControl1.SelectedIndex]);
        }

        void dgv_Click(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            var curCell = dgv.CurrentCell;
            if (curCell == null)
            {
                return;
            }
            if (curCell.ColumnIndex == _dgvColumnIndex_confirm)  //是否是点击了“确定”按钮
            {
                DataGridViewsOpt.Synchronize(_dgvsOpt, dgv.SelectedRows[0]);
            }
        }

        #endregion

        protected override void SetFatherComponents()
        {
            this.SuspendLayout();
            base.tabControl1.Controls.Remove(tabPage1);
            base.flowLayoutPanel1.Controls.Remove(base.editBtn);

            this.ResumeLayout();
        }

        //添加槽位的选项卡
        private void InitDataGridView(DataGridView dgv, object param)
        {
            Int32 slotId = (Int32)param;
            //初始化DataGridView的基本属性
            dgv.Location = new Point(10, 10);
            dgv.Size = new Size(10, 10);
            dgv.Dock = DockStyle.Fill;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //添加列
            dgv.ColumnCount = 4;
            dgv.Columns[0].Name = _dgvColumnTitle_serialNum;
            dgv.Columns[0].ReadOnly = true;
            dgv.Columns[1].Name = _dgvColumnTitle_end1SlotNum;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[2].Name = _dgvColumnTitle_end1PosNum;
            dgv.Columns[3].Name = _dgvColumnTitle_end2PosNum;

            //在相应位置插入comboboxColumn："端2槽位号"
            var comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = _dgvColumnTitle_end2SlotNum;
            comboColumn.DataSource = GetListRangeInt(0, _slotNum + 1, new List<int> { slotId });//包含外接口
            dgv.Columns.Insert(3, comboColumn);

            //在相应位置插入comboboxColumn："连接类型"
            comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = _dgvColumnTitle_linkType;
            comboColumn.DataSource = StringConvert.GetLinkType_StringList();
            dgv.Columns.Insert(1, comboColumn);

            //在最后添加comboboxColumn："带宽"
            comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = _dgvColumnTitle_dataWidth;
            comboColumn.DataSource = StringConvert.GetLinkLanes_StringList();
            dgv.Columns.Add(comboColumn);

            //最后添加一个确定按钮
            var btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = _dgvColumnTitle_confirm;
            dgv.Columns.Add(btnColumn);

            //注册事件处理函数
            dgv.Click += new EventHandler(dgv_Click);
        }

        /* 描述：XML文件写入前内容完整性、正确性校验
         * 参数：无
         * 返回值：
         * false----校验失败
         * true----校验成功，可以写入XML
         */
        private Boolean CompleteJudgment()
        {
            if (String.Empty == _typeTB.Text || String.Empty == _slotAddTextB.Text)
            {
                MessageBox.Show("请输入板卡型号及槽位数！");
                return false;
            }
            else if (tabControl1.TabCount <= 0)
            {
                MessageBox.Show("请添加槽位信息！");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 背板初始化界面初始化一个背板类
        /// </summary>
        /// <param name="bp"></param>
        private void ReFreshBackPlane(BackPlane bp)
        {
            bp.Name = _typeTB.Text;
            bp.Type = _typeTB.Text;
            List<BackPlaneLink> linksList;     //单个槽位对应的Links列表

            for (int i = 0; i < _slotNum; i++)
            {
                //TODO:行信息有效性检查
                //获取一个槽位的信息
                var dgv = _dgvsOpt.DataGridViweList[i];
                linksList = new List<BackPlaneLink>();

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    //获取一条连接的信息
                    string linkTypeString = row.Cells[_dgvColumnTitle_linkType].Value as string;
                    LinkType type = (LinkType)Enum.Parse(typeof(LinkType), linkTypeString);

                    int end1Slot = i;
                    int end1Pos = int.Parse(row.Cells[_dgvColumnTitle_end1PosNum].Value as string);
                    int end2Slot = int.Parse(row.Cells[_dgvColumnTitle_end2SlotNum].Value as string);
                    int end2Pos = int.Parse(row.Cells[_dgvColumnTitle_end2PosNum].Value as string);

                    linksList.Add(new BackPlaneLink(end1Slot, end1Pos, end2Slot, end2Pos, type));
                }
                bp.LinksArray[i] = linksList;
            }
        }

        /// <summary>
        /// 用来管理界面的DataGridView及其操作，特别是同步操作
        /// </summary>
        class BackPlaneInitFormDgvsOpt : DataGridViewsOpt.IDataGridViewsOpt
        {
            public List<DataGridView> DataGridViweList { get; set; }

            public BackPlaneInitFormDgvsOpt()
            {
                DataGridViweList = new List<DataGridView>();
                DgvRowsMap = new Dictionary<DataGridViewRow, DataGridViewRow>();
            }
            #region IDataGridViewsOpt接口

            public Dictionary<DataGridViewRow, DataGridViewRow> DgvRowsMap { get; set; }

            public bool JudgeCellsValue(DataGridViewRow row)
            {
                //判断该row的内容是否全
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                    {
                        return false;
                    }
                }
                return true;
            }

            public bool IsMapedRowChange(DataGridViewRow row, DataGridViewRow mapedRow)
            {
                if (0 != string.Compare(row.Cells[_dgvColumnTitle_end2SlotNum].Value as string
                    , mapedRow.Cells[_dgvColumnTitle_end1SlotNum].Value as string))
                {
                    return true;
                }
                return false;
            }

            public void DeleteRow(DataGridView dgv, int rowId)
            {
                try
                {
                    DataGridViewRow row = dgv.Rows[rowId];
                    dgv.Rows.Remove(row);

                    if (DgvRowsMap.Keys.Contains(row))
                    {
                        DataGridViewRow oppositeRow = DgvRowsMap[row];
                        DgvRowsMap.Remove(row);
                        DgvRowsMap.Remove(oppositeRow);
                    }
                    //刷新其他列的序号
                    foreach (DataGridViewRow dgvRow in dgv.Rows)
                    {
                        dgvRow.Cells[_dgvColumnTitle_serialNum].Value = dgv.Rows.IndexOf(dgvRow);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            public void FreshRow(DataGridViewRow row, DataGridViewRow mapedRow)
            {
                mapedRow.Cells[_dgvColumnTitle_linkType].Value = row.Cells[_dgvColumnTitle_linkType].Value;
                mapedRow.Cells[_dgvColumnTitle_end1PosNum].Value = row.Cells[_dgvColumnTitle_end2PosNum].Value;
                mapedRow.Cells[_dgvColumnTitle_end2SlotNum].Value = row.Cells[_dgvColumnTitle_end1SlotNum].Value;
                mapedRow.Cells[_dgvColumnTitle_end2PosNum].Value = row.Cells[_dgvColumnTitle_end1PosNum].Value;
                mapedRow.Cells[_dgvColumnTitle_dataWidth].Value = row.Cells[_dgvColumnTitle_dataWidth].Value;
            }

            public void AddRow_Fresh(DataGridView dgv, DataGridViewRow srcRow)
            {
                AddRow(dgv);
                var newRow = dgv.Rows[dgv.Rows.Count - 1];
                FreshRow(srcRow, newRow);
                DgvRowsMap.Add(srcRow, newRow);
                DgvRowsMap.Add(newRow, srcRow);
            }

            public DataGridView GetOppositeDgv(DataGridViewRow row)
            {
                int oppositeDgvId = int.Parse(row.Cells[_dgvColumnTitle_end2SlotNum].Value as string);
                return DataGridViweList[oppositeDgvId];
            }

            public void AddRow(DataGridView dgv)
            {
                dgv.Rows.Add();
                int rowSN = dgv.Rows.Count - 1;  //当前行的序列号
                //设置新行的“序号”值、“端1槽位号”值
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_serialNum].Value = rowSN.ToString();
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_end1SlotNum].Value = DataGridViweList.IndexOf(dgv).ToString();
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_confirm].Value = _dgvColumnTitle_confirm;
            }

            #endregion
        }
    }
}
