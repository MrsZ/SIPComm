using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
//using System.Windows.Forms;
using System.Configuration;

namespace SIPComm
{
    public static class Log
    {
		private static string path = "d:\\log.txt";
        public static string Path
        {
            get { return "d:\\log.txt"; }
            set { path = value; }
        }
        private static bool initialised;
        //private static NotifyIcon trey;
        private static bool silentMode;

        public static void Initialize(string logpath)
        {
            if (!initialised)
            {
                DateTime date = DateTime.Now;
                // Create a file to write to.
                using (StreamWriter sw = File.AppendText(string.Format("{0}\\{1}-{2}-{3}.txt",
                        logpath, date.Day, date.Month, date.Year)))
                {
                    // Notify icon
                    //trey = new NotifyIcon();
					//trey.Icon = Properties.Resources._1_red;
                    try
                    {
                        
                    }
                    catch
                    {
                        silentMode = false;
                    }
                    //trey.Visible = !silentMode;
                    sw.WriteLine();
                    sw.WriteLine("[Session]: " + DateTime.Now.TimeOfDay);
                    initialised = true;
                }
            }
        }

        public static void Write(string title, string message)
        {
            Write(title, message, true);
        }
        
        public static void Write(string title, string message, bool notify)
        {
            DateTime date = DateTime.Now;
			Initialize(path);
                using (StreamWriter sw = File.AppendText(string.Format("{0}\\{1}-{2}-{3}.txt",
                        path, date.Day, date.Month, date.Year)))
                {
                    sw.WriteLine(string.Format("[{0}]", message));
                }
            if (!silentMode || !notify)
            {
                //trey.ShowBalloonTip(3000, title, message, MessageType.Exceptn != type ? ToolTipIcon.Info : ToolTipIcon.Error);
                //    Thread.Sleep(3000);
            }
        }
		public static void HideTrayIcon()
		{
			//trey.Visible = false;
		}
    }
}
