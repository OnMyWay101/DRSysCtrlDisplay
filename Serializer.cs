using System;
using System.IO;
using System.Net;
using System.Text;

namespace UpperComputer
{
    public class Serialzer :  MemoryStream
    {
        public Serialzer(int size) : base(size)
        {
            
        }

        public Serialzer(byte[] bytes) : base(bytes, 0, bytes.Length, true,true)
        {

        }

        public Serialzer(byte[] bytes, int length) : base(bytes, 0, length, true, true)
        {

        }

         public void PushInt(int val)
        {
            byte[] data = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(val));
            Write(data, 0, data.Length);            
        }

        public void PushUint(UInt32 val)
        {
            byte[] data = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)(val)));
            Write(data, 0, data.Length);        
        }

        public void PushShort(Int16 val)
        {
            byte[] data = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(val));
            Write(data, 0, data.Length);
        }

        public void PushUShort(UInt16 val)
        {
            byte[] data = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)(val)));
            Write(data, 0, data.Length);
        }

        public void PushByte(byte val)
        {
            WriteByte(val);
        }

        public void PushData(byte[] data)
        {
            Write(data, 0, data.Length);
        }

        public void PushString(string str)
        {
            byte size = (byte)(str.Length % 256);
            PushByte(size);
            Write(Encoding.ASCII.GetBytes(str), 0, size);
        }

        public void PushUTF8String(String str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            byte size = (byte)(b.Length % 256);
            PushByte(size);
            Write(b, 0, size);
        }

        public int PopInt()
        {
            byte[] data = new byte[4];
            Read(data, 0, 4);

            int val = BitConverter.ToInt32(data, 0);

            return IPAddress.NetworkToHostOrder(val);
        }

        public UInt32 PopUInt()
        {
            return (UInt32)(PopInt());
        }

        public IPAddress PopIpAddr()
        {
            int addr = PopInt();
            IPAddress ipAddr = new IPAddress(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(addr)));
            return ipAddr;
        }

        public Int16 PopShort()
        {
            byte[] data = new byte[2];
            Read(data, 0, 2);

            Int16 val = BitConverter.ToInt16(data, 0);

            return IPAddress.NetworkToHostOrder(val);
        }

        public UInt16 PopUShort()
        {
            return (UInt16)(PopShort());
        }

        public byte PopByte()
        {
            return (byte)ReadByte();
        }

        //读取一个字符串，1字节的字符串长度，后面是N字节数据
        public string PopString()
        {
            int nameLen = ReadByte();            
            if(nameLen > 0)
            {
                byte[] nameBytes = new byte[nameLen];
                Read(nameBytes, 0, nameLen);
                return Encoding.ASCII.GetString(nameBytes);
            }
            return "";        
        }

        public String PopUTF8String()
        {
            int nameLen = ReadByte();
            if (nameLen > 0)
            {
                byte[] nameBytes = new byte[nameLen];
                Read(nameBytes, 0, nameLen);
                return Encoding.UTF8.GetString(nameBytes);
            }
            return "";
        }

        public byte[] PopData(int size)
        {
            byte[] readBytes = new byte[size];
            Read(readBytes, 0, size);
            return readBytes;
        }
    }
}