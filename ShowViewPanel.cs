using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DRSysCtrlDisplay
{
    public class ShowViewPanel : Panel
    {
        public BaseView ShowView{ get; private set; }
        public PPCInitForm.PropertyFormType FormTypr { get; private set; }
        public float ZoomFactor { get; private set; }

        private PointF _viewOffset;


        public ShowViewPanel(TreeNode node)
        {

            XmlDocument doc = new XmlDocument();
            NodeInfo info = (NodeInfo)node.Tag;

            //1.通过Xml文件创建view
            if (info._xmlPath != string.Empty)
            {
                try
                {
                    doc.Load(info._xmlPath);
                }
                catch
                {
                    MessageBox.Show("Error:XML文件（" + info._xmlPath + ")打开错误");
                    return;
                }
            }
            switch (info._formType)
            {
                case PPCInitForm.PropertyFormType.PPC:
                    ShowView = new PPC(doc, node.Text);
                    break;
                case PPCInitForm.PropertyFormType.FPAG:
                    return;
                    break;
                case PPCInitForm.PropertyFormType.TOPO:
                    ShowView = new Topo(this);
                    break;
                default:
                    return;
                    break;
            }

            //初始化其他成员
            FormTypr = info._formType;
            ZoomFactor = 1;
            _viewOffset = new PointF();      
        }

#region 事件处理函数

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.ScaleTransform(ZoomFactor, ZoomFactor);
            g.TranslateTransform(_viewOffset.X, _viewOffset.Y);
            ShowView.DrawView(g);
        }

#endregion 事件处理函数
    }

}
