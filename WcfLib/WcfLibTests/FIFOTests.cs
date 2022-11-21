using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfLib.Tests
{
  [TestClass()]
  public class FIFOTests
  {
    [TestMethod()]
    public void GetPagesCountTest()
    {
      string data = "4 1 2 3 4";
      int assumption = 4;

      FIFO fifo = new FIFO();
      var result = fifo.GetPagesCount(data);

      Assert.AreEqual(assumption, result);
    }

    [TestMethod()]
    public void GetPagesNumberTest()
    {
      string data = "2 1 3 4 56 85";
      List<int> assumtion = new List<int>() {2, 1, 3, 4, 56, 85 };

      FIFO fifo = new FIFO();
      var result = fifo.GetPagesNumber(data);

      for (int i = 0; i < assumtion.Count; i++)
      {
        Assert.AreEqual(assumtion[i], result[i]);
      }
    }

    [TestMethod()]
    public void CutPagesCountTest()
    {
      string data = "4 1 2 3 4";
      string assumption = "1 2 3 4";

      FIFO fifo = new FIFO();
      var result = fifo.CutPagesCount(data);

      Assert.AreEqual(assumption, result);
    }

    [TestMethod()]
    public void GetInitialPagesTest()
    {
      int pagesCount = 4;
      List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8};
      List<int> assumption = new List<int>() { 1, 2, 3, 4 };

      FIFO fifo = new FIFO();
      var result = fifo.GetInitialPages(numbers, pagesCount);

      for (int i = 0; i < result.Count; i++)
      {
        Assert.AreEqual(result[i], assumption[i]);
      }
    }

    [TestMethod()]
    public void CutInitialPagesTest()
    {
      int pagesCount = 4;
      List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
      List<int> assumption = new List<int>() { 5, 6, 7, 8};

      FIFO fifo = new FIFO();
      var result = fifo.CutInitialPages(numbers, pagesCount);

      for (int i = 0; i < result.Count; i++)
      {
        Assert.AreEqual(result[i], assumption[i]);
      }
    }
    //2 1 3 4 56 85
    [TestMethod()]
    public void CollectPagesTest()
    {
      List<int> data = new List<int>() { 1, 2, 3, 4};
      bool isInteruption = true;

      FIFO fifo = new FIFO();
       
      fifo.CollectPages(data, isInteruption);
    }

    [TestMethod()]
    public void FIFOAlgTest()
    {
      string assumption = "1 2 3 4\r\n2 3 4 9 |\r\n3 4 9 8 |\r\n4 9 8 7 |\r\n9 8 7 6 |\r\n8 7 6 4 |\r\n5";
      int pagesCount = 4;
      List<int> initialPages = new List<int>() { 1, 2, 3, 4, };
      List<int> externalPages = new List<int>() { 9, 8, 7, 6, 4, };

      FIFO fifo = new FIFO();

      var result = fifo.FIFOAlg(pagesCount, initialPages, externalPages);

      Assert.AreEqual(assumption, result);
    }

    [TestMethod()]
    public void AlgorithmTest()
    {
      //string assumption = "11 2 3 4\r\n2 3 4 9 |\r\n3 4 9 8 |\r\n4 9 8 7 |\r\n9 8 7 6 |\r\n8 7 6 4 |\r\n5";
      //string data = "4 4 11 2 3 4 9 8 7 6 4";
      string assumption = "3\r\n3 4 |\r\n4 56 |\r\n56 85 |\r\n3";
      string data = "2 1 3 4 56 85";

      FIFO fifo = new FIFO();

      var result = fifo.Algorithm(data);

      Assert.AreEqual(assumption, result);
    }
  }
}