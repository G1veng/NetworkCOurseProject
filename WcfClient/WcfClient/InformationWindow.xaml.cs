﻿using System;
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
  /// Interaction logic for InformationWindow.xaml
  /// </summary>
  public partial class InformationWindow : Window
  {
    public InformationWindow()
    {
      InitializeComponent();

      this.info.Text = System.IO.File.ReadAllText(@"D:/5 семестр/Сети/Курсач/WcfClient/WcfClient/Resources/Information.txt");
    }
  }
}
