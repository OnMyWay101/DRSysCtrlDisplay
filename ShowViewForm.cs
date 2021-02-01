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
        public ShowViewPanel showViewPanel1;    //显示图像窗口对应的显示面板
        TreeNode _bindNode;                     //当前显示窗体对应的TreeView节点

        public ShowViewForm(TreeNode node)
        {
            this.showViewPanel1 = new ShowViewPanel(node);
            this.showViewPanel1.Dock = DockStyle.Fill;
            this.showViewPanel1.Location = new Point(0, 0);
            this.showViewPanel1.Name = "showViewPanel1";
            this.showViewPanel1.Size = new Size(284, 261);
            this.showViewPanel1.TabIndex = 0;
            // ShowViewForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.showViewPanel1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ShowViewForm";
            this.Text = "ShowViewForm";

            //配置
            this._bindNode = node;
            NodeInfo.AttachForm(_bindNode, this);
            this.FormClosed += new FormClosedEventHandler(ShowViewFormClosed);
            this.FormClosing += new FormClosingEventHandler(ShowViewFormClosing);
        }

        private void ShowViewFormClosed(object sender, EventArgs e)
        {
            NodeInfo.DetachForm(_bindNode);
            _bindNode = null;
        }


        //关闭窗口的事件处理函数改写
        private void ShowViewFormClosing(object sender, FormClosingEventArgs e)
        {
            var indo = _bindNode.Tag as NodeInfo;
            if (indo._formType == Princeple.FormType.APP ||
                indo._formType == Princeple.FormType.STATUS)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ShowViewForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ShowViewForm";
            this.ResumeLayout(false);
        }
    }

}
