using System;
using AutoConnectLAN.Model;
using AutoConnectLAN.Control;

namespace AutoConnectLAN
{
	class Program
    {
        static int Main(string[] args)
        {
            /*
               CheckNAC checkNAC = null;
               checkNAC = new CheckNAC();

               if (checkNAC.isCheckEdgeDriver(checkNAC.getEdgeVesrion()) == false)
               {
                   checkNAC.downEdgeDriver(checkNAC.getEdgeVesrion());
               }

               bool chk = checkNAC.isLogin("user", "1234");
               Console.WriteLine("chk : " + chk);
              */

            Controller c = new Controller();
            bool chk = c.firstInternetCheck();
            Console.WriteLine("chk : " + chk);
            c.printWiFiList();

            return 0;
        }


    }
}
