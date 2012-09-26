using Microsoft.Win32;
using Sipek.Common;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace SIPComm
{
	public class RegSet
	{
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm\\vars");

		public RegSet()
		{
			regKey.SetAccessControl(new RegistrySecurity());
		}

		#region Account Properties

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
			get { return (string)regKey.GetValue("cfgAccountAddress", "10.10.10.4"); }
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

		#endregion Account Properties

		#region SIP Properties

		public bool IsNull
		{
			get { return false; }
		}

		public bool CFUFlag
		{
			get { return (int)regKey.GetValue("cfgCFUFlag", 1) == 1 ? true : false; }
			set { regKey.SetValue("cfgCFUFlag", (int)(value ? 1 : 0)); }
		}

		public string CFUNumber
		{
			get { return (string)regKey.GetValue("cfgCFUNumber", ""); }
			set { regKey.SetValue("cfgCFUNumber", (string)value); }
		}

		public bool CFNRFlag
		{
			get { return (int)regKey.GetValue("cfgCFNRFlag", 1) == 1 ? true : false; }
			set { regKey.SetValue("cfgCFNRFlag", (int)(value ? 1 : 0)); }
		}

		public string CFNRNumber
		{
			get { return (string)regKey.GetValue("cfgCFNRNumber", ""); }
			set { regKey.SetValue("cfgCFNRNumber", (string)value); }
		}

		public bool DNDFlag
		{
			get { return (int)regKey.GetValue("cfgDNDFlag", 1) == 1 ? true : false; }
			set { regKey.SetValue("cfgDNDFlag", (int)(value ? 1 : 0)); }
		}

		public bool AAFlag
		{
			get { return (int)regKey.GetValue("cfgAAFlag", 1) == 1 ? true : false; }
			set { regKey.SetValue("cfgAAFlag", (int)(value ? 1 : 0)); }
		}

		public bool CFBFlag
		{
			get { return (int)regKey.GetValue("cfgCFBFlag", 1) == 1 ? true : false; }
			set { regKey.SetValue("cfgCFBFlag", (int)(value ? 1 : 0)); }
		}

		public string CFBNumber
		{
			get { return (string)regKey.GetValue("cfgCFBNumber", ""); }
			set { regKey.SetValue("cfgCFBNumber", (string)value); }
		}

		public int SIPPort
		{
			get { return (int)regKey.GetValue("cfgSipPort", 5070); }
			set { regKey.SetValue("cfgSipPort", (int)value); }
		}

		public bool PublishEnabled
		{
			get { return (int)regKey.GetValue("cfgSipPublishEnabled", 1) == 1 ? true : false; }
			set { regKey.SetValue("cfgSipPublishEnabled", (int)(value ? 1 : 0)); }
		}

		public string StunServerAddress
		{
			get { return (string)regKey.GetValue("cfgStunServerAddress", ""); }
			set { regKey.SetValue("cfgSipPublishEnabled", (string)value); }
		}

		public EDtmfMode DtmfMode
		{
			get { return (EDtmfMode)regKey.GetValue("cfgDtmfMode", 0); }
			set { regKey.SetValue("cfgDtmfMode", (int)value); }
		}

		public int Expires
		{
			get { return (int)regKey.GetValue("cfgRegistrationTimeout", 3600); }
			set { regKey.SetValue("cfgRegistrationTimeout", (int)value); }
		}

		public int ECTail
		{
			get { return (int)regKey.GetValue("cfgECTail", 200); }
			set { regKey.SetValue("cfgECTail", (int)value); }
		}

		public bool VADEnabled
		{
			get { return (int)regKey.GetValue("cfgVAD", 1) == 1 ? true : false; }
			set { regKey.SetValue("cfgVAD", (int)(value ? 1 : 0)); }
		}

		public string NameServer
		{
			get { return (string)regKey.GetValue("cfgNameServer", ""); }
			set { regKey.SetValue("cfgNameServer", (string)value); }
		}

		public int DefaultAccountIndex
		{
			get { return (int)regKey.GetValue("cfgAccountDefault", 0); }
			set { regKey.SetValue("cfgAccountDefault", (int)value); }
		}

		public List<string> CodecList
		{
			get { return (List<string>)regKey.GetValue("cfgCodecList", new List<string>()); }
			set { regKey.SetValue("cfgCodecList", (List<string>)value); }
		}

		#endregion SIP Properties

		#region Other Properties

		public bool AutoRegister
		{
			get { return (int)regKey.GetValue("AutoRegister", 1) == 1 ? true : false; }
			set { regKey.SetValue("AutoRegister", (int)(value ? 1 : 0)); }
		}

		public bool UpdgradeSettings
		{
			get { return (int)regKey.GetValue("UpdgradeSettings", 1) == 1 ? true : false; }
			set { regKey.SetValue("UpdgradeSettings", (int)(value ? 1 : 0)); }
		}

		public int AATimeout
		{
			get { return (int)regKey.GetValue("cfgAATimeout", 10); }
			set { regKey.SetValue("cfgAATimeout", (int)value); }
		}

		public bool TrayMessageEnabled
		{
			get { return (int)regKey.GetValue("TrayMessageEnabled", 1) == 1 ? true : false; }
			set { regKey.SetValue("TrayMessageEnabled", (int)(value ? 1 : 0)); }
		}

		public bool StartHidden
		{
			get { return (int)regKey.GetValue("StartHidden", 0) == 1 ? true : false; }
			set { regKey.SetValue("StartHidden", (int)(value ? 1 : 0)); }
		}

		#endregion Other Properties
	}
}