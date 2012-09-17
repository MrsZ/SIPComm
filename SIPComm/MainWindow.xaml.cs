using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sipek.Common;

using Sipek.Common.CallControl;
using Sipek.Sip;
using System.Threading;
using System.Windows.Threading;

namespace SIPComm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
	   #region Fields

	   private SipekResources _resources = new SipekResources();
	   public IStateMachine callState;
	   private List<IAccount> AccountsList = new List<IAccount>();
	   private EUserStatus _lastUserStatus = EUserStatus.AVAILABLE;

	   ChatWindow _chatWindow;

	   string AccountStateText;
	   string dialDigit;

	   System.Timers.Timer tmr = new System.Timers.Timer();
	   
	   TrayNotify Notify;

	   Dispatcher _mainDispatcher = Dispatcher.CurrentDispatcher;
	   BrushConverter converter = new BrushConverter();

	   #endregion

	   #region SIP Properties

	   public bool AutoRegister
	   {
		  get { return Properties.Settings.Default.AutoRegister; }
		  set { Properties.Settings.Default.AutoRegister = value; }
	   }

	   public SipekResources SipekResources
	   {
		  get { return _resources; }
	   }

	   public CCallManager CallManager
	   {
		  get { return _resources.CallManager; }
	   }

	   public bool IsInitialized
	   {
		  get { return SipekResources.StackProxy.IsInitialized; }
	   }

	   Properties.Settings Settings
	   {
		  get { return Properties.Settings.Default; }
	   }

	   #endregion

	   public MainWindow()
	   {
		  InitializeComponent();

		  if (Settings.cfgUpdgradeSettings)
		  {
			 Settings.Upgrade();
			 Settings.cfgUpdgradeSettings = false;
		  }

		  _chatWindow = new ChatWindow(this);

		  Notify = new TrayNotify(this);

		  Notify.Tray.Visible = true;  
	   }

	   #region Call Operations
	   
	   public void MakeCall(string number)
	   {
		  callState = SipekResources.CallManager.createOutboundCall(number);
	   }

	   public void ReleaseCall()
	   {
		  if (callState != null) SipekResources.CallManager.onUserRelease(callState.Session);
	   }

	   public void AnswerCall()
	   {
		  if (callState != null) SipekResources.CallManager.onUserAnswer(callState.Session);
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

	   
	   public void Call()
	   {
		  MakeCall(Number.Tag.ToString());
	   }
	   
	   #endregion

	   #region Account Operations

	   public void RegisterAccount()
	   {
		  SipekResources.CallManager.Initialize();
		  SipekResources.Registrar.registerAccounts();
		  RefreshSIP();
	   }

	   public void UnregisterAccount()
	   {
		  SipekResources.Registrar.unregisterAccounts();
		  RefreshSIP();
	   }	 

	   #endregion

	   #region SIP Operations

	   public void ShutdownSIP()
	   {
		  SipekResources.CallManager.Shutdown();
	   }
	   
	   private void RefreshSIP()
		{
			UpdateAccountList();
		}

	   public void UpdateAccountList()
	   {
		  
		  AccountsList.Clear();
		  for (int i = 0; i < SipekResources.Configurator.Accounts.Count; i++)
		  {
			 IAccount acc = SipekResources.Configurator.Accounts[i];

			 // mark default account
			 if (i == SipekResources.Configurator.DefaultAccountIndex)
			 {
				    if (acc.RegState == 200)
				    {					  
					   _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					   {
						  Account.Tag = "acc.AccountName";
						  Account.Background = converter.ConvertFromString("#FA890404") as Brush;
					   }));						 

					   AccountStateText = acc.AccountName;
				    }
				    else if (acc.RegState == 0)
				    {
					   AccountStateText = "Trying..." + " - " + acc.AccountName;
					   _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					   {
						  Account.Tag = AccountStateText;
						  Account.Background = converter.ConvertFromString("#FA390404") as Brush;
					   }));
				    }
				    else
				    {
					   AccountStateText = "Not registered" + " - " + acc.AccountName;
					   _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					   {
						  Account.Tag = AccountStateText;
						  Account.Background = converter.ConvertFromString("#Fd890404") as Brush;
					   }));
				    }
			 }
			 else
			 {
			 }

			 Notify.SetByAccountState(acc);

			 AccountsList.Add(acc);
		  }
	   }

	   #endregion

	   #region SIP Events

	   delegate void DRefreshForm();
	   delegate void DCallStateChanged(int sessionId);
	   delegate void MessageReceivedDelegate(string from, string message);
	   delegate void BuddyStateChangedDelegate(int buddyId, int status, string text);
	   delegate void DMessageWaiting(int mwi, string text);
	   delegate void DIncomingCall(int sessionId, string number, string info);

	   private void CallManager_IncomingCallNotification(int sessionId, string number, string info)
	   {
		  callState = SipekResources.CallManager.getCall(sessionId);
		  OnIncomingCall(sessionId, number, info);		  
	   }	 

	   private void OnIncomingCall(int sessionId, string number, string info)
	   {
		  Notify.SetByCallState(callState);
		  RefreshUIByStatus();
	   }
	   
	   private void CallStateRefresh(int sessionId)
	   {
		  CallStateRefreshed(sessionId);
	   }

	   private void CallStateRefreshed(int sessionId)
	   {
		  callState = SipekResources.CallManager.getCall(sessionId);
		  RefreshUIByStatus();

		  if (callState.StateId == EStateId.NULL)
		  {
			 _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
			 {
				btnCall.Content = "Call";
				btnCall.Background = converter.ConvertFromString("#FD890404") as Brush;
			 }));
		  }
		  else if (callState.StateId == EStateId.INCOMING || callState.StateId == EStateId.ALERTING)
		  {
			 _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
			 {
				btnCall.Content = "Accept";
				btnCall.Background = converter.ConvertFromString("#FF890404") as Brush;
			 }));
		  }

		  if (callState.StateId == EStateId.RELEASED)
		  {
			 ReleaseCall();
		  }

		  if (callState != null)
			 _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
			 {
				Account.Tag = callState.StateId.ToString();
				Account.Background = converter.ConvertFromString("#Fd890404") as Brush;
			 }));


		  Notify.SetByCallState(callState);   
	   }

	   private void RefreshUIByStatus()
	   {

		  if (EStateId.HOLDING == callState.StateId)
		  {
			 _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
			 {
				btnHold.BorderBrush = converter.ConvertFromString("#FD890404") as Brush;
			 }));
		  }
		  else
		  {
			 _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
			 {
				btnHold.BorderBrush = converter.ConvertFromString("#FD590404") as Brush;
			 }));
		  }
		  /*
		  switch (callState.StateId)
		  {
			 case EStateId.ACTIVE:
				break;
			 case EStateId.ALERTING:
				break;
			 case EStateId.CONNECTING:
				break;
			 case EStateId.HOLDING:
				break;
			 case EStateId.IDLE:
				break;
			 case EStateId.INCOMING:
				break;
			 case EStateId.NULL:
				break;
			 case EStateId.RELEASED:
				break;
			 case EStateId.TERMINATED:
				break;
			 default: return;
		  }
		   * */
	   }


	   private void onAccountStateChanged(int accId, int accState)
	   {
		  RefreshSIP();		  
	   }


	   #region Message

	   public void onMessageWaitingIndication(int mwi, string text)
	   {
		 MessageWaiting(mwi, text);
	   }

	   public void onMessageReceived(string from, string message)
	   {
		  MessageReceived(from, message);
	   }

	   private void MessageReceived(string from, string message)
	   {

		  /*if (null == _chatWindow)
			 _chatWindow = new ChatWindow(this);
		  */
		  

		  /*
		  _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
		  {
			 Number.Tag = message;
		  }));
		  */

		  // extract buddy ID

		  string buddyId = parseFrom(from);
		  _mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
		  {
			 _chatWindow.AddIncomingMessage(buddyId, message);
			 _chatWindow.Show();
		  }));
		  

/*
		  // check if ChatForm already opened
		  foreach (Form ctrl in Application.OpenForms)
		  {
			 if (ctrl.Name == "ChatForm")
			 {
				((ChatForm)ctrl).BuddyName = buddyId;
				((ChatForm)ctrl).LastMessage = message;
				ctrl.Focus();
				return;
			 }
		  }

		  // if not, create new instance
		  ChatForm bf = new ChatForm(SipekResources);
		  int id = CBuddyList.getInstance().getBuddyId(buddyId);
		  if (id >= 0)
		  {
			 //_buddyId = id;        
			 CBuddyRecord buddy = CBuddyList.getInstance()[id];
			 //_titleText.Caption = buddy.FirstName + ", " + buddy.LastName;
			 bf.BuddyId = (int)id;
		  }
		  bf.BuddyName = buddyId;
		  bf.LastMessage = message;
		  bf.ShowDialog();
		  */
	   }

	   private string parseFrom(string from)
	   {
		  string number = from.Replace("<sip:", "");

		  int atPos = number.IndexOf('@');
		  if (atPos >= 0)
		  {
			 number = number.Remove(atPos);
			 int first = number.IndexOf('"');
			 if (first >= 0)
			 {
				int last = number.LastIndexOf('"');
				number = number.Remove(0, last + 1);
				number = number.Trim();
			 }
		  }
		  else
		  {
			 int semiPos = number.IndexOf(';');
			 if (semiPos >= 0)
			 {
				number = number.Remove(semiPos);
			 }
			 else
			 {
				int colPos = number.IndexOf(':');
				if (colPos >= 0)
				{
				    number = number.Remove(colPos);
				}
			 }
		  }
		  return number;
	   }

	   private void MessageWaiting(int mwi, string info)
	   {
		  string[] parts = info.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		  string vmaccount = "";
		  string noofvms = "";

		  if (parts.Length == 3)
		  {
			 int index = parts[1].IndexOf("Message-Account: ");
			 if (index == 0)
			 {
				vmaccount = parts[1].Substring("Message-Account: ".Length);
			 }

			 if (parts[2].IndexOf("Voice-Message: ") >= 0)
			 {
				noofvms = parts[2].Substring("Voice-Message: ".Length);
			 }

		  }
	   }
	   #endregion


	   #endregion

	   private void Configure()
	   {
		  new ConfigWindow().Show();
	   }


	   //Key Down
	   private void Window_KeyDown_1(object sender, KeyEventArgs e)
	   {

		  switch (e.Key)
		  {
			 case Key.Escape:
				//1 Release call
				if (null != callState && callState.StateId != EStateId.NULL)
				{
				    ReleaseCall();
				}
				//2 Clear number
				else if (!string.IsNullOrEmpty(Number.Content.ToString()))
				{
				    Number.Tag = "";
				}
				//3 Hide
				else
				    Hide();

				break;

			 case Key.Back:
				if (Number.Tag == null)
				{
				    return;
				}
				var t = Number.Tag.ToString();
				if (t.Length == 0)
				{
				    break;
				}
				Number.Tag = t.Remove(t.Length-1);
				break; 

			 case Key.Enter:
				if (Number.Tag.ToString().Length == 0)
				{
				    break;
				}
				int num;
				int.TryParse(Number.Tag.ToString(), out num);
				if (266344 == num )
				{
				    Configure();
				} 
				else 
				{
				    Call();
				}
				break;

			 //1
			 case Key.D1: 
			 case Key.NumPad1:
				Number.Tag += "1";
				dialDigit = "1";
				break; 
			 
			 //2
			 case Key.A:
			 case Key.B:
			 case Key.C:
			 case Key.D2:
			 case Key.NumPad2:
				Number.Tag += "2";
				dialDigit = "2";
				break;
			
			//3
			 case Key.D:
			 case Key.E:
			 case Key.F:
			 case Key.D3:	 
			 case Key.NumPad3:
				Number.Tag += "3";
				dialDigit = "3";
				break;

			 //4
			 case Key.G:
			 case Key.H:
			 case Key.I:
			 case Key.D4:
			 case Key.NumPad4:
				Number.Tag += "4";
				dialDigit = "4";
				break;

			 //5
			 case Key.J:
			 case Key.K:
			 case Key.L:
			 case Key.D5:
			 case Key.NumPad5:
				Number.Tag += "5";
				dialDigit = "5";
				break;

			 //6
			 case Key.M:
			 case Key.N:
			 case Key.O:
			 case Key.D6:
			 case Key.NumPad6:
				Number.Tag += "6";
				dialDigit = "6";
				break;

			 //7
			 case Key.P:
			 case Key.Q:
			 case Key.R:
			 case Key.S:
			 case Key.D7:
			 case Key.NumPad7:
				Number.Tag += "7";
				dialDigit = "7";
				break;

			 //8
			 case Key.T:
			 case Key.U:
			 case Key.V:
			 case Key.D8:
			 case Key.NumPad8:
				Number.Tag += "8";
				dialDigit = "8";
				break;

			 //9
			 case Key.W:
			 case Key.X:
			 case Key.Y:
			 case Key.Z:
			 case Key.D9:
			 case Key.NumPad9:
				Number.Tag += "9";
				dialDigit = "9";
				break;

			 //0
			 case Key.D0:
			 case Key.NumPad0:
				Number.Tag += "0";
				break;
			 
			 default:
				break;
		  }
	   }


	   #region Buttons Events

	   private void D_MouseEnter(object sender, MouseEventArgs e)
	   {
		  BrushConverter converter = new BrushConverter();
		  ((System.Windows.Controls.Control)(sender)).BorderBrush = converter.ConvertFromString("#FF17517A") as Brush; 
		  //D1.BorderBrush = 0;//FF17517A;
	   }

	   private void D_MouseLeave(object sender, MouseEventArgs e)
	   {
		  ((System.Windows.Controls.Control)(sender)).BorderBrush = null;
		  ((System.Windows.Controls.Control)(sender)).Background = null;
	   }

	   private void D_MouseDown(object sender, MouseButtonEventArgs e)
	   {
		  BrushConverter converter = new BrushConverter();
		  ((System.Windows.Controls.Control)(sender)).Background = converter.ConvertFromString("#FF24333C") as Brush;

		  Number.Tag += ((System.Windows.Controls.Control)(sender)).Tag.ToString();
		  dialDigit = ((System.Windows.Controls.Control)(sender)).Tag.ToString();
	   }

	   private void D_MouseUp(object sender, MouseButtonEventArgs e)
	   {
		  ((System.Windows.Controls.Control)(sender)).Background = null;
	   }

	   private void Del_MouseEnter(object sender, MouseEventArgs e)
	   {
		  BrushConverter converter = new BrushConverter();
		  ((System.Windows.Controls.Control)(sender)).BorderBrush = converter.ConvertFromString("#FF890404") as Brush;
	   }
	   
	   private void Del_MouseDown(object sender, MouseButtonEventArgs e)
	   {
		  BrushConverter converter = new BrushConverter();
		  ((System.Windows.Controls.Control)(sender)).Background = converter.ConvertFromString("#FF24333C") as Brush;
		  if (Number.Tag == null)
		  {
			 return;
		  }
		  var t = Number.Tag.ToString();
		  if (t.Length == 0)
		  {
			 return;
		  }
		  Number.Tag = t.Remove(t.Length - 1);
	   }	   

	   private void Call_MouseDown(object sender, MouseButtonEventArgs e)
	   {
		  if (null == callState) return;
		  switch(callState.StateId)
		  {
			 case EStateId.ACTIVE: ReleaseCall();
				break;
			 case EStateId.ALERTING :
				break;
			 case EStateId.CONNECTING :
				break;
			 case EStateId.HOLDING :
				break;
			 case EStateId.IDLE :
				break;
			 case EStateId.INCOMING: AnswerCall();
				break;
			 case EStateId.NULL: Call();
				break;
			 case EStateId.RELEASED:
				break;
			 case EStateId.TERMINATED:
				break;
			 default: return;
		  }

		  Number.Tag = "Calling...";
		  BrushConverter converter = new BrushConverter();
		  ((System.Windows.Controls.Control)(sender)).Background = converter.ConvertFromString("#FF24333C") as Brush;
	   }

	   private void Transfer_MouseDown(object sender, MouseButtonEventArgs e)
	   {
		  Number.Tag = "Release...";
		  ReleaseCall();
	   }

	   private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
	   {
		   Number.Tag = "Closing...";
		   ShutdownSIP();
		   Notify.Tray.Visible = false;
	   }

	   private void Window_Loaded_1(object sender, RoutedEventArgs e)
	   {
		  SipekResources.CallManager.CallStateRefresh += new DCallStateRefresh(CallStateRefresh);
		  SipekResources.CallManager.IncomingCallNotification += new DIncomingCallNotification(CallManager_IncomingCallNotification);
		  SipekResources.Registrar.AccountStateChanged += new DAccountStateChanged(onAccountStateChanged);
		  SipekResources.Messenger.MessageReceived += onMessageReceived;
		  //SipekResources.Messenger.BuddyStatusChanged += onBuddyStateChanged;
		  SipekResources.Registrar.AccountStateChanged += onAccountStateChanged;
		  SipekResources.StackProxy.MessageWaitingIndication += onMessageWaitingIndication;

		  if (AutoRegister)
		  {
			 RegisterAccount();
		  }

		  int noOfCodecs = SipekResources.StackProxy.getNoOfCodecs();
		  for (int i = 0; i < noOfCodecs; i++)
		  {
			 string codecname = SipekResources.StackProxy.getCodec(i);
			 if (SipekResources.Configurator.CodecList.Contains(codecname))
			 {
				SipekResources.StackProxy.setCodecPriority(codecname, 128);
			 }
			 else
			 {
				SipekResources.StackProxy.setCodecPriority(codecname, 0);
			 }
		  }

		  // Initialize BuddyList
		  CBuddyList.getInstance().Messenger = SipekResources.Messenger;
		  CBuddyList.getInstance().initialize();

		  Number.Tag = "";
	   }

	   #region Close button

	   private void btnClose_MouseEnter(object sender, MouseEventArgs e)
	   {
		  BrushConverter converter = new BrushConverter();
		  ((System.Windows.Controls.Control)(sender)).Background = converter.ConvertFromString("#FA890404") as Brush;
	   }

	   private void btnClose_MouseLeave(object sender, MouseEventArgs e)
	   {
		  BrushConverter converter = new BrushConverter();
		  ((System.Windows.Controls.Control)(sender)).Background = converter.ConvertFromString("#FF213944") as Brush;
	   }

	   private void btnClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	   {
		  BrushConverter converter = new BrushConverter();
		  ((System.Windows.Controls.Control)(sender)).Background = converter.ConvertFromString("#FC190404") as Brush;
		  if (e.RightButton == MouseButtonState.Pressed)
			 Close();
		  else
			 Hide();
	   }

	   private void btnClose_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	   {
		  Close();
	   }

	   #endregion Close button

	   #endregion


	   //Move window
	   private void Account_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	   {
		  this.DragMove();
	   }

	   private void btnHold_MouseDown(object sender, MouseButtonEventArgs e)
	   {
		  HoldCall();
	   }	   
    }
}

