namespace NoPaper.Models
{
  enum e_TypeSectorManuf           // Тип Оборудования, он же тип участка, он же SectorManufact.nType
  {
    e_tsm_none               =  0, // не определен
    e_tsm_Saw                =  1, // Раскрой
    e_tsm_Assembly           =  2, // Сборка
    e_tsm_Furnace            =  3, // Печка
    e_tsm_Triplex            =  4, // Триплекс
    e_tsm_ProcCenter         =  5, // Обр.Центр
    e_tsm_Drill              =  6, // Сверление
    e_tsm_Facet              =  7, // Фацет
    e_tsm_Blunt              =  8, // Притупление
    e_tsm_Wash               =  9, // Мойка
    e_tsm_Paint              = 10, // Покраска
    e_tsm_Film               = 11, // Ламинация\плёнка
    e_tsm_Assembly_Frame     = 12, // Сборка рамки
    e_tsm_Polish             = 13, // Полировка
    e_tsm_SandBlast          = 14, // Пескоструй
    e_tsm_UV_Gluing          = 15, // УФ склейка
    e_tsm_Packing            = 16, // Упаковка
    e_tsm_PackingManuf       = 17, // Создание упаковочной тары
    e_tsm_Shpros             = 18  // Производство шпрос
  };

  internal struct GlassProcessingInfo
  {
    public int IdGlassDetails;
    public int IdGlassProcessingPyramid;
    public int IdSectorManufact;

    public GlassProcessingInfo(int _idGlassDetails, int _idGlassProcessingPyramid, int _idSectorManufact)
    {
      IdGlassDetails = _idGlassDetails;
      IdGlassProcessingPyramid = _idGlassProcessingPyramid;
      IdSectorManufact = _idSectorManufact;
    }
  }

}