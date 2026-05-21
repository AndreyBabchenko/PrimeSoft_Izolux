namespace NoPaper.Queries
{
  static class SawGlassProcessingData
  {
    public static string GetDataBarCode(string barCode, int idSector) =>
      $@"select
          GD.ID as idGlassDetails,
          GP.ID as idGlassProcessing,
          GP.idGlassProcessingPyramid,
          B.ID as idBarCode,
          B.BarCode
        from GlassDetails GD
        inner join ProjectItem     PI on PI.ID = GD.idProjectItem   
        inner join GlassProcessing GP on GD.ID = GP.idGlassDetails
        inner join BarCode         B  on B.ID  = GD.idBarCode
        where BarCode          = '{barCode}'
        and   idSectorManufact =  {idSector}";

    public static string GetDataBarCode_Glass(string barCode, int idSector) =>
      $@"select
           GD.ID as idGlassDetails,
           GP.ID as idGlassProcessing,
           idGlassProcessingPyramid,
           B.ID as idBarCode,
           B.BarCode
         from GlassDetails GD
         inner join ProjectItem     PI on GD.idProjectItem     = PI.ID
         inner join GlassProcessing GP on GD.ID                = GP.idGlassDetails
         inner join BarCode         B  on B.ID                 = GD.idBarCode
         inner join BarCode_Glass   BG on BG.idBarCode         = GD.idBarCode and
                                          BG.idProjectItem     = PI.ID
         where BG.BarCode       = '{barCode}'
         and   idSectorManufact =  {idSector}";


    // Запрос для получения данных внутрицеховой логистики на основании штрихкода и участка
    public static string GetPostDataBarcode(string barCode, int idSector) =>
    $@"select
       GD.ID as idGlassDetails,
       GD.idPiramid,
       GD.idSawTaskMain,
       GP.ID as idGlassProcessing,
       GP.idGlassProcessingPyramid,
       B.ID as idBarCode,
       B.BarCode
     from GlassDetails GD
     inner join ProjectItem     PI on GD.idProjectItem     = PI.ID
     inner join GlassProcessing GP on GD.ID                = GP.idGlassDetails
     inner join BarCode         B  on B.ID                 = GD.idBarCode
     inner join BarCode_Glass   BG on BG.idBarCode         = GD.idBarCode and
                                      BG.idProjectItem     = PI.ID
     where (BG.BarCode      = '{barCode}' or B.BarCode     = '{barCode}')
         and   idSectorManufact =  {idSector}";
  }
}