using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfLib;
using System.Configuration;

namespace WcfServer
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var host = new ServiceHost(typeof(TransferObject), new Uri($"net.tcp://{ConfigurationManager.AppSettings["serviceAddress"]}/{ConfigurationManager.AppSettings["serviceName"]}"));
      var serverBinding = new NetTcpBinding();

      host.AddServiceEndpoint(typeof(ITransferObject), serverBinding, "");
      host.Open();

      Console.WriteLine("Host has started");
      Console.ReadKey();
    }
  }
}
