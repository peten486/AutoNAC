﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AutoConnectLAN
{
	class CurNetwork
	{
        public bool InternetChk { get; set; }
        
        public string ChkDate { get; set; }

        public int NetworkType { get; set; }    //  1 : Ethernet
                                                //  2 : Wireless
       
    }
}