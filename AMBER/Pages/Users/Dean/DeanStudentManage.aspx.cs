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

namespace AMBER.Pages.Users.Dean
{
    public partial class DeanStudentManage : System.Web.UI.Page
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
                populateDDL();
                GVbind();

            }

        }
        void populateDDL()
        {
            string selectme = "-section-";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT section_id, section_name FROM SECTION_TABLE JOIN COURSE_TABLE ON SECTION_TABLE.course_id = COURSE_TABLE.course_id WHERE SECTION_TABLE.school_id = '" + Session["school"].ToString() + "' AND SECTION_TABLE.isDeleted IS NULL AND COURSE_TABLE.dept_id = '" + Session["dept"].ToString() + "'";
                        ddlSection.DataValueField = "section_id";
                        ddlSection.DataTextField = "section_name";
                        ddlSection.DataSource = cmd.ExecuteReader();
                        ddlSection.DataBind();
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
                        cmd.CommandText = "SELECT DISTINCT section_id, section_name FROM SECTION_TABLE JOIN COURSE_TABLE ON SECTION_TABLE.course_id = COURSE_TABLE.course_id WHERE SECTION_TABLE.school_id = '" + Session["school"].ToString() + "' AND SECTION_TABLE.isDeleted IS NULL AND COURSE_TABLE.dept_id = '" + Session["dept"].ToString() + "'";
                        ddlinsertSec.DataValueField = "section_id";
                        ddlinsertSec.DataTextField = "section_name";
                        ddlinsertSec.DataSource = cmd.ExecuteReader();
                        ddlinsertSec.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlSection.Items.Add(selectme);
            ddlSection.Items.FindByText(selectme.ToString()).Selected = true;
            ddlinsertSec.Items.Add(selectme);
            ddlinsertSec.Items.FindByText(selectme.ToString()).Selected = true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string fileName = HttpContext.Current.Server.MapPath(@"~/Pictures/usericon.png");
            byte[] bytes = null;
            Stream stream = System.IO.File.OpenRead(fileName);
            BinaryReader binaryreader = new BinaryReader(stream);
            bytes = binaryreader.ReadBytes((int)stream.Length);

            bool lockstatus = false;

            int count = 0;
            var lineNumber = 0;
            if (FileUpload1.HasFile && ddlSection.SelectedValue == "-section-")
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','invalid Section', 'warning')", true);
            }
            else if (FileUpload1.HasFile && ddlSection.SelectedValue != "-section-")
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
                                            if (CheckifStudExists(values[0], values[1], values[2], values[3], values[4]))
                                            {
                                                //e swal nga mao ni nga value ang nag exist na
                                            }
                                            else
                                            {
                                                count++;
                                                cmd.CommandText = "INSERT INTO STUDENT_TABLE(STUD_ID, LNAME, FNAME, MNAME, EMAIL, PHONENUM, PASSWORD, ROLE, SECTION_ID, SCHOOL_ID, ISVERIFIED, PROFILE_PICTURE, LOCK_STATUS) VALUES(@id,@lname,@fname,@mname,@email,@phonenum,@password,@role,@section,@school, @isVerified, @profile, @lock)";
                                                cmd.Parameters.AddWithValue("@id", values[0]);
                                                cmd.Parameters.AddWithValue("@lname", values[1]);
                                                cmd.Parameters.AddWithValue("@fname", values[2]);
                                                cmd.Parameters.AddWithValue("@mname", values[3]);
                                                cmd.Parameters.AddWithValue("@email", values[4]);
                                                cmd.Parameters.AddWithValue("@phonenum", values[5]);
                                                cmd.Parameters.AddWithValue("@password", values[0]);
                                                cmd.Parameters.AddWithValue("@role", "student");
                                                cmd.Parameters.AddWithValue("@isVerified", false);
                                                cmd.Parameters.AddWithValue("@section", ddlSection.SelectedValue);
                                                cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                                cmd.Parameters.AddWithValue("@profile", bytes);
                                                cmd.Parameters.AddWithValue("@lock", lockstatus);
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
                                    Session["temp"] = count + " Students";
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
                                    GVbind();
                                }
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<pre>" + ex.ToString() + "</pre>");
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
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY stud_id)row_num,* FROM STUDENT_TABLE JOIN SECTION_TABLE ON STUDENT_TABLE.section_id = SECTION_TABLE.section_id JOIN COURSE_TABLE ON SECTION_TABLE.course_id = COURSE_TABLE.course_id WHERE STUDENT_TABLE.SCHOOL_ID = @school AND STUDENT_TABLE.isDeleted IS NULL AND dept_id = @dept", db);
                cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                cmd.Parameters.AddWithValue("@dept", Session["dept"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
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
                        cmd.CommandText = "SELECT STUD_ID FROM STUDENT_TABLE WHERE Id='" + Id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            value = reader["STUD_ID"].ToString();
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

        protected void btnAddstudent_Click(object sender, EventArgs e)
        {
            var id = txtID.Text;
            var lname = txtLname.Text;
            var fname = txtFname.Text;
            var mname = txtMName.Text;
            var email = txtEmail.Text;
            var phone = txtPhonenum.Text;
            var section = ddlinsertSec.SelectedValue;

            bool lockstatus = false;

            string fileName = HttpContext.Current.Server.MapPath(@"~/Pictures/usericon.png");
            byte[] bytes = null;
            Stream stream = System.IO.File.OpenRead(fileName);
            BinaryReader binaryreader = new BinaryReader(stream);
            bytes = binaryreader.ReadBytes((int)stream.Length);

            if (CheckifStudExists(id,fname,mname,lname,email))
            {
                clear();
                Session["temp"] = "Student";
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
                            cmd.CommandText = "INSERT INTO STUDENT_TABLE(STUD_ID, LNAME, FNAME, MNAME, EMAIL, PHONENUM, PASSWORD, ROLE, SECTION_ID, SCHOOL_ID, ISVERIFIED, PROFILE_PICTURE, LOCK_STATUS)"
                                + " VALUES("
                                + "@id,"
                                + "@lname,"
                                + "@fname,"
                                + "@mname,"
                                + "@email,"
                                + "@phonenum,"
                                + "@password,"
                                + "@role,"
                                + "@section,"
                                + "@school,"
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
                            cmd.Parameters.AddWithValue("@role", "student");
                            cmd.Parameters.AddWithValue("@section", section);
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            cmd.Parameters.AddWithValue("@profile", bytes);
                            cmd.Parameters.AddWithValue("@isVerified", false);
                            cmd.Parameters.AddWithValue("@lock", lockstatus);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                clear();
                                Session["temp"] = "Student";
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
            txtID.Text = String.Empty;
            txtLname.Text = String.Empty;
            txtFname.Text = String.Empty;
            txtMName.Text = String.Empty;
            txtPhonenum.Text = String.Empty;
            txtEmail.Text = String.Empty;
        }
        bool CheckifStudExists(string id,string fname,string mname,string lname, string email)
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
                        cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE ((STUD_ID='" + id + "' AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL) OR (fname = '" + fname + "' AND mname='" + mname + "' AND lname='" + lname + "' AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL) OR (email = '" + email + "' AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL))";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            chkr = true;
                            //Session["temp"] = "Student id:\""+id+"\"";
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
                Response.Write("<script>alert ('Under Maintenance')</script><pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox stud_id = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtID");
            TextBox lname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtthird");
            TextBox fname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtfirst");
            TextBox mname = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtsecond");
            TextBox email = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtmail");
            TextBox phone = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtnum");
            DropDownList section = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("GVddlsection");
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
                            + " STUD_ID = @id,"
                            + " LNAME = @lname,"
                            + " FNAME = @fname,"
                            + " MNAME = @mname,"
                            + " EMAIL = @email,"
                            + " PHONENUM = @phone,"
                            + " SECTION_ID = @section"
                            + " WHERE ID = '" + id + "' AND school_id = '" + Session["school"].ToString() + "'";
                        cmd.Parameters.AddWithValue("@id", stud_id.Text);
                        cmd.Parameters.AddWithValue("@lname", lname.Text);
                        cmd.Parameters.AddWithValue("@fname", fname.Text);
                        cmd.Parameters.AddWithValue("@mname", mname.Text);
                        cmd.Parameters.AddWithValue("@email", email.Text);
                        cmd.Parameters.AddWithValue("@phone", phone.Text);
                        cmd.Parameters.AddWithValue("@section", section.SelectedValue);
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVbind();
        }

        bool isStudentReferenced(int id, string table, string pk, string isdeleted)
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
                        if (isStudentReferenced(temp, "EVALUATION_TABLE", "evaluator_id",""))//ari napud ka
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','To avoid this error, you need to delete master records first on <b>EVALUATION</b> records with <b>" + id + "</b> and is already <b>EVALUATED</b> then after that you can delete this record.', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE STUDENT_TABLE SET isDeleted= @delete WHERE ID ='" + temp + "' AND school_id = '" + Session["school"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@delete", DateTime.Now);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','student is deleted with ID: " + id + "', 'success')", true);
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