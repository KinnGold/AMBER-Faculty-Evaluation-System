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
using System.Text;

namespace AMBER.Super_Admin
{
    public partial class SchoolUpload : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        int id;
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM SCHOOLLIST_TABLE", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    schoolGridView.DataSource = dr;
                    schoolGridView.DataBind();
                }
            }
        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            var lineNumber = 0;
            var max = 5;
            string id = null;

            if (FileUpload1.HasFile)
            {
                String path = Server.MapPath("~/CSVFiles/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                String str = Server.HtmlEncode(FileUpload1.FileName);
                String ext = Path.GetExtension(str);
                if ((ext == ".csv") || (ext == ".txt"))
                {
                    FileUpload1.SaveAs(path);
                    using (StreamReader reader = new StreamReader(path))
                    {
                        while (!reader.EndOfStream)
                        {
                            //cmd.Parameters.Clear();
                            var line = reader.ReadLine();

                            if (lineNumber != 0)
                            {
                                var values = line.Split(',');
                                if (!CheckifSchoolExists(getThisSchool(id, values[0])))
                                {
                                    insertSchool(values, lineNumber);
                                }
                            }
                            lineNumber++;
                        }
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Insert Success!','File \"" + str + "\" was uploaded and You have Subscribed Successfully!', 'success')", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','File does not match with .csv format', 'warning')", true);
                    //Label1.Text = "File does not match with .csv format";
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "errorFU()", true);
            }
        }

        void insertSchool(string[] values, int count)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO SCHOOLLIST_TABLE(school_name, school_address)"
                            + " VALUES(@name)," 
                            + " VALUES(@address)";
                        cmd.Parameters.AddWithValue("@name", values[0]);
                        cmd.Parameters.AddWithValue("@address", values[1]);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        bool CheckifSchoolExists(string school_name)
        {
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SCHOOLLIST_TABLE WHERE school_name='" + school_name + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            chkr = true;
                        }
                        else
                        {
                            chkr = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }

        public string getThisSchool(string id, string school_name)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SCHOOLLIST_TABLE WHERE school_name='" + school_name + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader["Id"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return id;
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
                        cmd.CommandText = "DELETE FROM SCHOOLLIST_TABLE WHERE Id='" + id + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Admin is deleted with ID: " + id + "', 'success')", true);
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            string schoolName = txtschoolName.Text;
            if (CheckifSchoolExists(schoolName))
            {
                Session["temp"] = "School";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
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
                            cmd.CommandText = "INSERT INTO SCHOOLLIST_TABLE(school_name, school_address)"
                                + " VALUES("
                                + "@name," 
                                + "@address)";
                            cmd.Parameters.AddWithValue("@name", schoolName.ToString());
                            cmd.Parameters.AddWithValue("@address", txtSchoolAddress.Text.ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                clear();
                                Session["temp"] = "School";
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
                                //Response.Write("<script>alert ('Department Added Successfully')</script>");
                                GVBind();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
            }
        }

        void clear()
        {
            txtschoolName.Text = String.Empty;
        }

        protected void schoolGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(schoolGridView.DataKeys[e.RowIndex].Value.ToString());
            TextBox name = (TextBox)schoolGridView.Rows[e.RowIndex].FindControl("txtName");
            TextBox address = (TextBox)schoolGridView.Rows[e.RowIndex].FindControl("txtAddress");
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SCHOOLLIST_TABLE"
                         + " SET"
                         + " school_name = @name," 
                         + " school_address = @address"
                         + " WHERE Id = '" + id + "';";
                        cmd.Parameters.AddWithValue("@name", name.Text);
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
    }
}