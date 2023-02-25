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
    public partial class ResultsByCategory : System.Web.UI.Page
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
                GVbind();
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM SCHOOL_TABLE ORDER BY school_name ASC";
                            DropDownSchool.DataValueField = "Id";
                            DropDownSchool.DataTextField = "school_name";
                            DropDownSchool.DataSource = cmd.ExecuteReader();
                            DropDownSchool.DataBind();
                            DropDownSchool.Items.Insert(0, new ListItem("ALL", ""));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                DropDownSchool.Items.Add("Select School");
                DropDownSchool.Items.FindByText("Select School").Selected = true;
            }
        }

        public void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SElECT CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE RIGHT JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id GROUP BY CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void DropDownSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownSchool.SelectedItem.Text == "ALL")
            {
                GridView1.EditIndex = -1;
                GVbind();
            }
            else if (DropDownSchool.SelectedItem.Text == "Select School")
            {
                GridView1.EditIndex = -1;
                GVbind();
            }
            else
            {
                try
                {
                    DataTable dt = new DataTable();
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SElECT CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE RIGHT JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE EVALUATION_TABLE.school_id='" + DropDownSchool.SelectedValue + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name";
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                        }
                        db.Close();
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
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
}