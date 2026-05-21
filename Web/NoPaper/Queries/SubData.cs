namespace NoPaper.Queries
{
  internal class SubData
  {
    public static string GetInnerJoinReverseGlassInPackByIdBarCode(int idSector) =>
      $@"inner join 
       (
         select
           idBarCode as id_BarCode,
           row_number() over (partition by idGlassProcessingPyramid order by nGlassInPack desc) as nGlassInPackReverse
         from #TempGlassProcessing
         where idSectorManufact = {idSector}
       ) SubQuery on SubQuery.id_BarCode = temp.idBarCode";

    public static string GetInnerJoinReverseGlassInPackByIdGlassDetails(int idSector) =>
      $@"inner join 
       (
         select
           idGlassDetails,
           row_number() over (partition by idGlassProcessingPyramid order by nGlassInPack desc) as nGlassInPackReverse
         from #TempGlassProcessing
         where idSectorManufact = {idSector}
       ) SubQuery on SubQuery.idGlassDetails = temp.idGlassDetails";
  }
}
