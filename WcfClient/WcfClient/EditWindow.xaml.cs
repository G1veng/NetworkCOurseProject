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

namespace WcfClient
{
  /// <summary>
  /// Interaction logic for EditWindow.xaml
  /// </summary>
  public partial class EditWindow : Window
  {
    private string _text = string.Empty;
    public string Text { get => _text; set { _text = value; } }
    public EditWindow(string data)
    {
      InitializeComponent();

      Text = data;
      this.data.Text = Text;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
      Text = this.data.Text;
      this.Close();
    }

    private void Check_Click(object sender, RoutedEventArgs e)
    {
      MessageBox.Show(CheckData(this.data.Text));
    }

    private string CheckData(string data)
    {
      data = data.TrimEnd('\n');
      data = data.TrimEnd('\r');

      if (data.Length == 0)
      {
        return "Input is null";
      }

      for (int i = 0; i < data.Length; i++)
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
          return "More than one space";
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
          return "Not only numbers in data";
        }
      }

      int size = GetFirstNumber(data);
      data = CutFirstNumber(data);

      if (size <= 0)
      {
        return "Amount of blocks less than zero";
      }

      int initialAmount = GetFirstNumber(data);
      data = CutFirstNumber(data);

      if (size < initialAmount)
      {
        return "Amount of blocks less than inital filled blocks";
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
          return "Not only numbers in data or extra spaces";
        }
      }

      return "All data is correct";
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

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      Text = this.data.Text;
    }
  }
}
