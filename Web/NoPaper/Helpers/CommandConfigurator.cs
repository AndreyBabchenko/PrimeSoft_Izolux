using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NoPaper.Helpers
{
  public static class CommandConfigurator
  {
    public static void ConfigureInsertCommand(SqlDataAdapter adapter, string commandText, Dictionary<string, SqlParameter> parameters, SqlConnection conn)
    {
      SqlCommand insertCommand = new SqlCommand(commandText, conn);

      foreach (var param in parameters)
        insertCommand.Parameters.Add(param.Value);

      adapter.InsertCommand = insertCommand;
    }
    public static SqlParameter CreateParameter(string paramName, SqlDbType sqlDbType, string sourceColumn)
    {
      return new SqlParameter(paramName, sqlDbType) { SourceColumn = sourceColumn };
    }

  }
}