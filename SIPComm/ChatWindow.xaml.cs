using System.Windows;
using System.Windows.Input;

namespace SIPComm
{
	/// <summary>
	/// Interaction logic for ChatWindow.xaml
	/// </summary>
	public partial class ChatWindow : Window
	{
		private Agent _agent;

		public ChatWindow(Agent agent)
		{
			InitializeComponent();
			_agent = agent;
		}

		private void MessageBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (Key.Enter == e.Key)
			{
				this.ChatBox.Text += this.MessageBox.Text + "\n";
				_agent.SendMessage("sip:1020@10.10.10.1:5070", this.MessageBox.Text);
				this.MessageBox.Text = "";
			}
		}

		internal void AddIncomingMessage(string from, string message)
		{
			this.ChatBox.Text += from + ": " + message;
		}
	}
}