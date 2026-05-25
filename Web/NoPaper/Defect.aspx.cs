using log4net;
using NoPaper.Controllers;
using NoPaper.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

namespace NoPaper
{
  public partial class Defect : PageModel
  {
    private static SqlConnection _conn;
    private static bool          bModal = false;
    private static readonly ILog log = LogManager.GetLogger(typeof(workplace));

    private void LoadItemsToDropDownList()
    {
      Dictionary<DropDownList, SqlDataSource> dropDownListMappings = new Dictionary<DropDownList, SqlDataSource>() // Словарь для связки значений с нужным SQLDataSource
      {
        { IDRejectAct,        SqlDSRejectAct     },
        { IDRejectPlace,      SqlDSRejectPlace   },
        { IDRejectType,       SqlDSRejectType    },
        { IDReject,           SqlDSReject        },
        { IDTypeExpense,      SqlDSTypeExpense   },
        { IDPersonnel_Guilty, SQLPersonnelGuilty }
      };

      foreach (var kvp in dropDownListMappings)
      {
        DropDownList  ddl           = kvp.Key;
        SqlDataSource sqlDataSource = kvp.Value;

        ddl.DataSource     = sqlDataSource;
        ddl.DataTextField  = "Name"; 
        ddl.DataValueField = "ID"; 
        ddl.DataBind(); // Привязываем данные

        // Добавление прочерка в начало каждого DropDownList
        ddl.Items.Insert(0, new ListItem("-", "0"));
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        // Читаем barcode из QueryString
        string barcode = Request.QueryString["barcode"],
               mode    = Request.QueryString["mode"],

               sFilter = "idUser is not null";

        LoadOperatorList(ddOperatorList, "ID", "Name", sFilter); // Грузим только тех Операторов у которых есть ссылка на Пользователя
        LoadItemsToDropDownList();

        bModal = !string.IsNullOrEmpty(mode);

        if (bModal && !string.IsNullOrEmpty(barcode))
        {
          Control form = FindControl("FormDefect");
          TextBox barCodeGlass = (TextBox)form.FindControl("DefectBarCode");

          barCodeGlass.Text    = barcode;
          barCodeGlass.Enabled = false;
        }
      }
    }

    private static readonly ConcurrentDictionary<string, bool> _processingBarcodes = new ConcurrentDictionary<string, bool>();

    protected async void DefectButton_Click(object sender, EventArgs e)
    {
      log.Info("Создание брака");

      Control form = FindControl("FormDefect");
      TextBox barCodeGlass = (TextBox)form.FindControl("DefectBarCode");
      string barCode = barCodeGlass.Text;

      // Сразу затрем штрихкод
      if (!bModal)
        barCodeGlass.Text = "";

      // Если проверяем может уже нажали обработать этот штрихкод
      if (!_processingBarcodes.TryAdd(barCode, true))
      {
        log.Warn($"Штрихкод уже обрабатывается: {barCode}");
        return;
      }

      // Если значения проинициализировались
      if (ddOperatorList.SelectedIndex != -1)
        _currentOperatorInfo = _operatorInfoList[ddOperatorList.SelectedIndex]; // Указываем текущего выбранного пользователя

      if (!string.IsNullOrEmpty(barCode))
      {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          BarCodeController barCodeController;
          
          CombinationReject combination = new CombinationReject
          {
            IDRejectAct   = SafeConvert.ToInt(IDRejectAct  .SelectedValue ),
            IDRejectPlace = SafeConvert.ToInt(IDRejectPlace.SelectedValue ),
            IDRejectType  = SafeConvert.ToInt(IDRejectType .SelectedValue ),
            IDReject      = SafeConvert.ToInt(IDReject     .SelectedValue ),
            IDTypeExpense = Math.Max(SafeConvert.ToInt(IDTypeExpense.SelectedValue), 1),
            CommentReject= SafeConvert.ToString(IDRejectComment.Text)
          };
           barCodeController = new BarCodeController(conn, barCode, combination);

          // Проверяем проинициализированн ли баркод
          if (barCodeController.IsInitializeBarCode() || barCodeController.IsInitializeBarCodeGlass())
          {
            log.Info("штрихкод изделия на брак найден");

            if (barCodeController.IsInitializeCombination())
             await barCodeController.CheckWriteCombination();

            int SPID              = barCodeController.GetSPID,
                idPersonnel_Guilty = SafeConvert.ToInt(IDPersonnel_Guilty.SelectedValue); // id виновного оператора

            await barCodeController.SetDefectByBarCode
                                   (
                                     conn.ConnectionString,
                                     SPID,
                                     barCodeController.GetBarCodeInfo,
                                     barCodeController.GetBarCodeGlassInfo,
                                     _currentOperatorInfo,
                                     idPersonnel_Guilty
                                   );
            ShowMessage("Операция успешна", true);
          }
          else ShowMessage("Операция не выполнена", true);
        }
      }

      if ( bModal )
      {
        // сообщаем родителю закрыться, если модальное окно
        string script = @"if (window.parent && window.parent.closeRejectModal) 
                              window.parent.closeRejectModal();";
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseRejectModal", script, true);
      }
 
      // Удалим из колекции 
      _processingBarcodes.TryRemove(barCode, out _);      
    }

    protected void RedirectToWorkplace(object sender, EventArgs e)
    {
      Response.Redirect("workplace.aspx");
    }
  }
}