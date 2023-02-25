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

namespace AMBER.Super_Admin.Reports
{
    public partial class OverallCampusResult : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        string school_id = "";
        string datetime = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int code = 0;
                if (Request.QueryString["school_id"] != null)
                {
                    if (int.TryParse(URLEncryption.GetdecryptedQueryString(Request.QueryString["school_id"].ToString()), out code))
                    {
                        school_id = AMBER.URLEncryption.GetdecryptedQueryString(Request.QueryString["school_id"]);
                    }
                }
                lbldatetime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
                this.celEvalPeriod.InnerHtml = "Evaluation Period: <b>" + getEvalPeriod(datetime) + "";
                overallresult();
                GVbind();
                GVbindResult();
                GVbindCOmment();//
                GVBindOverall();
                ltChart.Text = makeChart("resChart", getUserResults("constructor_name"), getUserResults("AVERAGE"));
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
                    cmd.CommandText = "SELECT CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),((CONSTRUCTOR_TABLE.weight)/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id = CONSTRUCTOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON EVALUATION_TABLE.evaluatee_id = INSTRUCTOR_TABLE.Id WHERE EVALUATION_TABLE.evaluator_id != EVALUATION_TABLE.evaluatee_id AND EVALUATION_TABLE.school_id='"+ school_id +"' AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY CONSTRUCTOR_TABLE.constructor_name, CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.weight ORDER BY constructor_id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            var temp = Convert.ToSingle(reader["WEIGHTED_MEAN"].ToString());
                            res += Convert.ToSingle(reader["WEIGHTED_MEAN"].ToString());
                            hfRESULT.Value = (res).ToString("0.00");
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
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
            if (chart == "resChart")
            {
                chart = "'rgba(54, 162, 235, 0.2)','rgba(254, 99, 131, 0.2)','rgba(76, 192, 192, 0.2)','rgba(255, 159, 64, 0.2)','rgba(153, 102, 255, 0.2)','rgba(255, 204, 86, 0.2)','rgba(202, 203, 207, 0.2)'";
                chart += ":";
                chart += "'rgba(54, 162, 235, 1)','rgba(254, 99, 131, 1)','rgba(76, 192, 192, 1)','rgba(255, 159, 64, 1)','rgba(153, 102, 255, 1)','rgba(255, 204, 86, 1)','rgba(202, 203, 207, 1)'";
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
                        cmd.CommandText = "SELECT CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),(CONSTRUCTOR_TABLE.weight/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id = CONSTRUCTOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON EVALUATION_TABLE.evaluatee_id = INSTRUCTOR_TABLE.Id WHERE EVALUATION_TABLE.evaluator_id != EVALUATION_TABLE.evaluatee_id AND EVALUATION_TABLE.school_id='"+ school_id +"' AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY CONSTRUCTOR_TABLE.constructor_name, CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.weight ORDER BY WEIGHTED_MEAN DESC";
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
        protected void GVbindResult()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,(CONVERT(DECIMAL(10,2),((CONSTRUCTOR_TABLE.weight)/100.0)))*(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))) AS WEIGHTED_MEAN,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id = INDICATOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE (EVALUATION_TABLE.evaluator_id != EVALUATION_TABLE.evaluatee_id) AND EVALUATION_TABLE.school_id = '"+ school_id +"' GROUP BY CONSTRUCTOR_TABLE.constructor_name, CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.weight ORDER BY constructor_id", db);
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
                SqlCommand cmd = new SqlCommand("SELECT 'OVERALL WEIGHTED MEAN' AS description, CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) AS result, CASE WHEN CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate FROM (SELECT constructor_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE, weight,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) * weight AS 'weightedSum' FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id = INDICATOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE evaluator_id != evaluatee_id AND EVALUATION_TABLE.school_id = '"+ school_id +"' GROUP BY constructor_name,weight) as weight", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridviewResults.DataSource = dr;
                    GridviewResults.DataBind();
                }
            }
        }

        protected void GVbindCOmment()
        {
            int count = 0;
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT comment FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE comment IS NOT NULL AND evaluator_id != evaluatee_id AND EVALUATION_TABLE.school_id='"+ school_id +"'", db);
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

        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id=EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id=INDICATOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON EVALUATION_TABLE.evaluatee_id=INSTRUCTOR_TABLE.Id WHERE EVALUATION_TABLE.evaluator_id != EVALUATION_TABLE.evaluatee_id AND EVALUATION_TABLE.school_id='"+ school_id +"' AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY CONSTRUCTOR_TABLE.constructor_name,CONSTRUCTOR_TABLE.constructor_id ORDER BY constructor_id", db);
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

        public void SchoolImage()
        {
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT school_picture, school_name, address FROM SCHOOL_TABLE WHERE Id = '"+ school_id +"'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        labelSchoolName.Text = reader["school_name"].ToString();
                        labelSchoolAddress.Text = reader["address"].ToString();
                        lblSchoolName.Text = reader["school_name"].ToString();
                        lblSchoolAddress.Text = reader["address"].ToString();
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
                        cmd.CommandText = "SELECT(SEMESTER_TABLE.description + ' SY ' + SEMESTER_TABLE.year) AS SEMESTER FROM SEMESTER_TABLE WHERE school_id = '202250083' AND status = 'Active'";
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

        protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvIndicator = (GridView)e.Row.FindControl("GridView2");
            int constructor = Int32.Parse(GridView1.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY INDICATOR_TABLE.indicator_Id)row_num, INDICATOR_TABLE.indicator_Id,indicator_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE,CASE WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate, CONVERT(VARCHAR(14),FLOOR(CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))/5 * 100))+ '%' AS PERCENTAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id=EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id=INDICATOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON EVALUATION_TABLE.evaluatee_id=INSTRUCTOR_TABLE.Id WHERE CONSTRUCTOR_TABLE.constructor_id='97' AND EVALUATION_TABLE.evaluator_id != EVALUATION_TABLE.evaluatee_id AND EVALUATION_TABLE.school_id='"+ school_id +"' AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY INDICATOR_TABLE.indicator_Id, indicator_name", db);
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