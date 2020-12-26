using SimpleWifi;
using System;
using System.Collections.Generic;
using System.Text;

namespace WIFI_TEST
{
	class WiFi_AP_Infro
	{
		public string name { get; set; }
		public string pass { get; set; }

		public AccessPoint ap { get; set; }


		public WiFi_AP_Infro(string _name, string _pass)
		{
			name = _name;
			pass = _pass;
			ap = null;
		}
	}
}
