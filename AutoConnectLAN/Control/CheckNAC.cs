using System;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.IO.Compression;
using HtmlAgilityPack;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using AutoConnectLAN.Model;

namespace AutoConnectLAN.Control
{
	public class CheckNAC
	{
		protected IWebDriver _driver = null;
		string _driver_path = "E:\\AutoNAC\\drivers";
		string _brower_path = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";


		public string getEdgeVesrion()
		{
			string result = null;
			FileVersionInfo.GetVersionInfo(_brower_path);
			FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(_brower_path);
			//Console.WriteLine("Version number: " + myFileVersionInfo.FileVersion);
			result = myFileVersionInfo.FileVersion;
			return result;
		}

		public bool downEdgeDriver(string edgeDriverVer)
		{
			bool chk = false;

			string url = "https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver/";

			HtmlWeb web = new HtmlWeb();
			HtmlDocument htmlDoc = web.Load(url);

			HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("body");

			HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//li/p[@class='driver-download__meta']");
			string cur_link = String.Empty;
			for (int i = 0; i < nodes.Count; i++)
			{
				string inner_text = nodes[i].InnerText;
				
				if (inner_text.Contains("Version:") == false)
					continue;

				if (inner_text.Contains("x86") == false)
					continue;

				string outter_text = nodes[i].OuterHtml;

				HtmlNodeCollection sub_nodes = nodes[i].SelectNodes("a");
				for (int j = 0; j < sub_nodes.Count; j++)
				{
					string data = sub_nodes[j].OuterHtml;

					if (data.Contains("x86") == false)
						continue;

					string href_link = sub_nodes[j].GetAttributeValue("href", "");

					if (href_link.Contains(edgeDriverVer) == false)
						continue;

					return DownloadEdgeDriver(href_link);
				}
			}

			return chk;
		}

		public bool DownloadEdgeDriver(string url_path)
		{
			string _now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
			string file_name = "driver_" + _now + ".zip";
			Console.WriteLine("file : " + _driver_path + "\\" + file_name);
			string _file_path = _driver_path + "\\" + file_name;

			WebClient wc = new WebClient();
			wc.DownloadFile( url_path, _file_path);

			return unZipFile(file_name);
		}

		public bool unZipFile(string file_name)
		{
			string zipPath = _driver_path + "\\" + file_name;
			string destinationPath = _driver_path + "\\" ;
			string new_filepath = destinationPath + "MicrosoftWebDriver.exe";

			using (ZipArchive zipArchive = ZipFile.OpenRead(zipPath))
			{
				foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
				{
					string filepath = destinationPath + zipArchiveEntry.FullName;
					if (filepath.Contains("msedgedriver.exe") == false)
						continue;

					if (System.IO.File.Exists(filepath) == true)
					{
						try
						{
							System.IO.File.Delete(filepath);
							Console.WriteLine("Delete past files");
						}
						catch (System.IO.IOException e)
						{
							// handle exception
						}
					}

					zipArchiveEntry.ExtractToFile(filepath);
					
					System.IO.File.Move(filepath, new_filepath);
					Console.WriteLine("MOVE FILE PATH : " + new_filepath);
				}
			}

			return true;
		}

		public bool isCheckEdgeDriver(string version)
		{
			bool chk = false;

			string result = null;
			string file = _driver_path + "\\MicrosoftWebDriver.exe";

			if (System.IO.File.Exists(file) == false)
				return false;

			FileVersionInfo.GetVersionInfo(file);
			FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(file);
			result = myFileVersionInfo.FileVersion;

			if (result.Contains(version) == true)
			{
				chk = true;
			}
			else
			{
				chk = false;
			}

			return chk;
		}
	
		public bool isLogin(string user_id, string password)
		{
			bool chk = false;
			using (IWebDriver driver = new EdgeDriver(_driver_path))
			{
				driver.Url = "http://192.168.25.48";
				// _driver.Manage().Window.Maximize(); //브라우져 최대 확대

				Thread.Sleep(5000);

				var txt_userid = driver.FindElement(By.Id("txt-userid"));
				var txt_passwd = driver.FindElement(By.Id("pw-userpw"));
				var btn_ok = driver.FindElement(By.Id("btn-submit"));

				txt_userid.SendKeys(user_id);
				txt_passwd.SendKeys(password);
				btn_ok.Click();


				Thread.Sleep(2000);

				var result = driver.FindElement(By.XPath("/html/body")).Text;
				
				if (result.Equals("OK"))
				{
					chk = true;
				}
				else
				{
					chk = false;
				}

				driver.Close();
			}

			return chk;
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