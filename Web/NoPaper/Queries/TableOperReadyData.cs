namespace NoPaper.Queries
{
  static class TableOperReadyData
  {
    public static string GetData(string sListIdSawTask, int idSectorNext, int SPID, string sWhereAdd = "") =>
    // Получаем пирамиды которые необходим забрать с предыдущего участка
      $@"select 
           t.idGlassProcessingPyramid,
           stuff((
               select ', ' + t2.NameGlassProduct
               from (
                   select distinct NameGlassProduct
                   from ##TempGlassProcessing{SPID} t2
                   where t2.idGlassProcessingPyramid = t.idGlassProcessingPyramid
                   and idSawTaskMain                in ({sListIdSawTask})
               ) t2
               for xml path('')
           ), 1, 2, '') as NameGlassProduct,
           t.PiramidNum,
           t.CurrentPyramidBarCode,
           t.NameSawTask,
           t.NameSectorManufact,
           t.idSectorManufactNext,
           t.ReceiveDateTime,
           t.ReadyDateTime,
           t.bChangeOrder
         from 
           ##TempGlassProcessing{SPID} t
         where 
               IsNull(t.ReadyDateTime, 0)  != 0  -- Готова, чтобы ее забрали
           and IsNull(t.ReceiveDateTime, 0) = 0  -- Еще не забрали на текущий участок
           and IsNull(t.bFinished, 0)       = 1  -- Все детали помечены как готовые
           and t.idSawTaskMain       in ({sListIdSawTask})
           and t.idSectorManufactNext = {idSectorNext}
           {sWhereAdd}
           -- sEquipmentWhere
         group by 
           t.idGlassProcessingPyramid,
           t.PiramidNum,
           t.CurrentPyramidBarCode,
           t.NameSawTask,
           t.NameSectorManufact,
           t.idSectorManufactNext,
           t.ReceiveDateTime,
           t.ReadyDateTime,
           t.bChangeOrder";
  }
}