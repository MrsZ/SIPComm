using Sipek.Common;
using Sipek.Common.CallControl;
using Sipek.Sip;

namespace SIPComm
{
	public class SipekResources : AbstractFactory
	{
		private IMediaProxyInterface _mediaProxy = new CMediaPlayerProxy();
		private ICallLogInterface _callLogger = new CCallLog();
		private pjsipStackProxy _stackProxy = pjsipStackProxy.Instance;
		private PhoneConfig _config = new PhoneConfig();

		#region Constructor

		public SipekResources()
		{
			SipConfigStruct.Instance.stunServer = this.Configurator.StunServerAddress;
			SipConfigStruct.Instance.publishEnabled = this.Configurator.PublishEnabled;
			SipConfigStruct.Instance.expires = this.Configurator.Expires;
			SipConfigStruct.Instance.VADEnabled = this.Configurator.VADEnabled;
			SipConfigStruct.Instance.ECTail = this.Configurator.ECTail;
			SipConfigStruct.Instance.nameServer = this.Configurator.NameServer;

			_callManager.StackProxy = _stackProxy;
			_callManager.Config = _config;
			_callManager.Factory = this;
			_callManager.MediaProxy = _mediaProxy;
			_stackProxy.Config = _config;
			_registrar.Config = _config;
			_messenger.Config = _config;
		}

		#endregion Constructor

		#region AbstractFactory methods

		public ITimer createTimer()
		{
			return new GUITimer();
		}

		public IStateMachine createStateMachine()
		{
			return new CStateMachine();
		}

		#endregion AbstractFactory methods

		#region Other Resources

		public pjsipStackProxy StackProxy
		{
			get { return _stackProxy; }
			set { _stackProxy = value; }
		}

		public PhoneConfig Configurator
		{
			get { return _config; }
			set { }
		}

		public IMediaProxyInterface MediaProxy
		{
			get { return _mediaProxy; }
			set { }
		}

		public ICallLogInterface CallLogger
		{
			get { return _callLogger; }
			set { }
		}

		private IRegistrar _registrar = pjsipRegistrar.Instance;

		public IRegistrar Registrar
		{
			get { return _registrar; }
		}

		private IPresenceAndMessaging _messenger = pjsipPresenceAndMessaging.Instance;

		public IPresenceAndMessaging Messenger
		{
			get { return _messenger; }
		}

		private CCallManager _callManager = CCallManager.Instance;

		public CCallManager CallManager
		{
			get { return _callManager; }
		}

		#endregion Other Resources
	}
}