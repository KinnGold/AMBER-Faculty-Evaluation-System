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
    public partial class SubscriptionDetails : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
        }

        protected void soloprembtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44311/Pages/SubscriptionMini.aspx");
        }

        protected void dualprembtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44311/Pages/SubscriptionMedium.aspx");
        }

        protected void packprembtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://localhost:44311/Pages/SubscriptionLarge.aspx");
        }
    }
}