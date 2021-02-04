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
    public class ComponentInitForm : InitFormBase
    {
        ComponentInitFormDgvsOpt _dgvsOpt = new ComponentInitFormDgvsOpt();
        Node[] _nodeArray;//构建所包含的计算颗粒Node的集合

        private const string _nodeName = "计算颗粒";
        private const string _dgvColumnTitle_serialNum = "序号";
        private const string _dgvColumnTitle_linkType = "连接类型";
        private const string _dgvColumnTitle_end1CmpNum = "端1构件号";
        private const string _dgvColumnTitle_end2CmpNum = "端2构件号";
        private const string _dgvColumnTitle_dataWidth = "带宽";
        private const string _dgvColumnTitle_confirm = "确定";
        private const int _dgvColumnIndex_confirm = 5;

        //Designer生成的相关控件
        private TextBox _typeTB;
        private TextBox _nodesNumTb;
        private Label label3;
        private Button _nodesNumBtn;
        private GroupBox groupBox2;
        private Button _sourceBtn;
        private Button _delNodeBtn;
        private Button _showNodeBtn;
        private Button _editNodeBtn;
        private Button _addNodeBtn;
        private Label label2;
        private ComboBox _nodeTypeCB;
        private Label label1;

        public ComponentInitForm()
        {
            SetFatherComponents();
            InitializeComponent();
            LocalInitialize();
        }

        public override string GetObjectName()
        {
            return _typeTB.Text;
        }

        //Designer生成代码
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._typeTB = new System.Windows.Forms.TextBox();
            this._nodesNumTb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._nodesNumBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._sourceBtn = new System.Windows.Forms.Button();
            this._delNodeBtn = new System.Windows.Forms.Button();
            this._showNodeBtn = new System.Windows.Forms.Button();
            this._editNodeBtn = new System.Windows.Forms.Button();
            this._addNodeBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._nodeTypeCB = new System.Windows.Forms.ComboBox();
            this._btnPanel.SuspendLayout();
            this._infoPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._collectionPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnPanel
            // 
            this._btnPanel.Controls.Add(this.groupBox2);
            this._btnPanel.Location = new System.Drawing.Point(0, 343);
            this._btnPanel.Size = new System.Drawing.Size(713, 99);
            this._btnPanel.Controls.SetChildIndex(this._cancleBtn, 0);
            this._btnPanel.Controls.SetChildIndex(this._yesBtn, 0);
            this._btnPanel.Controls.SetChildIndex(this.groupBox2, 0);
            // 
            // _cancleBtn
            // 
            this._cancleBtn.Location = new System.Drawing.Point(468, 63);
            // 
            // _yesBtn
            // 
            this._yesBtn.Location = new System.Drawing.Point(156, 63);
            // 
            // _infoPanel
            // 
            this._infoPanel.Size = new System.Drawing.Size(713, 55);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._nodesNumBtn);
            this.groupBox1.Controls.Add(this._nodesNumTb);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this._typeTB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Size = new System.Drawing.Size(713, 48);
            // 
            // _collectionPanel
            // 
            this._collectionPanel.Location = new System.Drawing.Point(0, 55);
            this._collectionPanel.Size = new System.Drawing.Size(713, 288);
            // 
            // tabControl1
            // 
            this.tabControl1.Size = new System.Drawing.Size(632, 288);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(624, 262);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "构件型号";
            // 
            // _typeTB
            // 
            this._typeTB.Location = new System.Drawing.Point(73, 17);
            this._typeTB.Name = "_typeTB";
            this._typeTB.Size = new System.Drawing.Size(90, 21);
            this._typeTB.TabIndex = 1;
            // 
            // _nodesNumTb
            // 
            this._nodesNumTb.Location = new System.Drawing.Point(365, 17);
            this._nodesNumTb.Name = "_nodesNumTb";
            this._nodesNumTb.Size = new System.Drawing.Size(90, 21);
            this._nodesNumTb.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(282, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "计算颗粒总数";
            // 
            // _nodesNumBtn
            // 
            this._nodesNumBtn.Location = new System.Drawing.Point(461, 15);
            this._nodesNumBtn.Name = "_nodesNumBtn";
            this._nodesNumBtn.Size = new System.Drawing.Size(37, 23);
            this._nodesNumBtn.TabIndex = 6;
            this._nodesNumBtn.Text = "添加";
            this._nodesNumBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._sourceBtn);
            this.groupBox2.Controls.Add(this._delNodeBtn);
            this.groupBox2.Controls.Add(this._showNodeBtn);
            this.groupBox2.Controls.Add(this._editNodeBtn);
            this.groupBox2.Controls.Add(this._addNodeBtn);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this._nodeTypeCB);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(713, 50);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "计算颗粒属性";
            // 
            // _sourceBtn
            // 
            this._sourceBtn.Location = new System.Drawing.Point(387, 19);
            this._sourceBtn.Name = "_sourceBtn";
            this._sourceBtn.Size = new System.Drawing.Size(75, 23);
            this._sourceBtn.TabIndex = 7;
            this._sourceBtn.Text = "资源定义";
            this._sourceBtn.UseVisualStyleBackColor = true;
            // 
            // _delNodeBtn
            // 
            this._delNodeBtn.Location = new System.Drawing.Point(247, 20);
            this._delNodeBtn.Name = "_delNodeBtn";
            this._delNodeBtn.Size = new System.Drawing.Size(69, 23);
            this._delNodeBtn.TabIndex = 6;
            this._delNodeBtn.Text = "删除";
            this._delNodeBtn.UseVisualStyleBackColor = true;
            // 
            // _showNodeBtn
            // 
            this._showNodeBtn.Location = new System.Drawing.Point(560, 19);
            this._showNodeBtn.Name = "_showNodeBtn";
            this._showNodeBtn.Size = new System.Drawing.Size(74, 23);
            this._showNodeBtn.TabIndex = 5;
            this._showNodeBtn.Text = "显示信息";
            this._showNodeBtn.UseVisualStyleBackColor = true;
            // 
            // _editNodeBtn
            // 
            this._editNodeBtn.Location = new System.Drawing.Point(468, 19);
            this._editNodeBtn.Name = "_editNodeBtn";
            this._editNodeBtn.Size = new System.Drawing.Size(86, 23);
            this._editNodeBtn.TabIndex = 3;
            this._editNodeBtn.Text = "修改";
            this._editNodeBtn.UseVisualStyleBackColor = true;
            // 
            // _addNodeBtn
            // 
            this._addNodeBtn.Location = new System.Drawing.Point(172, 20);
            this._addNodeBtn.Name = "_addNodeBtn";
            this._addNodeBtn.Size = new System.Drawing.Size(69, 23);
            this._addNodeBtn.TabIndex = 2;
            this._addNodeBtn.Text = "添加";
            this._addNodeBtn.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "型号";
            // 
            // _nodeTypeCB
            // 
            this._nodeTypeCB.FormattingEnabled = true;
            this._nodeTypeCB.Location = new System.Drawing.Point(73, 20);
            this._nodeTypeCB.Name = "_nodeTypeCB";
            this._nodeTypeCB.Size = new System.Drawing.Size(90, 20);
            this._nodeTypeCB.TabIndex = 0;
            // 
            // ComponentInitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(713, 464);
            this.Name = "ComponentInitForm";
            this._btnPanel.ResumeLayout(false);
            this._infoPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._collectionPanel.ResumeLayout(false);
            this._collectionPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void SetFatherComponents()
        {
            this.Text = "ComponentInitForm";
            /*清某些不必要的显示*/
            this.SuspendLayout();
            base.tabControl1.Controls.Remove(base.tabPage1);
            this.ResumeLayout();
        }

        private void LocalInitialize()
        {
            this.SuspendLayout();

            _nodeTypeCB.DataSource = StringConvert.GetComputeNodeType_StringList();

            //绑定事件处理函数
            _addNodeBtn.Click += new EventHandler(_addNodeBtn_Click);
            _editNodeBtn.Click += new EventHandler(_editNodeBtn_Click);
            _showNodeBtn.Click += new EventHandler(_showNodeBtn_Click);
            _nodesNumBtn.Click += new EventHandler(_nodesNumBtn_Click);
            _sourceBtn.Click += new System.EventHandler(this._sourceBtn_Click);
            base._yesBtn.Click += new EventHandler(_yesBtn_Click);
            base.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            this.ResumeLayout();
        }

        #region 界面事件处理函数

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _nodeTypeCB.Text = "未选择";
        }

        //添加计算颗粒节点
        void _addNodeBtn_Click(object sender, EventArgs e)
        {
            int nodeNum = tabControl1.SelectedIndex;//计算颗粒号
            string nodeName = _nodeName + nodeNum.ToString();
            //构件初始化
            try
            {
                var nodeType = (ComputeNodeType)Enum.Parse(typeof(ComputeNodeType), _nodeTypeCB.Text);
                var node = new Node(nodeType, nodeName);    //弹出初始化界面
                if (node._object == null)
                {
                    return;
                }
                _nodeArray[nodeNum] = node;
                FreshNodeStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void _editNodeBtn_Click(object sender, EventArgs e)
        {
            int nodeNum = tabControl1.SelectedIndex;//计算颗粒号,因为tabPage与node在各自的集合里面下标是一一对应的；
            var node = _nodeArray[nodeNum];
            node.EditNodeInfo();
        }


        void _showNodeBtn_Click(object sender, EventArgs e)
        {
            int nodeNum = tabControl1.SelectedIndex;
            var node = _nodeArray[nodeNum];
            node.ShowNodeInfo();
        }

        void _nodesNumBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int cmpNum = int.Parse(_nodesNumTb.Text);
                _nodeArray = new Node[cmpNum];
                for (int i = 0; i < cmpNum; i++)
                {
                    //添加对应的TabPage
                    base.AddTabPage<DataGridView>(_nodeName + i.ToString(), _dgvsOpt.DataGridViweList, InitDataGridView, i);
                }
                _nodesNumBtn.Enabled = false;
                tabControl1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ComponentInitForm._nodesNumBtn_Click:" + ex.Message);
            }
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FreshNodeStatus();
        }

        protected override void addBtn_Click(object sender, EventArgs e)
        {
            DataGridViewsOpt.AddRow(_dgvsOpt, _dgvsOpt.DataGridViweList[tabControl1.SelectedIndex]);
        }

        protected override void delBtn_Click(object sender, EventArgs e)
        {
            DataGridViewsOpt.DelRow(_dgvsOpt, _dgvsOpt.DataGridViweList[tabControl1.SelectedIndex]);
        }

        //资源定义按钮的响应事件
        private void _sourceBtn_Click(object sender, EventArgs e)
        {
            int nodeNum = tabControl1.SelectedIndex;//计算颗粒号
            Node selectedNode = this._nodeArray[nodeNum];
            if (selectedNode == null)
            {
                return;
            }
            //创建资源定义窗体
            var sourceForm = new ComponentInitForm_AddSource(selectedNode.Name);
            sourceForm.StartPosition = FormStartPosition.CenterParent;
            sourceForm.ShowDialog();

            if (sourceForm.DialogResult == DialogResult.Yes)
            {
                selectedNode._ethPbSources = sourceForm._ethPbSources;
                selectedNode._ethSubSources = sourceForm._ethSubSources;
                selectedNode._rioPbSources = sourceForm._rioPbSources;
                selectedNode._rioSubSources = sourceForm._rioSubSources;
            }
            sourceForm.Dispose();
        }

        void _yesBtn_Click(object sender, EventArgs e)
        {
            //检查信息准确性及完整性
            //始化构件实体并记录到XML文件;
            var retComponent = new ComponentViewModel();
            retComponent.InitTopo(_nodeArray.Length);
            retComponent.Name = _typeTB.Text;

            for (int i = 0; i < retComponent.NodeNum; i++)
            {
                if (_nodeArray[i] != null)
                {
                    //添加节点信息
                    Node curNode = _nodeArray[i];
                    var nodeType = (EndType)curNode._nodeType;
                    var nodeName = curNode.Name;
                    var nodeObj = curNode._object;
                    retComponent.CmpTopoNet.SetNodeValue(i, new ComponentViewModel.ComponentNode(nodeType, nodeName, i, nodeObj));
                    //添加连接信息
                    var dgv = _dgvsOpt.DataGridViweList[i];
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        var linkType = (LinkType)Enum.Parse(typeof(LinkType), row.Cells[_dgvColumnTitle_linkType].Value.ToString());
                        int endId1 = int.Parse(row.Cells[_dgvColumnTitle_end1CmpNum].Value.ToString());
                        int endId2 = int.Parse(row.Cells[_dgvColumnTitle_end2CmpNum].Value.ToString());
                        var linkLines = (LinkLanes)Enum.Parse(typeof(LinkLanes), row.Cells[_dgvColumnTitle_dataWidth].Value.ToString());

                        var curLine = new ComponentViewModel.ComponentLine(linkType, endId1, endId2, linkLines);
                        retComponent.CmpTopoNet.SetLinkValue(curLine);
                    }
                }
            }
            retComponent.SaveXmlByName();
            GenerateSourceFile();
            this.DialogResult = DialogResult.Yes;
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

        /// <summary>
        /// 形成资源文件
        /// </summary>
        private void GenerateSourceFile()
        {
            string cmpName = this._typeTB.Text;     //构件名称
            MainForm.SetOutPutText(string.Format("生成构件({0})相关节点资源文件!", cmpName));
            try
            {
                foreach (var node in _nodeArray)
                {
                    //EtherNet资源
                    if (node._ethPbSources.Count > 0 || node._ethSubSources.Count > 0)
                    {
                        MHalCodeGen egen = new MHalCodeGen(string.Format("{0}_{1}_Emhal", cmpName, node.Name));
                        foreach (var eSource in node._ethPbSources)
                        {
                            egen.AddEMHalPublishRes(eSource._sourceName);
                        }
                        foreach (var eSource in node._ethSubSources)
                        {
                            egen.AddEMHalSubscribeRes(eSource._sourceName);
                        }
                        egen.GenEMHalCode();
                        MainForm.SetOutPutText(string.Format("生成资源文件：{0}_{1}_Emhal", cmpName, node.Name));
                    }

                    //RapidIO资源
                    if (node._rioPbSources.Count > 0 || node._rioSubSources.Count > 0)
                    {
                        MHalCodeGen rgen = new MHalCodeGen(string.Format("{0}_{1}_Rmhal", cmpName, node.Name));
                        foreach (var rSource in node._rioPbSources)
                        {
                            rgen.AddRMHalPublishRes(rSource._sourceName, (uint)rSource._packMaxLen, (uint)rSource._bufSize);
                        }
                        foreach (var rSource in node._rioSubSources)
                        {
                            rgen.AddRMHalSubscribeRes(rSource._sourceName);
                        }
                        rgen.GenRMHalPpcCode(false);
                        MainForm.SetOutPutText(string.Format("生成资源文件：{0}_{1}_Rmhal", cmpName, node.Name));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("GenerateSourceFile" + e.Message);
            }
        }


        //通过选择不同的计算颗粒，刷新每个计算颗粒的状态
        private void FreshNodeStatus()
        {
            int nodeNum = tabControl1.SelectedIndex;//计算颗粒号
            var node = _nodeArray[nodeNum];
            if (node == null)
            {
                _nodeTypeCB.Enabled = true;
                _nodeTypeCB.Text = "未选择";
                _addNodeBtn.Enabled = true;
                _delNodeBtn.Enabled = false;
            }
            else
            {
                _nodeTypeCB.Enabled = false;
                _nodeTypeCB.Text = node._nodeType.ToString();
                _addNodeBtn.Enabled = false;
                _delNodeBtn.Enabled = true;
            }
        }

        /// <summary>
        /// 初始化计算颗粒的DataGridView
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="param">当前计算颗粒的序号(int)</param>
        private void InitDataGridView(DataGridView dgv, object param)
        {
            Int32 nodeNum = (Int32)param;
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
            dgv.ColumnCount = 2;
            dgv.Columns[0].Name = _dgvColumnTitle_serialNum;
            dgv.Columns[0].ReadOnly = true;
            dgv.Columns[1].Name = _dgvColumnTitle_end1CmpNum;
            dgv.Columns[1].ReadOnly = true;

            //在相应位置插入comboboxColumn："端2构件号"
            var comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = _dgvColumnTitle_end2CmpNum;
            comboColumn.DataSource = GetListRangeInt(0, _nodeArray.Length, new List<int> { nodeNum });
            dgv.Columns.Add(comboColumn);

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
            base.tabControl1.SelectedIndex = nodeNum;

            //最后添加一个确定按钮
            var btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = _dgvColumnTitle_confirm;
            dgv.Columns.Add(btnColumn);

            dgv.Click += new EventHandler(dgv_Click);
        }

        #region 相关子类
        /// <summary>
        /// 计算颗粒节点定义
        /// </summary>
        class Node
        {
            public ComputeNodeType _nodeType;               //节点对应的芯片类型
            public BaseViewCore _object = null;             //对应的芯片实例
            public string Name { get; private set; }        //节点对应芯片名字
            public List<EthSource> _ethPbSources = new List<EthSource>();  //以太网发布的资源
            public List<EthSource> _ethSubSources = new List<EthSource>();  //以太网订阅的资源
            public List<RioSource> _rioPbSources = new List<RioSource>();  //rio网订阅的资源
            public List<RioSource> _rioSubSources = new List<RioSource>();  //rio网订阅的资源

            public Node(ComputeNodeType nodeType, string nodeName)
            {
                _nodeType = nodeType;
                Name = nodeName;
                //初始界面
                ShowInitialForm();
            }

            private void ShowInitialForm()
            {
                switch (_nodeType)
                {
                    case ComputeNodeType.PPC:
                        Component_PPCInitForm ppcForm = new Component_PPCInitForm(Name);
                        ppcForm.ShowDialog();
                        if (ppcForm.DialogResult == DialogResult.Yes)
                        {
                            _object = ppcForm._ppc;
                        }
                        break;
                    case ComputeNodeType.FPGA:
                        Component_FPGAInitForm fpgaForm = new Component_FPGAInitForm(Name);
                        fpgaForm.ShowDialog();
                        if (fpgaForm.DialogResult == DialogResult.Yes)
                        {
                            _object = fpgaForm._fpga;
                        }
                        break;
                    default://ComputeNodeType.ZYNQ
                        Component_ZYNQInitForm zynqForm = new Component_ZYNQInitForm(Name);
                        zynqForm.ShowDialog();
                        if (zynqForm.DialogResult == DialogResult.Yes)
                        {
                            _object = zynqForm._zynq;
                        }
                        break;
                }
                //改名字
                this.Name = _object.Name;
            }

            public void ShowNodeInfo()
            {
                //TODO
            }

            public void EditNodeInfo()
            {
                //TODO
            }
        }

        class Component_PPCInitForm : PPCInitForm
        {
            public PPCViewModel _ppc = new PPCViewModel();
            //初始化结束
            public Component_PPCInitForm(string name)
            {
                base.Text = name;
            }

            protected override void _yesBtn_Click(object sender, EventArgs e)
            {
                base.RefreshPPC(_ppc);
                this.DialogResult = DialogResult.Yes;
            }
        }

        class Component_FPGAInitForm : FPGAInitForm
        {
            public FPGAViewModel _fpga = new FPGAViewModel();
            public Component_FPGAInitForm(string name)
            {
                base.Text = name;
            }

            protected override void yesBtn_Click(object sender, EventArgs e)
            {
                base.RefreshFPGA(_fpga);
                this.DialogResult = DialogResult.Yes;
            }
        }

        class Component_ZYNQInitForm : ZYNQInitForm
        {
            public ZYNQViewModel _zynq = new ZYNQViewModel();
            public Component_ZYNQInitForm(string name)
            {
                base.Text = name;
            }

            protected override void YesButton_Clicked(object sender, EventArgs e)
            {
                base.RefreshZYNQ(_zynq);
                this.DialogResult = DialogResult.Yes;
            }
        }

        /// <summary>
        /// 用来管理界面的DataGridView及其操作，特别是同步操作
        /// </summary>
        class ComponentInitFormDgvsOpt : DataGridViewsOpt.IDataGridViewsOpt
        {
            public List<DataGridView> DataGridViweList { get; set; }

            public ComponentInitFormDgvsOpt()
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
                if (0 != string.Compare(row.Cells[_dgvColumnTitle_end2CmpNum].Value as string
                    , mapedRow.Cells[_dgvColumnTitle_end1CmpNum].Value as string))
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
                mapedRow.Cells[_dgvColumnTitle_end2CmpNum].Value = row.Cells[_dgvColumnTitle_end1CmpNum].Value;
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
                int oppositeDgvId = int.Parse(row.Cells[_dgvColumnTitle_end2CmpNum].Value as string);
                return DataGridViweList[oppositeDgvId];
            }

            public void AddRow(DataGridView dgv)
            {
                dgv.Rows.Add();
                int rowSN = dgv.Rows.Count - 1;  //当前行的序列号
                //设置新行的“序号”值、“端1构件号”值
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_serialNum].Value = rowSN.ToString();
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_end1CmpNum].Value = DataGridViweList.IndexOf(dgv).ToString();
                dgv.Rows[rowSN].Cells[_dgvColumnTitle_confirm].Value = _dgvColumnTitle_confirm;
            }

            #endregion
        }
        #endregion

    }
}
