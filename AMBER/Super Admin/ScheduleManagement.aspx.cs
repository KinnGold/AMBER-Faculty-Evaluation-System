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
    public partial class ScheduleManagement : System.Web.UI.Page
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
            }
        }
        protected void GVBind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT SCHEDULE_TABLE.sched_Id, SCHEDULE_TABLE.sched_code ,SCHEDULE_TABLE.subject_id, SCHEDULE_TABLE.instructor_id, SCHEDULE_TABLE.time, SCHEDULE_TABLE.day, SCHEDULE_TABLE.isDeleted ,SUBJECT_TABLE.subject_Id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name) AS SUBJECT,INSTRUCTOR_TABLE.id AS ins_id,  (INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR, SECTION_TABLE.section_id,SECTION_TABLE.section_name,SCHEDULE_TABLE.section_id AS schedsec_id, SCHOOL_TABLE.Id AS sc_id, SCHOOL_TABLE.school_name FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.Id JOIN SECTION_TABLE ON SCHEDULE_TABLE.section_id=SECTION_TABLE.section_id JOIN SCHOOL_TABLE ON SCHEDULE_TABLE.school_id = SCHOOL_TABLE.Id", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox code = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtschedcode");
            DropDownList instructor = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlInstructor");
            TextBox time = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txttime");
            DropDownList day = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlDAY");
            DropDownList depschool = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlschool");
            DropDownList section = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSection");
            DropDownList subject = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSubject");

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SCHEDULE_TABLE"
                         + " SET"
                         + " sched_code = @code,"
                         + " subject_id = @subject," 
                         + " instructor_id = @ins," 
                         + " time = @time," 
                         + " day = @day," 
                         + " section_id = @section," 
                         + " school_id = @school"
                         + " WHERE sched_id = '" + id + "';";
                        cmd.Parameters.AddWithValue("@code", code.Text);
                        cmd.Parameters.AddWithValue("@subject", subject.SelectedValue);
                        cmd.Parameters.AddWithValue("@ins", instructor.SelectedValue);
                        cmd.Parameters.AddWithValue("@time", time.Text);
                        cmd.Parameters.AddWithValue("@day", day.SelectedValue);
                        cmd.Parameters.AddWithValue("@section", section.SelectedValue);
                        cmd.Parameters.AddWithValue("@school", depschool.SelectedValue);
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
                        cmd.CommandText = "DELETE FROM SCHEDULE_TABLE WHERE sched_id='" + id + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Schedule is deleted with ID: " + id + "', 'success')", true);
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

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVBind();
        }
    }
}