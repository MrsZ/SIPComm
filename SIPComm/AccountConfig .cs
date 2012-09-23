using Sipek.Common;

namespace SIPComm
{
	public class AccountConfig : IAccount
	{
		private RegSet RegSet;

		#region Properties in Register

		public AccountConfig()
		{
			RegSet = new RegSet();
		}

		public bool Enabled
		{
			get { return RegSet.Enabled; }
			set { RegSet.Enabled = value; }
		}

		public int Index
		{
			get { return RegSet.Index; }
			set { RegSet.Index = value; }
		}

		public string AccountName
		{
			get { return RegSet.AccountName; }
			set { RegSet.AccountName = value; }
		}

		public string HostName
		{
			get { return RegSet.HostName; }
			set { RegSet.HostName = value; }
		}

		public string Id
		{
			get { return RegSet.Id; }
			set { RegSet.Id = value; }
		}

		public string UserName
		{
			get { return RegSet.UserName; }
			set { RegSet.UserName = value; }
		}

		public string Password
		{
			get { return RegSet.Password; }
			set { RegSet.Password = value; }
		}

		public string DisplayName
		{
			get { return RegSet.DisplayName; }
			set { RegSet.DisplayName = value; }
		}

		public string DomainName
		{
			get { return RegSet.DomainName; }
			set { RegSet.DomainName = value; }
		}

		public int RegState
		{
			get { return RegSet.RegState; }
			set { RegSet.RegState = value; }
		}

		public string ProxyAddress
		{
			get { return RegSet.ProxyAddress; }
			set { RegSet.ProxyAddress = value; }
		}

		public ETransportMode TransportMode
		{
			get { return RegSet.TransportMode; }
			set { RegSet.TransportMode = value; }
		}

		#endregion Properties in Register
	}
}