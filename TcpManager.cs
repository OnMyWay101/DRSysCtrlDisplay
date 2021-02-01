using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DRSysCtrlDisplay
{
    //Tcp连接管理类
    public class TcpManager
    {
        public event Action<TargetTcpClient> TargetStatusChange;        //目标机状态改变的事件
        private const int TcpClientDefaultPort = 20102;                 //默认的tcp连接服务端端口号
        private bool _running = false;                                  //当前Tcp管理类的运行状态：false-未运行；true-在运行；
        //目标机管理类与之对应的IO管理器
        private Dictionary<TargetTcpClient, TcpClientIO> _clientIODir = new Dictionary<TargetTcpClient, TcpClientIO>();
        public static readonly TcpManager Instance = new TcpManager();  //管理类的实例（单例模式）
        private ITcpAutoCmdProc _autoCmdProcer = null;                  //自动发送命令处理器
        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();      //当多线程操作_clientIODir时候使用该读写锁

        public List<string> ipList
        {
            get
            {
                _rwLock.EnterReadLock();
                var clientsIp = _clientIODir.Select(clientIOPair => clientIOPair.Key.IpAddr.ToString()).ToList();
                _rwLock.ExitReadLock();
                return clientsIp;
            }
        }

        private TcpManager() { }

        public void StartWork()
        {
            if (_running)
            {
                return;
            }
            _running = true;
            ThreadPool.QueueUserWorkItem(IOWorking);
            ThreadPool.QueueUserWorkItem(ProcAutoCmd);
        }

        public void StopWork()
        {
            _running = false;
        }

        public void RegAutoCmdProc(ITcpAutoCmdProc autoCmdProc)
        {
            if (_autoCmdProcer == null)
            {
                _autoCmdProcer = autoCmdProc;
            }
        }

        /// <summary>
        /// 添加一个客户端到ClientManager,实现客户端连接到服务端(阻塞)；
        /// </summary>
        /// <param name="serverip"></param>
        public bool AddClient(string serverip)
        {
            //创建新的客户端对象
            var newClient = new TargetTcpClient(serverip, TcpClientDefaultPort);

            if (newClient.NetWork_Start())
            {
                var newClientIO = new TcpClientIO();

                _rwLock.EnterWriteLock();
                _clientIODir.Add(newClient, newClientIO);
                _rwLock.ExitWriteLock();
                //触发相关事件
                if (TargetStatusChange != null)
                {
                    TargetStatusChange(newClient);
                }
                return true;
            }
            return false;
        }

        public void DeleteClient(string serverip)
        {
            _rwLock.EnterWriteLock();
            var deleteClient = _clientIODir.Where(clientPair => clientPair.Key.IpAddr == IPAddress.Parse(serverip))
                .Select(clientPair => clientPair.Key).First();
            deleteClient.NetWork_Stop();
            _clientIODir.Remove(deleteClient);
            _rwLock.ExitWriteLock();
        }

        public void SendOneCmd(string serverip, ITcpCommand cmd)
        {
            _rwLock.EnterReadLock();
            var clientIO = _clientIODir.Where(clientPair => clientPair.Key.IpAddr.Equals(IPAddress.Parse(serverip)))
                .Select(clientPair => clientPair.Value).FirstOrDefault();
            _rwLock.ExitReadLock();
            clientIO.PushSendCmd(cmd);
        }

        /// <summary>
        /// 在固定ip上指定获取一个指定类型的命令回复
        /// </summary>
        /// <param name="serverip"></param>
        /// <param name="cmdCode"></param>
        /// <returns>收到的命令或者null</returns>
        public ITcpCommand RecvOneCmd(string serverip, CmdCode cmdCode)
        {
            _rwLock.EnterReadLock();
            var clientIO = _clientIODir.Where(clientPair => clientPair.Key.IpAddr.Equals(IPAddress.Parse(serverip)))
                .Select(clientPair => clientPair.Value).FirstOrDefault();
            _rwLock.ExitReadLock();
            if (clientIO == null)
            {
                return null;
            }
            return clientIO.GetRecvResponseCmd(cmdCode);
        }

        private void IOWorking(object stateInfo)
        {
            try
            {
                while (_running)
                {
                    _rwLock.EnterReadLock();
                    foreach (var clientPair in _clientIODir)
                    {
                        var curClient = clientPair.Key;     //获取当前的Client管理器
                        var curClientIO = clientPair.Value; //获取当前的ClientIO;
                        if (!curClient.Connected)
                        {
                            continue;
                        }
                        //发送调命令
                        var sendCmd = curClientIO.PopSendCmd();
                        if (sendCmd != null)
                        {
                            curClient.SendOneCmd(sendCmd);
                        }
                        //接收一条命令
                        curClient.RecvOneCmd();
                        var recvedCmd = curClient.GetOneCmd();
                        if (recvedCmd != null)
                        {
                            MainForm.SetOutPutText("RecvOneCmd:" + recvedCmd.GetCmdCode().ToString());
                            if (_autoCmdProcer != null)
                            {
                                //判断命令放是不是自发命令，不是则为相应命令
                                if (_autoCmdProcer.IsAutoCmd(recvedCmd))
                                {
                                    curClientIO.PushAutoCmd(recvedCmd);
                                }
                                else
                                {
                                    curClientIO.AddRecvResponseCmd(recvedCmd);
                                }
                            }
                        }
                    }
                    _rwLock.ExitReadLock();
                    Thread.Sleep(1);//让其他进程有机会运行,不会造成主界面过于卡顿
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("IOWorking:" + e.Message);
            }
        }

        private void ProcAutoCmd(object stateInfo)
        {
            try
            {
                while (_running)
                {
                    _rwLock.EnterReadLock();
                    foreach (var clientPair in _clientIODir)
                    {
                        var curClient = clientPair.Key;
                        var curClientIO = clientPair.Value;

                        var curCmd = curClientIO.PopAutoCmd();
                        if (_autoCmdProcer != null && curCmd != null)
                        {
                            _autoCmdProcer.ProcAutoCmd(curCmd);
                        }
                    }
                    _rwLock.ExitReadLock();
                    Thread.Sleep(1);//让其他进程有机会运行,不会造成主界面过于卡顿
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ProcAutoCmd:" + e.Message);
            }
        }

        /// <summary>
        /// 描述Tcp客户端接收和发送的命令集合
        /// </summary>
        public class TcpClientIO
        {
            private Queue<ITcpCommand> _sendCmdQueue = new Queue<ITcpCommand>();
            private List<ITcpCommand> _recvResponseCmdList = new List<ITcpCommand>();   //收到的回复信息
            private Queue<ITcpCommand> _recvAutoCmdQueue = new Queue<ITcpCommand>();     //收到的自发信息

            public ITcpCommand PopSendCmd()
            {
                lock (_sendCmdQueue)//Todo:可以验证这种Lock方式是否正确
                {
                    if (_sendCmdQueue.Count > 0)
                    {
                        return _sendCmdQueue.Dequeue();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public void PushSendCmd(ITcpCommand tcpCmd)
            {
                lock (_sendCmdQueue)
                {
                    _sendCmdQueue.Enqueue(tcpCmd);
                }
            }

            public ITcpCommand GetRecvResponseCmd(CmdCode cmdCode)
            {
                ITcpCommand resultTcpCmd = _recvResponseCmdList
                    .Where(tcpCmd => tcpCmd.GetCmdCode() == cmdCode).FirstOrDefault();
                if (resultTcpCmd == null)
                {
                    return null;
                }
                _recvResponseCmdList.Remove(resultTcpCmd);
                return resultTcpCmd;
            }

            public void AddRecvResponseCmd(ITcpCommand tcpCmd)
            {
                _recvResponseCmdList.Add(tcpCmd);
            }

            public ITcpCommand PopAutoCmd()
            {
                lock (_recvAutoCmdQueue)
                {
                    if (_recvAutoCmdQueue.Count > 0)
                    {
                        return _recvAutoCmdQueue.Dequeue();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public void PushAutoCmd(ITcpCommand tcpCmd)
            {
                lock (_recvAutoCmdQueue)//Todo:可以验证这种Lock方式是否正确
                {
                    _recvAutoCmdQueue.Enqueue(tcpCmd);
                }
            }
        }

        public class TargetTcpClient
        {
            const int ConnectTimeOutMs = 4000; //Client连接服务器的等待时间

            TcpClient _client;
            SendPacket _sendPacket = new SendPacket();
            RecvPacket _recvPacket = new RecvPacket();

            public IPAddress IpAddr { get; private set; }
            int _port;

            public bool Connected { get { return _client.Connected; } }
            byte[] _recvBuf;            //接收缓存区
            int _recvLen;               //需要接收的长度
            int _recvedCount;           //已经接收的长度

            //思考：可以把命令记录和生成放在一个接口类里面以实现解耦
            Queue _recvedCmdQueue = Queue.Synchronized(new Queue());
            Dictionary<UInt32, CmdCode> _CmdIdToCodeDir = new Dictionary<uint, CmdCode>(); //命令到序号到命令类型的映射

            public TargetTcpClient(string serverip, int port)
            {
                _port = port;
                IpAddr = IPAddress.Parse(serverip);
            }

            //连接服务器
            public bool NetWork_Start()
            {
                _client = new TcpClient();

                IAsyncResult ar = _client.BeginConnect(IpAddr, _port, null, null);
                if (ar.AsyncWaitHandle.WaitOne(ConnectTimeOutMs, false))
                {
                    return Connected;
                }
                else
                {
                    NetWork_Stop();
                    return false;
                }
            }

            //断开服务器
            public void NetWork_Stop()
            {
                _client.Close();
                _client = null;
            }

            //发送一条命令
            public void SendOneCmd(ITcpCommand tcpCmd)
            {
                int oneSendSizeMax = 1024;      //每次发送的最大大小
                int processCount = 1;           //过程计数，1个单位表示10%
                byte[] sendBuf = new byte[oneSendSizeMax];

                _sendPacket.FreshPacket(tcpCmd);
                var packetStream = _sendPacket.GetNetStream();
                var netStream = _client.GetStream();
                //记录该条发送的命令号与命令码的绑定
                _CmdIdToCodeDir.Add(_sendPacket.GetCurrentCmdId(), tcpCmd.GetCmdCode());

                MainForm.SetOutPutText(String.Format("SendOneCmd:start to send:cmd={0},size={1}", tcpCmd.GetCmdCode(), packetStream.Length));
                while (true)
                {
                    //发送数据
                    int retSize = packetStream.Read(sendBuf, 0, oneSendSizeMax);
                    if (retSize == 0)
                    {
                        break;
                    }
                    netStream.Write(sendBuf, 0, retSize);
                    //打印传输的进度
                    if (tcpCmd.GetCmdCode() == CmdCode.UpLoadApp)
                    {
                        if (packetStream.Position == (packetStream.Length / 1024) * processCount / 10 * 1024)
                        {
                            MainForm.SetOutPutText(string.Format("SendOneCmd:process {0}%", processCount * 10));
                            processCount++;
                        }
                    }
                }
                MainForm.SetOutPutText("SendOneCmd:send over");
                //MainForm.SetOutPutText(_sendPacket.ToString());
            }

            //异步接收
            public void RecvOneCmd()
            {
                if (_recvBuf != null)//_recvBuf不为null表示TcpClien正处于接收当中；
                {
                    return;
                }
                _recvBuf = new byte[TcpPacket._HeadLength];
                _recvedCount = 0;
                _recvLen = TcpPacket._HeadLength;
                BeginRecv();
            }

            //获取一个命令的信息
            public ITcpCommand GetOneCmd()
            {
                lock (_recvedCmdQueue)
                {
                    if (_recvedCmdQueue.Count > 0)
                    {
                        return (ITcpCommand)_recvedCmdQueue.Dequeue();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            private void BeginRecv()
            {
                var netStream = _client.GetStream();
                netStream.BeginRead(_recvBuf, _recvedCount, _recvLen - _recvedCount, ReadAsyncCallBack, null);
            }

            private void ReadAsyncCallBack(IAsyncResult result)
            {
                var netStream = _client.GetStream();
                _recvedCount += netStream.EndRead(result);//获得每次异步读取的数量

                //数量达到包头指定长度之后获取整包长度
                if (_recvedCount == TcpPacket._HeadLength)
                {
                    _recvLen = _recvPacket.GetLength(_recvBuf);
                    var temp = new byte[_recvLen];
                    _recvBuf.CopyTo(temp, 0);

                    _recvBuf = temp;
                }
                //数量达到指定长度后退出
                else if (_recvedCount == _recvLen)
                {
                    _recvPacket.FreshPacket(_recvBuf);
                    ProcessRecvPacket();
                    _recvBuf = null;
                    return;
                }
                //继续接收
                BeginRecv();
            }

            private void ProcessRecvPacket()
            {
                UInt32 recvedCmdId = _recvPacket.GetCurrentCmdId();
                byte[] recvedData = _recvPacket.GetCurrentCmdData();
                CmdCode recvedCmdCode = _CmdIdToCodeDir[recvedCmdId];
                _CmdIdToCodeDir.Remove(recvedCmdId);

                ITcpCommand repTcpCmd = TcpCommandConstructer.Create(recvedCmdCode, recvedData);

                lock (_recvedCmdQueue)
                {
                    _recvedCmdQueue.Enqueue(repTcpCmd);
                }
            }
        }

        public class TcpPacket //TCP数据包的基类
        {
            public const int _HeadLength = 12;  //包头包含12字节

            //数据包相关常量
            public const UInt32 _StartCode = 0x534D4350;
            public const UInt32 _Version = 0x10000;
            public const UInt32 _EndCode = 0x50434D53;

            //数据包内容
            protected UInt32 _Pack_StartCode;     //起始标识
            protected UInt32 _Pack_Version;       //协议版本
            protected UInt32 _Pack_Length;        //总长度
            protected UInt32 _Pack_CmdID;         //(响应)命令序号
            protected byte[] _Pack_Data;          //数据
            protected UInt32 _Pack_EndCode;       //结束标识
        }

        public class RecvPacket : TcpPacket
        {
            private const int _minSize = 24;//收的数据包最小为24字节
            private UInt32 _lastCmdId = UInt32.MaxValue;      //上一次收到的CmdId
            private UInt32 _Pack_Result;    //命令处理结果

            public void FreshPacket(Byte[] bytes)
            {
                ParsePacket(bytes);
            }

            public int GetLength(Byte[] headBytes)
            {
                var nsp = new Utilities.NetStreamProcess(headBytes);

                nsp.PopInt32();
                nsp.PopInt32();
                return nsp.PopInt32();
            }

            public UInt32 GetCurrentCmdId()
            {
                return base._Pack_CmdID;
            }

            public byte[] GetCurrentCmdData()
            {
                return base._Pack_Data;
            }

            private void ParsePacket(Byte[] bytes)
            {
                var nsp = new Utilities.NetStreamProcess(bytes);

                base._Pack_StartCode = (uint)nsp.PopInt32();
                base._Pack_Version = (uint)nsp.PopInt32();
                base._Pack_Length = (uint)nsp.PopInt32();

                base._Pack_CmdID = (uint)nsp.PopInt32();
                this._Pack_Result = (uint)nsp.PopInt32();
                base._Pack_Data = nsp.PopBytes((int)base._Pack_Length - _minSize);
                base._Pack_EndCode = (uint)nsp.PopInt32();

                _lastCmdId = base._Pack_CmdID;
            }

            public override string ToString()
            {
                string retString = "RecvPacket:\n";

                retString += string.Format("_Pack_StartCode:0x{0:x} \n", base._Pack_StartCode);
                retString += string.Format("_Pack_Version:0x{0:x} \n", base._Pack_Version);
                retString += string.Format("_Pack_Length:{0} \n", base._Pack_Length);
                retString += string.Format("_Pack_CmdID:0x{0:x} \n", base._Pack_CmdID);
                retString += string.Format("_Pack_Result:{0} \n", this._Pack_Result);

                retString += string.Format("_Pack_Data:length = {0} Content: \n", base._Pack_Length - _minSize);
                if (base._Pack_Data != null)
                {
                    foreach (var b in base._Pack_Data)
                    {
                        retString += string.Format("0x{0:x} ", b);
                    }
                    retString += "\n";
                }

                retString += string.Format("_Pack_EndCode:0x{0:x} \n", base._Pack_EndCode);

                return retString;
            }

        }

        public class SendPacket : TcpPacket
        {
            private const int _minSize = 28;                //发的数据包最小为28字节
            private const int _tcpCmdTailOffset = 4;        //命令数据在网络数据流的倒数4个字节前
            private UInt32 _cmdIdCount = 0;                 //命令刷新的计数
            private CmdCode _Pack_CmdCode;                  //命令码，大小4字节
            private Utilities.NetStreamProcess _netStream;  //网络数据流
            private ITcpCommand _tcpCmd;                    //命令数据

            public SendPacket()
            {
                base._Pack_StartCode = TcpPacket._StartCode;
                base._Pack_Version = TcpPacket._Version;
                base._Pack_EndCode = TcpPacket._EndCode;
            }

            public void FreshPacket(ITcpCommand tcpCmd)
            {
                base._Pack_Data = null;     //直接使用命令数据而不是数组
                if (tcpCmd.GetNetStream() == null)
                {
                    base._Pack_Length = (uint)(_minSize);
                }
                else
                {
                    base._Pack_Length = (uint)(_minSize + tcpCmd.GetNetStream().Length);
                }
                base._Pack_CmdID = _cmdIdCount++;
                this._Pack_CmdCode = tcpCmd.GetCmdCode();
                this._tcpCmd = tcpCmd;
                InitNetStream();
            }

            public UInt32 GetCurrentCmdId()
            {
                return base._Pack_CmdID;
            }

            private void InitNetStream()
            {
                _netStream = new Utilities.NetStreamProcess();
                var cmdStream = _tcpCmd.GetNetStream();

                _netStream.PushInt32((int)base._Pack_StartCode);
                _netStream.PushInt32((int)base._Pack_Version);
                _netStream.PushInt32((int)base._Pack_Length);
                _netStream.PushInt32((int)(base._Pack_CmdID));

                _netStream.PushInt32((int)this._Pack_CmdCode);
                _netStream.PushInt32(0);
                if (cmdStream != null)
                {
                    cmdStream.WriteTo(_netStream);
                }
                _netStream.PushInt32((int)base._Pack_EndCode);
            }

            public MemoryStream GetNetStream()
            {
                _netStream.Seek(0, SeekOrigin.Begin);
                return _netStream;
            }

            public override string ToString()
            {
                string retString = "SendPacket:\n";

                retString += string.Format("_StartCode:0x{0:x} \n", base._Pack_StartCode);
                retString += string.Format("_Version:0x{0:x} \n", base._Pack_Version);
                retString += string.Format("_Pack_Length:{0} \n", base._Pack_Length);
                retString += string.Format("_Pack_CmdID:0x{0:x} \n", base._Pack_CmdID);
                retString += string.Format("_Pack_CmdCode:0x{0:x} \n", this._Pack_CmdCode);

                retString += string.Format("_Pack_Data:length = {0} \n", base._Pack_Length - _minSize);
                retString += string.Format("_EndCode:0x{0:x}", base._Pack_EndCode);
                return retString;
            }
        }



    }
}
