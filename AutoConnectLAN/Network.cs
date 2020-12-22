using System;
using System.Collections.Generic;
using System.Text;

namespace AutoConnectLAN
{
    public sealed class Network
    {
        public string DeviceId { get; set; }

        public string Name { get; set; }

        public int NetworkType { get; set; }    //  1 : Ethernet
                                                //  2 : Wireless
        public string AdapterType { get; set; } // 만약 어댑터가 사용못함일 경우, AdapterType은 공백처리가 됩니다.

        public string NetConnectionID { get; set; } // 네트워크 어댑터 설정에 표시되는 이름이 적힙니다.


    }
}
