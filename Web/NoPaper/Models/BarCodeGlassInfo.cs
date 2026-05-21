namespace NoPaper.Models
{
  internal class BarCodeGlassInfo : BarCodeInfo
  {
    public int idBarCode;
    public int idProjectItem;

    public BarCodeGlassInfo() : base()
    { 
    }

    public BarCodeGlassInfo(int _ID, string _barCode, int _idBarCode, int _projectItem) : base(_ID, _barCode)
    {
      idProjectItem = _projectItem;
      idBarCode     = _idBarCode;
    }
  }
}