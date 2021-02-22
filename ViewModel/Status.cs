using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using SystemInformation = DRSysCtrlDisplay.TargetListener.MultiCastPacket.MultiCastPacketInfo.SystemInformation;

namespace DRSysCtrlDisplay
{
    public struct SlotInfo
    {
        public Rectangle _slotChantRec; //槽位号显示矩形
        public Rectangle _slotShowRec;  //槽位矩形
        public int slotChan;             //槽位号
        public string _type;            //板卡型号
        public float _temp;             //温度
        public float _voltage;          //电压
        public float _electric;         //电流
        public float _power;            //功率
        public string _status;          //状态
    };

    public class Status : BaseDrawer
    {
        [Category("槽位信息"), Description("槽位号")]
        public string SlotChan { get; set; }
        [Category("板卡信息"), Description("板卡型号")]
        public string Type { get; set; }
        [Category("板卡信息"), Description("温度")]
        public string Temp { get; set; }
        [Category("板卡信息"), Description("电压")]
        public string Voltage { get; set; }
        [Category("板卡信息"), Description("电流")]
        public string Electric { get; set; }
        [Category("板卡信息"), Description("功率")]
        public string Power { get; set; }
        [Category("板卡信息"), Description("状态")]
        public string BoardStatus { get; set; }
        [Category("机箱板卡数量"), Description("板卡总数")]
        public int _slotNum { get; set; }

        private Dictionary<int, SlotInfo> _statusInfo;  //槽位及对应矩形表
        private DrawStatus _drawStatus;

        public Status()
        {
            _slotNum = 10;
        }

        public Status( Rectangle rect)
        {
            _slotNum = 10;
            Init(rect);
        }

        public Status(Dictionary<int, SlotInfo> info, Rectangle rect)
        {
            _statusInfo = info;
            _slotNum = 10;
            Init(rect);
        }

        public override void Init(Rectangle rect)
        {
            base.Init(rect);
            Type = string.Empty;
            Temp = string.Empty;
            Voltage = string.Empty;
            Electric = string.Empty;
            Power = string.Empty;
            BoardStatus = string.Empty;
            _statusInfo = null;
            _drawStatus = null;
        }

        //public override void DrawView()
        //{
        //    int staWidth = 600;    //机箱状态视图默认宽度 
        //    int staHeight = 600;    //机箱状态视图默认高度 
        //    int staSlotNum = 10;    //机箱的默认槽位

        //    Rectangle rtg = new Rectangle(200, 20, staWidth, staHeight * _slotNum / staSlotNum);
        //    base._graph.DrawRectangle(Pens.Black, rtg);

        //    _drawStatus = new DrawStatus(this, base._graph, rtg);
        //    StatusUpdate(_drawStatus, _statusInfo);
        //    _drawStatus.Draw();
        //}

        public override void DrawView(Graphics g)
        {
            g.DrawRectangle(Pens.Black, base._rect);
            DrawStatus ds = new DrawStatus(this, g, base._rect);
            _drawStatus = ds;
            ds.Draw();         
        }

        public override Size GetViewSize()
        {
            int defaultHeight = 600;    //机箱状态视图默认高度 
            int defaultSlotNum = 10;    //机箱的默认槽位

            return new Size(600, defaultHeight * _slotNum / defaultSlotNum);
        }


        public void OnNodeInfoChanged(TargetNode tNode)
        {
            Dictionary<int, SlotInfo> infos = new Dictionary<int, SlotInfo>();   //当前的健康信息数据集合
            //处理数据
            ProcessNodeInfo(tNode.MultiPacket.SysInfo, infos);
            _statusInfo = infos;
            base.TriggerRedrawRequst();
        }

        private void ProcessNodeInfo(SystemInformation sysInfo, Dictionary<int, SlotInfo> infos)
        {
            try
            {
                //解析数据
                int slotNum = sysInfo._slotNum;       //槽位数
                _slotNum = slotNum;
                for (int i = 0; i < slotNum; i++)
                {
                    var slotInfo = sysInfo._boardsInfo[i];
                    int slotSn = i + 1;
                    
                    SlotInfo sInfo = new SlotInfo();
                    sInfo.slotChan = slotSn;
                    sInfo._type = "板卡" + slotSn.ToString();
                    if (slotInfo._isOnline == 1) //在线
                    {
                        sInfo._temp = slotInfo._temp;
                        sInfo._voltage = slotInfo._vol;
                        sInfo._electric = slotInfo._cur;
                        sInfo._power = slotInfo._power;
                        sInfo._status = IsException(sInfo) ? "异常" : "正常";
                    }
                    else
                    {
                        sInfo._status = "异常";
                    }
                    infos.Add(i, sInfo);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ProcessNodeInfo:" + e.Message);
                return;
            }
        }

        //判断信息是都异常
        private bool IsException(SlotInfo info)
        {
            return false;
        }

        public int StatusUpdate(DrawStatus ds,Dictionary<int, SlotInfo> info)
        {
            if (ds == null || info == null)
            {
                return -1;
            }
            ds.SetDic(info);           
            return 0;
        }

        /// <summary>
        /// 机箱状态视图绘图类
        /// </summary>
        public class DrawStatus
        {
            private Status _status;
            private Graphics _dsG;
            private Rectangle _dsRec;
            private Dictionary<int, SlotInfo> _listSlots;//槽位及对应矩形表

            public DrawStatus() { }

            public DrawStatus(Status st, Graphics parentG, Rectangle rtg)
            {
                _status = st;
                _dsG = parentG;
                _dsRec = rtg;
                if (_status._statusInfo == null)
                {
                    _listSlots = new Dictionary<int, SlotInfo>();
                    for (int i = 0; i < st._slotNum; i++)
                    {
                        _listSlots.Add(i, new SlotInfo());
                    }
                }
                else
                {
                    _listSlots = _status._statusInfo;
                }
            }

            /// <summary>
            /// 机箱槽位图像初始化
            /// </summary>
            public void Draw()
            {
                Rectangle link;
                string statusStr = string.Empty;

                AssignSloteRects(_status._slotNum, out link);
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                drawFormat.LineAlignment = StringAlignment.Center;
                foreach (KeyValuePair<int, SlotInfo> kvp in _listSlots)
                {
                    //画槽位号矩形
                    _dsG.FillRectangle(Brushes.SkyBlue, kvp.Value._slotChantRec);
                    //画槽位
                    if (kvp.Value._status == "正常")
                    {
                        _dsG.FillRectangle(Brushes.Green, kvp.Value._slotShowRec);
                    }
                    else 
                    {
                        _dsG.FillRectangle(Brushes.Red, kvp.Value._slotShowRec);
                    }
                    
                    //槽位号标识
                    _dsG.DrawString("槽位" + kvp.Value.slotChan .ToString(),
                        new Font("Arial", kvp.Value._slotChantRec.Width / 6),
                        Brushes.Black, kvp.Value._slotChantRec, drawFormat);
                    //槽位状态
                    statusStr = String.Format("{0,-6:G} | {1:F1}℃ | {2:F1}V | {3:F1}A | {4:F1}W | {5,4:G} ",
                        kvp.Value._type,
                        kvp.Value._temp,
                        kvp.Value._voltage,
                        kvp.Value._electric,
                        kvp.Value._power,
                        kvp.Value._status);
                    _dsG.DrawString(statusStr, new Font("Arial", 12),
                        Brushes.Black, kvp.Value._slotShowRec, drawFormat);
                }
                _dsG.FillRectangle(Brushes.Gray, link);
            }

            /// <summary>
            /// 状态视图各矩形生成
            /// </summary>
            /// <param name="slotNum">槽位数量</param>
            /// <param name="link"></param>
            private void AssignSloteRects(int slotNum, out Rectangle link)
            {
                List<Rectangle> rtgs = new List<Rectangle>();//均分矩形

                int slotWidth = _dsRec.Width;
                int slotHeight = _dsRec.Height / slotNum;
                int rtgX = _dsRec.Location.X;
                int rtgY = _dsRec.Location.Y;
                int slotKeyX = 0;
                int slotKeyY = 0;
                int countNum = 0;

                for (int i = 0; i < slotNum; i++)
                {
                    rtgs.Add(new Rectangle(rtgX, rtgY + slotHeight * i, slotWidth, slotHeight));
                }

                slotWidth = _dsRec.Width * 2 / 3;
                slotHeight /= 2;
                foreach (var rec in rtgs)
                {
                    SlotInfo slotInfo = new SlotInfo();
                    rtgX = rec.Location.X + slotWidth / 4;
                    rtgY = rec.Location.Y + slotHeight / 4;
                    slotKeyX = rtgX - slotHeight * 2;
                    slotKeyY = rtgY + slotHeight / 4;

                    slotInfo._slotChantRec = new Rectangle(slotKeyX, slotKeyY, slotHeight, slotHeight / 2);
                    slotInfo._slotShowRec = new Rectangle(rtgX, rtgY, slotWidth, slotHeight);
                    slotInfo.slotChan = countNum + 1;

                    slotInfo._temp = _listSlots[countNum]._temp;
                    slotInfo._type = _listSlots[countNum]._type;
                    slotInfo._voltage = _listSlots[countNum]._voltage;
                    slotInfo._electric = _listSlots[countNum]._electric;
                    slotInfo._power = _listSlots[countNum]._power;
                    slotInfo._status = _listSlots[countNum]._status;

                    //_listSlots.Add(countNum, slotInfo);
                    _listSlots[countNum] = slotInfo;
                    countNum++;
                }
                link = new Rectangle(slotKeyX + slotHeight, _dsRec.Y, slotHeight, _dsRec.Height);
            }

            public void SetDic(Dictionary<int, SlotInfo> info)
            {
                for (int i = 0; i < info.Count; i++)
                {
                    _listSlots[i] = info[i];
                }
            }
        }
    }  
}
