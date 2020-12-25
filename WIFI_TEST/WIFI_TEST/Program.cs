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
			wifi_conn(wifi_name, wifi_pass);

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
		static void wifi_conn(string wifi_name, string wifi_pass)
		{
			var accessPoints = GetAccessPointsOrderedBySignalStrength();
			foreach (var accessPoint in accessPoints)
			{
				if (accessPoint.Name.Equals(name))
				{
					if (accessPoint.IsConnected)
					{
						Debug.WriteLine($"Already connected to access point named {accessPoint.Name}.\n");
						return;
					}

					var authRequest = new AuthRequest(accessPoint);
					var overwrite = true;

					if (authRequest.IsPasswordRequired)
					{
						if (accessPoint.HasProfile)
						{
							// Console.Write("\r\nA network profile already exist, do you want to use it (y/n)? ");
							overwrite = false;
						}
					}

					if (overwrite)
					{
						if (authRequest.IsUsernameRequired)
						{
							// Popup window for username
							authRequest.Username = wifi_name;
						}

						// Popup window for password
						authRequest.Password = wifi_pass;

						if (authRequest.IsDomainSupported)
						{
							// Popup window for domain. Is this really necessary???
						}
					}

					accessPoint.ConnectAsync(authRequest, overwrite, OnConnectionCompleted);
					return;
				}
			}
		}

		/// <summary>
		/// Returns a list of access points ordered by their signal strength.
		/// </summary>
		/// <returns>
		/// An IEnumerable of AccessPoints.
		/// </returns>
		public static IEnumerable<AccessPoint> GetAccessPointsOrderedBySignalStrength()
		{
			var accessPoints = Wifi.GetAccessPoints();
			accessPoints.RemoveAll(ap => string.IsNullOrEmpty(ap.Name));
			var orderedAccessPoints = accessPoints.OrderByDescending(ap => ap.SignalStrength);

			return orderedAccessPoints;
		}


	}
}
