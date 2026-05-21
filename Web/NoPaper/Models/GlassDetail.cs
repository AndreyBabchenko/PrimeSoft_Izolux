
using System;

namespace NoPaper.Models
{
  public struct GlassDetail
  {
    private int _ID;
    public int ID => _ID;

    public int  Width, 
                Height,
                nType, 
                Thickness,
                PiramidSide,
                PiramidCell,
                nGlassInPack,
                nGlassInPackTake,
                nPack;

    public bool bFinished;

    public GlassDetail(int id, int width, int height, int _nType, int thickness, int piramidSide, int _nGlassInPack, int _nGlassInPackTake, int _nPack, int piramidCell, bool _bFinished)
    {
      _ID    = id;
      Width  = width;
      Height = height;
      nType = _nType;
      Thickness = thickness;
      PiramidSide  = piramidSide;
      nGlassInPack = _nGlassInPack;
      nGlassInPackTake = _nGlassInPackTake;
      PiramidCell = piramidCell;
      bFinished = _bFinished;
      nPack = _nPack;
    }
  }
}
