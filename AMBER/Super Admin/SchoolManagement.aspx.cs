using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.Super_Admin
{
    public partial class SchoolManagement : System.Web.UI.Page
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM SCHOOL_TABLE", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    schoolGridView.DataSource = dr;
                    schoolGridView.DataBind();
                }
            }
        }

        protected void schoolGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(schoolGridView.DataKeys[e.RowIndex].Value.ToString());
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM SCHOOL_TABLE WHERE Id='" + id + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','School is deleted with ID: " + id + "', 'success')", true);
                            schoolGridView.EditIndex = -1;
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

        protected void schoolGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            schoolGridView.EditIndex = -1;
            GVBind();
        }

        protected void schoolGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            schoolGridView.EditIndex = e.NewEditIndex;
            GVBind();
        }

        protected void schoolGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(schoolGridView.DataKeys[e.RowIndex].Value.ToString());
            TextBox name = (TextBox)schoolGridView.Rows[e.RowIndex].FindControl("txtSchoolName");
            TextBox phone = (TextBox)schoolGridView.Rows[e.RowIndex].FindControl("txtSchoolPhone");
            TextBox address = (TextBox)schoolGridView.Rows[e.RowIndex].FindControl("txtSchoolAddress");
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SCHOOL_TABLE"
                         + " SET"
                         + " school_name = @name," 
                         + " school_phone = @phone," 
                         + " address = @address"
                         + " WHERE Id = '" + id + "';";
                        cmd.Parameters.AddWithValue("@name", name.Text);
                        cmd.Parameters.AddWithValue("@phone", phone.Text);
                        cmd.Parameters.AddWithValue("@address", address.Text);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
                            schoolGridView.EditIndex = -1;
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
    }
}