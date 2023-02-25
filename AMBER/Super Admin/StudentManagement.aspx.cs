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
    public partial class StudentManagement : System.Web.UI.Page
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
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM SCHOOL_TABLE ORDER BY school_name ASC";
                            DropDownSchool.DataValueField = "Id";
                            DropDownSchool.DataTextField = "school_name";
                            DropDownSchool.DataSource = cmd.ExecuteReader();
                            DropDownSchool.DataBind();
                            DropDownSchool.Items.Insert(0, new ListItem("ALL", ""));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                DropDownSchool.Items.Add("Select School");
                DropDownSchool.Items.FindByText("Select School").Selected = true;
            }
        }

        protected void DropDownSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownSchool.SelectedItem.Text == "ALL")
            {
                GridView1.EditIndex = -1;
                GVBind();
            }
            else if (DropDownSchool.SelectedItem.Text == "Select School")
            {
                GridView1.EditIndex = -1;
                GVBind();
            }
            else
            {
                try
                {
                    DataTable dt = new DataTable();
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT DISTINCT stud.Id,stud.stud_id,stud.fname, stud.lname,stud.mname,stud.email,stud.phonenum,stud.password, stud.role,stud.section_id,stud.school_id,stud.isDeleted,stud.forgot_otp,stud.verify_code, stud.isVerified,stud.lock_status, stud.lockdatetime, sch.Id AS sc_id, sch.school_name, sec.section_id AS sec_id, sec.section_name FROM STUDENT_TABLE AS stud JOIN SCHOOL_TABLE AS sch ON stud.school_id = sch.Id JOIN SECTION_TABLE AS sec ON sec.section_id = stud.section_id WHERE sch.Id='" + DropDownSchool.SelectedValue + "'";
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                        }
                        db.Close();
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = GridView1.DataKeys[e.RowIndex].Value.ToString();
            TextBox fname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtfname");
            TextBox mname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtmname");
            TextBox lname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtlname");
            TextBox email = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtemail");
            TextBox cnum = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtcnum");
            TextBox role = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtrole");
            DropDownList depschool = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlschool");
            DropDownList depsection = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlsection");
            CheckBox check = (CheckBox)GridView1.Rows[e.RowIndex].FindControl("CheckBox2");
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE STUDENT_TABLE"
                         + " SET"
                         + " lname = @lname,"
                         + " fname = @fname,"
                         + " mname = @mname,"
                         + " email = @email,"
                         + " phonenum = @cnum,"
                         + " role = @role," 
                         + " section_id = @section," 
                         + " school_id = @school,"
                         + " lock_status = @lock"
                         + " WHERE Id = '" + id + "';";
                        cmd.Parameters.AddWithValue("@fname", fname.Text);
                        cmd.Parameters.AddWithValue("@mname", mname.Text);
                        cmd.Parameters.AddWithValue("@lname", lname.Text);
                        cmd.Parameters.AddWithValue("@email", email.Text);
                        cmd.Parameters.AddWithValue("@cnum", cnum.Text);
                        cmd.Parameters.AddWithValue("@role", role.Text);
                        cmd.Parameters.AddWithValue("@section", depsection.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@school", depschool.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@lock", check.Checked);
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
                        cmd.CommandText = "DELETE FROM STUDENT_TABLE WHERE Id='" + id + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Student is deleted with Number: " + id + "', 'success')", true);
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

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVBind();
        }

        public void GVBind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT stud.Id,stud.stud_id,stud.fname, stud.lname,stud.mname,stud.email,stud.phonenum,stud.password, stud.role,stud.section_id,stud.school_id,stud.isDeleted,stud.forgot_otp,stud.verify_code, stud.isVerified,stud.lock_status, stud.lockdatetime, sch.Id AS sc_id, sch.school_name, sec.section_id AS sec_id, sec.section_name FROM STUDENT_TABLE AS stud JOIN SCHOOL_TABLE AS sch ON stud.school_id = sch.Id JOIN SECTION_TABLE AS sec ON sec.section_id = stud.section_id", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

    }
}