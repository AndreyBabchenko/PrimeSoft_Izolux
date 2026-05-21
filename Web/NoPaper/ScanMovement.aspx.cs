using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;


namespace NoPaper
{
  public partial class ScanMovement : System.Web.UI.Page
  {
    public static int  m_idWorkZone_StandBy = 0,
                       m_eModeScan          = 0,
                       m_nOrderSequence     = 0;

    public static int idOperator = 0,
               idDepName_Debet = 0,
               idDepName_Credit = 0,
               idBarCode = 0,
               idDepTrans = 0;

    protected void InputBarcode(object sender, EventArgs e)
    {
      string barcodeValue = barcode.Text,
             OperatorID = Operator.SelectedValue,
             DepName_Debet_ID = DepName_Debet.SelectedValue,
             DepName_Credit_ID = DepName_Credit.SelectedValue;


      if ( OperatorID != "" )
        idOperator = Int32.Parse(OperatorID);


      if ( DepName_Debet_ID != "")
        idDepName_Debet = Int32.Parse(DepName_Debet_ID);

      if (DepName_Credit_ID != "")
        idDepName_Credit = Int32.Parse(DepName_Credit_ID);

      // Не сканируем пустоту
      if (barcodeValue.Length == 0)
        return;

      // Скан последовательности
      if ( barcodeValue.Substring(0, 11) == "GPTRANSFERB")
      {
        CreateDepTrans();
        
        barcode.Text = "";
        return;
      }

      // Скан последовательности
      if (barcodeValue.Substring(0, 11) == "GPTRANSFERE")
      {
        idDepTrans = 0;

        barcode.Text = "";
        return;
      }

      if ( idDepTrans == 0 )
        CreateDepTrans();


      if (barcodeValue.Substring(0, 2) == "SM" && barcodeValue.Length == 12 && idDepTrans != 0 && idOperator !=  0 )
      {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();

          string sSQL = String.Format("select * from dbo.f_FindBarCodeDepot('{0}')", barcodeValue);
          SqlCommand sqlBarCode = new SqlCommand(sSQL, conn);

          using (SqlDataReader reader = sqlBarCode.ExecuteReader())
          {
            if (reader.Read())
            {
              idBarCode = Int32.Parse(reader["idBarCode"].ToString());
            }
          }

            string     SQLCommand = String.Format("exec sp_AppendDepTrans_BarCode {0}, {1}, {2}", idDepTrans, idBarCode, idOperator);
          SqlCommand command    = new SqlCommand(SQLCommand, conn);

          command.ExecuteNonQuery();

          SqlCommand_GetScanLogs(conn, OperatorID);
        }

        barcode.Text = "";
      }
    }

    [WebMethod]
    public void CreateDepTrans()
    {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
      {
        conn.Open();

        //string sValue = "";

        string     SQLCommand = String.Format("exec sp_AddDepTrans {0}, {1}, {2}, @idDepTrans output  ", 5, idDepName_Debet, idDepName_Credit);
        SqlCommand command    = new SqlCommand(SQLCommand, conn);
        
        //command.CommandType = System.Data.CommandType.StoredProcedure;

        SqlParameter pidDepTrans = command.Parameters.Add("@idDepTrans", System.Data.SqlDbType.VarChar, 32);
        pidDepTrans.Direction = System.Data.ParameterDirection.Output;

        command.ExecuteNonQuery();

        string value = Convert.IsDBNull(pidDepTrans.Value) ? null : (string)pidDepTrans.Value;

        idDepTrans = Int32.Parse(value);

        SqlCommand_GetScanLogs(conn, idOperator.ToString());
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
        {
          conn.Open();

          SqlCommand command = new SqlCommand("select ID, Name from Operator where isNull(bTransfer, 0) <> 0 order by Name", conn);

          using (SqlDataReader reader = command.ExecuteReader())
          {
            ListItem listItem = new ListItem();

            listItem.Text = "";
            listItem.Value = "0";

            Operator.Items.Add("");

            while (reader.Read())
            {
                listItem = new ListItem();

                listItem.Text  = reader["Name"].ToString();
                listItem.Value = reader["ID"].  ToString();

                Operator.Items.Add(listItem);
            }
          }

          SqlCommand rcDepName = new SqlCommand("select * from DepName where nType = 2 order by Name", conn);

          using (SqlDataReader reader1 = rcDepName.ExecuteReader())
          {
            ListItem listItem = new ListItem();

            while (reader1.Read())
            {
              listItem = new ListItem();

              listItem.Text  = reader1["Name"].ToString();
              listItem.Value = reader1["ID"].ToString();

              DepName_Debet.Items.Add(listItem);
              DepName_Credit.Items.Add(listItem);
            }
          }
        }

      }
    }

    protected void Operator_SelectedIndexChanged(object sender, EventArgs e)
    {
      string OperatorID = Operator.SelectedValue;

      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlassConnectionString"].ConnectionString))
      {
          conn.Open();

          SqlCommand_GetScanLogs(conn, OperatorID);
      }
    }

    protected void DepName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void SqlCommand_GetScanLogs(SqlConnection conn, string OperatorID)
    {
      string     SQLCommand = String.Format("select * from v_ScanLog where idOperator = {0} and nSource = 4 order by TimeScan desc", OperatorID);
      SqlCommand command    = new SqlCommand(SQLCommand, conn);
      
      using (SqlDataReader reader = command.ExecuteReader())
      {
          OperationTable.DataSource = reader;
          OperationTable.DataBind();
      }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

  }
}

