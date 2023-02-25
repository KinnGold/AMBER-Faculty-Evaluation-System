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
    public partial class TwoFactorAuthentication : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int code = 0;
                var TFA_code = Request.QueryString["TFA_code"].ToString();


                if (Request.QueryString["TFA_code"] != null)
                {
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["TFA_code"].ToString()), out code))
                    {
                        try
                        {
                            using (var db = new SqlConnection(connDB))
                            {
                                db.Open();
                                using (var cmd = db.CreateCommand())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "SELECT * FROM SUPER_ADMIN_TABLE WHERE TFA_code='" + AMBER.URLEncryption.GetdecryptedQueryString(TFA_code) + "'";
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
            var TFA_code = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["TFA_code"].ToString());
            int approved = 1;
            var code = txtcode.Text.Trim();

            if (TFA_code.ToString() != code)
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
                            cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE SET login_approved=@approved WHERE TFA_code='" + TFA_code + "'";
                            cmd.Parameters.AddWithValue("@approved", approved);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Response.Redirect("https://localhost:44311/Super%20Admin/SuperAdminLandingPage.aspx");
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