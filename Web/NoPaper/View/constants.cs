namespace NoPaper
{
  internal class Constants
  {
    public static string getBasicQuery() => $@"
      if object_id('tempdb..#TempGlassProcessing', 'U') is not null
        drop table #TempGlassProcessing
       
      select
        idGlassDetails,
        idSawTaskMain,
        idSawTaskMain_Assembly,
        idPiramid,
        idTask,
        NameSawTask,
        GPName,
        ItemNameByOper,
        NameGlassProduct,
        Width,
        Height,
        TimeBeginProcessing,
        NameSectorManufact,
        ForCount,
        bPlot,
        PiramidNum,
        PiramidSide,
        nPack,
        nGlassInPack,
        CurrentPyramidBarCode,
        PreviousPyramidBarCode,
        idGlassProcessingPyramid,
        bFinished,
        nOrderOper,
        idGlassProcessing,
        idSectorManufact,
        ReceiveDateTime,
        ReadyDateTime,
        idBarCode,
        PiramidCell,
        Lead(NameSectorManufact)    over (partition by idGlassDetails order by nOrderOper ASC)  as SectorNext,
        Lead(idSectorManufact)    over (partition by idGlassDetails order by nOrderOper ASC)    as idSectorManufactNext,
        Lag(idSectorManufact)    over (partition by idGlassDetails order by nOrderOper ASC)     as idSectorManufactPrev,
        Lag(ReceiveDateTime)      over (partition by idGlassDetails order by nOrderOper ASC)    as ReceiveDateTimePrev,
        Lag(idGlassProcessing)    over (partition by idGlassDetails order by nOrderOper ASC)    as idGlassProcessingPrev,
        Lag(bFinished) over (partition by idGlassDetails order by nOrderOper ASC)               as bFinishedPrev
      into #TempGlassProcessing
      from
        v_SawTaskGlassProcessing";

    public static string GetFieldsGridOper(string nGlassInPack) => $@"
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
      SectorNext,
      ForCount,
      bPlot,
      PiramidNum,
      PiramidSide,
      nPack,
      {nGlassInPack},
      CurrentPyramidBarCode,
      PreviousPyramidBarCode,
      idGlassProcessingPyramid,
      bFinished,
      nOrderOper,
      idGlassProcessing,
      idSectorManufact,
      ReceiveDateTime,
      ReadyDateTime,
      idSectorManufactNext,
      idSectorManufactPrev,
      ReceiveDateTimePrev,
      idGlassProcessingPrev,
      bFinishedPrev,
      idBarCode,
      PiramidCell
    ";

    public static string GetUnionAssembly(int idSector, string sortExpression) => $@"
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
        nGlassInPackReverse,
        'assembly' as CurrentPyramidBarCode,
        ''          as PreviousPyramidBarCode,
        idGlassProcessingPyramid,
        bFinished,
        nOrderOper,
        idGlassProcessing,
        idSectorManufact,
        ReceiveDateTime,
        ReadyDateTime,
        idSectorManufactNext,
        idSectorManufactPrev,
        ReceiveDateTimePrev,
        idGlassProcessingPrev,
        bFinishedPrev,
        idBarCode,
        NULL as PiramidCell
          from #TempGlassProcessing temp                     
          inner join 
          (
            select
              idGlassDetails,
              row_number() over (partition by idGlassProcessingPyramid order by nGlassInPack desc) as nGlassInPackReverse
            from #TempGlassProcessing
            where idSectorManufact = {idSector}
          ) SubQuery on SubQuery.idGlassDetails = temp.idGlassDetails
           where 
             idSectorManufact                    = {idSector}
             and IsNull(ReceiveDateTimePrev, 0) != 0
             and IsNull(bFinished, 0)           != 1
      order by
        {sortExpression}
    ";

    public static string GetAssemblyGridOper(string sIdSawTask, int idSectorPrev, int idSector, string nGlassInPack, string subQuery) => $@"
      select
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
        SectorNext,
        ForCount,
        bPlot,
        PiramidNum,
        PiramidSide,
        nPack,
        {nGlassInPack},
        CurrentPyramidBarCode,
        CurrentPyramidBarCode as PreviousPyramidBarCode,
        idGlassProcessingPyramid,
        bFinished,
        nOrderOper,
        idGlassProcessing,
        idSectorManufact,
        ReceiveDateTime,
        ReadyDateTime,
        idSectorManufactNext,
        idSectorManufactPrev,
        ReceiveDateTimePrev,
        idGlassProcessingPrev,
        bFinishedPrev,
        idBarCode,
        PiramidCell
      from #TempGlassProcessing temp
      {subQuery}
      where idSawTaskMain_Assembly    = {sIdSawTask}
            and idSectorManufact      = {idSectorPrev}
            and idBarCode in (select 
        idBarCode
                              from #TempGlassProcessing
                              where idSectorManufact                = {idSector}
                                and IsNull(ReceiveDateTimePrev, 0) != 0
                                and IsNull(bFinished, 0)           != 1)
    ";

    public static string GetBasicGridOper(string nGlassInPack, string subQuery, int idSector, string sortExpression)
    {
      return $@"select {GetFieldsGridOper(nGlassInPack)}
      from #TempGlassProcessing temp
      {subQuery}
                where 
                  (
                    ( 
                      case 
                        when bFinishedPrev is null then 2
                        else bFinishedPrev
                      end
                    ) = 2
                    or  IsNull(ReceiveDateTimePrev, 0) != 0
                  )
                  and IsNull(bFinished, 0) != 1
                  and idSectorManufact      = {idSector}
                  and idPiramid             is not null
                order by
                  {sortExpression}";
    }
    public static string GetReverseGlassInPack(int sIdSector) => $@"
      inner join 
      (
        select
          idGlassDetails,
          row_number() over (partition by idGlassProcessingPyramid order by nGlassInPack desc) as nGlassInPackReverse
        from #TempGlassProcessing
        where idSectorManufact = {sIdSector}
      ) SubQuery on SubQuery.idGlassDetails = temp.idGlassDetails";

  }
}
