using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DRSysCtrlDisplay.Princeple;

namespace DRSysCtrlDisplay
{
    public partial class SystemStruInitForm : InitFormBase
    {
        public int CntNum { get; private set; }//机箱的数量
        private SystemStruInitFormDgvsOpt _dgvsOpt = new SystemStruInitFormDgvsOpt();//系统初始化界面数据表格操作器

        private const string _dgvColumnTitle_serialNum = "序号";
        private const string _dgvColumnTitle_linkType = "连接类型";
        private const string _dgvColumnTitle_end1CntNum = "端1机箱号";
        private const string _dgvColumnTitle_end1PosNum = "端1位置号";
        private const string _dgvColumnTitle_end2CntNum = "端2机箱号";
        private const string _dgvColumnTitle_end2PosNum = "端2位置号";
        private const string _dgvColumnTitle_dataWidth = "带宽";
        private const string _dgvColumnTitle_confirm = "确定";
        private const int _dgvColumnIndex_confirm = 7;

        //控件成员变量
        private TextBox _typeTb;
        private GroupBox groupBox2;
        private DataGridView _consDgv;
        private TextBox _conNumTb;
        private Label label2;
        private Button _cntNumConfirmBtn;
        private Label label1;

        public SystemStruInitForm()
        {
            InitializeComponent();
            Init();
        }

        //designer自动生成代码
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._typeTb = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._consDgv = new System.Windows.Forms.DataGridView();
            this._conNumTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._cntNumConfirmBtn = new System.Windows.Forms.Button();
            this._btnPanel.SuspendLayout();
            this._infoPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._collectionPanel.SuspendLayout();
            this._linkCollectionGb.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._consDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnPanel
            // 
            this._btnPanel.Location = new System.Drawing.Point(0, 459);
            this._btnPanel.Size = new System.Drawing.Size(983, 46);
            // 
            // _cancleBtn
            // 
            this._cancleBtn.Location = new System.Drawing.Point(786, 14);
            // 
            // _yesBtn
            // 
            this._yesBtn.Location = new System.Drawing.Point(133, 14);
            // 
            // _infoPanel
            // 
            this._infoPanel.Size = new System.Drawing.Size(983, 62);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._cntNumConfirmBtn);
            this.groupBox1.Controls.Add(this._conNumTb);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this._typeTb);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Size = new System.Drawing.Size(983, 55);
            // 
            // _collectionPanel
            // 
            this._collectionPanel.Controls.Add(this.groupBox2);
            this._collectionPanel.Location = new System.Drawing.Point(0, 62);
            this._collectionPanel.Size = new System.Drawing.Size(983, 397);
            this._collectionPanel.Controls.SetChildIndex(this.groupBox2, 0);
            this._collectionPanel.Controls.SetChildIndex(this._linkCollectionGb, 0);
            // 
            // _linkCollectionGb
            // 
            this._linkCollectionGb.Location = new System.Drawing.Point(208, 0);
            this._linkCollectionGb.Size = new System.Drawing.Size(775, 397);
            // 
            // tabControl1
            // 
            this.tabControl1.Size = new System.Drawing.Size(688, 377);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(680, 351);
            this.tabPage1.Text = "连接集合";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "系统型号：";
            // 
            // _typeTb
            // 
            this._typeTb.Location = new System.Drawing.Point(77, 16);
            this._typeTb.Name = "_typeTb";
            this._typeTb.Size = new System.Drawing.Size(100, 21);
            this._typeTb.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._consDgv);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 397);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "机箱信息集";
            // 
            // _consDgv
            // 
            this._consDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._consDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this._consDgv.Location = new System.Drawing.Point(3, 17);
            this._consDgv.Name = "_consDgv";
            this._consDgv.RowTemplate.Height = 23;
            this._consDgv.Size = new System.Drawing.Size(202, 377);
            this._consDgv.TabIndex = 10;
            // 
            // _conNumTb
            // 
            this._conNumTb.Location = new System.Drawing.Point(424, 16);
            this._conNumTb.Name = "_conNumTb";
            this._conNumTb.Size = new System.Drawing.Size(68, 21);
            this._conNumTb.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(375, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "机箱数：";
            // 
            // _cntNumConfirmBtn
            // 
            this._cntNumConfirmBtn.Location = new System.Drawing.Point(498, 16);
            this._cntNumConfirmBtn.Name = "_cntNumConfirmBtn";
            this._cntNumConfirmBtn.Size = new System.Drawing.Size(45, 23);
            this._cntNumConfirmBtn.TabIndex = 4;
            this._cntNumConfirmBtn.Text = "确定";
            this._cntNumConfirmBtn.UseVisualStyleBackColor = true;
            this._cntNumConfirmBtn.Click += new System.EventHandler(this._cntNumConfirmBtn_Click);
            // 
            // SystemStruInitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(983, 527);
            this.Name = "SystemStruInitForm";
            this._btnPanel.ResumeLayout(false);
            this._infoPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._collectionPanel.ResumeLayout(false);
            this._linkCollectionGb.ResumeLayout(false);
            this._linkCollectionGb.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._consDgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //初始化
        private void Init()
        {
            SetFatherComponents();
            InitContainersDgv();

            _consDgv.Click += new EventHandler(_consDgv_Clicked);
            base._yesBtn.Click += new EventHandler(_yesBtn_Click);
        }

        protected override void SetFatherComponents()
        {
            this.SuspendLayout();
            base.tabControl1.Controls.Remove(tabPage1);
            base.flowLayoutPanel1.Controls.Remove(base.editBtn);
            this.ResumeLayout();
        }

        //初始化机箱集对应的DataGridview
        private void InitContainersDgv()
        {
            //初始化DataGridView的基本属性
            _consDgv.RowHeadersVisible = false;
            _consDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _consDgv.AllowUserToAddRows = false;
            _consDgv.EditMode = DataGridViewEditMode.EditOnEnter;
            _consDgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //添加列
            _consDgv.ColumnCount = 1;
            _consDgv.Columns[0].Name = "机箱号";

            //添加板卡名
            DataGridViewComboBoxColumn comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = "机箱名";
            comboColumn.DataSource = FuncItemsForm.GetInstance().GetEqSetNames(Princeple.FormType.CONTIANER);
            _consDgv.Columns.Add(comboColumn);
        }

        private void InitLinkDgv(DataGridView linkDgv, object param)
        {
            Int32 ctnId = (Int32)param;
            //初始化DataGridView的基本属性
            linkDgv.Location = new Point(10, 10);
            linkDgv.Size = new Size(10, 10);
            linkDgv.Dock = DockStyle.Fill;
            linkDgv.RowHeadersVisible = false;
            linkDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            linkDgv.AllowUserToAddRows = false;
            linkDgv.EditMode = DataGridViewEditMode.EditOnEnter;
            linkDgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //添加列
            linkDgv.ColumnCount = 4;
            linkDgv.Columns[0].Name = _dgvColumnTitle_serialNum;
            linkDgv.Columns[0].ReadOnly = true;
            linkDgv.Columns[1].Name = _dgvColumnTitle_end1CntNum;
            linkDgv.Columns[1].ReadOnly = true;
            linkDgv.Columns[2].Name = _dgvColumnTitle_end1PosNum;
            linkDgv.Columns[3].Name = _dgvColumnTitle_end2PosNum;

            //在相应位置插入comboboxColumn："端2槽位号"
            var comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = _dgvColumnTitle_end2CntNum;
            comboColumn.DataSource = GetListRangeInt(0, CntNum, new List<int> { ctnId });
            linkDgv.Columns.Insert(3, comboColumn);

            //在相应位置插入comboboxColumn："连接类型"
            comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = _dgvColumnTitle_linkType;
            comboColumn.DataSource = StringConvert.GetLinkType_StringList();
            linkDgv.Columns.Insert(1, comboColumn);

            //在最后添加comboboxColumn："带宽"
            comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = _dgvColumnTitle_dataWidth;
            comboColumn.DataSource = StringConvert.GetLinkLanes_StringList();
            linkDgv.Columns.Add(comboColumn);

            //最后添加一个确定按钮
            var btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = _dgvColumnTitle_confirm;
            linkDgv.Columns.Add(btnColumn);

            //注册事件处理函数
            linkDgv.Click += new EventHandler(LinksDgv_Click);
        }

        #region 事件处理函数


        void _yesBtn_Click(object sender, EventArgs e)
        {
            //可以先检查数据有效性
            if (!CheckDataValid())
            {
                return;
            }

            SystemStruViewModel sys = new SystemStruViewModel(CntNum);
            ReFreshSys(sys);
            sys.SaveXmlByName();
            this.DialogResult = DialogResult.Yes;
        }

        //对连接信息的DataGridView数据的行的增，删
        protected override void addBtn_Click(object sender, EventArgs e)
        {
            int dgvIndex = _consDgv.CurrentRow.Index;
            DataGridViewsOpt.AddRow(_dgvsOpt, _dgvsOpt.DataGridViweList[dgvIndex]);
        }

        protected override void delBtn_Click(object sender, EventArgs e)
        {
            int dgvIndex = _consDgv.CurrentRow.Index;
            DataGridViewsOpt.DelRow(_dgvsOpt, _dgvsOpt.DataGridViweList[dgvIndex]);
        }

        private void _cntNumConfirmBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.SuspendLayout();
                CntNum = int.Parse(_conNumTb.Text);
                ConsDgv_Add(CntNum);
                tabControl_Add(CntNum);

                //默认显示第一个page，与consDgv默认第一行对应
                foreach (TabPage page in base.tabControl1.TabPages)
                {
                    if (base.tabControl1.TabPages.IndexOf(page) != 0)
                    {
                        page.Parent = null;//隐藏
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("_cntNumConfirmBtn_Click" + ex.Message);
                MessageBox.Show("槽位数填入有误！");
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        private void _consDgv_Clicked(object sender, EventArgs e)
        {
            if (_consDgv.CurrentRow != null)
            {
                int choosedCtnNum = _consDgv.CurrentRow.Index;
                //让tabcontrol1显示相应的page
                foreach (TabPage page in base.tabControl1.TabPages)
                {
                    page.Parent = null;//隐藏
                }
                var choosedPage = _dgvsOpt.DataGridViweList[choosedCtnNum].Parent;
                choosedPage.Parent = base.tabControl1;
            }
        }

        void LinksDgv_Click(object sender, EventArgs e)
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

        #endregion 事件处理函数


        //在机箱录入表格添加机箱
        private void ConsDgv_Add(int conNum)
        {
            for (int i = 0; i < conNum; i++)
            {
                int index = _consDgv.Rows.Add();
                _consDgv.Rows[index].Cells[0].Value = i.ToString();//显示的机箱号计数从0开始
                _consDgv.Rows[index].Cells[1].Value = "无";
            }
        }


        //在机箱录入表格添加机箱
        private void tabControl_Add(int conNum)
        {
            for (int i = 0; i < conNum; i++)
            {
                base.AddTabPage<DataGridView>("机箱" + i, _dgvsOpt.DataGridViweList, InitLinkDgv, i);
            }
            base.TSStatus1.Text = "添加槽位成功";
            _cntNumConfirmBtn.Enabled = false;
        }

        //检查数据的有效性；
        //主要是连接集合的数据是否齐全
        private bool CheckDataValid()
        {
            foreach (DataGridView dgv in _dgvsOpt.DataGridViweList)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value == null)
                        {
                            int cntNum = _dgvsOpt.DataGridViweList.IndexOf(dgv);//机箱号
                            MessageBox.Show(string.Format("机箱{0}-第{1}行：信息不全！", cntNum, row.Index));
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //利用界面填入的信息来刷新一个SystemStru
        private void ReFreshSys(SystemStruViewModel sys)
        {
            //刷新基本信息
            sys.Name = _typeTb.Text;
            sys.Type = _typeTb.Text;

            //刷新机箱信息
            foreach (DataGridViewRow row in _consDgv.Rows)
            {
                var cntSn = int.Parse((string)row.Cells[0].Value);  //机箱序号
                var cntName = (string)row.Cells[1].Value;           //机箱的名字
                sys.CntNames[cntSn] = cntName;
            }

            //刷新机箱连接信息
            for (int i = 0; i < _dgvsOpt.DataGridViweList.Count; i++)
            {
                var curDgv = _dgvsOpt.DataGridViweList[i];
                var linkList = new List<SystemStruViewModel.SystemStruLink>();//连接集合
                foreach (DataGridViewRow row in curDgv.Rows)
                {
                    var sysLink = new SystemStruViewModel.SystemStruLink(
                        int.Parse((string)row.Cells[_dgvColumnTitle_end1CntNum].Value),
                        int.Parse((string)row.Cells[_dgvColumnTitle_end1PosNum].Value),
                        int.Parse((string)row.Cells[_dgvColumnTitle_end2CntNum].Value),
                        int.Parse((string)row.Cells[_dgvColumnTitle_end2PosNum].Value),
                        (LinkType)Enum.Parse(typeof(LinkType), (string)row.Cells[_dgvColumnTitle_linkType].Value),
                        (LinkLanes)Enum.Parse(typeof(LinkLanes), (string)row.Cells[_dgvColumnTitle_dataWidth].Value));
                    linkList.Add(sysLink);
                }
                sys.LinksArray[i] = linkList;
            }
        }

        public override string GetObjectName()
        {
            return _typeTb.Text;
        }

        /// <summary>
        /// 用来管理界面的DataGridView及其操作，特别是同步操作
        /// </summary>
        class SystemStruInitFormDgvsOpt : DataGridViewsOpt.IDataGridViewsOpt
        {
            public List<DataGridView> DataGridViweList { get; set; }//数据表格的集合

            public SystemStruInitFormDgvsOpt()
            {
                DataGridViweList = new List<DataGridView>();
                DgvRowsMap = new Dictionary<DataGridViewRow, DataGridViewRow>();
            }
            #region IDataGridViewsOpt接口

            public Dictionary<DataGridViewRow, DataGridViewRow> DgvRowsMap { get; set; }//关联行的字典

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
                if (0 != string.Compare(row.Cells[_dgvColumnTitle_end2CntNum].Value as string
                    , mapedRow.Cells[_dgvColumnTitle_end1CntNum].Value as string))
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
                mapedRow.Cells[_dgvColumnTitle_end2CntNum].Value = row.Cells[_dgvColumnTitle_end1CntNum].Value;
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
                int oppositeDgvId = int.Parse(row.Cells[_dgvColumnTitle_end2CntNum].Value as string);
                return DataGridViweList[oppositeDgvId];
            }

            public void AddRow(DataGridView dgv)
            {
                dgv.Rows.Add();
                int rowSN = dgv.Rows.Count - 1;  //当前行的序列号
                //设置新行的“序号”值、“端1槽位号”值
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_serialNum].Value = rowSN.ToString();
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_end1CntNum].Value = DataGridViweList.IndexOf(dgv).ToString();
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_confirm].Value = _dgvColumnTitle_confirm;
            }

            #endregion
        }

    }


}
