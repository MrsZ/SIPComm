using Microsoft.Win32;
using Sipek.Common;
using Sipek.Sip;
using System.Collections.Generic;
using System.Security.AccessControl;

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
	}
}
