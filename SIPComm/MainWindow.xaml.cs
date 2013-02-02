using Microsoft.Win32;
using Sipek.Common;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SIPComm
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields

		private System.ComponentModel.BackgroundWorker SocketWorker;
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm");
		private static Agent Agent;
		private ChatWindow _chatWindow;
		private ConfigWindow _configWindow;
		private System.Windows.Forms.NotifyIcon _notifyIcon;
		private Dispatcher _mainDispatcher = Dispatcher.CurrentDispatcher;
		private BrushConverter converter = new BrushConverter();
		private static Listener Listener;
		private string dialDigit;

		#endregion Fields

		#region Properties

		public bool StartHidden
		{
			get { return (int)regKey.GetValue("StartHidden", 0) == 1 ? true : false; }
			set { regKey.SetValue("StartHidden", (int)(value ? 1 : 0)); }
		}
		
		public bool UseAgentUI
		{
			get { return (int)regKey.GetValue("UseAgentUI", 0) == 1 ? true : false; }
			set { regKey.SetValue("UseAgentUI", (int)(value ? 1 : 0)); }
		}

		public bool SimpleClient
		{
			get { return (int)regKey.GetValue("SimpleClient", 0) == 1 ? true : false; }
			set { regKey.SetValue("SimpleClient", (int)(value ? 1 : 0)); }
		}

		#endregion Properties

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
			_configWindow = new ConfigWindow();
			InitializeListener();
		}

		#region InitializeListener

		public void InitializeListener()
		{
			if (SimpleClient)
			{
				return;
			}

			try
			{
				Listener = new Listener();
				SocketWorker = new System.ComponentModel.BackgroundWorker();
				SocketWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SocketWorker_DoWork);
				Listener.OnReceiveCommand += Listener_OnReceiveCommand;
				SocketWorker.RunWorkerAsync();
			}
			catch (System.Net.Sockets.SocketException e)
			{
				_notifyIcon.ShowBalloonTip(2, "Ошибка!", e.Message, System.Windows.Forms.ToolTipIcon.Warning);
			}
		}

		private void SocketWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			try
			{
				Listener.Start();
			}
			catch { }
		}

		private void Listener_OnReceiveCommand(string message)
		{
			switch (message)
			{
				case "answer" :
					if (Agent.callStateID == Sipek.Common.EStateId.INCOMING)
						_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
						{
							Agent.AnswerCall();
						}));
					break;
				case "hold" : 
					if ((Agent.callStateID == Sipek.Common.EStateId.ACTIVE || Agent.callStateID == Sipek.Common.EStateId.HOLDING))
						_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
						{
							Agent.HoldCall();
						}));
					break;
				case "exit" :
					MainExit();
					break;
				case "reload":
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						ReloadSIP();
					}));
					break;
				default:
					ExecuteCustomReceivedCommand(message);
					break;
			}
		}

		private void ExecuteCustomReceivedCommand(string message)
		{
			foreach( KeyValuePair<string, string> command in ParseCommand(message))
			{
				regKey.SetValue(command.Key, command.Value);
			}
			_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
			{
				ReloadSIP();
			}));
		}

		private List<KeyValuePair<string, string>> ParseCommand(string message)
		{
			string [] command = message.Split(';');
			List<KeyValuePair<string, string>>commands = new List<KeyValuePair<string, string>>();
			for (int i = 0; i < command.Length; i++)
			{
				string [] tmp = command[i].Split('=');
				try
				{
					commands.Add(new KeyValuePair<string, string>(tmp[0].Trim(), tmp[1].Trim()));
				}
				catch { continue; }
			}
			return commands;
		}



		#endregion InitializeListener

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

		private void Agent_OnAutoAnswerChanged(bool value)
		{
			if (value)
				_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Orange;
			else
				_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Green;
		}

		private void Agent_OnDNDChanged(bool value)
		{
			if (value)
				_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
			else
				_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Green;
		}

		private void Agent_OnMessageWaiting(int mwi, string text)
		{
			//notifyIcon.ShowBalloonTip(1, mwi.ToString(), text, ToolTipIcon.Info);
		}

		private void Agent_OnMessageReceived(string from, string text)
		{
			//notifyIcon.ShowBalloonTip(1, from, text, ToolTipIcon.Info);
		}

		private void Agent_OnIncomingCall(Sipek.Common.EStateId callStateID, string number, string info)
		{
			DrawUIByCallState(callStateID);
		}

		private void Agent_OnCallStateRefresh(Sipek.Common.EStateId callStateID)
		{
			DrawUIByCallState(callStateID);

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

		#endregion Initialize Agent

		#region Initialize NotifyIcon

		private void InitializeNotifyIcon()
		{
			_notifyIcon = new System.Windows.Forms.NotifyIcon();
			_notifyIcon.Visible = true;
			_notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
			_notifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Reload SIP", new EventHandler(ReloadSIPItem_onClick)));
			_notifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Settings", new EventHandler(SettingsItem_onClick)));
			_notifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Exit", new EventHandler(ExitItem_onClick)));

			_notifyIcon.MouseClick += notifyIcon_MouseClick;
			_notifyIcon.MouseDoubleClick += notifyIcon_MouseClick;
			_notifyIcon.Visible = true;
			_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Grey;
		}

		private void ReloadSIPItem_onClick(object sender, EventArgs e)
		{
			ReloadSIP();
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
			MainExit();			
		}

		

		private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					if (2 == e.Clicks)
					{
						if (this.IsVisible)
						{ this.Hide(); }
						else
						{ this.Show(); }
					}
					break;

				case System.Windows.Forms.MouseButtons.XButton1:
				case System.Windows.Forms.MouseButtons.XButton2:
					OnHoldClick();
					break;

				case System.Windows.Forms.MouseButtons.Middle:
					OnCallClick();
					break;
			}
		}

		#endregion Initialize NotifyIcon

		private void MainExit()
		{
			_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
			Agent.ShutdownSIP();
			_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Grey;
			_notifyIcon.Visible = false;
			Environment.Exit(0);
		}

		#region Agent Operations

		public void Release()
		{
			Agent.ReleaseCall();
		}

		public void Answer()
		{
			Agent.AnswerCall();
		}

		public void Transfer(string number)
		{
			//throw new NotImplementedException();
		}

		public void Hold()
		{
			Agent.HoldCall();
		}

		public void DialDigit(string number)
		{
			Agent.DialDigit(number);
		}

		public void Call()
		{
			Agent.MakeCall(Number.Tag.ToString());
		}

		private void ReloadSIP()
		{
			Agent.ShutdownSIP();
			Agent.RegisterAccount();
		}

		#endregion Agent Operations

		private void OnHoldClick()
		{
			if (null == Agent.CallState) return;
			switch (Agent.callStateID)
			{
				case Sipek.Common.EStateId.ACTIVE:
					Agent.HoldCall();
					break;

				case Sipek.Common.EStateId.HOLDING:
					Agent.HoldCall();
					break;
			}
		}

		private void OnCallClick()
		{
			if (null == Agent.CallState) Call();
			switch (Agent.callStateID)
			{
				case Sipek.Common.EStateId.ACTIVE: Agent.ReleaseCall();
					break;

				case Sipek.Common.EStateId.ALERTING: Agent.ReleaseCall();
					break;

				case Sipek.Common.EStateId.INCOMING: Agent.AnswerCall();
					break;

				case Sipek.Common.EStateId.NULL:
					Call();
					break;
			}
		}

		//Keys
		private void Window_KeyDown_1(object sender, KeyEventArgs e)
		{
			dialDigit = "";
			switch (e.Key)
			{
				case Key.Escape:

					//1 Clear number
					if (!string.IsNullOrEmpty(Number.Content.ToString()))
					{
						Number.Tag = "";
					}

					//2 Release call
					else
						if (null != Agent.CallState && Agent.callStateID != EStateId.NULL)
						{
							Release();
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
						_configWindow.Show();
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
					dialDigit = "0";
					break;

				//*
				case Key.Multiply:
					Number.Tag += "*";
					dialDigit = "*";
					break;

				default:
					dialDigit = "";
					break;
			}
			if (!string.IsNullOrEmpty(dialDigit))
			{
				DialDigit(dialDigit);
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
			dialDigit = ((System.Windows.Controls.Control)(sender)).Tag.ToString();
			Number.Tag += dialDigit;
			dialDigit = ((System.Windows.Controls.Control)(sender)).Tag.ToString();
			DialDigit("dialDigit");
		}

		private void D_MouseUp(object sender, MouseButtonEventArgs e)
		{
			((System.Windows.Controls.Control)(sender)).Background = null;
		}

		private void CallControl_MouseLeave(object sender, MouseEventArgs e)
		{
			((System.Windows.Controls.Control)(sender)).BorderBrush = null;
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
			OnCallClick();

			BrushConverter converter = new BrushConverter();
			((System.Windows.Controls.Control)(sender)).Background = converter.ConvertFromString("#FF24333C") as Brush;
		}

		private void Transfer_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Release();
		}

		private void btnHold_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Hold();
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
			MainExit();
		}

		private void Window_Loaded_1(object sender, RoutedEventArgs e)
		{
			Number.Tag = "";
			if (StartHidden)
			{
				this.Hide();
			}
		}

		private void ShowBalloonTips(Sipek.Common.EStateId callStateID)
		{
			switch (callStateID)
			{
				case Sipek.Common.EStateId.ACTIVE:
					_notifyIcon.ShowBalloonTip(5, "Разговор", "Состояние звонка", System.Windows.Forms.ToolTipIcon.Info);
					break;
				case Sipek.Common.EStateId.ALERTING:
					_notifyIcon.ShowBalloonTip(2, "Вызов!", "Состояние звонка", System.Windows.Forms.ToolTipIcon.Info);
					break;
				case Sipek.Common.EStateId.CONNECTING:
					_notifyIcon.ShowBalloonTip(2, "Подключение...", "Состояние звонка", System.Windows.Forms.ToolTipIcon.Info);
					break;
				case Sipek.Common.EStateId.HOLDING:
					_notifyIcon.ShowBalloonTip(10, "Удержание...", "Состояние звонка", System.Windows.Forms.ToolTipIcon.Warning);
					break;
				case Sipek.Common.EStateId.IDLE:
					_notifyIcon.ShowBalloonTip(2, "Линия свободна...", "Состояние звонка", System.Windows.Forms.ToolTipIcon.Info);
					break;
				case Sipek.Common.EStateId.INCOMING:
					_notifyIcon.ShowBalloonTip(2, "Входящий звонок!", "Состояние звонка", System.Windows.Forms.ToolTipIcon.Info);
					break;
				case Sipek.Common.EStateId.NULL:
					break;
				case Sipek.Common.EStateId.RELEASED:
					_notifyIcon.ShowBalloonTip(1, "Звонок завершен!", "Состояние звонка", System.Windows.Forms.ToolTipIcon.Info);
					break;
				case Sipek.Common.EStateId.TERMINATED:
					_notifyIcon.ShowBalloonTip(1, "Звонок завершен", "Состояние звонка", System.Windows.Forms.ToolTipIcon.Warning);
					break;
			}
		}

		private void DrawUIByCallState(Sipek.Common.EStateId StateId)
		{
			ShowBalloonTips(StateId);
			BrushConverter converter = new BrushConverter();
			switch (StateId)
			{
				case Sipek.Common.EStateId.ACTIVE:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Red;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = converter.ConvertFromString("#FF0B4E2F") as Brush;
						btnHold.Background = null;
					}));

					break;

				case Sipek.Common.EStateId.ALERTING:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Orange;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = converter.ConvertFromString("#FA890404") as Brush;
						btnHold.Background = null;
					}));
					break;

				case Sipek.Common.EStateId.CONNECTING:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = converter.ConvertFromString("#FA890404") as Brush;
						btnHold.Background = null;
					}));
					break;

				case Sipek.Common.EStateId.HOLDING:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Blue;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = null;//converter.ConvertFromString("#FA890404") as Brush;
						btnHold.Background = converter.ConvertFromString("#FF0B4E2F") as Brush;
					}));
					break;

				case Sipek.Common.EStateId.IDLE:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = null;// converter.ConvertFromString("#FA890404") as Brush;
						btnHold.Background = null;// converter.ConvertFromString("#FF17517A") as Brush;
					}));
					break;

				case Sipek.Common.EStateId.INCOMING:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Orange;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = converter.ConvertFromString("#FA890404") as Brush;
						btnHold.Background = null;//converter.ConvertFromString("#FF17517A") as Brush;
					}));
					break;

				case Sipek.Common.EStateId.NULL:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Green;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = null;
						btnHold.Background = null;
					}));
					break;

				case Sipek.Common.EStateId.RELEASED:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = null;//converter.ConvertFromString("#FA890404") as Brush;
						btnHold.Background = null;//converter.ConvertFromString("#FF17517A") as Brush;
					}));
					break;

				case Sipek.Common.EStateId.TERMINATED:
					_notifyIcon.Icon = SIPComm.Properties.Resources.Circle_Yellow;
					_mainDispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						btnCall.Background = null;//converter.ConvertFromString("#FA890404") as Brush;
						btnHold.Background = null;//converter.ConvertFromString("#FF17517A") as Brush;
					}));
					break;
			}
		}

		private void Account_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void LabelSettings_MouseDown(object sender, MouseButtonEventArgs e)
		{
			_configWindow.Show();
		}
	}
}