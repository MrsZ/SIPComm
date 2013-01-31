using Microsoft.Win32;
using Sipek.Common;
using Sipek.Common.CallControl;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Security.AccessControl;
using SIPComm.Properties;

namespace SIPComm
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm");
		private static Agent Agent;
		private ChatWindow _chatWindow;
		private ConfigWindow _configWindow;
		private System.Windows.Forms.NotifyIcon _notifyIcon;
		private Dispatcher _mainDispatcher = Dispatcher.CurrentDispatcher;
		private BrushConverter converter = new BrushConverter();
		private string dialDigit;
		#endregion Fields

		#region  Properties
		public bool StartHidden
		{
			get { return (int)regKey.GetValue("StartHidden", 0) == 1 ? true : false; }
			set { regKey.SetValue("StartHidden", (int)(value ? 1 : 0)); }
		}

		#endregion  Properties

		public MainWindow()
		{
			regKey.SetAccessControl(new RegistrySecurity());
			InitializeComponent();
			Initialize();
		}

		private void Initialize()
		{		
			InitializeAgent();
			InitializeNotifyIcon();
			_chatWindow = new ChatWindow(this);
			//_configWindow = new ConfigWindow();
		}
		
		#region Initialize Agent
		private void InitializeAgent()
		{
			Agent = new Agent();
			Agent.OnAccountStateChange += Agent_OnAccountStateChange;
			Agent.OnCallStateRefresh += Agent_OnCallStateRefresh;
			Agent.OnIncomingCall += Agent_OnIncomingCall;
			Agent.OnMessageReceived += Agent_OnMessageReceived;
			Agent.OnMessageWaiting += Agent_OnMessageWaiting;
			Agent.OnDNDChanged += Agent_OnDNDChanged;
			Agent.OnAutoAnswerChanged += Agent_OnAutoAnswerChanged;

			_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
			{
				Agent.RegisterAccount();
			}));
			
		}

		void Agent_OnAutoAnswerChanged(bool value)
		{
			if (value)
				_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Orange;
			else
				_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Green;
		}

		void Agent_OnDNDChanged(bool value)
		{
			if (value)
				_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
			else
				_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Green;
		}

		void Agent_OnMessageWaiting(int mwi, string text)
		{
			//notifyIcon.ShowBalloonTip(1, mwi.ToString(), text, ToolTipIcon.Info);
		}

		void Agent_OnMessageReceived(string from, string text)
		{
			//notifyIcon.ShowBalloonTip(1, from, text, ToolTipIcon.Info);
		}

		private void Agent_OnIncomingCall(Sipek.Common.EStateId callStateID, string number, string info)
		{
			SetIconByCallStateID(callStateID);
		}

		private void Agent_OnCallStateRefresh(Sipek.Common.EStateId callStateID)
		{
			SetIconByCallStateID(callStateID);
			//ShowBalloonTips(callStateID);
		}

		private void Agent_OnAccountStateChange(int accState)
		{
			switch (accState)
			{
				case 200:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Green;
					break;

				case 0:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Grey;
					break;

				default:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					break;
			}
		}

		private void SetIconByCallStateID(Sipek.Common.EStateId callStateID)
		{
			switch (callStateID)
			{
				case Sipek.Common.EStateId.ACTIVE:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Red;
					break;

				case Sipek.Common.EStateId.ALERTING:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Orange;
					break;

				case Sipek.Common.EStateId.CONNECTING:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					break;

				case Sipek.Common.EStateId.HOLDING:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Blue;
					break;

				case Sipek.Common.EStateId.IDLE:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					break;

				case Sipek.Common.EStateId.INCOMING:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Orange;
					break;

				case Sipek.Common.EStateId.NULL:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Green;
					break;

				case Sipek.Common.EStateId.RELEASED:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					break;

				case Sipek.Common.EStateId.TERMINATED:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					break;
			}
		}


		#endregion Initialize Agent

		#region Initialize NotifyIcon
		private void InitializeNotifyIcon()
		{
			_notifyIcon = new System.Windows.Forms.NotifyIcon();
			_notifyIcon.Visible = true;
			_notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
			_notifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Перезапустить SIP", new EventHandler(ReloadSIPItem_onClick)));
			_notifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Настройки", new EventHandler(SettingsItem_onClick)));
			_notifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Выход", new EventHandler(ExitItem_onClick)));

			_notifyIcon.MouseClick += notifyIcon_MouseClick;
			_notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
			_notifyIcon.Visible = true;
			_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Grey;
		}

		private void ReloadSIPItem_onClick(object sender, EventArgs e)
		{
			Agent.ShutdownSIP();
			Agent.RegisterAccount();
		}

		private void TestCallItem_onClick(object sender, EventArgs e)
		{
		}

		private void SettingsItem_onClick(object sender, EventArgs e)
		{
			_configWindow.Show();
		}

		private void ExitItem_onClick(object sender, EventArgs e)
		{
			_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Grey;
			Agent.ShutdownSIP();
			_notifyIcon.Visible = false;
			Environment.Exit(0);
		}

		private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Right:
					break;
				case System.Windows.Forms.MouseButtons.Middle:
					if (Agent.callStateID == Sipek.Common.EStateId.ACTIVE)
						Agent.HoldCall();
					if (Agent.callStateID == Sipek.Common.EStateId.HOLDING)
						Agent.HoldCall();
					break;
			}
		}

		private void notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
			{

			}));
			switch (Agent.callStateID)
			{
				case Sipek.Common.EStateId.ACTIVE: Agent.ReleaseCall();
					break;
				case Sipek.Common.EStateId.ALERTING: Agent.ReleaseCall();
					break;
				case Sipek.Common.EStateId.INCOMING: Agent.AnswerCall();
					break;
			}
		}



		#endregion Initialize notifyIcon

		#region Call Operations

		public void MakeCall(string number)
		{
			throw new NotImplementedException();
		}

		public void ReleaseCall()
		{
			throw new NotImplementedException();
		}

		public void AnswerCall()
		{
			throw new NotImplementedException();
		}

		public void TransferCall(string number)
		{
			throw new NotImplementedException();
		}

		public void HoldCall()
		{
			throw new NotImplementedException();
		}

		public void DialDigitCall(string number)
		{
			throw new NotImplementedException();
		}

		public void Call()
		{
			Agent.MakeCall(Number.Tag.ToString());
		}

		#endregion Call Operations
		
		#region Agent Events

		#endregion Agent Events

		private void Configure()
		{
			_configWindow.Show();
		}

		private void Window_KeyDown_1(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:

					//1 Release call
					if (null != Agent.callStateID && Agent.callStateID != EStateId.NULL)
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
					Number.Tag = t.Remove(t.Length - 1);
					break;

				case Key.Enter:
					if (Number.Tag.ToString().Length == 0)
					{
						break;
					}
					int num;
					int.TryParse(Number.Tag.ToString(), out num);
					if (266344 == num)
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
			if (null == Agent.CallState) Call();

			switch (Agent.CallState.StateId)
			{
				case EStateId.ACTIVE:
				case EStateId.ALERTING: ReleaseCall();
					break;

				case EStateId.CONNECTING:
					break;

				case EStateId.HOLDING: HoldCall();
					break;

				case EStateId.IDLE:
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

			BrushConverter converter = new BrushConverter();
			((System.Windows.Controls.Control)(sender)).Background = converter.ConvertFromString("#FF24333C") as Brush;
		}

		private void Transfer_MouseDown(object sender, MouseButtonEventArgs e)
		{
			ReleaseCall();
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

		#endregion Close button

		#endregion Buttons Events

		private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Agent.ShutdownSIP();
			_notifyIcon.Visible = false;
		}

		private void Window_Loaded_1(object sender, RoutedEventArgs e)
		{
			Number.Tag = "";

			if (StartHidden)
			{
				this.Hide();
			}
		}

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