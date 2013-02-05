using Microsoft.Win32;
using Sipek.Common;
using System.Security.AccessControl;

namespace SIPComm
{
	public class AccountConfig : IAccount
	{
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm");

		#region Properties in Register

		public AccountConfig()
		{
			regKey.SetAccessControl(new RegistrySecurity());
		}

		public bool Enabled
		{
			get { return regKey.GetValue("AccountEnabled", "1").ToString() == "1" ? true : false; }
			set { regKey.SetValue("AccountEnabled", (value ? "1" : "0")); }
		}

		public int Index
		{
			get { return int.Parse(regKey.GetValue("AccountIndex", "0").ToString()); }
			set { regKey.SetValue("AccountIndex", value.ToString()); }
		}

		public string AccountName
		{
			get { return (string)regKey.GetValue("AccountName", "Default"); }
			set { regKey.SetValue("AccountName", (string)value); }
		}

		public string HostName
		{
			get { return (string)regKey.GetValue("AccountAddress", ""); }
			set { regKey.SetValue("AccountAddress", (string)value); }
		}

		public string Id
		{
			get { return (string)regKey.GetValue("AccountID", "1"); }
			set { regKey.SetValue("AccountID", (string)value); }
		}

		public string UserName
		{
			get { return (string)regKey.GetValue("AccountUsername", "1"); }
			set { regKey.SetValue("AccountUsername", (string)value); }
		}

		public string Password
		{
			get { return (string)regKey.GetValue("AccountPassword", ""); }
			set { regKey.SetValue("AccountPassword", (string)value); }
		}

		public string DisplayName
		{
			get { return (string)regKey.GetValue("AccountDisplayName", "1"); }
			set { regKey.SetValue("AccountDisplayName", (string)value); }
		}

		public string DomainName
		{
			get { return (string)regKey.GetValue("AccountDomain", ""); }
			set { regKey.SetValue("AccountDomain", (string)value); }
		}

		public int RegState
		{
			get { return int.Parse(regKey.GetValue("AccountState", "0").ToString()); }
			set { regKey.SetValue("AccountState", value.ToString()); }
		}

		public string ProxyAddress
		{
			get { return (string)regKey.GetValue("AccountProxyAddress", ""); }
			set { regKey.SetValue("AccountProxyAddress", (string)value); }
		}

		public ETransportMode TransportMode
		{
			get { return (ETransportMode)int.Parse(regKey.GetValue("AccountTransport", "0").ToString()); }
			set { regKey.SetValue("AccountTransport", ((int)value).ToString()); }
		}

		public int TransportModeID
		{
			get { return int.Parse(regKey.GetValue("AccountTransport", "0").ToString()); }
			set { regKey.SetValue("AccountTransport", value.ToString()); }
		}

		#endregion Properties in Register
	}
}