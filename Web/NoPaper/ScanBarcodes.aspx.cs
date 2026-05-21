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
using NoPaper.Models;
using NoPaper.Controllers;
using System.Drawing;


namespace NoPaper
{
  public partial class ScanBarcodes : PageModel
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        // Получаем параметр "sector" из строки запроса
        string sectorParam = Request.QueryString["sector"];
        int sector = 0;

        if (!string.IsNullOrEmpty(sectorParam) && int.TryParse(sectorParam, out int result))
        {
          sector = result; 
          ddSector.Enabled = false; // Если параметр есть, отключаем DropDownList
        }
        else
          ddSector.Enabled = true; // Если параметра нет, разрешаем изменение
        

        LoadSectorManufact(ddSector, sector);
      }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object PostPyramidBarCode(string pyramidBarCode)
    {
      MessageModel messageModel;
      int idPyramidBarCode = 0;

      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
      {
        conn.Open();

        string query = $"select ID from PyramidBarCode where BarCode = @pyramidBarCode";
        SqlCommand sqlCommand = new SqlCommand(query, conn);
        sqlCommand.Parameters.AddWithValue("@pyramidBarCode", pyramidBarCode);

        object result = sqlCommand.ExecuteScalar();

        if (result != null)
        {
          idPyramidBarCode = Convert.ToInt32(result);
          // Используйте ID по необходимости
          messageModel = new MessageModel(true, $"Штрих-код {pyramidBarCode} успешно отсканирован");
        }
        else
          messageModel = new MessageModel(false, "Штрих-код не найден.");
      }
      return new
      {
        idPyramidBarCode,
        isSuccess = messageModel.isSuccess,
        message = messageModel.message
      };
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object PostOperatorBarCode(string operBarCode)
    {
      int idOperator  = 0;
      string operName = "";
      MessageModel messageModel;

      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
      {
        conn.Open();

        string query = $"select ID, Name from Operator where BarCode = @operBarCode";
        SqlCommand sqlCommand = new SqlCommand(query, conn);
        sqlCommand.Parameters.AddWithValue("@operBarCode", operBarCode);

        using (SqlDataReader reader = sqlCommand.ExecuteReader())
        {
          if (reader.Read())
          {
            idOperator = Convert.ToInt32(reader["ID"]);
            operName = reader["Name"].ToString();
            messageModel = new MessageModel(true, $"Выбран оператор {operName}");
          }
          else
            messageModel = new MessageModel(false, "Оператор не найден.");
        }
      }

      return new
      {
        idOperator,
        operName,
        isSuccess = messageModel.isSuccess,
        message = messageModel.message
      };
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object PostBarCode(string barCodeGlassText, int idPyramidScanBarCode, int idOperator, int idSector)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          BarCodeInfo barCodeInfo = BarCodeController.CreateBarCodeInfo(conn, barCodeGlassText);
          return BarCodeController.WriteBarCodeGlass(conn, barCodeInfo, barCodeGlassText, idSector, idOperator, idPyramidScanBarCode);
        }
      }
      catch
      {
        return new MessageModel(false, "Произошла ошибка, повторите попытку");
      }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static MessageModel PostMessage(string currentBarCode, int idOperator)
    {
      if (string.IsNullOrWhiteSpace(currentBarCode))
        return new MessageModel(false, "Штрих код не введен");

      if (idOperator == 0)
        return new MessageModel(false, "Оператор не выбран");

      return new MessageModel(false, "", false);
    }
  }
}

