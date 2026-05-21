using NoPaper.Models;
using NoPaper.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace NoPaper.Controllers
{
  internal class SawTaskMainController
  {
    private SqlConnection _conn;
    private SawTaskInfo   _sawTask;

    public SawTaskMainController(SqlConnection conn)
    {
      _conn = conn;
      _conn.Open();
    }

    public SawTaskMainController(SqlConnection conn, SawTaskInfo sawTask) : this(conn)
    {
      _sawTask = sawTask;
    }

    public DataTable GetSawTaskTable(string dateStart, string dateEnd)
    {
      SqlCommand command = new SqlCommand(SawTaskMainData.GetData(dateStart, dateEnd), _conn);
      using (SqlDataAdapter adapter = new SqlDataAdapter(command))
      {
        DataSet dataSet = new DataSet();
        adapter.Fill(dataSet);
        return dataSet.Tables[0];
      }
    }

    public DataTable GetSawTaskTableByFilter(SectorManufactInfo sector, string dateStart, string dateEnd)
    {
      DataTable table = GetSawTaskTable(dateStart, dateEnd);
      string    sIdSector = sector.ID.ToString();

      // Фильтрация по idSector
      var filteredRows = table.AsEnumerable()
            .Where(row => row.Field<string>("ProcessingRoute")
            .Split(',')
            .Contains(sIdSector));

      DataTable filteredTable = table.Clone();

      // Импорт отфильтрованных строк в новую таблицу
      foreach (var row in filteredRows)
        filteredTable.ImportRow(row);
      

      return filteredTable;
    }

    public void UpdateStartStatus()
    {
      SqlCommand command = new SqlCommand(SawTaskMainData.UpdateStartStatus(_sawTask.ID), _conn);
      command.ExecuteNonQuery();
    }

    public static int GetIDSawTaskByBarCode(BarCodeInfo barCodeInfo, BarCodeGlassInfo barCodeGlassInfo, SqlConnection conn)
    {
      try
      {
        string s1 = $"{(int)e_TypeSectorManuf.e_tsm_Assembly}";
        string commandText = "select top 1 idSawTaskMain from GlassDetails where idSawTaskMain is not null and idBarCode in ";

        if (barCodeInfo != null)
          commandText += $"({barCodeInfo.ID})";
        else
          commandText += $"(select top 1 idBarCode from BarCode_Glass where ID = {barCodeGlassInfo.ID})";

        using (SqlCommand command = new SqlCommand(commandText, conn))
        {
          object resIdSawTask = command.ExecuteScalar();

          if (resIdSawTask != null)
            return Convert.ToInt32(resIdSawTask);
          else throw new Exception();
        }
      }
      catch
      {
        return 0;
      }
    }

    public static int GetIDSawTaskByIdGlassDetails(int? idGlassDetails, SqlConnection conn)
    {
      try
      {
        string s1 = $"{(int)e_TypeSectorManuf.e_tsm_Assembly}";
        string commandText = $"select top 1 idSawTaskMain from GlassDetails where ID = {idGlassDetails}";

        using (SqlCommand command = new SqlCommand(commandText, conn))
        {
          object resIdSawTask = command.ExecuteScalar();

          if (resIdSawTask != null)
            return Convert.ToInt32(resIdSawTask);
          else throw new Exception();
        }
      }
      catch
      {
        return 0;
      }
    }
  }
}