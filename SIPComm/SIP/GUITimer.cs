using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Sipek.Common;
using System.Runtime.InteropServices;

namespace SIPComm
{
	//[ClassInterfaceAttribute(ClassInterfaceType.AutoDual)]
	public class GUITimer : ITimer
	{
		Timer _guiTimer;

		public GUITimer()
		{
			_guiTimer = new Timer();
			if (this.Interval > 0) _guiTimer.Interval = this.Interval;
			_guiTimer.Interval = 100;
			_guiTimer.Enabled = true;
			_guiTimer.Elapsed += new ElapsedEventHandler(_guiTimer_Tick);
		}

		void _guiTimer_Tick(object sender, EventArgs e)
		{
			_guiTimer.Stop();
			/*
			//_elapsed(sender, e);
			// Synchronize thread with GUI because SIP stack works with GUI thread only
			if ((_form.IsDisposed) || (_form.Disposing) || (!_form.IsInitialized))
			{
				return;
			}
			_form.Invoke(_elapsed, new object[] { sender, e });
			 */
			return;
		}

		public bool Start()
		{
			_guiTimer.Start();
			return true;
		}

		public bool Stop()
		{
			_guiTimer.Stop();
			return true;
		}

		private int _interval;
		public int Interval
		{
			get { return _interval; }
			set { _interval = value; _guiTimer.Interval = value; }
		}

		private TimerExpiredCallback _elapsed;
		public TimerExpiredCallback Elapsed
		{
			set
			{
				_elapsed = value;
			}
		}
	}
}
