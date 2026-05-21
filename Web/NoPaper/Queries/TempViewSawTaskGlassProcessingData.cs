namespace NoPaper.Queries
{
  static class TempViewSawTaskGlassProcessingData
  {
    public static string GetQuery() => $@"
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
        AccountNum,
        PiramidOrder,
        ProjectNum,
        Max(nGlassInPack)         over (partition by idGlassProcessingPyramid) - nGlassInPack + 1 as nGlassInPackReverse,
        Lead(NameSectorManufact)  over (partition by idGlassDetails order by nOrderOper ASC)      as SectorNext,
        Lead(idSectorManufact)    over (partition by idGlassDetails order by nOrderOper ASC)      as idSectorManufactNext,
        Lag(idSectorManufact)     over (partition by idGlassDetails order by nOrderOper ASC)      as idSectorManufactPrev,
        Lag(ReceiveDateTime)      over (partition by idGlassDetails order by nOrderOper ASC)      as ReceiveDateTimePrev,
        Lag(idGlassProcessing)    over (partition by idGlassDetails order by nOrderOper ASC)      as idGlassProcessingPrev,
        Lag(bFinished)            over (partition by idGlassDetails order by nOrderOper ASC)      as bFinishedPrev
      into #TempGlassProcessing
      from
        v_SawTaskGlassProcessing";

    public static string GetFields() => $@"
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
      nGlassInPack,
      nGlassInPackReverse,
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
      PiramidCell,
      AccountNum,
      PiramidOrder,
      ProjectNum
    ";
  }
}