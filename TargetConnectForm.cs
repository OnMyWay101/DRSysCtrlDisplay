using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MultiCastPacket = DRSysCtrlDisplay.TargetListener.MultiCastPacket;

namespace DRSysCtrlDisplay
{
    public partial class TargetConnectForm : Form
    {
        public event Action<Dictionary<IPAddress, MultiCastPacket>> TargetInfoComed;        //目标机状态改变的事件

        private const int _listenPeriod = 100;  //进行监听的周期时间100ms
        public static TargetConnectForm Instance = new TargetConnectForm();
        TargetListener _listener;
        public ArrayList ChoosedTargets { get; private set; }
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        private TargetConnectForm()
        {
            InitializeComponent();
            ChoosedTargets = new ArrayList();

            //对listView属性进行设置
            this.listView1.View = View.Details;
            this.listView1.CheckBoxes = true;
            this.listView1.GridLines = true;
            listView1.Columns.Add("IP地址", -2, HorizontalAlignment.Center);

            //注册事件委托
            _timer.Tick += new EventHandler((o, e) => UpdataList());
            _yesBtn.Click += new EventHandler(_yesBtn_Click);
            _cancelBtn.Click += new EventHandler((o, e) => DialogResult = DialogResult.Cancel);

            //开启监听广播包的任务
            BeginListen();
        }

        public void ShowForm()
        {
            //开启Timer
            _timer.Interval = _listenPeriod;
            _timer.Start();

            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void CloseForm()
        {
            _timer.Stop();
            this.Hide();
        }

        public void EndListen()
        {
            _listener.EndListen();
        }

        private Dictionary<IPAddress, MultiCastPacket> GetChoosedTargetInfo()
        {
            Dictionary<IPAddress, MultiCastPacket> selectedInfos = new Dictionary<IPAddress, MultiCastPacket>();
            Dictionary<IPAddress, MultiCastPacket> allTargetInfos = _listener.GetTargets();
            foreach (var tInfo in allTargetInfos)
            {
                if (ChoosedTargets.Contains(tInfo.Key))    //录入选择的目标机信息
                {
                    selectedInfos.Add(tInfo.Key, tInfo.Value);
                }
            }
            return selectedInfos;
        }

        /// <summary>
        /// 初始里“取消”按键的点击事件：给choosedTargetList做更新，关闭窗体；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _yesBtn_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    ChoosedTargets.Add(IPAddress.Parse(item.Text));
                }
            }
            DialogResult = DialogResult.Yes;
        }

        /// <summary>
        /// 更新目标机在ListView的显示
        /// </summary>
        private void UpdataList()
        {
            var targets = _listener.GetTargets();
            ChoosedTargets.Clear();
            //先记录哪些ip对象是被选中的
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    ChoosedTargets.Add(IPAddress.Parse(item.Text));
                }
            }
            listView1.Items.Clear();
            foreach (var t in targets)
            {
                ListViewItem item = new ListViewItem(t.Key.ToString());
                if (ChoosedTargets.Contains(t.Key))
                {
                    item.Checked = true;
                }
                listView1.Items.Add(item);
            }
        }

        private void OnPacketArrived()
        {
            //发出事件
            if (TargetInfoComed != null)
            {
                TargetInfoComed(GetChoosedTargetInfo());
            }
        }

        /// <summary>
        /// TODO:这个listener本应该放在Main里面的创建并执行监听，现在为了调试方便暂时方面该类里面
        /// </summary>
        private void BeginListen()
        {
            _listener = new TargetListener();
            _listener.BeginListen();
            _listener.PacketArrived += new Action(OnPacketArrived);
        }
    }

    /// <summary>
    /// 提供组播目标的监听功能
    /// </summary>
    public class TargetListener
    {
        private const int _liveTime = 5000;     //目标机的存活有效时间5000ms
        private const int _sockPort = 20101;     //默认使用端口
        private const string _multiCaseIp = "230.20.10.19";

        public event Action PacketArrived;
        UdpClient udpClient;
        Boolean _running;   //是否一直在接收组播信息
        Thread _thread;
        Dictionary<IPAddress, MultiCastPacket> _targets = new Dictionary<IPAddress, MultiCastPacket>();
        Dictionary<IPAddress, DateTime> _liveTimes = new Dictionary<IPAddress, DateTime>();
        Mutex _mutex;       //互斥锁，主要用于互斥对_targets的访问

        public TargetListener()
        {
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, _sockPort));//广播设置

            _thread = new Thread(new ThreadStart(DoListenWork));
            _mutex = new Mutex();
        }

        public void BeginListen()
        {
            _thread.Start();
            _running = true;
        }

        public void EndListen()
        {
            _running = false;
            _thread.Join();
            udpClient.Close();
        }

        /// <summary>
        /// 获取在线目标机的IP信息及其对应的广播报文包的信息
        /// </summary>
        /// <returns>返回的是新建实例</returns>
        public Dictionary<IPAddress, MultiCastPacket> GetTargets()
        {
            _mutex.WaitOne();
            //滤掉超时的成员
            var rKeysList = _liveTimes.Where(timePair => (DateTime.Now - timePair.Value) > TimeSpan.FromSeconds(5))
                .Select(timePair => timePair.Key).ToList();

            for (int i = 0; i < rKeysList.Count; i++)
            {
                _liveTimes.Remove(rKeysList[i]);
                _targets.Remove(rKeysList[i]);
            }
            var result = new Dictionary<IPAddress, MultiCastPacket>(_targets);
            _mutex.ReleaseMutex();
            return result;
        }

        /// <summary>
        /// 实现对目标机的筛选功能
        /// </summary>
        private void FilterTarget()
        {

        }

        /// <summary>
        /// listen的工作循环
        /// </summary>
        private void DoListenWork()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 0);

            while (_running)
            {
                if (udpClient.Available > 0)
                {
                    var data = udpClient.Receive(ref iep);
                    //PrintBroadInfo(data);
                    _mutex.WaitOne();
                    //更新目标机对应的组播数据
                    if (!_targets.ContainsKey(iep.Address))
                    {
                        _targets.Add(iep.Address, new MultiCastPacket(data));
                        //测试使用保存一份广播数据
                        //using (var fs = new FileStream(@"C: \Users\wangcj.SINODSP\Desktop\MhalData\Mhal广播数据.txt", FileMode.Create))
                        //{
                        //    if (fs.CanWrite)
                        //    {
                        //        fs.Write(data, 0, data.Length);
                        //        fs.Flush();
                        //    }
                        //}
                    }
                    else
                    {
                        _targets[iep.Address] = new MultiCastPacket(data);
                    }

                    //更新目标机对应的最近收到的组播时间
                    if (_liveTimes.ContainsKey(iep.Address))
                    {
                        _liveTimes[iep.Address] = DateTime.Now;
                    }
                    else
                    {
                        _liveTimes.Add(iep.Address, DateTime.Now);
                    }
                    _mutex.ReleaseMutex();
                    //发出收到组播包事件
                    if (PacketArrived != null)
                    {
                        PacketArrived();
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void PrintBroadInfo(byte[] data)
        {
            Console.WriteLine("SaveInFile:");
            for (int i = 0; i < data.Length; i++)
            {
                if (i % 30 == 0)
                {
                    Console.WriteLine(" ");
                }
                Console.Write(Convert.ToString(data[i], 16) + " ");
            }
        }

        public class MultiCastPacket
        {
            public bool contentValid { get; private set; } //数据包内容是否有效
            public MultiCastPacketInfo.SystemInformation SysInfo { get; private set; }//组播包包含的信息
            public byte[] data { get; private set; }

            public MultiCastPacket(byte[] packet)
            {
                data = new byte[packet.Length];
                packet.CopyTo(data, 0);
                //Todo
                contentValid = false;
                if (Marshal.SizeOf(SysInfo) <= packet.Length)
                {
                    contentValid = true;
                    SysInfo = MultiCastPacketInfo.SystemInformation.Bytes2Struct(packet);
                }
            }

            public override string ToString()
            {
                if (contentValid)
                {
                    return SysInfo.ToString();
                }
                else
                {
                    return "收到的该数据包内容无效! \n";
                }
            }

            public class MultiCastPacketInfo
            {
                private const uint SysInfoTransFlag = 0xF8F7F6F5;
                private const int SysInfoTypeLen = 24;
                private const int SysInfoMaxSlotNum = 6;
                private const int SysInfoBufLen = 0;

                private const string SysInfoCaseTypr = "KCGJD";
                private const uint SysInfoCaseVer = 0x100;
                private const int SysInfoReportInt = 2;

                //仅测试使用
                public static void TestStructParse()
                {
                    MultiCastPacketInfo.BoardInformation boardInfo;
                    MultiCastPacketInfo.SystemInformation sysInfo;

                    byte[] boardArray = new byte[Marshal.SizeOf(typeof(MultiCastPacketInfo.BoardInformation))];
                    for (int i = 0; i < boardArray.Length; i++)
                    {
                        boardArray[i] = (byte)i;
                    }

                    byte[] sysArray = new byte[Marshal.SizeOf(typeof(MultiCastPacketInfo.SystemInformation))];
                    for (int i = 0; i < sysArray.Length; i++)
                    {
                        sysArray[i] = (byte)i;
                    }

                    boardInfo = MultiCastPacketInfo.BoardInformation.Bytes2Struct(boardArray);
                    sysInfo = MultiCastPacketInfo.SystemInformation.Bytes2Struct(sysArray);
                }

                //组播信息即系统信息
                [Serializable]//表示可序列化
                [StructLayout(LayoutKind.Sequential, Pack = 1)]//按1字节对齐
                public struct SystemInformation
                {
                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _flag;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _caseId;

                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SysInfoTypeLen)]//24个byte元素的数组成员
                    public byte[] _boardType;//TODO:初始化

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _caseVer;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _mgrCtrlSlot;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _slotNum;

                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SysInfoMaxSlotNum, ArraySubType = UnmanagedType.Struct)]//6个结构体元素的数组成员
                    public BoardInformation[] _boardsInfo;

                    public static SystemInformation Bytes2Struct(byte[] bytes)
                    {
                        //转化为本结构体，并调整字节序
                        var retStruct = (SystemInformation)Utilities.ByteStructConvert<SystemInformation>.BytesToStruct(bytes);

                        //调整子结构体的字节序
                        for (int i = 0; i < retStruct._boardsInfo.Length; i++)
                        {
                            var curBoardInfo = retStruct._boardsInfo[i];
                            var boardBytes = Utilities.ByteStructConvert<BoardInformation>.StructToBytes(curBoardInfo);
                            retStruct._boardsInfo[i] = BoardInformation.Bytes2Struct(boardBytes);
                        }

                        return retStruct;
                    }

                    public override string ToString()
                    {
                        string retString = string.Empty;
                        retString += string.Format("\n_flag:0x{0:x}\n", this._flag);
                        retString += string.Format("_caseId:{0}\n", this._caseId);

                        retString += string.Format("_boardType:{0} \n", Encoding.ASCII.GetString(_boardType));
                        retString = retString.Split(new char[] { '\0' })[0];

                        retString += string.Format("\n_caseVer:0x{0:x}\n", this._caseVer);
                        retString += string.Format("_mgrCtrlSlot:{0}\n", this._mgrCtrlSlot);
                        retString += string.Format("_slotNum:{0}\n", this._slotNum);

                        foreach (var bInfo in _boardsInfo)
                        {
                            retString += bInfo.ToString();
                        }

                        return retString;
                    }
                }

                //组播信息里面的板卡信息
                [Serializable]//表示可序列化
                [StructLayout(LayoutKind.Sequential, Pack = 1)]//按1字节对齐
                public struct BoardInformation
                {
                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _isOnline;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _slotId;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _status;

                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SysInfoTypeLen)]//大小24字节
                    public byte[] _boardType;//TODO:初始化

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public int _boardVer;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public float _temp;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public float _vol;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public float _cur;

                    [Utilities.EndianAttribute(Utilities.Endianness.BigEndian)]
                    public float _power;

                    public static BoardInformation Bytes2Struct(byte[] bytes)
                    {
                        return (BoardInformation)Utilities.ByteStructConvert<BoardInformation>.BytesToStruct(bytes);
                    }

                    public override string ToString()
                    {
                        string retString = string.Empty;
                        retString += string.Format("\n_isOnline:{0}\n", this._isOnline);
                        retString += string.Format("_slotId:{0}\n", this._slotId);
                        retString += string.Format("_status:{0}\n", this._status);

                        retString += string.Format("_boardType:{0} \n", Encoding.ASCII.GetString(_boardType));
                        retString = retString.Split(new char[] { '\0' })[0];

                        retString += string.Format("\n_boardVer:{0}\n", this._boardVer);
                        retString += string.Format("_temp:{0}\n", this._temp);
                        retString += string.Format("_vol:{0}\n", this._vol);
                        retString += string.Format("_cur:{0}\n", this._cur);
                        retString += string.Format("_power:{0}\n", this._power);

                        return retString;
                    }
                }
            }
        }

    }

}

