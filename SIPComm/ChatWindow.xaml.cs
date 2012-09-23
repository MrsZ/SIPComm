using System.Windows;
using System.Windows.Input;

namespace SIPComm
{
	/// <summary>
	/// Interaction logic for ChatWindow.xaml
	/// </summary>
	public partial class ChatWindow : Window
	{
		private MainWindow _parentWindow;

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