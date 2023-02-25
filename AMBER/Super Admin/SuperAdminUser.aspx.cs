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


namespace AMBER.Super_Admin
{
    public partial class SuperAdminUser : System.Web.UI.Page
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM SUPER_ADMIN_TABLE EXCEPT SELECT * FROM SUPER_ADMIN_TABLE WHERE Id='"+ Session["Id"].ToString() +"' AND username='"+ Session["user"].ToString() + "' AND password='"+ Session["pass"].ToString() + "'", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void addSuperAdmin_Click(object sender, EventArgs e)
        {
            var email = txtemail.Text;
            var fname = txtfname.Text;
            var lname = txtlname.Text;
            var mname = txtmname.Text;
            var contact = txtcnum.Text;
            var user = txtusername.Text;

            string fileName = HttpContext.Current.Server.MapPath(@"~/Pictures/usericon.png");
            byte[] bytes = null;
            Stream stream = System.IO.File.OpenRead(fileName);
            BinaryReader binaryreader = new BinaryReader(stream);
            bytes = binaryreader.ReadBytes((int)stream.Length);

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO SUPER_ADMIN_TABLE(username, password, first_name, middle_name, last_name, email, contact_no, profile_picture)VALUES("
                            + "@username," 
                            + "@password,"
                            + "@fname,"
                            + "@mname,"
                            + "@lname,"
                            + "@email,"
                            + "@contactno,"
                            + "@profile)";
                        cmd.Parameters.AddWithValue("@username", user);
                        cmd.Parameters.AddWithValue("@password", user);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mname", mname);
                        cmd.Parameters.AddWithValue("@lname", lname);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@contactno", contact);
                        cmd.Parameters.AddWithValue("@profile", bytes);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "InsertSuccess()", true);
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
                        cmd.CommandText = "DELETE FROM SUPER_ADMIN_TABLE WHERE Id='" + id + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Super Admin is deleted with ID: " + id + "', 'success')", true);
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
    }
}