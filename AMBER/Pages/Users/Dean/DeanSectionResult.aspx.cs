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

namespace AMBER.Pages.Users.Dean
{
    public partial class DeanSectionResult : System.Web.UI.Page
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
                populateDDL("");
            }
        }

        void populateDDL(string query)
        {
            string selectme = "-Section-";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT section_id, section_name FROM SECTION_TABLE JOIN COURSE_TABLE ON COURSE_TABLE.course_id = SECTION_TABLE.course_id JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id = COURSE_TABLE.dept_id WHERE SECTION_TABLE.school_id='" + Session["school"].ToString() + "' " + query + "";
                        ddlSection.DataValueField = "section_id";
                        ddlSection.DataTextField = "section_name";
                        ddlSection.DataSource = cmd.ExecuteReader();
                        ddlSection.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlSection.Items.Add(selectme);
            ddlSection.Items.FindByText(selectme.ToString()).Selected = true;
        }

        protected void GVbind(string condition1)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT section_id, section_name FROM SECTION_TABLE JOIN COURSE_TABLE ON COURSE_TABLE.course_id = SECTION_TABLE.course_id JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id = COURSE_TABLE.dept_id WHERE SECTION_TABLE.school_id=@school_id AND DEPARTMENT_TABLE.dept_id =" + Session["dept"].ToString() + " " + condition1 + " ORDER BY section_name", db);
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
                    //GridView1.DataSource = dr;
                    //GridView1.DataBind();
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvsched = (GridView)e.Row.FindControl("GridviewSchedule");
            int section = Int32.Parse(GridView1.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT SCHEDULE_TABLE.sched_Id,('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name) AS SUBJECT,(INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR,SCHEDULE_TABLE.time, SCHEDULE_TABLE.day,CASE WHEN AVG(rate) IS NULL THEN 'No' ELSE 'Yes' END AS isEvaluated FROM SECTION_TABLE JOIN SCHEDULE_TABLE ON SECTION_TABLE.section_id=SCHEDULE_TABLE.section_id JOIN SUBJECT_TABLE ON SUBJECT_TABLE.subject_Id=SCHEDULE_TABLE.subject_id JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id=SCHEDULE_TABLE.instructor_id LEFT JOIN EVALUATION_TABLE ON INSTRUCTOR_TABLE.Id=EVALUATION_TABLE.evaluatee_id WHERE SECTION_TABLE.section_id='" + section + "' AND SCHEDULE_TABLE.school_id=@school_id AND SCHEDULE_TABLE.isDeleted IS NULL AND EVALUATION_TABLE.dept_id IS NULL GROUP BY SCHEDULE_TABLE.sched_Id,('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name),(INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname),SCHEDULE_TABLE.time, SCHEDULE_TABLE.day, SECTION_TABLE.section_name,SCHEDULE_TABLE.instructor_id ORDER BY SCHEDULE_TABLE.instructor_id", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvsched.DataSource = dr;
                    gvsched.DataBind();
                }
            }
        }

        protected void GridviewSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "selectSched")
            {
                int parentRowIndex = ((GridViewRow)(((GridView)sender).NamingContainer)).RowIndex;
                GridViewRow ParentGvRow = GridView1.Rows[parentRowIndex];
                string parentKey = GridView1.DataKeys[parentRowIndex].Value.ToString();

                GridView ChildGV = ParentGvRow.FindControl("GridviewSchedule") as GridView;
                int childRowindex = Convert.ToInt32(e.CommandArgument.ToString());
                string childKey = ChildGV.DataKeys[childRowindex].Value.ToString();

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('https://localhost:44311/Pages/Reports/IndividualSectionResult.aspx?section_id=" + AMBER.URLEncryption.GetencryptedQueryString(parentKey) + "&schedule_id=" + AMBER.URLEncryption.GetencryptedQueryString(childKey) + "');", true);
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "NewTab", "window.open('https://localhost:44311/Pages/Reports/IndividualSectionResult.aspx?section_id="+ AMBER.URLEncryption.GetencryptedQueryString(parentKey) + "&schedule_id="+ AMBER.URLEncryption.GetencryptedQueryString(childKey) +"','_blank');", true);
            }

        }

        protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSection.SelectedValue != "-Section-")
            {
                GVbind("AND SECTION_TABLE.section_id=" + ddlSection.SelectedValue);
            }
            else
            {
                GVbind("");
            }

        }
    }
}