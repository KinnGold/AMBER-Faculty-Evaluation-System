using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.BM
{
    public partial class AdminDepartmentPage : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("LoginPage.aspx");

            }
            if (!IsPostBack)
            {
                GVbind();
            }
        }

        protected void btnAddDepartment_Click(object sender, EventArgs e)
        {
            var dept_code = txtdeptCode.Text;
            var dept_name = txtdeptName.Text;

            if (CheckifDeptExists(dept_code, dept_name))
            {
                clear();
                Session["temp"] = "Department";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
                //Response.Write("<script>alert ('Department Already Exists!')</script>");
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
                            cmd.CommandText = "INSERT INTO DEPARTMENT_TABLE(dept_code, dept_name, school_id)"
                                + " VALUES("
                                + "@code,"
                                + "@name,"
                                + "@school)";
                            cmd.Parameters.AddWithValue("@code", dept_code);
                            cmd.Parameters.AddWithValue("@name", dept_name);
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                clear();
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
                                //Response.Write("<script>alert ('Department Added Successfully')</script>");
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
            txtdeptCode.Text = String.Empty;
            txtdeptName.Text = String.Empty;
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM DEPARTMENT_TABLE WHERE SCHOOL_ID = @schoolid AND isDeleted IS NULL", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    plcNoData.Visible = false;
                    plcData.Visible = true;
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
                else
                {
                    plcNoData.Visible = true;
                    plcData.Visible = false;
                }
            }
        }

        protected void btnedit_Click(object sender, EventArgs e)
        {
            btnAddDepartment.Visible = false;
            btnupdatedept.Visible = true;
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVbind();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox depcode = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtDepCode");
            TextBox depname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtDepName");
            if(CheckifDeptExists(depcode.Text, depname.Text))
            {
                clear();
                Session["temp"] = "Department";
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
                            cmd.CommandText = "UPDATE DEPARTMENT_TABLE"
                                + " SET"
                                + " DEPT_CODE = @code,"
                                + " DEPT_NAME = @name"
                                + " WHERE DEPT_ID = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
                            cmd.Parameters.AddWithValue("@code", depcode.Text);
                            cmd.Parameters.AddWithValue("@name", depname.Text);
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
                    Response.Write("<script>alert ('" + ex.ToString() + "')</script>");
                }
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
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
            string name = getIDinfo(id, "DEPARTMENT_TABLE", "dept_id", "dept_code");
            //int count = countaffectrows(id); //CASCADE DELETE
            //cascadeLogdelete(id);
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        if (isSubjectReferenced(id, "INSTRUCTOR_TABLE", "dept_id") || isSubjectReferenced(id, "COURSE_TABLE", "dept_id"))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','To avoid this error, you need to delete master records first on <b>COURSE</b> and <b>INSTRUCTOR</b> records with <b>" + name + "</b> and after that you can delete this record. \"<b>Course</b> row/s: " + countaffectrows(id, "COURSE_TABLE", "dept_id") + " and <b>Instructor</b> row/s: " + countaffectrows(id, "INSTRUCTOR_TABLE", "dept_id") + "\"', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE DEPARTMENT_TABLE SET isDeleted= @delete WHERE DEPT_ID ='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
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

        //void cascadeLogdelete(int id)
        //{
        //    try
        //    {
        //        using (var db = new SqlConnection(connDB))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "UPDATE COURSE_TABLE SET isDeleted = @delete FROM COURSE_TABLE JOIN DEPARTMENT_TABLE ON COURSE_TABLE.dept_id = DEPARTMENT_TABLE.dept_id WHERE DEPARTMENT_TABLE.dept_Id = '" + id+"' AND COURSE_TABLE.SCHOOL_ID='" + Session["school"].ToString() + "'";
        //                cmd.Parameters.AddWithValue("@delete", DateTime.Now);
        //                var ctr = cmd.ExecuteNonQuery();
        //                if (ctr >= 1)
        //                {
        //                    //GVbind();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
        //        Response.Write("<pre>" + ex.ToString() + "</pre>");
        //    }
        //}

        //protected int countaffectrows(int id)
        //{   
        //    int count = 0;
        //    try
        //    {
        //        using (var db = new SqlConnection(connDB))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "SELECT DEPARTMENT_TABLE.dept_Id, DEPARTMENT_TABLE.dept_code, COURSE_TABLE.course_id, COURSE_TABLE.dept_id FROM COURSE_TABLE JOIN DEPARTMENT_TABLE ON COURSE_TABLE.dept_id=DEPARTMENT_TABLE.dept_id WHERE  DEPARTMENT_TABLE.dept_id ='" + id + "' AND DEPARTMENT_TABLE.school_id='" + Session["school"].ToString() + "' AND DEPARTMENT_TABLE.isDeleted IS NULL";
        //                SqlDataReader reader = cmd.ExecuteReader();
        //                if (reader.HasRows)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        count++;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
        //        Response.Write("<pre>" + ex.ToString() + "</pre>");
        //    }
        //    Session["temp"] = count;
        //    return count;
        //}
        bool CheckifDeptExists(string code, string name)
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
                        cmd.CommandText = "SELECT * FROM DEPARTMENT_TABLE WHERE (dept_code='" + code + "' AND dept_name='" + name + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL)";
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