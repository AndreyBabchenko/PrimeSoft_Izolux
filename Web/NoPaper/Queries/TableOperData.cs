using NoPaper.Models;

namespace NoPaper.Queries
{
  static class TableOperData
  {
    /// 
    /// № в пачке взять рассчитывался в обратном порядке в соответствии с текущим № в пачке, применимо ко всем операциям кроме закалки
    ///  
    //const string nGlassInPackTake = @"
    //   case 
    //     when PiramidCell is null then
    //       row_number() over (partition by NameSawTask, 
    //                                       idGlassProcessingPyramid, 
    //                                       PiramidNum, 
    //                                       PiramidSide, 
    //                                       nPack, 
    //                                       PiramidCell    order by nGlassInPack DESC)
    //     else NULL
    //    end as nGlassInPackTake
    //";

    // № в пачке взять, будет таким же как был nGlassInPack на предыдущем участке, применимо к закалке
    const string nGlassInPackTake = @"
        (
          select top 1
            nGlassInPack 
          from GlassDetails GD
          inner join GlassProcessing GP on GD.ID = GP.idGlassDetails
          where GD.ID = temp.idGlassDetails and GP.idSectorManufact = temp.idSectorManufactPrev
        ) as nGlassInPackTake
    ";

    public static string GetData(string sListIdSawTask, SectorManufactInfo sector, int SPID, string sWhereAdd, string sEquipmentWhere, string sortExpression)
    {
      return
      $@"select
           temp.*,
           E.Name as NameEquipment,
           {nGlassInPackTake}
         from ##TempGlassProcessing{SPID} temp
         left join SawLimit  SL on SL.ID = temp.idSawLimit
         left join Equipment E  on E.ID  = SL.idEquipment
         where 
           (
             ( 
               case 
                 when bFinishedPrev is null then 2
                 else bFinishedPrev
               end
             ) = 2
             or  IsNull(ReceiveDateTimePrev, 0) != 0
             {sWhereAdd}
           )
           and IsNull(bFinished, 0) != 1
           and temp.idSectorManufact = {sector.ID}
           and idSawTaskMain        in ({sListIdSawTask})
           {sEquipmentWhere}
         order by
           {sortExpression}";

    }
  }
}