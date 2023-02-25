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

namespace AMBER.Pages.Users.Faculty
{
    public partial class InstructorSendToEmail : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnsendemail_Click(object sender, EventArgs e)
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
                            smtp.Credentials = new System.Net.NetworkCredential("capstoners.amber@gmail.com", "ppfbcakbcllnqxjf");
                            smtp.EnableSsl = true;
                            MailMessage msg = new MailMessage();
                            msg.Subject = "Reset Password Link";
                            msg.Body += "Hello, " + email + "\n You requested for a reset in your password, just click the link below to complete the process.\n";
                            msg.Body += "Click here 'https://localhost:44311/Pages/Users/Faculty/InstructorForgotPassword.aspx?forgot_otp=" + AMBER.URLEncryption.GetencryptedQueryString(forgot_otp) + "\n";
                            string toaddress = email;
                            msg.To.Add(toaddress);
                            string fromaddress = "<capstoners.amber@gmail.com>";

                            msg.From = new MailAddress(fromaddress);
                            try
                            {
                                smtp.Send(msg);
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SendSuccess()", true);
                                txtemail.Text = String.Empty;
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
                            txtemail.Text = String.Empty;
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
                            txtemail.Text = null;
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