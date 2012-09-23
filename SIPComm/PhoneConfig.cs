using Sipek.Common;
using Sipek.Sip;
using System.Collections.Generic;

namespace SIPComm
{
	public class PhoneConfig : IConfiguratorInterface
	{
		private RegSet RegSet;

		#region Properties

		public PhoneConfig()
		{
			RegSet = new RegSet();
		}

		public bool IsNull
		{
			get { return false; }
		}

		public bool CFUFlag
		{
			get { return RegSet.CFUFlag; }
			set { RegSet.CFUFlag = value; }
		}

		public string CFUNumber
		{
			get { return RegSet.CFUNumber; }
			set { RegSet.CFUNumber = value; }
		}

		public bool CFNRFlag
		{
			get { return RegSet.CFNRFlag; }
			set { RegSet.CFNRFlag = value; }
		}

		public string CFNRNumber
		{
			get { return RegSet.CFNRNumber; }
			set { RegSet.CFNRNumber = value; }
		}

		public bool DNDFlag
		{
			get { return RegSet.DNDFlag; }
			set { RegSet.DNDFlag = value; }
		}

		public bool AAFlag
		{
			get { return RegSet.AAFlag; }
			set { RegSet.AAFlag = value; }
		}

		public bool CFBFlag
		{
			get { return RegSet.CFBFlag; }
			set { RegSet.CFBFlag = value; }
		}

		public string CFBNumber
		{
			get { return RegSet.CFBNumber; }
			set { RegSet.CFBNumber = value; }
		}

		public int SIPPort
		{
			get { return RegSet.SIPPort; }
			set { RegSet.SIPPort = value; }
		}

		public bool PublishEnabled
		{
			get
			{
				SipConfigStruct.Instance.publishEnabled = RegSet.PublishEnabled;
				return RegSet.PublishEnabled;
			}
			set
			{
				SipConfigStruct.Instance.publishEnabled = value;
				RegSet.PublishEnabled = value;
			}
		}

		public string StunServerAddress
		{
			get
			{
				SipConfigStruct.Instance.stunServer = RegSet.StunServerAddress;
				return RegSet.StunServerAddress;
			}
			set
			{
				RegSet.StunServerAddress = value;
				SipConfigStruct.Instance.stunServer = value;
			}
		}

		public EDtmfMode DtmfMode
		{
			get { return RegSet.DtmfMode; }
			set { RegSet.DtmfMode = value; }
		}

		public int Expires
		{
			get
			{
				SipConfigStruct.Instance.expires = RegSet.Expires;
				return RegSet.Expires;
			}
			set
			{
				RegSet.Expires = value;
				SipConfigStruct.Instance.expires = value;
			}
		}

		public int ECTail
		{
			get
			{
				SipConfigStruct.Instance.ECTail = RegSet.ECTail;
				return RegSet.ECTail;
			}
			set
			{
				RegSet.ECTail = value;
				SipConfigStruct.Instance.ECTail = value;
			}
		}

		public bool VADEnabled
		{
			get
			{
				SipConfigStruct.Instance.VADEnabled = RegSet.VADEnabled;
				return RegSet.VADEnabled;
			}
			set
			{
				RegSet.VADEnabled = value;
				SipConfigStruct.Instance.VADEnabled = value;
			}
		}

		public string NameServer
		{
			get
			{
				SipConfigStruct.Instance.nameServer = RegSet.NameServer;
				return RegSet.NameServer;
			}
			set
			{
				RegSet.NameServer = value;
				SipConfigStruct.Instance.nameServer = value;
			}
		}

		public int DefaultAccountIndex
		{
			get { return RegSet.DefaultAccountIndex; }
			set { RegSet.DefaultAccountIndex = value; }
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
			get { return RegSet.CodecList; }
			set { RegSet.CodecList = value; }
		}

		#endregion Properties

		public void Save()
		{			
		}
	}
}