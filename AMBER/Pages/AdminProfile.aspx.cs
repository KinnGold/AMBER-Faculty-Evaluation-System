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

namespace AMBER.Pages
{
    public partial class AdminProfile : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                }

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
                                cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE username='" + Request.QueryString["user"].ToString() + "'";
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    txteditfname.Text = reader["fname"].ToString();
                                    txteditmname.Text = reader["mname"].ToString();
                                    txteditlname.Text = reader["lname"].ToString();
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
                LoadSchool();
                subscriptionStatus();
                SchoolImage();
                loadSubscription();
                subscriptionStatus1();

                string type = "Sub-Admin";
                var id = Session["id"].ToString();
                try
                {
                    using (SqlConnection db = new SqlConnection(connDB))
                    {
                        db.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM ADMIN_TABLE WHERE Id='" + id + "' AND school_id='" + Session["school"].ToString() + "' AND role='" + type.ToString() + "'", db);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            plcSubscription.Visible = false;
                        }
                        else
                        {
                            plcSubscription.Visible = true;
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
        void LoadSchool()
        {
            var Id = Session["id"].ToString();
            var school_id = Session["school"].ToString();

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SCHOOL_TABLE WHERE Id='" + school_id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            labelschool.Text = reader["school_name"].ToString();
                            labeladdress.Text = reader["address"].ToString();
                            txteditschoolname.Text = reader["school_name"].ToString();
                            txteditaddress.Text = reader["address"].ToString();
                            labelschoolphone.Text = reader["school_phone"].ToString();
                            txteditschoolphone.Text = reader["school_phone"].ToString();

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

        void loadUser()
        {
            var Id = Session["id"].ToString();
            var school_id = Session["school"].ToString();

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE Id = '" + Id + "' AND school_id='" + school_id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            labelusername.Text = reader["username"].ToString();
                            labelname.Text = reader["fname"].ToString() + " " + reader["mname"].ToString() + " " + reader["lname"].ToString();
                            labelposition.Text = reader["position"].ToString();
                            labelemail.Text = reader["email"].ToString();
                            labelphone.Text = reader["contact_no"].ToString();
                            labeldate.Text = reader["datesignedup"].ToString();
                            txteditfname.Text = reader["fname"].ToString();
                            txteditmname.Text = reader["mname"].ToString();
                            txteditlname.Text = reader["lname"].ToString();
                            txteditemail.Text = reader["email"].ToString();
                            txteditnum.Text = reader["contact_no"].ToString();
                            txteditschoolposition.Text = reader["position"].ToString();
                            txtEditUsername.Text = reader["username"].ToString();
                            image();
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

        protected void btneditname_Click(object sender, EventArgs e)
        {
            var Id = Session["id"].ToString();
            var school_id = Session["school"].ToString();
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
                        cmd.CommandText = "UPDATE ADMIN_TABLE"
                            + " SET "
                            + " fname = @fname,"
                            + " mname = @mname,"
                            + " lname = @lname"
                            + " WHERE Id = '" + Id + "' AND school_id='" + school_id + "'";
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
            var Id = Session["id"].ToString();
            var newpass = txtnewpass.Text;
            var confirmpass = txtconfirmnewpass.Text;
            string pass = txtcurrpass.Text;
            var pass1 = Session["pass"].ToString();
            var user = Session["user"].ToString();
            var school_id = Session["school"].ToString();

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
                            cmd.CommandText = "UPDATE ADMIN_TABLE"
                                + " SET "
                                + " password = @newpass"
                                + " WHERE Id = '" + Id + "' AND school_id='" + school_id + "'";
                            cmd.Parameters.AddWithValue("@newpass", confirmpass);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Session["id"] = Id;
                                Session["user"] = user;
                                Session["password"] = newpass;
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
            var Id = Session["id"].ToString();
            var school_id = Session["school"].ToString();
            var newemail = txteditemail.Text;
            var newnum = txteditnum.Text;
            var pos = txteditschoolposition.Text;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE ADMIN_TABLE"
                            + " SET "
                            + " email = @newemail,"
                            + " contact_no = @newnum," 
                            + "position = @pos"
                            + " WHERE Id = '" + Id + "' AND school_id='" + school_id + "'";
                        cmd.Parameters.AddWithValue("@newemail", newemail);
                        cmd.Parameters.AddWithValue("@newnum", newnum);
                        cmd.Parameters.AddWithValue("@pos", pos);
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
                                cmd.CommandText = "UPDATE ADMIN_TABLE"
                                        + " SET"
                                        + " profile_picture = @Image"
                                        + " WHERE Id = '" + Session["id"].ToString() + "' AND school_id = '" + Session["school"].ToString() + "'";
                                cmd.Parameters.AddWithValue("@Image", pic);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    Response.Redirect("https://localhost:44311/Pages/AdminProfile.aspx?username=" + Session["user"].ToString() + "");
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

        void loadSubscription()
        {
            var id = Session["id"].ToString();
            var school = Session["school"].ToString();

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SUBSCRIPTION_TABLE WHERE admin_id='" + id + "' AND school_id='" + school + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        labelsubplan.Text = reader["subscription_plan"].ToString();
                        labelsubtype.Text = reader["subscription_type"].ToString();
                        labelstart.Text = reader["startDate"].ToString();
                        labelend.Text = reader["endDate"].ToString();
                    }
                }
            }
        }

        void subscriptionStatus()
        {
            var id = Session["id"].ToString();
            var school = Session["school"].ToString();
            string type = "Free";
            string type1 = "Premium";
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SUBSCRIPTION_TABLE WHERE admin_id='" + id + "' AND GETDATE() < endDate AND school_id='" + school + "' AND subscription_type='" + type + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        plcuploadicon.Visible = false;
                    }
                    else
                    {
                        plcuploadicon.Visible = true;
                    }
                }
            }
        }

        void subscriptionStatus1()
        {
            var id = Session["id"].ToString();
            var school = Session["school"].ToString();
            string type = "Free";
            string type1 = "Sub-Admin";

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SUBSCRIPTION_TABLE WHERE admin_id='" + id + "' AND school_id='" + school + "' AND subscription_type='" + type + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        plcsubnow.Visible = true;
                        plcsubplan.Visible = false;
                    }
                    else
                    {
                        plcsubnow.Visible = false;
                        plcsubplan.Visible = true;
                    }
                }
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
                    cmd.CommandText = "SELECT profile_picture FROM ADMIN_TABLE WHERE Id = '" + Session["id"].ToString() + "' AND school_id = '" + Session["school"].ToString() + "'";
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

        public void SchoolImage()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT school_picture FROM SCHOOL_TABLE WHERE Id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader["school_picture"] != System.DBNull.Value)
                        {
                            byte[] bytes = (byte[])reader["school_picture"];
                            string profilePicture = Convert.ToBase64String(bytes, 0, bytes.Length);
                            schoolProfilePic.ImageUrl = "data:image/png;base64," + profilePicture;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
        }

        protected void btnsaveschoolinfo_Click(object sender, EventArgs e)
        {
            var sch_name = txteditschoolname.Text;
            var sch_add = txteditaddress.Text;
            var sch_phone = txteditschoolphone.Text;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SCHOOL_TABLE"
                            + " SET "
                            + " school_name = @schname," 
                            + " school_phone = @schpone," 
                            + " address = @schaddress"
                            + " WHERE Id = '" + Session["school"].ToString() + "'";
                        cmd.Parameters.AddWithValue("@schname", sch_name);
                        cmd.Parameters.AddWithValue("@schpone", sch_phone);
                        cmd.Parameters.AddWithValue("@schaddress", sch_add);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Session["school"] = Session["school"].ToString();
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

        protected void updateschoolPic_Click(object sender, EventArgs e)
        {
            try
            {
                if (updateSchoolProfile.HasFile)
                {
                    string filename = Path.GetFileName(updateSchoolProfile.PostedFile.FileName);
                    string extension = Path.GetExtension(filename);
                    int length = updateSchoolProfile.PostedFile.ContentLength;
                    byte[] pic = new byte[length];
                    updateSchoolProfile.PostedFile.InputStream.Read(pic, 0, length);

                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE SCHOOL_TABLE"
                                        + " SET"
                                        + " school_picture = @Image"
                                        + " WHERE Id = '" + Session["school"].ToString() + "'";
                                cmd.Parameters.AddWithValue("@Image", pic);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    Response.Redirect("https://localhost:44311/Pages/AdminProfile.aspx?username=" + Session["user"].ToString() + "");
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

        protected void cancelsubsbtn_Click(object sender, EventArgs e)
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SUBSCRIPTION_TABLE WHERE school_id = '" + Session["school"].ToString() + "' AND admin_id='"+ Session["id"].ToString() +"'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var sub_id = reader["sub_id"].ToString();
                        Response.Redirect("https://localhost:44311/Pages/CancelSubscription.aspx?sub_id="+ AMBER.URLEncryption.GetencryptedQueryString(sub_id) +"");
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
        }

        protected void btnchangeplan_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/SubscriptionDetails.aspx");
        }

        protected void subnowbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/SubscriptionDetails.aspx");
        }

        protected void btnEditUsername_Click(object sender, EventArgs e)
        {
            var uname = txtEditUsername.Text;
            var id = Session["id"].ToString();
            if(CheckifUserExists() == true)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
                txtEditUsername.Text = String.Empty;
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
                            cmd.CommandText = "UPDATE ADMIN_TABLE"
                                + " SET "
                                + " username = @uname"
                                + " WHERE Id = '" + id + "' AND school_id='" + Session["school"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@uname", uname);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Session["id"] = id;
                                Session["user"] = uname;
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "UpdateSuccess()", true);
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


        bool CheckifUserExists()
        {
            var uname = txtEditUsername.Text;
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE username='" + uname + "' AND school_id='"+ Session["school"].ToString() +"'";
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
    }
}