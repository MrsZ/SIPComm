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
using SIPComm.Properties;

namespace SIPComm
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
	   public ConfigWindow()
	   {
		  InitializeComponent();
	   }

	   private void Window_Loaded_1(object sender, RoutedEventArgs e)
	   {

	   }

	   private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
	   {
		  Settings.Default.Save();
	   }

    }
}
