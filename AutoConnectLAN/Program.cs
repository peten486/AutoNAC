using System;

using System.Net.NetworkInformation;

namespace AutoConnectLAN
{
	class Program
    {
        static int Main(string[] args)
        {
            /*
              Console.WriteLine("Hello, World");
              if (IsConnectedToInternet() == true)
              {
                  Console.WriteLine("Lan Connect");
              }
              else
              {
                  Console.WriteLine("not connected");
              }

              Drive();
            */

            string Path = System.IO.Directory.GetCurrentDirectory();
            CheckNAC checkNAC = null;
            checkNAC = new CheckNAC();

            if (checkNAC.isCheckEdgeDriver(checkNAC.getEdgeVesrion()) == false)
            {
                checkNAC.downEdgeDriver(checkNAC.getEdgeVesrion());
            }

            bool chk = checkNAC.isLogin("user", "1234");
            Console.WriteLine("chk : " + chk);
            
            return 0;
        }

        public static bool IsConnectedToInternet()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable(); 
        }

        public static void Drive()
        {
            var networks = NetworkInterface.GetAllNetworkInterfaces(); // 사용가능한 모든 네트워크를 배열로 모음.

            foreach (NetworkInterface net in networks)
            {
                Console.WriteLine("net.Id: {0}", net.Id); // 네트워크의 고유id
                Console.WriteLine("net.Name: {0}", net.Name); // 표기되는 이름
                Console.WriteLine("net.IsReceiveOnly: {0}", net.IsReceiveOnly);
                Console.WriteLine("net.OperationalStatus: {0}", net.OperationalStatus); // 연결됐습니까?
                Console.WriteLine("net.NetworkInterfaceType: {0}", net.NetworkInterfaceType); // 구분용??
                Console.WriteLine("net.Description: {0}", net.Description); // 장치설명
                Console.WriteLine("net.SupportsMulticast: {0}", net.SupportsMulticast);
                Console.WriteLine("------------------");
            }
        }

    }
}
