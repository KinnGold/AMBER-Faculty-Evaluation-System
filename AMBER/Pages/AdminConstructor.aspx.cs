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
    public partial class AdminConstructor : System.Web.UI.Page
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
                populateddl();
                GVbind();
                
                lblStudentTotal.Text = "(student)Student Evaluate Total Weight: <b>" + getMAXweight("student")+"%</b>";
                lblInstructorTotal.Text = "(instructor)Peer Evaluate Total Weight: <b>" + getMAXweight("instructor") + "%</b>";
            }
        }
        void populateddl()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT semester_id,(description+' SY '+year) AS SEMESTER FROM SEMESTER_TABLE WHERE SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL ORDER BY year, description";
                        VersionDDL.DataValueField = "semester_id";
                        VersionDDL.DataTextField = "SEMESTER";
                        VersionDDL.DataSource = cmd.ExecuteReader();
                        VersionDDL.DataBind();
                    }

                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            string selectme = " ";
            VersionDDL.Items.Add(selectme);
            VersionDDL.Items.FindByText(selectme.ToString()).Selected = true;
            //try
            //{
            //    using (var db = new SqlConnection(connDB))
            //    {
            //        db.Open();
            //        using (var cmd = db.CreateCommand())
            //        {
            //            cmd.CommandType = CommandType.Text;
            //            cmd.CommandText = "SELECT DISTINCT dept_id, dept_name FROM DEPARTMENT_TABLE WHERE CAMPUS_ID = '" + Session["campus"].ToString() + "' AND isDeleted IS NULL";
            //            DepartmentDDL.DataValueField = "dept_id";
            //            DepartmentDDL.DataTextField = "dept_name";
            //            DepartmentDDL.DataSource = cmd.ExecuteReader();
            //            DepartmentDDL.DataBind();
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            //    Response.Write("<pre>" + ex.ToString() + "</pre>");
            //}
            //DepartmentDDL.Items.Add(selectme);
            //DepartmentDDL.Items.FindByText(selectme.ToString()).Selected = true;
            
        }

        protected void addConstructor_Click(object sender, EventArgs e)
        {
            var consname = txtConstructor.Text;
            var version = VersionDDL.SelectedValue;
            var group = ddlGroup.SelectedValue;
            var desc = txtDesc.InnerText;
            var weight = txtWeight.Text;
            //var selectcommand = " ";
            if (CheckifConstructorExists(consname,desc,group,version))
            {
                Session["temp"] = "Category";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "exist()", true);
                clear();
            }
            else
            {
                if(checkMAXweight(group,weight,version))
                {
                    try
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "INSERT INTO CONSTRUCTOR_TABLE(constructor_name, description, semester_id, weight, role, school_id)"
                                    + " VALUES("
                                    + "@name,"
                                    + "@desc,"
                                    + "@version,"
                                    + "@weight,"
                                    + "@role,"
                                    + "@school)";
                                cmd.Parameters.AddWithValue("@name", consname);
                                cmd.Parameters.AddWithValue("@desc", desc);
                                cmd.Parameters.AddWithValue("@version", version);
                                cmd.Parameters.AddWithValue("@weight", weight);
                                cmd.Parameters.AddWithValue("@role", group);
                                cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "InsertSuccess()", true);
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
                    if(weight != "" || weight !=null)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Max weight Exceeded!','the weight given exceeded the max Weight of <b>100%</b> on this semester <b>" + getSemesterName(version) + "</b> Assigned on <b>" + group + "</b> current Current Total weight <b>" + getWeightCurrentPercent(group, version) + "%</b> your input weight <b>" + weight + "%</b> INVALID: <b>" + (Convert.ToInt32(getWeightCurrentPercent(group, version)) + Convert.ToInt32(weight)) + "%</b>', 'warning')", true);
                    }
                }
            }
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
                        cmd.CommandText = "SELECT weight FROM CONSTRUCTOR_TABLE WHERE constructor_id='"+ id +"'";
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

        public string getWeightCurrentPercent(string role,string semester)
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
                        cmd.CommandText = "SELECT SUM(weight) as SUM FROM CONSTRUCTOR_TABLE WHERE role='"+ role +"' AND isDeleted IS NULL AND semester_id = '"+ semester + "' AND school_id='"+ Session["school"].ToString() +"'";
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

        public string getMAXweight(string role)
        {
            string total = null;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SUM(weight) as SUM FROM CONSTRUCTOR_TABLE WHERE role='" + role + "' AND isDeleted IS NULL AND school_id = '" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            total = reader["SUM"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return total;
        }
        public bool checkGroupChanged(int id, string role, string weight, string term)
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
                        cmd.CommandText = "SELECT * FROM CONSTRUCTOR_TABLE WHERE constructor_id = '" + id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader["role"].ToString() != role)
                            {
                                if (checkMAXweight(role, weight, term))
                                {
                                    chkr = false;
                                }
                                else
                                {
                                    chkr = true;
                                }
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
                        cmd.CommandText = "SELECT SUM(weight) as SUM FROM CONSTRUCTOR_TABLE WHERE role='" + role + "' AND isDeleted IS NULL AND semester_id = '" + semester + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if(reader["SUM"].ToString() == "" || (Convert.ToInt32(reader["SUM"]) + Convert.ToInt32(weight)) <= 100)
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

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox name = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtname");
            TextBox desc = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtdesc");
            TextBox weight = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtweight");
            DropDownList group = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("DDLgroup");
            DropDownList ver = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlSEMESTER");

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
                    if(!checkMAXweight(group.SelectedValue, (Convert.ToInt32(weight.Text) - Convert.ToInt32(getthisROWweight(id.ToString()))).ToString(), ver.SelectedValue))
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
                                     + " WHERE constructor_id = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
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
                else if (!CheckifConstructorExists(name.Text, desc.Text, group.SelectedValue, ver.SelectedValue) || getthisROWweight(id.ToString()) != weight.Text)
                {
                    if (checkMAXweight(group.SelectedValue, (Convert.ToInt32(weight.Text) - Convert.ToInt32(getthisROWweight(id.ToString()))).ToString(), ver.SelectedValue))
                    {
                        if(checkGroupChanged(id,group.SelectedValue,weight.Text,ver.SelectedValue))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','The Assigned group <b>" + group.SelectedValue +" weight</b> you just entered has exceed the maximum weight', 'warning')", true);
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
                                        cmd.CommandText = "UPDATE CONSTRUCTOR_TABLE"
                                         + " SET"
                                         + " constructor_name = @name,"
                                         + " description = @desc,"
                                         + " weight = @weight,"
                                         + " role = @role,"
                                         + " semester_id = @version"
                                         + " WHERE constructor_id = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
                                        cmd.Parameters.AddWithValue("@name", name.Text);
                                        cmd.Parameters.AddWithValue("@desc", desc.Text);
                                        cmd.Parameters.AddWithValue("@weight", weight.Text);
                                        cmd.Parameters.AddWithValue("@role", group.SelectedValue.ToString());
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
                    }
                    else
                    {

                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Max weight Exceeded!','the weight given exceeded the max Weight of <b>100%</b> on this semester <b>" + getSemesterName(ver.SelectedValue) + "</b> Assigned on <b>" + group.SelectedValue + "</b> current Current Total weight <b>" + getWeightCurrentPercent(group.SelectedValue, ver.SelectedValue) + "%</b> your updated input weight <b>" + (Convert.ToInt32(weight.Text) - Convert.ToInt32(getthisROWweight(id.ToString()))).ToString() + "%</b> INVALID: <b>" + (Convert.ToInt32(getWeightCurrentPercent(group.SelectedValue, ver.SelectedValue)) + Convert.ToInt32((Convert.ToInt32(weight.Text) - Convert.ToInt32(getthisROWweight(id.ToString()))).ToString())) + "%</b>', 'warning')", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "exist()", true);
                    clear();
                }
            }
            lblStudentTotal.Text = "(student)Student Evaluate Total Weight: <b>" + getMAXweight("student") + "%</b>";
            lblInstructorTotal.Text = "(instructor)Peer Evaluate Total Weight: <b>" + getMAXweight("instructor") + "%</b>";
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVbind();
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name, CONSTRUCTOR_TABLE.description, CONSTRUCTOR_TABLE.weight, CONSTRUCTOR_TABLE.role, CONSTRUCTOR_TABLE.school_id, SEMESTER_TABLE.version, SEMESTER_TABLE.semester_id,(SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER FROM CONSTRUCTOR_TABLE JOIN SEMESTER_TABLE ON CONSTRUCTOR_TABLE.semester_id = SEMESTER_TABLE.semester_id WHERE CONSTRUCTOR_TABLE.school_id = @schoolid AND CONSTRUCTOR_TABLE.isDeleted IS NULL", db);
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
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','To avoid this error, you need to delete master records first on <b>QUESTIONS</b> records with <b>" + name.Replace("\'",String.Empty) + "</b> and after that you can delete this record. \"<b>Questions</b> row/s: " + countaffectrows(id, "INDICATOR_TABLE", "constructor_id") + "\"', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE CONSTRUCTOR_TABLE SET isDeleted= @delete WHERE constructor_Id ='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
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
        bool CheckifConstructorExists(string name, string desc, string group, string semester)
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
                        cmd.CommandText = "SELECT * FROM CONSTRUCTOR_TABLE WHERE constructor_name='" + name + "' AND description = '"+ desc +"' AND role='" + group + "' AND semester_id='"+ semester +"' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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
                        cmd.CommandText = "SELECT INDICATOR_TABLE.constructor_id FROM INDICATOR_TABLE JOIN EVALUATION_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND INDICATOR_TABLE.constructor_id='" + id + "'";
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
        void clear()
        {
            txtConstructor.Text = String.Empty;
        }
    }
}