using Sipek.Common;
using System;
using System.Windows.Forms;

namespace SIPComm
{
	public class TrayNotify
	{
		private NotifyIcon _tray;
		private MainWindow _instanse;
		private RegSet _regSet;
		private bool _blinking;

		public NotifyIcon Tray
		{
			get { return _tray; }
		}

		public TrayNotify(MainWindow instanse)
		{
			_instanse = instanse;
			_regSet = new RegSet();
			_tray = new NotifyIcon();
			_tray.Icon = Properties.Resources.Circle_Grey;
			_tray.Visible = true;
			_tray.ContextMenu = new ContextMenu();
			_tray.ContextMenu.MenuItems.Add(new MenuItem("Exit", new EventHandler(this.ExitItem_onClick)));
			_tray.MouseClick += new MouseEventHandler(this.Tray_MouseClick);
		}

		private void ExitItem_onClick(object sender, EventArgs e)
		{
			_tray.Visible = false;
			Environment.Exit(0);
		}

		private void Tray_MouseClick(object sender, MouseEventArgs e)
		{
			if (_instanse.IsVisible)
				_instanse.Hide();
			else
				_instanse.Show();
		}

		public void ShowBalloon(int time, string title, string mess)
		{
			_tray.ShowBalloonTip(time, title, mess, ToolTipIcon.Info);
		}

		public void SetByAccountState(IAccount account)
		{
			SetIcon(account);

			if (!_regSet.TrayMessageEnabled)
			{
				return;
			}

			switch (account.RegState)
			{
				case 200:
					_tray.ShowBalloonTip(10, "Registereg", account.AccountName + ": " + account.UserName, ToolTipIcon.Info);
					break;

				case 0:
					_tray.ShowBalloonTip(10, "Traing...", account.AccountName + ": " + account.UserName, ToolTipIcon.Warning);
					break;
				default:
					_tray.ShowBalloonTip(10, "Error", account.AccountName + ": " + account.UserName, ToolTipIcon.Error);
					break;
			}
		}

		public void SetByCallState(IStateMachine callState)
		{
			SetIcon(callState);

			if (!_regSet.TrayMessageEnabled)
			{
				return;
			}
			switch (callState.StateId)
			{
				case EStateId.ACTIVE:

					//_tray.ShowBalloonTip(10, "ACTIVE", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;

				case EStateId.ALERTING:
					_tray.ShowBalloonTip(10, "ALERTING", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;

				case EStateId.CONNECTING:

					//_tray.ShowBalloonTip(10, "CONNECTING", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;

				case EStateId.HOLDING:
					_tray.ShowBalloonTip(10, "HOLDING", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;

				case EStateId.IDLE:

					//_tray.ShowBalloonTip(10, "IDLE", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;

				case EStateId.INCOMING:
					_tray.ShowBalloonTip(10, "INCOMING", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;

				case EStateId.NULL:

					//_tray.ShowBalloonTip(10, "NULL", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;

				case EStateId.RELEASED:

					//_tray.ShowBalloonTip(10, "RELEASED", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;

				case EStateId.TERMINATED:

					//_tray.ShowBalloonTip(10, "TERMINATED", callState.CallingName + " " + callState.CallingNumber, ToolTipIcon.Info);
					break;
			}
		}

		private void SetIcon(IAccount account)
		{
			switch (account.RegState)
			{
				case 200:
					_tray.Icon = Properties.Resources.Circle_Green;
					break;

				case 0: _tray.Icon = Properties.Resources.Circle_Yellow;
					break;
				default:
					_tray.Icon = Properties.Resources.Circle_Grey;
					break;
			}
		}

		private void SetIcon(IStateMachine callState)
		{
			_blinking = EStateId.INCOMING == callState.StateId;
			switch (callState.StateId)
			{
				case EStateId.ACTIVE:
					_tray.Icon = Properties.Resources.Circle_Red;
					break;

				case EStateId.ALERTING:
					_tray.Icon = Properties.Resources.Circle_Orange;
					break;

				case EStateId.CONNECTING:
					_tray.Icon = Properties.Resources.Circle_Green;
					break;

				case EStateId.HOLDING:
					_tray.Icon = Properties.Resources.Circle_Blue;
					break;

				case EStateId.IDLE:
					_tray.Icon = Properties.Resources.Circle_Grey;
					break;

				case EStateId.INCOMING:
					_tray.Icon = Properties.Resources.Circle_Orange;
					break;

				case EStateId.NULL:
					_tray.Icon = Properties.Resources.Circle_Grey;
					break;

				case EStateId.RELEASED:
					_tray.Icon = Properties.Resources.Circle_Green;
					break;

				case EStateId.TERMINATED:
					_tray.Icon = Properties.Resources.Circle_Green;
					break;
			}
		}
	}
}