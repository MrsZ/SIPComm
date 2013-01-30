using Microsoft.Win32;
using Sipek.Common;
using System.Security.AccessControl;

namespace SIPComm
{
	public class AccountConfig : IAccount
	{
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm\\Account");

		#region Properties in Register

		public AccountConfig()
		{
			regKey.SetAccessControl(new RegistrySecurity());
		}

		public bool Enabled
		{
			get { return (int)regKey.GetValue("cfgAccountEnabled", 1) == 1 ? true : false; }
			set { regKey.SetValue("cfgAccountEnabled", (int)(value ? 1 : 0)); }
		}

		public int Index
		{
			get { return (int)regKey.GetValue("cfgAccountIndex", 0); }
			set { regKey.SetValue("cfgAccountIndex", (int)value); }
		}

		public string AccountName
		{
			get { return (string)regKey.GetValue("cfgAccountName", "Default"); }
			set { regKey.SetValue("cfgAccountName", (string)value); }
		}

		public string HostName
		{
			get { return (string)regKey.GetValue("cfgAccountAddress", "10.10.10.1:5070"); }
			set { regKey.SetValue("cfgAccountAddress", (string)value); }
		}

		public string Id
		{
			get { return (string)regKey.GetValue("cfgAccountID", "1019"); }
			set { regKey.SetValue("cfgAccountID", (string)value); }
		}

		public string UserName
		{
			get { return (string)regKey.GetValue("cfgAccountUsername", "1019"); }
			set { regKey.SetValue("cfgAccountUsername", (string)value); }
		}

		public string Password
		{
			get { return (string)regKey.GetValue("cfgAccountPassword", "it-sfera.fs"); }
			set { regKey.SetValue("cfgAccountPassword", (string)value); }
		}

		public string DisplayName
		{
			get { return (string)regKey.GetValue("cfgAccountDisplayName", "1019"); }
			set { regKey.SetValue("cfgAccountDisplayName", (string)value); }
		}

		public string DomainName
		{
			get { return (string)regKey.GetValue("cfgAccountDomain", "10.10.10.4"); }
			set { regKey.SetValue("cfgAccountDomain", (string)value); }
		}

		public int RegState
		{
			get { return (int)regKey.GetValue("cfgAccountState", 0); }
			set { regKey.SetValue("cfgAccountState", (int)value); }
		}

		public string ProxyAddress
		{
			get { return (string)regKey.GetValue("cfgAccountProxyAddress", ""); }
			set { regKey.SetValue("cfgAccountProxyAddress", (string)value); }
		}

		public ETransportMode TransportMode
		{
			get { return (ETransportMode)regKey.GetValue("cfgAccountTransport", 0); }
			set { regKey.SetValue("cfgAccountTransport", (int)value); }
		}

		#endregion Properties in Register
	}
}