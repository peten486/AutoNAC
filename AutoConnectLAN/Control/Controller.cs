using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Management;
using AutoConnectLAN.Model;

namespace AutoConnectLAN.Control
{
	class Controller
	{
		bool run_flag = false;
		ReaderWriterLockSlim readerWriterLockSlim = null;
		CheckNAC nAC = null;
		CurNetwork curNetwork = null;
		List<Network> network_list = null;
		WIFI_Process wifi_process = null;


		public Controller()
		{
			run_flag = false;
			readerWriterLockSlim = new ReaderWriterLockSlim();
			nAC = new CheckNAC();
			curNetwork = new CurNetwork();
			network_list = new List<Network>();
			wifi_process = new WIFI_Process();
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
			ManagementObjectSearcher objadapter = new ManagementObjectSearcher("select * from Win32_NetworkAdapter");

			foreach (ManagementObject obj in objadapter.Get())
			{
				if (obj["NetEnabled"] != null)
				{
					if (obj["Name"].ToString().ToUpper().Contains("VIRTUALBOX"))
					{
						continue;
					}

					if (obj["NetEnabled"].ToString().Equals("True"))
					{
						Console.WriteLine("======================================");
						Console.WriteLine(string.Format("{0} : {1}", "Name", obj["Name"]));
						Console.WriteLine(string.Format("{0} : {1}", "NetEnabled", obj["NetEnabled"].ToString()));
						Console.WriteLine(string.Format("{0} : {1}", "AdapterType", obj["AdapterType"]));
						Console.WriteLine(string.Format("{0} : {1}", "AdapterTypeID", obj["AdapterTypeID"]));
						Console.WriteLine(string.Format("{0} : {1}", "Availability", obj["Availability"]));
						Console.WriteLine(string.Format("{0} : {1}", "Manufacturer", obj["Manufacturer"]));
						Console.WriteLine(string.Format("{0} : {1}", "NetConnectionID", obj["NetConnectionID"]));
						Console.WriteLine(string.Format("{0} : {1}", "NetConnectionStatus", obj["NetConnectionStatus"]));
						Console.WriteLine("======================================");

						Network n = new Network();
						n.Name = obj["Name"].ToString();
						n.NetEnabled = bool.Parse(obj["NetEnabled"].ToString());
						n.AdapterType = obj["AdapterType"].ToString();
						n.AdapterTypeID = int.Parse(obj["AdapterTypeID"].ToString());
						n.NetConnectionID = obj["NetConnectionID"].ToString();
						n.NetConnectionStatus = int.Parse(obj["NetConnectionStatus"].ToString());

						if (n.NetConnectionID.Contains("Wi-Fi"))
						{
							n.NetworkType = 2;
						}
						else if( n.NetConnectionID.ToUpper().Contains("ETHERNET") || n.NetConnectionID.Contains("이더넷"))
						{
							n.NetworkType = 1;
						}

						network_list.Add(n);
					}
				}

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
				Console.WriteLine("> Name : {0}", network_list[i].Name);
				Console.WriteLine("> AdapterType : {0}", network_list[i].AdapterType);
				Console.WriteLine("> NetConnectionID : {0}", network_list[i].NetConnectionID);
				Console.WriteLine("> AdapterTypeID : {0}", network_list[i].AdapterTypeID);
				Console.WriteLine("> NetworkType : {0}", networkType2str( network_list[i].NetworkType ));
				Console.WriteLine("======================================");
			}
		}

		public string networkType2str(int network_type)
		{
			if (network_type == 1)
			{
				return "Ethernet";
			}
			else if (network_type == 2)
			{
				return "Wireless";
			}
			else
			{
				return "UNKNOWN";
			}
		}

		public void printWiFiList()
		{
			bool chk = false;
			chk = wifi_process.get_connection_status();
			if (chk == false)
			{
				Console.WriteLine("ERR : Wifi missing ");
			}
			else
			{
				wifi_process.print_wifi();
			}
		}

		public void setWiFiSetting(int idx)
		{
			wifi_process.wifi_conn();
		}

	}
}
