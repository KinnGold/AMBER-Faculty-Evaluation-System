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
using System.IO;

namespace AMBER.Super_Admin
{
    public partial class SuperAdminProfile : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Super%20Admin/SuperAdminLogin.aspx");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["user"] != null)
                {
                    try
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "SELECT * FROM SUPER_ADMIN_TABLE WHERE username='" + Request.QueryString["user"].ToString() + "'";
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    db.Close();
                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                        Response.Write(ex);
                    }
                }
                loadUser();
            }
        }

        void loadUser()
        {
            var user = Session["user"].ToString();
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SUPER_ADMIN_TABLE WHERE username = '" + user + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            labelusername.Text = reader["username"].ToString();
                            labelname.Text = reader["first_name"].ToString() + " " + reader["middle_name"].ToString() + " " + reader["last_name"].ToString();
                            labelemail.Text = reader["email"].ToString();
                            labelcnum.Text = reader["contact_no"].ToString();
                            image();
                            txteditfname.Text = reader["first_name"].ToString();
                            txteditmname.Text = reader["middle_name"].ToString();
                            txteditlname.Text = reader["last_name"].ToString();
                            txteditemail.Text = reader["email"].ToString();
                            txteditnum.Text = reader["contact_no"].ToString();
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
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

        public void image()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT profile_picture FROM SUPER_ADMIN_TABLE WHERE username = '" + Session["user"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader["profile_picture"] != System.DBNull.Value)
                        {
                            byte[] bytes = (byte[])reader["profile_picture"];
                            string profilePicture = Convert.ToBase64String(bytes, 0, bytes.Length);
                            profilePic.ImageUrl = "data:image/png;base64," + profilePicture;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
        }

        protected void upload_Click(object sender, EventArgs e)
        {
            try
            {
                if (updateProfile.HasFile)
                {
                    string filename = Path.GetFileName(updateProfile.PostedFile.FileName);
                    string extension = Path.GetExtension(filename);
                    int length = updateProfile.PostedFile.ContentLength;
                    byte[] pic = new byte[length];
                    updateProfile.PostedFile.InputStream.Read(pic, 0, length);

                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE"
                                        + " SET"
                                        + " profile_picture = @Image"
                                        + " WHERE Id = '" + Session["Id"].ToString() + "' AND username = '" + Session["user"].ToString() + "'";
                                cmd.Parameters.AddWithValue("@Image", pic);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    image();
                                }
                            }
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "fileerror()", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "profilepicerror()", true);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        protected void btneditname_Click(object sender, EventArgs e)
        {
            var Id = Session["Id"].ToString();
            var user = Session["user"].ToString();
            var fname = txteditfname.Text;
            var mname = txteditmname.Text;
            var lname = txteditlname.Text;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE"
                            + " SET "
                            + " first_name = @fname,"
                            + " middle_name = @mname,"
                            + " last_name = @lname"
                            + " WHERE Id = '" + Id + "' AND username='" + user + "'";
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mname", mname);
                        cmd.Parameters.AddWithValue("@lname", lname);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Session["id"] = Id;
                            Session["fname"] = fname;
                            Session["mname"] = mname;
                            Session["lname"] = lname;
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
                            //clear();
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        protected void savepassbtn_Click(object sender, EventArgs e)
        {
            var Id = Session["Id"].ToString();
            var newpass = txtnewpass.Text;
            var confirmpass = txtconfirmnewpass.Text;
            string pass = txtcurrpass.Text;
            var pass1 = Session["pass"].ToString();
            var user = Session["user"].ToString();

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one lower case letter', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one lower case letter";
            }
            else if (!hasUpperChar.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one upper case letter', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one upper case letter";
            }
            else if (!hasMiniMaxChars.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password error!', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should not be less than or greater than 12 characters";
            }
            else if (!hasNumber.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one number character', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one numeric value";
            }
            else if (!hasSymbols.IsMatch(newpass))
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire ('Password must have at least one special character', 'Password must be minimum of 8 and must contain atleast one Upper case, at least one special character and at least one number.', 'error')", true);
                //ErrorMessage = "Password should contain At least one special case characters";
            }
            else if (pass1 != pass)
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "PassError()", true);
            }
            else if (confirmpass != newpass)
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "PassError()", true);
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
                            cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE"
                                + " SET "
                                + " password = @newpass"
                                + " WHERE Id = '" + Id + "' AND username='" + user + "'";
                            cmd.Parameters.AddWithValue("@newpass", confirmpass);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Session["Id"] = Id;
                                Session["user"] = user;
                                Session["pass"] = newpass;
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
                                //clear();
                            }
                            db.Close();
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

        protected void btnconinfo_Click(object sender, EventArgs e)
        {
            var Id = Session["Id"].ToString();
            var user = Session["user"].ToString();
            var newemail = txteditemail.Text;
            var newnum = txteditnum.Text;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SUPER_ADMIN_TABLE"
                            + " SET "
                            + " email = @newemail,"
                            + " contact_no = @newnum"
                            + " WHERE Id = '" + Id + "' AND username='" + user + "'";
                        cmd.Parameters.AddWithValue("@newemail", newemail);
                        cmd.Parameters.AddWithValue("@newnum", newnum);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Session["id"] = Id;
                            Session["email"] = newemail;
                            Session["contact_no"] = newnum;
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
                            clear();
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        void clear()
        {
            txteditemail.Text = String.Empty;
            txteditfname.Text = String.Empty;
            txteditlname.Text = String.Empty;
            txteditmname.Text = String.Empty;
            txteditnum.Text = String.Empty;
            txtcurrpass.Text = String.Empty;
            txtnewpass.Text = String.Empty;
            txtconfirmnewpass.Text = String.Empty;
        }
    }
}