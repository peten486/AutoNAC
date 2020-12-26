using System;
using System.Collections.Generic;
using System.Text;
using SimpleWifi;

namespace AutoConnectLAN.Model
{
	class WiFi_AP_Info
	{
		public string name { get; set; }
		public string pass { get; set; }

		public AccessPoint ap { get; set; }


		public WiFi_AP_Info(string _name, string _pass)
		{
			name = _name;
			pass = _pass;
			ap = null;
		}
	}
}
