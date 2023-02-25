using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;
using System.Collections;

namespace AMBER.Pages
{
    public partial class EvaluationLink : System.Web.UI.Page
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
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM DEPARTMENT_TABLE WHERE school_id='" + Session["school"].ToString() + "' ORDER BY dept_name ASC";
                            departmentDDL.DataValueField = "dept_id";
                            departmentDDL.DataTextField = "dept_name";
                            departmentDDL.DataSource = cmd.ExecuteReader();
                            departmentDDL.DataBind();
                            departmentDDL.Items.Insert(0, new ListItem("ALL", ""));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                departmentDDL.Items.Add("Select Department");
                departmentDDL.Items.FindByText("Select Department").Selected = true;
            }
        }

        public class emailCode
        {
            public string stud_email { get; set; }
            public string stud_id { get; set; }
            public string studpassword { get; set; }
        }

        protected void btngeneratelink_Click(object sender, EventArgs e)
        {
            if (Session["school"] != null)
            {
                if (departmentDDL.SelectedItem.Text == "Select Department")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Generate Link!','Select a Department first before generating the link.', 'warning')", true);
                }
                else
                {
                    txtLink.Text = "https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(Session["school"].ToString()) + "&dept_id=" + AMBER.URLEncryption.GetencryptedQueryString(departmentDDL.SelectedValue.ToString()) + "";
                }
            }
        }

        protected void btnsend_Click(object sender, EventArgs e)
        {
            if (departmentDDL.SelectedItem.Text == "Select Department")
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Send Link!','Select a Department first before sending the link.', 'warning')", true);
            }
            else
            {
                ArrayList emailArray = new ArrayList();
                var emails = new List<emailCode>();
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT STUDENT_TABLE.stud_id, STUDENT_TABLE.password, STUDENT_TABLE.email FROM STUDENT_TABLE UNION SELECT INSTRUCTOR_TABLE.ins_id, INSTRUCTOR_TABLE.password, INSTRUCTOR_TABLE.email FROM INSTRUCTOR_TABLE WHERE INSTRUCTOR_TABLE.school_id='" + Session["school"].ToString() + "' AND INSTRUCTOR_TABLE.dept_id='" + departmentDDL.SelectedValue.ToString() + "'";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.HasRows == true)
                            {
                                while (reader.Read())
                                {
                                    emails.Add(new emailCode
                                    {
                                        stud_email = Convert.ToString(reader["email"]),
                                        stud_id = Convert.ToString(reader["stud_id"]),
                                        studpassword = Convert.ToString(reader["password"]),
                                    });

                                }

                                foreach(emailCode email in emails)
                                {

                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Timeout = 600000;
                                    smtp.Port = 587;
                                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("evaluatenow.amber@gmail.com", "ffovzxvouiyxrwse");
                                    smtp.EnableSsl = true;
                                    MailMessage msg = new MailMessage();
                                    msg.IsBodyHtml = true;
                                    msg.Subject = "Login Link and Information";
                                    msg.Body += "Hello " + HttpUtility.HtmlEncode(email.stud_email) + ",<br />You have been added as a user for your school to use AMBER:Faculty Evaluation System.<br />Your Login Information is:<br />ID Number: <b>" + HttpUtility.HtmlEncode(email.stud_id) + "</b><br/> Password:<b>" + HttpUtility.HtmlEncode(email.studpassword) + "</b><br />And here is the link of the Login Page: " + txtLink.Text.ToString() + ".";
                                    string toaddress = email.stud_email;
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
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "SentSuccessful()", true);
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
}