using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace AMBER.Pages
{
    public partial class AdminInstructorPage : System.Web.UI.Page
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
                populateDDL();
                GVbind();
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
                        cmd.CommandText = "SELECT DISTINCT dept_id, dept_name FROM DEPARTMENT_TABLE WHERE SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        ddlDepartment.DataValueField = "dept_id";
                        ddlDepartment.DataTextField = "dept_name";
                        ddlDepartment.DataSource = cmd.ExecuteReader();
                        ddlDepartment.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT dept_id, dept_name FROM DEPARTMENT_TABLE WHERE SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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
            ddlDepartment.Items.Add(selectme);
            ddlDepartment.Items.FindByText(selectme.ToString()).Selected = true;
            ddlInsertDep.Items.Add(selectme);
            ddlInsertDep.Items.FindByText(selectme.ToString()).Selected = true;
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY ins_id)row_num,* FROM INSTRUCTOR_TABLE JOIN DEPARTMENT_TABLE ON INSTRUCTOR_TABLE.dept_id = DEPARTMENT_TABLE.dept_id WHERE INSTRUCTOR_TABLE.SCHOOL_ID = @schoolid AND INSTRUCTOR_TABLE.isDeleted IS NULL", db);
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

        protected void btnAddInstructor_Click(object sender, EventArgs e)
        {
            var id = txtID.Text;
            var lname = txtLname.Text;
            var fname = txtFname.Text;
            var mname = txtMName.Text;
            var email = txtEmail.Text;
            var department = ddlInsertDep.SelectedValue;
            var phone = txtPhonenum.Text;
            var role = ddlInsertRole.SelectedItem.Text;
            string fileName = HttpContext.Current.Server.MapPath(@"~/Pictures/usericon.png");
            byte[] bytes = null;
            Stream stream = System.IO.File.OpenRead(fileName);
            BinaryReader binaryreader = new BinaryReader(stream);
            bytes = binaryreader.ReadBytes((int)stream.Length);

            if (CheckifInsExists(id,lname,fname,mname,email))
            {
                clear();
                Session["temp"] = "Instructor";
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
                            cmd.CommandText = "INSERT INTO INSTRUCTOR_TABLE(INS_ID, LNAME, FNAME, MNAME, EMAIL, PHONENUM, PASSWORD, ROLE, SCHOOL_ID, DEPT_ID, ISVERIFIED, PROFILE_PICTURE, LOCK_STATUS)"
                                + " VALUES("
                                + "@id,"
                                + "@lname,"
                                + "@fname,"
                                + "@mname,"
                                + "@email,"
                                + "@phonenum,"
                                + "@password,"
                                + "@role,"
                                + "@school,"
                                + "@dept,"
                                + "@isVerified,"
                                + "@profile,"
                                + "@lock)";
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@lname", lname);
                            cmd.Parameters.AddWithValue("@fname", fname);
                            cmd.Parameters.AddWithValue("@mname", mname);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@phonenum", phone);
                            cmd.Parameters.AddWithValue("@password", id);
                            cmd.Parameters.AddWithValue("@role", role);//first row kay dean
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            cmd.Parameters.AddWithValue("@dept", department);
                            cmd.Parameters.AddWithValue("@isVerified", false);
                            cmd.Parameters.AddWithValue("@profile", bytes);
                            cmd.Parameters.AddWithValue("@lock", false);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                clear();
                                Session["temp"] = "Instructor";
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
            populateDDL();
            txtID.Text = String.Empty;
            txtLname.Text = String.Empty;
            txtFname.Text = String.Empty;
            txtMName.Text = String.Empty;
            txtPhonenum.Text = String.Empty;
            txtEmail.Text = String.Empty;
        }
        bool CheckifInsExists(string id, string lname, string fname, string mname, string email)
        {
            //var id = txtID.Text;
            //var email = txtEmail.Text;
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE ((INS_ID='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL) OR (fname = '" + fname + "' AND mname='" + mname + "' AND lname='" + lname + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL) OR (email = '" + email + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL))";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            chkr = true;
                            //Session["temp"] = "Instructor id:\"" + id + "\"";
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            var lineNumber = 0;
            if (FileUpload1.HasFile && ddlDepartment.SelectedValue == "-department-")
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','invalid Department', 'warning')", true);
            }
            else if (FileUpload1.HasFile && ddlDepartment.SelectedValue != "-department-")
            {
                String path = Server.MapPath("~/CSVFiles/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                String str = Server.HtmlEncode(FileUpload1.FileName);
                String ext = Path.GetExtension(str);

                if ((ext == ".csv") || (ext == ".txt"))
                {
                    //path += str;
                    FileUpload1.SaveAs(path);
                    try
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                using (StreamReader reader = new StreamReader(path))
                                {
                                    while (!reader.EndOfStream)
                                    {
                                        cmd.Parameters.Clear();
                                        var line = reader.ReadLine();
                                        if (lineNumber != 0)
                                        {
                                            var values = line.Split(',');
                                            if(CheckifInsExists(values[0], values[1], values[2], values[3], values[4]))
                                            {
                                                //e swal nga mao ni nga value ang nag exist na
                                            }
                                            else
                                            {
                                                count++;
                                                cmd.CommandText = "INSERT INTO INSTRUCTOR_TABLE(INS_ID, LNAME, FNAME, MNAME, EMAIL, PHONENUM, PASSWORD, ROLE, SCHOOL_ID,DEPT_ID, ISVERIFIED) VALUES(@id,@lname,@fname,@mname,@email,@phonenum,@password,@role,@school,@dept, @isVerified)";
                                                cmd.Parameters.AddWithValue("@id", values[0]);
                                                cmd.Parameters.AddWithValue("@lname", values[1]);
                                                cmd.Parameters.AddWithValue("@fname", values[2]);
                                                cmd.Parameters.AddWithValue("@mname", values[3]);
                                                cmd.Parameters.AddWithValue("@email", values[4]);
                                                cmd.Parameters.AddWithValue("@phonenum", values[5]);
                                                cmd.Parameters.AddWithValue("@password", values[0]);
                                                cmd.Parameters.AddWithValue("@role", "instructor");
                                                cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                                cmd.Parameters.AddWithValue("@dept", ddlDepartment.SelectedValue);
                                                cmd.Parameters.AddWithValue("@isVerified", false);
                                                var ctr = cmd.ExecuteNonQuery();
                                                if (ctr >= 1)
                                                {

                                                }
                                                else
                                                {

                                                }
                                            }
                                            
                                        }
                                        lineNumber++;
                                    }
                                    Session["temp"] = count +" Instructors";
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
                                    GVbind();
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert ('Under Maintenance')</script><pre>" + ex.ToString() + "</pre>");
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
            populateDDL();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox ins_id = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtinsID");
            TextBox lname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtlast");
            TextBox fname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtfirst");
            TextBox mname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtsecond");
            TextBox email = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtmail");
            TextBox phone = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtnum");
            DropDownList department = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("GVddldepartment");
            DropDownList role = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlRole");
            if (CheckifInsExists(ins_id.Text, lname.Text, fname.Text, mname.Text, email.Text))
            {
                clear();
                Session["temp"] = "Instructor";
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
                            cmd.CommandText = "UPDATE INSTRUCTOR_TABLE"
                                + " SET"
                                + " INS_ID = @id,"
                                + " LNAME = @lname,"
                                + " FNAME = @fname,"
                                + " MNAME = @mname,"
                                + " EMAIL = @email,"
                                + " PHONENUM = @phone,"
                                + " ROLE = @role,"
                                + " DEPT_ID = @dept"
                                + " WHERE ID = '" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@id", ins_id.Text);
                            cmd.Parameters.AddWithValue("@lname", lname.Text);
                            cmd.Parameters.AddWithValue("@fname", fname.Text);
                            cmd.Parameters.AddWithValue("@mname", mname.Text);
                            cmd.Parameters.AddWithValue("@email", email.Text);
                            cmd.Parameters.AddWithValue("@phone", phone.Text);
                            cmd.Parameters.AddWithValue("@role", role.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@dept", department.SelectedValue);
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVbind();
        }
        string selectID(int Id)
        {
            string value = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT INS_ID FROM INSTRUCTOR_TABLE WHERE ID='" + Id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            value = reader["INS_ID"].ToString();
                        }
                        else
                        {
                            Response.Write("<script>alert ('User not found')</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert ('Under Maintenance')</script><pre>" + ex.ToString() + "</pre>");
            }
            return value;
        }

        bool isInstructorReferenced(int id, string table, string pk, string isdeleted)
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
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + "='" + id + "' AND school_id='" + Session["school"].ToString() + "' " + isdeleted + "";
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
            int temp = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            string id = selectID(temp);
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        //PHYICAl DELETE QUERY "DELETE FROM INSTRUCTOR_TABLE WHERE ID ='" + temp + "' AND CAMPUS_ID='" + Session["campus"].ToString() + "'"; 
                        if (isInstructorReferenced(temp, "SCHEDULE_TABLE", "instructor_id", "AND isDeleted IS NULL") || isInstructorReferenced(temp, "EVALUATION_TABLE", "evaluatee_id","") || isInstructorReferenced(temp, "EVALUATION_TABLE", "evaluator_id",""))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','To avoid this error, you need to delete master records first on <b>SCHEDULE</b> and is already <b>EVALUATED</b> records with <b>" + id + "</b> then after that you can delete this record. \"<b>Schedule</b> row/s: " + countaffectrows(temp, "SCHEDULE_TABLE", "instructor_id") + "\"', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE INSTRUCTOR_TABLE SET isDeleted= @delete WHERE ID ='" + temp + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@delete", DateTime.Now);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','instructor is deleted with ID: " + id + "', 'success')", true);
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
    }
}