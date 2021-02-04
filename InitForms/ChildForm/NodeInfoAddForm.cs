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
    public partial class NodeInfoAddForm : Form
    {
        public NodeInfoAddForm()
        {
            InitializeComponent();
            InitControls();

            this._addAppBtn.Click += new EventHandler(AddAppBtn_Click);
            this._appLv.MouseClick += new MouseEventHandler(AppLv_Click);
            this._delMenuItem.Click += new EventHandler(DelMenuItem_Click);
            this._cancelBtn.Click += new EventHandler((o, e) => this.DialogResult = DialogResult.Cancel);
            this._yesBtn.Click += new EventHandler((o, e) => this.DialogResult = DialogResult.Yes);
        }

        void AddAppBtn_Click(object sender, EventArgs e)
        {
            if (_cmpCB.Text == string.Empty)
            {
                MessageBox.Show("请选定应用类型！");
                return;
            }
            int appSn = _appLv.Items.Count;
            _appLv.Items.Add(appSn.ToString());
            _appLv.Items[appSn].SubItems.Add(_cmpCB.Text);
        }

        void DelMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem choosedItem = _appLv.SelectedItems[0];

            _appLv.SuspendLayout();
            _appLv.Items.Remove(choosedItem);
            //序号重新刷新一下
            foreach (ListViewItem item in _appLv.Items)
            {
                item.Text = item.Index.ToString();
            }

            _appLv.ResumeLayout();
        }

        void AppLv_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(_appLv, e.Location);
            }
        }

        private void InitControls()
        {
            this.SuspendLayout();   //挂起布局
            //机箱的ComboBox
            var cntNames = FuncItemsForm.GetInstance().GetEqSetNames(Princeple.FormType.SYSTEM);
            _cntCB.Items.AddRange(cntNames);

            //构件的ComboBox
            var cmpNames = FuncItemsForm.GetInstance().GetEqSetNames(Princeple.FormType.COMPONENT);
            _cmpCB.Items.AddRange(cmpNames);

            //初始化listView
            _appLv.View = View.Details;
            _appLv.GridLines = true;
            _appLv.FullRowSelect = true;
            _appLv.Columns.Add("序号", 70, HorizontalAlignment.Center);
            _appLv.Columns.Add(new ColumnHeader() { Text = "应用类型", TextAlign = HorizontalAlignment.Center});
            //_appLv.Columns[1].Width = _appLv.ClientSize.Width - _appLv.Columns[0].Width;
            _appLv.Columns[1].Width = _appLv.Width - _appLv.Columns[0].Width;

            this.ResumeLayout();    //实施布局
        }

        public String GetContainerName()
        {
            return _cntCB.Text;
        }

        public List<String> GetComponentNames()
        {
            List<string> cmpNames = new List<string>();
            foreach (ListViewItem item in _appLv.Items)
            {
                cmpNames.Add(item.SubItems[1].Text);
            }
            return cmpNames;
        }

    }
}
