using NoPaper.Models;
using NoPaper.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace NoPaper.Controllers
{
  internal class TaskController: SPIDInfo
  {
    private readonly SqlConnection _conn;
    private readonly TaskData      _taskData;

    public TaskController(SqlConnection conn)
    {
      _conn = conn;
      _conn.Open();
      InitializeSPID(conn);
    }

    public TaskController(SqlConnection conn, TaskData taskData) : this(conn)
    {
      _taskData = taskData;
    }

    public void MakeTempTable(string accountNum)
    {
      string commandText = GlobalTempData.MakeData(GetSPID, $"AccountNum = '{accountNum}'");

      SqlCommand command = new SqlCommand(commandText, _conn);
      command.ExecuteNonQuery();
    }

    public DataTable GetTaskTable()
    {
      try
      {
        if (string.IsNullOrEmpty(_taskData.AccountNum))
          return null;

        MakeTempTable(_taskData.AccountNum);

        string     query   = _taskData.GetData(_SPID);
        SqlCommand command = new SqlCommand(query, _conn);
        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        {
          DataSet dataSet = new DataSet();
          adapter.Fill(dataSet);
          return dataSet.Tables[0];
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return null;
      }
    }
  }
}