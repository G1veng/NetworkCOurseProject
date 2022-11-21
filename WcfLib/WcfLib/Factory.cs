using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfLib
{
  public class Factory
  {
    public Factory() { }

    public Algorithms GetAlgorithm(string algorithmName)
    {
      if (algorithmName == "FIFO")
      {
        return new FIFO();
      }
      if (algorithmName == "LRU")
      {
        return new LRU();
      }

      return null;
    }
  }
}
