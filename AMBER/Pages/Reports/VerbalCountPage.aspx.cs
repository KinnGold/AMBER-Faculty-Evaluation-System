using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AMBER.Pages.Reports
{
    public partial class VerbalCountPage : System.Web.UI.Page
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
                GVbind2("","");
                populateDDL();
                initial();
            }
        }
        protected void GVbind(string query)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT verbalDesc,count  FROM (VALUES ('Outstanding',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id=@schoolid " + query + " GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 5 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 4.5) AS countOutstanding)),('Very Satisfactory',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id=@schoolid " + query + " GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 4.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 3.5) AS countVerySatisfactory)),('Satisfactory',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id=@schoolid " + query + " GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 3.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 2.5) AS countSatisfactory)),('Fair',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id=@schoolid " + query + " GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 2.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 1.5) AS countFair)),('Poor',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id=@schoolid " + query + " GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 1.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 1) AS countPoor))) as b(verbalDesc,count) WHERE count != 0", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
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

        protected void GVbind2(string query, string queryVerbal)
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT evaluatee_id, INSTRUCTOR_TABLE.dept_id, profile_picture, (''+INSTRUCTOR_TABLE.fname+' '+ INSTRUCTOR_TABLE.mname+' '+ INSTRUCTOR_TABLE.lname +'') AS NAME, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT)))AS AVERAGE, CASE WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 4.5 AND 5 THEN 'Outstanding' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 3.5 AND 4.49 THEN 'Very Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 2.5 AND 3.49 THEN 'Satisfactory' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1.5 AND 2.49 THEN 'Fair' WHEN  CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) BETWEEN 1 AND 1.49 THEN 'Poor' ELSE 'invalid' END AS verbal_rate FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON EVALUATION_TABLE.evaluatee_id = INSTRUCTOR_TABLE.Id WHERE EVALUATION_TABLE.school_id=@schoolid "+ query + " AND isDeleted IS NULL GROUP BY INSTRUCTOR_TABLE.dept_id, profile_picture, (''+INSTRUCTOR_TABLE.fname+' '+ INSTRUCTOR_TABLE.mname+' '+ INSTRUCTOR_TABLE.lname +''), evaluatee_id " + queryVerbal + " ORDER BY AVERAGE DESC ", db);
                cmd.Parameters.AddWithValue("@schoolid", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    PlaceHolder2.Visible = false;
                    PlaceHolder1.Visible = true;
                    gvVerbal.DataSource = dr;
                    gvVerbal.DataBind();
                }
                else
                {
                    PlaceHolder2.Visible = true;
                    PlaceHolder1.Visible = false;
                }
            }
        }
        void initial()
        {
            string[] query = { "SELECT verbalDesc,count  FROM (VALUES ('Outstanding',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 5 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 4.5) AS countOutstanding)),('Very Satisfactory',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 4.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 3.5) AS countVerySatisfactory)),('Satisfactory',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 3.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 2.5) AS countSatisfactory)),('Fair',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 2.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 1.5) AS countFair)),('Poor',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 1.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 1) AS countPoor))) as b(verbalDesc,count) WHERE count != 0" };
            ltCount.Text = makeChart("sampleChart", getChartValues("verbalDesc", query[0]), getChartValues("count", query[0]));
        }
        void populateDDL()
        {
            string selectme = "ALL";
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
            chart += "label: '# of Faculty Member',";
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
            if (chart == "sampleChart")
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
                            plcNoChart.Visible = false;
                            plcChart.Visible = true;
                        }
                        else
                        {
                            plcNoChart.Visible = true;
                            plcChart.Visible = false;
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

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDepartment.SelectedValue != "ALL")
            {
                string[] query = { "SELECT verbalDesc,count  FROM (VALUES ('Outstanding',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 5 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 4.5) AS countOutstanding)),('Very Satisfactory',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 4.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 3.5) AS countVerySatisfactory)),('Satisfactory',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 3.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 2.5) AS countSatisfactory)),('Fair',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 2.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 1.5) AS countFair)),('Poor',(SELECT COUNT(*) FROM (SELECT evaluatee_id FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE EVALUATION_TABLE.school_id='" + Session["school"].ToString() + "' AND INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "' GROUP BY evaluatee_id HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 1.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 1) AS countPoor))) as b(verbalDesc,count) WHERE count != 0" };
                ltCount.Text = makeChart("sampleChart", getChartValues("verbalDesc", query[0]), getChartValues("count", query[0]));
                GVbind("AND INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "'");
                GVbind2("AND INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "'","");
            }
            else
            {
                initial();
                GVbind("");
                GVbind2("","");
            }
        }

        protected void gvVerbal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id = Convert.ToInt32(e.Row.Cells[0].Text);
                string imageUrl = "data:image/png;base64," + image(id);
                (e.Row.FindControl("Image1") as Image).ImageUrl = imageUrl;

                int dept = Convert.ToInt32(e.Row.Cells[1].Text);
                Label rankAverage = (Label)e.Row.FindControl("lblAVERAGE");
                rankAverage.Text = getWeightedMean(id, dept);
            }
        }
        public string getWeightedMean(int instructor, int department)
        {
            string weightedMean = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT CONVERT(DECIMAL(10,2),SUM(weightedSum)/SUM(weight)) AS weightedMean FROM (SELECT constructor_name, CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE, weight,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT))) * weight AS 'weightedSum' FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id = INDICATOR_TABLE.constructor_id JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id WHERE INSTRUCTOR_TABLE.dept_id= '" + department + "' AND INSTRUCTOR_TABLE.Id= '" + instructor + "' AND evaluator_id != evaluatee_id AND EVALUATION_TABLE.school_id = '" + Session["school"].ToString() + "' GROUP BY constructor_name,weight) as weight";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            weightedMean = reader["weightedMean"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return weightedMean;
        }

        public string image(int id)
        {
            string profilePicture = "";
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT profile_picture FROM INSTRUCTOR_TABLE WHERE id = '" + id + "' AND school_id = '" + Session["school"].ToString() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader["profile_picture"] != System.DBNull.Value)
                        {
                            byte[] bytes = (byte[])reader["profile_picture"];
                            profilePicture = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "notFound()", true);
                    }
                }
            }
            return profilePicture;
        }

        protected void ddlVerbal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlDepartment.SelectedValue != "ALL")
            {
                GVbind2("AND INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "'", ddlVerbal.SelectedValue);
            }
            else
            {
                GVbind("");
                GVbind2("", ddlVerbal.SelectedValue);
            }
            
        }
    }
}