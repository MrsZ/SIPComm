using Microsoft.Win32;
using Sipek.Common;
using Sipek.Sip;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace SIPComm
{
	public class PhoneConfig : IConfiguratorInterface
	{
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm\\PhoneConfig");

		#region Properties

		public PhoneConfig()
		{
			regKey.SetAccessControl(new RegistrySecurity());
		}

		public bool IsNull
		{
			get { return false; }
		}

		public bool CFUFlag
		{
			get { return (int)regKey.GetValue("cfgCFUFlag", 0) == 1 ? true : false; }
			set { regKey.SetValue("cfgCFUFlag", (int)(value ? 1 : 0)); }
		}

		public string CFUNumber
		{
			get { return (string)regKey.GetValue("cfgCFUNumber", ""); }
			set { regKey.SetValue("cfgCFUNumber", (string)value); }
		}

		public bool CFNRFlag
		{
			get { return (int)regKey.GetValue("cfgCFNRFlag", 0) == 1 ? true : false; }
			set { regKey.SetValue("cfgCFNRFlag", (int)(value ? 1 : 0)); }
		}

		public string CFNRNumber
		{
			get { return (string)regKey.GetValue("cfgCFNRNumber", ""); }
			set { regKey.SetValue("cfgCFNRNumber", (string)value); }
		}

		public bool DNDFlag
		{
			get { return (int)regKey.GetValue("cfgDNDFlag", 0) == 1 ? true : false; }
			set { regKey.SetValue("cfgDNDFlag", (int)(value ? 1 : 0)); }
		}

		public bool AAFlag
		{
			get { return (int)regKey.GetValue("cfgAAFlag", 0) == 1 ? true : false; }
			set { regKey.SetValue("cfgAAFlag", (int)(value ? 1 : 0)); }
		}

		public bool CFBFlag
		{
			get { return (int)regKey.GetValue("cfgCFBFlag", 0) == 1 ? true : false; }
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
			get
			{
				SipConfigStruct.Instance.publishEnabled = (int)regKey.GetValue("cfgSipPublishEnabled", 1) == 1 ? true : false; ;
				return SipConfigStruct.Instance.publishEnabled;
			}
			set
			{
				SipConfigStruct.Instance.publishEnabled = value;
				regKey.SetValue("cfgSipPublishEnabled", (int)(value ? 1 : 0)); 
			}
		}

		public string StunServerAddress
		{
			get
			{
				SipConfigStruct.Instance.stunServer = (string)regKey.GetValue("cfgStunServerAddress", "");
				return SipConfigStruct.Instance.stunServer;
			}
			set
			{
				regKey.SetValue("cfgSipPublishEnabled", (string)value); 
				SipConfigStruct.Instance.stunServer = value;
			}
		}

		public EDtmfMode DtmfMode
		{
			get { return (EDtmfMode)regKey.GetValue("cfgDtmfMode", 0); }
			set { regKey.SetValue("cfgDtmfMode", (int)value); }
		}

		public int Expires
		{
			get
			{
				SipConfigStruct.Instance.expires = (int)regKey.GetValue("cfgRegistrationTimeout", 3600);
				return SipConfigStruct.Instance.expires;
			}
			set
			{
				regKey.SetValue("cfgRegistrationTimeout", (int)value);
				SipConfigStruct.Instance.expires = value;
			}
		}

		public int ECTail
		{
			get
			{
				SipConfigStruct.Instance.ECTail = (int)regKey.GetValue("cfgECTail", 200);
				return SipConfigStruct.Instance.ECTail;
			}
			set
			{
				regKey.SetValue("cfgECTail", (int)value);
				SipConfigStruct.Instance.ECTail = value;
			}
		}

		public bool VADEnabled
		{
			get
			{
				SipConfigStruct.Instance.VADEnabled = (int)regKey.GetValue("cfgVAD", 1) == 1 ? true : false;
				return SipConfigStruct.Instance.VADEnabled;
			}
			set
			{
				regKey.SetValue("cfgVAD", (int)(value ? 1 : 0));
				SipConfigStruct.Instance.VADEnabled = value;
			}
		}

		public string NameServer
		{
			get
			{
				SipConfigStruct.Instance.nameServer = (string)regKey.GetValue("cfgNameServer", "");
				return SipConfigStruct.Instance.nameServer;
			}
			set
			{
				regKey.SetValue("cfgNameServer", (string)value);
				SipConfigStruct.Instance.nameServer = value;
			}
		}

		public int DefaultAccountIndex
		{
			get { return (int)regKey.GetValue("cfgAccountDefault", 0); }
			set { regKey.SetValue("cfgAccountDefault", (int)value); }
		}

		public List<IAccount> Accounts
		{
			get
			{
				List<IAccount> accList = new List<IAccount>();
				for (int i = 0; i < 1; i++)
				{
					AccountConfig item = new AccountConfig();
					accList.Add(item);
				}
				return accList;
			}
		}

		public List<string> CodecList
		{
			get { return (List<string>)regKey.GetValue("cfgCodecList", new List<string>()); }
			set { regKey.SetValue("cfgCodecList", (List<string>)value); }
		}

		#endregion Properties

		public void Save()
		{			
		}
	}
}