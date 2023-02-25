using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.Pages
{
    public partial class AdminStudyLoadPage : System.Web.UI.Page
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
                populateDDL();
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
                        cmd.CommandText = "SELECT DISTINCT section_id, section_name FROM SECTION_TABLE WHERE CAMPUS_ID = '" + Session["campus"].ToString() + "' AND isDeleted IS NULL";
                        ddlSection.DataValueField = "section_id";
                        ddlSection.DataTextField = "section_name";
                        ddlSection.DataSource = cmd.ExecuteReader();
                        ddlSection.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlSection.Items.Add("-Section-");
            ddlSection.Items.FindByText("-Section-").Selected = true;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBSTRING(INSTRUCTOR_TABLE.fname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname+' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.Id WHERE SCHEDULE_TABLE.campus_id = '" + Session["campus"].ToString() + "' AND SCHEDULE_TABLE.isDeleted IS NULL";
                        ddlSchedule.DataValueField = "sched_id";
                        ddlSchedule.DataTextField = "SCHEDULE";
                        ddlSchedule.DataSource = cmd.ExecuteReader();
                        ddlSchedule.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlSchedule.Items.Add("-Schedule-");
            ddlSchedule.Items.FindByText("-Schedule-").Selected = true;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT course_id,(title+' - '+description) AS COURSE FROM COURSE_TABLE WHERE CAMPUS_ID = '" + Session["campus"].ToString() + "' AND isDeleted IS NULL";
                        ddlCourse.DataValueField = "course_id";
                        ddlCourse.DataTextField = "COURSE";
                        ddlCourse.DataSource = cmd.ExecuteReader();
                        ddlCourse.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlCourse.Items.Add("-Course-");
            ddlCourse.Items.FindByText("-Course-").Selected = true;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT semester_id, (description+' SY '+year) AS SEMESTER FROM SEMESTER_TABLE WHERE CAMPUS_ID = '" + Session["campus"].ToString() + "' AND isDeleted IS NULL AND status = 'ACTIVE'";
                        ddlSemester.DataValueField = "semester_id";
                        ddlSemester.DataTextField = "SEMESTER";
                        ddlSemester.DataSource = cmd.ExecuteReader();
                        ddlSemester.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlSemester.Items.Add("-Semester-");
            ddlSemester.Items.FindByText("-Semester-").Selected = true;
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT STUDY_LOAD.load_id,STUDY_LOAD.semester_id,STUDY_LOAD.sched_id,STUDY_LOAD.course_id, STUDY_LOAD.section_id,SECTION_TABLE.section_id,SECTION_TABLE.section_name,SCHEDULE_TABLE.sched_Id,('('+SUBJECT_TABLE.subject_code+') '+ SUBSTRING(INSTRUCTOR_TABLE.fname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname+' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE, SEMESTER_TABLE.semester_id, (SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER, COURSE_TABLE.course_id,(COURSE_TABLE.title+' - '+COURSE_TABLE.description) AS COURSE FROM STUDY_LOAD JOIN SECTION_TABLE ON STUDY_LOAD.section_id = SECTION_TABLE.section_id JOIN SCHEDULE_TABLE ON STUDY_LOAD.sched_id = SCHEDULE_TABLE.sched_Id JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id=INSTRUCTOR_TABLE.id JOIN SEMESTER_TABLE ON STUDY_LOAD.semester_id = SEMESTER_TABLE.semester_id join COURSE_TABLE ON STUDY_LOAD.course_id = COURSE_TABLE.course_id WHERE STUDY_LOAD.campus_id = @campusid AND STUDY_LOAD.isDeleted IS NULL", db);
                cmd.Parameters.AddWithValue("@campusid", Session["campus"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }
        bool CheckifStudyLoadExists(string section,string schedule, string course, string semester)
        {
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM STUDY_LOAD WHERE semester_id ='" + semester + "' AND sched_id='" + schedule + "' AND course_id='" + course + "' AND section_id='" + section + "' AND campus_id='" + Session["campus"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            chkr = true;
                        }
                        else
                        {
                            chkr = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }

        protected void btnAddsubject_Click(object sender, EventArgs e)
        {
            var section = ddlSection.SelectedValue;
            var schedule = ddlSchedule.SelectedValue;
            var course = ddlCourse.SelectedValue;
            var semester = ddlSemester.SelectedValue;

            if (CheckifStudyLoadExists(section,schedule,course,semester))
            {
                populateDDL();
                Session["temp"] = "Study Load";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
            }
            else
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "INSERT INTO STUDY_LOAD(semester_id, sched_id, course_id, section_id, campus_id)"
                                + " VALUES("
                                + "@sem,"
                                + "@sched,"
                                + "@course,"
                                + "@sec,"
                                + "@campus)";
                            cmd.Parameters.AddWithValue("@sem", semester);
                            cmd.Parameters.AddWithValue("@sched", schedule);
                            cmd.Parameters.AddWithValue("@course", course);
                            cmd.Parameters.AddWithValue("@sec", section);
                            cmd.Parameters.AddWithValue("@campus", Session["campus"].ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                //populateDDL();
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
                                GVbind();
                            }
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

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            DropDownList semester = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSEMESTER");
            DropDownList schedule = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSCHED");
            DropDownList course = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlCOURSE");
            DropDownList section = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSECTION");
            if (CheckifStudyLoadExists(section.SelectedValue, schedule.SelectedValue, course.SelectedValue, semester.SelectedValue))
            {
                populateDDL();
                Session["temp"] = "Study Load";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
            }
            else
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE STUDY_LOAD"
                                + " SET"
                                + " SEMESTER_ID = @sem,"
                                + " SCHED_ID = @sched,"
                                + " COURSE_ID = @crs,"
                                + " SECTION_ID = @sec"
                                + " WHERE load_Id = '" + id + "' AND CAMPUS_ID = '" + Session["campus"].ToString() + "';";
                            cmd.Parameters.AddWithValue("@sem", semester.SelectedValue);
                            cmd.Parameters.AddWithValue("@sched", schedule.SelectedValue);
                            cmd.Parameters.AddWithValue("@crs", course.SelectedValue);
                            cmd.Parameters.AddWithValue("@sec", section.SelectedValue);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "UpdateSuccess()", true);
                                GridView1.EditIndex = -1;
                                GVbind();
                            }
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVbind();
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
                        cmd.CommandText = "UPDATE STUDY_LOAD SET isDeleted= @delete WHERE sched_Id ='" + id + "' AND CAMPUS_ID='" + Session["campus"].ToString() + "'";
                        cmd.Parameters.AddWithValue("@delete", DateTime.Now);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','subject is deleted with ID: " + id + "', 'success')", true);
                            GridView1.EditIndex = -1;
                            GVbind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVbind();
        }
    }
}