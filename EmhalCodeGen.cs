using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UpperComputer;

namespace DRSysCtrlDisplay
{
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

    class EmhalCodeGen
    {
        string _nodeName;
        List<EMHalPubRes> pubResTbl = new List<EMHalPubRes>();
        List<EMHalSubRes> subResTbl = new List<EMHalSubRes>();

        public EmhalCodeGen(string nodeName)
        {
            _nodeName = nodeName;
        }

        public void AddPublishRes(string resName)
        {
            pubResTbl.Add(new EMHalPubRes(resName));
        }

        public void AddSubscribeRes(String resName)
        {
            subResTbl.Add(new EMHalSubRes(resName));
        }

        public void GenEMHalCode()
        {
            string path = "./" + _nodeName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string headFileName = "emhal.h";
            string srcFileName = "./resource/" + headFileName;
            string dstFileName = path + "/" + headFileName;
            System.IO.File.Copy(srcFileName, dstFileName, true);

            StreamReader reader = new StreamReader("./resource/emhalTemp.c");
            string cFileName = path + "/" + _nodeName + "_EMHal.c";
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
