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
using System.IO;

namespace AMBER.Pages
{
    public partial class SignupPage : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM SCHOOLLIST_TABLE ORDER BY school_name ASC";
                            school_nameDDL.DataValueField = "Id";
                            school_nameDDL.DataTextField = "school_name";
                            school_nameDDL.DataSource = cmd.ExecuteReader();
                            school_nameDDL.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                school_nameDDL.Items.Add("Select School");
                school_nameDDL.Items.FindByText("Select School").Selected = true;
            }
        }

        void populateSchoolAddress()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SCHOOLLIST_TABLE WHERE Id='"+ school_nameDDL.SelectedValue +"'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            address.Text = reader["school_address"].ToString();
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
       
        protected void btnsignup_Click(object sender, EventArgs e)
        {
            var id = makeID();
            var school_id = MakeSchoolID();
            var email = txtemail.Text;
            var fname = txtfname.Text;
            var lname = txtlname.Text;
            var mname = txtmi.Text;
            var pos = position.Text;
            var contact = txtcontact.Text;
            var pass = txtpassword.Text;
            var pass1 = txtconfirmpass.Text;
            var user = txtuname.Text;
            var sch_name = school_nameDDL.SelectedItem.Text;
            var sch_phone = school_phone.Text;
            var add = address.Text;


            string fileName = HttpContext.Current.Server.MapPath(@"~/Pictures/usericon.png");
            byte[] bytes = null;
            Stream stream = System.IO.File.OpenRead(fileName);
            BinaryReader binaryreader = new BinaryReader(stream);
            bytes = binaryreader.ReadBytes((int)stream.Length);

            

            Random random = new Random();
            string verify_code = random.Next(1001, 9999).ToString();
            bool lockstatus = false;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");


            if (!hasLowerChar.IsMatch(pass) || !hasLowerChar.IsMatch(pass1))
            {
                passwordclear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one lower case letter', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one lower case letter";
            }
            else if (!hasUpperChar.IsMatch(pass) || !hasUpperChar.IsMatch(pass1))
            {
                passwordclear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one upper case letter', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one upper case letter";
            }
            else if (!hasMiniMaxChars.IsMatch(pass) || !hasMiniMaxChars.IsMatch(pass1))
            {
                passwordclear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password error!', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should not be less than or greater than 12 characters";
            }
            else if (!hasNumber.IsMatch(pass) || !hasNumber.IsMatch(pass1))
            {
                passwordclear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one number character', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one numeric value";
            }
            else if (!hasSymbols.IsMatch(pass) || !hasSymbols.IsMatch(pass1))
            {
                passwordclear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one special character', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one special case characters";
            }
            else if (CheckifUserExists() == true)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
                usernameclear();
            }
            else if (CheckifSchoolExists() == true)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "schoolexist()", true);
                schoolclear();
            }
            else if(school_nameDDL.SelectedValue == "Select School" || school_nameDDL.SelectedValue == null)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "select()", true);
                schoolclear();
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
                            cmd.CommandText = "INSERT INTO ADMIN_TABLE(Id, fname, mname, lname, email, contact_no, school_id, position, role, password, username, datesignedup, verify_code, isVerified, profile_picture, lock_status)VALUES("
                                + "@id,"
                                + "@fname,"
                                + "@mname,"
                                + "@lname,"
                                + "@email,"
                                + "@contactno,"
                                + "@school,"
                                + "@position,"
                                + "@role,"
                                + "@pass,"
                                + "@username,"
                                + "@datesignedup," 
                                + "@verify," 
                                + "@isVerified," 
                                + "@profile," 
                                + "@lock)";
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@fname", fname);
                            cmd.Parameters.AddWithValue("@mname", mname);
                            cmd.Parameters.AddWithValue("@lname", lname);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@contactno", contact);
                            cmd.Parameters.AddWithValue("@school", school_id);
                            cmd.Parameters.AddWithValue("@position", pos);
                            cmd.Parameters.AddWithValue("@role", "Admin");
                            cmd.Parameters.AddWithValue("@pass", pass);
                            cmd.Parameters.AddWithValue("@username", user);
                            cmd.Parameters.AddWithValue("@datesignedup", DateTime.Now);
                            cmd.Parameters.AddWithValue("@verify", verify_code);
                            cmd.Parameters.AddWithValue("@isVerified", false);
                            cmd.Parameters.AddWithValue("@profile", bytes);
                            cmd.Parameters.AddWithValue("@lock", lockstatus);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Session["id"] = id;
                                Session["verify_code"] = verify_code;
                                string school_pic = HttpContext.Current.Server.MapPath(@"~/Pictures/AmberDefault.png");
                                byte[] schbytes = null;
                                Stream strm = System.IO.File.OpenRead(school_pic);
                                BinaryReader br = new BinaryReader(strm);
                                schbytes = br.ReadBytes((int)strm.Length);

                                cmd.CommandText = "INSERT INTO SCHOOL_TABLE(Id, school_name, school_phone, address, school_type, school_picture)"
                               + " VALUES("
                               + "@school_id,"
                               + "@school_name,"
                               + "@school_phone,"
                               + "@address,"
                               + "@school_type,"
                               + "@school_picture)";
                                cmd.Parameters.AddWithValue("@school_id", school_id);
                                cmd.Parameters.AddWithValue("@school_name", sch_name);
                                cmd.Parameters.AddWithValue("@school_phone", sch_phone);
                                cmd.Parameters.AddWithValue("@school_type", "Main");
                                cmd.Parameters.AddWithValue("@school_picture", schbytes);
                                cmd.Parameters.AddWithValue("@address", add);
                                cmd.ExecuteNonQuery();
                                Session["school"] = school_id;

                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.Timeout = 60000;
                                smtp.Port = 587;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                                smtp.EnableSsl = true;
                                MailMessage msg = new MailMessage();
                                msg.IsBodyHtml = true;
                                msg.Subject = "Account Verification Code";
                                msg.Body += "Hello " + fname + ",<br/> Thank you for signing up to AMBER, your verification code is <b>" + verify_code + "</b>, use it to verify your account.<br/><br/>Thank you & Regards Developers";
                                string toaddress = email;
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
                                freemium();
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

        }

        public void freemium()
        {
            var id = Session["id"];
            string verify_code = Session["verify_code"].ToString();
            var school_id = Session["school"].ToString();
            DateTime now = DateTime.Now;
            DateTime startDate = now;
            DateTime endDate = now.AddMonths(1);
            string type = "Free";
            string plan = "Not Subscribed";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO SUBSCRIPTION_TABLE(SUBSCRIPTION_TYPE, SUBSCRIPTION_PLAN, SCHOOL_ID, ADMIN_ID, STARTDATE, ENDDATE, STATUS)"
                            + " VALUES("
                            + "@type,"
                            + "@plan,"
                            + "@school,"
                            + "@admin_id,"
                            + "@start,"
                            + "@end,"
                            + "@status)";
                        cmd.Parameters.AddWithValue("@type", type.ToString());
                        cmd.Parameters.AddWithValue("@plan", plan.ToString());
                        cmd.Parameters.AddWithValue("@school", school_id);
                        cmd.Parameters.AddWithValue("@admin_id", id);
                        cmd.Parameters.AddWithValue("@start", startDate);
                        cmd.Parameters.AddWithValue("@end", endDate);
                        cmd.Parameters.AddWithValue("@status", 1);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Response.Redirect("https://localhost:44311/Pages/VerifyAccount.aspx?verify_code=" + AMBER.URLEncryption.GetencryptedQueryString(verify_code) + "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert ('Under Maintenance')</script><pre>" + ex.ToString() + "</pre>");
            }
        }
        public string makeID()
        {
            string res;
            Random rnd = new Random();
            int num = rnd.Next(1911, 10000);
            char last = char.Parse(txtlname.Text.Substring(0, 1));
            char first = char.Parse(txtfname.Text.Substring(0, 1));
            char second = char.Parse(txtmi.Text.Substring(0, 1));
            res = last + first + second + "2022" + num;
            return res;
        }

        public string MakeSchoolID()
        {
            string res;
            Random rnd = new Random();
            int num = rnd.Next(1911, 100000);
            res = "2022" + num;
            return res;
        }

        public void schoolclear()
        {
            school_phone.Text = String.Empty;
            address.Text = String.Empty;
            txtpassword.Text = String.Empty;
            txtconfirmpass.Text = String.Empty;
            position.Text = String.Empty;
        }


        public void usernameclear()
        {
            txtuname.Text = String.Empty;
            txtpassword.Text = String.Empty;
            txtconfirmpass.Text = String.Empty;
        }


        public void passwordclear()
        {
            txtpassword.Text = String.Empty;
            txtconfirmpass.Text = String.Empty;
        }

        bool CheckifUserExists()
        {
            var uname = txtuname.Text;
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE username='" + uname + "'";
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
        bool CheckifSchoolExists()
        {
            var name = school_nameDDL.SelectedItem.Text.ToString();
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SCHOOL_TABLE WHERE school_name='" + name + "'";
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

        protected void school_nameDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(school_nameDDL.SelectedItem.Text == "Select School")
            {
                address.Text = String.Empty;
            }
            else
            {
                populateSchoolAddress();
            }
        }
    }
}