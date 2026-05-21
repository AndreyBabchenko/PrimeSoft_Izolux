namespace NoPaper.Models
{
  public static class СolumnsModelGridOper
  {
    static string[] GetColumnsLabel()
    {
      return new string[]
      {
        "NameSawTask",               // Оптимизация
        "NameGlassProduct",          // Изделие
        "AccountNum",                // Заказ
        "NameEquipment",             // Оборудование
        "ProjectNum",                // Позиция Заказа   
        "PiramidOrder",              // Позиция Сборки
        "Width",                     // Размер
        "Height",                    
        "TimeBeginProcessing",       // Время Начала 
        "SectorNext",                // След. Участок     
        "ForCount",                  // Количество
        "PiramidNumLabel",           // Пирамида
        "PreviousPyramidBarCode",    // Штрих код пред. пирамиды
        "CurrentPyramidBarCode",     // Штрих код пирамиды
        "PiramidSide",               // Сторона
        "nPack",                     // Пачка
        "PiramidCell",               // Ячейка
        "nGlassInPack",              // № в пачке поставить
        "nGlassInPackTake"           // № в пачке взять
      };
    }

    static string[] GetColumnsButtons()
    {
      return new string[]
      {
        "MakePyramidButton",           // Пирамида
        "WritePyramidBarCodeButton",   // Штрих код пирамиды
        "MakeOperButton"               // Готова | Собрано
      };
    }
  }
}