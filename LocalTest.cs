using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DRSysCtrlDisplay
{
    class LocalTest
    {

        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();  

        //测试该类文件里面的类
        public static class Test
        {
            //连接测试使用
            public static void TcpClientTest()
            {
                //打开控制台
                AllocConsole();
                string testIp = "127.0.0.1";
                var tcpManager = TcpManager.Instance;
                tcpManager.StartWork();

                tcpManager.AddClient(testIp);

                tcpManager.SendOneCmd(testIp, new TcpCommand_Heart());

                FreeConsole();
            }

            public static void CodeGenTest()
            {
                //打开控制台
                AllocConsole();

                MHalCodeGen mpmGen = new MHalCodeGen("MPM");
                mpmGen.AddEMHalPublishRes("EMHalPPCResource");
                mpmGen.AddEMHalSubscribeRes("EMHalZynqResource0");
                mpmGen.AddEMHalSubscribeRes("EMHalZynqResource1");
                mpmGen.GenEMHalCode();

                mpmGen.AddRMHalPublishRes("RMHalPPCResource", 0x10000, 4);
                mpmGen.AddRMHalSubscribeRes("RMHalFpgaResource0");
                mpmGen.AddRMHalSubscribeRes("RMHalFpgaResource1");
                mpmGen.GenRMHalPpcCode(true);

                MHalCodeGen smpaGen = new MHalCodeGen("SMPA");
                smpaGen.AddEMHalPublishRes("EMHalZynqResource0");
                smpaGen.AddEMHalSubscribeRes("EMHalPPCResource");
                smpaGen.GenEMHalCode();

                smpaGen.AddRMHalPublishRes("RMHalFpgaResource0", 0x10000, 4);
                smpaGen.AddRMHalSubscribeRes("RMHalPPCResource");
                smpaGen.GenRMHalFpgaCode();
                smpaGen.GenLMHalFpgaCode();
                smpaGen.GenAMHalFpgaCode();

                MHalCodeGen smpbGen = new MHalCodeGen("SMPB");
                smpbGen.AddEMHalPublishRes("EMHalZynqResource1");
                smpbGen.AddEMHalSubscribeRes("EMHalPPCResource");
                smpbGen.GenEMHalCode();

                smpbGen.AddRMHalPublishRes("RMHalFpgaResource1", 0x10000, 4);
                smpbGen.AddRMHalSubscribeRes("RMHalPPCResource");
                smpbGen.GenRMHalFpgaCode();
                smpbGen.GenLMHalFpgaCode();
                smpbGen.GenAMHalFpgaCode();

                FreeConsole();
            }
        }
    }
}
