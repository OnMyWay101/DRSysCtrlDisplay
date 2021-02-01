using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;


namespace DRSysCtrlDisplay
{
    class Utilities
    {
        /// <summary>
        /// 字节数组与结构体的相互转化
        /// </summary>
        public static class ByteStructConvert<T>
            where T : struct
        {
            public static object BytesToStruct(byte[] bytes)
            {
                int size = Marshal.SizeOf(typeof(T));
                IntPtr ptr = Marshal.AllocHGlobal(size);//分配内存空间
                try
                {
                    bytes = RespectEndianness(bytes);
                    Marshal.Copy(bytes, 0, ptr, size);//拷贝到相应的分配内存空间
                    return Marshal.PtrToStructure(ptr, typeof(T));//把分配内存空间里的数据转化为结构体
                }
                catch (Exception e)//收到数据的错误处理
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);//释放空间
                }
            }

            public static byte[] StructToBytes(T stru)
            {
                int size = Marshal.SizeOf(stru);
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(stru, structPtr, false);

                byte[] bytes = new byte[size];
                Marshal.Copy(structPtr, bytes, 0, size);
                Marshal.FreeHGlobal(structPtr);

                //bytes = RespectEndianness(bytes);
                return bytes;
            }

            /// <summary>
            /// 注意：这里的struct(T)若包含有内部的struct,该字节序调整方法不支持调整内部struct的字节序
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="bytes"></param>
            /// <returns></returns>
            private static byte[] RespectEndianness(byte[] bytes)
            {
                Type type = typeof(T);
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => f.IsDefined(typeof(EndianAttribute), false)).Select(field => new
                    {
                        Field = field,
                        Attri = (EndianAttribute)field.GetCustomAttributes(typeof(EndianAttribute), false).First(),
                        Offset = Marshal.OffsetOf(type, field.Name).ToInt32()
                    }).ToList();
                foreach (var field in fields)
                {
                    if ((field.Attri.Endian == Endianness.BigEndian && BitConverter.IsLittleEndian) ||
                        (field.Attri.Endian == Endianness.LittleEndian && !BitConverter.IsLittleEndian))
                    {
                        Array.Reverse(bytes, field.Offset, Marshal.SizeOf(field.Field.FieldType));
                    }
                }
                return bytes;
            }
        }

        public enum Endianness
        {
            BigEndian,
            LittleEndian
        }

        /// <summary>
        /// 字节序特性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]//使用AttributeUsage特性限制此EndianAttribute特性仅限字段使用
        public class EndianAttribute : Attribute
        {
            public Endianness Endian { get; private set; }

            public EndianAttribute(Endianness endianness)
            {
                this.Endian = endianness;
            }
        }

        public class NetStreamProcess : MemoryStream
        {
            //主要作用生成byte[]数据
            public NetStreamProcess(){}

            //主要作用转化byte[]数据
            public NetStreamProcess(byte[] data) : base(data){ }

            public byte[] GetStreamBytes()
            {
                return base.ToArray();
            }

            //从stream中提取数据

            public int PopInt32()
            {
                return BitConverter.ToInt32(PopReverseBytes(4), 0);
            }

            public int PopInt16()
            {
                return BitConverter.ToInt16(PopReverseBytes(2), 0);
            }

            public byte PopByte()
            {
                return (byte)base.ReadByte();
            }

            //带字节序转化的提取字节数组
            private byte[] PopReverseBytes(int num)
            {
                var retBytes = PopBytes(num);
                Array.Reverse(retBytes, 0, num);

                return retBytes;
            }

            public byte[] PopBytes(int num)
            {
                if (num <= 0)
                {
                    return null;
                }
                var retBytes = new byte[num];
                base.Read(retBytes, 0, num);

                return retBytes;
            }

            //往stream中添加数据

            public void PushInt32(int num)
            {
                var intBytes = BitConverter.GetBytes(num);
                PushReverseBytes(intBytes);
            }

            public void PushInt16(Int16 num)
            {
                var intBytes = BitConverter.GetBytes(num);
                PushReverseBytes(intBytes);
            }

            public void PushByte(byte b)
            {
                base.WriteByte(b);
            }

            public void PushBytes(byte[] bytes)
            {
                if (bytes == null)
                {
                    return;
                }
                PushBytes(bytes, bytes.Length);
            }

            public void PushBytes(byte[] bytes, int num)
            {
                base.Write(bytes, 0, num);
            }

            private void PushReverseBytes(byte[] bytes)
            {
                Array.Reverse(bytes, 0, bytes.Length);
                PushBytes(bytes);
            }
        }
    }
}
