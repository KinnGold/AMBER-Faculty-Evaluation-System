using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace AMBER.Super_Admin
{
    public partial class OverallRankings : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Super%20Admin/SuperAdminLogin.aspx");
            }
            if (!IsPostBack)
            {
                populateDDL();
                DropDownDepartment.Enabled = false;
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM SCHOOL_TABLE ORDER BY school_name ASC";
                            DropDownSchool.DataValueField = "Id";
                            DropDownSchool.DataTextField = "school_name";
                            DropDownSchool.DataSource = cmd.ExecuteReader();
                            DropDownSchool.DataBind();
                            DropDownSchool.Items.Insert(0, new ListItem("ALL", ""));
                            DropDownSchool.Items.Insert(0, new ListItem("Select School", ""));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
            }
        }

        void populateDDL()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM DEPARTMENT_TABLE WHERE SCHOOL_ID='"+ DropDownSchool.SelectedValue +"' ORDER BY dept_name ASC";
                        DropDownDepartment.Enabled = true;
                        DropDownDepartment.DataValueField = "dept_id";
                        DropDownDepartment.DataTextField = "dept_name";
                        DropDownDepartment.DataSource = cmd.ExecuteReader();
                        DropDownDepartment.DataBind();
                        DropDownDepartment.Items.Insert(0, new ListItem("ALL", ""));
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            DropDownDepartment.Items.Add("Select Department");
            DropDownDepartment.Items.FindByText("Select Department").Selected = true;
        }

        protected void GVbind()
        {
            var test = totalEvaluator();
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT evaluatee_id, INSTRUCTOR_TABLE.dept_id, profile_picture, (''+INSTRUCTOR_TABLE.fname+' '+ INSTRUCTOR_TABLE.mname+' '+ INSTRUCTOR_TABLE.lname +'') AS NAME, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT)))AS AVERAGE, CASE WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON EVALUATION_TABLE.evaluatee_id = INSTRUCTOR_TABLE.Id WHERE INSTRUCTOR_TABLE.dept_id='"+ DropDownDepartment.SelectedValue +"' isDeleted IS NULL GROUP BY INSTRUCTOR_TABLE.dept_id, profile_picture, (''+INSTRUCTOR_TABLE.fname+' '+ INSTRUCTOR_TABLE.mname+' '+ INSTRUCTOR_TABLE.lname +''), evaluatee_id ORDER BY AVERAGE DESC", db);

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

        protected void DropDownSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateDDL();
        }

        protected void gvranks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = Convert.ToInt32(e.Row.Cells[0].Text);
                string imageUrl = "data:image/png;base64," + image(id);
                (e.Row.FindControl("Image1") as Image).ImageUrl = imageUrl;
            }
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
                    cmd.CommandText = "SELECT profile_picture FROM INSTRUCTOR_TABLE WHERE id = '" + id + "'";
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
            return (stud + ins);
        }

        protected void DropDownDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownDepartment.SelectedItem.Text == "ALL")
            {
                gvranks.EditIndex = -1;
                GVbind();
            }
            else if (DropDownDepartment.SelectedItem.Text == "Select Department")
            {
                gvranks.EditIndex = -1;
                populateDDL();
            }
            else
            {
                try
                {
                    DataTable dt = new DataTable();
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT evaluatee_id, INSTRUCTOR_TABLE.dept_id, profile_picture, (''+INSTRUCTOR_TABLE.fname+' '+ INSTRUCTOR_TABLE.mname+' '+ INSTRUCTOR_TABLE.lname +'') AS NAME, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT)))AS AVERAGE, CASE WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON EVALUATION_TABLE.evaluatee_id = INSTRUCTOR_TABLE.Id WHERE INSTRUCTOR_TABLE.dept_id='"+ DropDownDepartment.SelectedValue +"' AND isDeleted IS NULL GROUP BY INSTRUCTOR_TABLE.dept_id, profile_picture, (''+INSTRUCTOR_TABLE.fname+' '+ INSTRUCTOR_TABLE.mname+' '+ INSTRUCTOR_TABLE.lname +''), evaluatee_id ORDER BY AVERAGE DESC";
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                        }
                        db.Close();
                        gvranks.DataSource = dt;
                        gvranks.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
            }
        }
    }
}