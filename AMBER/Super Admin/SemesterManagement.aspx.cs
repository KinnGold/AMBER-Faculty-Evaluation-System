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
    public partial class SemesterManagement : System.Web.UI.Page
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

        public void GVBind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT sem.semester_id, sem.description, sem.year, sem.status, sem.version, sem.evaluationStart, sem.evaluationEnd, sem.isDeleted, sch.Id AS sc_id, sch.school_name FROM SEMESTER_TABLE AS sem JOIN SCHOOL_TABLE AS sch on sem.school_id = sch.Id", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
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
                            cmd.CommandText = "SELECT sem.semester_id, sem.description, sem.year, sem.status, sem.version, sem.evaluationStart, sem.evaluationEnd, sem.isDeleted, sch.Id AS sc_id, sch.school_name FROM SEMESTER_TABLE AS sem JOIN SCHOOL_TABLE AS sch on sem.school_id = sch.Id WHERE sch.Id='" + DropDownSchool.SelectedValue + "'";
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
            DropDownList depschool = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlschool");
            TextBox desc = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtDesc");
            TextBox year = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtYear");
            DropDownList status = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlStatus");
            string ver = "v" + year.Text[2] + year.Text[3] + "-" + desc.Text[0];
            TextBox start = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtEvalStart");
            TextBox end = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtEvalEnd");

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SEMESTER_TABLE"
                         + " SET" 
                         + " description = @desc," 
                         + " year = @year," 
                         + " status = @status," 
                         + " version= @ver," 
                         + " evaluationStart = @start," 
                         + " evaluationEnd = @end,"
                         + " school_id = @school"
                         + " WHERE semester_id = '" + id + "';";
                        cmd.Parameters.AddWithValue("@desc", desc.Text);
                        cmd.Parameters.AddWithValue("@year", year.Text);
                        cmd.Parameters.AddWithValue("@status", status.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@ver", ver);
                        cmd.Parameters.AddWithValue("@start", Convert.ToDateTime(start.Text));
                        cmd.Parameters.AddWithValue("@end", Convert.ToDateTime(end.Text));
                        cmd.Parameters.AddWithValue("@school", depschool.SelectedValue.ToString());
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
                        cmd.CommandText = "DELETE FROM SEMESTER_TABLE WHERE semester_id='" + id + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Semester is deleted with ID: " + id + "', 'success')", true);
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