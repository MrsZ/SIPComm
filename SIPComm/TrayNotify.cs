using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sipek.Common;
using SIPComm;

namespace SIPComm
{
    public class TrayNotify
    {
	   NotifyIcon _tray;
	   MainWindow _instanse;
	   bool _blinking;

	   public NotifyIcon Tray
	   {
		  get { return _tray; }
	   }

	   public TrayNotify(MainWindow instanse)
	   {
		  _instanse = instanse;
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
		  if(_instanse.IsVisible)
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

	   void SetIcon(IAccount account)
	   {
		  switch (account.RegState)
		  {
			 case 200:
				_tray.Icon = Properties.Resources.Circle_Green;
				break;
			 case 0:_tray.Icon = Properties.Resources.Circle_Yellow;
				break;
			 default:
				_tray.Icon = Properties.Resources.Circle_Grey;
				break;
		  }
	   }

	   void SetIcon(IStateMachine callState)
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


	   #region Blink Icon In Tray
	   void BlinkIcon()
	   {
		  Timer t = new Timer();
		  t.Tick += new EventHandler(Timer_Tick);
		  t.Interval = 500;
		  t.Enabled = _blinking;
		  if (_blinking)
			 t.Start();
		  else
			 t.Stop();
	   }

	   private void Timer_Tick(object sender, EventArgs e)
	   {
		  Random rnd = new Random(5);
		  while (_blinking)
		  { 
			 switch(rnd.Next(1,5))
			 {
				case 1: 
				    _tray.Icon = Properties.Resources.Circle_Grey;
				    break;
				case 2:
				    _tray.Icon = Properties.Resources.Circle_Green;
				    break;
				case 3:
				    _tray.Icon = Properties.Resources.Circle_Yellow;
				    break;
				case 4:
				    _tray.Icon = Properties.Resources.Circle_Orange;
				    break;
				case 5:
				    _tray.Icon = Properties.Resources.Circle_Red;
				    break;
				default:
				    _tray.Icon = Properties.Resources.Circle_Grey;
				    break;
			 }
		  }
	   }
	   #endregion
    }
}
