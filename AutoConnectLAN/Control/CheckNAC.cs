using System;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.IO.Compression;
using HtmlAgilityPack;
using OpenQA;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace AutoConnectLAN.Control
{
	public class CheckNAC
	{
	//	protected IWebDriver _driver = null;
		string _driver_path = "E:\\AutoNAC\\drivers";
		string _chrome_path = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
		protected ChromeDriverService _driverService = null;
		protected ChromeOptions _options = null;
		protected ChromeDriver _driver = null;

		public CheckNAC()
		{
			if (isCheckChromeDriver(getChromeVersion()) == false)
			{
				downChromeDriver(getChromeVersion());
			}

			_driverService = ChromeDriverService.CreateDefaultService(_driver_path);
			_driverService.HideCommandPromptWindow = true;

			_options = new ChromeOptions();
			_options.AddArgument("disable-gpu");
		}

		public string getChromeVersion()
		{
			string result = null;
			FileVersionInfo.GetVersionInfo(_chrome_path);
			FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(_chrome_path);
			//Console.WriteLine("Version number: " + myFileVersionInfo.FileVersion);
			result = myFileVersionInfo.FileVersion;
			return result;
		}

		public bool downChromeDriver(string driverVer)
		{
			bool chk = false;
			string down_simple_ver = "";
			string down_detail_ver = "";
			string real_detail_ver = "";
			string[] words = driverVer.Split('.');
			down_simple_ver = words[0];
			down_detail_ver = words[0] + "." + words[1] + "." + words[2];

			/* doc site :: https://www.selenium.dev/documentation/en/webdriver/driver_requirements/ */
			string url = "https://sites.google.com/a/chromium.org/chromedriver/downloads/";

			HtmlWeb web = new HtmlWeb();
			HtmlDocument htmlDoc = web.Load(url);
			HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("body");
			HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"sites-canvas-main-content\"]/table/tbody/tr/td/div/div[1]/ul/li");
			string cur_link = String.Empty;

			
			Console.WriteLine("driver version : " + driverVer);
			Console.WriteLine("simple version : " + down_simple_ver);
			Console.WriteLine("detail version : " + down_detail_ver);

			for (int i = 0; i < nodes.Count; i++)
			{
				string inner_text = nodes[i].InnerText;

				if (inner_text.Contains("Chrome version " + down_simple_ver) == false)
					continue;

				string outter_text = nodes[i].InnerText;
				Console.WriteLine("text[" + i + "] : " + outter_text);
				
				HtmlNodeCollection sub_nodes = nodes[i].SelectNodes("a");
				for (int j = 0; j < sub_nodes.Count; j++)
				{
					string data = sub_nodes[j].OuterHtml;
					string href_link = sub_nodes[j].GetAttributeValue("href", "");

					Console.WriteLine("href_link : " + href_link);
					real_detail_ver = href_link.Substring(href_link.LastIndexOf("=")+1);
					if (href_link.Contains(down_detail_ver) == false)
						continue;

					return DownloadDriver("https://chromedriver.storage.googleapis.com/" + real_detail_ver + "chromedriver_win32.zip");
				}
			}

			return chk;
		}

		public bool DownloadDriver(string url_path)
		{
			Console.WriteLine("download url : " + url_path);
			string _now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
			string file_name = "driver_" + _now + ".zip";
			Console.WriteLine("file : " + _driver_path + "\\" + file_name);
			string _file_path = _driver_path + "\\" + file_name;

			WebClient wc = new WebClient();
			wc.DownloadFile(url_path, _file_path);

			return unZipFile(file_name);
		}


		public bool unZipFile(string file_name)
		{
			string zipPath = _driver_path + "\\" + file_name;
			string destinationPath = _driver_path + "\\";
			//string new_filepath = destinationPath + "MicrosoftWebDriver.exe";

			using (ZipArchive zipArchive = ZipFile.OpenRead(zipPath))
			{
				foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
				{
					string filepath = destinationPath + zipArchiveEntry.FullName;
					if (filepath.Contains("chromedriver.exe") == false)
						continue;

					if (File.Exists(filepath) == true)
					{
						try
						{
							File.Delete(filepath);
							Console.WriteLine("Delete past files");
						}
						catch (IOException e)
						{
							// handle exception
						}
					}

					zipArchiveEntry.ExtractToFile(filepath);

					//System.IO.File.Move(filepath, new_filepath);
					//Console.WriteLine("MOVE FILE PATH : " + new_filepath);
				}
			}

			return true;
		}

		
		public bool isCheckChromeDriver(string version)
		{
			string destinationPath = _driver_path + "\\";
			string file = destinationPath + "chromedriver.exe";

			Console.WriteLine("file : " + file);

			if (File.Exists(file) == true)
			{
				// 읽기전용 옵션 해제를 위함
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
				Console.WriteLine("Delete past files");
				
			}
				
			return false;
		}

		public bool isLogin(string user_id, string password)
		{
			bool chk = false;

			_options.AddArgument("headless"); // 숨김

			_driver = new ChromeDriver(_driverService, _options);
			_driver.Navigate().GoToUrl("http://192.168.25.57/");
			_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


			Thread.Sleep(1000);

			var txt_userid = _driver.FindElement(By.Id("txt-userid"));
			var txt_passwd = _driver.FindElement(By.Id("pw-userpw"));
			var btn_ok = _driver.FindElement(By.Id("btn-submit"));

			txt_userid.SendKeys(user_id);
			txt_passwd.SendKeys(password);
			btn_ok.Click();

			var result = _driver.FindElement(By.XPath("/html/body")).Text;

			if (result.Equals("OK"))
			{
				Console.WriteLine("isLogin :: OK(user:" + user_id + ")");
				chk = true;
			}
			else
			{
				chk = false;
			}

			_driver.Quit();

			return chk;
		}

		public void closeDriver()
		{
			_driver.Close();
		}

	}

}

/*

The MicrosoftWebDriver.exe file does not exist in the current directory or in a directory on the PATH environment variable. The driver can be downloaded at http://go.microsoft.com/fwlink/?LinkId=619687

 */

/*
				var txt_userid = _driver.FindElement(By.Id("txt-userid"));
				var txt_passwd = _driver.FindElement(By.Id("pw-userpw"));
				var btn_ok = _driver.FindElement(By.Id("btn-submit"));

				txt_userid.SendKeys(user_id);
				txt_passwd.SendKeys(password);
				btn_ok.Click();
				*/