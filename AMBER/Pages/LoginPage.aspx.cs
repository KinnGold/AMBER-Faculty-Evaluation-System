using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AMBER.Pages
{
    public partial class LoginPage : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["invalidloginattempt"] = null;
            }
        }
        void clear()
        {
            txtpassword.Text = String.Empty;
            txtusername.Text = String.Empty;
            txtemail.Text = String.Empty;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var user = txtusername.Text;
            //getUserpass(user);
            var pass = txtpassword.Text;
            int attemptcount;

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
                changeLockStatus();
                clear();
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
                            cmd.CommandText = "SELECT lock_status, lockdatetime FROM ADMIN_TABLE WHERE username='" + user + "' AND password='" + pass + "' OR password='" + pass + "' COLLATE Latin1_General_CS_AS";
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
                                        unlockStatus();
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
                                    cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE username='" + user + "' AND password='" + pass + "' COLLATE Latin1_General_CS_AS";
                                    reader = cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        Session["user"] = user;
                                        Session["pass"] = pass;
                                        Session["fname"] = reader["fname"];
                                        Session["id"] = reader["Id"];
                                        Session["role"] = reader["role"];
                                        Session["lname"] = reader["lname"];
                                        Session["mname"] = reader["mname"];
                                        Session["contact_no"] = reader["contact_no"];
                                        Session["email"] = reader["email"];
                                        Session["school"] = reader["school_id"];
                                        isVerified();
                                        checkuserSUB();
                                    }
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "Wrong()", true);
                                    }
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "Wrong()", true);
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

        public void checkuserSUB()
        {
            var id = Session["id"].ToString();
            DateTime now = DateTime.Now;
            var school = Session["school"].ToString();
            var status = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SUBSCRIPTION_TABLE WHERE admin_id='" + id + "' AND endDate <= '" + now + "' AND school_id='" + school + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            db.Open();

                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE SUBSCRIPTION_TABLE SET subscription_type=@type, startDate=@start, endDate=@end, status=@status WHERE admin_id='" + id + "' AND school_id='" + school + "'";
                            cmd.Parameters.AddWithValue("@type", "Free");
                            cmd.Parameters.AddWithValue("@start", DateTime.Now);
                            cmd.Parameters.AddWithValue("@end", DateTime.Now.AddDays(30));
                            cmd.Parameters.AddWithValue("@status", status);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Response.Redirect("/Pages/SubscriptionExpired.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("/Pages/AdminLandingPage.aspx");

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

        public void isVerified()
        {
            var user = txtusername.Text;
            var pass = txtpassword.Text;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE username='" + user + "'AND password='" + pass + "' COLLATE Latin1_General_CS_AS AND isVerified IS NULL ";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            string verify_code = reader["verify_code"].ToString();
                            Response.Redirect("https://localhost:44311/Pages/VerifyAccount.aspx?verify_code=" + AMBER.URLEncryption.GetencryptedQueryString(verify_code) + "");
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

        protected void sendemail_Click(object sender, EventArgs e)
        {
            var email = txtemail.Text;

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
                            smtp.Credentials = new System.Net.NetworkCredential("capstoners.amber@gmail.com", "ppfbcakbcllnqxjf");
                            smtp.EnableSsl = true;
                            MailMessage msg = new MailMessage();
                            msg.Subject = "Reset Password Link";
                            msg.Body += "Hello, " + email + "\nYou requested for a reset in your password, just click the link below to complete the process.\n";
                            msg.Body += "Click here 'https://localhost:44311/Pages/ForgotPassword.aspx?forgot_otp=" + AMBER.URLEncryption.GetencryptedQueryString(forgot_otp) + "\n";
                            string toaddress = email;
                            msg.To.Add(toaddress);
                            string fromaddress = "<capstoners.amber@gmail.com>";

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
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "notfound()", true);
                            clear();
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

        protected void resendemail_Click(object sender, EventArgs e)
        {
            var email = txtemail.Text;
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
                            smtp.Credentials = new System.Net.NetworkCredential("capstoners.amber@gmail.com", "ppfbcakbcllnqxjf");
                            smtp.EnableSsl = true;
                            MailMessage msg = new MailMessage();
                            msg.Subject = "Reset Password Link";
                            msg.Body += "Hello, " + email + "\nYou requested for a reset in your password, just click the link below to complete the process.\n";
                            msg.Body += "Click here 'https://localhost:44311/Pages/ForgotPassword.aspx?forgot_otp=" + AMBER.URLEncryption.GetencryptedQueryString(forgot_otp) + "\n";
                            string toaddress = email;
                            msg.To.Add(toaddress);
                            string fromaddress = "<capstoners.amber@gmail.com>";

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
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "notfound()", true);
                            clear();
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

        void changeLockStatus()
        {
            string format = "MM/dd/yyyy HH:mm:ss";
            var username = txtusername.Text;
            var password = txtpassword.Text;
            var date = DateTime.Now.ToString(format);
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE username='" + username + "' AND password='" + password + "' COLLATE Latin1_General_CS_AS";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Session["user"] = username;
                            Session["pass"] = password;
                            Session["fname"] = reader["fname"];
                            Session["id"] = reader["Id"];
                            Session["lname"] = reader["lname"];
                            Session["mname"] = reader["mname"];
                            Session["contact_no"] = reader["contact_no"];
                            Session["email"] = reader["email"];
                            Session["school"] = reader["school_id"];
                            isVerified();
                            checkuserSUB();
                        }
                        else
                        {
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE ADMIN_TABLE SET lock_status=1, lockdatetime=@date WHERE username='" + username + "' OR password='" + password + "' COLLATE Latin1_General_CS_AS";
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

        void unlockStatus()
        {
            var username = txtusername.Text;
            var password = txtpassword.Text;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE ADMIN_TABLE SET lock_status=0, lockdatetime=NULL WHERE username='" + username + "' OR password='" + password + "' COLLATE Latin1_General_CS_AS";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE username='" + username + "' AND password='" + password + "' COLLATE Latin1_General_CS_AS";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                Session["user"] = username;
                                Session["pass"] = password;
                                Session["fname"] = reader["fname"];
                                Session["id"] = reader["Id"];
                                Session["lname"] = reader["lname"];
                                Session["mname"] = reader["mname"];
                                Session["contact_no"] = reader["contact_no"];
                                Session["email"] = reader["email"];
                                Session["school"] = reader["school_id"];
                                isVerified();
                                checkuserSUB();
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