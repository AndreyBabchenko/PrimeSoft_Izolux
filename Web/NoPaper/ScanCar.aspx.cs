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
using System.Collections;
using System.Drawing;

namespace NoPaper
{
// Сканирование выезда машин
  public partial class ScanCar : PageModel
  {
    private static int    m_idOperator              = 0,  // ID Оператора
                          m_idTripTransport         = 0;  // ID Машины
    private static string m_sBarCodePrefixOperator  = ""; // Префикс штрихкода оператора

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
        {
          conn.Open();
          // Инициализируем операторов с ключем как idSheduleOperator
          LoadOperatorList(Operator, "idSheduleOperator");

          // Префикс оператора сканирования
          if (m_sBarCodePrefixOperator == "")
          {
            SqlCommand command = new SqlCommand("select d_string from Config where Name = 'BarCodePrefixOperator'", conn);
            m_sBarCodePrefixOperator = command.ExecuteScalar().ToString().ToLower();
          }
        }
      }

      // Устанавливаем оператора
      if (m_idOperator != 0)
        Operator.SelectedValue = m_idOperator.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object PostBarCode(string barcode, int idOperator)
    {
      Tuple<bool, string> result = new Tuple<bool, string>(false, "");

      try
      {
        m_idOperator = idOperator;

        if (barcode.Length == 0)
          result = new Tuple<bool, string>(false, "Требуется ввести команду");

        string operBarCodePrefix = barcode.Substring(0, m_sBarCodePrefixOperator.Length).ToLower();

        if (operBarCodePrefix == m_sBarCodePrefixOperator && barcode.Length > 0)
          result = SetOperator(barcode);
        else
        {
          result = WriteBarCode(barcode);

          // Если не найден штрих-код машины, возможно имеем дело с номером накладной
          if (result.Item1 == false)
            result = ScanShip(barcode);
        }  

        return new
        {
          message         = result.Item2,
          idOperator      = m_idOperator
        };
      }
      catch (Exception ex)
      {
        return new Tuple<bool, string>(false, ex.Message);
      }
    }

    // Устанавливаем оператора
    [WebMethod]
    private static Tuple<bool, string> SetOperator(string barcode)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
        {
          conn.Open();
          string query = $"select ID from Operator where BarCode = @barcodeText";
          SqlCommand sqlCommand = new SqlCommand(query, conn);
          sqlCommand.Parameters.AddWithValue("@barcodeText", barcode);

          object result = sqlCommand.ExecuteScalar();

          if (result != null)
          {
            m_idOperator = Convert.ToInt32(result);
            return new Tuple<bool, string>(true, $"Отсканирован оператор: {barcode}");
          }
          else
            return new Tuple<bool, string>(false, $"Отсканирован не найден: {barcode}");
        }
      }
      catch (Exception ex)
      {
        return new Tuple<bool, string>(false, ex.Message);
      }
    }

    private static Tuple<bool, string> WriteBarCode(string barcode)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
        {
          conn.Open();
          string query = "select ID from TripTransport where barcode = @barcode";
          SqlCommand sqlCommand = new SqlCommand(query, conn);
          sqlCommand.Parameters.AddWithValue("@barcode", barcode);

          object result = sqlCommand.ExecuteScalar();

          if (result == null)
            return new Tuple<bool, string>(false, $"Штрих-код машины не найден: {barcode}");

          m_idTripTransport = Convert.ToInt32(result);
          AddScanTripTransport(m_idTripTransport, conn);

          return new Tuple<bool, string>(true, $"Отсканирован штрих-код машины: {barcode}");
        }
      }
      catch (Exception ex)
      {
        return new Tuple<bool, string>(false, ex.Message);
      }
    }

    /// <summary>
    /// Добавляет в таблицу отсканированных штрихкодов запись о машине
    /// </summary>
    /// <param name="idTripTransport">ID машины</param>
    /// <param name="conn">Открытое подключение</param>
    private static void AddScanTripTransport(int idTripTransport, SqlConnection conn)
    {
      string query = @"insert into ScanLog_TripTransport_BarCode (TimeScan,  idOperator,  idTripTransport)
                       select                                     getDate(), @idOperator, @idTripTransport";
      SqlCommand command = new SqlCommand(query, conn);
      command.Parameters.AddWithValue("@idOperator",      m_idOperator);
      command.Parameters.AddWithValue("@idTripTransport", idTripTransport);

      command.ExecuteNonQuery();
    }

    private static Tuple<bool, string> ScanShip(string barcode)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
        {
          conn.Open();

          string query = $"select top 1 ID from Ship where TransportNumList like '%{barcode}%'";
          SqlCommand command = new SqlCommand(query, conn);

          object result = command.ExecuteScalar();

          if (result != null)
          {
            int idShip = Convert.ToInt32(result);

            command.CommandText = $"select idTask, idBarCode from v_PyramidForShip where idShip = {idShip} and TreeLevel = -1 and TableLevel != 1";

            List<int> arIdBarCodes = new List<int>();
            HashSet<int> arIdTasks = new HashSet<int>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                int idTask    = reader.GetInt32(0);
                int idBarCode = reader.GetInt32(1);

                arIdBarCodes.Add(idBarCode);
                arIdTasks.Add(idTask);
              }
            }

            if (arIdBarCodes.Count == 0)
              return Tuple.Create(false, "Нет штрихкодов для отгрузки");

            string sWhere = $"where ID in ({string.Join(",", arIdBarCodes)})";
            command.CommandText = $"update BarCode set nState = IsNull(nState, 0) | 8 {sWhere}"; // Присвоить статус отгружен штрих-кодам
            command.ExecuteNonQuery();

            foreach (int idTask in arIdTasks)
            {
              command.CommandText = $"exec sp_UpdateTaskState {idTask}";
              command.ExecuteNonQuery();
            }

            return Tuple.Create(true, "Накладная отсканированна");
          }
          else throw new Exception("Ошибка сканирования накладной");
        }
      }
      catch (Exception ex)
      {
        return Tuple.Create(false, ex.Message);
      }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetScanLogs()
    {
      try
      {       
        using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
        {
          conn.Open();
          string query = $"select * from v_ScanLog_TripTransport_BarCode order by TimeScan desc";
          SqlCommand command = new SqlCommand(query, conn);
          

          DataTable table = new DataTable();
          using (SqlDataReader reader = command.ExecuteReader())
            table.Load(reader);

          return JsonConvert.SerializeObject(table);
        }
      }
      catch
      {
        return "[]";
      }
    }
  }
}

