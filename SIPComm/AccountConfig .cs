using Sipek.Common;

namespace SIPComm
{
	public class AccountConfig : IAccount
	{
		#region Properties

		public bool Enabled
		{
			get
			{
				return Properties.Settings.Default.cfgAccountEnabled;
			}
			set
			{
				Properties.Settings.Default.cfgAccountEnabled = value;
			}
		}

		public int Index
		{
			get
			{ 
				return Properties.Settings.Default.cfgAccountIndex;				
			}
			set 
			{ 
				Properties.Settings.Default.cfgAccountIndex = value; 				
			}
		}

		public string AccountName
		{
			get
			{
				return Properties.Settings.Default.cfgAccountName;

			}
			set
			{
				Properties.Settings.Default.cfgAccountName = value;
			}
		}

		public string HostName
		{
			get
			{
				return Properties.Settings.Default.cfgAccountAddress;
			}
			set
			{
				Properties.Settings.Default.cfgAccountAddress = value;
			}
		}

		public string Id
		{
			get
			{
				return Properties.Settings.Default.cfgAccountID;
			}
			set
			{
				Properties.Settings.Default.cfgAccountID = value;
			}
		}

		public string UserName
		{
			get
			{
				return Properties.Settings.Default.cfgAccountUsername;
			}
			set
			{
				Properties.Settings.Default.cfgAccountUsername = value;
			}
		}

		public string Password
		{
			get
			{
				return Properties.Settings.Default.cfgAccountPassword;
			}
			set
			{
				Properties.Settings.Default.cfgAccountPassword = value;
			}
		}

		public string DisplayName
		{
			get
			{
				return Properties.Settings.Default.cfgAccountDisplayName;
			}
			set
			{
				Properties.Settings.Default.cfgAccountDisplayName = value;
			}
		}

		public string DomainName
		{
			get
			{
				return Properties.Settings.Default.cfgAccountDomain;
			}
			set
			{
				Properties.Settings.Default.cfgAccountDomain = value;
			}
		}

		public int RegState
		{
			get 
			{
				return Properties.Settings.Default.cfgAccountState;
			}
			set
			{
				Properties.Settings.Default.cfgAccountState = value;
			}
		}

		public string ProxyAddress
		{
			get
			{
				return Properties.Settings.Default.cfgAccountProxyAddress;
			}
			set
			{
				Properties.Settings.Default.cfgAccountProxyAddress = value;
			}
		}

		public ETransportMode TransportMode
		{
			get
			{
				return (ETransportMode)Properties.Settings.Default.cfgAccountTransport;
			}
			set
			{
				Properties.Settings.Default.cfgAccountTransport = (int)value;
			}
		}

		#endregion
	
	}
}
