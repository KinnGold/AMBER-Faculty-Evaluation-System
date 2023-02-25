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
    public partial class AccountVerification : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int code = 0;
                var ver_code = Request.QueryString["verify_code"].ToString();

                if (Request.QueryString["verify_code"] != null)
                {
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["verify_code"].ToString()), out code))
                    {
                        try
                        {
                            using (var db = new SqlConnection(connDB))
                            {
                                db.Open();
                                using (var cmd = db.CreateCommand())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE verify_code='" + AMBER.URLEncryption.GetdecryptedQueryString(ver_code) + "'";
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        plcinput.Visible = true;
                                        db.Close();
                                    }
                                    else
                                    {
                                        plcinput.Visible = false;
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

        protected void btnverifynow_Click(object sender, EventArgs e)
        {
            var ver_code = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["verify_code"].ToString());
            int ver = 1;
            var code = txtcode.Text;

            if (ver_code.ToString() != code)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "Verifyerror()", true);
                code = String.Empty;
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
                            cmd.CommandText = "UPDATE STUDENT_TABLE SET isVerified=@ver WHERE verify_code='" + ver_code + "'";
                            cmd.Parameters.AddWithValue("@ver", ver);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "VerificationSuccess()", true);
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