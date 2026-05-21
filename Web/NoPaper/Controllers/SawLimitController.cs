using NoPaper.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace NoPaper.Controllers
{
  internal class SawLimitController
  {
    private SqlConnection  _conn;   
    private List<SawLimit> _sawLimits;
    public  List<SawLimit> GetListSawLimit => _sawLimits;
    
    public SawLimitController(SqlConnection conn) 
    { 
      _conn = conn;
      InitializeSawLimits();
    }

    private void InitializeSawLimits()
    {
      _sawLimits = new List<SawLimit>();

      SqlCommand command = new SqlCommand($@"select 
                                               SL.idSectorManufact,
                                               SL.ID   as idSawLimit,
                                               SL.Name as nameSawLimit,
                                               E.ID    as idEquipment,
                                               E.Name  as nameEquipment
                                             from       SawLimit  SL
                                             inner join Equipment E  on SL.idEquipment = E.ID
                                             where idSectorManufact is not null",
                                             _conn);

      using (SqlDataReader reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          Equipment equipment = new Equipment( _ID  : reader.GetInt32 (reader.GetOrdinal("idEquipment"  ) ),
                                               _Name: reader.GetString(reader.GetOrdinal("nameEquipment") ));

          SawLimit sawLimit = new SawLimit(_ID:               reader.GetInt32 (reader.GetOrdinal("idSawLimit"      )),
                                           _Name:             reader.GetString(reader.GetOrdinal("nameSawLimit"    )),
                                           _idSectorManufact: reader.GetInt32 (reader.GetOrdinal("idSectorManufact")),
                                           _equipment:        equipment );

          _sawLimits.Add(sawLimit);
        }
      }
    }

  }
}