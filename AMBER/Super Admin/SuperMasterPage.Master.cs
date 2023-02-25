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
    public partial class SuperMasterPage : System.Web.UI.MasterPage
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            lbluser.Text = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
            image();
            GetNotifications();
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

        public void GetNotifications()
        {
            int count = 0;
            var status = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SUPERADMINNOTIFICATIONS_TABLE WHERE isRead='" + status + "'";
                        cmd.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        count = Convert.ToInt32(dt.Rows.Count.ToString());
                        notifications1.Text = count.ToString();
                        Repeater1.DataSource = dt;
                        Repeater1.DataBind();

                        db.Close();
                        db.Open();
                        cmd.CommandText = "SELECT * FROM SUPERADMINNOTIFICATIONS_TABLE WHERE isRead='" + status + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            plcnotifications.Visible = true;
                            plcNOnotifications.Visible = false;
                        }
                        else
                        {
                            plcnotifications.Visible = false;
                            plcNOnotifications.Visible = true;
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


        protected void adminprofilebtn_Click(object sender, EventArgs e)
        {
            var username = Session["user"].ToString();
            Response.Redirect("https://localhost:44311/Super%20Admin/SuperAdminProfile.aspx?username=" + username + "");
        }

        protected void markasreadbtn_Click(object sender, EventArgs e)
        {
            var status = 1;
            var status1 = 0;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SUPERAMINNOTIFICATIONS_TABLE WHERE isRead='" + status1 + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            db.Open();

                            cmd.CommandText = "UPDATE SUPERAMINNOTIFICATIONS_TABLE"
                           + " SET"
                           + " isRead = @stat";
                            cmd.Parameters.AddWithValue("@stat", status);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                GetNotifications();
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

        protected void viewallbtn_Click(object sender, EventArgs e)
        {

            var status = 1;
            var status1 = 0;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SUPERADMINNOTIFICATIONS_TABLE WHERE isRead='" + status1 + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            db.Open();

                            cmd.CommandText = "UPDATE SUPERADMINNOTIFICATIONS_TABLE"
                           + " SET"
                           + " isRead = @stat";
                            cmd.Parameters.AddWithValue("@stat", status);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                GetNotifications();
                                Response.Redirect("/Super%20Admin/SuperAdminNotifications.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("/Super%20Admin/SuperAdminNotifications.aspx");
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

        protected void logoutbtn_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            if (Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Super%20Admin/SuperAdminLogin.aspx");
            }
        }
    }
}