using NoPaper.Models;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NoPaper.Queries
{
  public class TaskData
  {
    public string AccountNum;          // Номер счета 
    public bool   bShowFinishedGlass;  // Показывать готовые изделя для перемещения
    public bool   bShowCompletedGlass; // Показать изготавливаемые

    public TaskData(string accountNum, bool _bShowFinishedGlass, bool _bShowCompletedGlass)
    {
      AccountNum       = accountNum;
      bShowFinishedGlass  = _bShowFinishedGlass;
      bShowCompletedGlass = _bShowCompletedGlass;
    }

    public TaskData(TaskData t)
    {
      AccountNum        = t.AccountNum;
    }

    public TaskData()
    {
      AccountNum          = null;
      bShowFinishedGlass  = true;
      bShowCompletedGlass = true;
    }

    /// <summary>
    /// Запрос на получение данных на прямую из вьюхи
    /// </summary>

    /*
    public string GetData()
    {
      return $@"select 
                 NameSawTask,
                 AccountNum,
                 NameGlass,
                 NameSectorManufact,
                 CurrentPyramidBarCode,
                 Height,
                 Width,
                 count (1) nCountGlass
               from 
                 v_SawTaskGlassProcessing
               where AccountNum = '{AccountNum}' and
                       bFinished != 1            and
                       CurrentPyramidBarCode is not null
               group by
                 NameSawTask,
                               AccountNum,
                 NameGlass,
                 NameSectorManufact,
                 CurrentPyramidBarCode,
                 Height,
                 Width";
    }
    */


    /// <summary>
    ///   Получение данных из временной таблицы по введенному заказу
    /// </summary>
    /// <returns>
    ///   Возвращаем все стекла которые находятся на резке и
    ///   которые готовы или находятся ждут перемещения на новый участок
    /// </returns>

    public string GetData(int SPID)
    {
      string query,
             sWhere = "";
      bool xorResult = bShowFinishedGlass ^ bShowCompletedGlass;

      if (!xorResult) // Если значения чек боксов одинаковы
      {
        if (!bShowFinishedGlass && !bShowCompletedGlass)
          sWhere += " and ReadyDateTime is null and ReceiveDateTime is null";
      }
      else // Если значения чек боксов отличаются
      {
        if (bShowFinishedGlass)
          sWhere += " and ReadyDateTime is not null";

        if (bShowCompletedGlass)
          sWhere += " and ReceiveDateTime is not null";
      }

      query = $@"select 
                   NameSawTask,
                   NameGlassProduct,
                   AccountNum,
                   NameSectorManufact,
                   CurrentPyramidBarCode,
                   PreviousPyramidBarCode,
                   ReceiveDateTime,
                   ReadyDateTime,
                   Height,
                   Width,
                   count (1) nCountGlass
                 from 
                   ##TempGlassProcessing{SPID}
                 where AccountNum               = '{AccountNum}'    and
                       IsNull(bFinishedPrev, 1) = 1                 
                       {sWhere}
                 group by
                   NameSawTask,
                   NameGlassProduct,
                   AccountNum,
                   NameSectorManufact,
                   CurrentPyramidBarCode,
                   PreviousPyramidBarCode,
                   ReadyDateTime,
                   ReceiveDateTime,
                   Height,
                   Width";

      return query;
    }
  }
}