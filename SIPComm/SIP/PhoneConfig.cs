using Microsoft.Win32;
using Sipek.Common;
using Sipek.Sip;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace SIPComm
{
	public class PhoneConfig : IConfiguratorInterface
	{
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm");

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
			get { return regKey.GetValue("CFUFlag", "0").ToString() == "1" ? true : false; }
			set { regKey.SetValue("CFUFlag", (value ? "1" : "0")); }
		}

		public string CFUNumber
		{
			get { return (string)regKey.GetValue("CFUNumber", ""); }
			set { regKey.SetValue("CFUNumber", (string)value); }
		}

		public bool CFNRFlag
		{
			get { return regKey.GetValue("cfgCFNRFlag", "0").ToString() == "1" ? true : false; }
			set { regKey.SetValue("cfgCFNRFlag", (value ? "1" : "0")); }
		}

		public string CFNRNumber
		{
			get { return (string)regKey.GetValue("cfgCFNRNumber", ""); }
			set { regKey.SetValue("cfgCFNRNumber", (string)value); }
		}

		public bool DNDFlag
		{
			get { return regKey.GetValue("cfgDNDFlag", "0").ToString() == "1" ? true : false; }
			set { regKey.SetValue("cfgDNDFlag", (value ? "1" : "0")); }
		}

		public bool AAFlag
		{
			get { return regKey.GetValue("cfgAAFlag", "0").ToString() == "1" ? true : false; }
			set { regKey.SetValue("cfgAAFlag", (value ? "1" : "0")); }
		}

		public bool CFBFlag
		{
			get { return regKey.GetValue("cfgCFBFlag", "0").ToString() == "1" ? true : false; }
			set { regKey.SetValue("cfgCFBFlag", (value ? "1" : "0")); }
		}

		public string CFBNumber
		{
			get { return (string)regKey.GetValue("cfgCFBNumber", ""); }
			set { regKey.SetValue("cfgCFBNumber", (string)value); }
		}

		public int SIPPort
		{
			get { return int.Parse(regKey.GetValue("SipPort", "5060").ToString()); }
			set { regKey.SetValue("SipPort", value.ToString()); }
		}

		public bool PublishEnabled
		{
			get
			{
				SipConfigStruct.Instance.publishEnabled = regKey.GetValue("PublishEnabled", "1").ToString() == "1" ? true : false;
				return SipConfigStruct.Instance.publishEnabled;
			}
			set
			{
				SipConfigStruct.Instance.publishEnabled = value;
				regKey.SetValue("PublishEnabled", (value ? "1" : "0")); 
			}
		}

		public string StunServerAddress
		{
			get
			{
				SipConfigStruct.Instance.stunServer = (string)regKey.GetValue("StunServerAddress", "");
				return SipConfigStruct.Instance.stunServer;
			}
			set
			{
				regKey.SetValue("SipPublishEnabled", (string)value); 
				SipConfigStruct.Instance.stunServer = value;
			}
		}

		public EDtmfMode DtmfMode
		{
			get { return (EDtmfMode)int.Parse(regKey.GetValue("DtmfMode", "0").ToString()); }
			set { regKey.SetValue("DtmfMode", ((int)value).ToString()); }
		}

		public int DtmfModeID
		{
			get { return int.Parse(regKey.GetValue("DtmfMode", "0").ToString()); }
			set { regKey.SetValue("DtmfMode", ((int)value).ToString()); }
		}

		public int Expires
		{
			get
			{
				SipConfigStruct.Instance.expires = int.Parse(regKey.GetValue("RegistrationTimeout", "3600").ToString());
				return SipConfigStruct.Instance.expires;
			}
			set
			{
				regKey.SetValue("RegistrationTimeout", ((int)value).ToString());
				SipConfigStruct.Instance.expires = value;
			}
		}

		public int ECTail
		{
			get
			{
				SipConfigStruct.Instance.ECTail = int.Parse(regKey.GetValue("ECTail", "200").ToString());
				return SipConfigStruct.Instance.ECTail;
			}
			set
			{
				regKey.SetValue("ECTail", ((int)value).ToString());
				SipConfigStruct.Instance.ECTail = value;
			}
		}

		public bool VADEnabled
		{
			get
			{
				SipConfigStruct.Instance.VADEnabled = regKey.GetValue("VAD", "1").ToString() == "1" ? true : false;
				return SipConfigStruct.Instance.VADEnabled;
			}
			set
			{
				regKey.SetValue("VAD", (value ? "1" : "0"));
				SipConfigStruct.Instance.VADEnabled = value;
			}
		}

		public string NameServer
		{
			get
			{
				SipConfigStruct.Instance.nameServer = (string)regKey.GetValue("NameServer", "");
				return SipConfigStruct.Instance.nameServer;
			}
			set
			{
				regKey.SetValue("NameServer", (string)value);
				SipConfigStruct.Instance.nameServer = value;
			}
		}

		public int DefaultAccountIndex
		{
			get { return int.Parse(regKey.GetValue("AccountDefault", "0").ToString()); }
			set { regKey.SetValue("AccountDefault", ((int)value).ToString()); }
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
			get { return (List<string>)regKey.GetValue("CodecList", new List<string>()); }
			set { regKey.SetValue("CodecList", (List<string>)value); }
		}

		#endregion Properties

		public void Save()
		{			
		}
	}
}