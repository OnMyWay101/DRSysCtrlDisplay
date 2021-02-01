namespace DRSysCtrlDisplay
{
    partial class TargetConnectForm
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


        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.Button _yesBtn;
        private System.Windows.Forms.ListView listView1;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this._yesBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancelBtn);
            this.panel1.Controls.Add(this._yesBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 223);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(287, 38);
            this.panel1.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(287, 223);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // button2
            // 
            this._yesBtn.Location = new System.Drawing.Point(55, 5);
            this._yesBtn.Name = "button2";
            this._yesBtn.Size = new System.Drawing.Size(50, 20);
            this._yesBtn.TabIndex = 1;
            this._yesBtn.Text = "确定";
            this._yesBtn.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this._cancelBtn.Location = new System.Drawing.Point(205, 5);
            this._cancelBtn.Name = "button3";
            this._cancelBtn.Size = new System.Drawing.Size(50, 20);
            this._cancelBtn.TabIndex = 2;
            this._cancelBtn.Text = "取消";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // TargetConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 261);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Name = "TargetConnectForm";
            this.Text = "TargetConnectForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

    }
}