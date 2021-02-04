namespace DRSysCtrlDisplay
{
    partial class ComponentInitForm_AddSource
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
            this._btnPanel = new System.Windows.Forms.Panel();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.yesButton = new System.Windows.Forms.Button();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._ethTP = new System.Windows.Forms.TabPage();
            this._ethSubDelBtn = new System.Windows.Forms.Button();
            this._ethPbDelBtn = new System.Windows.Forms.Button();
            this._ethSubAddBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._ethSubDgv = new System.Windows.Forms.DataGridView();
            this._ethPbAddBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._ethPbDgv = new System.Windows.Forms.DataGridView();
            this._rioTP = new System.Windows.Forms.TabPage();
            this._rioSubDelBtn = new System.Windows.Forms.Button();
            this._rioPbDelBtn = new System.Windows.Forms.Button();
            this._rioSubAddBtn = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this._rioSubDgv = new System.Windows.Forms.DataGridView();
            this._rioPbAddBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._rioPbDgv = new System.Windows.Forms.DataGridView();
            this._btnPanel.SuspendLayout();
            this._tabControl.SuspendLayout();
            this._ethTP.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ethSubDgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ethPbDgv)).BeginInit();
            this._rioTP.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._rioSubDgv)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._rioPbDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnPanel
            // 
            this._btnPanel.Controls.Add(this.cancelBtn);
            this._btnPanel.Controls.Add(this.yesButton);
            this._btnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._btnPanel.Location = new System.Drawing.Point(0, 426);
            this._btnPanel.Name = "_btnPanel";
            this._btnPanel.Size = new System.Drawing.Size(589, 51);
            this._btnPanel.TabIndex = 3;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(337, 18);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(41, 18);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 0;
            this.yesButton.Text = "确定";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._ethTP);
            this._tabControl.Controls.Add(this._rioTP);
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point(0, 0);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(589, 426);
            this._tabControl.TabIndex = 4;
            // 
            // _ethTP
            // 
            this._ethTP.Controls.Add(this._ethSubDelBtn);
            this._ethTP.Controls.Add(this._ethPbDelBtn);
            this._ethTP.Controls.Add(this._ethSubAddBtn);
            this._ethTP.Controls.Add(this.groupBox2);
            this._ethTP.Controls.Add(this._ethPbAddBtn);
            this._ethTP.Controls.Add(this.groupBox1);
            this._ethTP.Location = new System.Drawing.Point(4, 22);
            this._ethTP.Name = "_ethTP";
            this._ethTP.Padding = new System.Windows.Forms.Padding(3);
            this._ethTP.Size = new System.Drawing.Size(581, 400);
            this._ethTP.TabIndex = 0;
            this._ethTP.Text = "Eth";
            this._ethTP.UseVisualStyleBackColor = true;
            // 
            // _ethSubDelBtn
            // 
            this._ethSubDelBtn.Location = new System.Drawing.Point(487, 316);
            this._ethSubDelBtn.Name = "_ethSubDelBtn";
            this._ethSubDelBtn.Size = new System.Drawing.Size(75, 39);
            this._ethSubDelBtn.TabIndex = 2;
            this._ethSubDelBtn.Text = "删除";
            this._ethSubDelBtn.UseVisualStyleBackColor = true;
            // 
            // _ethPbDelBtn
            // 
            this._ethPbDelBtn.Location = new System.Drawing.Point(487, 120);
            this._ethPbDelBtn.Name = "_ethPbDelBtn";
            this._ethPbDelBtn.Size = new System.Drawing.Size(75, 43);
            this._ethPbDelBtn.TabIndex = 4;
            this._ethPbDelBtn.Text = "删除";
            this._ethPbDelBtn.UseVisualStyleBackColor = true;
            // 
            // _ethSubAddBtn
            // 
            this._ethSubAddBtn.Location = new System.Drawing.Point(487, 236);
            this._ethSubAddBtn.Name = "_ethSubAddBtn";
            this._ethSubAddBtn.Size = new System.Drawing.Size(75, 42);
            this._ethSubAddBtn.TabIndex = 1;
            this._ethSubAddBtn.Text = "添加";
            this._ethSubAddBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._ethSubDgv);
            this.groupBox2.Location = new System.Drawing.Point(3, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(478, 191);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "订阅资源";
            // 
            // _ethSubDgv
            // 
            this._ethSubDgv.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this._ethSubDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ethSubDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ethSubDgv.Location = new System.Drawing.Point(3, 17);
            this._ethSubDgv.Name = "_ethSubDgv";
            this._ethSubDgv.RowTemplate.Height = 23;
            this._ethSubDgv.Size = new System.Drawing.Size(472, 171);
            this._ethSubDgv.TabIndex = 0;
            // 
            // _ethPbAddBtn
            // 
            this._ethPbAddBtn.Location = new System.Drawing.Point(487, 43);
            this._ethPbAddBtn.Name = "_ethPbAddBtn";
            this._ethPbAddBtn.Size = new System.Drawing.Size(75, 44);
            this._ethPbAddBtn.TabIndex = 3;
            this._ethPbAddBtn.Text = "添加";
            this._ethPbAddBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._ethPbDgv);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 185);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "发布资源";
            // 
            // _ethPbDgv
            // 
            this._ethPbDgv.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this._ethPbDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._ethPbDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ethPbDgv.Location = new System.Drawing.Point(3, 17);
            this._ethPbDgv.Name = "_ethPbDgv";
            this._ethPbDgv.RowTemplate.Height = 23;
            this._ethPbDgv.Size = new System.Drawing.Size(469, 165);
            this._ethPbDgv.TabIndex = 0;
            this._ethPbDgv.TabStop = false;
            // 
            // _rioTP
            // 
            this._rioTP.Controls.Add(this._rioSubDelBtn);
            this._rioTP.Controls.Add(this._rioPbDelBtn);
            this._rioTP.Controls.Add(this._rioSubAddBtn);
            this._rioTP.Controls.Add(this.groupBox4);
            this._rioTP.Controls.Add(this._rioPbAddBtn);
            this._rioTP.Controls.Add(this.groupBox3);
            this._rioTP.Location = new System.Drawing.Point(4, 22);
            this._rioTP.Name = "_rioTP";
            this._rioTP.Padding = new System.Windows.Forms.Padding(3);
            this._rioTP.Size = new System.Drawing.Size(581, 400);
            this._rioTP.TabIndex = 1;
            this._rioTP.Text = "Rio";
            this._rioTP.UseVisualStyleBackColor = true;
            // 
            // _rioSubDelBtn
            // 
            this._rioSubDelBtn.Location = new System.Drawing.Point(486, 315);
            this._rioSubDelBtn.Name = "_rioSubDelBtn";
            this._rioSubDelBtn.Size = new System.Drawing.Size(75, 47);
            this._rioSubDelBtn.TabIndex = 2;
            this._rioSubDelBtn.Text = "删除";
            this._rioSubDelBtn.UseVisualStyleBackColor = true;
            // 
            // _rioPbDelBtn
            // 
            this._rioPbDelBtn.Location = new System.Drawing.Point(486, 123);
            this._rioPbDelBtn.Name = "_rioPbDelBtn";
            this._rioPbDelBtn.Size = new System.Drawing.Size(75, 43);
            this._rioPbDelBtn.TabIndex = 2;
            this._rioPbDelBtn.Text = "删除";
            this._rioPbDelBtn.UseVisualStyleBackColor = true;
            // 
            // _rioSubAddBtn
            // 
            this._rioSubAddBtn.Location = new System.Drawing.Point(486, 238);
            this._rioSubAddBtn.Name = "_rioSubAddBtn";
            this._rioSubAddBtn.Size = new System.Drawing.Size(75, 45);
            this._rioSubAddBtn.TabIndex = 1;
            this._rioSubAddBtn.Text = "添加";
            this._rioSubAddBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this._rioSubDgv);
            this.groupBox4.Location = new System.Drawing.Point(6, 200);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(471, 191);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "订阅资源";
            // 
            // _rioSubDgv
            // 
            this._rioSubDgv.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this._rioSubDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._rioSubDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rioSubDgv.Location = new System.Drawing.Point(3, 17);
            this._rioSubDgv.Name = "_rioSubDgv";
            this._rioSubDgv.RowTemplate.Height = 23;
            this._rioSubDgv.Size = new System.Drawing.Size(465, 171);
            this._rioSubDgv.TabIndex = 0;
            // 
            // _rioPbAddBtn
            // 
            this._rioPbAddBtn.Location = new System.Drawing.Point(486, 46);
            this._rioPbAddBtn.Name = "_rioPbAddBtn";
            this._rioPbAddBtn.Size = new System.Drawing.Size(75, 44);
            this._rioPbAddBtn.TabIndex = 1;
            this._rioPbAddBtn.Text = "添加";
            this._rioPbAddBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._rioPbDgv);
            this.groupBox3.Location = new System.Drawing.Point(9, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(471, 188);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "发布资源";
            // 
            // _rioPbDgv
            // 
            this._rioPbDgv.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this._rioPbDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._rioPbDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rioPbDgv.Location = new System.Drawing.Point(3, 17);
            this._rioPbDgv.Name = "_rioPbDgv";
            this._rioPbDgv.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._rioPbDgv.RowTemplate.Height = 23;
            this._rioPbDgv.Size = new System.Drawing.Size(465, 168);
            this._rioPbDgv.TabIndex = 0;
            // 
            // ComponentInitForm_AddSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 477);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._btnPanel);
            this.Name = "ComponentInitForm_AddSource";
            this.Text = "Form1";
            this._btnPanel.ResumeLayout(false);
            this._tabControl.ResumeLayout(false);
            this._ethTP.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ethSubDgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ethPbDgv)).EndInit();
            this._rioTP.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._rioSubDgv)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._rioPbDgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _btnPanel;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _ethTP;
        private System.Windows.Forms.Button _ethSubDelBtn;
        private System.Windows.Forms.Button _ethPbDelBtn;
        private System.Windows.Forms.Button _ethSubAddBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView _ethSubDgv;
        private System.Windows.Forms.Button _ethPbAddBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView _ethPbDgv;
        private System.Windows.Forms.TabPage _rioTP;
        private System.Windows.Forms.Button _rioSubDelBtn;
        private System.Windows.Forms.Button _rioPbDelBtn;
        private System.Windows.Forms.Button _rioSubAddBtn;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView _rioSubDgv;
        private System.Windows.Forms.Button _rioPbAddBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView _rioPbDgv;
    }
}

