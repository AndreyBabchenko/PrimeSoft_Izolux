using System;

namespace NoPaper.Models
{
  enum TypeEquipment
  {
    e_default  = 0, // На всех участках
    e_assembly = 1  // Только на сборке
  }

  [Serializable]
  internal class Equipment
  {
    public int ID { get; private set; }
    public string Name { get; private set; }

    public TypeEquipment typeEquipment;
    public Equipment(int _ID, string _Name, TypeEquipment type = TypeEquipment.e_default)
    {
      ID   = _ID;
      Name = _Name;
      typeEquipment = type;
    }

  }
}