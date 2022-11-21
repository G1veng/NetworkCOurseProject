using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfLib
{
  public class TransferObject : ITransferObject
  {
    public string ProcessAlgorithms(string data, string algorithmName)
    {
      Factory factory = new Factory();

      var algorithm = factory.GetAlgorithm(algorithmName);

      if (algorithm != null)
      {
        return algorithm.Algorithm(data);
      }
      return "404";
    }

    public List<string> GetAlgorithmsTitles()
    {
      List<string> titles = new List<string>();
      int maxValue = Enum.GetValues(typeof(AlgorithmTitle)).Cast<int>().Max();
      int minValue = Enum.GetValues(typeof(AlgorithmTitle)).Cast<int>().Min();

      for (int i = minValue; i <= maxValue; i++)
      {
        var enumDisplayStatus = (AlgorithmTitle)i;
        string stringValue = enumDisplayStatus.ToString();

        titles.Add(stringValue);
      }

      return titles;
    }
  }
}
