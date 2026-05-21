using NoPaper.Controllers;
using NoPaper.Models;
using NoPaper.Queries;
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
  public partial class TaskSearchInfo : PageModel
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        LoadDataGrid();
      }
      else
      {
        
      }
    }
    protected void LoadDataGrid()
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          TaskData task = new TaskData(accountNum: AccountNum_TextBox.Text, bShowFinishedGlass_CheckBox.Checked, bShowComplitedGlass_CheckBox.Checked);

          TaskController taskStateController = new TaskController(conn, task);
          DataTable dataTable = taskStateController.GetTaskTable();
          TaskGrid.DataSource = dataTable;
          TaskGrid.DataBind();
        }
      }
      catch
      { }
    }

    protected void ButBegin_Click(object sender, EventArgs e)
    {
      LoadDataGrid();
    }
  }
}