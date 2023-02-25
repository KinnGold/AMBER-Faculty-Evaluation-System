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
    public partial class ContactDevelopers : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var school_name = txtschoolName.Text;
            string msg = "Please add my School <b>" + school_name + "</b> , thank you very much!";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO SUPERADMINNOTIFICATIONS_TABLE(message, isRead, dateTimeSent)VALUES("
                            + "@msg," 
                            + "@read,"
                            + "@date)";
                        cmd.Parameters.AddWithValue("@msg", msg);
                        cmd.Parameters.AddWithValue("@read", false);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SubmittedSuccessful()", true);
                            clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        void clear()
        {
            txtschoolName.Text = String.Empty;
        }
    }
}