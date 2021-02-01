namespace DRSysCtrlDisplay
{
    partial class ReconfigForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this._autoReCfgCB = new System.Windows.Forms.CheckBox();
            this._yesBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._strategyLv = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._infoLv = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._autoReCfgCB);
            this.panel1.Controls.Add(this._yesBtn);
            this.panel1.Controls.Add(this._cancelBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 301);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(936, 40);
            this.panel1.TabIndex = 0;
            // 
            // _autoReCfgCB
            // 
            this._autoReCfgCB.AutoSize = true;
            this._autoReCfgCB.Location = new System.Drawing.Point(830, 14);
            this._autoReCfgCB.Name = "_autoReCfgCB";
            this._autoReCfgCB.Size = new System.Drawing.Size(72, 16);
            this._autoReCfgCB.TabIndex = 2;
            this._autoReCfgCB.Text = "自动重构";
            this._autoReCfgCB.UseVisualStyleBackColor = true;
            // 
            // _yesBtn
            // 
            this._yesBtn.Location = new System.Drawing.Point(707, 9);
            this._yesBtn.Name = "_yesBtn";
            this._yesBtn.Size = new System.Drawing.Size(75, 23);
            this._yesBtn.TabIndex = 1;
            this._yesBtn.Text = "确定";
            this._yesBtn.UseVisualStyleBackColor = true;
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Location = new System.Drawing.Point(118, 9);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(75, 23);
            this._cancelBtn.TabIndex = 0;
            this._cancelBtn.Text = "取消";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._strategyLv);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 301);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "方案选择";
            // 
            // _strategyLv
            // 
            this._strategyLv.Dock = System.Windows.Forms.DockStyle.Fill;
            this._strategyLv.Location = new System.Drawing.Point(3, 17);
            this._strategyLv.Name = "_strategyLv";
            this._strategyLv.Size = new System.Drawing.Size(194, 281);
            this._strategyLv.TabIndex = 0;
            this._strategyLv.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._infoLv);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(200, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(736, 301);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "方案详细信息";
            // 
            // _infoLv
            // 
            this._infoLv.Dock = System.Windows.Forms.DockStyle.Fill;
            this._infoLv.Location = new System.Drawing.Point(3, 17);
            this._infoLv.Name = "_infoLv";
            this._infoLv.Size = new System.Drawing.Size(730, 281);
            this._infoLv.TabIndex = 1;
            this._infoLv.UseCompatibleStateImageBehavior = false;
            // 
            // ReconfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 341);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "ReconfigForm";
            this.Text = "ReconfigForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _yesBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView _strategyLv;
        private System.Windows.Forms.ListView _infoLv;
        private System.Windows.Forms.CheckBox _autoReCfgCB;
    }
}