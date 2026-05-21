namespace NoPaper.Models
{
  class FrameSize
  {
    public int   num;
    public double leng;
    public double lengReal;
    public double R;
    public double R_Real;


    public FrameSize(int _num, double _leng, double _lengReal, double _R, double _R_Real)
    {
      num = _num;
      leng = _leng;
      lengReal = _lengReal;
      R = _R;
      R_Real = _R_Real;
    }
  }

  class ShprosInfo
  {
    public string MarkIdentic;
    public int    Count;
    public double LengReal;
    public float  AngleLef;
    public float  AngleRig;


    public ShprosInfo(string markIdentic, int count, double lengReal, float angleLef, float angleRig)
    {
      MarkIdentic = markIdentic;
      Count       = count;
      LengReal    = lengReal;
      AngleLef    = angleLef;
      AngleRig    = angleRig;
    }
  }

}