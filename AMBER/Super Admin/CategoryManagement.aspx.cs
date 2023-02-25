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
    public partial class CategoryManagement : System.Web.UI.Page
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
                GVbind();
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
                GVbind();
            }
            else if (DropDownSchool.SelectedItem.Text == "Select School")
            {
                GridView1.EditIndex = -1;
                GVbind();
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
                            cmd.CommandText = "SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name, CONSTRUCTOR_TABLE.description, CONSTRUCTOR_TABLE.weight, CONSTRUCTOR_TABLE.role, CONSTRUCTOR_TABLE.school_id, CONSTRUCTOR_TABLE.isDeleted, SEMESTER_TABLE.version, SEMESTER_TABLE.semester_id,(SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER, SCHOOL_TABLE.Id AS sc_id, SCHOOL_TABLE.school_name FROM CONSTRUCTOR_TABLE JOIN SEMESTER_TABLE ON CONSTRUCTOR_TABLE.semester_id = SEMESTER_TABLE.semester_id JOIN SCHOOL_TABLE ON SEMESTER_TABLE.school_id = SCHOOL_TABLE.Id WHERE SCHOOL_TABLE.Id='" + DropDownSchool.SelectedValue + "'";
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

        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name, CONSTRUCTOR_TABLE.description, CONSTRUCTOR_TABLE.weight, CONSTRUCTOR_TABLE.role, CONSTRUCTOR_TABLE.school_id, CONSTRUCTOR_TABLE.isDeleted, SEMESTER_TABLE.version, SEMESTER_TABLE.semester_id,(SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER, SCHOOL_TABLE.Id AS sc_id, SCHOOL_TABLE.school_name FROM CONSTRUCTOR_TABLE JOIN SEMESTER_TABLE ON CONSTRUCTOR_TABLE.semester_id = SEMESTER_TABLE.semester_id JOIN SCHOOL_TABLE ON SEMESTER_TABLE.school_id = SCHOOL_TABLE.Id", db);
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
            TextBox name = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtname");
            TextBox desc = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtdesc");
            TextBox weight = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtweight");
            DropDownList group = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDLgroup");
            DropDownList ver = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSEMESTER");
            DropDownList school = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlschool");

            if (cannotBeUpdated(id) == true)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Update Record!','This Constructor Has Been Added to The Evaluation', 'warning')", true);
                GridView1.EditIndex = -1;
                GVbind();
            }
            else
            {
                if (getthisROWweight(id.ToString()) == "" || getthisROWweight(id.ToString()) == null)
                {
                    try
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE CONSTRUCTOR_TABLE"
                                 + " SET"
                                 + " weight = @weight"
                                 + " WHERE constructor_id = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
                                cmd.Parameters.AddWithValue("@weight", weight.Text);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    GridView1.EditIndex = -1;
                                    GVbind();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                        Response.Write("<pre>" + ex.ToString() + "</pre>");
                    }
                    if (!checkMAXweight(group.SelectedValue, (Convert.ToInt32(weight.Text) - Convert.ToInt32(getthisROWweight(id.ToString()))).ToString(), ver.SelectedValue))
                    {
                        try
                        {
                            using (var db = new SqlConnection(connDB))
                            {
                                db.Open();
                                using (var cmd = db.CreateCommand())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "UPDATE CONSTRUCTOR_TABLE"
                                     + " SET"
                                     + " weight = NULL"
                                     + " WHERE constructor_id = '" + id + "';";
                                    var ctr = cmd.ExecuteNonQuery();
                                    if (ctr >= 1)
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','The <b>weight</b> you just entered has exceed the maximum weight', 'warning')", true);
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
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Update Successfully','We only update your category <b>weight</b> because it was empty', 'success')", true);
                    }
                }
                else if (!CheckifConstructorExists(name.Text, group.SelectedValue, ver.SelectedValue) || getthisROWweight(id.ToString()) != weight.Text)
                {
                    if (checkMAXweight(group.SelectedValue, (Convert.ToInt32(weight.Text) - Convert.ToInt32(getthisROWweight(id.ToString()))).ToString(), ver.SelectedValue))
                    {
                        try
                        {
                            using (var db = new SqlConnection(connDB))
                            {
                                db.Open();
                                using (var cmd = db.CreateCommand())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "UPDATE CONSTRUCTOR_TABLE"
                                     + " SET"
                                     + " constructor_name = @name,"
                                     + " description = @desc,"
                                     + " weight = @weight,"
                                     + " role = @role," 
                                     + " school_id = @school,"
                                     + " semester_id = @version"
                                     + " WHERE constructor_id = '" + id + "';";
                                    cmd.Parameters.AddWithValue("@name", name.Text);
                                    cmd.Parameters.AddWithValue("@desc", desc.Text);
                                    cmd.Parameters.AddWithValue("@weight", weight.Text);
                                    cmd.Parameters.AddWithValue("@role", group.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@school", school.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@version", ver.SelectedValue.ToString());
                                    var ctr = cmd.ExecuteNonQuery();
                                    if (ctr >= 1)
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
                                        GridView1.EditIndex = -1;
                                        GVbind();
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
                    else
                    {

                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Max weight Exceeded!','the weight given exceeded the max Weight of <b>100%</b> on this semester <b>" + getSemesterName(ver.SelectedValue) + "</b> Assigned on <b>" + group.SelectedValue + "</b> current Current Total weight <b>" + getWeightCurrentPercent(group.SelectedValue, ver.SelectedValue) + "%</b> your updated input weight <b>" + (Convert.ToInt32(weight.Text) - Convert.ToInt32(getthisROWweight(id.ToString()))).ToString() + "%</b> INVALID: <b>" + (Convert.ToInt32(getWeightCurrentPercent(group.SelectedValue, ver.SelectedValue)) + Convert.ToInt32((Convert.ToInt32(weight.Text) - Convert.ToInt32(getthisROWweight(id.ToString()))).ToString())) + "%</b>', 'warning')", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "exist()", true);
                }
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVbind();
        }

        public string getSemesterName(string id)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT (SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER FROM SEMESTER_TABLE WHERE semester_id = '" + id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader["SEMESTER"].ToString();
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

        public string getthisROWweight(string id)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT weight FROM CONSTRUCTOR_TABLE WHERE constructor_id='" + id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader["weight"].ToString();
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

        public string getWeightCurrentPercent(string role, string semester)
        {
            string current = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SUM(weight) as SUM FROM CONSTRUCTOR_TABLE WHERE role='" + role + "' AND semester_id = '" + semester + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            current = reader["SUM"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return current;
        }

        public bool checkMAXweight(string role, string weight, string semester)
        {
            var chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SUM(weight) as SUM FROM CONSTRUCTOR_TABLE WHERE role='" + role + "' AND semester_id = '" + semester + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if ((Convert.ToInt32(reader["SUM"]) + Convert.ToInt32(weight)) <= 100)
                            {
                                chkr = true;
                            }
                            else
                            {
                                chkr = false;
                            }
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVbind();
        }

        bool isConstructorReferenced(int id, string table, string pk)
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
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + "='" + id + "'";
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
                        cmd.CommandText = "SELECT COUNT(" + pk + ") AS COUNT FROM " + table + " WHERE " + pk + "='" + id + "'";
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
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + " = '" + id + "'";
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
            string name = getIDinfo(id, "CONSTRUCTOR_TABLE", "constructor_id", "constructor_name");
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        if (isConstructorReferenced(id, "INDICATOR_TABLE", "constructor_id"))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','To avoid this error, you need to delete master records first on <b>QUESTIONS</b> records with <b>" + name.Replace("\'", String.Empty) + "</b> and after that you can delete this record. \"<b>Questions</b> row/s: " + countaffectrows(id, "INDICATOR_TABLE", "constructor_id") + "\"', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE CONSTRUCTOR_TABLE SET isDeleted= @delete WHERE constructor_Id ='" + id + "'";
                            cmd.Parameters.AddWithValue("@delete", DateTime.Now);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','category <b>" + name + "</b> is deleted successfully', 'success')", true);
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
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }
        bool CheckifConstructorExists(string name, string group, string semester)
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
                        cmd.CommandText = "SELECT * FROM CONSTRUCTOR_TABLE WHERE constructor_name='" + name + "' AND role='" + group + "' AND semester_id='" + semester + "'";
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
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }

        bool cannotBeUpdated(int id)
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
                        cmd.CommandText = "SELECT INDICATOR_TABLE.constructor_id FROM INDICATOR_TABLE JOIN EVALUATION_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id";
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
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }
    }
}