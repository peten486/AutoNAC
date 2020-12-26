
using SimpleWifi;
using System;
using System.Collections.Generic;
using System.Text;
using AutoConnectLAN.Model;

namespace AutoConnectLAN.Control
{
	public class WIFI_Process
	{
		//string wifi_name = "WIFI";
		static string wifi_pass = "1234";

		static Wifi Wifi = null;
		static List<AccessPoint> accessPoints = null;
		static WiFi_AP_Info cur_ap = null;

		public WIFI_Process()
		{

			Wifi = new Wifi();
			accessPoints = new List<AccessPoint>();
			cur_ap = null;
		}

		/// <summary>
		/// 연결 가능한 AccessPoint 리스트를 조회하고 Console에 출력하는 함수
		/// </summary>
		public void print_wifi()
		{
			accessPoints = Wifi.GetAccessPoints();

			for (int i = 0; i < accessPoints.Count; i++)
			{
				Console.WriteLine("name : " + accessPoints[0].Name);
				Console.WriteLine("SignalStrength : " + accessPoints[0].SignalStrength);
				Console.WriteLine("HasProfile : " + accessPoints[0].HasProfile);
				Console.WriteLine("IsSecure : " + accessPoints[0].IsSecure);
				Console.WriteLine("IsConnected : " + accessPoints[0].IsConnected);
			}
		}


		/// <summary>
		/// 현재 Wi-Fi Connection 상태( true : Connected, false : Disconnected )
		/// </summary>
		public bool get_connection_status()
		{
			if (Wifi.ConnectionStatus == WifiStatus.Connected)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 연결된 AccessPoint에서 연결 해제하는 함수(WiFi 연결 해제)
		/// </summary>
		public void wifi_disconn()
		{
			Console.WriteLine("-->DisConnection");
			Wifi.Disconnect();
		}

		/// <summary>
		/// AccessPoint에 연결하는 함수(WiFi 연결)
		/// </summary>
		public bool wifi_conn()
		{
			if (cur_ap.ap != null)
			{
				AuthRequest authRequest = new AuthRequest(cur_ap.ap);
				authRequest.Password = wifi_pass;
				try
				{
					while (!cur_ap.ap.Connect(authRequest, true))
					{
						cur_ap.ap.Connect(authRequest);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				return true;
			}
			return false;
		}
	}
}
