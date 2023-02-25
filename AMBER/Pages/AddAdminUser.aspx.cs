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
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

namespace AMBER.Pages
{
    public partial class AddAdminUser : System.Web.UI.Page
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
                string type = "Sub-Admin";
                GVbind();

                try
                {
                    using (SqlConnection db = new SqlConnection(connDB))
                    {
                        db.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM ADMIN_TABLE WHERE school_id='"+ Session["school"].ToString() +"' AND role='" + type.ToString() + "'", db);
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

        void UserClear()
        {
            txtFname.Text = String.Empty;
            txtLname.Text = String.Empty;
            txtMName.Text = String.Empty;
            txtPhonenum.Text = String.Empty;
            txtposition.Text = String.Empty;
            txtEmail.Text = String.Empty;
        }

        public void GVbind()
        {
            string type = "Sub-Admin";
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM ADMIN_TABLE WHERE school_id=@schoolid AND role='"+ type.ToString() +"'", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }


        protected void btnAddAdmin_Click(object sender, EventArgs e)
        {
            var id = makeID();
            var username = generateUsername();

            string fileName = HttpContext.Current.Server.MapPath(@"~/Pictures/usericon.png");
            byte[] bytes = null;
            Stream stream = System.IO.File.OpenRead(fileName);
            BinaryReader binaryreader = new BinaryReader(stream);
            bytes = binaryreader.ReadBytes((int)stream.Length);

            var fname = txtFname.Text;
            var mname = txtMName.Text;
            var lname = txtLname.Text;
            var email = txtEmail.Text;
            var phone = txtPhonenum.Text;
            var pos = txtposition.Text;
            bool lockstatus = false;


            if (CheckifUserExists() == true)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
                UserClear();
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
                            cmd.CommandText = "INSERT INTO ADMIN_TABLE(Id, fname, mname, lname, email, contact_no, school_id, position, role, password, username, datesignedup, isVerified, profile_picture, lock_status)VALUES("
                                + "@id,"
                                + "@fname,"
                                + "@mname,"
                                + "@lname,"
                                + "@email,"
                                + "@contactno,"
                                + "@school,"
                                + "@position,"
                                + "@role,"
                                + "@pass,"
                                + "@username,"
                                + "@datesignedup,"
                                + "@isVerified,"
                                + "@profile,"
                                + "@lock)";
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@fname", fname);
                            cmd.Parameters.AddWithValue("@mname", mname);
                            cmd.Parameters.AddWithValue("@lname", lname);
                            cmd.Parameters.AddWithValue("@type", "Extension");
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@contactno", phone);
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            cmd.Parameters.AddWithValue("@position", pos);
                            cmd.Parameters.AddWithValue("@role", "Sub-Admin");
                            cmd.Parameters.AddWithValue("@pass", username);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@datesignedup", DateTime.Now);
                            cmd.Parameters.AddWithValue("@isVerified", false);
                            cmd.Parameters.AddWithValue("@profile", bytes);
                            cmd.Parameters.AddWithValue("@lock", lockstatus);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                db.Close();
                                db.Open();
                                cmd.CommandText = "SELECT Id, school_name FROM SCHOOL_TABLE WHERE Id='"+ Session["school"].ToString() + "'";
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    var school_name = dr["school_name"].ToString();

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
                                    msg.Subject = "Welcome to AMBER: FACULTY EVALUATION SYSTEM";
                                    msg.Body += "Hello <b>" + fname + " " + mname + " "+ lname +"</b>,<br />You have been added by <b>" + Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString() + "</b> as an Admin user for your school <b> " + school_name + " </b> to manage your Faculty Evaluation through AMBER:Faculty Evaluation System.<br /><br />Your Login Information is:<br />ID Number: <b>" + username + "</b><br/> Password:<b>" + username + "</b><br /> Welcome to Amber! <br/><br/>Best regards, Developers.";
                                    string toaddress = email.ToString();
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
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
                                    UserClear();
                                }
                                db.Close();
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

        public string makeID()
        {
            string res;
            Random rnd = new Random();
            int num = rnd.Next(1911, 10000);
            char last = char.Parse(txtLname.Text.Substring(0, 1));
            char first = char.Parse(txtFname.Text.Substring(0, 1));
            char second = char.Parse(txtMName.Text.Substring(0, 1));
            res = last + first + second + "2022" + num;
            return res;
        }

        public string generateUsername()
        {
            var fname = txtFname.Text.ToLower().Trim();
            var lname = txtLname.Text.ToLower();
            string res;
            Random rnd = new Random();
            int num = rnd.Next(1911, 100000);
            res = String.Concat(fname.Where(c => !Char.IsWhiteSpace(c))) + lname.ToString()[0] + num;
            return res;
        }

        bool CheckifUserExists()
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
                        cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE fname='" + txtFname.Text.ToString() + "' AND mname='" + txtMName.Text.ToString() + "' AND lname='" + txtLname.Text.ToString() + "'";
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

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = GridView1.DataKeys[e.RowIndex].Value.ToString();
            string type = "Sub-Admin";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM ADMIN_TABLE WHERE Id ='" + id + "' AND school_id = '" + Session["school"].ToString() + "' AND role='"+ type.ToString() + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Admin User is deleted with ID: " + id + "', 'success')", true);
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
}