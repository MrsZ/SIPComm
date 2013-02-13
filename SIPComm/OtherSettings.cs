using Microsoft.Win32;
using Sipek.Common;
using Sipek.Sip;
using System.Collections.Generic;
using System.Security.AccessControl;
using WaveLib.AudioMixer;

namespace SIPComm
{
	class OtherSettings
	{
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm");

		public OtherSettings()
		{
			regKey.SetAccessControl(new RegistrySecurity());
		}

		#region Properties

		public bool StartHidden
		{
			get { return regKey.GetValue("StartHidden", "1").ToString() == "1" ? true : false; }
			set { regKey.SetValue("StartHidden", (value ? "1" : "0")); }
		}

		public bool UseAgentUI
		{
			get { return regKey.GetValue("UseAgentUI", "0").ToString() == "1" ? true : false; }
			set { regKey.SetValue("UseAgentUI", (value ? "1" : "0")); }
		}

		public bool SimpleClient
		{
			get { return regKey.GetValue("SimpleClient", "0").ToString() == "1" ? true : false; }
			set { regKey.SetValue("SimpleClient", (value ? "1" : "0")); }
		}

		#endregion Properties

		public int SelectedOutput
		{
			get { return int.Parse(regKey.GetValue("SelectedOutput", "0").ToString()); }
			set { regKey.SetValue("SelectedOutput", value); }
		}

		public int SelectedInput
		{
			get { return int.Parse(regKey.GetValue("SelectedInput", "0").ToString()); }
			set { regKey.SetValue("SelectedInput", value); }
		}

		public string OutputName
		{
			get { return regKey.GetValue("OutputName", "Default").ToString(); }
			set { regKey.SetValue("OutputName", value); }
		}

		public string InputName
		{
			get { return regKey.GetValue("InputName", "Default").ToString(); }
			set { regKey.SetValue("InputName", value); }
		}

	}
}
