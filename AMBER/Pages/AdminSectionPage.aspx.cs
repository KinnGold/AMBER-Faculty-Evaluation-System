using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace AMBER.Pages
{
    public partial class AdminSectionPage : System.Web.UI.Page
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
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT SECTION_TABLE.section_id, SECTION_TABLE.section_name,SECTION_TABLE.program,  SECTION_TABLE.yearLevel, CASE WHEN program = 1 THEN 'DAY' WHEN program = 0 THEN 'EVENING' ELSE 'INVALID' END AS programdesc, CASE WHEN yearLevel = 1 THEN '1st year' WHEN yearLevel = 2 THEN '2nd year' WHEN yearLevel = 3 THEN '3rd year' WHEN yearLevel = 4 THEN '4th year' ELSE 'INVALID' END AS yearLeveldesc, SEMESTER_TABLE.semester_id, (SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER,COURSE_TABLE.course_id,(COURSE_TABLE.title+' - '+COURSE_TABLE.description) AS COURSE FROM SECTION_TABLE JOIN SEMESTER_TABLE ON SECTION_TABLE.semester_id = SEMESTER_TABLE.semester_id JOIN COURSE_TABLE ON SECTION_TABLE.course_id = COURSE_TABLE.course_id WHERE SECTION_TABLE.SCHOOL_ID = @schoolid AND SECTION_TABLE.isDeleted IS NULL", db);
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
                        cmd.CommandText = "SELECT course_id,(title+' - '+description) AS COURSE FROM COURSE_TABLE WHERE SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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
                        cmd.CommandText = "SELECT semester_id, (description+' SY '+year) AS SEMESTER FROM SEMESTER_TABLE WHERE SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL ORDER BY year, description";
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

        protected void btnAddSection_Click(object sender, EventArgs e)
        {
            var name = txtsecname.Text;
            var course = ddlCourse.SelectedValue;
            var term = ddlSemester.SelectedValue;
            var yearLvl = ddlyearlevel.SelectedValue;
            var program = ddlProgram.SelectedValue;
            if (CheckifSectionExists(name,course,term, program, yearLvl))
            {
                clear();
                Session["temp"] = "Section";
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
                            cmd.CommandText = "INSERT INTO SECTION_TABLE(section_name, program, yearLevel, course_id, semester_id, school_id)"
                                + " VALUES("
                                + "@name,"
                                + "@program,"
                                + "@year,"
                                + "@course,"
                                + "@term,"
                                + "@school)";
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@program", program);
                            cmd.Parameters.AddWithValue("@year", yearLvl);
                            cmd.Parameters.AddWithValue("@course", course);
                            cmd.Parameters.AddWithValue("@term", term);
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
        public void clear()
        {
            txtsecname.Text = String.Empty;
        }
        bool CheckifSectionExists(string name, string course, string term, string program, string year)
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
                        cmd.CommandText = "SELECT * FROM SECTION_TABLE WHERE (section_name='" + name + "' AND program = '" + program + "' AND yearLevel = '"+ year +"' AND course_id = '" + course + "') AND semester_id = '" + term + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox name = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtNAME");
            DropDownList course = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlCOURSE");
            DropDownList term = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSEMESTER");
            DropDownList program = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlprogram");
            DropDownList yearLvl = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlyear");
            if (CheckifSectionExists(name.Text, course.SelectedValue, term.SelectedValue, program.SelectedValue, yearLvl.SelectedValue))
            {
                clear();
                Session["temp"] = "Section";
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
                            cmd.CommandText = "UPDATE SECTION_TABLE"
                                + " SET"
                                + " SECTION_NAME = @name,"
                                + " PROGRAM = @program,"
                                + " YEARLEVEL = @year,"
                                + " COURSE_ID = @crs,"
                                + " SEMESTER_ID = @sem"
                                + " WHERE section_Id = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
                            cmd.Parameters.AddWithValue("@name", name.Text);
                            cmd.Parameters.AddWithValue("@PROGRAM", program.SelectedValue);
                            cmd.Parameters.AddWithValue("@year", yearLvl.SelectedValue);
                            cmd.Parameters.AddWithValue("@crs", course.SelectedValue);
                            cmd.Parameters.AddWithValue("@sem", term.SelectedValue);
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

        bool isSectionReferenced(int id, string table,string pk)
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
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + "='" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        protected int countaffectrows(int id,string table,string pk)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(" + pk + ") AS COUNT FROM " + table + " WHERE " + pk + "='" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = Int32.Parse(reader["COUNT"].ToString());
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return id;
        }
        public string getIDinfo(int id, string table, string pk, string infoNeed)
        {
            var value = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + " = '" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            value = reader[infoNeed].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return value;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            string name = getIDinfo(id,"SECTION_TABLE","section_id","section_name");
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        //"DELETE FROM SUBJECT_TABLE WHERE ID ='" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "'";
                        if (isSectionReferenced(id,"SCHEDULE_TABLE","section_id") || isSectionReferenced(id, "STUDENT_TABLE", "section_id"))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','To avoid this error, you need to delete master records first on <b>SCHEDULE</b> and <b>STUDENT</b> records with <b>" + name + "</b> then after that you can delete this record. \"<b>Schedule</b> row/s: " + countaffectrows(id,"SCHEDULE_TABLE","section_id") + " and <b>Student</b> row/s: " + countaffectrows(id,"STUDENT_TABLE", "section_id") + "\"', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE SECTION_TABLE SET isDeleted= @delete WHERE section_Id ='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@delete", DateTime.Now);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','section <b>" + name + "</b> is deleted successfully', 'success')", true);
                                GridView1.EditIndex = -1;
                                GVbind();
                            }
                            //UPDATE
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
        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlCourse.SelectedValue != "-Course-")
            {
                txtsecname.Text = getIDinfo(Convert.ToInt32(ddlCourse.SelectedValue), "COURSE_TABLE", "course_id", "title");
            }
            else
            {
                txtsecname.Text = String.Empty;
            }
            Session.Remove("ctr");
            Session.Remove("temp");
        }

        protected void ddlyearlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["temp"] == null)
            {
                Session["temp"] = txtsecname.Text;
            }
            if (ddlyearlevel.SelectedValue != "-Year Level-")
            {
                txtsecname.Text = Session["temp"] + "_" + ddlyearlevel.SelectedValue;
            }
            else
            {
                txtsecname.Text = Session["temp"].ToString();
            }
            //Session.Remove("ctr");
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Session["ctr"] == null)
            {
                Session["ctr"] = txtsecname.Text;
            }
            
            if(ddlProgram.SelectedValue != "-Program-")
            {
                txtsecname.Text = Session["ctr"] + "_" + ddlProgram.SelectedItem.Text.Substring(0,3);
            }
            else
            {
                txtsecname.Text = Session["ctr"].ToString();
            }
            Session.Remove("temp");
        }
    }
}