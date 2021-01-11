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
		ReaderWriterLockSlim nACLockSlim = null;

		public CheckNAC nAC = null;
		public CurNetwork curNetwork = null;
		List<Network> network_list = null;
		WIFI_Process wifi_process = null;


		public Controller()
		{
			run_flag = false;
			readerWriterLockSlim = new ReaderWriterLockSlim();
			nACLockSlim = new ReaderWriterLockSlim();
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

		public void run()
		{
			Thread thread = new Thread(() => THR_CheckInternet());
			thread.Start();

			Thread.Sleep(2000);

			Thread thread2 = new Thread(() => THR_CheckLoginNAC());
			thread2.Start();
		}

		public void setFlag()
		{
			run_flag = true;
		}

		public void setFlag_Exit()
		{
			run_flag = false;
		}

		void THR_CheckInternet()
		{
			bool chk = false;
			int cnt = 0;
			nACLockSlim.EnterReadLock();
			try
			{	
				while (run_flag == true)
				{
					Thread.Sleep(2000);
					Console.WriteLine("THR_CheckInternet start");
					readerWriterLockSlim.EnterReadLock();
					try
					{
						if (curNetwork.InternetChk == false)
						{
							// internet이 연결되지 않는 상태이면, 연결 시도
							chk = isInternetConnected();
							if (chk == false)
							{
								cnt = 0;
								// 인터넷이 완전히 끊겼을 경우, WIFI 체크 후에 설정한 wifi가 설정되는지 확인
								
							}
							else
							{
								getNetworkSystem();
								if (network_list.Count <= 0)
								{
									Console.WriteLine("THR_CheckInternet :: getNetworkSystem count : " + network_list.Count );
									continue;
								}

								Console.WriteLine("THR_CheckInternet :: setCurNetwork count : " + network_list.Count);

								setCurNetwork();
								if (curNetwork.InternetChk == true && curNetwork.NetworkType == 2)
								{
									printWiFiList();
									wifi_process.wifi_conn();
								}

								if (curNetwork.InternetChk == true && curNetwork.NetworkType == 1)
								{
									Console.WriteLine("THR_CheckInternet :: curNetwork true");
								}

								if (curNetwork.InternetChk == false)
								{
									Console.WriteLine("THR_CheckInternet :: curNetwork false");
								}

								if (cnt == 0)
								{
									cnt++;
									getNetworkSystem();
									printNetworkSystem();
								}
							}
						}
						else
						{
							Console.WriteLine("curNetwork true");
						}
					}
					finally
					{
						readerWriterLockSlim.ExitReadLock();
					}
				}
			}
			catch
			{
				nACLockSlim.ExitReadLock();
			}
		}

		void THR_CheckLoginNAC()
		{
			/*
			Console.WriteLine("Run Start - THR_CheckLoginNAC");
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine(string.Format("Run THR_CheckLoginNAC {0}", i));
			}
			Console.WriteLine("Run End - THR_CheckLoginNAC");
			*/
			nACLockSlim.EnterReadLock(); 
			try
			{
				while (run_flag == true)
				{
					Thread.Sleep(2000);
					Console.WriteLine("THR_CheckLoginNAC start");
					readerWriterLockSlim.EnterReadLock();
					try
					{
						if (curNetwork.InternetChk == false)
						{
							// internet이 연결 안되어있으면 pass
							Console.WriteLine("THR_CheckLoginNAC :: curNetwork false");
							continue;
						}
						else
						{
							Console.WriteLine("THR_CheckLoginNAC :: curNetwork true");
						}
					}
					finally
					{
						readerWriterLockSlim.ExitReadLock();
					}

					/*
					 * 
					 */

					nACLockSlim.EnterWriteLock();
					try
					{
						if (nAC.isCheckEdgeDriver(nAC.getEdgeVesrion()) == false)
						{
							nAC.downEdgeDriver(nAC.getEdgeVesrion());
						}

						bool chk = nAC.isLogin("user", "1234");
						Console.WriteLine("THR_CheckLoginNAC :: chk : " + chk);
					}
					finally
					{
						nACLockSlim.ExitReadLock();
					}

				}
			}
			finally
			{
				nACLockSlim.ExitReadLock();
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

		public void setCurNetwork()
		{
			int wifi_chk = -1;
			int lan_chk = -1;
			for (int i = 0; i < network_list.Count; i++)
			{
				if (network_list[i].NetworkType == 2)
				{
					wifi_chk = i;
				}
				else if (network_list[i].NetworkType == 1)
				{
					lan_chk = i;
				}
			}

			if (wifi_chk == -1 && lan_chk == -1)
			{
				Console.WriteLine("err:not internet");
				return;
			}

			/* lan_chk를 먼저 확인 */
			if (lan_chk != -1)
			{
				// network
				// CurNetwork
				readerWriterLockSlim.EnterWriteLock();
				curNetwork.NetworkType = network_list[lan_chk].NetworkType;
				curNetwork.ChkDate = getNowDate();
				curNetwork.InternetChk = true;
				readerWriterLockSlim.ExitWriteLock();
				Console.WriteLine("lan chk :: count(" + network_list.Count + ")");
			}
			else if (wifi_chk != -1)
			{
				readerWriterLockSlim.EnterWriteLock();
				curNetwork.NetworkType = network_list[wifi_chk].NetworkType;
				curNetwork.ChkDate = getNowDate();
				curNetwork.InternetChk = true;
				readerWriterLockSlim.ExitWriteLock();
				Console.WriteLine("wifi chk :: count(" + network_list.Count + ")");
			}
			else
			{
				readerWriterLockSlim.EnterWriteLock();
				curNetwork.NetworkType = -1;
				curNetwork.ChkDate = "";
				curNetwork.InternetChk = false;
				readerWriterLockSlim.ExitWriteLock();
				Console.WriteLine("chk :: count(" + network_list.Count + ")");
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

		public string getNowDate()
		{
			string now_date = DateTime.Today.ToString();
			return now_date;
		}
	}
}
