using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Management;

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

		public void printN222()
		{
			/*
			ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection MOC = MC.GetInstances();
			
			foreach (ManagementObject MO in MOC)
			{
				if (MO["IPAddress"] != null)
				{
					if (MO["IPAddress"] is Array)
					{ //IP 및 Subnet, Gatway String 배열로 변환... 
						string[] addresses = (string[])MO["IPAddress"];
						string[] subnets = (string[])MO["IPSubnet"];
						string[] gateways = (string[])MO["DefaultIPGateway"];
						
						string cur_index = null;
						if (MO["InterfaceIndex"].ToString().Equals("7"))
						{
							cur_index = "Wi-Fi";
						}
						else if (MO["InterfaceIndex"].ToString().Equals("3"))
						{
							cur_index = "Ethernet";
						}
						else
						{
							cur_index = MO["InterfaceIndex"].ToString();
						}

						//모두 null 이 아니면... 
							if (addresses != null && subnets != null && gateways != null )
						{
							Console.WriteLine("======================================");
							Console.WriteLine("> IPAddress : {0}", addresses[0]);
							Console.WriteLine("> IPSubnet : {0}", subnets[0]);
							Console.WriteLine("> DefaultIPGateway : {0}", gateways[0]);
							Console.WriteLine("> Interface : {0}", cur_index);
							Console.WriteLine("======================================");
						}
					}
					else
					{
					}
				}
			} 
			*/
			/*
			ManagementClass objMC = new ManagementClass("Win32_NetworkAdapter");
			ManagementObjectCollection objMOC = objMC.GetInstances();

			foreach (ManagementObject objMO in objMOC)
			{
				if (objMO["NetConnectionID"] != null)
				{
					Console.WriteLine(string.Format("{0} : {1}", "AdapterType", objMO["AdapterType"]));
					Console.WriteLine(string.Format("{0} : {1}", "DeviceID", objMO["DeviceID"]));
					Console.WriteLine(string.Format("{0} : {1}", "NetConnectionID", objMO["NetConnectionID"]));
					Console.WriteLine("=====================================================================");
				}
			}
			*/

			var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
			var moc = mc.GetInstances();

			foreach (var mo in moc)
			{
				if ((bool)mo["ipEnabled"])
				{
					Console.WriteLine(string.Format("{0} : {1}", "Caption", mo["Caption"].ToString()));
				}
			}

			//return nicNames;
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
