namespace NoPaper.Queries
{
  static class TableOperReadyData
  {
    public static string GetData(string sListIdSawTask, int SPID, string sWhereAdd = "") =>
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
           t.idSawTaskMain,
           t.NameSectorManufact,
           t.idSectorManufactNext,
           t.ReceiveDateTime,
           t.ReadyDateTime,
           t.bChangeOrder,
           t.ProcessingRoute
         from 
           ##TempGlassProcessing{SPID} t
         where
          -- Решили что нужно выводить пирамиды, если хоть одно стекло на предыдущем этапе было помечено, как готовое
          -- IsNull(t.ReadyDateTime, 0)  != 0 and -- Готова, чтобы ее забрали
             IsNull(t.ReceiveDateTime, 0) = 0  -- Еще не забрали на текущий участок
           and IsNull(t.bFinished, 0)       = 1  -- Все детали помечены как готовые
           and t.idSawTaskMain       in ({sListIdSawTask})
           {sWhereAdd}
           -- sEquipmentWhere
         group by 
           t.idGlassProcessingPyramid,
           t.PiramidNum,
           t.CurrentPyramidBarCode,
           t.NameSawTask,
           t.idSawTaskMain,
           t.NameSectorManufact,
           t.idSectorManufactNext,
           t.ReceiveDateTime,
           t.ReadyDateTime,
           t.bChangeOrder,
           t.ProcessingRoute";
  }
}