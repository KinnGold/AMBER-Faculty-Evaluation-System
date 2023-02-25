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
    public partial class AdminBulkUpload : System.Web.UI.Page
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM FILES_TABLE WHERE file_name='Sample CSV File For Users Bulk Upload.csv'", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //int count = 0;
            var lineNumber = 0;
            if (FileUpload1.HasFile)
            {
                String path = Server.MapPath("~/CSVFiles/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                String str = Server.HtmlEncode(FileUpload1.FileName);
                String ext = Path.GetExtension(str);

                if ((ext == ".csv") || (ext == ".txt"))
                {
                    FileUpload1.SaveAs(path);
                    using (StreamReader reader = new StreamReader(path))
                    {
                        while (!reader.EndOfStream)
                        {
                            //cmd.Parameters.Clear();
                            var line = reader.ReadLine();
                            if (lineNumber != 0)
                            {
                                var values = line.Split(',');

                                if (!CheckifTermExists(values[10], values[9]))
                                {
                                    insertTerm(values[9], values[10]);
                                }

                                if (!CheckifDeptExists(values[13], values[14]))
                                {
                                    insertDepartment(values[13], values[14]);
                                }

                                if (!CheckifDeptExists(values[27], values[28]))
                                {
                                    insertDepartment(values[27], values[28]);
                                }

                                if (!CheckifCourseExists(values[11], values[12]))
                                {
                                    insertCourse(values[11],values[12], getThisDepartment(values[13], values[14]));
                                }
                                
                                if (!CheckifSectionExists(values[6], getThisCourse(values[11], values[12]), getThisSemester(values[9], values[10]), values[7], values[8]))
                                {
                                    InsertSection(values[6], values[7], values[8], getThisCourse(values[11], values[12]), getThisSemester(values[9], values[10]));
                                }

                                if (!CheckifStudExists(values[0], values[1], values[2], values[3], values[4]))
                                {
                                    InsertStudent(values[0], values[1], values[2], values[3], values[4], values[5], getThisSection(values[6]));
                                }

                                if (!CheckifSubjectExists(values[16], values[17], values[18], getThisDepartment(values[27], values[28])))
                                {
                                    insertSubject(values[16], values[17], values[18], getThisDepartment(values[27], values[28]));
                                }

                                if (!CheckifInsExists(values[21], values[22], values[23], values[24], values[25]))
                                {
                                    insertInstructor(values[21], values[22], values[23], values[24], values[25], values[26], getThisDepartment(values[27], values[28]), values[29]);
                                }

                                if (!CheckifSchedExists(getThisSubject(values[16], values[17]), getThisInstructor(values[21], values[23], values[24], values[22]), values[19], values[20], getThisSection(values[6])))
                                {
                                    insertSchedule(values[15], getThisSubject(values[16], values[17]), getThisInstructor(values[21], values[23], values[24], values[22]), values[19], values[20], getThisSection(values[6]));
                                }
                            }
                            lineNumber++;
                        }
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Upload Success!','File \""+ str +"\" was uploaded successfully', 'success')", true);
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
        bool CheckifSchedExists(string subject, string instructor, string time, string day, string section)
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
                        cmd.CommandText = "SELECT * FROM SCHEDULE_TABLE WHERE subject_id = '" + subject + "' AND instructor_id = '" + instructor + "' AND time = '" + time + "' AND day = '" + day + "' AND section_id = '" + section + "' AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        void insertSchedule(string MIS_code, string subject, string instructor, string time, string day, string section)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO SCHEDULE_TABLE(sched_code, subject_id, instructor_id, time, day, section_id, school_id)"
                            + " VALUES("
                            + "@code,"
                            + "@sub,"
                            + "@ins,"
                            + "@time,"
                            + "@day,"
                            + "@section,"
                            + "@school)";
                        cmd.Parameters.AddWithValue("@code", MIS_code);
                        cmd.Parameters.AddWithValue("@sub", subject);
                        cmd.Parameters.AddWithValue("@ins", instructor);
                        cmd.Parameters.AddWithValue("@time", time);
                        cmd.Parameters.AddWithValue("@day", day);
                        cmd.Parameters.AddWithValue("@section", section);
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

        public string getThisInstructor(string id, string fname, string mname, string lname)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE ins_id = '" + id + "' AND lname = '" + lname + "' AND fname = '" + fname + "' AND mname = '" + mname + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader["id"].ToString();
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

        public string getThisSubject(string code, string desc)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SUBJECT_TABLE WHERE subject_code = '" + code + "' AND subject_name = '" + desc + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            code = reader["subject_id"].ToString();
                        }
                        else
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
            return code;
        }

        bool CheckifInsExists(string id, string lname, string fname, string mname, string email)
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
                        cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE ((INS_ID='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL) OR (fname = '" + fname + "' AND mname='" + mname + "' AND lname='" + lname + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL) OR (email = '" + email + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL))";
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

        void insertInstructor(string id, string lname, string fname, string mname, string email, string phonenum, string dept, string role)
        {
            string fileName = HttpContext.Current.Server.MapPath(@"~/Pictures/usericon.png");
            byte[] bytes = null;
            Stream stream = System.IO.File.OpenRead(fileName);
            BinaryReader binaryreader = new BinaryReader(stream);
            bytes = binaryreader.ReadBytes((int)stream.Length);

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
                                + "@verified,"
                                + "@profile,"
                                + "@lock)";
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@lname", lname);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mname", mname);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phonenum", phonenum);
                        cmd.Parameters.AddWithValue("@password", id);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                        cmd.Parameters.AddWithValue("@dept", dept);//ARI NKA
                        cmd.Parameters.AddWithValue("@verified", false);
                        cmd.Parameters.AddWithValue("@profile", bytes);
                        cmd.Parameters.AddWithValue("@lock", false);
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
                        cmd.CommandText = "SELECT * FROM SUBJECT_TABLE WHERE subject_code='" + code + "' AND subject_name='" + name + "' AND unit = '" + unit + "' AND dept_id = '" + dept + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        void insertSubject(string code, string name, string unit, string dept)
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
                            + "@dept_id,"
                            + "@school)";
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@unit", unit);
                        cmd.Parameters.AddWithValue("@dept_id", dept);
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

        public string getThisDepartment(string code,string desc)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM DEPARTMENT_TABLE WHERE DEPT_CODE = '" + code + "' AND DEPT_NAME = '"+ desc +"' AND SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            code = reader["dept_id"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return code;
        }

        bool CheckifCourseExists(string code, string desc)
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
                        cmd.CommandText = "SELECT * FROM COURSE_TABLE WHERE (title='" + code + "' AND description='" + desc + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL)";
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

        void insertCourse(string title, string desc, string dept)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO COURSE_TABLE(TITLE,DESCRIPTION, DEPT_ID, SCHOOL_ID)"
                            + " VALUES("
                            + "@title,"
                            + "@desc,"
                            + "@code,"
                            + "@school)";
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@desc", desc);
                        cmd.Parameters.AddWithValue("@code", dept);
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
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

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
                        cmd.CommandText = "SELECT * FROM DEPARTMENT_TABLE WHERE dept_code='" + code + "' AND dept_name='" + name + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        void insertDepartment(string code,string name)
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
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@name", name);
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

        bool CheckifTermExists(string year,string term)
        {
            string ver = "v" + year[2] + year[3] + "-" + term[0];
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE VERSION ='" + ver + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        void insertTerm(string term, string year)
        {
            //var start = Convert.ToDateTime(txtEvalStart.Text);
            //var end = Convert.ToDateTime(txtEvalEnd.Text);
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO SEMESTER_TABLE(description, year, status, version, school_id)"
                            + " VALUES("
                            + "@name,"
                            + "@year,"
                            + "@status,"
                            + "@version,"
                            //+ "@start,"
                            //+ "@end,"
                            + "@school)";
                        cmd.Parameters.AddWithValue("@name", term);
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@status", "Inactive");
                        cmd.Parameters.AddWithValue("@version", "v" + year[2] + year[3] + "-" + term[0]);
                        //cmd.Parameters.AddWithValue("@start", start);
                        //cmd.Parameters.AddWithValue("@end", end);
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

        public string getThisSection(string name)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SECTION_TABLE WHERE SECTION_NAME = '"+ name +"' AND SCHOOL_ID = '"+ Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            name = reader["section_id"].ToString();
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

        public string getThisCourse(string name, string desc)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM COURSE_TABLE WHERE title = '" + name + "' AND description = '" + desc + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            name = reader["course_id"].ToString();
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

        void InsertSection(string name, string program, string yearlvl, string course, string semester)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO SECTION_TABLE(section_name, program, yearLevel, course_id, semester_id, school_id)"
                                + " VALUES("
                                + "@name,"
                                + "@program,"
                                + "@year,"
                                + "@course,"
                                + "@term,"
                                + "@school)";
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@program", program);
                        cmd.Parameters.AddWithValue("@year", yearlvl);
                        cmd.Parameters.AddWithValue("@course", course);
                        cmd.Parameters.AddWithValue("@term", semester);
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

        bool CheckifSectionExists(string name, string course, string term, string program, string yearlevel)
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
                        cmd.CommandText = "SELECT * FROM SECTION_TABLE WHERE section_name='" + name + "' AND course_id = '" + course + "' AND semester_id = " + term + " AND program = '"+ program +"' AND yearLevel = '"+ yearlevel +"' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        void InsertStudent(string id, string lname, string fname, string mname, string email, string phonenum, string section)
        {
            string fileName = HttpContext.Current.Server.MapPath(@"~/Pictures/usericon.png");
            byte[] bytes = null;
            Stream stream = System.IO.File.OpenRead(fileName);
            BinaryReader binaryreader = new BinaryReader(stream);
            bytes = binaryreader.ReadBytes((int)stream.Length);

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO STUDENT_TABLE(STUD_ID, LNAME, FNAME, MNAME, EMAIL, PHONENUM, PASSWORD, ROLE, SECTION_ID, SCHOOL_ID, isVerified, PROFILE_PICTURE, LOCK_STATUS) VALUES(@id,@lname,@fname,@mname,@email,@phonenum,@password,@role,@section,@school, @verified, @profile, @lock)";
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@lname", lname);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mname", mname);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phonenum", phonenum);
                        cmd.Parameters.AddWithValue("@password", id);
                        cmd.Parameters.AddWithValue("@role", "student");
                        cmd.Parameters.AddWithValue("@section", section);
                        cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                        cmd.Parameters.AddWithValue("@verified", false);
                        cmd.Parameters.AddWithValue("@profile", bytes);
                        cmd.Parameters.AddWithValue("@lock", false);
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

        bool CheckifStudExists(string id, string fname, string mname, string lname, string email)
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
                        cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE ((STUD_ID='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL) OR (fname = '" + fname + "' AND mname='" + mname + "' AND lname='" + lname + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL) OR (email = '" + email + "' AND SCHOOL_ID='" + Session["school"].ToString() + "' AND isDeleted IS NULL))";
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