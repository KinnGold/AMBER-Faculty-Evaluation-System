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
    public partial class AdminSchedulePage : System.Web.UI.Page
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
                        cmd.CommandText = "SELECT id,(fname +' '+ SUBSTRING(mname, 1, 1)+'. '+lname) AS INSTRUCTOR FROM INSTRUCTOR_TABLE WHERE [school_id] = '" + Session["school"].ToString() + "' AND [isDeleted] IS NULL";
                        ddlIns.DataValueField = "id";
                        ddlIns.DataTextField = "INSTRUCTOR";
                        ddlIns.DataSource = cmd.ExecuteReader();
                        ddlIns.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlIns.Items.Add("-Instructor-");
            ddlIns.Items.FindByText("-Instructor-").Selected = true;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT section_id, section_name FROM SECTION_TABLE WHERE SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT SCHEDULE_TABLE.sched_code ,SCHEDULE_TABLE.sched_id ,SCHEDULE_TABLE.subject_id, SCHEDULE_TABLE.instructor_id, SCHEDULE_TABLE.time, CONVERT(time,LEFT(time,CHARINDEX('-',time)-1)) AS startTime, CONVERT(time,RIGHT(time,CHARINDEX('-',time)-1)) AS endTime, SCHEDULE_TABLE.day ,SUBJECT_TABLE.subject_Id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name) AS SUBJECT, (INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR, INSTRUCTOR_TABLE.id, SECTION_TABLE.section_id,SECTION_TABLE.section_name,SCHEDULE_TABLE.section_id FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.Id JOIN SECTION_TABLE ON SCHEDULE_TABLE.section_id=SECTION_TABLE.section_id WHERE SCHEDULE_TABLE.school_id = @schoolid AND SCHEDULE_TABLE.isDeleted IS NULL", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    plcNoData.Visible = false;
                    plcData.Visible = true;
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
                else
                {
                    plcNoData.Visible = true;
                    plcData.Visible = false;
                }
            }
        }
        public void clear()
        {
            populateDDL();
            //txtDay.Text = String.Empty;
            //txtTime.Text = String.Empty;
        }

        bool CheckifSchedExists(string subject, string instructor, string time, string day,string section)
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
                        cmd.CommandText = "SELECT * FROM SCHEDULE_TABLE WHERE subject_id = '"+ subject + "' AND instructor_id = '"+ instructor +"' AND time = '"+ time + "' AND day = '"+ day + "' AND section_id = '" + section + "' AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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
        protected void btnAddSchedule_Click(object sender, EventArgs e)
        {
            var subject = ddlSub.SelectedValue;
            var instructor = ddlIns.SelectedValue;
            var time = Convert.ToDateTime(txtTimeStart.Text).ToString("hh:mm tt") + " - " + Convert.ToDateTime(txtTimeEnd.Text).ToString("hh:mm tt");
            var day = ddlDay.SelectedValue;
            var section = ddlSection.SelectedValue;
            var code = txtCode.Text;

            if (CheckifSchedExists(subject,instructor,time,day,section))
            {
                //clear();
                Session["temp"] = "Schedule";
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
                            cmd.CommandText = "INSERT INTO SCHEDULE_TABLE(sched_code, subject_id, instructor_id, time, day, section_id, school_id)"
                                + " VALUES("
                                + "@code,"
                                + "@sub,"
                                + "@ins,"
                                + "@time,"
                                + "@day,"
                                + "@section,"
                                + "@school)";
                            cmd.Parameters.AddWithValue("@code", code);
                            cmd.Parameters.AddWithValue("@sub", subject);
                            cmd.Parameters.AddWithValue("@ins", instructor);
                            cmd.Parameters.AddWithValue("@time", time);
                            cmd.Parameters.AddWithValue("@day", day);
                            cmd.Parameters.AddWithValue("@section", section);
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                clear();
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
            TextBox code = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtCode");
            DropDownList subject = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSUBJECT");
            DropDownList instructor = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlINSTRUCTOR");
            TextBox timestart = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txttoTimeStart");
            TextBox timeend = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txttoTimeEnd");
            var time = Convert.ToDateTime(timestart.Text).ToString("hh:mm tt") + " - " + Convert.ToDateTime(timeend.Text).ToString("hh:mm tt");
            DropDownList day = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlDAY");
            DropDownList section = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSECTION");
            if (CheckifSchedExists(subject.SelectedValue, instructor.SelectedValue, time, day.Text,section.SelectedValue))
            {
                //clear();
                Session["temp"] = "Schedule";
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
                            cmd.CommandText = "UPDATE SCHEDULE_TABLE"
                                + " SET"
                                + " SCHED_CODE = @code,"
                                + " SUBJECT_ID = @sub,"
                                + " INSTRUCTOR_ID = @ins,"
                                + " TIME = @time,"
                                + " DAY = @day,"
                                + " SECTION_ID = @section"
                                + " WHERE sched_Id = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
                            cmd.Parameters.AddWithValue("@code", code.Text);
                            cmd.Parameters.AddWithValue("@sub", subject.SelectedValue);
                            cmd.Parameters.AddWithValue("@ins", instructor.SelectedValue);
                            cmd.Parameters.AddWithValue("@time", time);
                            cmd.Parameters.AddWithValue("@day", day.SelectedValue);
                            cmd.Parameters.AddWithValue("@section", section.SelectedValue);
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

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
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
                        cmd.CommandText = "UPDATE SCHEDULE_TABLE SET isDeleted= @delete WHERE sched_Id ='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
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

        public string getInstructorDEPT(string id)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT dept_id FROM INSTRUCTOR_TABLE WHERE Id = '"+ id +"' AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader["dept_id"].ToString();
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
            return id;
        }

        protected void ddlIns_SelectedIndexChanged(object sender, EventArgs e)
        {//populate subjects & SECTION(WALA PA)
            string dept_id = "";
            if (ddlIns.SelectedValue != "-Instructor-")
            {
                dept_id = getInstructorDEPT(ddlIns.SelectedValue);
            }
            
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT subject_Id,('('+subject_code+') '+ subject_name) AS SUBJECT,dept_id FROM SUBJECT_TABLE WHERE dept_id='" + dept_id +"' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL UNION ALL SELECT DISTINCT subject_Id,('('+subject_code+') '+ subject_name) AS SUBJECT,DEPARTMENT_TABLE.dept_Id FROM DEPARTMENT_TABLE JOIN SUBJECT_TABLE ON DEPARTMENT_TABLE.dept_Id = SUBJECT_TABLE.dept_id WHERE SUBJECT_TABLE.SCHOOL_ID = '" + Session["school"].ToString() + "' AND SUBJECT_TABLE.isDeleted IS NULL AND SUBJECT_TABLE.dept_id != '" + dept_id +"'";
                        ddlSub.DataValueField = "subject_id";
                        ddlSub.DataTextField = "SUBJECT";
                        ddlSub.DataSource = cmd.ExecuteReader();
                        ddlSub.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            //ddlSub.Items.Add("-Subject-");
            //ddlSub.Items.FindByText("-Subject-").Selected = true;
        }
    }
}