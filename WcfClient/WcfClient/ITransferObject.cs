using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfClient
{
  [ServiceContract]
  public interface ITransferObject
  {
    [OperationContract]
    string ProcessAlgorithms(string data, string algorithName);

    [OperationContract]
    List<string> GetAlgorithmsTitles();
  }
}
