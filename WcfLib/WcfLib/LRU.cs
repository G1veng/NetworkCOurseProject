using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfLib
{
  internal class LRU : Algorithms
  {
    private bool HasNextNumber(string data)
    {
      if (data.Length == 0)
      {
        return false;
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

    public string Algorithm(string data)
    {
      string result = "";
      List<int> initialPages = new List<int>();
      List<int> externalPages = new List<int>();

      /*int pagesCount = GetPagesCount(data);
      data = CutPagesCount(data)*/;

      int pagesCount = GetFirstNumber(data);
      data = CutFirstNumber(data);

      int initialFilled = GetFirstNumber(data);
      data = CutFirstNumber(data);

      externalPages = GetPagesNumber(data);

      //initialPages = GetInitialPages(externalPages, pagesCount);
      initialPages = GetInitialPages(externalPages, initialFilled);
      externalPages = CutInitialPages(externalPages, initialFilled);

      result = LRUAlg(pagesCount, initialPages, externalPages);

      return result;
    }

    public List<int> GetInitialPages(List<int> data, int pagesCount)
    {
      List<int> initialPages = new List<int>();

      for (int i = 0; i < pagesCount; i++)
      {
        initialPages.Add(data[i]);
      }

      return initialPages;
    }

    public List<int> CutInitialPages(List<int> data, int pagesCount)
    {
      data.RemoveRange(0, pagesCount);

      return data;
    }

    public int GetPagesCount(string data)
    {
      int count = Int32.Parse(data[0].ToString());

      return count;
    }

    public string CutPagesCount(string data)
    {
      return data.Remove(0, 2);
    }

    public List<int> GetPagesNumber(string data)
    {
      List<int> numbers = new List<int>();

      while (HasNextNumber(data))
      {
        numbers.Add(GetFirstNumber(data));
        data = CutFirstNumber(data);
      }

      return numbers;
    }

    public string CollectPages(List<int> pages, bool isIntreuption)
    {
      string result = "";

      for (int i = 0; i < pages.Count; i++)
      {
        result += pages[i].ToString() + " ";
      }

      if (isIntreuption)
      {
        result = result + "|" + Environment.NewLine;
      }

      else
      {
        result = result.TrimEnd(' ');
        result += Environment.NewLine;
      }

      return result;
    }

    private string LRUAlg(int pagesCount, List<int> initialPages, List<int> externalPages)
    {
      string result = "";

      result += CollectPages(initialPages, false);
      //Console.WriteLine("LRU");
      //ShowBlocks(pages, false);
      int countOfInterruptions = 0;
      for (int i = 0; i < externalPages.Count; i++)
      {
        if (initialPages.Count < pagesCount && !initialPages.Contains(externalPages[i]))
        {
          initialPages.Add(externalPages[i]);
          countOfInterruptions++;
          result += CollectPages(initialPages, true);
          //ShowBlocks(pages, true);
          continue;
        }
        if (initialPages.Contains(externalPages[i]))
        {
          int counter = 0;
          while (initialPages[counter] != externalPages[i])
            counter++;
          int save = initialPages[counter];
          for (int j = pagesCount - 1; j > counter; j--)
          {
            int temp = initialPages[counter];
            initialPages[counter] = initialPages[j - 1];
            if (j == pagesCount - 1)
              initialPages[j - 1] = initialPages[j];
            else
              initialPages[j - 1] = temp;
          }
          initialPages[pagesCount - 1] = save;
          result += CollectPages(initialPages, false);
          //ShowBlocks(pages, false);
        }
        else
        {
          for (int j = 0; j < pagesCount - 1; j++)
          {
            initialPages[j] = initialPages[j + 1];
          }
          initialPages[pagesCount - 1] = externalPages[i];
          countOfInterruptions++;
          result += CollectPages(initialPages, true);
          //ShowBlocks(pages, true);
        }
      }
      result += countOfInterruptions.ToString();
      //Console.WriteLine(countOfInterruptions);
      return result;
    }
  }
}
