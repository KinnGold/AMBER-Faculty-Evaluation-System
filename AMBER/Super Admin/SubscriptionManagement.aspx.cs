using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.Super_Admin
{
    public partial class SubscriptionManagement : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Super%20Admin/SuperAdminLogin.aspx");
            }
            if (!IsPostBack)
            {
                GVBind();
            }
        }

        public void GVBind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT subs.sub_id, subs.subscription_type, subs.subscription_plan, subs.startDate, subs.endDate, subs.status, subs.school_id, sch.Id AS sch_id, sch.school_name, admin.Id AS admin_id, (admin.fname +' '+ SUBSTRING(admin.mname, 1, 1)+'. '+admin.lname) AS NAME FROM SUBSCRIPTION_TABLE AS subs JOIN SCHOOL_TABLE AS sch ON subs.school_id = sch.Id JOIN ADMIN_TABLE AS admin ON sch.Id = admin.school_id", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox type = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtsubtype");
            TextBox plan = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtsubplan");
            TextBox start = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtstart");
            TextBox end = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtend");
            CheckBox status = (CheckBox)GridView1.Rows[e.RowIndex].FindControl("CheckBox1");

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SUBSCRIPTION_TABLE"
                         + " SET"
                         + " subscription_type = @type," 
                         + " subscription_plan = @plan,"
                         + " startDate = @start," 
                         + " endDate = @end," 
                         + " status = @status"
                         + " WHERE sub_id = '" + id + "';";
                        cmd.Parameters.AddWithValue("@type", type.Text);
                        cmd.Parameters.AddWithValue("@plan", plan.Text);
                        cmd.Parameters.AddWithValue("@start", start.Text);
                        cmd.Parameters.AddWithValue("@end", end.Text);
                        cmd.Parameters.AddWithValue("@status", status.Checked);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
                            GridView1.EditIndex = -1;
                            GVBind();
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM SUBSCRIPTION_TABLE WHERE sub_id='" + id + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Subscription is deleted with ID: " + id + "', 'success')", true);
                            GridView1.EditIndex = -1;
                            GVBind();
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

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVBind();
        }
    }
}