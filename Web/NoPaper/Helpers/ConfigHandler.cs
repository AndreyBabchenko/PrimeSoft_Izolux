using log4net;
using System;
using System.Configuration;
using System.Web;

namespace NoPaper.Helpers
{
  public class ConfigHandler : IHttpHandler
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(ConfigHandler));

    /// <summary>
    /// You will need to configure this handler in the Web.config file of your 
    /// web and register it with IIS before being able to use it. For more information
    /// see the following link: https://go.microsoft.com/?linkid=8101007
    /// </summary>
    #region IHttpHandler Members

    public bool IsReusable
    {
      // Return false in case your Managed Handler cannot be reused for another request.
      // Usually this would be false in case you have some state information preserved per request.
      get { return true; }
    }

    public void ProcessRequest(HttpContext context)
    {
      try
      {

        log.Info("Запуск обработчика");

        context.Response.ContentType = "application/javascript";

        var maxLength = ConfigurationManager.AppSettings["MaxPyramidBarCodeLength"];
        var rulesJson = ConfigurationManager.AppSettings["BarCodeRules"];
        var shipRulesJson = ConfigurationManager.AppSettings["ShipRules"];

        log.Info($"MaxPyramidBarCodeLength = {maxLength}");

        context.Response.Write($@"
                window.appConfig = window.appConfig || {{}};
                window.appConfig.maxPyramidBarCodeLength = {maxLength};
                window.appConfig.rules = {rulesJson};
                window.appConfig.shipRules = {shipRulesJson};
        ");
      }
      catch (Exception ex)
      {
        log.Error("Ошибка в Обработчике", ex);
        throw;
      }
    }

    #endregion
  }
}
