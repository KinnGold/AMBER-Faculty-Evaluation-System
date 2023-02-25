using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace AMBER.Pages.Users
{
    public partial class ContactAdmin : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            string selectme = "Select School";
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
                            cmd.CommandText = "SELECT DISTINCT Id, school_name FROM SCHOOL_TABLE";
                            SchoolDDL.DataValueField = "Id";
                            SchoolDDL.DataTextField = "school_name";
                            SchoolDDL.DataSource = cmd.ExecuteReader();
                            SchoolDDL.DataBind();
                        }

                    }

                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }
                SchoolDDL.Items.Add(selectme);
                SchoolDDL.Items.FindByText(selectme.ToString()).Selected = true;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var fname = txtfname.Text;
            var mname = txtmname.Text;
            var lname = txtlname.Text;
            var email = txtemail.Text;
            var id = txtidnumber.Text;
            var other = txtotherreason.Text;
            var role = txtrole.Text;

           if(SchoolDDL.SelectedValue == "Select School")
            {
                if (rblReason.SelectedIndex >= 0)
                {
                    try
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {

                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "INSERT INTO NOTIFICATIONS_TABLE(message, first_name, middle_name, last_name, user_id, user_role, user_email, school_id)VALUES("
                                    + "@msg,"
                                    + "@fname,"
                                    + "@mname,"
                                    + "@lname,"
                                    + "@id,"
                                    + "@role,"
                                    + "@email,"
                                    + "@school)";
                                cmd.Parameters.AddWithValue("@msg", rblReason.SelectedItem.Text);
                                cmd.Parameters.AddWithValue("@fname", fname);
                                cmd.Parameters.AddWithValue("@mname", mname);
                                cmd.Parameters.AddWithValue("@lname", lname);
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@role", role);
                                cmd.Parameters.AddWithValue("@email", email);
                                cmd.Parameters.AddWithValue("@school", SchoolDDL.SelectedValue);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SubmittedSuccessful()", true);
                                    clear();
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
                else if (rblReason.SelectedIndex == -1)
                {
                    try
                    {
                        using (var db = new SqlConnection(connDB))
                        {
                            db.Open();
                            using (var cmd = db.CreateCommand())
                            {

                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "INSERT INTO NOTIFICATIONS_TABLE(message, first_name, middle_name, last_name, user_id, user_role, user_email, school_id)VALUES("
                                    + "@msg,"
                                    + "@fname,"
                                    + "@mname,"
                                    + "@lname,"
                                    + "@id,"
                                    + "@role,"
                                    + "@email,"
                                    + "@school)";
                                cmd.Parameters.AddWithValue("@msg", other);
                                cmd.Parameters.AddWithValue("@fname", fname);
                                cmd.Parameters.AddWithValue("@mname", mname);
                                cmd.Parameters.AddWithValue("@lname", lname);
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@role", role);
                                cmd.Parameters.AddWithValue("@email", email);
                                cmd.Parameters.AddWithValue("@school", SchoolDDL.SelectedValue);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "SubmittedSuccessful()", true);
                                    clear();
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
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "ErrorReason()", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "ErrorSchool()", true);
            }
        }

        public void clear()
        {
            txtfname.Text = String.Empty;
            txtmname.Text = String.Empty;
            txtlname.Text = String.Empty;
            txtrole.Text = String.Empty;
            txtidnumber.Text = String.Empty;
            txtemail.Text = String.Empty;
            txtotherreason.Text = String.Empty;
            rblReason.SelectedIndex = -1;
        }
    }
}