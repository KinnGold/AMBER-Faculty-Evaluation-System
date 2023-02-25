using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.BM.Users
{
    public partial class LandingPage : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["role"] == null)
                {
                    Response.Redirect("UsersLogin.aspx");
                }
                if (Session["role"].Equals("instructor"))
                {
                    studentplc.Visible = false;
                }
                if (Session["role"].Equals("student"))
                {
                    instructorplc.Visible = false;
                }
                passwordNotChange();
            }
            if(Session["mname"].ToString() != "")
            {
                lbluser.Text = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
            }
            else
            {
                lbluser.Text = Session["fname"].ToString() + " " + Session["lname"].ToString();
            }
            getTERMstatus();
           
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

        // function to get the full month name
        static string getFullName(int month)
        {
            DateTime date = new DateTime(2022, month, 12);

            return date.ToString("MMMM");
        }

        void passwordNotChange()
        {
            var school_id = Session["school"].ToString();

            if (Session["role"].Equals("instructor") || Session["role"].Equals("dean"))
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM INSTRUCTOR_TABLE WHERE school_id='" + school_id + "' AND password='" + Session["insID"].ToString() + "'";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (!reader.Read())
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "setSession", "sessionStorage.setItem('shown-modal', 'true');", true);
                            }
                            else
                            {
                                db.Close();
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
            else if (Session["role"].Equals("student"))
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM STUDENT_TABLE WHERE school_id='" + school_id + "' AND password='" + Session["studID"].ToString() + "'";
                            SqlDataReader reader1 = cmd.ExecuteReader();
                            if (reader1.Read())
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "setSession", "sessionStorage.setItem('shown-modal', 'true');", true);
                            }
                            else
                            {
                                db.Close();
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

        void getTERMstatus()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                string[] year = reader["year"].ToString().Split('-');
                                //compare date time then kuhaa nga semester
                                var test1 = DateTime.Now.Year;
                                var test2 = Int32.Parse(year[0]);
                                var test3 = Int32.Parse(year[1]);
                                if (DateTime.Now.Year >= Int32.Parse(year[0]) && DateTime.Now.Year <= Int32.Parse(year[1]))
                                {
                                    if (reader["status"].ToString() == "Active")
                                    {
                                        lblsem.Text = reader["description"].ToString() + " SY " + reader["year"].ToString();
                                        //lblstatus.Text = reader["evaluationStart"].ToString() + "-" + reader["evaluationEnd"].ToString();
                                        //IF NULL AYAW PAG SET OG TEXT PRA SA label period
                                        lblstatus.Text = getFullName(Convert.ToDateTime(reader["evaluationStart"]).Month) + " " + Convert.ToDateTime(reader["evaluationStart"]).Day + ", " + Convert.ToDateTime(reader["evaluationStart"]).Year + " " + Convert.ToDateTime(reader["evaluationStart"]).ToString("hh:mm tt") + " to " + getFullName(Convert.ToDateTime(reader["evaluationStart"]).Month) + " " + Convert.ToDateTime(reader["evaluationEnd"]).Day + ", " + Convert.ToDateTime(reader["evaluationStart"]).Year + " " + Convert.ToDateTime(reader["evaluationStart"]).ToString("hh:mm tt");
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                        //if (reader.Read())
                        //{

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        protected void goToProfile_Click(object sender, EventArgs e)
        {

            if (Session["role"].Equals("instructor") || Session["role"].Equals("dean"))
            {
                Response.Redirect("https://localhost:44311/Pages/Users/Faculty/InstructorProfile.aspx?Id=" + Session["insID"].ToString() + "");
            }
            else if (Session["role"].Equals("student"))
            {
                Response.Redirect("https://localhost:44311/Pages/Users/Student/StudentProfile.aspx?Id=" + Session["studID"].ToString() + "");
            }
        }
    }
}