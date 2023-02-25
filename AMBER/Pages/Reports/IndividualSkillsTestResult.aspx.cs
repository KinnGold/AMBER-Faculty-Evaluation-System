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
using System.Web.UI.DataVisualization.Charting;

namespace AMBER.Pages.Reports
{
    public partial class IndividualSkillsTestResult : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        string dept_id = "";
        string member_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int code = 0;
                if (Request.QueryString["dept_id"] != null && Request.QueryString["id"] != null)
                {
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["dept_id"].ToString()), out code))
                    {
                        dept_id = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["dept_id"]);
                    }
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["id"].ToString()), out code))
                    {
                        member_id = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["id"]);
                    }
                }
                lbldatetime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
                this.celEvalPeriod.InnerHtml = "Evaluation Period: <b>" + getScheduleInfo(dept_id, member_id, "period") + "";
                this.celInstructorName.InnerHtml = "Name of the Faculty Member: <b>" + getScheduleInfo(dept_id, member_id, "name") + "</b>";
                this.celSubject.InnerHtml = "Department Code: <b>" + getScheduleInfo(dept_id, member_id, "code") + "</b>";
                overallresult();
                GVbind(dept_id, member_id);
                GVbindResult(dept_id, member_id);
                GVbindCOmment(dept_id, member_id);
                GVBindOverall();
                showPercentageScore();
            }
            SchoolImage();
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
            chart += "padding: 15";
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
            chart += "position: 'border'";
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
            if (chart == "UserChart")
            {
                chart = "'rgba(54, 162, 235, 0.2)','rgba(254, 99, 131, 0.2)','rgba(76, 192, 192, 0.2)','rgba(255, 159, 64, 0.2)','rgba(153, 102, 255, 0.2)','rgba(255, 204, 86, 0.2)','rgba(202, 203, 207, 0.2)'";
                chart += ":";
                chart += "'rgba(54, 162, 235, 1)','rgba(254, 99, 131, 1)','rgba(76, 192, 192, 1)','rgba(255, 159, 64, 1)','rgba(153, 102, 255, 1)','rgba(255, 204, 86, 1)','rgba(202, 203, 207, 1)'";
            }
            else if (chart == "participateChart2")
            {
                chart = "'rgba(255, 26, 104, 0.2)','rgba(54, 162, 235, 0.2)'";
                chart += ":";
                chart += "'rgba(255, 26, 104, 1)','rgba(54, 162, 235, 1)'";
            }
            return chart;
        }

        public string getChartValues(string ColumnName, string query)
        {
            if (Session["PrevColumn"] == null)
            {
                Session["PrevColumn"] = ColumnName;
            }
            string result = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                if (ColumnName != Session["PrevColumn"].ToString())
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

            return result = result.Substring(0, result.Length - 1);
        }

        private void showPercentageScore()
        {
            string chart = "";
            //!important
            chart = "<canvas id=\"UserChart\"></canvas>";
            chart += "<script>";
            chart += "const data = {";
            chart += "labels: [" + getUserResults("constructor_name") + "],";
            chart += "datasets: [{";
            chart += "label: 'Average',";
            chart += "data: [" + getUserResults("AVERAGE") + "],";
            //chart += "backgroundColor: [\"#7A0DFF\",\"#1275B3\",\"#FFD84D\"],";
            //chart += "borderColor: [\"#5A12B3\",\"#0DA3FF\",\"#FFCD19\"],";
            chart += "backgroundColor: ['rgba(54, 162, 235, 0.2)','rgba(254, 99, 131, 0.2)','rgba(76, 192, 192, 0.2)','rgba(255, 159, 64, 0.2)','rgba(153, 102, 255, 0.2)','rgba(255, 204, 86, 0.2)','rgba(202, 203, 207, 0.2)'],";
            chart += "borderColor: ['rgba(54, 162, 235, 1)','rgba(254, 99, 131, 1)','rgba(76, 192, 192, 1)','rgba(255, 159, 64, 1)','rgba(153, 102, 255, 1)','rgba(255, 204, 86, 1)','rgba(202, 203, 207, 1)'],";
            chart += "borderWidth: 1 }]};";
            chart += "const config = {";
            chart += "type: 'pie',"; //chart type
            chart += "data,";
            chart += "options: {";
            chart += "layout: {padding: 15},";
            chart += "plugins: {";
            chart += "tooltip: {enabled: false},";
            chart += "labels: {position: 'border',fontStyle: 'bold',fontSize: 14,textMargin: 6,";
            chart += "render: (args) => {if(args.percentage > 4){return `${args.label}: ${args.percentage}%`;}}},";
            chart += "datalabels: {";
            chart += "font: {size: 18, weight: 'bold'},";
            chart += "formatter: (value) => {";
            chart += "if(value < 3) {return value;}}}}},";
            chart += "plugins: [ChartDataLabels]};";
            //!important
            chart += "new Chart(document.getElementById('UserChart'),config);";
            chart += "</script>";
            ltOverallGraph.Text = chart;
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
                        cmd.CommandText = "SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + member_id + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id";
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

            return result = result.Substring(0, result.Length - 1);
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
                    cmd.CommandText = "SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + member_id + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id";
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

        protected void GVbindResult(string dept_id, string member_id)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + member_id + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id", db);
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
                SqlCommand cmd = new SqlCommand("SELECT 'OVERALL WEIGHTED MEAN' AS description,'" + hfRESULT.Value + "' AS result, CASE WHEN '" + hfRESULT.Value + "' BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN '" + hfRESULT.Value + "' BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN '" + hfRESULT.Value + "' BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN '" + hfRESULT.Value + "' BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN '" + hfRESULT.Value + "' BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridviewResults.DataSource = dr;
                    GridviewResults.DataBind();
                }
            }
        }

        protected void GVbindCOmment(string dept_id, string member_id)
        {
            int count = 0;
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT comment FROM SKILLSTEST_TABLE WHERE comment IS NOT NULL AND evaluatee_id='" + member_id + "' AND evaluator_id !='" + member_id + "' AND school_id=@schoolid AND dept_id IS NOT NULL", db);
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

        protected void GVbind(string dept_id, string member_id)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + member_id + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GVResults.DataSource = dr;
                    GVResults.DataBind();
                }
                else
                {
                    Response.Redirect("/Pages/Reports/Noresult.aspx");
                }
            }
        }
        public string getScheduleInfo(string dept_id, string member_id, string getString)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT datetime_eval,role,dept_code,dept_name,(INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR  FROM DEPARTMENT_TABLE JOIN INSTRUCTOR_TABLE ON DEPARTMENT_TABLE.dept_Id=INSTRUCTOR_TABLE.dept_id JOIN EVALUATION_TABLE ON INSTRUCTOR_TABLE.Id=EVALUATION_TABLE.evaluatee_id WHERE DEPARTMENT_TABLE.dept_Id='" + dept_id + "' AND INSTRUCTOR_TABLE.Id='" + member_id + "' AND EVALUATION_TABLE.evaluator_id!='" + member_id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if (getString == "name")
                            {
                                getString = reader["INSTRUCTOR"].ToString();
                            }
                            else if (getString == "subject")
                            {
                                getString = reader["SUBJECT"].ToString();
                            }
                            else if (getString == "code")
                            {
                                getString = reader["dept_code"].ToString();
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

        public void SchoolImage()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT school_picture, school_name,address FROM SCHOOL_TABLE WHERE Id = '" + Session["school"].ToString() + "'";
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

        protected void GVResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvIndicator = (GridView)e.Row.FindControl("GridView2");
            int constructor = Int32.Parse(GVResults.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY INDICATOR_TABLE.indicator_id),CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name, INDICATOR_TABLE.indicator_name,INDICATOR_TABLE.indicator_id,CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(SKILLSTEST_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM SKILLSTEST_TABLE JOIN INDICATOR_TABLE ON SKILLSTEST_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE SKILLSTEST_TABLE.evaluatee_id ='" + member_id + "' AND INDICATOR_TABLE.constructor_id='" + constructor + "' AND SKILLSTEST_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id, INDICATOR_TABLE.indicator_id, CONSTRUCTOR_TABLE.constructor_name, INDICATOR_TABLE.indicator_name, CONSTRUCTOR_TABLE.weight ORDER BY CONSTRUCTOR_TABLE.constructor_id", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvIndicator.DataSource = dr;
                    gvIndicator.DataBind();
                }
            }
        }
    }
}