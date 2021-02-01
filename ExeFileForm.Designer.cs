namespace DRSysCtrlDisplay
{
    partial class ExeFileForm
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
            this.ComponentLV = new System.Windows.Forms.ListView();
            this._UpdateCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._UpdateItem = new System.Windows.Forms.ToolStripMenuItem();
            this._ClearItem = new System.Windows.Forms.ToolStripMenuItem();
            this.YesBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this._UpdateCMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComponentLV
            // 
            this.ComponentLV.Location = new System.Drawing.Point(0, 0);
            this.ComponentLV.Name = "ComponentLV";
            this.ComponentLV.Size = new System.Drawing.Size(850, 263);
            this.ComponentLV.TabIndex = 0;
            this.ComponentLV.UseCompatibleStateImageBehavior = false;
            this.ComponentLV.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ComponentLV_MouseClick);
            // 
            // _UpdateCMS
            // 
            this._UpdateCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._UpdateItem, this._ClearItem});
            this._UpdateCMS.Name = "_UpdateCMS";
            this._UpdateCMS.Size = new System.Drawing.Size(101, 26);
            // 
            // _UpdateItem
            // 
            this._UpdateItem.Name = "_UpdateItem";
            this._UpdateItem.Size = new System.Drawing.Size(100, 22);
            this._UpdateItem.Text = "上传";
            this._UpdateItem.Click += new System.EventHandler(this._UpdateItem_Click);
            // 
            // _ClearItem
            // 
            this._ClearItem.Name = "_ClearItem";
            this._ClearItem.Size = new System.Drawing.Size(100, 22);
            this._ClearItem.Text = "清空";
            this._ClearItem.Click += new System.EventHandler(this._ClearItem_Click);
            // 
            // YesButton
            // 
            this.YesBtn.Location = new System.Drawing.Point(100, 270);
            this.YesBtn.Name = "YesButton";
            this.YesBtn.Size = new System.Drawing.Size(75, 23);
            this.YesBtn.TabIndex = 1;
            this.YesBtn.Text = "部署文件";
            this.YesBtn.UseVisualStyleBackColor = true;
            this.YesBtn.Click += new System.EventHandler(this.YesButton_Click);
            // 
            // CancelButton
            // 
            this.CancelBtn.Location = new System.Drawing.Point(650, 270);
            this.CancelBtn.Name = "CancelButton";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ExeFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 305);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ComponentLV);
            this.Controls.Add(this.YesBtn);
            this.Name = "ExeFileForm";
            this.Text = "ExeFileForm";
            this._UpdateCMS.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ComponentLV;
        private System.Windows.Forms.ContextMenuStrip _UpdateCMS;
        private System.Windows.Forms.ToolStripMenuItem _UpdateItem;
        private System.Windows.Forms.ToolStripMenuItem _ClearItem;
        private System.Windows.Forms.Button YesBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}