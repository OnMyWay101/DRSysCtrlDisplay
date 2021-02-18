using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DRSysCtrlDisplay
{
    /// <summary>
    /// 该名称空间定义了所有的共用信息，作为其他文件使用的原则
    /// </summary>
    namespace Princeple
    {
        /// <summary>
        /// 表示各种显示窗口的类型
        /// </summary>
        public enum FormType
        {
            None,           //无效的窗体种类
            PPC,            //PowerPC图像显示界面
            FPGA,           //FPGA图像显示界面
            ZYNQ,           //ZYNQ图像显示界面
            BOARD,          //板卡
            BACKPLANE,      //背板
            CONTIANER,      //机箱
            SYSTEM,         //系统
            COMPONENT,      //构件
            APPLICATION,    //应用
            TOPO,           //资源池拓扑图
            STATUS,         //资源池状态图
            APP             //资源池应用
        };

        /// <summary>
        /// 表示各个计算颗粒现在处于的状态
        /// </summary>
        public enum NodeStatus
        {
            None,
            OnLine,     //在线
            Used,       //节点被选做为计算颗粒
            OffLine,    //节点掉线
            Error       //节点异常
        }

        /// <summary>
        /// 一个用来定义计算颗粒对应颜色的类
        /// </summary>
        public static class ComputeNodeColor
        {
            public static Pen Pen_None { get { return Pens.Gray; } }
            public static Pen Pen_PPC { get { return Pens.Blue; } }
            public static Pen Pen_FPGA { get { return Pens.Red; } }
            public static Pen Pen_PL { get { return Pens.Purple; } }
            public static Pen Pen_PS { get { return Pens.GreenYellow; } }

            public static Brush Brushes_None { get { return Brushes.Gray; } }
            public static Brush Brushes_PPC { get { return Brushes.Blue; } }
            public static Brush Brushes_FPGA { get { return Brushes.Red; } }
            public static Brush Brushes_PL { get { return Brushes.Purple; } }
            public static Brush Brushes_PS { get { return Brushes.GreenYellow; } }
        }

        /// <summary>
        /// 连接区域应该显示的颜色
        /// </summary>
        public static class ConnectAreaColor
        {
            public static Pen Pen_EtherNet { get { return Pens.Orange; } }
            public static Pen Pen_RapidIO { get { return Pens.Gray; } }
            public static Pen Pen_GTX { get { return Pens.Green; } }
            public static Pen Pen_LVDS { get { return Pens.LightBlue; } }

            public static Brush Brushes_EtherNet { get { return Brushes.Orange; } }
            public static Brush Brushes_RapidIO { get { return Brushes.Gray; } }
            public static Brush Brushes_GTX { get { return Brushes.Green; } }
            public static Brush Brushes_LVDS { get { return Brushes.LightBlue; } }
        }

        /// <summary>
        /// 一条Link的连接端点类型
        /// </summary>
        public enum EndType
        {
            None,
            PPC,
            FPGA,
            ZYNQ,
            SW,
            VPX
        }

        /// <summary>
        /// 一条Link的类型
        /// </summary>
        public enum LinkType
        {
            None,
            EtherNet,
            RapidIO,
            GTX,
            LVDS
        }

        public enum LinkLanes
        {
            None,
            X1 = 1,
            X2 = 2,
            X4 = 4
        }

        public enum VpxCategory
        {
            Board,          //板卡
            EmptySlot,       //空槽位
            BackPlane,      //背板
            IndicateArea    //区域说明
        }


        public static class TypeConvert
        {
            public static Type GetEndType(EndType eType)
            {
                switch (eType)
                {
                    case EndType.PPC:
                        return typeof(PPCViewModel);
                    case EndType.FPGA:
                        return typeof(FPGAViewModel);
                    case EndType.ZYNQ:
                        return typeof(ZYNQViewModel);
                    default:
                        return null;
                }
            }
        }


        /// <summary>
        /// 把一些约定的值与string做相关转化
        /// </summary>
        public static class StringConvert
        {
            public static List<string> GetLinkType_StringList()
            {
                var list = new List<string>();
                list.Add(LinkType.EtherNet.ToString());
                list.Add(LinkType.RapidIO.ToString());
                list.Add(LinkType.GTX.ToString());
                list.Add(LinkType.LVDS.ToString());
                return list;
            }

            public static List<string> GetLinkLanes_StringList()
            {
                var list = new List<string>();
                list.Add(LinkLanes.X1.ToString());
                list.Add(LinkLanes.X2.ToString());
                list.Add(LinkLanes.X4.ToString());
                return list;
            }

            public static List<string> GetComputeNodeType_StringList()
            {
                var list = new List<string>();
                list.Add(EndType.PPC.ToString());
                list.Add(EndType.ZYNQ.ToString());
                list.Add(EndType.FPGA.ToString());
                return list;
            }
        }
    }
}
