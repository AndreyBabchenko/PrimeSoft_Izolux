using NoPaper.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Utils;

namespace NoPaper.Controllers
{
  internal class SawLimitController
  {
    private SqlConnection  _conn;   
    private List<SawLimit> _sawLimits;
    public  List<SawLimit> GetListSawLimit => _sawLimits;
    public SawLimitController(SqlConnection conn, bool bAssembly = false)
    {
      _conn = conn;
      InitializeSawLimits(bAssembly);
    }

    private void InitializeSawLimits(bool bAssembly)
    {
      _sawLimits = new List<SawLimit>();

      string commandText;
      if (bAssembly)
        commandText = @"select 
                          SM.ID    as idSectorManufact,
                          0       as idSawLimit,    -- Пока без ограничений
                          ''      as nameSawLimit,
                          0       as idEquipment,   -- Пока уберем оборудование
                          ''      as nameEquipment,
                          AL.ID   as idAssemblyLine, -- Линия сборки
                          AL.Name as nameAsseblyLine
                        from AssemblyLine AL
                        inner join SectorManufact SM on SM.nType = 2";
      else
        commandText = $@"select 
                           SL.idSectorManufact,
                           SL.ID   as idSawLimit,
                           SL.Name as nameSawLimit,
                           E.ID    as idEquipment,
                           E.Name  as nameEquipment,
                           0       as idAssemblyLine,
                           ''      as nameAsseblyLine
                         from SawLimit  SL
                         inner join Equipment E  on SL.idEquipment = E.ID
                         where idSectorManufact is not null";

      SqlCommand command = new SqlCommand(commandText, _conn);

      using (SqlDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          Equipment equipment = bAssembly 
                              ? new Equipment ( _ID  : SafeConvert.ToInt   (reader["idAssemblyLine"] ),
                                                _Name: SafeConvert.ToString(reader["nameAsseblyLine"]),
                                                type : TypeEquipment.e_assembly)
                              : new Equipment ( _ID  : SafeConvert.ToInt   (reader["idEquipment"  ] ),
                                                _Name: SafeConvert.ToString(reader["nameEquipment"] ),
                                                type : TypeEquipment.e_default);

          SawLimit sawLimit = new SawLimit(_ID:               SafeConvert.ToInt   (reader["idSawLimit"  ]),
                                           _Name:             SafeConvert.ToString(reader["nameSawLimit"]),
                                           _idSectorManufact: SafeConvert.ToInt   (reader["idSectorManufact"]),
                                           _equipment:        equipment);

          _sawLimits.Add(sawLimit);
        }
      }
    }

  }
}