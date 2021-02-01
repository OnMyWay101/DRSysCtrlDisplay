using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DRSysCtrlDisplay
{
    public partial class AboutForm : Form
    {
        string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }
        string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((AssemblyDescriptionAttribute)attributes[0]).Description;
                }
                return "";
            }
        }
        string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((AssemblyCompanyAttribute)attributes[0]).Company;
                }
                return "";
            }
        }
        public AboutForm()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            int firstY = 10;    //第一个标签的起始Y坐标
            int offsetY = 40;   //标签的Y坐标便宜
            this.label2.Location = new System.Drawing.Point(5, firstY);//名称
            this.label2.Size = new System.Drawing.Size(41, 12);

            this.label3.Location = new System.Drawing.Point(5, firstY + offsetY);//版本
            this.label3.Size = new System.Drawing.Size(41, 12);

            this.label4.Location = new System.Drawing.Point(5, firstY + offsetY * 2);//描述
            this.label4.Size = new System.Drawing.Size(41, 12);

            this.label5.Location = new System.Drawing.Point(5, firstY + offsetY * 3);//所有权
            this.label5.Size = new System.Drawing.Size(53, 12);

            this.label9.Location = new System.Drawing.Point(59, firstY);//名称内容
            this.label9.Size = new System.Drawing.Size(41, 12);

            this.label8.Location = new System.Drawing.Point(59, firstY + offsetY);//版本内容
            this.label8.Size = new System.Drawing.Size(41, 12);

            this.label7.Location = new System.Drawing.Point(59, firstY + offsetY * 2);//描述内容
            this.label7.Size = new System.Drawing.Size(41, 12);

            this.label6.Location = new System.Drawing.Point(59, firstY + offsetY * 3);//所有权内容
            this.label6.Size = new System.Drawing.Size(41, 12);

            this.StartPosition = FormStartPosition.CenterParent;

            SetContent();
        }

        private void SetContent()
        {
            label9.Text = AssemblyTitle;
            label8.Text = AssemblyVersion;
            label7.Text = AssemblyDescription;
            label6.Text = AssemblyCompany;
        }
    }
}
