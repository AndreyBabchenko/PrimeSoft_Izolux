namespace NoPaper.Queries
{
  static class GlobalTempData
  {
    public static string MakeData(int SPID, string condition) =>
    // Все стёкла на всех пирамидах на всех участках
     $@"if object_id('tempdb..##TempGlassProcessing{SPID}', 'U') is not null
          drop table          ##TempGlassProcessing{SPID}

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
          SawOrder,                                                        -- Порядок упаковки
          ProjectNum,
          BarCode,
          BarCode_Glass,
          ProcessingRoute,
          idSawLimit,
          idProject,
          bShpros,
          bChangeOrder,
          case when nState       & 512 = 512
            then 1
            else 0
          end as isDefect,
          case when nState_Glass & 512 = 512
            then 1
            else 0
          end as isDefectGlass,
          Min(SawOrder) over (partition by idBarCode) as assemblyOrder,  -- Для сортировки на участке сборки
          case
           when Lead(NameSectorManufact) over (partition by idGlassDetails order by nOrderOper asc) is     null and
                Lead(  idSectorManufact) over (partition by idGlassDetails order by nOrderOper asc) is not null then
           (
             select Name
             from SectorManufact
             where ID = SubString(ProcessingRoute, 
                        CharIndex(ltrim(rtrim(str(idSectorManufact))), ProcessingRoute) + len(ltrim(rtrim(str(idSectorManufact)))) + 1,
                                  2)
           )
           else
             Lead(NameSectorManufact) over (partition by idGlassDetails order by nOrderOper asc)
          end as SectorNext,

          case when Lead(idSectorManufact) over (partition by idGlassDetails order by nOrderOper asc) is     null and
                    Lead(idSectorManufact) over (partition by idGlassDetails order by nOrderOper asc) is not null
            then 
              case when CharIndex(',' + Cast(idSectorManufact as varchar), ProcessingRoute) < Len(ProcessingRoute)
                then 
                  SubString(ProcessingRoute, 
                  CharIndex(ltrim(rtrim(str(idSectorManufact))), ProcessingRoute) + len(ltrim(rtrim(str(idSectorManufact)))) + 1,
                            2)
                else NULL
              end
            else Lead(idSectorManufact) over (partition by idGlassDetails order by nOrderOper asc)
          end as idSectorManufactNext,                                                              -- Следующий сектор
          
          Lag(idSectorManufact)     over (partition by idGlassDetails order by nOrderOper asc) as idSectorManufactPrev,
          Lag(ReceiveDateTime)      over (partition by idGlassDetails order by nOrderOper asc) as ReceiveDateTimePrev,
          Lag(idGlassProcessing)    over (partition by idGlassDetails order by nOrderOper asc) as idGlassProcessingPrev,
          Lag(bFinished)            over (partition by idGlassDetails order by nOrderOper asc) as bFinishedPrev,
          Lag(bChangeOrder)         over (partition by idGlassDetails order by nOrderOper asc) as bChangeOrderPrev
        into ##TempGlassProcessing{SPID}
        from
          v_SawTaskGlassProcessing
        where {condition}";
  }
}
