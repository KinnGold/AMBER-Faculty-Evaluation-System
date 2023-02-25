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

namespace AMBER.Pages
{
    public partial class CategoryQuestionsUpload : System.Web.UI.Page
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
                BindFiles();
            }
        }

        protected void BindFiles()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM FILES_TABLE WHERE file_name='SampleCSVConstructorIndicator.csv'", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }


        protected void btnconsupload_Click(object sender, EventArgs e)
        {
            var lineNumber = 0;
            if (FileUpload2.HasFile)
            {
                String path = Server.MapPath("~/CSVFiles/") + Path.GetFileName(FileUpload2.PostedFile.FileName);
                String str = Server.HtmlEncode(FileUpload2.FileName);
                String ext = Path.GetExtension(str);

                if ((ext == ".csv") || (ext == ".txt"))
                {
                    FileUpload2.SaveAs(path);
                    using (StreamReader reader = new StreamReader(path))
                    {
                        while (!reader.EndOfStream)
                        {
                            //cmd.Parameters.Clear();
                            var line = reader.ReadLine();
                            if (lineNumber != 0)
                            {
                                var values = line.Split(',');

                                if (!CheckifConstructorExists(values[0],values[2],getThisSemester(values[3],values[4])))
                                {
                                    insertConstructor(values[0], values[1], values[2], getThisSemester(values[3], values[4]));
                                }
                                if (!CheckifIndicatorExists(values[5],getThisConstructor(values[0], values[2], getThisSemester(values[3], values[4]))))
                                {
                                    insertIndicator(values[5], getThisConstructor(values[0], values[2], getThisSemester(values[3], values[4])));
                                }
                            }
                            lineNumber++;
                        }
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Upload Success!','File \"" + str + "\" was uploaded successfully', 'success')", true);
                        notify();
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

        public void notify()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO NOTIFICATIONS_TABLE(message, isRead, DateTimeSent, first_name, middle_name, last_name, user_id, user_role, user_email, school_id)"
                            + " VALUES("
                            + "@msg,"
                            + "@status,"
                            + "@date,"
                            + "@fname,"
                            + "@mname,"
                            + "@lname,"
                            + "@id,"
                            + "@role,"
                            + "@email,"
                            + "@school)";
                        cmd.Parameters.AddWithValue("@msg", "You just uploaded a Categories and Question you can now add WEIGHT to this categories to Start Evaluating! ");
                        cmd.Parameters.AddWithValue("@status", 0);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@fname", "AMBER");
                        cmd.Parameters.AddWithValue("@mname", "");
                        cmd.Parameters.AddWithValue("@lname", "");
                        cmd.Parameters.AddWithValue("@id", 8);
                        cmd.Parameters.AddWithValue("@role", "system");
                        cmd.Parameters.AddWithValue("@email", "system");
                        cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<pre>" + ex.ToString() + "</pre>");
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
            }
        }

        public string getThisSemester(string term, string year)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE year = '" + year + "' AND description = '" + term + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            term = reader["semester_id"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return term;
        }

        void insertConstructor(string name, string desc, string assigned, string semester)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO CONSTRUCTOR_TABLE(constructor_name, semester_id, description, role, school_id)"
                                + " VALUES("
                                + "@name,"
                                + "@sem,"
                                + "@desc,"
                                + "@role,"
                                + "@school_id)";
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@sem", semester);
                        cmd.Parameters.AddWithValue("@desc", desc);
                        cmd.Parameters.AddWithValue("@role", assigned);
                        cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
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

        void insertIndicator(string name, string cons)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO INDICATOR_TABLE(indicator_name, constructor_id, school_id)"
                                + " VALUES("
                                + "@indiname,"
                                + "@cons,"
                                + "@school)";
                        cmd.Parameters.AddWithValue("@indiname", name);
                        cmd.Parameters.AddWithValue("@cons", cons);
                        cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
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

        public string getThisConstructor(string name, string role, string semester)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM CONSTRUCTOR_TABLE WHERE CONSTRUCTOR_NAME = '" + name + "' AND role='" + role + "' AND semester_id='" + semester + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            name = reader["constructor_id"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return name;
        }

        //public string getThisSemester(string term)
        //{
        //    try
        //    {
        //        using (var db = new SqlConnection(connDB))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE description = '" + term + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
        //                SqlDataReader reader = cmd.ExecuteReader();
        //                if (reader.Read())
        //                {
        //                    term = reader["semester_id"].ToString();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
        //        Response.Write("<pre>" + ex.ToString() + "</pre>");
        //    }
        //    return term;
        //}

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
                        cmd.CommandText = "SELECT * FROM CONSTRUCTOR_TABLE WHERE constructor_name='" + name + "' AND role='" + group + "' AND semester_id='" + semester + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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
        bool CheckifIndicatorExists(string indiname, string cons)
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
                        cmd.CommandText = "SELECT * FROM INDICATOR_TABLE WHERE indicator_name = '" + indiname + "' AND constructor_id='" + cons + "' AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes = null;
            string fileName = "";
            string contentType = "";

            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM FILES_TABLE WHERE Id='" + id + "'", db);
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
    }
}