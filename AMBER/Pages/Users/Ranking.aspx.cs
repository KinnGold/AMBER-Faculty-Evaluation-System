using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace AMBER.Pages.Users
{
    public partial class Ranking : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GVbind();
                try
                {
                    using (SqlConnection db = new SqlConnection(connDB))
                    {
                        db.Open();
                        SqlCommand cmd = new SqlCommand("SELECT evaluatee_id, INSTRUCTOR_TABLE.dept_id,profile_picture,(''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') AS NAME,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL GROUP BY INSTRUCTOR_TABLE.dept_id, profile_picture,(''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+''),evaluatee_id ORDER BY AVERAGE DESC", db);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            plcNoData.Visible = false;
                            plcData.Visible = true;
                        }
                        else
                        {
                            plcNoData.Visible = true;
                            plcData.Visible = false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
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

        protected void GVbind()
        {
            var test = totalEvaluator();
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT evaluatee_id, INSTRUCTOR_TABLE.dept_id, profile_picture, (''+INSTRUCTOR_TABLE.fname+' '+ INSTRUCTOR_TABLE.mname+' '+ INSTRUCTOR_TABLE.lname +'') AS NAME, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT)))AS AVERAGE, CASE WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON EVALUATION_TABLE.evaluatee_id = INSTRUCTOR_TABLE.Id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL AND INSTRUCTOR_TABLE.dept_Id = '" + Session["dept"].ToString() + "' GROUP BY INSTRUCTOR_TABLE.dept_id, profile_picture, (''+INSTRUCTOR_TABLE.fname+' '+ INSTRUCTOR_TABLE.mname+' '+ INSTRUCTOR_TABLE.lname +''), evaluatee_id ORDER BY AVERAGE DESC", db);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
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
                string imageUrl = "data:image/png;base64," + image(id);
                (e.Row.FindControl("Image1") as Image).ImageUrl = imageUrl;

                int dept = Convert.ToInt32(e.Row.Cells[1].Text);
                Label rankAverage = (Label)e.Row.FindControl("lblAVERAGE");
                rankAverage.Text = getWeightedMean(id, dept);
            }
        }
        public string getWeightedMean(int instructor, int department)
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
                        cmd.CommandText = "SELECT CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) AS weightedMean FROM (SELECT constructor_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE, weight,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) * weight AS 'weightedSum' FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id = INDICATOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE INSTRUCTOR_TABLE.dept_id= '" + department + "' AND INSTRUCTOR_TABLE.Id= '" + instructor + "' AND evaluator_id != evaluatee_id AND EVALUATION_TABLE.school_id = '" + Session["school"].ToString() + "' GROUP BY constructor_name,weight) as weight";
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

        public string image(int id)
        {
            string profilePicture = "";
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT profile_picture FROM INSTRUCTOR_TABLE WHERE id = '" + id + "' AND school_id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader["profile_picture"] != System.DBNull.Value)
                        {
                            byte[] bytes = (byte[])reader["profile_picture"];
                            profilePicture = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
            return profilePicture;
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
            return (stud+ins);
        }
    }
}