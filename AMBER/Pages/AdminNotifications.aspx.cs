using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;

namespace AMBER.Pages
{
    public partial class AdminNotifications : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["role"] == null)
            {
                Response.Redirect("/Pages/LoginPage.aspx");
            }
            if (!IsPostBack)
            {
                GVbind();
                populateDDL();

                try
                {
                    using (SqlConnection db = new SqlConnection(connDB))
                    {
                        db.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM NOTIFICATIONS_TABLE WHERE school_id = '" + Session["school"].ToString() + "'", db);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            plcnoNotifications.Visible = false;
                            plcnotifications.Visible = true;
                        }
                        else
                        {
                            plcnoNotifications.Visible = true;
                            plcnotifications.Visible = false;
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
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM NOTIFICATIONS_TABLE WHERE school_id = '" + Session["school"].ToString() + "'", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        private void populateDDL()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "  SELECT user_id, ('' + NOTIFICATIONS_TABLE.first_name + ' ' + NOTIFICATIONS_TABLE.middle_name + ' ' + NOTIFICATIONS_TABLE.last_name + '') AS NAME FROM NOTIFICATIONS_TABLE WHERE school_id = '" + Session["school"].ToString() + "'";
                        SqlDataAdapter sqlDa = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sqlDa.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            nameDDL.DataValueField = "user_id";
                            nameDDL.DataTextField = "NAME";
                            nameDDL.DataSource = cmd.ExecuteReader();
                            nameDDL.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            nameDDL.Items.Add("Select Name");
            nameDDL.Items.FindByText("Select Name").Selected = true;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM NOTIFICATIONS_TABLE WHERE Id ='" + id + "' AND school_id = '" + Session["school"].ToString() + "'";
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','Notification is deleted with ID: " + id + "', 'success')", true);
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

        protected void sendreplybtn_Click(object sender, EventArgs e)
        {
            var fromID = Session["id"].ToString();
            var toID = nameDDL.SelectedValue;
            var fromRole = Session["role"].ToString();
            var school = Session["school"].ToString();
            var toName = nameDDL.SelectedItem.Text;
            var fromName = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
            var status = 0;
            var msg = txtreply.Text;

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM NOTIFICATIONS_TABLE WHERE user_id='"+ toID +"' AND school_id='"+ school +"'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            var toRole = reader["user_role"].ToString();
                            db.Close();
                            db.Open();

                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "INSERT INTO USERSNOTIFICATIONS_TABLE(message, isRead, DateTimeSent, toName, fromName, toUser_id, fromUser_id, toUser_role, fromUser_role, school_id)"
                                + " VALUES("
                                + "@msg,"
                                + "@status,"
                                + "@date,"
                                + "@toName,"
                                + "@fromName,"
                                + "@toId,"
                                + "@fromId," 
                                + "@toRole,"
                                + "@fromRole,"
                                + "@school)";
                            cmd.Parameters.AddWithValue("@msg", msg);
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@toName", toName);
                            cmd.Parameters.AddWithValue("@fromName", fromName);
                            cmd.Parameters.AddWithValue("@toId", toID);
                            cmd.Parameters.AddWithValue("@fromId", fromID);
                            cmd.Parameters.AddWithValue("@toRole", toRole);
                            cmd.Parameters.AddWithValue("@fromRole", fromRole);
                            cmd.Parameters.AddWithValue("@school", school);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                clear();
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SendSuccess()", true);
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notfound()", true);
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

        void clear()
        {
            txtreply.Text = String.Empty;
        }
    }
}