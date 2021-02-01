using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRSysCtrlDisplay.Princeple;

namespace DRSysCtrlDisplay
{
    //Tcp通信命令的操作接口
    public interface ITcpCommand
    {
        CmdCode GetCmdCode();                                        //获取命令的命令码
        MemoryStream GetNetStream();                                //获取该命令的数据流
        bool DeserializeFromNet(byte[] netData);                    //通过网络数据形成类实体
    }

    public enum CmdCode
    {
        None,
        Heart = 0x1,
        RioTopoGet = 0x2,
        UpLoadApp = 0x3,
        StartApp = 0x4,
        StopApp = 0x5
    }

    public class TcpCommandConstructer
    {
        public static ITcpCommand Create(CmdCode cmdCode, byte[] netData)
        {
            ITcpCommand tcpCommand;

            switch (cmdCode)
            {
                case CmdCode.Heart:
                    tcpCommand = new TcpCommand_Heart();
                    break;
                case CmdCode.RioTopoGet:
                    tcpCommand = new TcpCommand_RioTopoGet();
                    break;
                case CmdCode.UpLoadApp:
                    tcpCommand = new TcpCommand_UpLoadApp();
                    break;
                case CmdCode.StartApp:
                    tcpCommand = new TcpCommand_StartApp();
                    break;
                case CmdCode.StopApp:
                    tcpCommand = new TcpCommand_StopApp();
                    break;
                default:
                    throw new System.ArgumentException("can't find the cmdCode", "cmdCode");
            }

            tcpCommand.DeserializeFromNet(netData);
            return tcpCommand;
        }
    }


    #region 具体的命令类

    //心跳类
    public class TcpCommand_Heart : ITcpCommand
    {
        private const CmdCode _cmdCode = CmdCode.Heart;

        public CmdCode GetCmdCode()
        {
            return _cmdCode;
        }

        public MemoryStream GetNetStream()
        {
            return null;
        }

        public bool DeserializeFromNet(byte[] netData)
        {
            return true;
        }
    }

    //RioTopoGet类
    public class TcpCommand_RioTopoGet : ITcpCommand
    {
        private const CmdCode _cmdCode = CmdCode.RioTopoGet;

        public CmdCode GetCmdCode()
        {
            return _cmdCode;
        }

        public MemoryStream GetNetStream()
        {
            return null;
        }

        public bool DeserializeFromNet(byte[] netData)
        {
            //等待协议
            return true;
        }
    }

    //UpLoadApp类
    public class TcpCommand_UpLoadApp : ITcpCommand
    {
        private const int FileNameMaxLength = 32;
        private const CmdCode _cmdCode = CmdCode.UpLoadApp;
        private string _fileName;
        private int _slot;
        private EndType _type;
        private Utilities.NetStreamProcess _netStream;  //网络数据流
        private FileStream _fileStream;
        //Todo:可以考虑使用MemoryStream来存储数据

        public TcpCommand_UpLoadApp()
        {

        }

        public TcpCommand_UpLoadApp(string file, int slot, EndType type, FileStream fs)
        {
            _fileName = file;
            _slot = slot;
            _type = type;
            _fileStream = fs;
            if (_fileName.Length >= FileNameMaxLength)
            {
                MainForm.SetOutPutText("TcpCommand_UpLoadApp:文件名太长！");
            }
            InitNetStream();
        }

        public CmdCode GetCmdCode()
        {
            return _cmdCode;
        }

        //初始化网络数据流
        private void InitNetStream()
        {
            int buffSize = 1024;
            byte[] tempBuf = new byte[buffSize];
            byte[] fileNameArray = new byte[FileNameMaxLength];
            _netStream = new Utilities.NetStreamProcess();

            try
            {
                byte[] fileBytes = System.Text.Encoding.ASCII.GetBytes(this._fileName);
                fileBytes.CopyTo(fileNameArray, 0);
                _netStream.PushBytes(fileNameArray);
                _netStream.PushInt32(this._slot);
                _netStream.PushInt32(this._type == EndType.FPGA ? 1 : 2);
                //把fileStream的内容添加到MemoryStream
                _fileStream.Seek(0, SeekOrigin.Begin);
                while (true)
                {
                    int retSize = _fileStream.Read(tempBuf, 0, buffSize);
                    if (retSize == 0)
                    {
                        break;
                    }
                    _netStream.PushBytes(tempBuf, retSize);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("InitNetStream:" + e.Message);
            }
        }

        public MemoryStream GetNetStream()
        {
            return _netStream;
        }

        public bool DeserializeFromNet(byte[] netData)
        {
            //等待协议
            return true;
        }
    }

    //StartApp类
    public class TcpCommand_StartApp : ITcpCommand
    {
        private const CmdCode _cmdCode = CmdCode.StartApp;

        public CmdCode GetCmdCode()
        {
            return _cmdCode;
        }

        public MemoryStream GetNetStream()
        {
            return null;
        }

        public bool DeserializeFromNet(byte[] netData)
        {
            //等待协议
            return true;
        }
    }

    //StopApp类
    public class TcpCommand_StopApp : ITcpCommand
    {
        private const CmdCode _cmdCode = CmdCode.StopApp;

        public CmdCode GetCmdCode()
        {
            return _cmdCode;
        }

        public MemoryStream GetNetStream()
        {
            return null;
        }

        public bool DeserializeFromNet(byte[] netData)
        {
            //等待协议
            return true;
        }
    }

    #endregion 具体的命令类


    //Tcp自发命令的处理接口
    public interface ITcpAutoCmdProc
    {
        bool IsAutoCmd(ITcpCommand tcpCmd);      //判断该命令是否是自发命令
        void ProcAutoCmd(ITcpCommand tcpCmd);    //处理一条自发命令 
    }

    //只把心跳作为服务端主动发出信息，并且只做记录处理
    public class TcpAutoCmdProcer_Log : ITcpAutoCmdProc
    {
        public bool IsAutoCmd(ITcpCommand tcpCmd)
        {
            var getCmd = tcpCmd.GetCmdCode();
            if (getCmd == CmdCode.Heart)
            {
                return true;
            }
            return false;
        }

        public void ProcAutoCmd(ITcpCommand tcpCmd)
        {
            MainForm.SetOutPutText(String.Format("TcpAutoCmdProcer_Log:cmdCode = {0}", tcpCmd.GetCmdCode().ToString()));
        }
    }
}
