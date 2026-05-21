using AjaxControlToolkit;
using log4net;
using NoPaper.Helpers;
using NoPaper.Models;
using NoPaper.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utils;

namespace NoPaper.Controllers
{
  internal class BarCodeController : SPIDInfo
  {
    SqlConnection            _conn;
    static BarCodeInfo       _barCodeInfo;
    static BarCodeGlassInfo  _barCodeGlassInfo;
    CombinationReject        _CR;                      // Комбинкация брака
    int?                     _idBarCode   = null;      // Текущий id баркода
    bool                     _isAlredyFillProcessing,  // Флаг для исключения бесконечной рекурсии, при пересоздании GlassProcessing?
                             _isAlredyParking;         // Флаг для исключения бесконечной рекурсии, при выполнении PiramidParking?

    public BarCodeInfo      GetBarCodeInfo      => _barCodeInfo;
    public BarCodeGlassInfo GetBarCodeGlassInfo => _barCodeGlassInfo;

    private static readonly ILog log = LogManager.GetLogger(typeof(BarCodeController));

    public BarCodeController(SqlConnection conn)
    {
      _conn = conn;
      _conn.Open();
      _isAlredyFillProcessing = false;
      _isAlredyParking        = false;
    }

    public BarCodeController(SqlConnection conn, string barCode) : this(conn)
    {
      SqlCommand    command = new SqlCommand($"select ID from BarCode where BarCode = '{barCode}'", _conn);
      SqlDataReader reader  = command.ExecuteReader();

      InitializeSPID(_conn);

      // если прочитали то создаем объект баркода
      if (reader.Read())
      {
        int ID       = Convert.ToInt32(reader["ID"].ToString());
        _barCodeInfo = new BarCodeInfo(ID, barCode);
        _idBarCode   = ID;  // Записываем id текущего баркода
        reader.Close();
      }
      else // Иначе проверим на баркод стелкла
      {
        command.CommandText = $"select * from BarCode_Glass where BarCode = '{barCode}'";
        reader.Close();
        reader = command.ExecuteReader();

        if (reader.Read())
        {
          int ID             = Convert.ToInt32(reader["ID"           ].ToString());
          int idProjectItem  = Convert.ToInt32(reader["idProjectItem"].ToString());
          int idBarCode      = Convert.ToInt32(reader["idBarCode"    ].ToString());
          _idBarCode         = idBarCode;  // Записываем id текущего баркода

          _barCodeGlassInfo   = new BarCodeGlassInfo(ID, barCode, idBarCode, idProjectItem);
        }
        reader.Close();
      }
    }

    public static BarCodeInfo CreateBarCodeInfo(SqlConnection conn, string barCode)
    {
      conn.Open();
      SqlCommand    command = new SqlCommand($"select ID from BarCode where BarCode = '{barCode}'", conn);
      object        result  = command.ExecuteScalar();

      // если прочитали то создаем объект баркода
      if (result != null)
      {
        int ID = Convert.ToInt32(result);
        return new BarCodeInfo(ID, barCode);
      }

      return new BarCodeInfo();
    }

    //public static BarCodeGlassInfo CreateBarCodeGlassInfo(SqlConnection conn, string barcode)
    //{
    //  try
    //  {
    //    string commandText = $"select * from BarCode_Glass where BarCode = '{barCode}'";

    //    using (SqlCommand command = new SqlCommand(commandText, conn))
    //    {
    //      using (SqlDataReader reader = command.ExecuteReader())
    //      {
    //        if (reader.Read())
    //        {
    //          int ID             = Convert.ToInt32(reader["ID"           ].ToString());
    //          int idProjectItem  = Convert.ToInt32(reader["idProjectItem"].ToString());
    //          int idBarCode      = Convert.ToInt32(reader["idBarCode"    ].ToString());
    //          _idBarCode = idBarCode;  // Записываем id текущего баркода

    //          _barCodeGlassInfo = new BarCodeGlassInfo(ID, barCode, idBarCode, idProjectItem);
    //        }
    //      }
    //    }
    //  }
    //  catch
    //  {

    //  }
    //}

    public BarCodeController(SqlConnection conn, string barCode, CombinationReject CR) : this(conn, barCode)
    {
      _CR = CR;
    }

    // Проверка на то что данный баркод существует
    public bool IsInitializeBarCode()
    {
      if (_barCodeInfo != null)
        return _barCodeInfo.IsInitialized;
      else
        return false;
    }

    // Проверка что данный баркод стекла существует
    public bool IsInitializeBarCodeGlass()
    {
      if (_barCodeGlassInfo != null)
        return _barCodeGlassInfo.IsInitialized;
      else 
        return false;
    }

    /// <summary>
    ///   Потоковая функция, создает заказ на переделку брака для переданного штрихкода
    /// </summary>
    /// <param name="connectionString">Передаем строку подключения для открытия паралельного подключения</param>
    /// <param name="SPID">текущий SPID</param>
    /// <param name="barCodeInfo">информация о штрихкоде СП</param>
    /// <param name="barCodeGlassInfo">информация о штриходе стекла</param>
    /// <param name="operatorInfo">Информация о операторе</param>
    /// <returns>Поток создания заказа на переделку</returns>
    public async Task SetDefectByBarCode(string connectionString, int SPID, BarCodeInfo barCodeInfo, BarCodeGlassInfo barCodeGlassInfo, OperatorInfo operatorInfo, int idOperator_Guilty = 0)
    {
      using (SqlConnection asyncConnection = new SqlConnection(connectionString))
      {
        await asyncConnection.OpenAsync();

        // Команда для присвоения брака
        string defectCommand = barCodeInfo != null
                             ? $"update BarCode       set nState = IsNull(nState, 0) | 512, idGuiltyOperator = @idGuiltyOperator where lower(BarCode) = @barcode"
                             : $"update BarCode_Glass set nState = IsNull(nState, 0) | 512 where ID = @ID ";

        // Присваиваем признак брака
        using (SqlCommand command = new SqlCommand(defectCommand, asyncConnection))
        {
          if ( barCodeInfo != null )
          {
            command.Parameters.Add("@BarCode", SqlDbType.VarChar).Value = barCodeInfo.barCode.ToLower();
            // idGuiltyOperator: если 0 → NULL
            command.Parameters.Add("@idGuiltyOperator", SqlDbType.Int).Value = idOperator_Guilty != 0
                                                                             ? (object)idOperator_Guilty
                                                                             : DBNull.Value;
          }
          else
            command.Parameters.Add("@ID", SqlDbType.Int).Value = barCodeGlassInfo.ID;

          await command.ExecuteNonQueryAsync();

          // Очищаем стор
          await ClearTempStore(asyncConnection, SPID, 3);
          await ClearTempStore(asyncConnection, SPID, 100);

          // Записываем в стор брак
          command.CommandText = $@"insert into IDTempStore (ID, nType, SPID)
                                   select B.ID, 3, {SPID} 
                                   from BarCode B 
                                   where B.ID in ({_idBarCode})";
          await command.ExecuteNonQueryAsync();

          command.CommandText = $"exec sp_RemakeTaskByBarCodes {SPID}, 0, 0, 0,";
          command.CommandText += barCodeInfo != null
                                          ? $"'', {barCodeInfo.ID}"
                                          : $"'{barCodeGlassInfo.idProjectItem}'";
          await command.ExecuteNonQueryAsync();

          // Вытягиваем id созданного Task
          command.CommandText = $"select ID from IDTempStore where nType = 100 and SPID = {SPID}";
          object idTaskResult = await command.ExecuteScalarAsync();

          if (idTaskResult == null)
            return;

          int idTask = Convert.ToInt32(idTaskResult);

          // Присвоение штрихкодов стекл
          command.CommandText = $"exec sp_FillBarCode_Glass {idTask}, {SPID}";
          await command.ExecuteNonQueryAsync();

          // Присвоение менеджера заказа
          if (operatorInfo.idUser != 0)
          {
            command.CommandText = $"update Task set idUsers = {operatorInfo.idUser} where ID = {idTask}";
            await command.ExecuteNonQueryAsync();
          }
        }
      }
    }

    /// <summary>
    /// Статический метод для проверки отсканирован текущий штрихкод или нет,
    /// Если участок сборка проверяется по idBarCode
    /// Иначе проверяем по idGlassDetails
    /// </summary>
    /// <param name="glassDetailsOper"></param>
    /// <returns></returns>
    private static bool CheckIsAlredyScanDetail(GlassDetailsOper glassDetailsOper)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
        {
          conn.Open();
          // Чтобы эта проверка работала успешно необходимо убедиться что в справочнике операторов сканирования, операторы назначены на участок иначе сканирование может быть повторным
          string commandText = $@"select Min(IsNull(idSheduleOperator, 0)) as idSheduleOperator from GlassProcessing
                              inner join GlassDetails on GlassProcessing.idGlassDetails = GlassDetails.ID 
                              where idSectorManufact = {glassDetailsOper.sectorManufact.ID} ";

          if (glassDetailsOper.sectorManufact.nType == 2 && _barCodeInfo != null) // Сборка?
            commandText += $" and idBarCode = {glassDetailsOper.idBarCode}";
          else
            commandText += $@" and idGlassDetails = {glassDetailsOper.idGlassDetails}";

          using (SqlCommand command = new SqlCommand(commandText, conn))
          {
            object result = command.ExecuteScalar();
            int    idOper = SafeConvert.ToInt(result);


            // Если есть результат и он не равен 0 тогда true иначе false
            return idOper != 0;
          }
        }
      }
      catch (Exception ex)
      {
        log.Error(ex.Message);
        return false;
      }
    }

    private bool CheckIsAlredyScan(GlassDetailsOper glassDetailsOper)
    {
      bool isAlredyScan = false;

      // Чтобы эта проверка работала успешно необходимо убедиться что в справочнике операторов сканирования, операторы назначены на участок иначе сканирование может быть повторным
      string commandText = $@"select Min(IsNull(idSheduleOperator, 0)) as idSheduleOperator from GlassProcessing
                              inner join GlassDetails on GlassProcessing.idGlassDetails = GlassDetails.ID 
                              where idSectorManufact = {glassDetailsOper.sectorManufact.ID} ";

      if (glassDetailsOper.sectorManufact.nType == 2 && _barCodeInfo != null) // Сборка?
        commandText += $" and idBarCode = {glassDetailsOper.idBarCode}";
      else
        commandText += $@" and idGlassDetails = {glassDetailsOper.idGlassDetails}";

      using (SqlCommand command = new SqlCommand(commandText, _conn))
      {
        object result = command.ExecuteScalar();

        // Если есть результат и он не равен 0 тогда true иначе false
        if (result != DBNull.Value)
          isAlredyScan = Convert.ToInt32(result) != 0 ? true : false;
      }

      return isAlredyScan;
    }

    // Отчистка стора
    public async Task ClearTempStore(SqlConnection conn, int SPID, int nType)
    {
      using (SqlCommand command = new SqlCommand($"delete from IDTempStore where SPID = {SPID} and nType = {nType}", conn)) 
        await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Сканирование штрихкода вызов в параллельном потоке из сервенных методов
    /// </summary>
    /// <param name="barcode">Штрихкод</param>
    /// <param name="sector">Участок на котором нужно отсканировать штрихкод</param>
    /// <param name="idSheduleOperator">Оператор сканирования</param>
    /// <param name="idPyramidBarcode">ID Пирамиды на котором расположен штрихкод</param>
    /// <param name="bFillGlassProcessingInPyramid">Требуется повторно провести заполнение idGlassProcessing</param>
    /// <param name="bNeedParking">Требуется повторно провести перепарковку idGlassProcessingPyramid</param>
    /// <returns>Объект с обработанными данными</returns>
    public static ScanResponse<IScanResult> PostBarCodeGlass(string barcode, SectorManufactInfo sector, OperatorInfo operatorInfo, int idPyramidBarcode, bool bFillGlassProcessingInPyramid = true, bool bNeedParking = true)
    {
      try
      {
        log.Info($"Сканирование штрихкода: {barcode} на участке: {sector.Name} оператором: {operatorInfo.Name} с idSheuleOperator: {operatorInfo.idSheduleOperator}");
        PostGlassProcessing glassProcessing = new PostGlassProcessing(); // Объект где храним все данные

        using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
        {
          conn.Open();

          // Считываем данные
          SqlCommand command = new SqlCommand(SawGlassProcessingData.GetPostDataBarcode(barcode, sector.ID), conn);
          using (SqlDataReader reader = command.ExecuteReader())
          {
            if (reader.Read())
            {

              int idPyramid                = SafeConvert.ToInt(reader["idPiramid"]),
                  idGlassDetails           = SafeConvert.ToInt(reader["idGlassDetails"]),
                  idSawTaskMain            = SafeConvert.ToInt(reader["idSawTaskMain"]),
                  idBarCode                = SafeConvert.ToInt(reader["idBarCode"]),
                  idGlassProcessing        = SafeConvert.ToInt(reader["idGlassProcessing"]),
                  idGlassProcessingPyramid = SafeConvert.ToInt(reader["idGlassProcessingPyramid"]);

              glassProcessing = new PostGlassProcessing(idGlassDetails, idPyramid, idSawTaskMain, idGlassProcessing, idGlassProcessingPyramid, idBarCode, barcode);
            }
          }

          // Записей нет, Штрих-код не найден
          if (glassProcessing.IDBarCode == 0)
          {
            // Еще не пробывали заполнить GlassProcessing?
            if (bFillGlassProcessingInPyramid)
            {
              bFillGlassProcessingInPyramid = false;

              GlassProcessingController glassProcessingController = new GlassProcessingController(conn);

              //  Формируем фильтр для переприсвоения idGlassProcessingPyramid для текущей пирамиды
              string sAddFilter = $"and GD.idPiramid = {glassProcessing.IDPyramid} and GP.idSectorManufact = {sector.ID}";

              glassProcessingController.TryFillGlassProcessingOnPyramid(glassProcessing.IDSawTaskMain, sAddFilter);

              // Повторно пробуем записать штрих-код
              return PostBarCodeGlass(barcode, sector, operatorInfo, idPyramidBarcode, bFillGlassProcessingInPyramid, bNeedParking);
            }

            return ScanResponse<IScanResult>.Fail(new BarcodeScanResult(glassProcessing.Barcode, glassProcessing.IDGlassProcessing, idPyramidBarcode, sector.ID, glassProcessing.IDPyramid), "Штрихкод не найден");
          }

          // Если значение не задано, не сможем сделать ничего, попробуем переприсвоить
          if (glassProcessing.IDGlassProcessing == 0)
          {
            // Еще не пробывали заполнить GlassProcessing?
            if (bFillGlassProcessingInPyramid)
            {
              bFillGlassProcessingInPyramid = false;

              if (glassProcessing.IDSawTaskMain != 0)
              {
                GlassProcessingController glassProcessingController = new GlassProcessingController(conn);

                //  Формируем фильтр для переприсвоения idGlassProcessingPyramid для текущей пирамиды
                string sAddFilter = $"and GD.idPiramid = {glassProcessing.IDPyramid} and GP.idSectorManufact = {sector.ID}";

                glassProcessingController.TryFillGlassProcessingOnPyramid(glassProcessing.IDSawTaskMain, sAddFilter);

                // Повторно пробуем записать штрих-код
                return PostBarCodeGlass(barcode, sector, operatorInfo, idPyramidBarcode, bFillGlassProcessingInPyramid, bNeedParking);
              }
            }
          }

          // Если здесь пусто значит все остальные значения есть, пробуем выполнить команду команду расчет времени обработки
          if (glassProcessing.IDGlassProcessingPyramid == 0)
          {
            if (bNeedParking)
            {
              bNeedParking  = false;

              //  Формируем фильтр для переприсвоения idGlassProcessingPyramid для текущей пирамиды
              string sAddFilter = $"and GD.idPiramid = {glassProcessing.IDPyramid} and GP.idSectorManufact = {sector.ID}";

              GlassProcessingController glassProcessingController = new GlassProcessingController(conn);
              glassProcessingController.PiramidParking(glassProcessing.IDSawTaskMain, sAddFilter);

              // Повторно пробуем отсканировать штрих-код
              return PostBarCodeGlass(barcode, sector, operatorInfo, idPyramidBarcode, bFillGlassProcessingInPyramid, bNeedParking);              
            }

            return ScanResponse<IScanResult>.Fail(new BarcodeScanResult(barcode, glassProcessing.IDGlassProcessing, idPyramidBarcode, sector.ID, glassProcessing.IDPyramid), "Штрихкод найден, но не возможно назначить пирамиду! Отсутствует idPyramid");
          }

          GlassDetailsOper glassDetailsOper = new GlassDetailsOper(operatorInfo, sector, glassProcessing.IDGlassDetails, glassProcessing.IDGlassProcessingPyramid.ToString(), glassProcessing.IDBarCode);

          if (glassDetailsOper.IsInitialized)
          {
            // Проверяем возможно штрихкод уже отсканирован другим оператором
            if (CheckIsAlredyScanDetail(glassDetailsOper))
              return ScanResponse<IScanResult>.Fail(new BarcodeScanResult(barcode, glassProcessing.IDGlassProcessing, idPyramidBarcode, sector.ID, glassProcessing.IDPyramid), $"Штрих код {barcode} уже отсканирован!");

            GlassProcessingController glassProcessingController = new GlassProcessingController(conn);
            if (glassDetailsOper.sectorManufact.nType == 2 && _barCodeInfo != null)
            {
              glassProcessingController.MakeOperSP(glassDetailsOper, true, idPyramidBarcode);
              SetStateBarCode(_barCodeInfo.ID);
              return ScanResponse<IScanResult>.Ok(new BarcodeScanResult(barcode, glassProcessing.IDGlassProcessing, idPyramidBarcode, sector.ID, glassProcessing.IDPyramid), $"Штрих код СП {barcode} прочитан");
            }
            else
            {
              // Проверим успешность выполнения операции
              if (glassProcessingController.MakeOper(glassDetailsOper, idPyramidBarcode))
                return ScanResponse<IScanResult>.Ok(new BarcodeScanResult(barcode, glassProcessing.IDGlassProcessing, idPyramidBarcode, sector.ID, glassProcessing.IDPyramid), $"Штрих код стекла {barcode} прочитан");
              else
                return ScanResponse<IScanResult>.Fail(new BarcodeScanResult(barcode, glassProcessing.IDGlassProcessing, idPyramidBarcode, sector.ID, glassProcessing.IDPyramid), $"Не удалось считать штрих код отсутствует штрихкод пирамиды или нет значения idGlassProcessing");
            }
          }
        }

        return ScanResponse<IScanResult>.Fail(new BarcodeScanResult(barcode, glassProcessing.IDGlassProcessing, idPyramidBarcode, sector.ID, glassProcessing.IDPyramid), $"Невозможно прочитать штрихкод {barcode}");
      }
      catch
      {
        return ScanResponse<IScanResult>.Fail(new BarcodeScanResult(barcode, 0, idPyramidBarcode, sector.ID, 0), $"Не известная ошибка при сканировании штрихкода {barcode}");
      }
    }

    // Для сканирования
    public static MessageModel WriteBarCodeGlass(SqlConnection conn, BarCodeInfo barCodeInfo, string barCodeText, int idSector, int idOperator, int idScanPyramid)
    {
      string commandText = barCodeInfo.IsInitialized
                         ? SawGlassProcessingData.GetDataBarCode(barCodeText, idSector)
                         : SawGlassProcessingData.GetDataBarCode_Glass(barCodeText, idSector);

      SqlCommand sqlCommand = new SqlCommand(commandText, conn);
      GlassDetailsOper glassDetailsOper;
      OperatorInfo     operatorInfo = OperatorInfo.CheckOperatorInfoByPlanCalendar(conn, idOperator).Item1;

      using (SqlDataReader reader = sqlCommand.ExecuteReader())
      {
        if (!reader.Read())
          return new MessageModel(false, "Операция не возможна, не известный штрих код"); // Выводим сообщение о том что бар кода нет

        int    idGlassDetails            = Convert.ToInt32(reader["idGlassDetails"].ToString()),
               idBarCode                 = barCodeInfo.IsInitialized ? barCodeInfo.ID : 0;
        string sIdGlassProcessingPyramid = reader["idGlassProcessingPyramid"].ToString();

        glassDetailsOper = new GlassDetailsOper(operatorInfo, new SectorManufactInfo(idSector, 0, ""), idGlassDetails, sIdGlassProcessingPyramid, idBarCode);
      }
      if (glassDetailsOper.IsInitialized)
      {
        GlassProcessingController glassProcessingController = new GlassProcessingController(conn);

        if (glassDetailsOper.sectorManufact.nType == 2 && barCodeInfo.IsInitialized)
        {
          glassProcessingController.MakeOperSP(glassDetailsOper, true, idScanPyramid);
          SqlCommand command = new SqlCommand($@"update BarCode set
                                               TimeScan = GetDate(),
                                               nState   = nState | 128
                                             where ID = {barCodeInfo.ID}",
                                            conn);

          command.ExecuteNonQuery();
        }
        else
          glassProcessingController.MakeOper(glassDetailsOper, idScanPyramid);

        return new MessageModel(true, $"Штрих код {barCodeText} успешно отсканирован");
      }

      return new MessageModel(false, $"Не известная ошибка");
    }

    public static void SetStateBarCode(int idBarCode)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
        {
          conn.Open();
          SqlCommand command = new SqlCommand($@"update BarCode set
                                                   TimeScan = GetDate(),
                                                   nState   = nState | 128
                                                 where ID = {idBarCode}",
                                              conn);

          command.ExecuteNonQuery();
        }
      }
      catch
      {
      }
    }

    public async Task CheckWriteCombination()
    {
      int idCombinationReject; // id Комбинации Брака

      SqlCommand command = new SqlCommand($@"select top 1
                                               ID
                                             from CombinationReject
                                             where
                                              idRejectAct   = {_CR.IDRejectAct} and 
                                              idRejectPlace = {_CR.IDRejectPlace} and 
                                              idRejectType  = {_CR.IDRejectType} and
                                              idReject      = {_CR.IDReject} and
                                              idTypeExpense = {_CR.IDTypeExpense} and
                                              CommentReject = @CommentReject",
                                              _conn);

      command.Parameters.AddWithValue("@CommentReject", _CR.CommentReject);

      SqlDataReader reader = await command.ExecuteReaderAsync();
      // Ищем данную комбинацию брака
      if (reader.Read()) // Если найдена
      {
        idCombinationReject = reader.GetInt32(0); // Получаем ID комбинации
        reader.Close();
      }
      else
      {
        // Если нет добавим
        reader.Close();
        command.CommandText = ($@"insert into CombinationReject(idRejectType,             idReject,       idRejectPlace,       idRejectAct,       idTypeExpense,  CommentReject)
                                                        values ({_CR.IDRejectType}, {_CR.IDReject}, {_CR.IDRejectPlace}, {_CR.IDRejectAct}, {_CR.IDTypeExpense}, @CommentReject)
                                  select Scope_Identity()"); // Получение ID новой записи
        // Выполним insert и получаем ID новой комбинации
        idCombinationReject = Convert.ToInt32(await command.ExecuteScalarAsync()); 
      }

      command.CommandText = ($@"update BarCode
                                set  
                                  idCombinationReject         = {idCombinationReject}   ,
                                  DateSetCombinationReject    = Convert(Date, GetDate()),
                                  idUser_SetCombinationReject = 2 
                                where 
                                   ID = {_idBarCode}"); // [TODO?] В качестве пользователя будем писать пока sa
      await command.ExecuteNonQueryAsync();
    }

    public bool IsInitializeCombination()
    {
      return _CR != null;
    }

    public RemakeModel GetInfoByBarCodeReject(string sIdBarCode_reject)
    {
      RemakeModel result;
      SqlCommand command = new SqlCommand($@"select top 1
                                               T.AccountNum,
                                               T.Date   as TaskDate,
                                               STM.Name as SawTaskName,
                                               STM.Data as SawTaskDate,
                                               P.GPName,
                                               P.nCount,
                                               P.Area,
                                               P.Width,
                                               P.Height,
                                               GP.bFinished,
                                               SM.ID,
                                               SM.Name as SectorName
                                             from BarCode B
                                             inner join Project        P   on P.ID   = B.idProject
                                             inner join Task           T   on T.ID   = P.idTask
                                             left join GlassDetails    GD  on B.ID   = GD.idBarCode
                                             left join GlassProcessing GP  on GD.ID  = GP.idGlassDetails
                                             left join SectorManufact  SM  on SM.ID  = GP.idSectorManufact
                                             left join SawTaskMain     STM on STM.ID = GD.idSawTaskMain
                                             where idBarCode_Reject = {sIdBarCode_reject} and IsNull(GP.bFinished, 0) = 0
                                             order by nOrderOper", _conn);

      using (SqlDataReader reader = command.ExecuteReader())
      {
        if (reader.Read())
        {
          string    accountName = reader["AccountNum"].ToString();
          DateTime? taskDate    = reader.IsDBNull(reader.GetOrdinal("TaskDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("TaskDate"));
          string    sawTaskName = reader["SawTaskName"].ToString();
          DateTime? sawTaskDate = reader.IsDBNull(reader.GetOrdinal("SawTaskDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SawTaskDate"));
          string    sectorName  = reader["SectorName"].ToString(),
                    gpName      = reader["GPName"    ].ToString();
          int       nCount      = reader .GetInt32(reader.GetOrdinal("nCount")),
                    width       = Convert.ToInt32 (reader["Width" ]),
                    height      = Convert.ToInt32 (reader["Height"]);
          double    area        = Convert.ToDouble(reader["Area"  ]);

          result = new RemakeModel(accountName, taskDate, sawTaskName, sawTaskDate, sectorName, gpName, nCount, width, height, area);
        }
        else
          result = new RemakeModel();
      }

      return result;
    }
  }
}
