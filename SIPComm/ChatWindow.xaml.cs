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
using System.Windows.Shapes;

namespace SIPComm
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
	   MainWindow _parentWindow;
	   public ChatWindow(MainWindow parentWindow)
	   {
		  InitializeComponent();
		  _parentWindow = parentWindow;
	   }

	   private void MessageBox_KeyDown(object sender, KeyEventArgs e)
	   {
		  if (Key.Enter == e.Key)
		  {
			 this.ChatBox.Text += this.MessageBox.Text + "\n";
			 this.MessageBox.Text = "";
		  }
	   }

	   internal void AddIncomingMessage(string from, string message)
	   {
		  this.ChatBox.Text += from + ": " + message;
	   }
    }
}
