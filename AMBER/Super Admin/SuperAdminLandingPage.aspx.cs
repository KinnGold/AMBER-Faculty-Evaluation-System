using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.Super_Admin
{
    public partial class SuperAdminLandingPage : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Super%20Admin/SuperAdminLogin.aspx");
            }
            lbladmin.Text = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
            if (!IsPostBack)
            {
                lblinsBind();
                adminbind();
                lblstudBind();
                schoolBind();
                subscriptionbind();
            }
        }

        void lblinsBind()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS totalRow FROM INSTRUCTOR_TABLE";
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        db.Close();
                        instlbl.Text = ds.Tables[0].Rows[0]["totalRow"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        void lblstudBind()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS totalRow FROM STUDENT_TABLE";
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        db.Close();
                        stdlbl.Text = ds.Tables[0].Rows[0]["totalRow"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        void adminbind()
        {
           
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS totalRow FROM ADMIN_TABLE";
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        db.Close();
                        labeladmin.Text = ds.Tables[0].Rows[0]["totalRow"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        void subscriptionbind()
        {

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(sub_id) AS totalRow FROM SUBSCRIPTION_TABLE";
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        db.Close();
                        lblsubs.Text = ds.Tables[0].Rows[0]["totalRow"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        void schoolBind()
        {

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS totalRow FROM SCHOOL_TABLE";
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        db.Close();
                        lblschools.Text = ds.Tables[0].Rows[0]["totalRow"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        void evalbind()
        {

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS totalRow FROM SCHOOL_TABLE";
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        db.Close();
                        labeleval.Text = ds.Tables[0].Rows[0]["totalRow"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }
    }
}