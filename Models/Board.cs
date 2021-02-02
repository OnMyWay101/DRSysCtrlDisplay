using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathManager = DRSysCtrlDisplay.XMLManager.PathManager;

namespace DRSysCtrlDisplay.Models
{
    public class Board : ModelBase
    {
        public string Type { get; set; }
        public string Version { get; set; }
   
        public List<PPC> PPCList { get; set; }              //板上的PPC芯片集合
        public List<FPGA> FPGAList { get; set; }            //板上的FPGA芯片集合
        public List<ZYNQ> ZYNQList { get; set; }            //板上的ZYNQ芯片集合
        public List<SwitchDevice> SwitchList { get; set; }  //板上的交换机芯片集合

        public List<BoardLink> LinkList { get; set; }

    }
}
