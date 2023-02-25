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

namespace AMBER.Pages
{
    public partial class CancelSubscription : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                }
                if (Request.QueryString["sub_id"] != null)
                {
                    try
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "SELECT * FROM SUBSCRIPTION_TABLE WHERE school_id = '" + Session["school"].ToString() + "' AND admin_id='" + Session["id"].ToString() + "'";
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
            }
        }

        protected void adminprofilebtn_Click(object sender, EventArgs e)
        {
            var username = Session["user"].ToString();
            Response.Redirect("https://localhost:44311/Pages/AdminProfile.aspx?username=" + username + "");
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime OneMonth = now.AddMonths(1);
            var school_id = Session["school"].ToString();
            var admin = Session["id"].ToString();
            DateTime startDate = now;
            string type = "Free";
            string plan = "Not Subscribed";
            var reason = rblReason.SelectedItem.Text;
            var other = txtotherreason.Text;

            if (rblReason.SelectedIndex >= 0)
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE SUBSCRIPTION_TABLE SET"
                                + " subscription_type = @type,"
                                + " subscription_plan = @plan,"
                                + " startDate = @start,"
                                + " endDate = @end,"
                                + " status = @status,"
                                + "cancellation_reason = @reason"
                                + " WHERE admin_id ='" + admin + "' AND school_id='" + school_id + "'";
                            cmd.Parameters.AddWithValue("@type", type.ToString());
                            cmd.Parameters.AddWithValue("@plan", plan.ToString());
                            cmd.Parameters.AddWithValue("@start", now);
                            cmd.Parameters.AddWithValue("@end", OneMonth);
                            cmd.Parameters.AddWithValue("@status", 1);
                            cmd.Parameters.AddWithValue("@reason", reason);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SubmittedSuccessful()", true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert ('Under Maintenance')</script><pre>" + ex.ToString() + "</pre>");
                }
            }
            else if (rblReason.SelectedIndex == -1)
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE SUBSCRIPTION_TABLE SET"
                                + " subscription_type = @type,"
                                + " subscription_plan = @plan,"
                                + " startDate = @start,"
                                + " endDate = @end,"
                                + " status = @status,"
                                + " cancellation_reason= @reason"
                                + "WHERE admin_id ='" + admin + "' AND school_id='" + school_id + "'";
                            cmd.Parameters.AddWithValue("@type", type.ToString());
                            cmd.Parameters.AddWithValue("@plan", plan.ToString());
                            cmd.Parameters.AddWithValue("@start", now);
                            cmd.Parameters.AddWithValue("@end", OneMonth);
                            cmd.Parameters.AddWithValue("@status", 1);
                            cmd.Parameters.AddWithValue("@reason", other);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SubmittedSuccessful()", true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert ('Under Maintenance')</script><pre>" + ex.ToString() + "</pre>");
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "ErrorReason()", true);
            }
        }
    }
}