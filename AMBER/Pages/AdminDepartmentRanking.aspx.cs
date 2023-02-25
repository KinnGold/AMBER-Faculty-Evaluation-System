using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace AMBER.Pages
{
    public partial class AdminDepartmentRanking : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("LoginPage.aspx");

            }
            if (!IsPostBack)
            {
                GVbind();
            }
        }
        protected void GVbind()
        {
            var test = totalEvaluator();
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT DEPARTMENT_TABLE.dept_Id,DEPARTMENT_TABLE.dept_code, DEPARTMENT_TABLE.dept_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT)))AS AVERAGE, CASE WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id = INSTRUCTOR_TABLE.dept_id WHERE EVALUATION_TABLE.school_id = '" + Session["school"].ToString() + "' GROUP BY DEPARTMENT_TABLE.dept_name, DEPARTMENT_TABLE.dept_code, DEPARTMENT_TABLE.dept_Id ORDER BY AVERAGE DESC", db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if(ds != null)
                {
                    plcNoData.Visible = false;
                    plcData.Visible = true;
                }
                else
                {
                    plcNoData.Visible = true;
                    plcData.Visible = false;
                }
                gvranks.DataSource = ds;
                gvranks.DataBind();
                //SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows == true)
                //{
                //    gvranks.DataSource = dr;
                //    gvranks.DataBind();
                //}
            }
        }

        protected void gvranks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = Convert.ToInt32(e.Row.Cells[0].Text);
                Label rankAverage = (Label)e.Row.FindControl("lblAVERAGE");
                rankAverage.Text = getWeightedMean(id);
            }
        }

        public string getWeightedMean(int department)
        {
            string weightedMean = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) AS weightedMean FROM (SELECT constructor_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE, weight,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) * weight AS 'weightedSum' FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id = INDICATOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE INSTRUCTOR_TABLE.dept_id = '" + department + "' AND evaluator_id != evaluatee_id AND EVALUATION_TABLE.school_id = '" + Session["school"].ToString() + "' GROUP BY constructor_name,weight) as weight";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            weightedMean = reader["weightedMean"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return weightedMean;
        }


        protected void gvranks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvranks.PageIndex = e.NewPageIndex;
            GVbind();
        }
        protected int totalEvaluator()
        {
            int stud = 0, ins = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {

                    using (var cmd = db.CreateCommand())
                    {
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM INSTRUCTOR_TABLE";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            ins = Int32.Parse(reader["COUNT"].ToString());
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM STUDENT_TABLE";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            stud = Int32.Parse(reader["COUNT"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return (stud + ins);
        }
    }
}