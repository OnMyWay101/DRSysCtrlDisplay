using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DRSysCtrlDisplay
{
    public class ShowViewForm : DockContent
    {
        public ShowViewPanel showViewPanel1;
        TreeNode _bindNode;

        public ShowViewForm(TreeNode node)
        {
            //配置
            this._bindNode = node;
            NodeInfo.AttachForm(_bindNode, this);
            this.FormClosed += new FormClosedEventHandler(ShowViewFormClosed);

            //showViewPanel1
            this.SuspendLayout();
            this.showViewPanel1 = new DRSysCtrlDisplay.ShowViewPanel(node);
            this.showViewPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showViewPanel1.Location = new System.Drawing.Point(0, 0);
            this.showViewPanel1.Name = "showViewPanel1";
            this.showViewPanel1.Size = new System.Drawing.Size(284, 261);
            this.showViewPanel1.TabIndex = 0;
            // ShowViewForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.showViewPanel1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ShowViewForm";
            this.Text = "ShowViewForm";
            this.ResumeLayout();
        }

        private void ShowViewFormClosed(object sender, EventArgs e)
        {
            NodeInfo.DetachForm(_bindNode);
            _bindNode = null;
        }
    }

}
