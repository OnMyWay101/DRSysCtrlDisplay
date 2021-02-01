using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace DRSysCtrlDisplay
{
    public class NetClient
    {
        private TcpClient _client;
        private ManualResetEvent _conEvent;
        private Boolean _connectResult;
        static private int _defaultTimeout = 3000;

        public NetClient()
        {
            _client = new TcpClient();
            _conEvent = new ManualResetEvent(false);
        }


        public void Connect(IPAddress addr, int port)
        {
            Connect(addr, port, _defaultTimeout);
        }

        public void Connect(IPAddress addr, int port, int timeoutMs)
        {
            _connectResult = false;
            _conEvent.Reset();
            _client.BeginConnect(addr, port, new AsyncCallback(ConnectCallback), this);

            if (_conEvent.WaitOne(timeoutMs, false))
            {
                if (_connectResult)
                {
                    _client.SendBufferSize = 4096;
                }
                else
                {
                    _client.Close();
                    throw new Exception();/*连接失败*/
                }

            }
            else
            {
                _client.Close();
                throw new TimeoutException();/*连接超时*/
            }
        }

        public void Close()
        {
            _client.Close();
            _connectResult = false;
        }

        public void SendData(Byte[] data, int size)
        {
            _client.Client.Send(data, size, SocketFlags.None);
        }

        public void RecvData(Byte[] buf, int size)
        {
            RecvData(buf, size, _defaultTimeout);
        }

        public void RecvData(Byte[] buf, int size, int timeOutMs)
        {
            DateTime n = DateTime.Now;
            do
            {
                if (_client.Available < size && _client.Connected)
                {
                    Thread.Sleep(1);
                    continue;
                }
                _client.GetStream().Read(buf, 0, size);
                return;

            } while ((DateTime.Now - n).TotalMilliseconds < timeOutMs);
            throw new TimeoutException();
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            NetClient cl = ar.AsyncState as NetClient;
            cl.ConnectCallbackInternal(ar);
        }

        private void ConnectCallbackInternal(IAsyncResult ar)
        {
            try
            {
                _client.EndConnect(ar);
                _connectResult = true;
            }
            catch (System.Exception)
            {
                
                MessageBox.Show("网络连接异常");
            }
            _conEvent.Set();
        }
    }
}
