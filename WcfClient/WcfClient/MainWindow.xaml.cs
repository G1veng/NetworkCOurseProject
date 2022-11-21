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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Configuration;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Threading;

namespace WcfClient
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    private ITransferObject _service;
    private string _data = string.Empty;

    private int _state = 0;

    public MainWindow()
    {
      InitializeComponent();

      SetInitialChart();
    }

    private void SetInitialChart()
    {
      WpfPlot1.Plot.XLabel(" ");
      WpfPlot1.Plot.YLabel(" ");

      WpfPlot1.Plot.YAxis.Hide(true);
      WpfPlot1.Plot.XAxis.Hide(true);

      WpfPlot1.Plot.YAxis2.Hide(true);
      WpfPlot1.Plot.XAxis2.Hide(true);
      
    }

    private void DrawChart(double[] values, string[] labels)
    {
      WpfPlot1.Plot.Clear();

      WpfPlot1.Plot.YAxis.Hide(false);
      WpfPlot1.Plot.XAxis.Hide(false);

      WpfPlot1.Plot.YLabel("Interruption amount");
      WpfPlot1.Plot.XLabel("Algorithms");

      WpfPlot1.Plot.AddBar(values);
      WpfPlot1.Plot.XTicks(labels);
      
      WpfPlot1.Refresh();
    }

    private void CalculateButton_Click(object sender, RoutedEventArgs e)
    {
      string result = string.Empty;
      string value = string.Empty;

      try
      {
        _state = 0;

        ComboBoxItem typeItem = (ComboBoxItem)comboBox.SelectedItem;
        value = typeItem.Content.ToString();

        TimerCallback tm = new TimerCallback(TimeError);
        Timer timer = new Timer(tm, null, 5000, Timeout.Infinite);

        var res = Task.Run(() => _service.ProcessAlgorithms(_data, value));
        
        while(res.Status != TaskStatus.RanToCompletion)
        {
          Thread.Sleep(10);

          if (_state == 1)
          {
            break;
          }
        }

        if (res.Status == TaskStatus.RanToCompletion)
        {
          

          FillChart();

          this.output.Text = res.Result;
          this.saveButton.IsEnabled = true;
        }
      }
      catch
      {
        MessageBox.Show("Unable to connect to the server");
        
      }
    }

    public void TimeError(object obj)
    {
      MessageBox.Show("Could not get respond from server");
      _state = 1;
    }

    private void FillChart()
    {
      List<string> result = _service.GetAlgorithmsTitles();
      List<string> labels = new List<string>();
      List<double> algResult = new List<double>();
      string algAnsw = string.Empty;

      ComboBoxItem typeItem = (ComboBoxItem)comboBox.SelectedItem;
      string value = typeItem.Content.ToString();

      result.Remove(value);
      
      labels.Add(value);

      foreach (string label in result)
      {
        labels.Add(label);
      }

      for(int i = 0; i < labels.Count; i++)
      {
        algAnsw = _service.ProcessAlgorithms(_data, labels[i]);
        algResult.Add(GetNumber(algAnsw));
      }

      var arrLabels = labels.ToArray();
      var arrValues = algResult.ToArray();

      DrawChart(arrValues, arrLabels);
    }

    private void MenuSettings_Click(object sender, RoutedEventArgs e)
    {
      SettingsWindow settingsWindow = new SettingsWindow();

      settingsWindow.ShowDialog();
    }

    private void MenuInformation_Click(object sender, RoutedEventArgs e)
    {
      InformationWindow informationWindow = new InformationWindow();
      informationWindow.Show();
    }

    private double GetNumber(string value)
    {
      int startIndex = 0;
      string result = string.Empty;

      for (int i = value.Length - 1; i >= 0; i--)
      {
        if (value[i] != '\n')
        {
          continue;
        }

        startIndex = i;
        break;
      }

      for (int i = startIndex; i < value.Length; i++)
      {
        result += value[i].ToString();
      }

      return Double.Parse(result);
    }

    private void ClearComboBox()
    {
      if (this.comboBox.Items != null)
      {
        this.comboBox.Items.Clear();
      }
    }

    private void FillComboBox()
    {
      List<string> result = null;

      try
      {
        result = _service.GetAlgorithmsTitles();
      }
      catch
      {
        MessageBox.Show("Unable to connect to the server");
        return;
      }

      for (int i = 0; i < result.Count; i++)
      {
        this.comboBox.Items.Add(new ComboBoxItem { Content = result[i] });
      }
    }

    private bool TryConnect()
    {
      try
      {
        _service.GetAlgorithmsTitles();
      }
      catch
      {
        MessageBox.Show("Unable to connect to the server");
        return false;
      }
      return true;
    }

    private void Reset()
    {
      this.calculateButton.IsEnabled = false;
      this.saveButton.IsEnabled = false;
    }

    private void ConnectButton_Click(object sender, RoutedEventArgs e)
    {
      ClearComboBox();
      Reset();

      Uri tcpUri = new Uri($"net.tcp://{ConfigurationManager.AppSettings["serviceAddress"]}/{ConfigurationManager.AppSettings["serviceName"]}");
      EndpointAddress address = new EndpointAddress(tcpUri);
      NetTcpBinding clientBinding = new NetTcpBinding();
      ChannelFactory<ITransferObject> factory = new ChannelFactory<ITransferObject>(clientBinding, address);
      _service = factory.CreateChannel();

      if (TryConnect())
      {
        FillComboBox();
      }
    }

    private void SetDataButton_Click(object sender, RoutedEventArgs e)
    {
      Reset();

      Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

      bool? result = openFileDlg.ShowDialog();
      if (result == true)
      {

        string data = System.IO.File.ReadAllText(openFileDlg.FileName);

        this._data = data;
        this.editButton.IsEnabled = true;

        if (CheckData(data))
        {
          if (comboBox.SelectedItem != null)
          {
            this.calculateButton.IsEnabled = true;
          }
          
        }
      }
    }

    private bool CheckData(string data)
    {
      data = data.TrimEnd('\n');
      data = data.TrimEnd('\r');

      if (data.Length == 0)
      {
        return false;
      }

      for(int i = 0; i < data.Length; i++)
      {
        int counter = 0;

        if (data[i] == ' ')
        {
          counter++;
        }

        else
        {
          counter = 0;
        }

        if (counter > 1)
        {
          return false;
        }
      }

      for (int i = 0; i < data.Length; i++)
      {
        if (data[i] == ' ' || Int32.TryParse(data[i].ToString(), out int res))
        {
          continue;
        }
        else
        {
          return false;
        }
      }

      int size = GetFirstNumber(data);
      data = CutFirstNumber(data);

      if(size <= 0)
      {
        return false;
      }

      int initialAmount = GetFirstNumber(data);
      data = CutFirstNumber(data);

      if(size < initialAmount)
      {
        return false;
      }

      for (int i = 0; i < size; i++)
      {
        try
        {
          GetFirstNumber(data);
          data = CutFirstNumber(data);
        }
        catch
        {
          return false;
        }
      }

      return true;
    }

    private int GetFirstNumber(string data)
    {
      string result = string.Empty;

      for (int i = 0; i < data.Length; i++)
      {
        if (Int32.TryParse(data[i].ToString(), out int num))
        {
          result += data[i].ToString();
        }
        else
        {
          break;
        }
      }
      if (result.Length == 0)
      {
        throw new Exception("Result is not a number");
      }

      return Int32.Parse(result);
    }

    private string CutFirstNumber(string data)
    {
      string result = string.Empty;
      int startIndex = 0;

      for (int i = 0; i < data.Length; i++)
      {
        if (!Int32.TryParse(data[i].ToString(), out int num))
        {
          startIndex = i;
          break;
        }
      }

      return data.Substring(startIndex + 1);
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
      EditWindow editWindow = new EditWindow(_data);
      editWindow.ShowDialog();

      this._data = editWindow.Text;

      if (CheckData(this._data))
      {
        if (comboBox.SelectedItem != null)
        {
          this.calculateButton.IsEnabled = true;
        }
        this.editButton.IsEnabled = true;
      }
      else
      {
        this.calculateButton.IsEnabled = false;
      }
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (CheckData(this._data))
      {
        if (comboBox.SelectedItem != null)
        {
          this.calculateButton.IsEnabled = true;
        }
        this.editButton.IsEnabled = true;
      }
      else
      {
        this.calculateButton.IsEnabled = false;
      }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

      if (saveFileDialog.ShowDialog() == false)
      {
        return;
      }

      System.IO.File.WriteAllText(saveFileDialog.FileName, this.output.Text);
      MessageBox.Show("File saved");
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
