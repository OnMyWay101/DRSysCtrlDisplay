namespace DRSysCtrlDisplay
{
    partial class NodeInfoAddForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this._cntCB = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._appLv = new System.Windows.Forms.ListView();
            this._addAppBtn = new System.Windows.Forms.Button();
            this._cmpCB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._yesBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._delMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 475);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this._cntCB);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(251, 66);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "系统信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "系统型号：";
            // 
            // _cntCB
            // 
            this._cntCB.FormattingEnabled = true;
            this._cntCB.Location = new System.Drawing.Point(73, 27);
            this._cntCB.Name = "_cntCB";
            this._cntCB.Size = new System.Drawing.Size(166, 20);
            this._cntCB.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._appLv);
            this.groupBox1.Controls.Add(this._addAppBtn);
            this.groupBox1.Controls.Add(this._cmpCB);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 362);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "应用信息";
            // 
            // _appLv
            // 
            this._appLv.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._appLv.Location = new System.Drawing.Point(3, 53);
            this._appLv.Name = "_appLv";
            this._appLv.Size = new System.Drawing.Size(245, 306);
            this._appLv.TabIndex = 7;
            this._appLv.UseCompatibleStateImageBehavior = false;
            // 
            // _addAppBtn
            // 
            this._addAppBtn.Location = new System.Drawing.Point(199, 24);
            this._addAppBtn.Name = "_addAppBtn";
            this._addAppBtn.Size = new System.Drawing.Size(40, 23);
            this._addAppBtn.TabIndex = 6;
            this._addAppBtn.Text = "添加";
            this._addAppBtn.UseVisualStyleBackColor = true;
            // 
            // _cmpCB
            // 
            this._cmpCB.FormattingEnabled = true;
            this._cmpCB.Location = new System.Drawing.Point(67, 25);
            this._cmpCB.Name = "_cmpCB";
            this._cmpCB.Size = new System.Drawing.Size(127, 20);
            this._cmpCB.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "应用型号：";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._cancelBtn);
            this.panel2.Controls.Add(this._yesBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 428);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(251, 47);
            this.panel2.TabIndex = 6;
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Location = new System.Drawing.Point(12, 12);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(75, 23);
            this._cancelBtn.TabIndex = 5;
            this._cancelBtn.Text = "取消";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _yesBtn
            // 
            this._yesBtn.Location = new System.Drawing.Point(164, 12);
            this._yesBtn.Name = "_yesBtn";
            this._yesBtn.Size = new System.Drawing.Size(75, 23);
            this._yesBtn.TabIndex = 5;
            this._yesBtn.Text = "确定";
            this._yesBtn.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._delMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // _delMenuItem
            // 
            this._delMenuItem.Name = "_delMenuItem";
            this._delMenuItem.Size = new System.Drawing.Size(100, 22);
            this._delMenuItem.Text = "删除";
            // 
            // NodeInfoAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 475);
            this.Controls.Add(this.panel1);
            this.Name = "NodeInfoAddForm";
            this.Text = "NodeInfoAddForm";
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _yesBtn;
        private System.Windows.Forms.ComboBox _cntCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.ListView _appLv;
        private System.Windows.Forms.Button _addAppBtn;
        private System.Windows.Forms.ComboBox _cmpCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _delMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}