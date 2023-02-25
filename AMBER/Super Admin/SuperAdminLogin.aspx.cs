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

namespace AMBER.Super_Admin
{
    public partial class SuperAdminLogin : System.Web.UI.Page
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
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var user = txtusername.Text;
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
                            cmd.CommandText = "SELECT lock_status, lockdatetime FROM SUPER_ADMIN_TABLE WHERE username='" + user + "' AND password='" + pass + "' OR password='" + pass + "' COLLATE Latin1_General_CS_AS";
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
                                    cmd.CommandText = "SELECT * FROM SUPER_ADMIN_TABLE WHERE username='" + user + "' AND password='" + pass + "' COLLATE Latin1_General_CS_AS";
                                    reader = cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        Session["user"] = user;
                                        Session["pass"] = pass;
                                        Session["fname"] = reader["first_name"];
                                        Session["id"] = reader["Id"];
                                        Session["lname"] = reader["last_name"];
                                        Session["mname"] = reader["middle_name"];
                                        Session["contact_no"] = reader["contact_no"];
                                        Session["email"] = reader["email"];

                                        db.Close();
                                        Random random = new Random();
                                        string code = random.Next(1001, 9999).ToString();

                                        /*db.Open();
                                        cmd.CommandType = CommandType.Text;
                                        cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE SET TFA_code=@code WHERE Id='" + Session["Id"].ToString() + "' AND email='" + Session["email"].ToString() + "';";
                                        cmd.Parameters.AddWithValue("@code", code);
                                        var ctr = cmd.ExecuteNonQuery();
                                        if (ctr >= 1)
                                        {
                                            SmtpClient smtp = new SmtpClient();
                                            smtp.Host = "smtp.gmail.com";
                                            smtp.Timeout = 60000;
                                            smtp.Port = 587;
                                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                            smtp.UseDefaultCredentials = false;
                                            smtp.Credentials = new System.Net.NetworkCredential("capstoners.amber@gmail.com", "ppfbcakbcllnqxjf");
                                            smtp.EnableSsl = true;
                                            MailMessage msg = new MailMessage();
                                            msg.Subject = "Account Two Factor Authentication Code";
                                            msg.Body += "Hello, " + Session["email"].ToString() + "\nyour Two-Factor Authentication code is " + code + " to proceed logging into your account.\n\nThank you & Regards\nDevelopers";
                                            string toaddress = Session["email"].ToString();
                                            msg.To.Add(toaddress);
                                            string fromaddress = "<capstoners.amber@gmail.com>";

                                            msg.From = new MailAddress(fromaddress);
                                            try
                                            {
                                                smtp.Send(msg);
                                            }
                                            catch
                                            {
                                                throw;
                                            }
                                            Response.Redirect("https://localhost:44311/Super%20Admin/TwoFactorAuthentication.aspx?TFA_code=" + AMBER.URLEncryption.GetencryptedQueryString(code) + "");
                                        }
                                        else
                                        {
                                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "Wrong()", true);
                                        }*/
                                        Response.Redirect("/Super%20Admin/SuperAdminLandingPage.aspx");
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
                        cmd.CommandText = "SELECT * FROM SUPER_ADMIN_TABLE WHERE username='" + username + "' AND password='" + password + "' COLLATE Latin1_General_CS_AS";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Session["user"] = username;
                            Session["pass"] = password;
                            Session["fname"] = reader["first_name"];
                            Session["id"] = reader["Id"];
                            Session["lname"] = reader["last_name"];
                            Session["mname"] = reader["middle_name"];
                            Session["contact_no"] = reader["contact_no"];
                            Session["email"] = reader["email"];
                        }
                        else
                        {
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE SET lock_status=1, lockdatetime=@date WHERE username='" + username + "' OR password='" + password + "' COLLATE Latin1_General_CS_AS";
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
                        cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE SET lock_status=0, lockdatetime=NULL WHERE username='" + username + "' OR password='" + password + "' COLLATE Latin1_General_CS_AS";
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
                                Session["fname"] = reader["first_name"];
                                Session["id"] = reader["Id"];
                                Session["lname"] = reader["last_name"];
                                Session["mname"] = reader["middle_name"];
                                Session["contact_no"] = reader["contact_no"];
                                Session["email"] = reader["email"];

                                /*db.Close();
                                Random random = new Random();
                                string code = random.Next(1001, 9999).ToString();

                                db.Open();
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE SET TFA_code=@code WHERE Id='" + Session["Id"].ToString() + "' AND email='" + Session["email"].ToString() + "';";
                                cmd.Parameters.AddWithValue("@code", code);
                                ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Timeout = 60000;
                                    smtp.Port = 587;
                                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("capstoners.amber@gmail.com", "ppfbcakbcllnqxjf");
                                    smtp.EnableSsl = true;
                                    MailMessage msg = new MailMessage();
                                    msg.Subject = "Account Two Factor Authentication Code";
                                    msg.Body += "Hello, " + Session["email"].ToString() + "\nyour Two-Factor Authentication code is " + code + " to proceed logging into your account.\n\nThank you & Regards\nDevelopers";
                                    string toaddress = Session["email"].ToString();
                                    msg.To.Add(toaddress);
                                    string fromaddress = "<capstoners.amber@gmail.com>";

                                    msg.From = new MailAddress(fromaddress);
                                    try
                                    {
                                        smtp.Send(msg);
                                    }
                                    catch
                                    {
                                        throw;
                                    }
                                    Response.Redirect("https://localhost:44311/Super%20Admin/TwoFactorAuthentication.aspx?TFA_code=" + AMBER.URLEncryption.GetencryptedQueryString(code) + "");
                                }
                                else
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "Wrong()", true);
                                }*/

                                Response.Redirect("/Super%20Admin/SuperAdminLandingPage.aspx");
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