using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DRSysCtrlDisplay
{
    class NetAgree
    {
        public NetClient _client = new NetClient();
        private int _port = 8000;
        private const uint _packHead = 0x0001;
        private const int _packMin = 8;//数据包最小长度
        private byte[] _recvBuf = new byte[4096];

        public NetAgree()
        {

        }

        public void Connect(IPAddress addr)
        {
            _client.Connect(addr, _port);
        }

        public void Connect(IPAddress addr, int timeoutMs)//设置超时时间
        {
            _client.Connect(addr, _port, timeoutMs);
        }

        public void Close()
        {
            _client.Close();
        }
    }
}
