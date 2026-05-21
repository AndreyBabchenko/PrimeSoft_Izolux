using Newtonsoft.Json;
using NoPaper.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

namespace NoPaper
{
  public partial class Plot : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {
        if (!IsPostBack)
        {
          ViewState["idProject"] = Request.Params["idProject"];
          ViewState["bShpros"]   = Request.Params["bShpros"];
        }

        int  idProject      = SafeConvert.ToInt(ViewState["idProject"]);
        bool bShpros        = SafeConvert.ToBool(ViewState["bShpros"]); // Шпроссы?
        string json; // Для формирования данных таблицы

      using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
      {
        conn.Open();
          object result = null;

          // Если шпроссы действуем по сценарию отображения шпросс по аналогии с отчетом Чертежи Шпрос
          if (bShpros)
          {
            object info         = null;
            object shprossInfo  = null;
            List<FrameSize> frames  = new List<FrameSize>();
            List<ShprosInfo> shpros = new List<ShprosInfo>();
            string guidProject;

            SqlCommand command = new SqlCommand(@"select 
                                                    ClientID, 
                                                    ClientName, 
                                                    DateComplite, 
                                                    TaskAccountNum, 
                                                    PosNum, 
                                                    nCountGP, 
                                                    Width, 
                                                    Height,
                                                    F1, 
                                                    F2, 
                                                    nCountRam, 
                                                    Argon1, 
                                                    Argon2, 
                                                    GPName,
                                                    
                                                    bShpros,
                                                    ProjectRasWidth,
                                                    ProjectRasColor,
                                                    ProjectRasColorIns,
                                                    
                                                    guidProject,
                                                    plot
                                                    
                                                  from v_stis_FrameReport_Select 
                                                  where idProject = @idProject",
                                                  conn);
            command.Parameters.AddWithValue("@idProject", idProject);

            // Общая таблица
            using (SqlDataReader reader = command.ExecuteReader())
            {
              if (!reader.Read())
                return;

              info = new
              {
                bShpros      = true,
                ClientID     = SafeConvert.ToString(reader["ClientID"]),
                ClientName   = SafeConvert.ToString(reader["ClientName"]),
                DateComplite = SafeConvert.ToString(reader["DateComplite"]),

                AccountNum   = SafeConvert.ToString(reader["TaskAccountNum"]),
                PosNum       = SafeConvert.ToString(reader["PosNum"]),
                nCountGP     = SafeConvert.ToString(reader["nCountGP"]),

                Size         = $"{SafeConvert.ToString(reader["Width"])} x {SafeConvert.ToString(reader["Height"])}".Trim(),

                Frame        = $"{SafeConvert.ToString(reader["F1"])} {SafeConvert.ToString(reader["F2"])}".Trim(),
                FrameCount   = SafeConvert.ToString(reader["nCountRam"]),

                Gas          = $"{SafeConvert.ToString(reader["Argon1"])} {SafeConvert.ToString(reader["Argon2"])}".Trim(),
                Formula      = SafeConvert.ToString(reader["GPName"]),

                Layout       = $"{SafeConvert.ToString(reader["ProjectRasWidth"])}, нар.:{SafeConvert.ToString(reader["ProjectRasColor"])}, вн:{SafeConvert.ToString(reader["ProjectRasColorIns"])}",

                GuidProject  = SafeConvert.ToString(reader["guidProject"]),
              };


              guidProject = SafeConvert.ToString(reader["guidProject"]);

              result = reader["plot"];

              json = JsonConvert.SerializeObject(info);
            }

            // данные о рамке
            command.CommandText = $"select * from v_FrameSeg_SawTask 	where IsNull(bShpros, 0) = 1 and idProject = {idProject}";
            using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              int    num      = SafeConvert.ToInt   (reader["Num"]);
              double leng     = SafeConvert.ToDouble(reader["Leng"]),
                     lengReal = SafeConvert.ToDouble(reader["LengReal"]),
                     R        = SafeConvert.ToDouble(reader["R"]),
                     R_Real   = SafeConvert.ToDouble(reader["R_Real"]);

              FrameSize frame = new FrameSize(num, leng, lengReal, R, R_Real);
              frames.Add(frame);
            }
          }

            // общие данные о шпросах
            command.CommandText = $"select top 1 * from v_RasShrink_SawTask \twhere IsNull(bShpros, 0) = 1 and guidProject = '{guidProject}'";
            using (SqlDataReader reader = command.ExecuteReader())
          {
            if(reader.Read())
            {
                shprossInfo = new
                {
                  ColorName        = SafeConvert.ToString(reader["ColorName"]),
                  ColorInsName     = SafeConvert.ToString(reader["ColorInsName"]),
                  WidthName        = SafeConvert.ToString(reader["WidthName"]),
                  Manufacter       = SafeConvert.ToString(reader["Manufacter"]),
                  CameraShprosName = SafeConvert.ToString(reader["CameraShprosName"])
                };
              }
            }

            // данные о шпросах в таблицу
            command.CommandText = $"select MarkIdentic,  count(1) as nCount, LengReal,  AngleLef, AngleRig from v_RasShrink_SawTask where IsNull(bShpros, 0) = 1 and guidProject = '{guidProject}' group by MarkIdentic, LengReal,  AngleLef, AngleRig";
            using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              string MarkIdentic = SafeConvert.ToString(reader["MarkIdentic"]);
              int    Count       = SafeConvert.ToInt(reader["nCount"]);
              double LengReal    = SafeConvert.ToDouble(reader["LengReal"]);
              float AngleLef     = SafeConvert.ToFloat(reader["AngleLef"]),
                    AngleRig     = SafeConvert.ToFloat(reader["AngleRig"]);

              ShprosInfo shprosInfo = new ShprosInfo(MarkIdentic, Count, LengReal, AngleLef, AngleRig);
              shpros.Add(shprosInfo);
            }
          }

            var finalJson = new
            {
              info,
              frames,
              shprossInfo,
              shpros
            };

            // Регистрируем объект
            json = JsonConvert.SerializeObject(finalJson);
          }
          else
          {
            SqlCommand command = new SqlCommand("select Plot from v_SawTaskGlassProcessing where idProject = @idProject", conn);
            command.Parameters.AddWithValue("@idProject", idProject);

            result = command.ExecuteScalar();


            var sheetData = new
            {
              info = new
              {
                bShpros = false
              }
            };

            json = JsonConvert.SerializeObject(sheetData);
          }

          // проверяем результат
          if (result == null || result == DBNull.Value)
            return;

          // Инфу вывводим
          ClientScript.RegisterStartupScript(this.GetType(), "sheetData", $"window.sheetData = {json};", true);

          // делаем отрисовку
          string s = Convert.ToBase64String((byte[])result);
          Image1.ImageUrl = "data:image;base64," + s;
        }
      }
      catch
      {

      }
    }

  }
}