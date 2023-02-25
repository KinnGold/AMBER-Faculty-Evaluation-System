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
    public partial class SubscriptionExpired : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
        }

        protected void btnLater_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/AdminLandingPage.aspx");
        }

        protected void btnRenew_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SUBSCRIPTION_TABLE WHERE admin_id='" + Session["id"].ToString() + "' AND school_id='" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            string plan = reader["subscription_plan"].ToString();
                            if (plan == "Mini")
                            {
                                Response.Redirect("/Pages/SubscriptionMini.aspx");
                            }
                            else if (plan == "Medium")
                            {
                                Response.Redirect("/Pages/SubscriptionMedium.aspx");
                            }
                            else if (plan == "Large")
                            {
                                Response.Redirect("/Pages/SubscriptionLarge.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("/Pages/AdminLandingPage.aspx");
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
        }
    }
}