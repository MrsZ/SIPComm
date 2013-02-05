using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SIPComm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		System.Threading.Mutex mutex;
		private void App_Startup(object sender, StartupEventArgs e)
		{
			bool createdNew;
			string mutName = "SIPComm";
			mutex = new System.Threading.Mutex(true, mutName, out createdNew);
			if (!createdNew)
			{
				Environment.Exit(0);
				Shutdown();
			}
		}
    }
}
