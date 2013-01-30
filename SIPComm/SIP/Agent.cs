using System;
using System.Collections.Generic;
using Sipek.Common.CallControl;
using Sipek.Common;
using System.Security.AccessControl;
using Microsoft.Win32;


namespace SIPComm
{
	public class Agent : IDisposable
	{
		#region Fields
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\ITSComm\\Agent");
		private static SipekResources _resources;// = new SipekResources();
		private IStateMachine callState;
		private List<IAccount> AccountsList = new List<IAccount>();
		private EUserStatus _lastUserStatus = EUserStatus.AVAILABLE;
		private string dialDigit;
		#endregion Fields

		#region SIP Properties

		public SipekResources SipekResources
		{
			get { return _resources; }
		}

		public CCallManager CallManager
		{
			get { return _resources.CallManager; }
		}

		public IStateMachine CallState
		{
			get { return this.callState; }
		}

		public EStateId callStateID
		{
			get { return this.callState.StateId; }
		}

		public bool IsInitialized
		{
			get { return SipekResources.StackProxy.IsInitialized; }
		}

		public bool RegisterOnStart
		{
			get { return true; }
		}

		public bool DNDFlag
		{
			get { return SipekResources.Configurator.DNDFlag; }
			set 
			{ 
				SipekResources.Configurator.DNDFlag = value;
				OnDNDChanged(value);
			}
		}

		public bool AAFlag
		{
			get { return SipekResources.Configurator.AAFlag; }
			set 
			{
				SipekResources.Configurator.AAFlag = value;
				OnAutoAnswerChanged(value);
			}
		}

		#endregion SIP Properties

		#region Call Operations

		public void MakeCall(string number)
		{
			callState = SipekResources.CallManager.createOutboundCall(number);
		}

		public void ReleaseCall()
		{
			if (callState != null)
			{
				SipekResources.CallManager.onUserRelease(callState.Session);
			}
		}

		public void AnswerCall()
		{
			if (callState != null)
			{
				SipekResources.CallManager.onUserAnswer(callState.Session);				
			}
		}

		public void TransferCall(string number)
		{
			SipekResources.CallManager.onUserTransfer(callState.Session, number);
		}

		public void HoldCall()
		{
			SipekResources.CallManager.onUserHoldRetrieve(callState.Session);
		}

		public void DialDigitCall(string number)
		{
			SipekResources.CallManager.onUserDialDigit(callState.Session, dialDigit, EDtmfMode.DM_Outband);
		}

		public void SetSoundDevice(string PlaybackDeviceID, string RecordingDeviceID)
		{
			SipekResources.StackProxy.setSoundDevice(PlaybackDeviceID, RecordingDeviceID);
		}
		#endregion Call Operations

		public void RegisterAccount()
		{
			SubscribeEvents();
			int s = SipekResources.CallManager.Initialize();
			SipekResources.Registrar.registerAccounts();
			callState = SipekResources.CallManager.getCall(-1);
		}

		public void ShutdownSIP()
		{
			SipekResources.CallManager.CallStateRefresh -= CallStateRefresh;
			SipekResources.CallManager.IncomingCallNotification -= IncomingCall;
			SipekResources.Registrar.AccountStateChanged -= AccountStateChange;
			SipekResources.Messenger.MessageReceived -= MessageReceived;
			//SipekResources.Messenger.BuddyStatusChanged -= onBuddyStateChanged;
			SipekResources.StackProxy.MessageWaitingIndication -= MessageWaiting;
			SipekResources.CallManager.Shutdown();
			OnAccountStateChange(0);
		}

		public void UnregisterAccount()
		{
			SipekResources.Registrar.unregisterAccounts();
		}

		private void SubscribeEvents()
		{
			SipekResources.CallManager.CallStateRefresh += new DCallStateRefresh(CallStateRefresh);
			SipekResources.CallManager.IncomingCallNotification += new DIncomingCallNotification(IncomingCall);
			SipekResources.Registrar.AccountStateChanged += new DAccountStateChanged(AccountStateChange);

			SipekResources.Messenger.MessageReceived += new DMessageReceived(MessageReceived);
			//SipekResources.Messenger.BuddyStatusChanged += onBuddyStateChanged;
			SipekResources.StackProxy.MessageWaitingIndication += new DMessageWaitingNotification(MessageWaiting);
		}

		private void MessageWaiting(int mwi, string text)
		{
			OnMessageWaiting.Invoke(mwi, text);
		}

		private void MessageReceived(string from, string text)
		{
			OnMessageReceived.Invoke(from, text);
		}

		private void AccountStateChange(int accId, int accState)
		{
			OnAccountStateChange.Invoke(accState);
		}

		private void CallStateRefresh(int sessionId)
		{
			callState = SipekResources.CallManager.getCall(sessionId);			
			if (EStateId.RELEASED == callState.StateId)
				ReleaseCall();
			if (EStateId.IDLE == callState.StateId)
			{ }
			if (EStateId.NULL == callState.StateId)
			{
				OnAccountStateChange.Invoke(SipekResources.Configurator.Accounts[0].RegState);
				if (0 < SipekResources.CallManager.getNoCallsInState(EStateId.HOLDING))
				{
					callState = SipekResources.CallManager.getCallInState(EStateId.HOLDING);
				}
			}
			OnCallStateRefresh.Invoke(callState.StateId);
		}

		private void IncomingCall(int sessionId, string number, string info)
		{
			callState = SipekResources.CallManager.getCall(sessionId);
			OnIncomingCall.Invoke(callStateID, number, info + sessionId);
		}

		#region External Events
		public delegate void DAccountStateChangeHandler(int accState);
		public event DAccountStateChangeHandler OnAccountStateChange;

		public delegate void DCallStateRefreshHandler(EStateId callStateID);
		public event DCallStateRefreshHandler OnCallStateRefresh;

		public delegate void DIncomingCallHandler(EStateId callStateID, string number, string info);
		public event DIncomingCallHandler OnIncomingCall;

		public delegate void DMessageReceivedHandler(string from, string text);
		public event DMessageReceivedHandler OnMessageReceived;

		public delegate void DMessageWaitingHandler(int mwi, string text);
		public event DMessageWaitingHandler OnMessageWaiting;

		public delegate void DDNDChangeHandler(bool value);
		public event DDNDChangeHandler OnDNDChanged;

		public delegate void DAutoAnswerChangeHandler(bool value);
		public event DAutoAnswerChangeHandler OnAutoAnswerChanged;
		#endregion External Events

		public Agent()
		{
			regKey.SetAccessControl(new RegistrySecurity());
			_resources = new SipekResources();
		}

		public void Dispose()
		{
			this.Dispose();
		}
	}
}
