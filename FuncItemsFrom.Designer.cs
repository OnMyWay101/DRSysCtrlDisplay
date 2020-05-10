namespace DRSysCtrlDisplay
{
    partial class FuncItemsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("资源拓扑图");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("状态视图");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("构件A");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("构件B");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("构件C");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("ABC应用", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("192.168.11.1", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode6});
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._srTreeView = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._eqTreeView = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this._cpTreeView = new System.Windows.Forms.TreeView();
            this._libCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._libAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this._cpCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cpDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this._cpModifyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this._libCMS.SuspendLayout();
            this._cpCMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(160, 442);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._srTreeView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(152, 416);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "资源池";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _srTreeView
            // 
            this._srTreeView.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this._srTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._srTreeView.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._srTreeView.Location = new System.Drawing.Point(3, 3);
            this._srTreeView.Name = "_srTreeView";
            treeNode1.Name = "Node1";
            treeNode1.Text = "资源拓扑图";
            treeNode2.Name = "Node2";
            treeNode2.Text = "状态视图";
            treeNode3.Name = "Node4";
            treeNode3.Text = "构件A";
            treeNode4.Name = "Node5";
            treeNode4.Text = "构件B";
            treeNode5.Name = "Node6";
            treeNode5.Text = "构件C";
            treeNode6.Name = "Node3";
            treeNode6.Text = "ABC应用";
            treeNode7.Name = "Node0";
            treeNode7.Text = "192.168.11.1";
            this._srTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
            this._srTreeView.Size = new System.Drawing.Size(146, 410);
            this._srTreeView.TabIndex = 0;
            this._srTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SrTreeView_AfterSelect);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this._eqTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(152, 416);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "设备库";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // _eqTreeView
            // 
            this._eqTreeView.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this._eqTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._eqTreeView.Location = new System.Drawing.Point(3, 3);
            this._eqTreeView.Name = "_eqTreeView";
            this._eqTreeView.Size = new System.Drawing.Size(146, 410);
            this._eqTreeView.TabIndex = 0;
            this._eqTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LibContextMS_MouseClick);
            this._eqTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.EqTreeView_AfterSelect);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this._cpTreeView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(152, 416);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "构件库";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // _cpTreeView
            // 
            this._cpTreeView.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this._cpTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cpTreeView.Location = new System.Drawing.Point(3, 3);
            this._cpTreeView.Name = "_cpTreeView";
            this._cpTreeView.Size = new System.Drawing.Size(146, 410);
            this._cpTreeView.TabIndex = 0;
            this._cpTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CpTreeView_AfterSelect);
            // 
            // _libCMS
            // 
            this._libCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._libAddItem});
            this._libCMS.Name = "_libCMS";
            this._libCMS.Size = new System.Drawing.Size(101, 26);
            // 
            // _libAddItem
            // 
            this._libAddItem.Name = "_libAddItem";
            this._libAddItem.Size = new System.Drawing.Size(100, 22);
            this._libAddItem.Text = "添加";
            this._libAddItem.Click += new System.EventHandler(this.ContextMSAdd_Click);
            // 
            // _cpCMS
            // 
            this._cpCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cpDeleteItem,
            this._cpModifyItem});
            this._cpCMS.Name = "_cpCMS";
            this._cpCMS.Size = new System.Drawing.Size(153, 70);
            // 
            // _cpDeleteItem
            // 
            this._cpDeleteItem.Name = "_cpDeleteItem";
            this._cpDeleteItem.Size = new System.Drawing.Size(152, 22);
            this._cpDeleteItem.Text = "删除";
            this._cpDeleteItem.Click += new System.EventHandler(this.ContextMSDelete_Click);
            // 
            // _cpModifyItem
            // 
            this._cpModifyItem.Name = "_cpModifyItem";
            this._cpModifyItem.Size = new System.Drawing.Size(152, 22);
            this._cpModifyItem.Text = "修改";
            this._cpModifyItem.Click += new System.EventHandler(this.ContextMSModify_Click);
            // 
            // FuncItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(160, 442);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FuncItems";
            this.Text = "功能";
            this.Load += new System.EventHandler(this.FuncItems_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this._libCMS.ResumeLayout(false);
            this._cpCMS.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.TreeView _srTreeView;
        public System.Windows.Forms.TreeView _eqTreeView;
        public System.Windows.Forms.TreeView _cpTreeView;
        private System.Windows.Forms.ContextMenuStrip _libCMS;
        private System.Windows.Forms.ToolStripMenuItem _libAddItem;
        private System.Windows.Forms.ContextMenuStrip _cpCMS;
        private System.Windows.Forms.ToolStripMenuItem _cpDeleteItem;
        private System.Windows.Forms.ToolStripMenuItem _cpModifyItem;


    }
}