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

namespace AMBER.Pages.Reports
{
    public partial class DepartmentReport : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Pages/LoginPage.aspx");
            }
            if (!IsPostBack)
            {
                populateDDL();
                populateDDL2("");
                
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

        void populateDDL2(string query)
        {
            string  selectme = "-Peer-";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT id, (''+INSTRUCTOR_TABLE.fname+' '+INSTRUCTOR_TABLE.mname+' '+INSTRUCTOR_TABLE.lname+'') AS NAME FROM INSTRUCTOR_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL "+ query;
                        ddlPeer.DataValueField = "id";
                        ddlPeer.DataTextField = "NAME";
                        ddlPeer.DataSource = cmd.ExecuteReader();
                        ddlPeer.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlPeer.Items.Add(selectme);
            ddlPeer.Items.FindByText(selectme.ToString()).Selected = true;
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDepartment.SelectedValue != "-Department-")
            {
                string[] query = { "SELECT CONSTRUCTOR_TABLE.constructor_name, CAST(AVG(CAST(rate  as float)) as DECIMAL(10,2)) AS AVERAGE FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id = INDICATOR_TABLE.constructor_id WHERE INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "' AND EVALUATION_TABLE.school_id = '" + Session["school"].ToString() + "'  GROUP BY constructor_name,CONSTRUCTOR_TABLE.role ORDer BY AVERAGE DESC", "" };
                populateDDL2(" AND dept_id =" + ddlDepartment.SelectedValue);
                ltChart.Text = makeChart("sampleChart", getChartValues("constructor_name", query[0]), getChartValues("AVERAGE", query[0]));
            }
            else
            {
                ltChart.Text = String.Empty;
                populateDDL2("");
            }
        }

        protected void ddlPeer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPeer.SelectedValue != "-Peer-")
            {
                string[] query = {"SELECT CONSTRUCTOR_TABLE.constructor_name, CAST(AVG(CAST(rate  as float)) as DECIMAL(10,2)) AS AVERAGE FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluatee_id JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id JOIN CONSTRUCTOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id = INDICATOR_TABLE.constructor_id WHERE INSTRUCTOR_TABLE.dept_id = '" + ddlDepartment.SelectedValue + "' AND EVALUATION_TABLE.evaluatee_id = '" + ddlPeer.SelectedValue + "' AND EVALUATION_TABLE.school_id = '"+ Session["school"].ToString() +"'  GROUP BY constructor_name,CONSTRUCTOR_TABLE.role ORDer BY AVERAGE DESC" };
                //GVbind("AND SECTION_TABLE.section_id=" + ddlPeer.SelectedValue, " AND DEPARTMENT_TABLE.dept_id ='" + ddlDepartment.SelectedValue + "'");
                ltChart.Text = makeChart("sampleChart", getChartValues("constructor_name", query[0]), getChartValues("AVERAGE", query[0]));
            }
            else
            {

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
    }
}