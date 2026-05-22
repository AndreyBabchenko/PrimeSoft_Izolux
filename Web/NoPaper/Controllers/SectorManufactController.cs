using NoPaper.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Web.UI.WebControls;
using Utils;

namespace NoPaper.Controllers
{
  internal class SectorManufactController
  {
    private SqlConnection _conn;
    private List<SectorManufactInfo> _sectorManufactList;

    public List<SectorManufactInfo> GetSectorManufactInfo => _sectorManufactList;

    public SectorManufactController(SqlConnection conn)
    {
      _conn = conn;
      InitializeDefaultSectorManufact();
    }

    public SectorManufactController(SqlConnection conn, string sIdSectorManufact)
    {
      _conn = conn;
      InitializeSectorManufactByValue(sIdSectorManufact);
    }

    public SectorManufactController(SqlConnection conn, int nType)
    {
      _conn = conn;
      InitializeSectorManufactBy_nType(nType);
    }

    private void InitializeDefaultSectorManufact()
    {
      _sectorManufactList = new List<SectorManufactInfo>();
      SqlCommand command  = new SqlCommand("select ID, Name, nType, bChangeOrder from SectorManufact order by nOrderReport", _conn);

      using (SqlDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          SectorManufactInfo sector = new SectorManufactInfo(
            _ID:           SafeConvert.ToInt(reader["ID"   ].ToString()),
            _nType:        SafeConvert.ToInt(reader["nType"].ToString()),
            _Name:         reader["Name"].ToString(),
            _bChangeOrder: SafeConvert.ToBool(reader["bChangeOrder"]));

          _sectorManufactList.Add(sector);
        }
      }
    }

    public void InitializeSectorManufactByValue(string sIdSectorManufact)
    {
      SqlCommand     command = new SqlCommand($@"select
                                                   ID,
                                                   Name,
                                                   nType,
                                                   bChangeOrder
                                                 from SectorManufact
                                                 where ID = {sIdSectorManufact}", 
                                              _conn);
      SqlDataReader  reader = command.ExecuteReader();
      if (           reader.Read() )
      {
         SectorManufactInfo sector = new SectorManufactInfo(
            _ID:    Convert.ToInt32(reader["ID"   ].ToString()),
            _nType: Convert.ToInt32(reader["nType"].ToString()),
            _Name:  reader["Name"].ToString(),
            _bChangeOrder: SafeConvert.ToBool(reader["bChangeOrder"]));

        _sectorManufactList.Add(sector);
      }
    }

    public void InitializeSectorManufactBy_nType(int nType)
    {
      _sectorManufactList    = new List<SectorManufactInfo>();
      SqlCommand     command = new SqlCommand($@"select
                                                   ID,
                                                   Name,
                                                   nType,
                                                   bChangeOrder
                                                 from SectorManufact
                                                 where nType = {nType}",
                                              _conn);
      using (SqlDataReader reader = command.ExecuteReader())
      {
        if (reader.Read())
        {
          SectorManufactInfo sector = new SectorManufactInfo(
              _ID:    Convert.ToInt32(reader["ID"   ].ToString()),
              _nType: Convert.ToInt32(reader["nType"].ToString()),
              _Name:  reader["Name"].ToString(),
              _bChangeOrder: SafeConvert.ToBool(reader["bChangeOrder"]));

          _sectorManufactList.Add(sector);
        }
      }
    }

    public static int GetPreviousSectorManufactID(string sList_idSawTask, int SPID, SectorManufactInfo curentSector, SqlCommand command)
    {
      command.CommandText = $@"select top 1
                                 idSectorManufactPrev
                               from ##TempGlassProcessing{SPID}
                               where idSawTaskMain_Assembly in ({sList_idSawTask})
                                 and idSectorManufact       = {curentSector.ID}";

      using (SqlDataReader reader = command.ExecuteReader())
        return reader.Read() ? Convert.ToInt32(reader["idSectorManufactPrev"].ToString()) : 0;
    }
  }
}