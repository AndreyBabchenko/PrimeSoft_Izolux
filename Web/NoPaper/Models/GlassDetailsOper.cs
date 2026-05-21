using System;
using System.Data.SqlClient;
using System.Data;

namespace NoPaper.Models
{
  public struct GlassDetailsOper
  {
    private bool isInitialized;
    public  bool IsInitialized =>  isInitialized;

    public OperatorInfo            operatorInfo;
    public int?                    idGlassDetails,
                                   idBarCode;
    public string                  sIdGlassProcessingPyramidList;
    public SectorManufactInfo      sectorManufact;
    public Action< string, bool >  showMessage;

    public GlassDetailsOper(OperatorInfo _operatorInfo, SectorManufactInfo _sectorManufact, string _idGlassProcessingPyramidList, int? _idBarCode, Action<string, bool> _ShowMessage = null)
    {
      operatorInfo                  = _operatorInfo;
      sIdGlassProcessingPyramidList = _idGlassProcessingPyramidList;
      sectorManufact                = _sectorManufact;
      idBarCode                     = _idBarCode;
      showMessage                   = _ShowMessage;
      idGlassDetails                = null;
      isInitialized                 = true;
    }

    public GlassDetailsOper(OperatorInfo _operatorInfo, SectorManufactInfo _sectorManufact, int? _idGlassDetails, string _sIdGlassProcessingPyramid, int? _idBarCode, Action<string, bool> _ShowMessage = null)
    {
      operatorInfo                  = _operatorInfo;
      idGlassDetails                = _idGlassDetails;
      sIdGlassProcessingPyramidList = _sIdGlassProcessingPyramid;
      sectorManufact                = _sectorManufact;
      idBarCode                     = _idBarCode;
      showMessage                   = _ShowMessage;
      isInitialized                 = true;
    }
  }
}
