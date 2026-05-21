using System;
using System.Data.SqlClient;

namespace NoPaper.Models
{
  [Serializable]
  public class SectorManufactInfo
  {
    public int    ID    { get; private set; }
    public int    nType { get; private set; }
    public string Name  { get; private set; }

    public bool bChangeOrder { get; private set; }

    public SectorManufactInfo(int _ID, int _nType, string _Name, bool _bChangeOrder = false)
    {
      ID    = _ID;
      nType = _nType;
      Name  = _Name;
      bChangeOrder = _bChangeOrder;
    }
  }
}