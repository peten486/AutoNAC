using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace AutoConnectLAN
{
	class Controller
	{
		bool run_flag = false;
		ReaderWriterLockSlim readerWriterLockSlim = null;
		CheckNAC nAC = null;
		CurNetwork curNetwork = null;
		List<Network> network_list = null;

		public Controller()
		{
			run_flag = false;
			readerWriterLockSlim = new ReaderWriterLockSlim();
			nAC = new CheckNAC();
			curNetwork = new CurNetwork();
			network_list = new List<Network>();
		}

		public bool firstInternetCheck()
		{
			bool chk = isInternetConnected();
			if (chk == false)
			{
				return false;
			}

			getNetworkSystem();
			printNetworkSystem();

			return true;
		}

		void setFlag()
		{
			run_flag = true;
		}

		void setFlag_Exit()
		{
			run_flag = false;
		}

		void THR_CheckPing()
		{
			while (run_flag == false)
			{
				//curNetwork.InternetChk = false;
				

			}
		}

		void THR_CheckLoginNAC()
		{
			while (run_flag == false)
			{
			}
		}

		public bool isInternetConnected()
		{
			bool conn = NetworkInterface.GetIsNetworkAvailable();
			return conn;
		}
		
		public bool getNetworkSystem()
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
				Network n = new Network();
				n.DeviceId = net.Id;
				n.Name = net.Name;
				string temp = net.NetworkInterfaceType.ToString();
				if (temp.Contains("Ethernet"))
				{
					n.NetworkType = 1;
				}
				else if (temp.Contains("Wireless"))
				{
					n.NetworkType = 2;
				}
				else
				{
					continue;
				}

				temp = net.Description.ToString();
				if (temp.Contains("Bluetooth"))
				{
					continue;
				}

				network_list.Add(n);
			}

			if (network_list.Count <= 0)
			{
				return false;
			}

			return true;
		}

		public void printNetworkSystem()
		{
			for (int i = 0; i < network_list.Count; i++)
			{
				Console.WriteLine("======================================");
				Console.WriteLine("> DeviceID : {0}", network_list[i].DeviceId);
				Console.WriteLine("> Name : {0}", network_list[i].Name);
				Console.WriteLine("> NetworkType : {0}", networkType2str( network_list[i].NetworkType ));
				Console.WriteLine("======================================");
			}
		}

		public string networkType2str(int network_type)
		{
			if (network_type == 1)
			{
				return "ETHERNET";
			}
			else if (network_type == 2)
			{
				return "WIRELESS";
			}
			else
			{
				return "UNKNOWN";
			}
		}


	}
}
