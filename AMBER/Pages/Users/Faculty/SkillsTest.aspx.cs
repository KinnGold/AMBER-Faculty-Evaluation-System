using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;

namespace AMBER.Pages.Users.Faculty
{
    public partial class SkillsTest : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null && Session["role"] == null)
                {
                    Response.Redirect("/Pages/Users/UsersLogin.aspx");
                }
                GVbind();
                populateDDL();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AutoRedirect();
        }

        public void AutoRedirect()
        {
            int int_MilliSecondsTimeOut = this.SessionLengthMinutes * 60000;
            var sch_id = Session["school"].ToString();
            string str_Script = @"
               <script type='text/javascript'> 
                   intervalset = window.setInterval('Redirect()'," +
                       int_MilliSecondsTimeOut.ToString() + @");
                   function Redirect()
                   {
                       window.location.href='https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(sch_id) + @"';
                   }
               </script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", str_Script);
        }

        public int SessionLengthMinutes
        {
            get { return Session.Timeout; }
        }

        void populateDDL()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT ins_id, (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') AS NAME FROM INSTRUCTOR_TABLE WHERE dept_id='" + Session["dept"].ToString() + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL EXCEPT SELECT ins_id, (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') FROM INSTRUCTOR_TABLE WHERE ins_id='" + Session["Id"].ToString() + "'";

                        InstructorDDL.DataValueField = "ins_id";
                        InstructorDDL.DataTextField = "NAME";
                        InstructorDDL.DataSource = cmd.ExecuteReader();
                        InstructorDDL.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            InstructorDDL.Items.Add("Select Instructor");
            InstructorDDL.Items.FindByText("Select Instructor").Selected = true;
        }

        protected void GVbind()
        {
            var camp_id = Session["school"].ToString();
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SKILLSTEST_TABLE WHERE school_id='" + camp_id + "' AND dean_id='" + Session["id"].ToString() + "' AND dept_id='" + Session["dept"].ToString() + "'", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GVSkillsTest.DataSource = dr;
                    GVSkillsTest.DataBind();
                }
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes = null;
            string fileName = "";
            string contentType = "";
            var camp_id = Session["school"].ToString();

            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SKILLSTEST_TABLE WHERE Id='" + id + "' AND school_id='" + camp_id + "' AND dept_id='" + Session["dept"].ToString() + "'", db);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    dr.Read();
                    fileName = dr["file_name"].ToString();
                    contentType = dr["content_type"].ToString();
                    bytes = (byte[])dr["data"];
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        protected void GVSkillsTest_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GVSkillsTest.DataKeys[e.RowIndex].Value.ToString());
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM SKILLSTEST_TABLE WHERE Id ='" + id + "' AND school_ID = '" + Session["school"].ToString() + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Data is deleted with ID: " + id + "', 'success')", true);
                            GVSkillsTest.EditIndex = -1;
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

        protected void btnupload_Click(object sender, EventArgs e)
        {
            var school = Session["school"].ToString();
            var ins = InstructorDDL.SelectedValue;
            string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string contentType = FileUpload1.PostedFile.ContentType;
            String ext = Path.GetExtension(filename);

            if (FileUpload1.HasFile && InstructorDDL.SelectedValue == "Select Instructor")
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','invalid Instructor', 'warning')", true);
            }
            else if (FileUpload1.HasFile == false)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','No File Chosen!', 'warning')", true);
            }
            else
            {
                if (ext == ".xlsx" || ext == ".xls")
                {
                    try
                    {
                        using (Stream fs = FileUpload1.PostedFile.InputStream)
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                using (var db = new SqlConnection(connDB))
                                {
                                    db.Open();
                                    using (var cmd = db.CreateCommand())
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        cmd.CommandText = "INSERT INTO GETSKILLSTESTRESULTS_TABLE(dean_id, ins_id, dept_id, file_name, content_type, data, school_id)"
                                      + " VALUES("
                                      + "@dean," 
                                      + "@ins,"
                                      + "@dept,"
                                      + "@name,"
                                      + "@content,"
                                      + "@data,"
                                      + "@school)";
                                        cmd.Parameters.AddWithValue("@dean", Session["Id"].ToString());
                                        cmd.Parameters.AddWithValue("@ins", ins);
                                        cmd.Parameters.AddWithValue("@dept", Session["dept"].ToString());
                                        cmd.Parameters.AddWithValue("@name", filename);
                                        cmd.Parameters.AddWithValue("@content", contentType);
                                        cmd.Parameters.AddWithValue("@data", bytes);
                                        cmd.Parameters.AddWithValue("@school", school);
                                        var ctr = cmd.ExecuteNonQuery();
                                        if (ctr >= 1)
                                        {
                                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "requestsuccessful()", true);
                                        }

                                    }
                                }

                            }
                        }
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                        Response.Write("<pre>" + ex.ToString() + "</pre>");
                    }

                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "invalidfile()", true);
                }
            }
        }
    }
}