using System;
using System.Collections.Generic;
using System.Text;

namespace AutoConnectLAN
{
    public sealed class Network
    {
        public string Name { get; set; }

        public bool NetEnabled { get; set; }

        public string AdapterType { get; set; }
        public int AdapterTypeID {get; set;}


        public string NetConnectionID { get; set; } // 네트워크 어댑터 설정에 표시되는 이름이 적힙니다.
        public int NetConnectionStatus { get; set; }


        public int NetworkType { get; set; }    //  1 : Ethernet
                                                //  2 : Wireless


    }
}
