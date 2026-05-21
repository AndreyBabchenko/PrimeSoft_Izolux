using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Web.Services.Description;
using NoPaper.Controllers;
using NoPaper.Models;
using Utils;
using log4net;

// Сканирование стеклопакетов

namespace NoPaper
{
  public partial class Scan1 : PageModel
  {
    enum ETypeBarCode
    {
      e_type_unknow  = 0,
      e_type_oper    = 1,
      e_type_pyramid = 2,
      e_type_barcode = 3,
      e_type_ship    = 4
    }

    public struct ResonseString
    {
      bool         isSuccess;
      string       message;
      ETypeBarCode code;
    }

    private static SqlConnection _conn;
    
    private static readonly ILog log = LogManager.GetLogger(typeof(workplace));

    public static int m_idOperator         = 0,
                      m_idPyramidCompleted = 0,
                      m_PackNum            = 0,
                      m_PackPosNum         = 0,
                      m_nBarCodeSHLeng     = 0;

    public static double m_dPyramidMaxWeight = 0.0,
                         m_dCurrentWeight    = 0.0;

    public static SectorManufactInfo m_sector;

    public static string m_sBarCodePrefixPyramid  = ""; // Префикс штрихкода транспортной пирамиды
    public static string m_sBarCodePrefixOperator = ""; // Префикс штрихкода оператора
    public static string m_sBarCodePrefixShip     = ""; // Префикс штрихкода отгрузки
    public static bool   m_bUseTaskToManuf,   // Использовать документ _T("Задание в производство")
                                              // Взято из Гласс где данное поле читается из реестра
                                              // true: необходимо вызывать sp_UpdateManufTaskState
                                              // false: необходимо вызывать sp_UpdateTaskState
                         m_bSetDelivStatus;

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object PostBarCode(string barcode, int idOperator, int idPyramidCompleted)
    {
      m_idOperator         = idOperator;
      m_idPyramidCompleted = idPyramidCompleted;

      Tuple<bool, string, int>  result = CheckDefaultStateBarCode(barcode);

      if ( !result.Item1 )
        return new
        {
          message            = result.Item2,
          idPyramidCompleted = m_idPyramidCompleted
        };

      string operBarCodePrefix = barcode.Substring(0, m_sBarCodePrefixOperator.Length).ToLower(),
             barCodePrefix     = barcode.Substring(0, m_sBarCodePrefixPyramid.Length).ToLower();
      bool   bScanShip         = barcode.Substring(0, m_sBarCodePrefixShip.Length).ToLower() == m_sBarCodePrefixShip.ToLower(); // Баркод отгрузки

      if ( operBarCodePrefix == m_sBarCodePrefixOperator && barcode.Length > 0 )
      {
        log.Info($"Префикс оператора введенный: {operBarCodePrefix}, Префикс оператора в базе: {m_sBarCodePrefixOperator}");
        result = SetOperator(barcode);                            // Сканирован оператор
      }  
      else if ( barCodePrefix == m_sBarCodePrefixPyramid && barcode.Length > 0 )
      {
        log.Info($"Префикс пирамиды введенный: {barCodePrefix}, Префикс пирамиды в базе: {m_sBarCodePrefixPyramid}");
        result = WriteTransportPyramid(barcode);                  // Сканирована транспортная пирамида
      }
      else if ( bScanShip ) 
        result = WriteBarCodeShip(barcode);                       // Сканирована отгрузка
      else if ( barcode.Length > 0 && m_idPyramidCompleted != 0 )
        result = WriteBarCode(barcode);                           // Сканирован штрихкод
      else
        result = new Tuple<bool, string, int>(false, $"Ошибка сканирования неизвестная команда: {barcode}", (int)ETypeBarCode.e_type_unknow);

      log.Info(result.Item2); // Логируем результат

      return new
      {
        message            = result.Item2,
        idPyramidCompleted = m_idPyramidCompleted,
        idOperator         = m_idOperator,
        typeBarCode        = result.Item3,
        currentMassIsBigger= m_dCurrentWeight > m_dPyramidMaxWeight,
        m_dPyramidMaxWeight,
        m_dCurrentWeight
      };
    }

    // Метод для получения префикса из базы или кеша
    private static string GetBarCodePrefixPyramid()
    {
      // Проверяем, есть ли значение в памяти приложения
      string prefix = HttpContext.Current.Application["BarCodePrefixPyramid"] as string;
      if (string.IsNullOrEmpty(prefix))
      {
        // Если значения нет, запрашиваем из базы и сохраняем в память приложения
        prefix = LoadDBPrefix().ToLower();
        HttpContext.Current.Application["BarCodePrefixPyramid"] = prefix;
      }
      return prefix;
    }

    public static Tuple<bool, string, int> CheckDefaultStateBarCode(string barcode)
    {
      // Не сканируем пустоту
      if (barcode.Length == 0)
        return new Tuple<bool, string, int>(false, "Требуется ввести команду", (int)ETypeBarCode.e_type_unknow);

      if (barcode.Length < m_sBarCodePrefixPyramid.Length)
        return new Tuple<bool, string, int>(false, $"Ошибка сканирования неизвестная команда: {barcode}", (int)ETypeBarCode.e_type_unknow);

      // Баркод прошел базовую проверку
      return new Tuple<bool, string, int>(true, "", (int)ETypeBarCode.e_type_unknow);
    }

    [WebMethod]
    public static Tuple<bool, string, int> SetOperator(string barcodeText)
    {
      try
      {
        log.Info($"Сканирование Оператора: {barcodeText}");
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();
  
          string query = $"select ID from Operator where BarCode = @barcodeText";
          SqlCommand sqlCommand = new SqlCommand(query, conn);
          sqlCommand.Parameters.AddWithValue("@barcodeText", barcodeText);
  
          object result = sqlCommand.ExecuteScalar();
  
          if (result != null)
          {
            m_idOperator = Convert.ToInt32(result);
            return new Tuple<bool, string, int>(true, $"Отсканирован оператор: {barcodeText}",  (int)ETypeBarCode.e_type_oper);
          }
          else
            return new Tuple<bool, string, int>(false, $"Отсканирован не найден: {barcodeText}", (int)ETypeBarCode.e_type_oper);
        }
      }
      catch
      {
        log.Error($"Отсканирован не найден: {barcodeText}");
        return new Tuple<bool, string, int>(false, $"Отсканирован не найден: {barcodeText}", (int)ETypeBarCode.e_type_oper);
      }
    }


    [WebMethod]
    public static Tuple<bool, string, int> WriteTransportPyramid(string barcodeText)
    {
      try
      {
        log.Info($"Сканирование транспортной пирамиды: {barcodeText}");
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();
          SqlCommand command = new SqlCommand($@"select
                                                  max(PC.ID) as Value,
                                                  max(PO.MaxWeight) as MaxWeight
                                               from PyramidCompleted PC
                                               inner join PyramidOut PO on PO.ID                = PC.idPyramidOut
                                               left  join BarCode B     on B.idPyramidCompleted = PC.ID
                                               where PO.Barcode = '{barcodeText}' and IsNull(B.idTransport, 0) = 0", conn);
          
          using (SqlDataReader reader = command.ExecuteReader())
          {
            if (reader.Read())
            {
              m_idPyramidCompleted = SafeConvert.ToInt(reader["Value"]);
              m_dPyramidMaxWeight  = SafeConvert.ToDouble(reader["MaxWeight"]);
            }
            else
            {
              m_idPyramidCompleted = 0;
              m_dPyramidMaxWeight  = 0.0;
            }
          }
  
          if (m_idPyramidCompleted != 0)
          {
            command.CommandText = $"update PyramidCompleted set TimePyramid = GetDate() where ID = {m_idPyramidCompleted}";
            command.ExecuteNonQuery();
  
            command.CommandText = $@"select
                                       B2.PackNum,
                                       max(B2.PackPosNum) as PackPosNum
                                     from BarCode B1
                                       inner join BarCode B2 on B2.idPyramidCompleted = B1.idPyramidCompleted 
                                     where B1.idPyramidCompleted = {m_idPyramidCompleted}
                                     group by
                                       B2.PackNum
                                     having B2.PackNum = MAX(B1.PackNum)";
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
              m_PackNum = reader.GetInt32(reader.GetOrdinal("PackNum"));
              m_PackPosNum = reader.GetInt32(reader.GetOrdinal("PackPosNum"));
            }
            reader.Close();
          }
          else
          {
            command.CommandText = $"select ID as Value, MaxWeight from PyramidOut where BarCode = '{barcodeText}'";
            int idPyramidOut = 0;
            
            using (SqlDataReader reader = command.ExecuteReader())
            {
              if (reader.Read())
              {
                idPyramidOut        = SafeConvert.ToInt   (reader["Value"]);
                m_dPyramidMaxWeight = SafeConvert.ToDouble(reader["MaxWeight"]);
              }
            }
  
            // Добавим новую пирамиду если не нашли запись
            if (idPyramidOut == 0)
            {
              string sGUID = Guid.NewGuid().ToString();
              command.CommandText = $@"insert into PyramidOut ( BarCode , GUID,  bReturned  )
                                       output inserted.ID
                                       values                ( @BarCode, @GUID, @bReturned )";
  
              command.Parameters.AddWithValue("@BarCode", barcodeText);
              command.Parameters.AddWithValue("@GUID", sGUID);
              command.Parameters.AddWithValue("@bReturned", true);
  
              idPyramidOut = Convert.ToInt32(command.ExecuteScalar());
            }
  
            command.CommandText = $@"update PyramidOut
                                       set 
                                         bReturned = 1,  
                                         DateReturn = getdate(), 
                                         bHide = 0
                                     where ID = {idPyramidOut}";
            command.ExecuteNonQuery();
  
            command.CommandText = $@"insert into PyramidCompleted(idPyramidOut, TimePyramid)
                                       output inserted.ID
                                       select ID, GetDate() from PyramidOut where PyramidOut.Barcode = '{barcodeText}'";
            m_idPyramidCompleted = Convert.ToInt32(command.ExecuteScalar());
          }
  
          return new Tuple<bool, string, int>(true, $"Сканированная пирамида {barcodeText}", (int)ETypeBarCode.e_type_pyramid);
        }
      }
      catch
      {
        log.Error($"Ошибка при санировании пирамиды {barcodeText}");
        return new Tuple<bool, string, int>(false, $"Ошибка при санировании пирамиды {barcodeText}", (int)ETypeBarCode.e_type_pyramid);
      }
    }

    /* Тестовая серверная кнопка
    protected void ServerButton_Click(object sender, EventArgs e)
    {
      try
      {
        string result = "Нет данных";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();
          string query = "select top 1 ID from Task";

          using (SqlCommand cmd = new SqlCommand(query, conn))
          {
            object id = cmd.ExecuteScalar();
            if (id != null)
              result = $"Результат серверной кнопки: ID оператора: {id}";
          }
        }

        ResultLabel.Text = result;
        ShowMessage(result, true);
      }
      catch
      {
        ShowMessage("Ошибка", false);
      }
    }
    */

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetTaskId()
    {
      try
      {
        string result = "Нет данных";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();
          string query = "select top 1 ID from Task";

          using (SqlCommand cmd = new SqlCommand(query, conn))
          {
            object id = cmd.ExecuteScalar();
            if (id != null)
              result = $"ID оператора: {id}";
          }
        }

        // Возвращаем объект с результатами
        return new { success = true, message = result };
      }
      catch
      {
        return new { success = false, message = "Ошибка метода" };
      }
    }

    [WebMethod]

    // Добавим штрихкод СП в транспортную пирамиду
    public static Tuple<bool, string, int> WriteBarCode(string barcodeText)
    {
      try
      {
        int  idBarCode          = 0,
             idTransport        = 0,
             idPyramidCompleted = 0;
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();
          SqlCommand command = new SqlCommand($@"select   
                                                   B.ID,
                                                   IsNull(B.idPyramidCompleted, 0) as idPyramidCompleted,
                                                   IsNull(B.idTransport, 0) as idTransport, 
                                                   BS.PackNum,
                                                   Sum(P.Weight) as Weight
                                                 from BarCode B  
                                                   left join BarCode_Scan BS on BS.idBarCode = B.ID 
                                                   left join GlassDetails GD on GD.idBarCode = B.ID
                                                   left join Product      P  on GD.idGlass   = P.ID
                                                 where B.BarCode = @barcode
                                                 group by
                                                   B.ID,
                                                   IsNull(B.idPyramidCompleted, 0),
                                                   IsNull(B.idTransport, 0),
                                                   BS.PackNum", 
                                              conn);
  
          command.Parameters.Add("@barcode", SqlDbType.NVarChar).Value = barcodeText;

          SqlDataReader  reader = command.ExecuteReader();

          if ( reader.Read() )
          {
            idBarCode          = SafeConvert.ToInt(reader["ID"]);
            idTransport        = SafeConvert.ToInt(reader["idTransport"]);
            idPyramidCompleted = SafeConvert.ToInt(reader["idPyramidCompleted"]);
            m_dCurrentWeight  += SafeConvert.ToDouble(reader["Weight"]);
          }
          reader.Close();
  
          if (idBarCode == 0)
            return new Tuple<bool, string, int>(false, $"Неизвестный штрихкод: {barcodeText}", (int)ETypeBarCode.e_type_unknow);
  
          if (idTransport != 0)
            return new Tuple<bool, string, int>(false, $"Штрихкод отгружен!", (int)ETypeBarCode.e_type_unknow);
  
          if (m_idPyramidCompleted == 0)
            return new Tuple<bool, string, int>(false, "Транспортная пирамида не введена или не известна", (int)ETypeBarCode.e_type_unknow);
  
            int iState = 128;       // Статус изготовлен e_TaskState_Complite
  
            // устанавливать статус отгружен?
            if ( m_bSetDelivStatus )
              iState |= 8;          // Отгружен e_TaskState_Deliv
  
            command.CommandText = $@"update BarCode 
                                       set   
                                         nState             = IsNull(nState, 0) | {iState},
                                         idPyramidCompleted = {m_idPyramidCompleted},
                                         TimeScan   = getdate(),
                                         PackNum    = {m_PackNum},
                                         PackPosNum = {m_PackPosNum},
                                         idOperator = {m_idOperator}
                                       where
                                         ID = {idBarCode} and idTransport is NULL";
            command.ExecuteNonQuery();
  
            command.CommandText = $@"update BarCode set nState = (nState | 65536 | {iState}) ^ 65536, idPyramidCompleted = {m_idPyramidCompleted}, idOperator = {m_idOperator} where ID = {idBarCode}";
            command.ExecuteNonQuery();
  
            // Закрыть транспортную пирамиду, на которой установлен этот баркод:
            command.CommandText = $@"exec sp_CloseTripPyramidByIdBarCode {idBarCode}";
            command.ExecuteNonQuery();
  
            // СП переставили
            if ( m_idPyramidCompleted != idPyramidCompleted )
            {
              command.CommandText = $@"delete from ScanLog_BarCode
                                     where idBarCode = {idBarCode} and idPyramidCompleted = {idPyramidCompleted}";
              command.ExecuteNonQuery();
            }
  
          if ( m_idPyramidCompleted != 0 )
          {
            // Добавить в таблицу
            command.CommandText = $@"insert into ScanLog_BarCode(TimeScan   ,  idBarCode   , idOperator,   idPyramidCompleted)
                                   select                      getdate()  , {idBarCode}  , {m_idOperator}, {m_idPyramidCompleted}
                                   where not exists (select 1 from ScanLog_BarCode where idBarCode = {idBarCode})";
            command.ExecuteNonQuery();
  
              command.CommandText = $"select idTask from v_ScanLog_BarCode where idBarCode = {idBarCode}";
              int idTask =  SafeConvert.ToInt(command.ExecuteScalar());
  
              // установливаем статус заказу
              if ( m_bUseTaskToManuf )
                command.CommandText = $"exec sp_UpdateManufTaskState {idTask} ";
              else
                command.CommandText = $"exec sp_UpdateTaskState {idTask}";
  
              command.ExecuteNonQuery();
  
              // Устанавливаем статус готовности
  
              if (m_sector.ID != 0)
              {
                OperatorInfo      operatorInfo = OperatorInfo.CheckOperatorInfoByPlanCalendar(_conn, m_idOperator).Item1;
                GlassDetailsOper  oper         = new GlassDetailsOper(operatorInfo, m_sector, "", idBarCode);
  
                GlassProcessingController glassProcessingController = new GlassProcessingController(_conn);
                glassProcessingController.MakeOperSP(oper, true);
              }
          }
  
            return m_idPyramidCompleted != idPyramidCompleted
            ? new Tuple<bool, string, int>(true, $"Команда [Перм.Скан.СП]: {barcodeText}", (int)ETypeBarCode.e_type_pyramid)
            : new Tuple<bool, string, int>(true, $"Команда [Скан.СП]: {barcodeText}", (int)ETypeBarCode.e_type_pyramid);
        }
      }
      catch (Exception ex)
      {
        log.Error(ex.Message);
        return new Tuple<bool, string, int>(false, ex.Message, (int)ETypeBarCode.e_type_unknow);
      }
    }

    [WebMethod]
    public static Tuple<bool, string, int>WriteBarCodeShip(string barcodeText)
    {
      try
      {
        log.Info($"Команда [Скан.Отгрузки]: {barcodeText}");

        if (m_nBarCodeSHLeng > 0 && barcodeText.Length != m_nBarCodeSHLeng)
        {
          log.Error($"Команда [Скан.Отгрузки] неизвестный штрихкод: {barcodeText}");
          return new Tuple<bool, string, int>(false, $"Команда [Скан.Отгрузки] неизвестный штрихкод: {barcodeText}", (int)ETypeBarCode.e_type_ship);
        }

        Tuple<bool, string, int> response = AddShipment(barcodeText);

        if (response.Item1)
        {
          // Обновляем отображение истории сканирования (таблицу/грид)
          //if (m_pPyramid)
          //  m_pPyramid->ReloadGrid();
        }
          
        return response;

      }
      catch (Exception ex)
      {
         return new Tuple<bool, string, int>(false, ex.Message, (int)ETypeBarCode.e_type_unknow);
      }
    }

    // Сканирование отгрузки.
    public static Tuple<bool, string, int> AddShipment(string barcode)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();
          
          string sShipID = barcode.ToLower().Replace(m_sBarCodePrefixShip, "");
          long idShip = SafeConvert.ToInt(sShipID);

          if (idShip <= 0)
            return new Tuple<bool, string, int>(false, $"Неверный номер отгрузки: {barcode}", (int)ETypeBarCode.e_type_ship);

          SqlCommand command = new SqlCommand($"select ID, Num, IsNull(bLock, 0) as bLock, DateScan from Ship where ID = {idShip}", _conn);


          bool       bLock     = false;
          DateTime ? vDateScan = new DateTime(); 
          string     sShipNum;


          using (SqlDataReader reader = command.ExecuteReader())
          {
            ListItem listItem = new ListItem();

            if (reader.Read())
            {
             bLock     = SafeConvert.ToBool(reader["bLock"]);
             vDateScan = SafeConvert.ToDateTime(reader["DateScan"], DateTime.MinValue);
             sShipNum  = SafeConvert.ToString(reader["Num"]);
            }
            else
            {
              log.Error($"Отгрузка с номером '{idShip}' не найдена в базе данных.");
              return new Tuple<bool, string, int>(false, $"Отгрузка с номером '{idShip}' не найдена в базе данных.", (int)ETypeBarCode.e_type_ship);
            }
          }

          if ( bLock )
          {
            log.Error($"ОШИБКА: Отгрузка (№{sShipNum}: {idShip}) уже была подтверждена.");
            return new Tuple<bool, string, int>(false, $"ОШИБКА: Отгрузка №%s ({sShipNum}: {idShip}) уже была подтверждена.", (int)ETypeBarCode.e_type_ship);
          }

          DateTime dNow = DateTime.Now;

          command.CommandText = $"update Ship set DateScan = '{dNow:yyyy-MM-ddTHH:mm:ss}', ShipBarCode = '{barcode}', bLock = 1 where ID = {idShip}";
          command.ExecuteNonQuery();

          command.CommandText = $"exec sp_SetNextNumCalcFactForTransportTaskInShip {idShip}";
          command.ExecuteNonQuery();
        }

        return new Tuple<bool, string, int>(true, $"Комманда скан отгрузки прошла успешно", (int)ETypeBarCode.e_type_ship);
      }
      catch
      {
         return new Tuple<bool, string, int>(false, $"ОШИБКА", (int)ETypeBarCode.e_type_ship);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString);
        Session["DbConnection"] = _conn;
        _conn.Open();

        DBConfig setting = LoadDBSettings();
        m_sBarCodePrefixPyramid  = setting.sBarCodePrefixPyramid;
        m_sBarCodePrefixOperator = setting.sBarCodePrefixOperator;
        m_sBarCodePrefixShip     = setting.sBarCodePrefixShip;
        m_bUseTaskToManuf        = setting.bUseTaskToManuf;
        m_bSetDelivStatus        = setting.bSetDelivStatus;
        m_nBarCodeSHLeng         = setting.nBarCodeSHLeng;

        SqlCommand rcDepName = new SqlCommand("select * from DepName where nType = 2 order by Name", _conn);

        using (SqlDataReader reader = rcDepName.ExecuteReader())
        {
          ListItem listItem = new ListItem();

          while ( reader.Read() )
          {
            listItem = new ListItem();

            listItem.Text  = reader["Name"].ToString();
            listItem.Value = reader["ID"].ToString();
          }
        }

        // Привязки нужно сделать в отдельном подключении, чтобы лишний раз не открывать, закрывать
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();
          // Инициализируем операторов с ключем как idSheduleOperator
          LoadOperatorList(Operator);

          // Узнаем id участка упаковка
          SectorManufactController sectorManufactController = new SectorManufactController(conn, 16);
          m_sector = sectorManufactController.GetSectorManufactInfo.Count == 0
                   ? new SectorManufactInfo(0, 0, "")                   // Пустой участок если нет элементов
                   : sectorManufactController.GetSectorManufactInfo[0];
        }

      }

      // Устанавливаем оператора
      if ( m_idOperator != 0 )
        Operator.SelectedValue = m_idOperator.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetScanLogs()
    {
      if (m_idPyramidCompleted == 0)
        return "[]";

      string commandText = $"select * from v_ScanLog_BarCode where idPyramidCompleted = {m_idPyramidCompleted} order by TimeScan desc";
      SqlCommand command = new SqlCommand(commandText, _conn);

      DataTable table = new DataTable();
      using (SqlDataReader reader = command.ExecuteReader())
        table.Load(reader);

      m_dCurrentWeight = 0.0; // обнуляем текущую массу

      foreach (DataRow row in table.Rows)
        m_dCurrentWeight += SafeConvert.ToDouble(row["weight"]);

      return JsonConvert.SerializeObject(table);
    }    

    public static string LoadDBPrefix()
    {
      string prefix = HttpContext.Current.Application["BarCodePrefixPyramid"] as string;
      if (string.IsNullOrEmpty(prefix))
      { 
        SqlCommand command = new SqlCommand("select d_string from Config where Name = 'BarCodePrefixPyramid'", _conn);
        HttpContext.Current.Application["BarCodePrefixPyramid"] = prefix = command.ExecuteScalar().ToString().ToLower();
      }
      return prefix;
    }

    protected void Operator_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void ReconectConnection()
    {
      if (_conn != null)
      {
        _conn.Close();
        _conn.Dispose();
      }

      _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString);
      _conn.Open();
    }

  }
}

