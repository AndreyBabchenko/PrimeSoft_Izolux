namespace NoPaper
{
  internal class Constants
  {
    public static string mainQuery(string idSawTask)
    {
      return $@"
               if object_id('tempdb..#TempGlassProcessing', 'U') is not null
                  drop table #TempGlassProcessing
               
                select
                  idGlassDetails,
                  idSawTaskMain,
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
                  SectorNext,
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
                  Lead(idSectorManufact)    over (partition by idGlassDetails order by nOrderOper ASC)    as idSectorManufactNext,
                  Lag(ReceiveDateTime)      over (partition by idGlassDetails order by nOrderOper ASC)    as ReceiveDateTimePrev,
                  Lag(idGlassProcessing)    over (partition by idGlassDetails order by nOrderOper ASC)    as idGlassProcessingPrev,
                  Lag(bFinished) over (partition by idGlassDetails order by nOrderOper ASC)               as bFinishedPrev
                into #TempGlassProcessing
                from
                  v_SawTaskGlassProcessing
                where
                  idSawTaskMain in ({idSawTask})";
    }

    public static string GetFieldsGridOper(string nGlassInPack)
    {
      return $@"
        temp.idGlassDetails,
        idSawTaskMain,
        idPiramid,
        NameSawTask,
        GPName,
        ItemNameByOper,
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
        ReceiveDateTimePrev,
        idGlassProcessingPrev,
        bFinishedPrev 
    ";
    }

    public static string GetReverseGlassInPack(string sIdSector)
    {
      return $@"
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
}