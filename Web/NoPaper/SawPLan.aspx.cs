using NoPaper.Controllers;
using NoPaper.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NoPaper
{
  public partial class SawPlan : PageModel
  {
    protected void LoadDataGrid()
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          string dateStart = DateTime.Parse(DateTextBoxStart.Text).ToString("dd-MM-yyyy"),
                 dateEnd   = DateTime.Parse(DateTextBoxEnd  .Text).ToString("dd-MM-yyyy");

          SawTaskMainController sawTaskMainController = new SawTaskMainController(conn);
          DataTable dataTable = sawTaskMainController.GetSawTaskTableByFilter(_curentSectorManufact, dateStart, dateEnd);
          PlanSawTaskGrid.DataSource = dataTable;
          PlanSawTaskGrid.DataBind();
        }
      }
      catch
      { }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if ( !IsPostBack )
      {
        DateTextBoxStart.Text = DateTime.Today.ToString("yyyy-MM-dd");
        DateTextBoxEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");

        LoadSectorManufact(ddListSector);
        LoadDataGrid();
      }
      else
      {
        _sectorManufactList = new List<SectorManufactInfo>((List<SectorManufactInfo>)ViewState["SectorManufactList"]);
        _curentSectorManufact = _sectorManufactList[ddListSector.SelectedIndex];
      }
    }

    protected void UpdateSawTaskGrid_StartStatus(string[] paramArguments)
    {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
      {
        SawTaskInfo  sawTask = new SawTaskInfo
        {
          ID          = Convert.ToInt32(paramArguments[0]),
          NameSawTask = paramArguments[1]
        };

        SawTaskMainController sawTaskMainController = new SawTaskMainController(conn, sawTask);
        sawTaskMainController.UpdateStartStatus();
      }
    }

    protected void PlanSawTaskGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      switch (e.CommandName)
      {
        case "OnRedirectButton":
          string parameterValue = e.CommandArgument.ToString() + $"_{_curentSectorManufact.ID}";

          // Передаем параметры в виде массива и обновляем bStart
          UpdateSawTaskGrid_StartStatus(parameterValue.Split('_')); 

          Response.Redirect("workplace.aspx?param=" + parameterValue, false);
          break;
      }
    }

    // Событие при смене элемента sectorManufact
    public void ddListSector_SelectedIndexChanged(object sender, EventArgs e) 
    {
      LoadDataGrid();
    }

    protected void ButBegin_Click(object sender, EventArgs e)
    {
      LoadDataGrid();
    }

    protected void PlanSawTaskGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        int    idSawTask = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ID"));
        string sawName   = DataBinder.Eval(e.Row.DataItem, "Name").ToString();

        Button pyramidButton = (Button)(e.Row.FindControl("RedirectToWorkplace_Button"));

        if (pyramidButton != null)
          pyramidButton.CommandArgument = $"{idSawTask}_{sawName}";
      }
    }
  }
}