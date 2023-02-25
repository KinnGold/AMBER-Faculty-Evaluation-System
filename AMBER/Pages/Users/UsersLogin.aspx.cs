using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

namespace AMBER.BM.Users
{
    public partial class UsersLogin : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int code = 0;
                if (Request.QueryString["school_id"] != null)
                {
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["school_id"].ToString()), out code))
                    {
                        Session["school"] = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["school_id"]);
                    }
                }
                else
                {
                    Session["school"] = 12345;
                }
                Session["invalidloginattempt"] = null;
                if(Session["school"].ToString() != "12345")
                {
                    SchoolImage();
                    SchoolSched();
                    plcDefault.Visible = false;
                    plcSchool.Visible = true;
                }
                else
                {
                    plcDefault.Visible = true;
                    plcSchool.Visible = false;
                }
                
            }
        }

        //Instructor login button click event
        protected void instructorloginbtn_Click(object sender, EventArgs e)
        {
            var idnum = txtinsnum.Text;
            var pass = txtinspass.Text;
            int attemptcount;
            var status = 1;

            if (Session["invalidloginattempt"] != null)
            {
                attemptcount = Convert.ToInt16(Session["invalidloginattempt"].ToString());
                attemptcount = attemptcount + 1;
            }
            else
            {
                attemptcount = 1;
            }

            Session["invalidloginattempt"] = attemptcount;

            if (attemptcount == 3)
            {
                changeInstructorLockStatus();
                clear();
            }
            else
            {
                if (Convert.ToInt32(Session["school"]) == 12345)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Something went wrong!', '<b>Invalid Link</b> please check your email for the Session of your evaluation', 'error')", true);
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
                                cmd.CommandText = "SELECT lock_status, lockdatetime FROM INSTRUCTOR_TABLE WHERE ins_id='" + idnum + "' AND password='" + pass + "' OR password='" + pass + "' COLLATE Latin1_General_CS_AS";
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    if (Convert.ToBoolean(reader["lock_status"].ToString()) == true)
                                    {
                                        DateTime locktime = DateTime.Now;
                                        locktime = Convert.ToDateTime(reader["lockdatetime"].ToString());
                                        locktime = Convert.ToDateTime(locktime.ToString("MM/dd/yyyy HH:mm:ss"));


                                        DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                                        TimeSpan ts = dt.Subtract(locktime);
                                        Int32 timelock = Convert.ToInt32(ts.TotalSeconds);
                                        Int32 pending = 60 - timelock;

                                        if (pending <= 0)
                                        {
                                            unlockInstructorStatus();
                                        }
                                        else
                                        {
                                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "accountLocked()", true);
                                        }
                                    }
                                    else
                                    {
                                        db.Close();
                                        db.Open();

                                        cmd.CommandType = CommandType.Text;
                                        cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND ins_id='" + idnum + "' AND password='" + pass + "' COLLATE Latin1_General_CS_AS";
                                        reader = cmd.ExecuteReader();
                                        if (reader.Read())
                                        {
                                            Session["Id"] = idnum;
                                            Session["insID"] = reader["id"];
                                            Session["password"] = pass;
                                            Session["fname"] = reader["fname"];
                                            Session["lname"] = reader["lname"];
                                            Session["mname"] = reader["mname"];
                                            Session["contact_no"] = reader["phonenum"];
                                            Session["email"] = reader["email"];
                                            Session["dept"] = reader["dept_id"];
                                            Session["role"] = reader["role"];
                                            Session["school"] = reader["school_id"];
                                            Session["verify"] = reader["verify_code"];
                                            getTerm();
                                            db.Close();

                                            db.Open();
                                            cmd.CommandType = CommandType.Text;
                                            cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE ins_id='" + Session["Id"].ToString() + "' AND school_id = '"+ Session["school"].ToString() + "' AND isVerified='" + status + "';";
                                            reader = cmd.ExecuteReader();
                                            if (reader.Read())
                                            {
                                                Response.Redirect("/Pages/Users/LandingPage.aspx");
                                            }
                                            else
                                            {
                                                db.Close();
                                                Random random = new Random();
                                                string verify_code = random.Next(1001, 9999).ToString();

                                                db.Open();
                                                cmd.CommandType = CommandType.Text;
                                                cmd.CommandText = "UPDATE INSTRUCTOR_TABLE SET verify_code=@ver WHERE ins_id='" + Session["Id"].ToString() + "' AND email='" + Session["email"].ToString() + "';";
                                                cmd.Parameters.AddWithValue("@ver", verify_code);
                                                var ctr = cmd.ExecuteNonQuery();
                                                if (ctr >= 1)
                                                {
                                                    SmtpClient smtp = new SmtpClient();
                                                    smtp.Host = "smtp.gmail.com";
                                                    smtp.Timeout = 60000;
                                                    smtp.Port = 587;
                                                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                                    smtp.UseDefaultCredentials = false;
                                                    smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                                                    smtp.EnableSsl = true;
                                                    MailMessage msg = new MailMessage();
                                                    msg.Subject = "Account Verification Code";
                                                    msg.Body += "Hello, " + Session["email"].ToString() + "\nThank you for signing up to AMBER\nyour verification code is " + verify_code + " to verify your account.\n\nThank you & Regards\nDevelopers";
                                                    string toaddress = Session["email"].ToString();
                                                    msg.To.Add(toaddress);
                                                    string fromaddress = "<evaluatenow.amber@gmail.com>";

                                                    msg.From = new MailAddress(fromaddress);
                                                    try
                                                    {
                                                        smtp.Send(msg);
                                                    }
                                                    catch
                                                    {
                                                        throw;
                                                    }
                                                    Response.Redirect("/Pages/Users/Faculty/AccountVerification.aspx?verify_code=" + AMBER.URLEncryption.GetencryptedQueryString(verify_code) + "");
                                                    db.Close();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(Session["school"]) == 12345)
                                            {
                                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Something went wrong!', '<b>Invalid Link</b> please check your email for the Session of your evaluation', 'error')", true);
                                            }
                                            else
                                            {
                                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "Wrong()", true);
                                                clear();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(Session["school"]) == 12345)
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Something went wrong!', '<b>Invalid Link</b> please check your email for the Session of your evaluation', 'error')", true);
                                    }
                                    else
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "Wrong()", true);
                                        clear();
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
            }
        }

        //Student login button click event
        protected void studloginbtn_Click(object sender, EventArgs e)
        {
            var idnum = txtstudnum.Text;
            var pass = txtstudpass.Text;
            int attemptcount;
            var status = 1;

            if (Session["invalidloginattempt"] != null)
            {
                attemptcount = Convert.ToInt16(Session["invalidloginattempt"].ToString());
                attemptcount = attemptcount + 1;
            }
            else
            {
                attemptcount = 1;
            }

            Session["invalidloginattempt"] = attemptcount;

            if (attemptcount == 3)
            {
                changeStudentLockStatus();
                clear();
            }
            else
            {
                if (Convert.ToInt32(Session["school"]) == 12345)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Something went wrong!', '<b>Invalid Link</b> please check your email for the Session of your evaluation', 'error')", true);
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
                                cmd.CommandText = "SELECT lock_status, lockdatetime FROM STUDENT_TABLE WHERE stud_id='" + idnum + "' AND password='" + pass + "' OR password='" + pass + "' COLLATE Latin1_General_CS_AS";
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    if (Convert.ToBoolean(reader["lock_status"].ToString()) == true)
                                    {
                                        DateTime locktime = DateTime.Now;
                                        locktime = Convert.ToDateTime(reader["lockdatetime"].ToString());
                                        locktime = Convert.ToDateTime(locktime.ToString("MM/dd/yyyy HH:mm:ss"));


                                        DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                                        TimeSpan ts = dt.Subtract(locktime);
                                        Int32 timelock = Convert.ToInt32(ts.TotalSeconds);
                                        Int32 pending = 60 - timelock;

                                        if (pending <= 0)
                                        {
                                            Session["invalidloginattempt"] = null;
                                            unlockStudentStatus();
                                        }
                                        else
                                        {
                                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "accountLocked()", true);
                                        }
                                    }
                                    else
                                    {
                                        db.Close();
                                        db.Open();

                                        cmd.CommandType = CommandType.Text;
                                        cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND stud_id='" + idnum + "' AND password='" + pass + "' COLLATE Latin1_General_CS_AS";
                                        reader = cmd.ExecuteReader();
                                        if (reader.Read())
                                        {
                                            Session["Id"] = idnum;
                                            Session["studID"] = reader["Id"];
                                            Session["password"] = pass;
                                            Session["fname"] = reader["fname"];
                                            Session["lname"] = reader["lname"];
                                            Session["mname"] = reader["mname"];
                                            Session["email"] = reader["email"];
                                            Session["role"] = reader["role"];
                                            Session["school"] = reader["school_id"];
                                            Session["section"] = reader["section_id"];
                                            var end = getStudentTerm(idnum);
                                            string notEvaluated = "";
                                            if ((Convert.ToDateTime(end) - DateTime.Now).Days < 5)
                                            {
                                                if (!checkEvaluatedCountValid(Session["section"].ToString(), 0.5))
                                                {
                                                    //swal toast req
                                                    int[] evaluated = getEvaluatedInstructors(Session["section"].ToString(),0);
                                                    
                                                    int[] ALLevaluatee = getEvaluatedInstructors(Session["section"].ToString(), 1);
                                                    foreach(var instructor in ALLevaluatee)
                                                    {
                                                        if(evaluated.Length != 0)
                                                        {
                                                            for (int i = 0; i < evaluated.Length; i++)
                                                            {
                                                                if (instructor != evaluated[i])
                                                                {
                                                                    notEvaluated += getInstructorFullname(instructor.ToString()) + ", ";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            notEvaluated += getInstructorFullname(instructor.ToString()) + ", ";
                                                        }
                                                    }
                                                }
                                                if(notEvaluated != "")
                                                {
                                                    notify(notEvaluated.Substring(0, notEvaluated.Length - 2), (Convert.ToDateTime(end) - DateTime.Now).Days.ToString());
                                                }
                                            }
                                            db.Close();

                                            db.Open();
                                            cmd.CommandType = CommandType.Text;
                                            cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE stud_id='" + Session["Id"].ToString() + "'  AND school_id = '" + Session["school"].ToString() + "' AND isVerified='" + status + "';";
                                            reader = cmd.ExecuteReader();
                                            if (reader.Read())
                                            {
                                                Response.Redirect("/Pages/Users/LandingPage.aspx");
                                            }
                                            else
                                            {
                                                db.Close();
                                                Random random = new Random();
                                                string verify_code = random.Next(1001, 9999).ToString();

                                                db.Open();
                                                cmd.CommandType = CommandType.Text;
                                                cmd.CommandText = "UPDATE STUDENT_TABLE SET verify_code=@ver WHERE stud_id='" + Session["Id"].ToString() + "' AND email='" + Session["email"].ToString() + "';";
                                                cmd.Parameters.AddWithValue("@ver", verify_code);
                                                var ctr = cmd.ExecuteNonQuery();
                                                if (ctr >= 1)
                                                {
                                                    SmtpClient smtp = new SmtpClient();
                                                    smtp.Host = "smtp.gmail.com";
                                                    smtp.Timeout = 60000;
                                                    smtp.Port = 587;
                                                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                                    smtp.UseDefaultCredentials = false;
                                                    smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                                                    smtp.EnableSsl = true;
                                                    MailMessage msg = new MailMessage();
                                                    msg.Subject = "Account Verification Code";
                                                    msg.Body += "Hello, " + Session["email"].ToString() + "\nThank you for signing up to AMBER\nyour verification code is " + verify_code + " to verify your account.\n\nThank you & Regards\nDevelopers";
                                                    string toaddress = Session["email"].ToString();
                                                    msg.To.Add(toaddress);
                                                    string fromaddress = "<evaluatenow.amber@gmail.com>";

                                                    msg.From = new MailAddress(fromaddress);
                                                    try
                                                    {
                                                        smtp.Send(msg);
                                                    }
                                                    catch
                                                    {
                                                        throw;
                                                    }
                                                    Response.Redirect("/Pages/Users/Student/AccountVerification.aspx?verify_code=" + AMBER.URLEncryption.GetencryptedQueryString(verify_code) + "");
                                                    db.Close();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(Session["school"]) == 12345)
                                            {
                                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Something went wrong!', '<b>Invalid Link</b> please check your email for the Session of your evaluation', 'error')", true);
                                            }
                                            else
                                            {
                                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "Wrong()", true);
                                                clear();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(Session["school"]) == 12345)
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Something went wrong!', '<b>Invalid Link</b> please check your email for the Session of your evaluation', 'error')", true);
                                    }
                                    else
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "Wrong()", true);
                                        clear();
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
            }
        }

        public string getInstructorFullname(string id)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT (INSTRUCTOR_TABLE.fname +' '+ CASE WHEN INSTRUCTOR_TABLE.mname ='' THEN '' ELSE SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. ' END +INSTRUCTOR_TABLE.lname) AS INSTRUCTOR FROM INSTRUCTOR_TABLE WHERE Id = '" + id + "' AND school_id = '" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader["INSTRUCTOR"].ToString();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "notfound()", true);
                            clear();
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

        bool checkIfMessageALreadySent(string message)
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
                        cmd.CommandText = "SELECT * FROM USERSNOTIFICATIONS_TABLE WHERE message = '" + message + "' AND school_id='" + Session["school"].ToString() + "'";
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

        public void notify(string instructors, string daysLeft)
        {
            string msg = "Hello, " + Session["lname"].ToString() + " it seems that you have not yet evaluated this Instructor/s (" + instructors + ") you have " + daysLeft + " day/s left to evaluate";
            if(!checkIfMessageALreadySent(msg))
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "INSERT INTO USERSNOTIFICATIONS_TABLE(message, isRead, DateTimeSent, toName, fromName, toUser_Id, fromUser_id, toUser_role, fromUser_role, school_id)"
                                + " VALUES("
                                + "@msg,"
                                + "@status,"
                                + "@date,"
                                + "@to,"
                                + "@from,"
                                + "@toUser,"
                                + "@fromUser,"
                                + "@torole,"
                                + "@fromrole,"
                                + "@school)";
                            cmd.Parameters.AddWithValue("@msg", msg);
                            cmd.Parameters.AddWithValue("@status", 0);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@to", "AMBER");
                            cmd.Parameters.AddWithValue("@from", "");
                            cmd.Parameters.AddWithValue("@toUser", Session["Id"].ToString());
                            cmd.Parameters.AddWithValue("@fromUser", 8);
                            cmd.Parameters.AddWithValue("@torole", Session["role"].ToString());
                            cmd.Parameters.AddWithValue("@fromrole", "system");
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                }
            }
        }

        public bool checkEvaluatedCountValid(string section, double required)
        {
            required = 0.5;
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(DISTINCT SCHEDULE_TABLE.sched_code) AS COUNT,(SELECT COUNT(SCHEDULE_TABLE.instructor_id) FROM SCHEDULE_TABLE WHERE section_id = '" + section + "') AS TOTAL FROM SCHEDULE_TABLE LEFT JOIN EVALUATION_TABLE ON EVALUATION_TABLE.evaluatee_id = SCHEDULE_TABLE.instructor_id WHERE (EVALUATION_TABLE.evaluator_id IN (SELECT STUDENT_TABLE.Id FROM SECTION_TABLE JOIN STUDENT_TABLE ON STUDENT_TABLE.section_id = SECTION_TABLE.section_id WHERE SECTION_TABLE.section_id = '" + section + "') AND EVALUATION_TABLE.evaluatee_id IN (SELECT SCHEDULE_TABLE.instructor_id FROM SCHEDULE_TABLE WHERE section_id = '" + section + "' )) AND SCHEDULE_TABLE.section_id = '" + section + "' AND SCHEDULE_TABLE.school_id= '" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            var test = Convert.ToInt32(reader["COUNT"]);
                            var test2 = Math.Floor(Convert.ToInt32(reader["TOTAL"]) * required);
                            if (Convert.ToInt32(reader["COUNT"]) >= Math.Floor(Convert.ToInt32(reader["TOTAL"]) * required))
                            {
                                chkr = true;
                            }
                            else
                            {
                                chkr = false;
                            }
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
            return chkr;
        }

        public int[] getEvaluatedInstructors(string section,int index)
        {
            string[] query = { "SELECT DISTINCT SCHEDULE_TABLE.instructor_id FROM SCHEDULE_TABLE LEFT JOIN EVALUATION_TABLE ON EVALUATION_TABLE.evaluatee_id = SCHEDULE_TABLE.instructor_id WHERE (EVALUATION_TABLE.evaluator_id IN (SELECT STUDENT_TABLE.Id FROM SECTION_TABLE JOIN STUDENT_TABLE ON STUDENT_TABLE.section_id = SECTION_TABLE.section_id WHERE SECTION_TABLE.section_id = " + section + ") AND EVALUATION_TABLE.evaluatee_id IN (SELECT SCHEDULE_TABLE.instructor_id FROM SCHEDULE_TABLE WHERE section_id = " + section + " )) AND SCHEDULE_TABLE.section_id = " + section + " AND SCHEDULE_TABLE.school_id= '" + Session["school"].ToString() + "'", "SELECT DISTINCT SCHEDULE_TABLE.instructor_id FROM SCHEDULE_TABLE WHERE section_id = " + section + ""};
            //int[] id = new int[] = { };
            List<int> id = new List<int>();
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query[index];
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                id.Add(Convert.ToInt32(reader["instructor_id"]));
                            }
                        }
                        else
                        {

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
            return id.ToArray();
        }

        void getTerm()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE status='Active' AND school_id='" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Session["term"] = reader["semester_id"];
                        }
                        else
                        {
                            Session["term"] = null;
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "notfound()", true);
                            clear();
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
        }

        public string getStudentTerm(string id)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SECTION_TABLE.semester_id,STUDENT_TABLE.stud_id, SEMESTER_TABLE.evaluationEnd FROM SECTION_TABLE JOIN STUDENT_TABLE ON SECTION_TABLE.section_id=STUDENT_TABLE.section_id JOIN SEMESTER_TABLE ON SEMESTER_TABLE.semester_id=SECTION_TABLE.semester_id WHERE STUDENT_TABLE.stud_id='" + id + "' AND SEMESTER_TABLE.status='Active' AND SECTION_TABLE.school_id='" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Session["term"] = reader["semester_id"];
                            id = reader["evaluationEnd"].ToString();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "notfound()", true);
                            clear();
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

        //Clear function for textboxes
        void clear()
        {
            txtinsnum.Text = String.Empty;
            txtinspass.Text = String.Empty;
            txtstudnum.Text = String.Empty;
            txtstudpass.Text = String.Empty;
        }

        //Student change lock status to true if wrong ID number or password is entered
        void changeStudentLockStatus()
        {
            string format = "MM/dd/yyyy HH:mm:ss";
            var studId = txtstudnum.Text;
            var studpassword = txtstudpass.Text;
            var date = DateTime.Now.ToString(format);
            var status = 1;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE stud_id='" + studId + "' AND password='" + studpassword + "' COLLATE Latin1_General_CS_AS";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Session["Id"] = studId;
                            Session["studID"] = reader["Id"];
                            Session["password"] = studpassword;
                            Session["fname"] = reader["fname"];
                            Session["lname"] = reader["lname"];
                            Session["mname"] = reader["mname"];
                            Session["email"] = reader["email"];
                            Session["role"] = reader["role"];
                            Session["school"] = reader["school_id"];
                            Session["section"] = reader["section_id"];

                            db.Close();

                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE stud_id='" + Session["Id"].ToString() + "' AND isVerified='" + status + "';";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                Response.Redirect("/Pages/Users/LandingPage.aspx");
                            }
                            else
                            {
                                db.Close();
                                Random random = new Random();
                                string verify_code = random.Next(1001, 9999).ToString();

                                db.Open();
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE STUDENT_TABLE SET verify_code=@ver WHERE stud_id='" + Session["Id"].ToString() + "' AND email='" + Session["email"].ToString() + "';";
                                cmd.Parameters.AddWithValue("@ver", verify_code);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Timeout = 60000;
                                    smtp.Port = 587;
                                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                                    smtp.EnableSsl = true;
                                    MailMessage msg = new MailMessage();
                                    msg.Subject = "Account Verification Code";
                                    msg.Body += "Hello, " + Session["email"].ToString() + "\nThank you for signing up to AMBER\nyour verification code is " + verify_code + " to verify your account.\n\nThank you & Regards\nDevelopers";
                                    string toaddress = Session["email"].ToString();
                                    msg.To.Add(toaddress);
                                    string fromaddress = "<evaluatenow.amber@gmail.com>";

                                    msg.From = new MailAddress(fromaddress);
                                    try
                                    {
                                        smtp.Send(msg);
                                    }
                                    catch
                                    {
                                        throw;
                                    }
                                    Response.Redirect("/Pages/Users/Student/AccountVerification.aspx?verify_code=" + AMBER.URLEncryption.GetencryptedQueryString(verify_code) + "");
                                    db.Close();
                                }
                            }
                        }
                        else
                        {
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE STUDENT_TABLE SET lock_status=1, lockdatetime=@date WHERE stud_id='" + studId + "' OR password='" + studpassword + "' COLLATE Latin1_General_CS_AS";
                            cmd.Parameters.AddWithValue("@date", date);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "accountLocked()", true);
                                clear();
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

        //Student change lock status to false after one minute or 60 seconds
        void unlockStudentStatus()
        {
            var studId = txtstudnum.Text;
            var studpassword = txtstudpass.Text;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE STUDENT_TABLE SET lock_status=0, lockdatetime=NULL WHERE stud_id='" + studId + "' OR password='" + studpassword + "' COLLATE Latin1_General_CS_AS";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE stud_id='" + studId + "' AND password='" + studpassword + "' COLLATE Latin1_General_CS_AS";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                Session["Id"] = studId;
                                Session["studID"] = reader["Id"];
                                Session["password"] = studpassword;
                                Session["fname"] = reader["fname"];
                                Session["lname"] = reader["lname"];
                                Session["mname"] = reader["mname"];
                                Session["email"] = reader["email"];
                                Session["role"] = reader["role"];
                                Session["school"] = reader["school_id"];
                                Session["section"] = reader["section_id"];
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

        //Instructor change lock status to true if wrong ID number or password is entered
        void changeInstructorLockStatus()
        {
            string format = "MM/dd/yyyy HH:mm:ss";
            var insId = txtinsnum.Text;
            var inspassword = txtinspass.Text;
            var date = DateTime.Now.ToString(format);
            var status = 1;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE ins_id='" + insId + "' AND password='" + inspassword + "' COLLATE Latin1_General_CS_AS";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Session["Id"] = insId;
                            Session["password"] = inspassword;
                            Session["fname"] = reader["fname"];
                            Session["lname"] = reader["lname"];
                            Session["mname"] = reader["mname"];
                            Session["contact_no"] = reader["phonenum"];
                            Session["email"] = reader["email"];
                            Session["dept"] = reader["dept_id"];
                            Session["role"] = reader["role"];
                            Session["school"] = reader["school_id"];
                            Session["verify"] = reader["verify_code"];

                            db.Close();

                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE ins_id='" + Session["Id"].ToString() + "' AND isVerified='" + status + "';";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                Response.Redirect("/Pages/Users/LandingPage.aspx");
                            }
                            else
                            {
                                db.Close();
                                Random random = new Random();
                                string verify_code = random.Next(1001, 9999).ToString();

                                db.Open();
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE INSTRUCTOR_TABLE SET verify_code=@ver WHERE ins_id='" + Session["Id"].ToString() + "' AND email='" + Session["email"].ToString() + "';";
                                cmd.Parameters.AddWithValue("@ver", verify_code);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Timeout = 60000;
                                    smtp.Port = 587;
                                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                                    smtp.EnableSsl = true;
                                    MailMessage msg = new MailMessage();
                                    msg.Subject = "Account Verification Code";
                                    msg.Body += "Hello, " + Session["email"].ToString() + "\nThank you for signing up to AMBER\nyour verification code is " + verify_code + " to verify your account.\n\nThank you & Regards\nDevelopers";
                                    string toaddress = Session["email"].ToString();
                                    msg.To.Add(toaddress);
                                    string fromaddress = "<evaluatenow.amber@gmail.com>";

                                    msg.From = new MailAddress(fromaddress);
                                    try
                                    {
                                        smtp.Send(msg);
                                    }
                                    catch
                                    {
                                        throw;
                                    }
                                    Response.Redirect("/Pages/Users/Faculty/AccountVerification.aspx?verify_code=" + AMBER.URLEncryption.GetencryptedQueryString(verify_code) + "");
                                    db.Close();
                                }
                            }
                        }
                        else
                        {
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE INSTRUCTOR_TABLE SET lock_status=1, lockdatetime=@date WHERE ins_id='" + insId + "' OR password='" + inspassword + "' COLLATE Latin1_General_CS_AS";
                            cmd.Parameters.AddWithValue("@date", date);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "accountLocked()", true);
                                clear();
                                Session["invalidloginattempt"] = null;
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

        protected void inssendemail_Click(object sender, EventArgs e)
        {
            var email = txtinsemail.Text;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE email='" + email + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            Random rand = new Random();
                            int myrand = rand.Next(100000000, 999999999);
                            string forgot_otp = myrand.ToString();

                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE INSTRUCTOR_TABLE SET forgot_otp=@otp WHERE email='" + email + "'";
                            cmd.Parameters.AddWithValue("@otp", forgot_otp);
                            cmd.ExecuteNonQuery();


                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.Timeout = 10000;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                            smtp.EnableSsl = true;
                            MailMessage msg = new MailMessage();
                            msg.Subject = "Reset Password Link";
                            msg.Body += "Hello, " + email + "\n You requested for a reset in your password, just click the link below to complete the process.\n";
                            msg.Body += "Click here 'https://localhost:44311/Pages/Users/Faculty/InstructorForgotPassword.aspx?forgot_otp=" + AMBER.URLEncryption.GetencryptedQueryString(forgot_otp) + "\n";
                            string toaddress = email;
                            msg.To.Add(toaddress);
                            string fromaddress = "<evaluatenow.amber@gmail.com>";

                            msg.From = new MailAddress(fromaddress);
                            try
                            {
                                smtp.Send(msg);
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SendSuccess()", true);
                                txtinsemail.Text = String.Empty;
                            }
                            catch
                            {
                                throw;
                            }
                            db.Close();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "EmailnotFound()", true);
                            txtinsemail.Text = String.Empty;
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

        protected void insresendemail_Click(object sender, EventArgs e)
        {
            var email = txtinsemail.Text;
            try
            {

                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE email='" + email + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            Random rand = new Random();
                            int myrand = rand.Next(100000000, 999999999);
                            string forgot_otp = myrand.ToString();

                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE ADMIN_TABLE SET forgot_otp=@otp WHERE email='" + email + "'";
                            cmd.Parameters.AddWithValue("@otp", forgot_otp);
                            cmd.ExecuteNonQuery();


                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.Timeout = 10000;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                            smtp.EnableSsl = true;
                            MailMessage msg = new MailMessage();
                            msg.Subject = "Reset Password Link";
                            msg.Body += "Hello, " + email + "\nYou requested for a reset in your password, just click the link below to complete the process.\n";
                            msg.Body += "Click here 'https://localhost:44311/Pages/ForgotPassword.aspx?forgot_otp=" + AMBER.URLEncryption.GetencryptedQueryString(forgot_otp) + "\n";
                            string toaddress = email;
                            msg.To.Add(toaddress);
                            string fromaddress = "<evaluatenow.amber@gmail.com>";

                            msg.From = new MailAddress(fromaddress);
                            try
                            {
                                smtp.Send(msg);
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SendSuccess()", true);
                            }
                            catch
                            {
                                throw;
                            }
                            db.Close();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "EmailnotFound()", true);
                            txtinsemail.Text = null;
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
        protected void studresendemail_Click(object sender, EventArgs e)
        {
            var email = txtstudemail.Text;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE email='" + email + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            Random rand = new Random();
                            int myrand = rand.Next(100000000, 999999999);
                            string forgot_otp = myrand.ToString();

                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE STUDENT_TABLE SET forgot_otp=@otp WHERE email='" + email + "'";
                            cmd.Parameters.AddWithValue("@otp", forgot_otp);
                            cmd.ExecuteNonQuery();


                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.Timeout = 10000;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                            smtp.EnableSsl = true;
                            MailMessage msg = new MailMessage();
                            msg.Subject = "Reset Password Link";
                            msg.Body += "Hello, " + email + "\n You requested for a reset in your password, just click the link below to complete the process.\n";
                            msg.Body += "Click here 'https://localhost:44311/Pages/Users/Student/StudentForgotPassword.aspx?forgot_otp=" + AMBER.URLEncryption.GetencryptedQueryString(forgot_otp) + "\n";
                            string toaddress = email;
                            msg.To.Add(toaddress);
                            string fromaddress = "<evaluatenow.amber@gmail.com>";

                            msg.From = new MailAddress(fromaddress);
                            try
                            {
                                smtp.Send(msg);
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SendSuccess()", true);
                                txtstudemail.Text = String.Empty;
                            }
                            catch
                            {
                                throw;
                            }
                            db.Close();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "EmailnotFound()", true);
                            txtstudemail.Text = String.Empty;
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

        protected void studsendemail_Click(object sender, EventArgs e)
        {
            var email = txtstudemail.Text;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE email='" + email + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            Random rand = new Random();
                            int myrand = rand.Next(100000000, 999999999);
                            string forgot_otp = myrand.ToString();

                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE STUDENT_TABLE SET forgot_otp=@otp WHERE email='" + email + "'";
                            cmd.Parameters.AddWithValue("@otp", forgot_otp);
                            cmd.ExecuteNonQuery();


                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.Timeout = 10000;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                            smtp.EnableSsl = true;
                            MailMessage msg = new MailMessage();
                            msg.Subject = "Reset Password Link";
                            msg.Body += "Hello, " + email + "\n You requested for a reset in your password, just click the link below to complete the process.\n";
                            msg.Body += "Click here 'https://localhost:44311/Pages/Users/Student/StudentForgotPassword.aspx?forgot_otp=" + AMBER.URLEncryption.GetencryptedQueryString(forgot_otp) + "\n";
                            string toaddress = email;
                            msg.To.Add(toaddress);
                            string fromaddress = "<evaluatenow.amber@gmail.com>";

                            msg.From = new MailAddress(fromaddress);
                            try
                            {
                                smtp.Send(msg);
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SendSuccess()", true);
                                txtstudemail.Text = String.Empty;
                            }
                            catch
                            {
                                throw;
                            }
                            db.Close();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "EmailnotFound()", true);
                            txtstudemail.Text = String.Empty;
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
        //Instructor change lock status to false after one minute or 60 seconds
        void unlockInstructorStatus()
        {
            var insId = txtinsnum.Text;
            var inspassword = txtinspass.Text;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE INSTRUCTOR_TABLE SET lock_status=0, lockdatetime=NULL WHERE ins_id='" + insId + "' OR password='" + inspassword + "' COLLATE Latin1_General_CS_AS";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE ins_id='" + insId + "' AND password='" + inspassword + "' COLLATE Latin1_General_CS_AS";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                Session["Id"] = insId;
                                Session["password"] = inspassword;
                                Session["fname"] = reader["fname"];
                                Session["lname"] = reader["lname"];
                                Session["mname"] = reader["mname"];
                                Session["contact_no"] = reader["phonenum"];
                                Session["email"] = reader["email"];
                                Session["dept"] = reader["dept_id"];
                                Session["role"] = reader["role"];
                                Session["school"] = reader["school_id"];
                                Session["verify"] = reader["verify_code"];
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
        public void SchoolSched()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE school_id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblSched.Text = "Evaluation Started on " + Convert.ToDateTime(reader["evaluationStart"]).ToString("(dddd) MMMM dd, yyyy hh:mm tt") + "\n Ends at " + Convert.ToDateTime(reader["evaluationEnd"]).ToString("(dddd) MMMM dd, yyyy hh:mm tt");
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
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
    }
}