using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;


namespace AMBER.Pages.Users.Faculty
{
    public partial class EvaluationResult : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["role"] == null)
            {
                Response.Redirect("/Pages/Users/UsersLogin.aspx");
            }
            if (!IsPostBack)
            {
                GVbind();
            }
            GridViewHelper helper = new GridViewHelper(this.GridView1);
            helper.RegisterGroup("constructor_name", true, true);
            helper.GroupHeader += new GroupEvent(helper_GroupHeader);
            helper.RegisterSummary("AVERAGE", SummaryOperation.Avg, "constructor_name");
            helper.GroupSummary += new GroupEvent(helper_GroupSummary);
            helper.ApplyGroupSort();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AutoRedirect();
        }

        public void AutoRedirect()
        {
            int int_MilliSecondsTimeOut = this.SessionLengthMinutes * 60000;
            var sch_id = Session["school"].ToString();
            string str_Script = @"
               <script type='text/javascript'> 
                   intervalset = window.setInterval('Redirect()'," +
                       int_MilliSecondsTimeOut.ToString() + @");
                   function Redirect()
                   {
                       window.location.href='https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(sch_id) + @"';
                   }
               </script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", str_Script);
        }

        public int SessionLengthMinutes
        {
            get { return Session.Timeout; }
        }

        private void helper_GroupSummary(string groupName, object[] values, GridViewRow row)
        {
            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row.Cells[0].Text = "Average";
        }

        private void helper_GroupHeader(string groupName, object[] values, GridViewRow row)
        {
            if (groupName == "constructor_name")
            {

                row.BackColor = Color.Orange;
                row.Cells[0].Font.Bold = true;
                row.Cells[0].Font.Size = 15;
                //row.Cells[0].Font.Size=FontUnit.Medium;
                //row.Cells[0].Text = alp++ + row.Cells[0].Text;
                //row.ForeColor = Color.White;
            }
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name, INDICATOR_TABLE.indicator_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) AS AVERAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE EVALUATION_TABLE.evaluatee_id = @instructor AND EVALUATION_TABLE.school_id=@school_id GROUP BY CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name, INDICATOR_TABLE.indicator_name ORDER BY CONSTRUCTOR_TABLE.constructor_id", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                cmd.Parameters.AddWithValue("@instructor", Session["insID"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            GVbind();
        }
    }
}