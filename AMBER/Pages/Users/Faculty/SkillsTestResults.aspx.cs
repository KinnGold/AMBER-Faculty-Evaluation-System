using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace AMBER.Pages.Users.Faculty
{
    public partial class SkillsTestResults : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Pages/Users/UsersLogin.aspx");
            }
            if (!IsPostBack)
            {
                GVbind();
                overallresult();
                GVBindOverall();
                GVbindResult();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AutoRedirect();
        }

        public void AutoRedirect()
        {
            int int_MilliSecondsTimeOut = this.SessionLengthMinutes * 60000;
            var sch_id = Session["school"].ToString();
            string str_Script = @"
               <script type='text/javascript'> 
                   intervalset = window.setInterval('Redirect()'," +
                       int_MilliSecondsTimeOut.ToString() + @");
                   function Redirect()
                   {
                       window.location.href='https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(sch_id) + @"';
                   }
               </script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", str_Script);
        }

        public int SessionLengthMinutes
        {
            get { return Session.Timeout; }
        }

        protected void GVbindResult()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + Session["insID"].ToString() + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridviewGeneralResult.DataSource = dr;
                    GridviewGeneralResult.DataBind();
                    plcerror.Visible = false;
                    plcgv.Visible = true;
                }
                else
                {
                    plcgv.Visible = false;
                    plcerror.Visible = true;
                }
            }
        }

        protected void GVbind()
        {
            var camp_id = Session["school"].ToString();
            var ins = Session["Id"].ToString();
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + Session["insID"].ToString() + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id", db); ;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GVResults.DataSource = dr;
                    GVResults.DataBind();
                    plcerror.Visible = false;
                    plcgv.Visible = true;
                }
                else
                {
                    plcgv.Visible = false;
                    plcerror.Visible = true;
                }
            }
        }


        public void overallresult()
        {
            float res = 0;
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + Session["insID"].ToString() + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id";
                    cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            var temp = Convert.ToSingle(reader["WEIGHTED_MEAN"].ToString());
                            res += Convert.ToSingle(reader["WEIGHTED_MEAN"].ToString());
                            hfRESULT.Value = res.ToString("0.00");

                        }
                        plcerror.Visible = false;
                        plcgv.Visible = true;
                    }
                    else
                    {
                        plcgv.Visible = false;
                        plcerror.Visible = true;
                    }
                }
            }
        }

        protected void GVBindOverall()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT 'OVERALL WEIGHTED MEAN' AS description,'" + hfRESULT.Value + "' AS result, CASE WHEN '" + hfRESULT.Value + "' BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN '" + hfRESULT.Value + "' BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN '" + hfRESULT.Value + "' BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN '" + hfRESULT.Value + "' BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN '" + hfRESULT.Value + "' BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridviewResults.DataSource = dr;
                    GridviewResults.DataBind();
                    plcerror.Visible = false;
                    plcgv.Visible = true;
                }
                else
                {
                    plcgv.Visible = false;
                    plcerror.Visible = true;
                }
            }
        }

        protected void GVResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvIndicator = (GridView)e.Row.FindControl("GridView2");
            int constructor = Int32.Parse(GVResults.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY INDICATOR_TABLE.indicator_id),CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name, INDICATOR_TABLE.indicator_name,INDICATOR_TABLE.indicator_id,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + Session["insID"].ToString() + "' AND INDICATOR_TABLE.constructor_id='" + constructor + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, INDICATOR_TABLE.indicator_id, CONSTRUCTOR_TABLE.constructor_name, INDICATOR_TABLE.indicator_name, CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvIndicator.DataSource = dr;
                    gvIndicator.DataBind();
                }
            }
        }
    }
}