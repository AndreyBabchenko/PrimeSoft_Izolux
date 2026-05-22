using System.Data.SqlClient;
using System.Drawing;
using System;
using System.Web.UI;
using System.Web;

namespace NoPaper.Models
{
  internal class SPIDInfo
  {
    private protected int _SPID;

    public int GetSPID
    {
      get
      {
        // когда HttpContext или сессия недоступны
        if (HttpContext.Current == null || HttpContext.Current.Session == null)
          return _SPID; 
        
        return (int)(HttpContext.Current.Session["SPID"] ?? _SPID);
      }
    }

    public void GetOrInitSPID(out int spid, SqlConnection conn)
    {
      InitializeSPID(conn);

      spid = GetSPID;
    }


    private protected void InitializeSPID(SqlConnection conn)
    {
      // Если уже есть то инициализация не нужна
      if (GetSPID != 0)
      {
        _SPID = GetSPID;
        return;
      }

      SqlConnection _conn = new SqlConnection(conn.ConnectionString);
      _conn.Open();

      SqlCommand    command  = new SqlCommand("select @@spid as SPID", _conn);
      SqlDataReader reader = command.ExecuteReader();

      if (reader.Read())
      {
        _SPID = Convert.ToInt32(reader["SPID"].ToString());
        // Если код выполняется вне контекста http или при асинхроном запросе
        if (HttpContext.Current != null) 
          HttpContext.Current.Session["SPID"] = _SPID;
      }  

      _conn.Close();
    }
  }
}