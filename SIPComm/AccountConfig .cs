using Sipek.Common;
using Microsoft.Win32;
using System.Security.AccessControl;

namespace SIPComm
{
	public class AccountConfig : IAccount
	{
		#region Properties in File

		//public bool Enabled
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountEnabled;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountEnabled = value;
		//	}
		//}

		//public int Index
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountIndex;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountIndex = value;
		//	}
		//}

		//public string AccountName
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountName;

		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountName = value;
		//	}
		//}

		//public string HostName
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountAddress;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountAddress = value;
		//	}
		//}

		//public string Id
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountID;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountID = value;
		//	}
		//}

		//public string UserName
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountUsername;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountUsername = value;
		//	}
		//}

		//public string Password
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountPassword;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountPassword = value;
		//	}
		//}

		//public string DisplayName
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountDisplayName;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountDisplayName = value;
		//	}
		//}

		//public string DomainName
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountDomain;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountDomain = value;
		//	}
		//}

		//public int RegState
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountState;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountState = value;
		//	}
		//}

		//public string ProxyAddress
		//{
		//	get
		//	{
		//		return Properties.Settings.Default.cfgAccountProxyAddress;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountProxyAddress = value;
		//	}
		//}

		//public ETransportMode TransportMode
		//{
		//	get
		//	{
		//		return (ETransportMode)Properties.Settings.Default.cfgAccountTransport;
		//	}
		//	set
		//	{
		//		Properties.Settings.Default.cfgAccountTransport = (int)value;
		//	}
		//}

		#endregion

		#region Properties in Register

		public AccountConfig()
		{
			regKey.SetAccessControl(new RegistrySecurity());
		}

		RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm\\vars");

		public bool Enabled
		{
			get
			{
				return (bool)regKey.GetValue("cfgAccountEnabled", true);
			}
			set
			{
				regKey.SetValue("cfgAccountEnabled", (bool)value);
			}
		}

		public int Index
		{
			get
			{
				return (int)regKey.GetValue("cfgAccountIndex", 0);
			}
			set
			{
				regKey.SetValue("cfgAccountIndex", (int)value);
			}
		}

		public string AccountName
		{
			get
			{
				return (string)regKey.GetValue("cfgAccountName", "Default");

			}
			set
			{
				regKey.SetValue("cfgAccountName", (string)value);
			}
		}

		public string HostName
		{
			get
			{
				return (string)regKey.GetValue("cfgAccountAddress", "10.10.10.4");
			}
			set
			{
				regKey.SetValue("cfgAccountAddress", (string)value);
			}
		}

		public string Id
		{
			get
			{
				return (string)regKey.GetValue("cfgAccountID", "1019");
			}
			set
			{
				regKey.SetValue("cfgAccountID", (string)value);
			}
		}

		public string UserName
		{
			get
			{
				return (string)regKey.GetValue("cfgAccountUsername", "1019");
			}
			set
			{
				regKey.SetValue("cfgAccountUsername", (string)value);
			}
		}

		public string Password
		{
			get
			{
				return (string)regKey.GetValue("cfgAccountPassword", "it-sfera.fs");
			}
			set
			{
				regKey.SetValue("cfgAccountPassword", (string)value);
			}
		}

		public string DisplayName
		{
			get
			{
				return (string)regKey.GetValue("cfgAccountDisplayName", "1019");
			}
			set
			{
				regKey.SetValue("cfgAccountDisplayName", (string)value);
			}
		}

		public string DomainName
		{
			get
			{
				return (string)regKey.GetValue("cfgAccountDomain", "10.10.10.4");
			}
			set
			{
				regKey.SetValue("cfgAccountDomain", (string)value);
			}
		}

		public int RegState
		{
			get
			{
				return (int)regKey.GetValue("cfgAccountState", 0);
			}
			set
			{
				regKey.SetValue("cfgAccountState", (int)value);
			}
		}

		public string ProxyAddress
		{
			get
			{
				return (string)regKey.GetValue("cfgAccountProxyAddress", "");
			}
			set
			{
				regKey.SetValue("cfgAccountProxyAddress", (string)value);
			}
		}

		public ETransportMode TransportMode
		{
			get
			{
				return (ETransportMode)regKey.GetValue("cfgAccountTransport", ETransportMode.TM_UDP);
			}
			set
			{
				regKey.SetValue("cfgAccountTransport", (ETransportMode)value);
			}
		}

		#endregion

	}
}
