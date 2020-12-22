using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AutoConnectLAN
{
	class Controller
	{
		bool run_flag = false;
		ReaderWriterLockSlim readerWriterLockSlim = null;
		CheckNAC nAC = null;
		CurNetwork curNetwork = null;

		public Controller()
		{
			run_flag = false;
			readerWriterLockSlim = new ReaderWriterLockSlim();
			nAC = new CheckNAC();
			curNetwork = new CurNetwork();

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

				curNetwork.InternetChk = false;


			}
		}

		void THR_CheckLoginNAC()
		{
			while (run_flag == false)
			{
			}
		}
	}
}
