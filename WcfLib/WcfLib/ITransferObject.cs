using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfLib
{
  // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransferObject" in both code and config file together.
  [ServiceContract]
  public interface ITransferObject
  {
    [OperationContract]
    string ProcessAlgorithms(string data, string algorithName);

    [OperationContract]
    List<string> GetAlgorithmsTitles();
  }
}
