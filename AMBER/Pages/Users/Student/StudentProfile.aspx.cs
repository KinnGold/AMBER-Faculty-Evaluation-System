using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;


namespace AMBER.Pages.Users.Student
{
    public partial class StudentProfile : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] == null && Session["role"] == null && Session["password"] == null)
            {
                Response.Redirect("/Pages/Users/UsersLogin.aspx");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    try
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE stud_id='" + Request.QueryString["Id"].ToString() + "'";
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    db.Close();
                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                        Response.Write(ex);
                    }
                }

                loadUser();
                LoadSchool();
                LoadSection();
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
        void loadUser()
        {
            var Id = Session["Id"].ToString();
            var school_id = Session["school"].ToString();

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE stud_id = '" + Id + "' AND school_id='" + school_id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            displayidnum.Text = reader["stud_id"].ToString();
                            displaystudname.Text = reader["fname"].ToString() + " " + reader["mname"].ToString() + " " + reader["lname"].ToString();
                            displaystudrole.Text = reader["role"].ToString();
                            displayemail.Text = reader["email"].ToString();
                            displayphonenumber.Text = reader["phonenum"].ToString();
                            image();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
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

        void LoadSchool()
        {
            var school_id = Session["school"].ToString();

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SCHOOL_TABLE WHERE Id='" + school_id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            displaystudschool.Text = reader["school_name"].ToString();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "schoolnotFound()", true);
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

        void LoadSection()
        {
            var section = Session["section"].ToString();

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SECTION_TABLE WHERE section_id='" + section + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            displaystudsection.Text = reader["section_name"].ToString();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "schoolnotFound()", true);
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

        void passclear()
        {
            txtconfirmnewpass.Text = String.Empty;
            txtnewpass.Text = String.Empty;
            txtcurrpass.Text = String.Empty;
        }

        void clear()
        {
            txtrequest.Text = String.Empty;
        }

        protected void btnrequest_Click(object sender, EventArgs e)
        {
            var userID = Session["Id"].ToString();
            var email = Session["email"].ToString();
            var role = Session["role"].ToString();
            var school = Session["school"].ToString();
            var fname = Session["fname"].ToString();
            var mname = Session["mname"].ToString();
            var lname = Session["lname"].ToString();
            var status = 0;
            var msg = txtrequest.Text;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO NOTIFICATIONS_TABLE(message, isRead, DateTimeSent, first_name, middle_name, last_name, user_id, user_role, user_email, school_id)"
                            + " VALUES("
                            + "@msg,"
                            + "@status,"
                            + "@date,"
                            + "@fname,"
                            + "@mname,"
                            + "@lname,"
                            + "@id,"
                            + "@role,"
                            + "@email,"
                            + "@school)";
                        cmd.Parameters.AddWithValue("@msg", msg);
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mname", mname);
                        cmd.Parameters.AddWithValue("@lname", lname);
                        cmd.Parameters.AddWithValue("@id", userID);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@school", school);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            clear();
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SendSuccess()", true);
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

        protected void savepassbtn_Click(object sender, EventArgs e)
        {
            var Id = Session["Id"].ToString();
            var newpass = txtnewpass.Text;
            var confirmpass = txtconfirmnewpass.Text;
            string pass = txtcurrpass.Text;
            var pass1 = Session["password"].ToString();
            var role = Session["role"].ToString();
            var school = Session["school"].ToString();

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");


            if (!hasLowerChar.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one lower case letter', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one lower case letter";
            }
            else if (!hasUpperChar.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one upper case letter', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one upper case letter";
            }
            else if (!hasMiniMaxChars.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password error!', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should not be less than or greater than 12 characters";
            }
            else if (!hasNumber.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one number character', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one numeric value";
            }
            else if (!hasSymbols.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one special character', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one special case characters";
            }
            else if (pass1 != pass)
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "PassError()", true);
            }
            else if (confirmpass != newpass)
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "PassError()", true);
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
                            cmd.CommandText = "UPDATE STUDENT_TABLE"
                                + " SET "
                                + " password = @newpass"
                                + " WHERE stud_id = '" + Id + "' AND school_id='" + school + "'";
                            cmd.Parameters.AddWithValue("@newpass", newpass);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Session["Id"] = Id;
                                Session["password"] = newpass;
                                Session["role"] = role;
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
                                passclear();
                            }
                            db.Close();
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

        protected void upload_Click(object sender, EventArgs e)
        {
            try
            {
                if (updateProfile.HasFile)
                {
                    int length = updateProfile.PostedFile.ContentLength;
                    byte[] pic = new byte[length];
                    updateProfile.PostedFile.InputStream.Read(pic, 0, length);
                    string filename = Path.GetFileName(updateProfile.PostedFile.FileName);
                    string extension = Path.GetExtension(filename);
                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE STUDENT_TABLE"
                                        + " SET"
                                        + " profile_picture = @Image"
                                        + " WHERE stud_id = '" + Session["Id"].ToString() + "' AND school_id = '" + Session["school"].ToString() + "'";
                                cmd.Parameters.AddWithValue("@Image", pic);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    Response.Redirect("https://localhost:44311/Pages/Users/Student/StudentProfile.aspx?stud_id=" + Session["Id"].ToString() + "");
                                }
                            }
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "fileerror()", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "profilepicerror()", true);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        public void image()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT profile_picture FROM STUDENT_TABLE WHERE stud_id = '" + Session["Id"].ToString() + "' AND school_id = '" + Session["school"].ToString() + "'";
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
    }
}