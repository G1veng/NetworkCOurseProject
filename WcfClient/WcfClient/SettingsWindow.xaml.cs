using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;

namespace WcfClient
{
  /// <summary>
  /// Interaction logic for SettingsWindow.xaml
  /// </summary>
  public partial class SettingsWindow : Window
  {
    public SettingsWindow()
    {
      InitializeComponent();

      this.adress.Text = ConfigurationManager.AppSettings["serviceAddress"];
      this.name.Text = ConfigurationManager.AppSettings["serviceName"];
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

      config.AppSettings.Settings.Clear();

      config.AppSettings.Settings.Add("serviceAddress", this.adress.Text);
      config.AppSettings.Settings.Add("serviceName", this.name.Text);

      config.Save(ConfigurationSaveMode.Modified);

      ConfigurationManager.RefreshSection("appSettings");
    }

    private void close_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
