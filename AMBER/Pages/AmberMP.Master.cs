using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.BM
{
    public partial class AmberMP : System.Web.UI.MasterPage
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbluser.Text = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
                image();
                chkEvalStatus();
                GetNotifications();
                checkAdminType();
            }
            
            image();
            chkEvalStatus();
            //notificationCount();
            getTERMstatus();
            subscriptionStatus();
            SchoolImage();
        }

        void checkAdminType()
        {
            var id = Session["id"].ToString();
            var school = Session["school"].ToString();
            string type = "Sub-Admin";

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM ADMIN_TABLE WHERE Id='" + id + "' AND school_id='" + school + "' AND role='" + type + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        plcAddAdmin.Visible = false;
                    }
                    else
                    {
                        plcAddAdmin.Visible = true;
                    }
                }
            }
        }
        void subscriptionStatus()
        {
            var id = Session["id"].ToString();
            var school = Session["school"].ToString();
            string type = "Free";
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
                        plcsubscription.Visible = true;
                        plcbulkupload.Visible = false;
                        plcAddAdmin.Visible = false;
                    }
                    else
                    {
                        plcbulkupload.Visible = true;
                        plcsubscription.Visible = false;
                        plcAddAdmin.Visible = true;
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
                        cmd.CommandText = "SELECT * FROM NOTIFICATIONS_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isRead='" + status + "'";
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
                        cmd.CommandText = "SELECT * FROM NOTIFICATIONS_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isRead='" + status + "'";
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

        public void SchoolImage()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT school_picture, school_name FROM SCHOOL_TABLE WHERE Id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblschool.Text = reader["school_name"].ToString();

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
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            if (Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Pages/Amber.aspx");
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

        protected void adminprofilebtn_Click(object sender, EventArgs e)
        {
            var username = Session["user"].ToString();
            Response.Redirect("https://localhost:44311/Pages/AdminProfile.aspx?username=" + username + "");
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
                        cmd.CommandText = "SELECT * FROM NOTIFICATIONS_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isRead='" + status1 + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            db.Open();

                            cmd.CommandText = "UPDATE NOTIFICATIONS_TABLE"
                           + " SET"
                           + " isRead = @stat"
                           + " WHERE SCHOOL_ID = '" + Session["school"].ToString() + "';";
                            cmd.Parameters.AddWithValue("@stat", status);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                GetNotifications();
                                Response.Redirect("/Pages/AdminNotifications.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("/Pages/AdminNotifications.aspx");
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
                        cmd.CommandText = "SELECT * FROM NOTIFICATIONS_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isRead='" + status1 + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            db.Close();
                            db.Open();

                            cmd.CommandText = "UPDATE NOTIFICATIONS_TABLE"
                           + " SET"
                           + " isRead = @stat"
                           + " WHERE SCHOOL_ID = '" + Session["school"].ToString() + "';";
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
        void chkEvalStatus()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE GETDATE() BETWEEN evaluationStart AND evaluationEnd";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                UpdateStatus(reader["semester_id"].ToString(), "Active");
                            }
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE GETDATE() NOT BETWEEN evaluationStart AND evaluationEnd";
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                UpdateStatus(reader["semester_id"].ToString(), "Inactive");
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

        //void notificationCount()
        //{
        //    try
        //    {
        //        using (var db = new SqlConnection(connDB))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM NOTIFICATIONS_TABLE WHERE school_id='" + Session["school"].ToString() + "'";
        //                SqlDataReader reader = cmd.ExecuteReader();
        //                if (reader.Read())
        //                {
        //                    lblnotif.InnerText = reader["COUNT"].ToString();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
        //        Response.Write("<pre>" + ex.ToString() + "</pre>");
        //    }
        //}

        void getTERMstatus()
        {
            var start = "";
            var end = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE status='Active' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            //start = Convert.ToDateTime(reader["EvaluationStart"]);
                            //end = Convert.ToDateTime(reader["EvaluationEnd"]);
                            start = reader["EvaluationStart"].ToString();
                            end = reader["EvaluationEnd"].ToString();
                            plcevaluationlink.Visible = true;
                        }
                        else
                        {
                            plcevaluationlink.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            if (start != "" && end != "")
            {
                var test = Convert.ToDateTime(start);
                if (DateTime.Now >= Convert.ToDateTime(start) && DateTime.Now <= Convert.ToDateTime(end))
                {
                    lblstatus.Text = "Starting";
                    lblstatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblstatus.Text = "Ended";
                    lblstatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        void UpdateStatus(string id, string status)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SEMESTER_TABLE"
                            + " SET"
                            + " STATUS = @stat"
                            + " WHERE SEMESTER_ID = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
                        cmd.Parameters.AddWithValue("@stat", status);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            //GVbind();
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

        protected void adminprofilebtn_Click1(object sender, EventArgs e)
        {
            var username = Session["user"].ToString();
            Response.Redirect("https://localhost:44311/Pages/AdminProfile.aspx?username=" + username + "");
        }
        protected void subscribenowbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/SubscriptionDetails.aspx");
        }

        //protected void btnGenerate_Click(object sender, EventArgs e)
        //{
        //    if (Session["school"] != null)
        //    {
        //       // byte[] bytes = (byte[])Session["school"];
        //        //string s3 = Convert.ToBase64String(bytes);
        //        //byte[] school = Convert.FromBase64String(s3);

        //        txtLink.Text = "https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(Session["school"].ToString()) + "";
        //    }
        //}
    }
}