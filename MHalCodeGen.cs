using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UpperComputer;

namespace DRSysCtrlDisplay
{
    class SerializeName
    {
        static public void Serialize(Serialzer s, string name)
        {
            s.PushData(System.Text.Encoding.Default.GetBytes(name));
            for (int i = 0; i < 32 - name.Length; i++)
            {
                s.PushByte(0);
            }
        }
    }

    class RMHalNodeAttr
    {
        UInt32 _magicCode = 0x524D4850;
        UInt32 _version = 0x10000;
        string _name = "";

        public RMHalNodeAttr(string name)
        {
            _name = name;
        }

        public void Serialize(Serialzer s)
        {
            s.PushUint(_magicCode);
            s.PushUint(_version);
            SerializeName.Serialize(s, _name);
            byte[] reserved = new byte[88];
            s.PushData(reserved);
        }

        public string GetName()
        {
            return _name;
        }
    }

    class RMHalPubRes
    {
        UInt32 _flag = 0;
        UInt32 _type = 1;
        public string _name = "";
        public UInt32 _maxPktSize = 0;
        public UInt32 _bufNum = 0;
        UInt32 _lenAddr = 0;
        UInt32 _dataAddr = 0;
        UInt16 _firstDb = 0;
        UInt16 _reserved0 = 0;
        UInt16 _dstId = 0xFFFF;
        UInt16 _reserved1 = 0;

        public void SetResInfo(String resName, UInt32 maxPktSize, UInt32 bufNum)
        {
            _flag = 0x53525150;
            _name = resName;
            _maxPktSize = maxPktSize;
            _bufNum = bufNum;
        }

        public bool IsEnable()
        {
            return _flag == 0x53525150;
        }

        public void Serialize(Serialzer s)
        {
            s.PushUint(_flag);
            s.PushUint(_type);
            SerializeName.Serialize(s, _name);
            s.PushUint(_maxPktSize);
            s.PushUint(_bufNum);
            s.PushUint(_lenAddr);
            s.PushUint(_dataAddr);
            s.PushUShort(_firstDb);
            s.PushUShort(_reserved0);
            s.PushUShort(_dstId);
            s.PushUShort(_reserved1);
        }
    }

    class RMHalSubRes
    {
        UInt32 _flag = 0;
        UInt32 _reserved0 = 0;
        public string _name = "";
        UInt32 _maxPktSize = 0;
        UInt32 _bufNum = 0;
        UInt32 _lenAddr = 0;
        UInt32 _dataAddr = 0;
        UInt16 _srcId = 0xFFFF;
        UInt16 _firstDb = 0;
        UInt32 _reserved1 = 0;

        public void SetSubInfo(String resName)
        {
            _flag = 0x53525150;
            _name = resName;
        }

        public bool IsEnable()
        {
            return _flag == 0x53525150;
        }

        public void Serialize(Serialzer s)
        {
            s.PushUint(_flag);
            s.PushUint(_reserved0);
            SerializeName.Serialize(s, _name);
            s.PushUint(_maxPktSize);
            s.PushUint(_bufNum);
            s.PushUint(_lenAddr);
            s.PushUint(_dataAddr);
            s.PushUShort(_srcId);
            s.PushUShort(_firstDb);
            s.PushUint(_reserved1);
        }
    }

    class EMHalPubRes
    {
        public string _name;
        public string _transType;

        public EMHalPubRes(string name)
        {
            _name = name;
            _transType = "EMHAL_TRANS_TYPE_UDP";
        }
    }

    class EMHalSubRes
    {
        public string _name;

        public EMHalSubRes(string name)
        {
            _name = name;
        }
    }

    class MHalCodeGen
    {
        static UInt32 _resNum = 511;
        static int lineBytes = 8;
        string _nodeName;
        RMHalNodeAttr nodeAttr;
        RMHalPubRes[] pubRess;
        RMHalSubRes[] subRess;
        Serialzer s = new Serialzer(0x10000);

        List<EMHalPubRes> pubResTbl = new List<EMHalPubRes>();
        List<EMHalSubRes> subResTbl = new List<EMHalSubRes>();

        string _path;

        public MHalCodeGen(String nodeName)
        {
            _nodeName = nodeName;
            nodeAttr = new RMHalNodeAttr(nodeName);
            pubRess = new RMHalPubRes[_resNum];
            subRess = new RMHalSubRes[_resNum];
            for (int i = 0; i < _resNum; i++)
            {
                pubRess[i] = new RMHalPubRes();
                subRess[i] = new RMHalSubRes();
            }
            _path = "./MHal/" + _nodeName + "/";
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        public void AddRMHalPublishRes(String resName, UInt32 maxPktSize, UInt32 bufNum)
        {
            foreach (RMHalPubRes res in pubRess)
            {
                if (res.IsEnable())
                {
                    continue;
                }

                res.SetResInfo(resName, maxPktSize, bufNum);
                return;
            }
        }

        public void AddRMHalSubscribeRes(String resName)
        {
            foreach (RMHalSubRes res in subRess)
            {
                if (res.IsEnable())
                {
                    continue;
                }

                res.SetSubInfo(resName);
                return;
            }
        }

        public void GenRMHalPpcCode(bool isMaster)
        {
            string headFileName = "rmhal.h";
            string srcFileName =  "./resource/" + headFileName;
            string dstFileName = _path + "/" + headFileName;
            System.IO.File.Copy(srcFileName, dstFileName, true);

            StreamReader reader = new StreamReader("./resource/rmhalTemp.c");
            string cFileName = _path + "/" + nodeAttr.GetName() + "_RMHal.c";
            FileStream fStream = new FileStream(cFileName, FileMode.Create);
            StreamWriter writer = new StreamWriter(fStream, Encoding.GetEncoding("gbk"));
            string temp;

            do
            {
                temp = reader.ReadLine();
                writer.WriteLine(temp);
                if (temp == "/* The application start entry point ---------------------------------------- */")
                {
                    writer.WriteLine("int " + nodeAttr.GetName() + "_StartRMHal()");
                }

                if (temp == "/* Init RMHAL */")
                {
                    writer.WriteLine(@"    RMHAL_Init(""" + nodeAttr.GetName() + @""");");
                    writer.WriteLine("    if (ret != 0)");
                    writer.WriteLine("    {");
                    writer.WriteLine(@"        printf(""Init RMHAL failed\n"");");
                    writer.WriteLine("        return -1;");
                    writer.WriteLine("    }");
                }

                if (temp == "/* The application stop entry point ----------------------------------------- */")
                {
                    writer.WriteLine("void " + nodeAttr.GetName() + "_StopRMHal()");
                }

                if (temp == "/* If node is master then config */")
                {
                    if (isMaster)
                    {
                        writer.WriteLine("    ret = RMHAL_StartMaster();");
                        writer.WriteLine("    if (ret != 0)");
                        writer.WriteLine("    {");
                        writer.WriteLine(@"        printf(""Start RapidIO master config failed\n"");");
                        writer.WriteLine("        return -1;");
                        writer.WriteLine("    }");
                    }
                }

                if (temp == "/* Private variables ---------------------------------------------------------*/")
                {
                    foreach (RMHalPubRes res in pubRess)
                    {
                        if (res.IsEnable())
                        {
                            writer.WriteLine("int g_rmhalPub" + res._name + "_resId;");
                        }
                    }
                    foreach (RMHalSubRes res in subRess)
                    {
                        if (res.IsEnable())
                        {
                            writer.WriteLine("int g_rmhalSub" + res._name + "_resId;");
                        }
                    }
                }

                if (temp == "/* Private function prototypes -----------------------------------------------*/")
                {
                    foreach (RMHalSubRes res in subRess)
                    {
                        if (res.IsEnable())
                        {
                            writer.WriteLine("void " + res._name + "_DataHandle(int param, char *data, int size);");
                        }
                    }
                }

                if (temp == "/* Publish resource */")
                {
                    foreach (RMHalPubRes res in pubRess)
                    {
                        if (res.IsEnable())
                        {
                            string resIdStr = "g_rmhalPub" + res._name + "_resId";
                            writer.WriteLine("    " + resIdStr + @" = RMHAL_PublishRes(""" + 
                                res._name + @"""," + "0x" +res._maxPktSize.ToString("X") + ", " + 
                                res._bufNum.ToString() + ");");
                            writer.WriteLine("    if(" + resIdStr + " < 0)");
                            writer.WriteLine("    {");
                            writer.WriteLine(@"        printf(""Publish resource " + res._name + @" failed\n"");");
                            writer.WriteLine("        return -1;");
                            writer.WriteLine("    }");
                            writer.WriteLine();
                        }
                    }
                }

                if (temp == "/* Unpublish resource */")
                {
                    foreach (RMHalPubRes res in pubRess)
                    {
                        if (res.IsEnable())
                        {
                            string resIdStr = "g_rmhalPub" + res._name + "_resId";
                            writer.WriteLine("    RMHAL_UnPublishRes(" + resIdStr + ");");
                        }
                    }
                }

                if (temp == "/* Subscribe resource handle  ----------------------------------------------- */")
                {
                    foreach (RMHalSubRes res in subRess)
                    {
                        if (res.IsEnable())
                        {
                            writer.WriteLine("/* Resource " + res._name + " handle */");
                            writer.WriteLine("void " + res._name + "_DataHandle(int param, char *data, int size)");
                            writer.WriteLine("{");
                            writer.WriteLine("/* USER CODE BEGIN */");
                            writer.WriteLine();
                            writer.WriteLine("/* USER CODE END */");
                            writer.WriteLine("}");
                            writer.WriteLine();
                        }
                    }
                }

                if (temp == "/* Subscribe resource */")
                {
                    foreach (RMHalSubRes res in subRess)
                    {
                        if (res.IsEnable())
                        {
                            string resIdStr = "g_rmhalSub" + res._name + "_resId";
                            writer.WriteLine("/* Change param value which will be used in " + res._name + "_DataHandle */");
                            writer.WriteLine("    param = 0;");
                            writer.WriteLine("    " + resIdStr + @" = RMHAL_RegisterRes(""" +
                                res._name + @"""," + "(RMHAL_RecvHandler)" + res._name + "_DataHandle, param" + ");");
                            writer.WriteLine("    if(" + resIdStr + " < 0)");
                            writer.WriteLine("    {");
                            writer.WriteLine(@"        printf(""Sublish resource " + res._name + @" failed\n"");");
                            writer.WriteLine("        return -1;");
                            writer.WriteLine("    }");
                            writer.WriteLine();
                        }
                    }
                }

                if (temp == "/* Unsubscribe resource */")
                {
                    foreach (RMHalSubRes res in subRess)
                    {
                        if (res.IsEnable())
                        {
                            string resIdStr = "g_rmhalSub" + res._name + "_resId";
                            writer.WriteLine("    RMHAL_UnRegisterRes(" + resIdStr + ");");
                        }
                    }
                }

            } while (temp != null);

            writer.Flush();
            writer.Close();
        }

        private void GenCoeFile()
        {
            Serialzer s = new Serialzer(0x10000);

            //节点属性域
            nodeAttr.Serialize(s);

            //发布域
            for (int i = 0; i < _resNum; i++)
            {
                pubRess[i].Serialize(s);
            }

            //订阅域域
            for (int i = 0; i < _resNum; i++)
            {
                subRess[i].Serialize(s);
            }

            FileStream fStream = new FileStream(_path + "srio_source/config/" + "rmhal_psl.dat", FileMode.Create);
            StreamWriter writer = new StreamWriter(fStream, Encoding.UTF8);
            Serialzer p = new Serialzer(s.ToArray());
            byte[] line = new byte[lineBytes];
            int count = 0;
            while (p.Position != p.Length)
            {
                count++;
                p.Read(line, 0, lineBytes);
                string str = string.Empty;
                for (int i = 0; i < lineBytes; i++)
                {
                    str += line[i].ToString("X2");
                }
                //Console.WriteLine(count.ToString() + ":" + str);
                writer.WriteLine(str);
            }
            writer.Flush();
            writer.Close();
        }

        private void CopyEntireDir(string sourcePath, string destPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
               SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
               SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destPath), true);
        }

        private void copyXdcFile()
        {
            string newPath = _path + "xdc/";
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            File.Copy("./resource/source_file/xdc/mhal.xdc", newPath + "mhal.xdc", true);
        }

        public void GenRMHalFpgaCode()
        {
            CopyEntireDir("./resource/source_file/srio_source", _path + "srio_source");
            copyXdcFile();
            GenCoeFile();
        }

        public void GenLMHalFpgaCode()
        {
            CopyEntireDir("./resource/source_file/lvds_source", _path + "lvds_source");
            copyXdcFile();
        }

        public void GenAMHalFpgaCode()
        {
            CopyEntireDir("./resource/source_file/aurora_source", _path + "aurora_source");
            copyXdcFile();
        }

        public void AddEMHalPublishRes(string resName)
        {
            pubResTbl.Add(new EMHalPubRes(resName));
        }

        public void AddEMHalSubscribeRes(String resName)
        {
            subResTbl.Add(new EMHalSubRes(resName));
        }

        public void GenEMHalCode()
        {
            string headFileName = "emhal.h";
            string srcFileName = "./resource/" + headFileName;
            string dstFileName = _path + "/" + headFileName;
            System.IO.File.Copy(srcFileName, dstFileName, true);

            string emhalLibName = "libemhal.a";
            srcFileName = "./resource/" + emhalLibName;
            dstFileName = _path + "/" + emhalLibName;
            System.IO.File.Copy(srcFileName, dstFileName, true);

            StreamReader reader = new StreamReader("./resource/emhalTemp.c");
            string cFileName = _path + "/" + _nodeName + "_EMHal.c";
            FileStream fStream = new FileStream(cFileName, FileMode.Create);
            StreamWriter writer = new StreamWriter(fStream, Encoding.GetEncoding("gbk"));
            string temp;

            do
            {
                temp = reader.ReadLine();
                writer.WriteLine(temp);
                if (temp == "/* The application start entry point ---------------------------------------- */")
                {
                    writer.WriteLine("int " + _nodeName + "_StartEMHal()");
                }

                if (temp == "/* Init EMHAL */")
                {
                    writer.WriteLine(@"    EMHAL_Init(""" + _nodeName + @""");");
                }

                if (temp == "/* The application stop entry point ----------------------------------------- */")
                {
                    writer.WriteLine("void " + _nodeName + "_StopEMHal()");
                }

                if (temp == "/* Private variables ---------------------------------------------------------*/")
                {
                    foreach (EMHalPubRes res in pubResTbl)
                    {
                        writer.WriteLine("int g_emhalPub" + res._name + "_resId;");
                    }
                    foreach (EMHalSubRes res in subResTbl)
                    {
                        writer.WriteLine("int g_emhalSub" + res._name + "_resId;");
                    }
                }

                if (temp == "/* Private function prototypes -----------------------------------------------*/")
                {
                    foreach (EMHalSubRes res in subResTbl)
                    {
                        writer.WriteLine("void " + res._name + "_DataHandle(int param, char *data, int size);");
                    }
                }

                if (temp == "/* Publish resource */")
                {
                    foreach (EMHalPubRes res in pubResTbl)
                    {
                        string resIdStr = "g_emhalPub" + res._name + "_resId";
                        writer.WriteLine("    " + resIdStr + @" = EMHAL_PublishRes(""" +
                            res._name + @"""," + res._transType + ");");
                        writer.WriteLine("    if(" + resIdStr + " < 0)");
                        writer.WriteLine("    {");
                        writer.WriteLine(@"        printf(""Publish resource " + res._name + @" failed\n"");");
                        writer.WriteLine("        return -1;");
                        writer.WriteLine("    }");
                        writer.WriteLine();
                    }
                }

                if (temp == "/* Unpublish resource */")
                {
                    foreach (EMHalPubRes res in pubResTbl)
                    {
                        string resIdStr = "g_emhalPub" + res._name + "_resId";
                        writer.WriteLine("    EMHAL_UnPublishRes(" + resIdStr + ");");
                    }
                }

                if (temp == "/* Subscribe resource */")
                {
                    foreach (EMHalSubRes res in subResTbl)
                    {
                        string resIdStr = "g_emhalSub" + res._name + "_resId";
                        writer.WriteLine("/* Change param value which will be used in " + res._name + "_DataHandle */");
                        writer.WriteLine("    param = 0;");
                        writer.WriteLine("    " + resIdStr + @" = EMHAL_RegisterRes(""" +
                                res._name + @"""," + "(EMHAL_RecvHandler)" + res._name + "_DataHandle, param" + ");");
                        //writer.WriteLine("    " + resIdStr + @" = EMHAL_RegisterRes(""" + res._name + @""");");
                        writer.WriteLine("    if(" + resIdStr + " < 0)");
                        writer.WriteLine("    {");
                        writer.WriteLine(@"        printf(""Register resource " + res._name + @" failed\n"");");
                        writer.WriteLine("        return -1;");
                        writer.WriteLine("    }");
                        writer.WriteLine();
                    }
                }

                if (temp == "/* Unsubscribe resource */")
                {
                    foreach (EMHalSubRes res in subResTbl)
                    {
                        string resIdStr = "g_emhalSub" + res._name + "_resId";
                        writer.WriteLine("    EMHAL_UnRegisterRes(" + resIdStr + ");");
                    }
                }

                if (temp == "/* Subscribe resource handle  ----------------------------------------------- */")
                {
                    foreach (EMHalSubRes res in subResTbl)
                    {
                        writer.WriteLine("/* Resource " + res._name + " handle */");
                        writer.WriteLine("void " + res._name + "_DataHandle(int param, char *data, int size)");
                        writer.WriteLine("{");
                        writer.WriteLine("/* USER CODE BEGIN */");
                        writer.WriteLine();
                        writer.WriteLine("/* USER CODE END */");
                        writer.WriteLine("}");
                        writer.WriteLine();
                    }
                }

            } while (temp != null);

            writer.Flush();
            writer.Close();

            
        }
    }
}
