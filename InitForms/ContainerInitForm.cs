using DRSysCtrlDisplay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// 背板初始化界面类
    /// </summary>
    public class ContainerInitForm : InitFormBase
    {
        private Label label2;
        private TextBox _typeTB;
        private Button _addBtn;
        private ComboBox _bpTypeCB;
        private DataGridView dataGridView1;
        private Label label1;

        public ContainerInitForm()
        {
            InitializeComponent();
            SetFatherComponents();
            ListDataGridView();
            BpTypeComboBoxInit();
            base._yesBtn.Click += new EventHandler((s, e) =>
            {
                var ctn = new Models.Container();
                RefreshContainer(ctn);
                ctn.SaveXmlByName();
                this.DialogResult = DialogResult.Yes;
            });
        }
        /// <summary>
        /// 通过一个Container实例来初始化ContainerInitForm，用于修改机箱的时候
        /// </summary>
        /// <param name="ctn"></param>
        public ContainerInitForm(ContainerViewModel ctn)
        {
            //TODO
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._typeTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._addBtn = new System.Windows.Forms.Button();
            this._bpTypeCB = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this._btnPanel.SuspendLayout();
            this._infoPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._collectionPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._bpTypeCB);
            this.groupBox1.Controls.Add(this._addBtn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this._typeTB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Size = new System.Drawing.Size(697, 57);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Text = "槽位信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "类型：";
            // 
            // _typeTB
            // 
            this._typeTB.Location = new System.Drawing.Point(60, 18);
            this._typeTB.Name = "_typeTB";
            this._typeTB.Size = new System.Drawing.Size(100, 21);
            this._typeTB.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "背板型号：";
            // 
            // _addBtn
            // 
            this._addBtn.Location = new System.Drawing.Point(377, 17);
            this._addBtn.Name = "_addBtn";
            this._addBtn.Size = new System.Drawing.Size(41, 23);
            this._addBtn.TabIndex = 4;
            this._addBtn.Text = "添加";
            this._addBtn.UseVisualStyleBackColor = true;
            this._addBtn.Click += new System.EventHandler(this._addBtn_Click);
            // 
            // _bpTypeCB
            // 
            this._bpTypeCB.FormattingEnabled = true;
            this._bpTypeCB.Location = new System.Drawing.Point(268, 18);
            this._bpTypeCB.Name = "_bpTypeCB";
            this._bpTypeCB.Size = new System.Drawing.Size(94, 20);
            this._bpTypeCB.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(602, 307);
            this.dataGridView1.TabIndex = 0;
            // 
            // ContainerInitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 464);
            this.Name = "ContainerInitForm";
            this.Text = "ContainerInitForm";
            this._btnPanel.ResumeLayout(false);
            this._infoPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._collectionPanel.ResumeLayout(false);
            this._collectionPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void SetFatherComponents()
        {
            this.SuspendLayout();
            base.flowLayoutPanel1.Controls.Remove(base.editBtn);
            base.flowLayoutPanel1.Controls.Remove(base.addBtn);
            base.flowLayoutPanel1.Controls.Remove(base.delBtn);

            this.ResumeLayout();
        }

        private void ListDataGridView()
        {
            //初始化DataGridView的基本属性
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;

            //添加列
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Name = "槽位号";

            //添加板卡名
            DataGridViewComboBoxColumn comboColumn = new DataGridViewComboBoxColumn();
            comboColumn.Name = "板卡名";
            comboColumn.DataSource = FuncItemsForm.GetInstance().GetEqSetNames(Princeple.FormType.BOARD);
            dataGridView1.Columns.Add(comboColumn);
        }

        private void BpTypeComboBoxInit()
        {
            var names = FuncItemsForm.GetInstance().GetEqSetNames(Princeple.FormType.BACKPLANE);
            _bpTypeCB.Items.AddRange(names);
        }

        private void _addBtn_Click(object sender, EventArgs e)
        {
            BackPlane bp = ModelFactory<BackPlane>.CreateByName(_bpTypeCB.Text);
            for (int i = 0; i < bp.SlotsNum; i++)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = i.ToString();
                dataGridView1.Rows[index].Cells[1].Value = "无";
            }
        }

        //把初始化界面填入的内容赋值到一个Container的对象上面去；
        private void RefreshContainer(Models.Container ctn)
        {
            ctn.Name = this._typeTB.Text;
            ctn.Type = this._typeTB.Text;
            ctn.BackPlaneName = this._bpTypeCB.Text;

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                int slotNum = int.Parse(item.Cells[0].Value.ToString());
                string boardName = item.Cells[1].Value.ToString();
                ctn.BoardNameDir.Add(slotNum, boardName);
            }
        }

        public override string GetObjectName()
        {
            return new string(_typeTB.Text.ToCharArray());
        }
    }
}
