namespace NoPaper.Queries
{
  internal class BasicData
  {
    public static string GetData(int idSector, string sortExpression) => 
      $@"select {TempViewSawTaskGlassProcessingData.GetFields()}
                from #TempGlassProcessing temp
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
}