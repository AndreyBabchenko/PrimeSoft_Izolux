namespace NoPaper.Models
{
  internal class CombinationReject
  {
    public int IDRejectAct,    // Вид акта
               IDRejectPlace,  // Место обнаружения
               IDRejectType,   // Вид брака
               IDReject,       // Наименование Брака
               IDTypeExpense;  // Вид издержек
    public string CommentReject;  // Комментарий брака
  }
}
