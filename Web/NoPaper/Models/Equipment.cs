using System;

namespace NoPaper.Models
{
  [Serializable]
  internal class Equipment
  {
    public int    ID   { get; private set; }
    public string Name { get; private set; }

    public Equipment(int _ID, string _Name)
    {
      ID   = _ID;
      Name = _Name;
    }
  }
}