using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.Super_Admin
{
    public partial class SectionManagement : System.Web.UI.Page
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
                GVBind();
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
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                DropDownSchool.Items.Add("Select School");
                DropDownSchool.Items.FindByText("Select School").Selected = true;
            }
        }

        public void GVBind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT SECTION_TABLE.section_id, SECTION_TABLE.section_name,SECTION_TABLE.program,  SECTION_TABLE.yearLevel, CASE WHEN program = 1 THEN 'DAY' WHEN program = 0 THEN 'EVENING' ELSE 'INVALID' END AS programdesc, CASE WHEN yearLevel = 1 THEN '1st year' WHEN yearLevel = 2 THEN '2nd year' WHEN yearLevel = 3 THEN '3rd year' WHEN yearLevel = 4 THEN '4th year' ELSE 'INVALID' END AS yearLeveldesc, SECTION_TABLE.isDeleted, SEMESTER_TABLE.semester_id, (SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER,COURSE_TABLE.course_id,(COURSE_TABLE.title+' - '+COURSE_TABLE.description) AS COURSE, SCHOOL_TABLE.Id AS sch_id, SCHOOL_TABLE.school_name FROM SECTION_TABLE JOIN SEMESTER_TABLE ON SECTION_TABLE.semester_id = SEMESTER_TABLE.semester_id JOIN COURSE_TABLE ON SECTION_TABLE.course_id = COURSE_TABLE.course_id JOIN SCHOOL_TABLE ON SECTION_TABLE.school_id = SCHOOL_TABLE.Id", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void DropDownSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownSchool.SelectedItem.Text == "ALL")
            {
                GridView1.EditIndex = -1;
                GVBind();
            }
            else if (DropDownSchool.SelectedItem.Text == "Select School")
            {
                GridView1.EditIndex = -1;
                GVBind();
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
                            cmd.CommandText = "SELECT SECTION_TABLE.section_id, SECTION_TABLE.section_name,SECTION_TABLE.program,  SECTION_TABLE.yearLevel, CASE WHEN program = 1 THEN 'DAY' WHEN program = 0 THEN 'EVENING' ELSE 'INVALID' END AS programdesc, CASE WHEN yearLevel = 1 THEN '1st year' WHEN yearLevel = 2 THEN '2nd year' WHEN yearLevel = 3 THEN '3rd year' WHEN yearLevel = 4 THEN '4th year' ELSE 'INVALID' END AS yearLeveldesc, SECTION_TABLE.isDeleted, SEMESTER_TABLE.semester_id, (SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER,COURSE_TABLE.course_id,(COURSE_TABLE.title+' - '+COURSE_TABLE.description) AS COURSE, SCHOOL_TABLE.Id AS sch_id, SCHOOL_TABLE.school_name FROM SECTION_TABLE JOIN SEMESTER_TABLE ON SECTION_TABLE.semester_id = SEMESTER_TABLE.semester_id JOIN COURSE_TABLE ON SECTION_TABLE.course_id = COURSE_TABLE.course_id JOIN SCHOOL_TABLE ON SECTION_TABLE.school_id = SCHOOL_TABLE.Id WHERE SECTION_TABLE.school_id='" + DropDownSchool.SelectedValue + "'";
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                        }
                        db.Close();
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox name = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtNAME");
            DropDownList course = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlCOURSE");
            DropDownList term = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSEMESTER");
            DropDownList program = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlprogram");
            DropDownList yearLvl = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlyear");
            DropDownList depschool = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlschool");

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SECTION_TABLE"
                         + " SET" +
                         " section_name = @name," 
                         + " program = @program," 
                         + " yearLevel = @year," 
                         + " course_id = @course," 
                         + " semester_id = @sem," 
                         + " school_id = @school"
                         + " WHERE section_id = '" + id + "';";
                        cmd.Parameters.AddWithValue("@name", name.Text);
                        cmd.Parameters.AddWithValue("@program", program.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@year", yearLvl.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@course", course.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@sem", term.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@school", depschool.SelectedValue.ToString());
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
                            GridView1.EditIndex = -1;
                            GVBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM SECTION_TABLE WHERE section_id='" + id + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Section is deleted with ID: " + id + "', 'success')", true);
                            GridView1.EditIndex = -1;
                            GVBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVBind();
        }
    }
}