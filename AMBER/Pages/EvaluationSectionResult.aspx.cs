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
    public partial class EvaluationResult : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        string query = "";
        protected HtmlGenericControl PageTitle = new HtmlGenericControl();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("LoginPage.aspx");

            }
            if (!IsPostBack)
            {
                GVbind("","","");
                populateDDL("");
                popuplateDDL2();
                
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
                        cmd.CommandText = "SELECT section_id, section_name FROM SECTION_TABLE JOIN COURSE_TABLE ON COURSE_TABLE.course_id = SECTION_TABLE.course_id JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id = COURSE_TABLE.dept_id WHERE SECTION_TABLE.school_id='" + Session["school"].ToString() + "' "+ query +"";
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

        void popuplateDDL2()
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

        protected void GVbind(string condition1,string condition2, string condition3)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT SECTION_TABLE.section_id, SECTION_TABLE.section_name FROM SECTION_TABLE JOIN COURSE_TABLE ON COURSE_TABLE.course_id = SECTION_TABLE.course_id JOIN DEPARTMENT_TABLE ON DEPARTMENT_TABLE.dept_Id = COURSE_TABLE.dept_id JOIN STUDENT_TABLE ON STUDENT_TABLE.section_id = SECTION_TABLE.section_id LEFT JOIN EVALUATION_TABLE ON EVALUATION_TABLE.evaluator_id = STUDENT_TABLE.Id WHERE SECTION_TABLE.school_id = @school_id " + condition1 + " " + condition2 + " GROUP BY SECTION_TABLE.section_id, SECTION_TABLE.section_name " + condition3 + " ORDER BY SECTION_TABLE.section_name", db);
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
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT COUNT(DISTINCT evaluator_id) COUNT,SCHEDULE_TABLE.sched_Id ,SCHEDULE_TABLE.sched_code,(INSTRUCTOR_TABLE.fname +' '+ CASE WHEN INSTRUCTOR_TABLE.mname ='' THEN '' ELSE SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. ' END +INSTRUCTOR_TABLE.lname) AS INSTRUCTOR, ('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name) AS SUBJECT, SCHEDULE_TABLE.time, SCHEDULE_TABLE.day,CASE WHEN AVG(rate) IS NULL THEN 'No' ELSE 'Yes' END AS isEvaluated FROM SCHEDULE_TABLE LEFT JOIN EVALUATION_TABLE ON EVALUATION_TABLE.evaluatee_id = SCHEDULE_TABLE.instructor_id JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = SCHEDULE_TABLE.instructor_id JOIN SUBJECT_TABLE ON SUBJECT_TABLE.subject_Id = SCHEDULE_TABLE.subject_id WHERE (EVALUATION_TABLE.evaluator_id IN (SELECT STUDENT_TABLE.Id FROM SECTION_TABLE JOIN STUDENT_TABLE ON STUDENT_TABLE.section_id = SECTION_TABLE.section_id WHERE SECTION_TABLE.section_id = '" + section + "') AND EVALUATION_TABLE.evaluatee_id IN (SELECT SCHEDULE_TABLE.instructor_id FROM SCHEDULE_TABLE WHERE section_id = '" + section + "') OR rate is NULL) AND SCHEDULE_TABLE.section_id = '" + section + "' AND SCHEDULE_TABLE.school_id= @school_id GROUP BY SCHEDULE_TABLE.sched_code, INSTRUCTOR_TABLE.fname, INSTRUCTOR_TABLE.mname, INSTRUCTOR_TABLE.lname, SUBJECT_TABLE.subject_code, SUBJECT_TABLE.subject_name, SCHEDULE_TABLE.time, SCHEDULE_TABLE.day,SCHEDULE_TABLE.sched_Id "+query , db);
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
                if(ddlDepartment.SelectedValue != "-Department-")
                {
                    GVbind("AND SECTION_TABLE.section_id=" + ddlSection.SelectedValue, "AND DEPARTMENT_TABLE.dept_id ='" + ddlDepartment.SelectedValue + "'","");
                }
                else
                {
                    GVbind("AND SECTION_TABLE.section_id=" + ddlSection.SelectedValue,"","");
                }
                
            }
            else
            {
                if (ddlDepartment.SelectedValue != "-Department-")
                {
                    GVbind("", "AND DEPARTMENT_TABLE.dept_id ='" + ddlDepartment.SelectedValue + "'","");
                }
                else
                {
                    GVbind("", "","");
                }
            }
            
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDepartment.SelectedValue != "-Department-")
            {
                populateDDL(" AND DEPARTMENT_TABLE.dept_id =" + ddlDepartment.SelectedValue);
                if (ddlSection.SelectedValue != "-Section-")
                {
                    GVbind("AND SECTION_TABLE.section_id=" + ddlSection.SelectedValue, "DEPARTMENT_TABLE.dept_id ='" + ddlDepartment.SelectedValue + "'","");
                }
                else
                {
                    GVbind("", "AND DEPARTMENT_TABLE.dept_id ='" + ddlDepartment.SelectedValue + "'","");
                }
            }
            else
            {
                populateDDL("");
                GVbind("","","");
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlStatus.SelectedValue == "-Evaluated?-")
            {
                query = "";
                GVbind("", "", "");
            }
            else if(ddlStatus.SelectedValue == "Yes")
            {
                populateDDL(" AND DEPARTMENT_TABLE.dept_id =" + ddlDepartment.SelectedValue);
                if (ddlSection.SelectedValue != "-Section-")
                {
                    GVbind("AND SECTION_TABLE.section_id=" + ddlSection.SelectedValue, "DEPARTMENT_TABLE.dept_id ='" + ddlDepartment.SelectedValue + "'", "");
                }
                else
                {
                    query = "HAVING (CASE WHEN AVG(rate) IS NULL THEN 'No' ELSE 'Yes' END) = 'Yes'";
                    GVbind("", "AND DEPARTMENT_TABLE.dept_id ='" + ddlDepartment.SelectedValue + "'", "HAVING (CASE WHEN AVG(rate) IS NULL THEN 'No' ELSE 'Yes' END) = 'Yes'");
                }
            }
            else
            {
                query = "HAVING (CASE WHEN AVG(rate) IS NULL THEN 'No' ELSE 'Yes' END) = 'No'";
                GVbind("", "", "HAVING (CASE WHEN AVG(rate) IS NULL THEN 'No' ELSE 'Yes' END) = 'No'");
            }
        }
    }
}