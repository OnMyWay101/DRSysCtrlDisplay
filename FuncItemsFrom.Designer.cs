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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._srTreeView = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._eqTreeView = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this._cpTreeView = new System.Windows.Forms.TreeView();
            this._addCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._addCMSAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this._refreshCMSAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this._editCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._editCMSDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this._editCMSModifyItem = new System.Windows.Forms.ToolStripMenuItem();
            this._srcCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._srcCMS_AddInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._srcCMS_MatchApp = new System.Windows.Forms.ToolStripMenuItem();
            this._srcCMS_Upload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._srcCMS_Recrt = new System.Windows.Forms.ToolStripMenuItem();
            this._srcCMS_ClearInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this._addCMS.SuspendLayout();
            this._editCMS.SuspendLayout();
            this._srcCMS.SuspendLayout();
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
            this._srTreeView.Size = new System.Drawing.Size(146, 410);
            this._srTreeView.TabIndex = 0;
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
            // 
            // _addCMS
            // 
            this._addCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addCMSAddItem,
            this._refreshCMSAddItem});
            this._addCMS.Name = "_libCMS";
            this._addCMS.Size = new System.Drawing.Size(101, 48);
            // 
            // _addCMSAddItem
            // 
            this._addCMSAddItem.Name = "_addCMSAddItem";
            this._addCMSAddItem.Size = new System.Drawing.Size(100, 22);
            this._addCMSAddItem.Text = "添加";
            // 
            // _refreshCMSAddItem
            // 
            this._refreshCMSAddItem.Name = "_refreshCMSAddItem";
            this._refreshCMSAddItem.Size = new System.Drawing.Size(100, 22);
            this._refreshCMSAddItem.Text = "刷新";
            // 
            // _editCMS
            // 
            this._editCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._editCMSDeleteItem,
            this._editCMSModifyItem});
            this._editCMS.Name = "_cpCMS";
            this._editCMS.Size = new System.Drawing.Size(101, 48);
            // 
            // _editCMSDeleteItem
            // 
            this._editCMSDeleteItem.Name = "_editCMSDeleteItem";
            this._editCMSDeleteItem.Size = new System.Drawing.Size(100, 22);
            this._editCMSDeleteItem.Text = "删除";
            // 
            // _editCMSModifyItem
            // 
            this._editCMSModifyItem.Name = "_editCMSModifyItem";
            this._editCMSModifyItem.Size = new System.Drawing.Size(100, 22);
            this._editCMSModifyItem.Text = "修改";
            // 
            // _srcCMS
            // 
            this._srcCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._srcCMS_AddInfo,
            this._srcCMS_ClearInfo,
            this.toolStripSeparator1,
            this._srcCMS_MatchApp,
            this._srcCMS_Upload,
            this.toolStripSeparator2,
            this._srcCMS_Recrt});
            this._srcCMS.Name = "_srcCMS";
            this._srcCMS.Size = new System.Drawing.Size(153, 148);
            // 
            // _srcCMS_AddInfo
            // 
            this._srcCMS_AddInfo.Name = "_srcCMS_AddInfo";
            this._srcCMS_AddInfo.Size = new System.Drawing.Size(152, 22);
            this._srcCMS_AddInfo.Text = "添加信息";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // _srcCMS_MatchApp
            // 
            this._srcCMS_MatchApp.Name = "_srcCMS_MatchApp";
            this._srcCMS_MatchApp.Size = new System.Drawing.Size(152, 22);
            this._srcCMS_MatchApp.Text = "匹配";
            this._srcCMS_MatchApp.Click += new System.EventHandler(this._srcCMS_MatchApp_Click);
            // 
            // _srcCMS_Upload
            // 
            this._srcCMS_Upload.Name = "_srcCMS_Upload";
            this._srcCMS_Upload.Size = new System.Drawing.Size(152, 22);
            this._srcCMS_Upload.Text = "部署文件";
            this._srcCMS_Upload.Click += new System.EventHandler(this._srcCMS_Upload_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // _srcCMS_Recrt
            // 
            this._srcCMS_Recrt.Name = "_srcCMS_Recrt";
            this._srcCMS_Recrt.Size = new System.Drawing.Size(152, 22);
            this._srcCMS_Recrt.Text = "重构";
            this._srcCMS_Recrt.Click += new System.EventHandler(this._srcCMS_Recrt_Click);
            // 
            // toolStripMenuItem1
            // 
            this._srcCMS_ClearInfo.Name = "toolStripMenuItem1";
            this._srcCMS_ClearInfo.Size = new System.Drawing.Size(152, 22);
            this._srcCMS_ClearInfo.Text = "清除信息";
            this._srcCMS_ClearInfo.Click += new System.EventHandler(this.SrcCMS_ClearInfo_Click);
            // 
            // FuncItemsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(160, 442);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FuncItemsForm";
            this.Text = "功能";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this._addCMS.ResumeLayout(false);
            this._editCMS.ResumeLayout(false);
            this._srcCMS.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip _addCMS;
        private System.Windows.Forms.ToolStripMenuItem _addCMSAddItem;
        private System.Windows.Forms.ContextMenuStrip _editCMS;
        private System.Windows.Forms.ToolStripMenuItem _editCMSDeleteItem;
        private System.Windows.Forms.ToolStripMenuItem _editCMSModifyItem;
        private System.Windows.Forms.ToolStripMenuItem _refreshCMSAddItem;
        private System.Windows.Forms.ContextMenuStrip _srcCMS;
        private System.Windows.Forms.ToolStripMenuItem _srcCMS_MatchApp;
        private System.Windows.Forms.ToolStripMenuItem _srcCMS_AddInfo;
        private System.Windows.Forms.ToolStripMenuItem _srcCMS_Recrt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _srcCMS_Upload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _srcCMS_ClearInfo;


    }
}