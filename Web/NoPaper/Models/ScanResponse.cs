using System.Data.SqlTypes;

namespace NoPaper.Models
{

  public class PostGlassProcessing
  {
    public int IDGlassDetails { get; set; }
    public int IDPyramid { get; set; }
    public int IDSawTaskMain { get; set; }
    public int IDGlassProcessing { get; set; }
    public int IDGlassProcessingPyramid { get; set; }
    public int IDBarCode { get; set; }
    public string Barcode { get; set; }

    public PostGlassProcessing(int idGlassDetails, int idPyramid, int idSawTaskMain, int idGlassProcessing, int idGlassProcessingPyramid, int idBarCode, string barcode)
    {
      IDGlassDetails    = idGlassDetails;
      IDPyramid         = idPyramid;
      IDSawTaskMain     = idSawTaskMain;
      IDGlassProcessing = idGlassProcessing;
      IDGlassProcessingPyramid = idGlassProcessingPyramid;
      IDBarCode         = idBarCode;
      Barcode           = barcode;
    }

    public PostGlassProcessing()
    {
      IDGlassDetails           = 0;
      IDPyramid                = 0;
      IDSawTaskMain            = 0;
      IDGlassProcessing        = 0;
      IDGlassProcessingPyramid = 0;
      IDBarCode                = 0;
      Barcode                  = "";
    }
  }

  public enum ScanType
  {
    Unknown = 0,          // Неизвестно
    Operator = 1,         // Операторов
    Barcode = 2,          // Стекла или СП
    GlassProcessingPyramid = 3 // Внутрицеховой пирамиды
  }

  public interface IScanResult
  {
    ScanType Type { get; }
  }

  public class UnknowResult : IScanResult
  {
    public ScanType Type => ScanType.Unknown;
  }

  public class OperatorScanResult : IScanResult
  {
    public int ID { get; set; }
    public int IDSectorManufact { get; set; }
    public string Name { get; set; }

    public ScanType Type => ScanType.Operator;

    public OperatorScanResult(int id, int idSectorManufact, string name)
    {
      ID = id;
      IDSectorManufact = idSectorManufact;
      Name = name;
    }
  }

  public class BarcodeScanResult : IScanResult
  {
    public string Barcode { get; set; }
    public int IDGlassProcessing { get; set; }
    public int IDGlassProcessingPyramid { get; set; }
    public int IDSectorManufact { get; set; }
    public int IDPyramid { get; set; }

    public ScanType Type => ScanType.Barcode;

    public BarcodeScanResult(string barcode, int idGlassProcessing, int idGlassProcessingPyramid, int idSectorManufact, int idPyramid)
    {
      Barcode = barcode;
      IDGlassProcessing = idGlassProcessing;
      IDGlassProcessingPyramid = idGlassProcessingPyramid;
      IDSectorManufact = idSectorManufact;
      IDPyramid = idPyramid;
    }
  }

  public class GlassProcessingPyramidResult : IScanResult
  {
    public int ID { get; set; }

    public string Barcode { get; set; }

    public ScanType Type => ScanType.GlassProcessingPyramid;

    public GlassProcessingPyramidResult(int id, string barcode)
    {
      ID = id;
      Barcode = barcode;
    }
  }

  public class ScanResponse<T> where T : IScanResult
  {
    public string Message { get; set; }
    public bool IsSuccess { get; set; }

    public bool IsNeedRetryScan { get; }
    public T Data { get; set; }

    public ScanResponse(string message, bool isSuccess, T data = default, bool isNeedRetryScan = false)
    {
      Message = message;
      IsSuccess = isSuccess;
      Data = data;
      IsNeedRetryScan = isNeedRetryScan;
    }

    public static ScanResponse<T> Ok(T data, string message = "") => new ScanResponse<T>(message, true, data);
    public static ScanResponse<T> Fail(T data, string message, bool isNeedRetryScan = false) => new ScanResponse<T>(message, false, data, isNeedRetryScan);
    public static ScanResponse<T> Fail(string message) => new ScanResponse<T>(message, false, default);
  }

  public class DBConfig
  {
    /// <summary>
    /// Префикс транспортной пирамиды
    /// </summary>
    public string sBarCodePrefixPyramid { get; set; }     
    /// <summary>
    /// Префикс внутрицеховой пирамиды
    /// </summary>
    public string sBarCodePrefixGlassProcessingPyramid { get; set; }
    /// <summary>
    ///  Префикс штрихкода оператора
    /// </summary>
    public string sBarCodePrefixOperator { get; set; }
    /// <summary>
    ///  Префикс штрихкода отгрузки
    /// </summary>
    public string sBarCodePrefixShip { get; set; }

    /// <summary>
    /// Использовать документ _T("Задание в производство")
    /// </summary>
    public bool bUseTaskToManuf { get; set; }

    /// <summary>
    /// Выставлять статус отгружен при сканировании ШК
    /// </summary>
    public bool bSetDelivStatus { get; set; }

    /// <summary>
    /// Данные полученны успешно?
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Длина штрих-кода отгрузки
    /// </summary>
    public int nBarCodeSHLeng { get; set; }


    public DBConfig(string sBarCodePrefixPyramid, string sBarCodePrefixGlassProcessingPyramid, string sBarCodePrefixOperator, string sBarCodePrefixShip, int nBarCodeSHLeng, bool bUseTaskToManuf, bool bSetDelivStatus)
    {
      this.sBarCodePrefixPyramid  = sBarCodePrefixPyramid;
      this.sBarCodePrefixGlassProcessingPyramid = sBarCodePrefixGlassProcessingPyramid;
      this.sBarCodePrefixOperator = sBarCodePrefixOperator;
      this.sBarCodePrefixShip     = sBarCodePrefixShip;
      this.nBarCodeSHLeng         = nBarCodeSHLeng;
      this.bUseTaskToManuf        = bUseTaskToManuf;
      this.bSetDelivStatus        = bSetDelivStatus;
      IsSuccess                   = true;
    }

    public DBConfig()
    {
      sBarCodePrefixPyramid  = "";
      sBarCodePrefixGlassProcessingPyramid = "";
      sBarCodePrefixOperator = "OPER";
      sBarCodePrefixShip     = "SH";
      nBarCodeSHLeng         = 10;
      bUseTaskToManuf        = false;
      bSetDelivStatus        = false;

      IsSuccess = false;
    }
  }

}
