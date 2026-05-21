using NoPaper.Models;
using System;

namespace NoPaper
{
  public partial class CustomError : PageModel
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Exception ex = Server.GetLastError();

      if (ex != null)
      {
        #if DEBUG
          ErrorDetails.Text    = $"<pre>{ex.Message}<br/>{ex.StackTrace}</pre>";
          ErrorDetails.Visible = true;
        #endif

        // Очистим ошибку
        Server.ClearError();
      }
    }
  }
}
