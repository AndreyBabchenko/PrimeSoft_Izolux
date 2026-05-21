using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoPaper.View
{
  internal class MainData
  {
    public static string GetMainQuery(string sIdSawTask)
    {
      return $@"{Constants.getBasicQuery()}
                 where
                 idSawTaskMain in ({sIdSawTask})";
    }

    public static string GetMainQueryAssembly(string sIdSawTask)
    {
      return $@"{Constants.getBasicQuery()}
                where
                idSawTaskMain_Assembly in ({sIdSawTask})";
    }
  }
}