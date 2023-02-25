using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace AMBER.Pages
{
    public partial class AdminSkillsTest : System.Web.UI.Page
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
                skillsTestStatus();
                BindDeptDDL();
                GVbind();
                DeanDDL.Enabled = false;
                InstructorDDL.Enabled = false;
                try
                {
                    using (SqlConnection db = new SqlConnection(connDB))
                    {
                        db.Open();
                        SqlCommand cmd = new SqlCommand("SELECT evaluatee_id,(''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') AS NAME,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id  WHERE EVALUATION_TABLE.school_id=@school_id AND isDeleted IS NULL GROUP BY (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+''),evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))<2.00", db);
                        cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            plcNoData.Visible = false;
                            plcData.Visible = true;
                        }
                        else
                        {
                            plcNoData.Visible = true;
                            plcData.Visible = false;
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

        void skillsTestStatus()
        {
            var school = Session["school"].ToString();
            string status = "True";
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE skillstest_status='" + status + "' AND school_id='" + school + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        deactivateSkillsTest.Visible = true;
                        activateSkillsTest.Visible = false;
                    }
                    else
                    {
                        deactivateSkillsTest.Visible = false;
                        activateSkillsTest.Visible = true;
                    }
                }
            }
        }
        private void BindDeptDDL()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT dept_id, dept_name FROM DEPARTMENT_TABLE WHERE school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataAdapter sqlDa = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sqlDa.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DepartmentDDL.DataValueField = "dept_id";
                            DepartmentDDL.DataTextField = "dept_name";
                            DepartmentDDL.DataSource = cmd.ExecuteReader();
                            DepartmentDDL.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            DepartmentDDL.Items.Add("Select Department");
            DepartmentDDL.Items.FindByText("Select Department").Selected = true;
        }

        private void BindDeanDDL()
        {
            var dept = DepartmentDDL.SelectedValue.ToString();
            if (DepartmentDDL.SelectedItem.Text == "Select Department")
            {
                DeanDDL.Enabled = false;
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
                            cmd.CommandText = "SELECT ins_id, ('' + INSTRUCTOR_TABLE.fname + ' ' + INSTRUCTOR_TABLE.mname + ' ' + INSTRUCTOR_TABLE.lname + '') AS NAME FROM INSTRUCTOR_TABLE WHERE role = 'dean' AND school_id = '" + Session["school"].ToString() + "' AND dept_id='" + dept + "' AND isDeleted IS NULL";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                db.Close();
                                db.Open();
                                cmd.CommandText = "SELECT ins_id, ('' + INSTRUCTOR_TABLE.fname + ' ' + INSTRUCTOR_TABLE.mname + ' ' + INSTRUCTOR_TABLE.lname + '') AS NAME FROM INSTRUCTOR_TABLE WHERE role = 'dean' AND school_id = '" + Session["school"].ToString() + "' AND dept_id='" + dept + "' AND isDeleted IS NULL";
                                SqlDataAdapter sqlDa = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                sqlDa.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    DeanDDL.Enabled = true;
                                    DeanDDL.DataValueField = "ins_id";
                                    DeanDDL.DataTextField = "NAME";
                                    DeanDDL.DataSource = cmd.ExecuteReader();
                                    DeanDDL.DataBind();

                                }
                            }
                            else
                            {
                                DeanDDL.DataBind();
                                DeanDDL.Enabled = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                DeanDDL.Items.Add("Select Dean");
                DeanDDL.Items.FindByText("Select Dean").Selected = true;
            }

        }

        private void BindInstructorDDL()
        {
            var dept = DepartmentDDL.SelectedValue.ToString();
            if (DepartmentDDL.SelectedItem.Text == "Select Department")
            {
                InstructorDDL.Enabled = false;
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
                            cmd.CommandText = "SELECT DISTINCT evaluatee_id, (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') AS NAME, role, dept_code, dept_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id=EVALUATION_TABLE.evaluatee_id JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id=INSTRUCTOR_TABLE.dept_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND DEPARTMENT_TABLE.dept_id='" + dept + "' AND INSTRUCTOR_TABLE.role='instructor' GROUP BY (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+''),evaluatee_id, role, dept_code, dept_name HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))<2.00";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                db.Close();
                                db.Open();
                                cmd.CommandText = "SELECT DISTINCT evaluatee_id, (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') AS NAME, role, dept_code, dept_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id=EVALUATION_TABLE.evaluatee_id JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id=INSTRUCTOR_TABLE.dept_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND DEPARTMENT_TABLE.dept_id='" + dept + "' AND INSTRUCTOR_TABLE.role='instructor' GROUP BY (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+''),evaluatee_id, role, dept_code, dept_name HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))<2.00";
                                SqlDataAdapter sqlDa = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                sqlDa.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    InstructorDDL.Enabled = true;
                                    InstructorDDL.DataValueField = "evaluatee_id";
                                    InstructorDDL.DataTextField = "NAME";
                                    InstructorDDL.DataSource = cmd.ExecuteReader();
                                    InstructorDDL.DataBind();

                                }
                            }
                            else
                            {
                                InstructorDDL.DataBind();
                                InstructorDDL.Enabled = false;
                            }
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

        }

        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT evaluatee_id,(''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') AS NAME,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id  WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL GROUP BY (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+''),evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))<2.00 ORDER BY AVERAGE DESC", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GVResults.DataSource = dr;
                    GVResults.DataBind();
                }
            }
        }

        protected void btnrequest_Click(object sender, EventArgs e)
        {
            var dept = DepartmentDDL.SelectedValue;
            var dean = DeanDDL.SelectedValue;
            var ins = InstructorDDL.SelectedValue.ToString();

            if (DepartmentDDL.SelectedValue == "Select Department")
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "invaliddept()", true);
            }
            else if (DeanDDL.SelectedValue == "Select Dean")
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "invaliddean()", true);
            }
            else if (InstructorDDL.SelectedValue == "Select Instructor")
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "invalidins()", true);
            }
            else if (txtDateTime.Text == "")
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "invalidDate()", true);
            }
            else
            {
                string mssg = "Dear Dean " + DeanDDL.SelectedItem.ToString() + ", please conduct a Skills Test by observing one of the class of Instructor " + InstructorDDL.SelectedItem.ToString() + " because the Instructor has a low evaluation ratings. The schedule for the Skills Test Observation will be on " + Convert.ToDateTime(txtDateTime.Text).ToString("dddd, dd MMMM yyyy hh:mm:ss") + ". Thank you And God Speed!";

                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "INSERT INTO USERSNOTIFICATIONS_TABLE(message, isRead, DateTimeSent, toName, fromName,  toUser_id, fromUser_id, toUser_role, fromUser_role, school_id)"
                          + " VALUES("
                          + "@msg,"
                          + "@read,"
                          + "@date,"
                          + "@toName,"
                          + "@fromName,"
                          + "@toUser_id,"
                          + "@fromUser_id,"
                          + "@toUser_role,"
                          + "@fromUser_role,"
                          + "@school_id)";
                            cmd.Parameters.AddWithValue("@msg", mssg);
                            cmd.Parameters.AddWithValue("@read", false);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@toName", DeanDDL.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@fromName", Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString());
                            cmd.Parameters.AddWithValue("@toUser_id", DeanDDL.SelectedValue);
                            cmd.Parameters.AddWithValue("@fromUser_id", Session["id"].ToString());
                            cmd.Parameters.AddWithValue("@toUser_role", "dean");
                            cmd.Parameters.AddWithValue("@fromUser_role", Session["role"].ToString());
                            cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "SendSuccess()", true);
                                cmd.CommandText = "SELECT DISTINCT evaluatee_id, (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') AS NAME, role, email,dept_code, dept_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id=EVALUATION_TABLE.evaluatee_id JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id=INSTRUCTOR_TABLE.dept_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND DEPARTMENT_TABLE.dept_id='" + dept + "' AND INSTRUCTOR_TABLE.role='instructor' GROUP BY (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+''),evaluatee_id, role, email, dept_code, dept_name HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))<2.00";
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    string email = reader["email"].ToString();

                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Timeout = 60000;
                                    smtp.Port = 587;
                                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                                    smtp.EnableSsl = true;
                                    MailMessage msg = new MailMessage();
                                    msg.IsBodyHtml = true;
                                    msg.Subject = "Skills Test Notification";
                                    msg.Body += "Hello <b>" + InstructorDDL.SelectedItem.ToString() + "</b>,<br/><br/>You're evaluation results was too low, so the Dean of your department will conduct an observation while you're having class for your skills test.<br/>The schedule for the Skills Test Observation will be on <b>" + Convert.ToDateTime(txtDateTime.Text).ToString("dddd, dd MMMM yyyy hh:mm:ss") + "<b/><br/><br/> Good luck and God Bless.<br/><br/>";
                                    string toaddress = email;
                                    msg.To.Add(toaddress);
                                    string fromaddress = "<evaluatenow.amber@gmail.com>";

                                    msg.From = new MailAddress(fromaddress);
                                    try
                                    {
                                        smtp.Send(msg);
                                    }
                                    catch
                                    {
                                        throw;
                                    }

                                }
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

        protected void DepartmentDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DepartmentDDL.SelectedItem.Text == "Select Department")
            {
                BindDeanDDL();
                BindInstructorDDL();
            }
            else
            {
                BindDeanDDL();
                BindInstructorDDL();
            }
        }

        protected void activateSkillsTest_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SEMESTER_TABLE SET skillstest_status=@status WHERE school_id='" + Session["school"].ToString() + "'";
                        cmd.Parameters.AddWithValue("@status", true);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            deactivateSkillsTest.Visible = true;
                            activateSkillsTest.Visible = false;
                            Response.Redirect("/Pages/AdminSkillsTest.aspx");
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

        protected void deactivateSkillsTest_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SEMESTER_TABLE SET skillstest_status=@status WHERE school_id='" + Session["school"].ToString() + "'";
                        cmd.Parameters.AddWithValue("@status", false);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {

                            activateSkillsTest.Visible = true;
                            deactivateSkillsTest.Visible = false;
                            Response.Redirect("/Pages/AdminSkillsTest.aspx");
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
}