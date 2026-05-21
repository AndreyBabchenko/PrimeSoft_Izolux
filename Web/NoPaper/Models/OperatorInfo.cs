using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using Utils;

namespace NoPaper.Models
{
  [Serializable]
  public class OperatorInfo
  {
    public int    ID    { get; private set; }
    public int?   idSectorManufact { get; private set; }
    public int    idSheduleOperator { get; set; }
    public int    idUser            { get; set; }
    public string Name  { get; private set; }

    private static readonly ILog log = LogManager.GetLogger(typeof(OperatorInfo));

    public OperatorInfo()
    {
      ID                = 0;
      idSectorManufact  = 0;
      idSheduleOperator = 0;
      idUser            = 0;
      Name              = "";
    }

    public OperatorInfo(int _ID) : base()
    {
      ID = _ID;
    }

    public OperatorInfo(int _ID, int _idSheduleOperator, int _idUser, int? _idSectorManufact, string _Name)
    {
      ID = _ID;
      idSheduleOperator = _idSheduleOperator;
      idSectorManufact = _idSectorManufact;
      idUser            = _idUser;
      Name = _Name;
    }

    /// <summary>
    /// Используем для создание нового записи в таблице SheduleOperator
    /// </summary>
    /// <param name="idOperator">id оператора</param>
    /// <returns>Возвращает новое значение idSheduleOperator</returns>
    public static int CreateSheduleOperator(SqlConnection conn, int idOperator)
    {
      try
      {
        log.Info($"Создание нового idSheduleOperator для оператора ID: {idOperator}");
        // Проверим может уже есть
        Tuple<OperatorInfo, bool> operatorInf = CheckOperatorInfoByPlanCalendar(conn, idOperator);

        if (operatorInf.Item1.idSheduleOperator != 0)
        {
          log.Info($"idSheuleOperator уже существует, idSheduleOperator: {operatorInf.Item1.idSheduleOperator}");
          return operatorInf.Item1.idSheduleOperator;
        }

        using (SqlCommand command = new SqlCommand("sp_CreateSheduleOperator", conn))
        {
          command.CommandType = CommandType.StoredProcedure;

          // входные параметры
          command.Parameters.AddWithValue("@idOperator", idOperator);
          command.Parameters.AddWithValue("@date", DateTime.Now);

          SqlParameter outParam = new SqlParameter("@idSheduleOperatorNew", SqlDbType.Int);
          outParam.Direction = ParameterDirection.Output;
          command.Parameters.Add(outParam);

          command.ExecuteNonQuery();

          log.Info($"Создан новый idSheduleOperator: {SafeConvert.ToInt(outParam.Value)}");
          return SafeConvert.ToInt(outParam.Value);
        }
      }
      catch (Exception ex)
      {
        log.Error(ex.Message);
        return 0;
      }
    }

    // Посмотрим существует ли введеный оператор в таблице SheduleOperator
    public static Tuple<OperatorInfo, bool> CheckOperatorInfoByPlanCalendar(SqlConnection conn, int idOperator)
    {
      log.Info($"Проверка наличия оператора в текущем календаре планирования, idOperator: {idOperator}");
      try
      {
        string commandText = $@"select 
                                 O.ID, 
                                 O.Name,
                                 O.idSectorManufact,
                                 IsNull(SO.ID,    0) as idSheduleOperator,
                                 IsNull(O.idUser, 0) as idUser
                               from Operator O
                               outer apply  
                               (
                                select top 1 ID
                                from SheduleOperator SO
                                where SO.idOperator = O.ID    and 
                                      SO.dtBegin <= getdate() and
                                      SO.dtEnd   >= getdate()
                               ) SO
                               where O.ID = {idOperator}
                               order by Name";

        using (SqlCommand command = new SqlCommand(commandText, conn))
          using (SqlDataReader reader = command.ExecuteReader())
            if (reader.Read())
            {
              OperatorInfo operatorInfo = new OperatorInfo
                                        (
                                          _ID:                Convert.ToInt32(reader["ID"]),
                                          _idSheduleOperator: Convert.ToInt32(reader["idSheduleOperator"]),
                                          _idUser:            Convert.ToInt32(reader["idUser"]),
                                          _idSectorManufact:  reader["idSectorManufact"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["idSectorManufact"]),
                                          _Name:              reader["Name"].ToString()
                                        );


              log.Info($"оператор найден idOperator: {operatorInfo.ID}, idSheduleOperator: {operatorInfo.idSheduleOperator}");
              return new Tuple<OperatorInfo, bool>(operatorInfo, true);
            }

        log.Warn($"Нет idSheduleOperator");
        return new Tuple<OperatorInfo, bool>(new OperatorInfo(idOperator), false);
      }
      catch (Exception ex)
      {
        log.Error(ex.Message);
        return new Tuple<OperatorInfo, bool>(new OperatorInfo(idOperator), false);
      }
    }
  }
}