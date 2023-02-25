using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AMBER.BM.Users
{
    public partial class UsersMP : System.Web.UI.MasterPage
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getTERMstatus();
                if (Session["id"] == null && Session["role"] == null)
                {
                    Response.Redirect("/Pages/Users/UsersLogin.aspx");
                }
                if (Session["role"].Equals("instructor"))
                {
                    if (Session["mname"].ToString() != "")
                    {
                        lbluser.Text = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
                    }
                    else
                    {
                        lbluser.Text = Session["fname"].ToString() + " " + Session["lname"].ToString();
                    }
                    studentProfile.Visible = false;
                    evaluatebtn2.Visible = false;
                    skillstestbtn.Visible = false;
                    rankingsbtn.Visible = false;
                    plcDeanManage.Visible = false;
                    facultyevalbtn.Visible = false;
                    //lbtnReports.Visible = false;
                    plcDeanResult.Visible = false;
                    instructorimage();
                    GetNotifications();
                    var school = Session["school"].ToString();
                    string status = "True";
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE skillstest_status='" + status + "' AND school_id='" + school + "'";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                skillstestbtn.Visible = false;
                                skillsTestResult.Visible = false;
                            }
                            else
                            {
                                skillstestbtn.Visible = false;
                                skillsTestResult.Visible = false;
                            }
                        }
                    }
                }
                else if (Session["role"].Equals("dean"))
                {
                    if (Session["mname"].ToString() != "")
                    {
                        lbluser.Text = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
                    }
                    else
                    {
                        lbluser.Text = Session["fname"].ToString() + " " + Session["lname"].ToString();
                    }
                    plcevaluation.Visible = false;
                    plcresults.Visible = false;
                    resultsbtn.Visible = false;
                    rankingsbtn.Visible = true;
                    plcDeanManage.Visible = true;
                    studentProfile.Visible = false;
                    evaluatebtn2.Visible = false;
                    instructorimage();
                    GetNotifications();
                    var school = Session["school"].ToString();
                    string status = "True";
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE skillstest_status='" + status + "' AND school_id='" + school + "'";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                skillstestbtn.Visible = true;
                                skillsTestResult.Visible = true;
                            }
                            else
                            {
                                skillstestbtn.Visible = false;
                                skillsTestResult.Visible = false;
                            }
                        }
                    }
                }
                else if (Session["role"].Equals("student"))
                {
                    if (Session["mname"].ToString() != "")
                    {
                        lbluser.Text = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
                    }
                    else
                    {
                        lbluser.Text = Session["fname"].ToString() + " " + Session["lname"].ToString();
                    }
                    instructorProfile.Visible = false;
                    plcevaluation.Visible = false;
                    plcresults.Visible = false;
                    rankingsbtn.Visible = false;
                    plcDeanManage.Visible = false;
                    skillstestbtn.Visible = false;
                    facultyevalbtn.Visible = false;
                    //lbtnReports.Visible = false;
                    plcDeanResult.Visible = false;
                    studentimage();
                    GetNotifications();
                    string status = "True";
                    var school = Session["school"].ToString();
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE skillstest_status='" + status + "' AND school_id='" + school + "'";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                skillstestbtn.Visible = false;
                                skillsTestResult.Visible = false;
                            }
                            else
                            {
                                skillstestbtn.Visible = false;
                                skillsTestResult.Visible = false;
                            }
                        }
                    }
                }
                SchoolImage();
                if(Session["dept"] != null)
                {
                    lbluserdisplay.Text = getDeptName(Session["dept"].ToString()) + "(" + Session["role"].ToString() + ")";
                }
                else
                {
                    lbluserdisplay.Visible = false;
                }
                
            }
        }

        void getTERMstatus()
        {
            var start = "";
            var end = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE status='Active' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            //start = Convert.ToDateTime(reader["EvaluationStart"]);
                            //end = Convert.ToDateTime(reader["EvaluationEnd"]);
                            start = reader["EvaluationStart"].ToString();
                            end = reader["EvaluationEnd"].ToString();
                        }
                        else
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            if (start != "" && end != "")
            {
                var test = Convert.ToDateTime(start);
                if (DateTime.Now >= Convert.ToDateTime(start) && DateTime.Now <= Convert.ToDateTime(end))
                {
                    lblstatus.Text = "Starting";
                    lblstatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblstatus.Text = "Ended";
                    lblstatus.ForeColor = System.Drawing.Color.Red;
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

        /*public string SessionExpireDestinationUrl
        {

            get { return "https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(Session["school"].ToString()) + ""; }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.PageHead.Controls.Add(new LiteralControl(
                String.Format("<meta http-equiv=<%# refresh %> content=<%# {0};url={1} %>>",
                SessionLengthMinutes * 60, SessionExpireDestinationUrl)));
        }*/

        public void GetNotifications()
        {
            int count = 0;
            var status = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM USERSNOTIFICATIONS_TABLE WHERE toUser_id='" + Session["Id"].ToString() + "' AND school_id='" + Session["school"].ToString() + "' AND toUser_role='" + Session["role"].ToString() + "' AND isRead='" + status + "'";
                        cmd.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        count = Convert.ToInt32(dt.Rows.Count.ToString());
                        notifications1.Text = count.ToString();
                        Repeater1.DataSource = dt;
                        Repeater1.DataBind();

                        db.Close();
                        db.Open();
                        cmd.CommandText = "SELECT * FROM USERSNOTIFICATIONS_TABLE WHERE toUser_id='" + Session["Id"].ToString() + "' AND school_id='" + Session["school"].ToString() + "' AND toUser_role='" + Session["role"].ToString() + "' AND isRead='" + status + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            plcnotifications.Visible = true;
                            plcNOnotifications.Visible = false;
                        }
                        else
                        {
                            plcnotifications.Visible = false;
                            plcNOnotifications.Visible = true;
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

        protected void instructorProfile_Click(object sender, EventArgs e)
        {
            var Id = Session["Id"].ToString();
            Response.Redirect("https://localhost:44311/Pages/Users/Faculty/InstructorProfile.aspx?Id=" + Id + "");
        }

        protected void studentProfile_Click(object sender, EventArgs e)
        {
            var Id = Session["Id"].ToString();
            Response.Redirect("https://localhost:44311/Pages/Users/Student/StudentProfile.aspx?Id=" + Id + "");
        }

        protected void logoutbtn_Click(object sender, EventArgs e)
        {
            var sch_id = Session["school"].ToString();
            Response.Redirect("https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(sch_id) + "");
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CLEARSESSIONSTORAGE", "clearSessionForModalEval()", true);
            Session["role"] = null;
            Session["Id"] = null;
            Session["password"] = null;
        }

        //protected void instructorProfile_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("/Pages/Users/Faculty/InstructorProfile.aspx");
        //}

        //protected void studentProfile_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("/Pages/Users/Student/StudentProfile.aspx");
        //}

        protected void landingpage_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/LandingPage.aspx");
        }

        protected void peerevalbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Faculty/PeerEvaluation.aspx");
        }

        protected void evaluatebtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Student/EvaluateInstructor.aspx");
        }

        protected void evaluatebtn2_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Student/Evaluateform.aspx");
        }

        protected void btnNotif_Click(object sender, EventArgs e)
        {

        }

        protected void selfevalbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Faculty/SelfEvaluation.aspx");
        }

        protected void resultsbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Faculty/EvaluationResult.aspx");
        }
        protected void skillstestresultsbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Faculty/SkillsTestResults.aspx");
        }

        protected void skillstestbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/SkillsTest.aspx");
        }

        public string getDeptName(string id)
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM DEPARTMENT_TABLE WHERE dept_Id='"+ id +"'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        id = reader["dept_name"].ToString();
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
            return id;
        }

        public void SchoolImage()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT school_picture, school_name FROM SCHOOL_TABLE WHERE Id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblschool.Text = reader["school_name"].ToString();

                        if (reader["school_picture"] != System.DBNull.Value)
                        {
                            byte[] bytes = (byte[])reader["school_picture"];
                            string profilePicture = Convert.ToBase64String(bytes, 0, bytes.Length);
                            schoolProfilePic.ImageUrl = "data:image/png;base64," + profilePicture;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
        }
        protected void viewallbtn_Click(object sender, EventArgs e)
        {
            var status = 1;
            var status1 = 0;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM USERSNOTIFICATIONS_TABLE WHERE toUser_id='" + Session["Id"].ToString() + "' AND school_id='" + Session["school"].ToString() + "' AND toUser_role='" + Session["role"].ToString() + "' AND isRead='" + status1 + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            db.Open();

                            cmd.CommandText = "UPDATE USERSNOTIFICATIONS_TABLE"
                           + " SET"
                           + " isRead = @stat"
                           + "  WHERE toUser_id='" + Session["Id"].ToString() + "' AND school_id='" + Session["school"].ToString() + "' AND toUser_role='" + Session["role"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@stat", status);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                GetNotifications();
                                Response.Redirect("/Pages/Users/Notifications.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("/Pages/Users/Notifications.aspx");
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

        protected void markasreadbtn_Click(object sender, EventArgs e)
        {
            var status = 1;
            var status1 = 0;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM USERSNOTIFICATIONS_TABLE WHERE toUser_id='" + Session["Id"].ToString() + "' AND school_id='" + Session["school"].ToString() + "' AND toUser_role='" + Session["role"].ToString() + "' AND isRead='" + status1 + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            db.Open();

                            cmd.CommandText = "UPDATE USERSNOTIFICATIONS_TABLE"
                           + " SET"
                           + " isRead = @stat"
                           + " WHERE toUser_id='" + Session["Id"].ToString() + "' AND school_id='" + Session["school"].ToString() + "' AND toUser_role='" + Session["role"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@stat", status);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                GetNotifications();
                            }
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
        public void instructorimage()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT profile_picture FROM INSTRUCTOR_TABLE WHERE ins_id = '" + Session["id"].ToString() + "' AND school_id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader["profile_picture"] != System.DBNull.Value)
                        {
                            byte[] bytes = (byte[])reader["profile_picture"];
                            string profilePicture = Convert.ToBase64String(bytes, 0, bytes.Length);
                            profilePic.ImageUrl = "data:image/png;base64," + profilePicture;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
        }

        public void studentimage()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT profile_picture FROM STUDENT_TABLE WHERE stud_id = '" + Session["id"].ToString() + "' AND school_id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader["profile_picture"] != System.DBNull.Value)
                        {
                            byte[] bytes = (byte[])reader["profile_picture"];
                            string profilePicture = Convert.ToBase64String(bytes, 0, bytes.Length);
                            profilePic.ImageUrl = "data:image/png;base64," + profilePicture;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
        }

        protected void selfevalbtn_Click1(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Faculty/SelfEvaluation.aspx");
        }

        protected void rankingsbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Ranking.aspx");
        }

        protected void facultyevalbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Faculty/PeerEvaluation.aspx");
        }

        protected void lbtnCourse_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanCourseManage.aspx");
        }

        protected void lbtnSection_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanSectionManage.aspx");
        }

        protected void lbtnSubject_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanSubjectManage.aspx");
        }

        protected void lbtnInstructor_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanInstructorManage.aspx");
        }

        protected void lbtnStudent_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanStudentManage.aspx");
        }

        protected void lbtnReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanReports.aspx");
        }

        protected void btnStudEval_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanSectionResult.aspx");
        }

        protected void btnPeerEval_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanPeerResult.aspx");
        }

        protected void btnSelfEval_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanSelfResult.aspx");
        }

        protected void btnOverEval_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanOverallResult.aspx");
        }

        protected void skillsTestResult_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Users/Dean/DeanSkillsTestResult.aspx");
        }
    }
}