using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace AMBER.Pages
{
    public partial class EvaluationPeerResult : System.Web.UI.Page
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
                GVbind("");
                populateDDL();
            }
        }

        void populateDDL()
        {
            string selectme = "-Department-";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT dept_id, dept_name FROM DEPARTMENT_TABLE WHERE SCHOOL_ID = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        ddlDepartment.DataValueField = "dept_id";
                        ddlDepartment.DataTextField = "dept_name";
                        ddlDepartment.DataSource = cmd.ExecuteReader();
                        ddlDepartment.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlDepartment.Items.Add(selectme);
            ddlDepartment.Items.FindByText(selectme.ToString()).Selected = true;
        }

        protected void GVbind(string query)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT dept_Id,dept_name FROM DEPARTMENT_TABLE WHERE school_id=@school_id AND isDeleted IS NULL "+ query +" ORDER BY dept_name", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    plcNoData.Visible = false;
                    plcData.Visible = true;
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
                else
                {
                    plcNoData.Visible = true;
                    plcData.Visible = false;
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvMember = (GridView)e.Row.FindControl("GridviewMembers");
            int sched = Int32.Parse(GridView1.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(DISTINCT evaluator_id) AS COUNT, evaluatee_id,(INSTRUCTOR_TABLE.fname +' '+ CASE WHEN INSTRUCTOR_TABLE.mname ='' THEN '' ELSE SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. ' END +INSTRUCTOR_TABLE.lname) AS INSTRUCTOR,CASE WHEN rate IS NULL THEN 'No' ELSE 'Yes' END AS isEvaluated FROM INSTRUCTOR_TABLE LEFT JOIN EVALUATION_TABLE ON EVALUATION_TABLE.evaluatee_id = INSTRUCTOR_TABLE.Id WHERE ((EVALUATION_TABLE.evaluator_id IN (SELECT DISTINCT INSTRUCTOR_TABLE.Id FROM INSTRUCTOR_TABLE JOIN EVALUATION_TABLE ON EVALUATION_TABLE.evaluator_id = INSTRUCTOR_TABLE.Id WHERE INSTRUCTOR_TABLE.dept_id = '" + sched + "') AND EVALUATION_TABLE.evaluatee_id IN (SELECT Id FROM INSTRUCTOR_TABLE WHERE dept_id = '" + sched + "')) AND EVALUATION_TABLE.evaluator_id != EVALUATION_TABLE.evaluatee_id OR rate IS NULL) AND INSTRUCTOR_TABLE.dept_id = '" + sched + "' GROUP BY evaluatee_id,fname,mname,lname,rate", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvMember.DataSource = dr;
                    gvMember.DataBind();
                }
            }
        }

        protected void GridviewMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "selectMember")
            {
                int parentRowIndex = ((GridViewRow)(((GridView)sender).NamingContainer)).RowIndex;
                GridViewRow ParentGvRow = GridView1.Rows[parentRowIndex];
                string parentKey = GridView1.DataKeys[parentRowIndex].Value.ToString();

                GridView ChildGV = ParentGvRow.FindControl("GridviewMembers") as GridView;
                int childRowindex = Convert.ToInt32(e.CommandArgument.ToString());
                string childKey = ChildGV.DataKeys[childRowindex].Value.ToString();

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "NewTab", "customOpen('https://localhost:44311/Pages/Reports/IndividualPeerResult.aspx?dept_id=" + AMBER.URLEncryption.GetencryptedQueryString(parentKey) + "&id=" + AMBER.URLEncryption.GetencryptedQueryString(childKey) + "');", true);
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "NewTab", "window.open('https://localhost:44311/Pages/Reports/IndividualPeerResult.aspx?dept_id=" + AMBER.URLEncryption.GetencryptedQueryString(parentKey) + "&id=" + AMBER.URLEncryption.GetencryptedQueryString(childKey) + "','_blank');", true);
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDepartment.SelectedValue != "-Department-")
            {
                GVbind("AND dept_id=" + ddlDepartment.SelectedValue);
            }
            else
            {
                GVbind("");
            }
        }
    }
}