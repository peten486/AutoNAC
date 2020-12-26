using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SimpleWifi;

namespace WIFI_TEST
{
	class Program
	{
		const string wifi_name = "WiFi";
		const string wifi_pass = "1234";

		static Wifi Wifi = new Wifi();
		static List<AccessPoint> accessPoints = new List<AccessPoint>();
		static WiFi_AP_Infro cur_ap = null;


		static void Main(string[] args)
		{
			cur_ap = new WiFi_AP_Infro(wifi_name, wifi_pass);
			
			print_wifi();

			for (int i = 0; i < accessPoints.Count; i++)
			{
				if (accessPoints[i].Name.Equals(wifi_name))
				{
					cur_ap.ap = accessPoints[i];
				}
			}

			if (cur_ap.ap != null)
			{
				wifi_conn(cur_ap.ap, wifi_pass);
			}
			else
			{
				Console.WriteLine("fail::connecting wifi");
			}
		}

		/// <summary>
		/// 연결 가능한 AccessPoint 리스트를 조회하고 Console에 출력하는 함수
		/// </summary>
		static void print_wifi()
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
		static bool get_connection_status()
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
		static void wifi_disconn()
		{
			Console.WriteLine("-->DisConnection");
			Wifi.Disconnect();
		}

		/// <summary>
		/// AccessPoint에 연결하는 함수(WiFi 연결)
		/// </summary>
		/// <param name="ap">연결할 Access Point 객체</param>
		/// <param name="password">연결할 Access Point의 비밀번호</param>
		static bool wifi_conn(AccessPoint ap, string password)
		{
			if (ap != null)
			{
				AuthRequest authRequest = new AuthRequest(ap);
				authRequest.Password = password;
				try
				{
					while (!ap.Connect(authRequest, true))
					{
						ap.Connect(authRequest);
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
