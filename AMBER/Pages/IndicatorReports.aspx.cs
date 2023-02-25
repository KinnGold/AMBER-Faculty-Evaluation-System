using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AMBER.Pages
{
    public partial class IndicatorReports : System.Web.UI.Page
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
                hasRecord();
            }
        }
        public void hasRecord()
        {
            try
            {
                using (SqlConnection db = new SqlConnection(connDB))
                {
                    db.Open();
                    SqlCommand cmd = new SqlCommand("SELECT INDICATOR_TABLE.indicator_id,INDICATOR_TABLE.indicator_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE RIGHT JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id WHERE EVALUATION_TABLE.school_id=@school_id AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY INDICATOR_TABLE.indicator_id,INDICATOR_TABLE.indicator_name", db);
                    cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
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
}