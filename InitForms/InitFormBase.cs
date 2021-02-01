using System;
using System.Reflection;
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
    public class InitFormBase : Form
    {
        //定义一个泛型委托,来初始化元素T(T一般为：DataGridview/ListView)
        protected delegate void InitElement<T>(T element, object param);

        protected StatusStrip statusStrip1;
        protected Panel _btnPanel;
        protected Button _cancleBtn;
        protected Button _yesBtn;
        protected Panel _infoPanel;
        protected GroupBox groupBox1;
        protected Panel _collectionPanel;
        protected GroupBox _linkCollectionGb;
        protected TabControl tabControl1;
        protected TabPage tabPage1;
        protected FlowLayoutPanel flowLayoutPanel1;
        protected Button addBtn;
        protected Button editBtn;
        protected Button delBtn;
        protected ToolStripStatusLabel TSStatus1;


        public InitFormBase()
        {
            InitializeComponent();
            //设置窗体显示位置
            this.StartPosition = FormStartPosition.CenterParent;

            this.addBtn.Click += new EventHandler(addBtn_Click);
            this.delBtn.Click += new EventHandler(delBtn_Click);
            this._cancleBtn.Click += new EventHandler(_cancleBtn_Click);
        }

        /// <summary>
        /// 获取初始化对象的名字
        /// </summary>
        /// <returns></returns>
        public virtual string GetObjectName()
        {
            return string.Empty;
        }

        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TSStatus1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._btnPanel = new System.Windows.Forms.Panel();
            this._cancleBtn = new System.Windows.Forms.Button();
            this._yesBtn = new System.Windows.Forms.Button();
            this._infoPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._collectionPanel = new System.Windows.Forms.Panel();
            this._linkCollectionGb = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addBtn = new System.Windows.Forms.Button();
            this.editBtn = new System.Windows.Forms.Button();
            this.delBtn = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this._btnPanel.SuspendLayout();
            this._infoPanel.SuspendLayout();
            this._collectionPanel.SuspendLayout();
            this._linkCollectionGb.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSStatus1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(697, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TSStatus1
            // 
            this.TSStatus1.Name = "TSStatus1";
            this.TSStatus1.Size = new System.Drawing.Size(66, 17);
            this.TSStatus1.Text = "Welcome!";
            // 
            // _btnPanel
            // 
            this._btnPanel.Controls.Add(this._cancleBtn);
            this._btnPanel.Controls.Add(this._yesBtn);
            this._btnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._btnPanel.Location = new System.Drawing.Point(0, 402);
            this._btnPanel.Name = "_btnPanel";
            this._btnPanel.Size = new System.Drawing.Size(697, 40);
            this._btnPanel.TabIndex = 2;
            // 
            // _cancleBtn
            // 
            this._cancleBtn.Location = new System.Drawing.Point(458, 14);
            this._cancleBtn.Name = "_cancleBtn";
            this._cancleBtn.Size = new System.Drawing.Size(75, 23);
            this._cancleBtn.TabIndex = 1;
            this._cancleBtn.Text = "取消";
            this._cancleBtn.UseVisualStyleBackColor = true;
            // 
            // _yesBtn
            // 
            this._yesBtn.Location = new System.Drawing.Point(166, 14);
            this._yesBtn.Name = "_yesBtn";
            this._yesBtn.Size = new System.Drawing.Size(75, 23);
            this._yesBtn.TabIndex = 0;
            this._yesBtn.Text = "确定";
            this._yesBtn.UseVisualStyleBackColor = true;
            // 
            // _infoPanel
            // 
            this._infoPanel.Controls.Add(this.groupBox1);
            this._infoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._infoPanel.Location = new System.Drawing.Point(0, 0);
            this._infoPanel.Name = "_infoPanel";
            this._infoPanel.Size = new System.Drawing.Size(697, 63);
            this._infoPanel.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(697, 36);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本信息";
            // 
            // _collectionPanel
            // 
            this._collectionPanel.Controls.Add(this._linkCollectionGb);
            this._collectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._collectionPanel.Location = new System.Drawing.Point(0, 63);
            this._collectionPanel.Name = "_collectionPanel";
            this._collectionPanel.Size = new System.Drawing.Size(697, 339);
            this._collectionPanel.TabIndex = 6;
            // 
            // groupBox2
            // 
            this._linkCollectionGb.Controls.Add(this.tabControl1);
            this._linkCollectionGb.Controls.Add(this.flowLayoutPanel1);
            this._linkCollectionGb.Dock = System.Windows.Forms.DockStyle.Fill;
            this._linkCollectionGb.Location = new System.Drawing.Point(0, 0);
            this._linkCollectionGb.Name = "groupBox2";
            this._linkCollectionGb.Size = new System.Drawing.Size(697, 339);
            this._linkCollectionGb.TabIndex = 0;
            this._linkCollectionGb.TabStop = false;
            this._linkCollectionGb.Text = "连接信息";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(610, 319);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(602, 293);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.addBtn);
            this.flowLayoutPanel1.Controls.Add(this.editBtn);
            this.flowLayoutPanel1.Controls.Add(this.delBtn);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(613, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 80, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(81, 319);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(3, 83);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 0;
            this.addBtn.Text = "添加";
            this.addBtn.UseVisualStyleBackColor = true;
            // 
            // editBtn
            // 
            this.editBtn.Location = new System.Drawing.Point(3, 112);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(75, 23);
            this.editBtn.TabIndex = 3;
            this.editBtn.Text = "编辑";
            this.editBtn.UseVisualStyleBackColor = true;
            // 
            // delBtn
            // 
            this.delBtn.Location = new System.Drawing.Point(3, 141);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(75, 23);
            this.delBtn.TabIndex = 1;
            this.delBtn.Text = "删除";
            this.delBtn.UseVisualStyleBackColor = true;
            // 
            // InitFormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 464);
            this.Controls.Add(this._collectionPanel);
            this.Controls.Add(this._infoPanel);
            this.Controls.Add(this._btnPanel);
            this.Controls.Add(this.statusStrip1);
            this.Name = "InitFormBase";
            this.Text = "InitFormBase";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this._btnPanel.ResumeLayout(false);
            this._infoPanel.ResumeLayout(false);
            this._collectionPanel.ResumeLayout(false);
            this._linkCollectionGb.ResumeLayout(false);
            this._linkCollectionGb.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region 事件处理函数

        protected virtual void addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Control.ControlCollection controls = this.tabControl1.SelectedTab.Controls;
                Control view = controls[0];
                Type typeInfo = view.GetType();
                if (typeInfo == typeof(ListView))
                {
                    ListView choosedLv = view as ListView;
                    choosedLv.Items.Add("New Item");
                }
                else if (typeInfo == typeof(DataGridView))
                {
                    DataGridView choosedDgv = view as DataGridView;
                    choosedDgv.Rows.Add();
                }
                else { }

            }
            catch (NullReferenceException NRException)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + ":" + NRException.Message);
            }
        }

        protected virtual void delBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Control.ControlCollection controls = this.tabControl1.SelectedTab.Controls;
                Control view = controls[0];
                Type typeInfo = view.GetType();
                if (typeInfo == typeof(ListView))
                {
                    ListView choosedLv = view as ListView;
                    var choosedItem = choosedLv.SelectedItems[0];
                    choosedLv.Items.Remove(choosedItem);
                }
                else if (typeInfo == typeof(DataGridView))
                {
                    DataGridView choosedDgv = view as DataGridView;
                    var choosedRow = choosedDgv.SelectedRows[0];
                    choosedDgv.Rows.Remove(choosedRow);
                }
                else { }

            }
            catch (NullReferenceException NRException)
            {
                MessageBox.Show(NRException.Message);
            }
        }

        void _cancleBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        //private void AddOneListView(ListView choosedLv)
        //{
        //    //添加ListView的Item
        //    choosedLv.SuspendLayout();
        //    ListViewItem item = new ListViewItem(choosedLv.Items.Count.ToString());
        //    SlotsInitForm_AddSlot addSlotForm = new SlotsInitForm_AddSlot();
        //    addSlotForm.StartPosition = FormStartPosition.CenterScreen;
        //    addSlotForm.ShowDialog();
        //    if (addSlotForm.DialogResult == DialogResult.Yes)
        //    {
        //        item.SubItems.Add(addSlotForm.LocalLocation);
        //        item.SubItems.Add(addSlotForm.OppositeNum);
        //        item.SubItems.Add(addSlotForm.OppositeLoction);
        //        item.SubItems.Add(addSlotForm.LinkRelation);
        //        addSlotForm.Dispose();
        //    }
        //    else
        //    {
        //        choosedLv.ResumeLayout();
        //        return;
        //    }
        //    choosedLv.Items.Add(item);
        //    item.BackColor = Color.LightBlue;
        //    choosedLv.ResumeLayout();
        //}

        //用于子类设置父类界面组件的虚函数

        protected virtual void SetFatherComponents() { }

        /// <summary>
        /// 给当前的TabControl添加一页TagPage,并在其上设置一个控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageName"></param>
        /// <param name="elementList">控件所在的列表</param>
        /// <param name="InitDelegate">初始化控件的委托</param>
        /// <param name="delegateParam">委托使用的参数</param>
        protected void AddTabPage<T>(string pageName, List<T> elementList, InitElement<T> InitDelegate, object delegateParam)
            where T : System.Windows.Forms.Control, new()
        {
            //判断添加元素类型是否为指定类型
            if (typeof(T) != typeof(ListView) && typeof(T) != typeof(DataGridView))
            {
                MessageBox.Show("AddtabPage:指定的元素不在规定范围内.(规定:ListView,DataGridView)");
                return;
            }
            TabPage tp = new TabPage();
            T element = new T();

            this.tabControl1.SuspendLayout();
            this.tabControl1.Controls.Add(tp);
            tp.Controls.Add(element);

            //初始化该TabPage
            tp.Location = new System.Drawing.Point(4, 22);
            tp.Padding = new System.Windows.Forms.Padding(0);
            tp.Size = new System.Drawing.Size(608, 287);
            tp.Text = pageName;
            tp.UseVisualStyleBackColor = true;

            //初始化TabPage的DataGridView
            InitDelegate(element, delegateParam);
            elementList.Add(element);

            this.tabControl1.ResumeLayout();
        }

        /// <summary>
        /// 往一个对象集合里面添加一个整数集合
        /// </summary>
        /// <param name="begin">开始的数值(包括)</param>
        /// <param name="end">结束的数值(不包括)</param>
        /// <param name="cutElement">需要去掉的中间数值</param>
        /// <returns></returns>
        protected static List<string> GetListRangeInt(int begin, int end, List<int> cutElements)
        {
            var intList = new List<string>();
            for (int i = begin; i < end; i++)
            {
                if (cutElements != null && cutElements.Contains(i))
                {
                    continue;
                }
                intList.Add(i.ToString());
            }
            return intList;
        }

        #region 暂时不用
        /// <summary>
        /// 实现一个可以编辑单个单元格值的ListView
        /// </summary>
        public class ListView_TextBox : ListView
        {
            private TextBox _textBox;
            private ListViewItem.ListViewSubItem _subItem;

            public ListView_TextBox()
            {
                _textBox = new TextBox();
                base.Controls.Add(_textBox);
                _textBox.Visible = false;
                base.FullRowSelect = true;
                SetItemHeight(40);

                //绑定事件订阅函数
                base.MouseUp += new MouseEventHandler(OnMouseUp);
                _textBox.Leave += new EventHandler(_textBox_Leave);
                _textBox.KeyDown += new KeyEventHandler(_textBox_KeyDown);
            }

            public void SetItemHeight(int h)
            {
                ImageList imageList = new ImageList();
                imageList.ImageSize = new System.Drawing.Size(1, h);
                base.SmallImageList = imageList;
            }

            //处理Enter键完成的效果
            void _textBox_KeyDown(object sender, KeyEventArgs e)
            {
                if ((int)e.KeyCode == 13)//Enter键的KeyCode是13
                {
                    _textBox_Leave(sender, e);
                }
            }

            //鼠标离开TextBox的事件处理函数
            void _textBox_Leave(object sender, EventArgs e)
            {
                _subItem.Text = _textBox.Text;
                _textBox.Visible = false;
                _subItem = null;
                base.Focus();
            }

            private void OnMouseUp(object sender, MouseEventArgs me)
            {
                int columnNum = 0;//当前选择的单元格在第几列
                if (base.SelectedItems.Count == 0)
                {
                    return;
                }
                ListViewItem lvItem = base.SelectedItems[0];//默认选择第一个ListViewItem

                Rectangle textRect = lvItem.Bounds;
                //调整textBox的高度
                textRect.Height = base.SmallImageList.ImageSize.Height;

                foreach (ColumnHeader column in base.Columns)
                {
                    if (me.X >= textRect.X && me.X <= (textRect.X + column.Width))
                    {
                        textRect.Width = column.Width;
                        break;
                    }
                    //匹配不上列数加一，textBox的矩形框起点往后移一个列的长度
                    columnNum++;
                    textRect.X += column.Width;
                }
                _subItem = lvItem.SubItems[columnNum];
                _textBox.AutoSize = false;
                _textBox.Bounds = textRect;
                _textBox.Text = _subItem.Text;
                _textBox.Visible = true;
                _textBox.BringToFront();
                _textBox.Focus();
                return;
            }
        }
        #endregion
    }

    //实现对DataGridView操作的静态类
    static class DataGridViewsOpt
    {
        public static void Synchronize(IDataGridViewsOpt iDgvOpt, DataGridViewRow row)
        {
            if (!iDgvOpt.JudgeCellsValue(row))
            {
                MessageBox.Show("填入内容不全！");
                return;
            }

            //同步其他槽位的一个相应的连接
            if (iDgvOpt.DgvRowsMap.Keys.Contains(row))
            {
                var mapedRow = iDgvOpt.DgvRowsMap[row];
                DataGridView mapedDgv = mapedRow.DataGridView;
                //判断连接的槽位是否更改了
                if (iDgvOpt.IsMapedRowChange(row, mapedRow))
                {
                    //！删除oppositeRow，在对应槽位创建一行新的row
                    int rowId = mapedDgv.Rows.IndexOf(mapedRow);
                    iDgvOpt.DeleteRow(mapedDgv, rowId);

                    //!添加一行
                    DataGridView oppositeDgv = iDgvOpt.GetOppositeDgv(row);
                    iDgvOpt.AddRow_Fresh(oppositeDgv, row);
                }
                else
                {
                    //更新oppositeRow的值
                    iDgvOpt.FreshRow(row, mapedRow);
                }
            }
            else
            {
                //在对应槽位创建一行新的row
                DataGridView oppositeDgv = iDgvOpt.GetOppositeDgv(row);
                iDgvOpt.AddRow_Fresh(oppositeDgv, row);
            }
        }
        public static void AddRow(IDataGridViewsOpt iDgvOpt, DataGridView dgv)
        {
            iDgvOpt.AddRow(dgv);
        }
        public static void DelRow(IDataGridViewsOpt iDgvOpt, DataGridView dgv)
        {
            try
            {
                DataGridViewRow choosedRow = dgv.SelectedRows[0];
                int rowIndex = dgv.Rows.IndexOf(choosedRow);

                //查看有无对应对端row,有则删除；
                if (iDgvOpt.DgvRowsMap.Keys.Contains(choosedRow))
                {
                    DataGridViewRow oppositeRow = iDgvOpt.DgvRowsMap[choosedRow];
                    var oppositeDgv = oppositeRow.DataGridView;
                    int oppositeRowIndex = oppositeDgv.Rows.IndexOf(oppositeRow);
                    iDgvOpt.DeleteRow(oppositeDgv, oppositeRowIndex);
                }
                //删除本端的row
                iDgvOpt.DeleteRow(dgv, rowIndex);
            }
            catch (Exception ep)
            {
                MessageBox.Show(ep.Message);
            }
        }

        /// <summary>
        /// 用来做DataGridViews的同步操作的接口
        /// </summary>
        public interface IDataGridViewsOpt
        {
            //用于记录成对需互相同步的rows
            Dictionary<DataGridViewRow, DataGridViewRow> DgvRowsMap { get; set; }

            /// <summary>
            ///判断该行所有cell的值是否合法,及完整性
            /// </summary>
            /// <param name="row"></param>
            /// <returns>false:不合法/完整；true:合法且完整</returns>
            bool JudgeCellsValue(DataGridViewRow row);

            //映射的行是否改变(是指映射关系)
            bool IsMapedRowChange(DataGridViewRow row, DataGridViewRow mapedRow);

            void DeleteRow(DataGridView dgv, int rowId);

            void FreshRow(DataGridViewRow row, DataGridViewRow mapedRow);

            void AddRow_Fresh(DataGridView dgv, DataGridViewRow srcRow);

            DataGridView GetOppositeDgv(DataGridViewRow row);

            void AddRow(DataGridView dgv);
        }
    }
}
