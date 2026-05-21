namespace NoPaper.Models
{
  internal class BarCodeInfo
  {
    private protected bool isInitialized = false;
    public            bool IsInitialized => isInitialized;

    public int    ID;
    public string barCode;

    public BarCodeInfo()
    {
      ID = 0;
      barCode = "";
      isInitialized = false;
    }

    public BarCodeInfo(int _ID, string _barCode)
    {
      ID = _ID;
      barCode = _barCode;
      isInitialized = true;
    }
  }
}