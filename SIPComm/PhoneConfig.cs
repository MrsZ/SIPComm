using System.Collections.Generic;
using Sipek.Common;
using Sipek.Sip;

namespace SIPComm
{
	public class PhoneConfig : IConfiguratorInterface
	{
		#region Properties

		public bool IsNull 
		{ 
			get { return false; } 
		}

		public bool CFUFlag
		{
			get { return Properties.Settings.Default.cfgCFUFlag; }
			set { Properties.Settings.Default.cfgCFUFlag = value; }
		}

		public string CFUNumber
		{
			get { return Properties.Settings.Default.cfgCFUNumber; }
			set { Properties.Settings.Default.cfgCFUNumber = value; }
		}

		public bool CFNRFlag
		{
			get { return Properties.Settings.Default.cfgCFNRFlag; }
			set { Properties.Settings.Default.cfgCFNRFlag = value; }
		}

		public string CFNRNumber
		{
			get { return Properties.Settings.Default.cfgCFNRNumber; }
			set { Properties.Settings.Default.cfgCFNRNumber = value; }
		}

		public bool DNDFlag
		{
			get { return Properties.Settings.Default.cfgDNDFlag; }
			set { Properties.Settings.Default.cfgDNDFlag = value; }
		}

		public bool AAFlag
		{
			get { return Properties.Settings.Default.cfgAAFlag; }
			set { Properties.Settings.Default.cfgAAFlag = value; }
		}

		public bool CFBFlag
		{
			get { return Properties.Settings.Default.cfgCFBFlag; }
			set { Properties.Settings.Default.cfgCFBFlag = value; }
		}

		public string CFBNumber
		{
			get { return Properties.Settings.Default.cfgCFBNumber; }
			set { Properties.Settings.Default.cfgCFBNumber = value; }
		}

		public int SIPPort
		{
			get { return Properties.Settings.Default.cfgSipPort; }
			set { Properties.Settings.Default.cfgSipPort = value; }
		}

		public bool PublishEnabled
		{
			get
			{
				SipConfigStruct.Instance.publishEnabled = Properties.Settings.Default.cfgSipPublishEnabled;
				return Properties.Settings.Default.cfgSipPublishEnabled;
			}
			set
			{
				SipConfigStruct.Instance.publishEnabled = value;
				Properties.Settings.Default.cfgSipPublishEnabled = value;
			}
		}

		public string StunServerAddress
		{
			get
			{
				SipConfigStruct.Instance.stunServer = Properties.Settings.Default.cfgStunServerAddress;
				return Properties.Settings.Default.cfgStunServerAddress;
			}
			set
			{
				Properties.Settings.Default.cfgStunServerAddress = value;
				SipConfigStruct.Instance.stunServer = value;
			}
		}

		public EDtmfMode DtmfMode
		{
			get
			{
				return (EDtmfMode)Properties.Settings.Default.cfgDtmfMode;
			}
			set
			{
				Properties.Settings.Default.cfgDtmfMode = (int)value;
			}
		}

		public int Expires
		{
			get
			{
				SipConfigStruct.Instance.expires = Properties.Settings.Default.cfgRegistrationTimeout;
				return Properties.Settings.Default.cfgRegistrationTimeout;
			}
			set
			{
				Properties.Settings.Default.cfgRegistrationTimeout = value;
				SipConfigStruct.Instance.expires = value;
			}
		}

		public int ECTail
		{
			get
			{
				SipConfigStruct.Instance.ECTail = Properties.Settings.Default.cfgECTail;
				return Properties.Settings.Default.cfgECTail;
			}
			set
			{
				Properties.Settings.Default.cfgECTail = value;
				SipConfigStruct.Instance.ECTail = value;
			}
		}

		public bool VADEnabled
		{
			get
			{
				SipConfigStruct.Instance.VADEnabled = Properties.Settings.Default.cfgVAD;
				return Properties.Settings.Default.cfgVAD;
			}
			set
			{
				Properties.Settings.Default.cfgVAD = value;
				SipConfigStruct.Instance.VADEnabled = value;
			}
		}

		public string NameServer
		{
			get
			{
				SipConfigStruct.Instance.nameServer = Properties.Settings.Default.cfgNameServer;
				return Properties.Settings.Default.cfgNameServer;
			}
			set
			{
				Properties.Settings.Default.cfgNameServer = value;
				SipConfigStruct.Instance.nameServer = value;
			}
		}

		public int DefaultAccountIndex
		{
			get
			{
				return Properties.Settings.Default.cfgAccountDefault;
			}
			set
			{
				Properties.Settings.Default.cfgAccountDefault = value;
			}
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
			get
			{
				List<string> codecList = new List<string>();
				System.Collections.Specialized.StringCollection CodecList = new System.Collections.Specialized.StringCollection();
				//CodecList = Properties.Settings.Default.cfgCodecList;
				if (null != Properties.Settings.Default.cfgCodecList)
				{
				    CodecList = Properties.Settings.Default.cfgCodecList;
				}

				foreach (string item in CodecList)
				{
					codecList.Add(item);
				}
				return codecList;
			}
			set
			{
				Properties.Settings.Default.cfgCodecList.Clear();
				List<string> cl = value;
				System.Collections.Specialized.StringCollection CodecList = new System.Collections.Specialized.StringCollection();
				foreach (string item in cl)
				{
				    CodecList.Add(item);
				}
				Properties.Settings.Default.cfgCodecList = CodecList;
			}
		}

		#endregion
		
		public void Save()
		{
			Properties.Settings.Default.Save();
		}
	}
}
