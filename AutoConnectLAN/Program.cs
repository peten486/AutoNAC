using System;
using AutoConnectLAN.Model;
using AutoConnectLAN.Control;

namespace AutoConnectLAN
{
	class Program
    {
        Controller c;

        Program()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Ctrl_C_Pressed);
            c = new Controller();
        }
        static int Main(string[] args)
        {
           (new Program()).Run();
           return 0;
        }

        void Run()
        {
            c.setFlag();
            c.run();
        }

        void Ctrl_C_Pressed(object sender, ConsoleCancelEventArgs eventArgs)
        {
            c.setFlag_Exit();
            Console.WriteLine("program 종료");
           // c.nAC.closeDriver();
        }


    }
}
