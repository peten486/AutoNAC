using System;
using System.Collections.Generic;
using System.Text;

namespace AutoConnectLAN.Model
{
	class CurNetwork
	{
        public bool InternetChk { get; set; }
        
        public bool NACChk { get; set; }
        public string ChkDate { get; set; }

        public int NetworkType { get; set; }    //  1 : Ethernet
                                                //  2 : Wireless
       
    }
}
