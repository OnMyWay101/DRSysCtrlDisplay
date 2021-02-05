using DRSysCtrlDisplay.Princeple;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DRSysCtrlDisplay.ViewModel.Others
{
    /// <summary>
    /// 用于表示一条板卡芯片或者VPX的连接；
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BoardLink : BaseLine
    {
        [Description("端1类型")]
        public EndType FirstEndType { get; set; }  //端点1的类型

        [Description("端2类型")]
        public EndType SecondEndType { get; set; }

        [Description("端1位置")]
        private int FirstEndPositionArray { get { return FirstEndPositionList[0]; } }

        [Description("端2位置")]
        private int SecondEndPositionArray { get { return SecondEndPositionList[0]; } }

        //@Todo:后面判断一条Link如果加上LaneNum带宽后是否需要该List
        //端点1的位置清单： 如果EndType不为VPX，则指的是端口号，一般都是一个；
        //                  如果EndType为VPX,则位置可能有多个，对应了VPX的
        //                  {P0:8,P1:16,P2:16,P3:16,P4:16,P5:16,P6:16,}104个位置,使用int:0-103表示
        public List<int> FirstEndPositionList { get; set; }
        public List<int> SecondEndPositionList { get; set; }

        public BoardLink()
        {
            InitSelf();
        }

        public BoardLink(LinkType type, int firstId, int secondId)
            : base(type, firstId, secondId, LinkLanes.X1)
        {
            InitSelf();
        }

        private void InitSelf()
        {
            FirstEndPositionList = new List<int>();
            SecondEndPositionList = new List<int>();
        }

        public override void DrawLine(Graphics graph, List<Point> line) { }
    }
}
