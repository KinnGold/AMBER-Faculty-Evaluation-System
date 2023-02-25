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

namespace AMBER.Pages.Reports
{
    public partial class IndividualResult : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        string section = "";
        string schedule = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int code = 0;
                if (Request.QueryString["section_id"] != null && Request.QueryString["schedule_id"] != null)
                {
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["section_id"].ToString()), out code))
                    {
                        section = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["section_id"]);
                    }
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["schedule_id"].ToString()), out code))
                    {
                        schedule = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["schedule_id"]);
                    }
                }
                lbldatetime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
                this.celSection.InnerHtml = "Section Code: <b>" + getScheduleInfo(section, schedule, "section") + "</b>";
                this.celEvalPeriod.InnerHtml = "Evaluation Period: <b>" + getScheduleInfo(section, schedule, "period") + "</b>";
                this.celInstructorName.InnerHtml = "Name of the Faculty Member: <b>" + getScheduleInfo(section,schedule,"name")+ "</b>";
                this.celSubject.InnerHtml = "Subject: <b>" + getScheduleInfo(section,schedule,"subject") + "</b>";
                this.celGroupCount.InnerHtml = "Total of students in this Class: <b>" + getTotalGroupCount(section,"") + "</b>";
                GVbind(section,schedule);
                overallresult();
                GVbindResult(section, schedule);
                GVbindCOmment(section, schedule);
                GVBindOverall();
                ltChart.Text = makeChart("resChart", getUserResults("constructor_name"), getUserResults("AVERAGE"));
                ltCount.Text = makeChart("countChart", getUserCount("role"), getUserCount("count"));
            }
            SchoolImage();
        }
        public void overallresult()
        {
            float res = 0;
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id=EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id=INDICATOR_TABLE.constructor_id JOIN STUDENT_TABLE ON EVALUATION_TABLE.evaluator_id=STUDENT_TABLE.Id WHERE STUDENT_TABLE.section_id='" + section + "' AND CONSTRUCTOR_TABLE.role='student' AND EVALUATION_TABLE.evaluatee_id='" + getSchedInstructor(schedule) + "' AND EVALUATION_TABLE.dept_id IS NULL AND EVALUATION_TABLE.school_id=@schoolid AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY CONSTRUCTOR_TABLE.constructor_name, CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.weight ORDER BY WEIGHTED_MEAN DESC";
                    cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            var temp = Convert.ToSingle(reader["WEIGHTED_MEAN"].ToString());
                            res += Convert.ToSingle(reader["WEIGHTED_MEAN"].ToString());
                            hfRESULT.Value = res.ToString("0.00");
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
        }

        protected void GVbindResult(string section, string schedule)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id=EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id=INDICATOR_TABLE.constructor_id JOIN STUDENT_TABLE ON EVALUATION_TABLE.evaluator_id=STUDENT_TABLE.Id WHERE STUDENT_TABLE.section_id='" + section + "' AND CONSTRUCTOR_TABLE.role='student' AND EVALUATION_TABLE.evaluatee_id='" + getSchedInstructor(schedule) + "' AND EVALUATION_TABLE.dept_id IS NULL AND EVALUATION_TABLE.school_id=@schoolid AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.weight ORDER BY WEIGHTED_MEAN DESC", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridviewGeneralResult.DataSource = dr;
                    GridviewGeneralResult.DataBind();
                }
                else
                {
                    Response.Redirect("/Pages/Reports/Noresult.aspx");
                }
            }
        }
        protected void GVBindOverall()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT 'OVERALL WEIGHTED MEAN' AS description," + hfRESULT.Value + " AS result, CASE WHEN " + hfRESULT.Value + " BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN " + hfRESULT.Value + " BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN " + hfRESULT.Value + " BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN " + hfRESULT.Value + " BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN " + hfRESULT.Value + " BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridviewResults.DataSource = dr;
                    GridviewResults.DataBind();
                }
            }
        }
        protected void GVbindCOmment(string dept_id, string schedule)
        {
            int count = 0;
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT comment FROM EVALUATION_TABLE WHERE comment IS NOT NULL AND evaluatee_id='" + getSchedInstructor(schedule) + "' AND school_id=@schoolid AND dept_id IS NULL", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {

                        if (count == 0)
                        {
                            txtcomment.Text += dr["comment"].ToString();
                        }
                        else
                        {
                            txtcomment.Text += ", " + dr["comment"].ToString();
                        }
                        count++;
                    }

                    //gvComments.DataSource = dr;
                    //gvComments.DataBind();
                }
            }
            lblcomment.InnerText += " (" + count + ")";
        }
        protected void GVbind(string section,string schedule)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_id,constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN STUDENT_TABLE ON STUDENT_TABLE.Id=EVALUATION_TABLE.evaluator_id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE STUDENT_TABLE.section_id='" + section + "' AND EVALUATION_TABLE.evaluatee_id='" + getSchedInstructor(schedule) + "' AND CONSTRUCTOR_TABLE.role='student' AND EVALUATION_TABLE.school_id=@schoolid AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name ORDER BY constructor_id", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
                else
                {
                    Response.Redirect("/Pages/Reports/Noresult.aspx");
                }
            }
        }

        public string makeChart(string chartID, string label, string data)
        {
            string chart = "";
            chart = "<canvas id=\"" + chartID + "\"></canvas>";
            chart += "<script>";
            chart += "new Chart(document.getElementById('" + chartID + "'), {";
            chart += "type: 'pie',";
            chart += "data: {";
            chart += "labels: [" + label + "],";
            chart += "datasets: [{";
            chart += "label: '# of Users',";
            chart += "data: [" + data + "],";
            string[] bgColor = hexColor(chartID).Split(':');
            chart += "backgroundColor: [" + bgColor[0] + "],";
            chart += "borderColor: [" + bgColor[1] + "],";
            chart += "borderWidth: 1";
            chart += "}]";
            chart += "},";
            chart += "options: {";
            chart += "layout: {";
            chart += "padding: {left:55,right:55}";
            chart += "},";
            chart += "plugins: {";
            chart += "tooltip: {";
            chart += "enabled: true";
            chart += "},";
            chart += "labels: [";
            chart += "{";
            chart += "fontStyle: 'bold',";
            chart += "fontColor: '#303234',";
            chart += "fontSize: 16,";
            chart += "textMargin: 6,";
            chart += "render: (args) => {";
            chart += "if (args.percentage > 1) {";
            chart += "const display = args.label.toString().replace(\" \",\"\\n\");";
            chart += "return display;}},";
            chart += "position: 'center'";
            chart += "},{";
            chart += "fontStyle: 'bold',";
            chart += "fontColor: '#303234',";
            chart += "fontSize: 20,";
            chart += "textMargin: 6,";
            chart += "render: 'percentage',";
            chart += "position: 'outside'";
            chart += "},],";
            chart += "legend: {";
            chart += "display: true,";
            chart += "},";
            chart += "datalabels: {";
            chart += "color: '#303234',";
            chart += "align: 'center',";
            chart += "font: {";
            chart += "size: 18,weight: 'bold',},";
            chart += "}}},});";
            chart += "</script>";
            Session["PrevColumn"] = null;

            return chart;
        }
        public string hexColor(string chart)
        {
            if (chart == "resChart")
            {
                chart = "'rgba(54, 162, 235, 0.2)','rgba(254, 99, 131, 0.2)','rgba(76, 192, 192, 0.2)','rgba(255, 159, 64, 0.2)','rgba(153, 102, 255, 0.2)','rgba(255, 204, 86, 0.2)','rgba(202, 203, 207, 0.2)'";
                chart += ":";
                chart += "'rgba(54, 162, 235, 1)','rgba(254, 99, 131, 1)','rgba(76, 192, 192, 1)','rgba(255, 159, 64, 1)','rgba(153, 102, 255, 1)','rgba(255, 204, 86, 1)','rgba(202, 203, 207, 1)'";
            }
            else
            {
                chart = "'rgba(255, 159, 64, 0.2)','rgba(153, 102, 255, 0.2)','rgba(255, 204, 86, 0.2)','rgba(202, 203, 207, 0.2)'";
                chart += ":";
                chart += "'rgba(255, 159, 64, 1)','rgba(153, 102, 255, 1)','rgba(255, 204, 86, 1)','rgba(202, 203, 207, 1)'";
            }
            return chart;
        }
        public string getUserResults(string ColumnName)
        {
            string result = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id=EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id=INDICATOR_TABLE.constructor_id JOIN STUDENT_TABLE ON EVALUATION_TABLE.evaluator_id=STUDENT_TABLE.Id WHERE STUDENT_TABLE.section_id='" + section + "' AND CONSTRUCTOR_TABLE.role='student' AND EVALUATION_TABLE.evaluatee_id='" + getSchedInstructor(schedule) + "' AND EVALUATION_TABLE.dept_id IS NULL AND EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.weight ORDER BY constructor_id";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                if (ColumnName == "AVERAGE")
                                {
                                    result += reader[ColumnName].ToString() + ",";
                                }
                                else
                                {
                                    result += "'" + reader[ColumnName].ToString() + "',";
                                }
                            }
                        }
                        else
                        {

                        }

                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }

            var res = "";
            if (result != "")
            {
                res = result.Substring(0, result.Length - 1);
            }
            else
            {
                res = null;
            }
            return res;
        }

        public string getUserCount(string ColumnName)
        {
            string result = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM (VALUES('Participated',(SELECT COUNT(DISTINCT evaluator_id) FROM EVALUATION_TABLE WHERE evaluator_id in (SELECT Id FROM STUDENT_TABLE WHERE section_id = '" + section + "') AND school_id = '" + Session["school"].ToString() + "' AND evaluatee_id = '" + getSchedInstructor(schedule) + "')),('Not Participated',(SELECT COUNT(DISTINCT Id) FROM STUDENT_TABLE WHERE section_id = '" + section + "' AND id NOT IN (SELECT evaluator_id FROM EVALUATION_TABLE WHERE evaluatee_id = '" + getSchedInstructor(schedule) + "') AND school_id ='" + Session["school"].ToString() + "'))) userCount(role,count) WHERE count != 0";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                if (ColumnName == "count")
                                {
                                    result += reader[ColumnName].ToString() + ",";
                                }
                                else
                                {
                                    result += "'" + reader[ColumnName].ToString() + "',";
                                }
                            }
                        }
                        else
                        {

                        }

                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }

            var res = "";
            if (result != "")
            {
                res = result.Substring(0, result.Length - 1);
            }
            else
            {
                res = null;
            }
            return res;
        }

        public string getTotalGroupCount(string section, string getString)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(*) AS COUNT FROM STUDENT_TABLE WHERE section_id = '" + section + "' AND school_id = '"+ Session["school"].ToString() +"'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            getString = reader["COUNT"].ToString();
                        }
                        else
                        {
                            Response.Redirect("/Pages/Reports/Noresult.aspx");
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
            return getString;
        }

        public string getScheduleInfo(string section,string sched,string getString)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT EVALUATION_TABLE.datetime_eval,STUDENT_TABLE.role,SECTION_TABLE.section_name,(INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR,('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name) AS SUBJECT From SCHEDULE_TABLE JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id=INSTRUCTOR_TABLE.Id JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id=SUBJECT_TABLE.subject_Id JOIN SECTION_TABLE ON SCHEDULE_TABLE.section_id=SECTION_TABLE.section_id JOIN STUDENT_TABLE ON SECTION_TABLE.section_id=STUDENT_TABLE.section_id JOIN EVALUATION_TABLE ON EVALUATION_TABLE.evaluator_id=STUDENT_TABLE.Id WHERE SECTION_TABLE.section_id='" + section + "'AND SCHEDULE_TABLE.sched_Id = '" + sched + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if(getString == "name")
                            {
                                getString = reader["INSTRUCTOR"].ToString();
                            }
                            else if(getString == "subject")
                            {
                                getString = reader["SUBJECT"].ToString();
                            }
                            else if (getString == "section")
                            {
                                getString = reader["section_name"].ToString();
                            }
                            else if (getString == "role")
                            {
                                getString = reader["role"].ToString();
                            }
                            else if (getString == "period")
                            {
                                getString = getEvalPeriod(reader["datetime_eval"].ToString());
                            }
                        }
                        else
                        {
                            Response.Redirect("/Pages/Reports/Noresult.aspx");
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
            return getString;
        }
        public string getEvalPeriod(string datetime)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT (SEMESTER_TABLE.description+' SY '+SEMESTER_TABLE.year) AS SEMESTER FROM SEMESTER_TABLE WHERE '" + datetime + "' BETWEEN evaluationStart AND evaluationEnd";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            datetime = reader["SEMESTER"].ToString();
                        }
                        else
                        {
                            Response.Redirect("/Pages/Reports/Noresult.aspx");
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
            return datetime;
        }

        public string getSchedInstructor(string id)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SCHEDULE_TABLE WHERE sched_Id='" + id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader["instructor_id"].ToString();
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
            return id;
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            GVbind(section,schedule);
        }

        protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvIndicator = (GridView)e.Row.FindControl("GridView2");
            int constructor = Int32.Parse(GridView1.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY INDICATOR_TABLE.indicator_Id)row_num, INDICATOR_TABLE.indicator_Id,indicator_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id=EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id=INDICATOR_TABLE.constructor_id JOIN STUDENT_TABLE ON EVALUATION_TABLE.evaluator_id=STUDENT_TABLE.Id WHERE CONSTRUCTOR_TABLE.constructor_id='" + constructor + "' AND EVALUATION_TABLE.evaluatee_id='" + getSchedInstructor(schedule) + "' AND EVALUATION_TABLE.dept_id IS NULL AND STUDENT_TABLE.section_id='" + section + "' AND EVALUATION_TABLE.school_id=@school_id AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY INDICATOR_TABLE.indicator_Id, indicator_name ", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvIndicator.DataSource = dr;
                    gvIndicator.DataBind();
                }
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
                    cmd.CommandText = "SELECT school_picture, school_name, address FROM SCHOOL_TABLE WHERE Id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        labelSchoolName.Text = reader["school_name"].ToString();
                        labelSchoolAddress.Text = reader["address"].ToString();
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
    }
}