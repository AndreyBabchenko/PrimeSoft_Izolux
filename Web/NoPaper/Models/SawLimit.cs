using System;

namespace NoPaper.Models
{
  [Serializable]
  internal class SawLimit
  {
    public int    ID               { get; private set; } 
    public string Name             { get; private set; }
    public int    idSectorManufact { get; private set; }
    public bool   bAssembly;

    Equipment equipment { get; }

    public int    EquipmentID   => equipment.ID;
    public string EquipmentName => equipment.Name;

    public SawLimit(int _ID, string _Name, int _idSectorManufact, Equipment _equipment)
    {
      ID               = _ID;
      Name             = _Name;
      idSectorManufact = _idSectorManufact;
      equipment        = _equipment;
    }
  }
}