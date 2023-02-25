using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.Pages
{
    public partial class SubscriptionLargeSuccess : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime OneMonth = now.AddMonths(6);
            var school_id = Session["school"].ToString();
            var admin = Session["id"].ToString();
            DateTime startDate = now;
            string type = "Premium";

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
                            + " status = @status "
                            + "WHERE admin_id ='" + admin + "' AND school_id='" + school_id + "'";
                        cmd.Parameters.AddWithValue("@type", type.ToString());
                        cmd.Parameters.AddWithValue("@plan", "Large");
                        cmd.Parameters.AddWithValue("@start", now);
                        cmd.Parameters.AddWithValue("@end", OneMonth);
                        cmd.Parameters.AddWithValue("@status", 1);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Response.Redirect("AdminLandingPage.aspx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert ('Under Maintenance')</script><pre>" + ex.ToString() + "</pre>");
            }
        }
    }
}