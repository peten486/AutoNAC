using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SimpleWifi;

namespace WIFI_TEST
{
	class Program
	{
		static string wifi_name = "WiFi";
		static string wifi_pass = "1234";

		static Wifi Wifi = new Wifi();
		static List<AccessPoint> accessPoints = new List<AccessPoint>();

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			print_wifi();

			AccessPoint ap = null;

			for (int i = 0; i < accessPoints.Count; i++)
			{
				if (accessPoints[i].Name.Equals(wifi_name))
				{
					ap = accessPoints[i];
				}
			}

			wifi_conn( ap, wifi_pass);

		}

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

		static void wifi_disconn()
		{
			Console.WriteLine("-->DisConnection");
			Wifi.Disconnect();
		}

		/// <summary>
		/// Tries to connect to the given access point.
		/// </summary>
		/// <param name="name">The name of the access point.</param>
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
