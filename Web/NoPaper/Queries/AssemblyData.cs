using NoPaper.Models;

namespace NoPaper.Queries
{
  static class AssemblyData
  {
    public static string GetUnionAssembly(string sListIdSawTask, string sWhereEquipment, int idSector, int SPID, string sortExpression) => 
      $@"union all
         select 
           temp.idGlassDetails,
           idSawTaskMain,
           idPiramid,
           NameSawTask,
           GPName,
           GPName as NameGlassProduct,
           Width,
           Height,
           TimeBeginProcessing,
           NameSectorManufact,
           SectorNext,
           NULL as ForCount,
           bPlot,
           '' as PiramidNum,
           PiramidSide,
           nPack,
           nGlassInPack, 
           'assembly' as CurrentPyramidBarCode,
           ''          as PreviousPyramidBarCode,
           idGlassProcessingPyramid,
           bFinished,
           nOrderOper,
           idGlassProcessing,
           temp.idSectorManufact,
           ReceiveDateTime,
           ReadyDateTime,
           idSectorManufactNext,
           idSectorManufactPrev,
           ReceiveDateTimePrev,
           idGlassProcessingPrev,
           bFinishedPrev,
           idBarCode,
           BarCode,
           BarCode_Glass,
           NULL as PiramidCell,
           AccountNum,
           0 as PiramidOrder,
           SawOrder,
           ProjectNum,
           NULL as nGlassInPackTake,
           0 as isDefect,
           0 as isDefectGlass,
           assemblyOrder,
           E.Name as NameEquipment,
           temp.idProject,
           0 as GlassNum,
           temp.idAssemblyLine
         from ##TempGlassProcessing{SPID} temp
         left join SawLimit  SL on SL.ID = temp.idSawLimit
         left join Equipment E  on E.ID  = SL.idEquipment    
         where 
               idSawTaskMain                   in ({sListIdSawTask})
           and temp.idSectorManufact           = {idSector}
           and IsNull(ReceiveDateTimePrev, 0) != 0
           and IsNull(bFinished, 0)           != 1
           {sWhereEquipment}
         order by
           {sortExpression}";

    /// <summary>
    ///  Создание строки запроса
    /// </summary>
    /// <param name="sListIdSawTask">Список расскроев либо введенных, либо тех на которых есть пирамида для транспортировки</param>
    /// <param name="idSector">Идентификатор нужного участка</param>
    /// <param name="SPID">Идентификатор сессии, для запроса к временной таблице</param>
    /// <param name="sWhereAssemblySector">Строка условия, используемая только для сборки, для вывода только тех стекл которые используются на сборке по idSectorNext</param>
    /// <param name="sEquipmentWhere">Строка условия, для фильтрации по оборудованию</param>
    /// <param name="sortExpression">Выражения сортировки</param>
    /// <returns>Возвращает строку запроса для получения данных из базы</returns>
    public static string GetData(string sListIdSawTask, int idSector, int SPID, string sWhereAssemblySector, string sEquipmentWhere, string sortExpression)
    {
      return
      $@"select
           temp.idGlassDetails,
           idSawTaskMain,
           idPiramid,
           NameSawTask,
           GPName,
           NameGlassProduct,
           Width,
           Height,
           TimeBeginProcessing,
           NameSectorManufact,
           NULL as SectorNext,
           ForCount,
           bPlot,
           PiramidNum,
           PiramidSide,
           nPack,
           nGlassInPack,
           CurrentPyramidBarCode,
           CurrentPyramidBarCode as PreviousPyramidBarCode,
           idGlassProcessingPyramid,
           bFinished,
           nOrderOper,
           idGlassProcessing,
           temp.idSectorManufact,
           ReceiveDateTime,
           ReadyDateTime,
           idSectorManufactNext,
           idSectorManufactPrev,
           ReceiveDateTimePrev,
           idGlassProcessingPrev,
           bFinishedPrev,
           idBarCode,
           temp.BarCode,
           BarCode_Glass,
           PiramidCell,
           AccountNum,
           PiramidOrder,
           SawOrder,
           ProjectNum,
           nGlassInPack as nGlassInPackTake,
           isDefect,
           isDefectGlass,
           assemblyOrder,
           E.Name as NameEquipment,
           temp.idProject,
           temp.GlassNum,
           temp.idAssemblyLine
         from ##TempGlassProcessing{SPID} temp
         inner join SectorManufact SM on SM.ID = temp.idSectorManufact
         left join SawLimit  SL       on SL.ID = temp.idSawLimit
         left join Equipment E        on E.ID  = SL.idEquipment
         where idSawTaskMain_Assembly   in ({sListIdSawTask})
               {sWhereAssemblySector}
               {sEquipmentWhere}
               and idGlassDetails in (
                                      select 
                                        idGlassDetails 
                                      from ##TempGlassProcessing{SPID} t
                                      where 
                                        idSawTaskMain in ({sListIdSawTask}) and
                                        IsNull(ReceiveDateTime, 0) != 0     and 
                                        IsNull(bFinished,       0)  = 1     and
                                        (
                                          t.idSectorManufactNext = {idSector} or
                                          SM.nType               = 1 -- На резке не факт что найдет следующий участок
                                        )
                                     )
               and (select top 1 IsNull(bFinished, 0) from ##TempGlassProcessing{SPID} t where idBarCode = temp.idBarCode and idSectorManufact = {idSector}) != 1 -- Выводим только если участок сборки еще не был готов
        {GetUnionAssembly(sListIdSawTask, sEquipmentWhere, idSector, SPID, sortExpression)}";
    }
  }
}