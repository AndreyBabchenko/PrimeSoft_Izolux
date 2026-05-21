using NoPaper.Models;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;

namespace NoPaper.Queries
{
  static class SawTaskMainData
  {
    public static string GetData(string dateStart, string dateEnd) =>
      $@"set dateformat dmy 
         select 
           v.ID,
           Name,
           Data,
           Comment,
           ListGlass,
           GlassCount,
           nCount_Assembly,
           Max(GD.ProcessingRoute) as ProcessingRoute
         from v_SawTaskMain           v
              inner join GlassDetails GD on GD.idSawTaskMain = v.ID
         where cast    (Data as date) >= cast('{ dateStart }' as date)
               and cast(Data as date) <= cast('{ dateEnd   }' as date)
               and IsNull(bStart, 0) != 1
               and (IsNull(nCount_Assembly, 0) != 0 or IsNull(GlassCount, 0) != 0)
               and ProcessingRoute is not null
         group by
           v.ID,
           Name,
           Data,
           Comment,
           ListGlass,
           GlassCount,
           nCount_Assembly
         order by Name";

    public static string GetDataByNameSawTask(string nameSawTask) =>
    $@"select
         ID,
         Name
       from SawTaskMain
       where Name = '{nameSawTask}'";

    // Запрос на получение idSawTaskMain для отображения всех пирамид готовых к перемещению
    public static string GetAllReadyToTransportData(SectorManufactInfo sector) =>
    $@"with cte as
       (
         select
           GPP.idSawTaskMain as ID,
           GPP.ID            as idGlassProcessingPyramid,
           GPP.idSawTaskMain as idSawTaskMain_Assembly,
           STM.Name
         from        GlassProcessingPyramid GPP
           inner join SawTaskMain           STM on STM.ID = GPP.idSawTaskMain
   			 where IsNull(ReadyDateTime,   0) != 0 or  -- Пирамида готова
               IsNull(ReceiveDateTime, 0) != 0     -- Пирамида забрана на следующем рабочем месте 
   	     group by
           GPP.idSawTaskMain,
           GPP.ID,
           STM.Name
       )
       select 
         cte.ID,
         cte.idSawTaskMain_Assembly,
         cte.Name
       from cte
       outer apply 
       (
         select 
           count(1)                                                     as nCount,          -- количество деталей
           sum(case when IsNull(GP.bFinished, 0) = 1 then 1 else 0 end) as nCount_Finished  -- количество готовых деталей
         from         GlassDetails    GD
           inner join GlassProcessing GP on GP.idGlassDetails = GD.ID
         where GD.idSawTaskMain = cte.ID                               -- Соединяем по idSawTaskMain
       ) as AP
       where nCount > nCount_Finished                                  -- Выводим раскрои с пирамидами, где количество сделаных деталей меньше общего количества
       group by
          cte.ID,
          cte.idSawTaskMain_Assembly,
          cte.Name
       -- Сортируем в порядке оратном порядку создания раскроя
       order by cte.ID desc";    // Запрос получает список idSawTaskMain, где есть не готовые детали

    public static string UpdateStartStatus(int ID) =>
      $@"update SawTaskMain set 
           bStart = 1 
         where ID = {ID}";
  }
}