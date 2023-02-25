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

namespace AMBER.Pages.Users.Student
{
    public partial class StudentForgotPassword : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int code = 0;
                var forgot_otp = Request.QueryString["forgot_otp"].ToString();

                if (Request.QueryString["forgot_otp"] != null)
                {
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["forgot_otp"].ToString()), out code))
                    {
                        try
                        {
                            using (var db = new SqlConnection(connDB))
                            {
                                db.Open();
                                using (var cmd = db.CreateCommand())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE forgot_otp='" + AMBER.URLEncryption.GetdecryptedQueryString(forgot_otp) + "'";
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        newpassplc.Visible = true;
                                        expiredplc.Visible = false;
                                        db.Close();
                                    }
                                    else
                                    {
                                        newpassplc.Visible = false;
                                        expiredplc.Visible = true;
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
                }
            }
        }
        void clear()
        {
            txtnewpassword.Text = String.Empty;
            txtconfirmpass.Text = String.Empty;
        }
        protected void btnconfirm_Click(object sender, EventArgs e)
        {
            var forgot_otp = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["forgot_otp"].ToString());
            var confirmpass = txtconfirmpass.Text;
            var newpass = txtnewpassword.Text;

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
                            cmd.CommandText = "UPDATE STUDENT_TABLE SET password=@pass WHERE forgot_otp='" + forgot_otp + "'";
                            cmd.Parameters.AddWithValue("@pass", newpass);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
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
        }
    }
}