using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace AMBER.Pages.Users.Dean
{
    public partial class DeanSubjectManage : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Pages/Users/UsersLogin.aspx");
            }
            if (!IsPostBack)
            {
                GVbind();
                populateDDL();
            }
        }

        void populateDDL()
        {
            string selectme = "-department-";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT dept_id, dept_name FROM DEPARTMENT_TABLE WHERE SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL AND dept_Id = '" + Session["dept"].ToString() + "'";
                        ddlInsertDep.DataValueField = "dept_id";
                        ddlInsertDep.DataTextField = "dept_name";
                        ddlInsertDep.DataSource = cmd.ExecuteReader();
                        ddlInsertDep.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlInsertDep.Items.Add(selectme);
            ddlInsertDep.Items.FindByText(selectme.ToString()).Selected = true;
        }

        protected void btnAddsubject_Click(object sender, EventArgs e)
        {
            var subcode = txtsubcode.Text;
            var subname = txtsubname.Text;
            var unit = txtUnit.Text;
            var dept = ddlInsertDep.SelectedValue;
            if (CheckifSubjectExists(subcode, subname, unit, dept))
            {
                clear();
                Session["temp"] = "Subject";
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
                            cmd.CommandText = "INSERT INTO SUBJECT_TABLE(subject_code, subject_name, unit, dept_id, school_id)"
                                + " VALUES("
                                + "@code,"
                                + "@name,"
                                + "@unit,"
                                + "@dept,"
                                + "@school)";
                            cmd.Parameters.AddWithValue("@code", subcode);
                            cmd.Parameters.AddWithValue("@name", subname);
                            cmd.Parameters.AddWithValue("@unit", unit);
                            cmd.Parameters.AddWithValue("@dept", dept);
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                clear();
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
                                GVbind();
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
        public void clear()
        {
            txtsubcode.Text = String.Empty;
            txtsubname.Text = String.Empty;
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SUBJECT_TABLE JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id = SUBJECT_TABLE.dept_id WHERE SUBJECT_TABLE.SCHOOL_ID = @schoolid AND SUBJECT_TABLE.isDeleted IS NULL AND SUBJECT_TABLE.dept_id = '" + Session["dept"].ToString() + "'", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
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
            TextBox subcode = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtSubCode");
            TextBox subname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtSubName");
            TextBox unit = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtUNIT");
            DropDownList depcode = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlDepCode");
            if (CheckifSubjectExists(subcode.Text, subname.Text, unit.Text, depcode.SelectedValue))
            {
                clear();
                Session["temp"] = "Subject";
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
                            cmd.CommandText = "UPDATE SUBJECT_TABLE"
                                + " SET"
                                + " SUBJECT_CODE = @code,"
                                + " SUBJECT_NAME = @name,"
                                + " UNIT = @unit,"
                                + " DEPT_ID = @depcode"
                                + " WHERE subject_Id = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
                            cmd.Parameters.AddWithValue("@code", subcode.Text);
                            cmd.Parameters.AddWithValue("@name", subname.Text);
                            cmd.Parameters.AddWithValue("@unit", unit.Text);
                            cmd.Parameters.AddWithValue("@depcode", depcode.SelectedValue.ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "UpdateSuccess()", true);
                                GridView1.EditIndex = -1;
                                GVbind();
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVbind();
        }

        bool isSubjectReferenced(int id, string table, string pk)
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
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + "='" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        protected int countaffectrows(int id, string table, string pk)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(" + pk + ") AS COUNT FROM " + table + " WHERE " + pk + "='" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = Int32.Parse(reader["COUNT"].ToString());
                        }
                        else
                        {
                            id = 0;
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
        public string getIDinfo(int id, string table, string pk, string infoNeed)
        {
            var value = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + " = '" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            value = reader[infoNeed].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return value;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            string name = getIDinfo(id, "SUBJECT_TABLE", "subject_id", "subject_code");
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        //"DELETE FROM SUBJECT_TABLE WHERE ID ='" + id + "' AND CAMPUS_ID = '" + Session["campus"].ToString() + "'";
                        if (isSubjectReferenced(id, "SCHEDULE_TABLE", "subject_id"))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','To avoid this error, you need to delete master records first on <b>SCHEDULE</b> records with <b>" + name + "</b> and after that you can delete this record. \"<b>Schedule</b> row/s: " + countaffectrows(id, "SCHEDULE_TABLE", "subject_id") + "\"', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE SUBJECT_TABLE SET isDeleted= @delete WHERE subject_Id ='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@delete", DateTime.Now);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','subject <b>" + name + "</b> is deleted successfully', 'success')", true);
                                GridView1.EditIndex = -1;
                                GVbind();
                            }
                            //UPDATE
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

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVbind();
        }

        bool CheckifSubjectExists(string code, string name, string unit, string dept)
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
                        cmd.CommandText = "SELECT * FROM SUBJECT_TABLE WHERE subject_code='" + code + "' AND subject_name='" + name + "' AND unit='" + unit + "' AND dept_id='" + dept + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL AND isDeleted IS NULL";
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
    }
}