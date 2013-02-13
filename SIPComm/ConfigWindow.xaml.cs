using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Security.AccessControl;
using WaveLib.AudioMixer;

namespace SIPComm
{
	/// <summary>
	/// Interaction logic for ConfigWindow.xaml
	/// </summary>
	public partial class ConfigWindow : Window
	{
		private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\SIPComm");
		Agent _agent;
		Mixers mMixers;
		public ConfigWindow(Agent agent)
		{
			_agent = agent;
			mMixers = new Mixers();
			InitializeComponent();
			regKey.SetAccessControl(new RegistrySecurity());
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			TabSettings.SelectedIndex--;
			TabSettings.SelectedIndex++;
			//e.Cancel = true;
			//Hide();
		}		
	}
}
