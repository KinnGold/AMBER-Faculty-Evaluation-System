using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;


namespace AMBER.Pages.Reports
{
    public partial class Student : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["school"] != null)
            {
                
            }
            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Pages/LoginPage.aspx");
            }
            if (!IsPostBack)
            {
                string[] query = { "SELECT COUNT(id) AS COUNT,UPPER(LEFT(role,1))+LOWER(SUBSTRING(role,2,LEN(role))) as role FROM STUDENT_TABLE WHERE school_id= '" + Session["school"].ToString() + "' GROUP BY role UNION ALL SELECT COUNT(id)AS COUNT,UPPER(LEFT(role,1))+LOWER(SUBSTRING(role,2,LEN(role))) as role FROM INSTRUCTOR_TABLE WHERE school_id= '" + Session["school"].ToString() + "' GROUP BY role", "" };
                totalEvaluator();
                checkStudentEvaluators();
                checkInstructorEvaluators();
                showQuestionResult();
                showCategoryResult();
                showPercentageUsers();
                showInsPercentageParticipate();
                showStudPercentageParticipate();
                Literal2.Text = makeChart("sampleChart", getChartValues("role",query[0]), getChartValues("COUNT", query[0]));
                Literal1.Text = makeChart("participateChart2", "'Participated','Not Participated'", hvStudHasEvaluated.Value +","+hvStudNotEvaluated.Value);
                Literal3.Text = makeChart("participateChart3", "'Participated','Not Participated'", hvInsHasEvaluated.Value + "," + hvInsNotEvaluated.Value);
            }
        }

        public string makeChart(string chartID, string label, string data)
        {
            string chart = "";
            chart = "<canvas id=\""+ chartID +"\"></canvas>";
            chart += "<script>";
            chart += "new Chart(document.getElementById('" + chartID + "'), {";
            chart += "type: 'pie',";
            chart += "data: {";
            chart += "labels: ["+ label +"],";
            chart += "datasets: [{";
            chart += "label: '# of Users',";
            chart += "data: ["+ data +"],";
            string[] bgColor = hexColor(chartID).Split(':');
            chart += "backgroundColor: ["+ bgColor[0] +"],";
            chart += "borderColor: ["+ bgColor[1] +"],";
            chart += "borderWidth: 1";
            chart += "}]";
            chart += "},";
            chart += "options: {";
            chart += "layout: {";
            chart += "padding: 15";
            chart += "},";
            chart += "plugins: {";
            chart += "tooltip: {";
            chart += "enabled: false";
            chart += "},";
            chart += "labels: {";
            chart += "position: 'border',";
            chart += "fontStyle: 'bold',";
            chart += "fontColor: '#303234',";
            chart += "fontSize: 20,";
            chart += "textMargin: 6,";
            chart += "render: (args) => {";
            chart += "if (args.percentage > 0) {";
            chart += "const display = args.percentage+'%' +'\\n'+ args.label.toString().replace(\" \",\"" + "\\n" + " \");";
            chart += "return display;";
            chart += "}";
            chart += "}";
            chart += "},";
            chart += "datalabels: {";
            chart += "color: '#303234',";
            chart += "align: 'center',";
            chart += "font: {";
            chart += "size: 18,";
            chart += "weight: 'bold',";
            chart += "},";
            chart += "formatter: (value) => {";
            chart += "if (value < 0) {";
            chart += "return value;";
            chart += "}}}}},";
            chart += "plugins: [ChartDataLabels]";
            chart += "});";
            chart += "</script>";
            Session["PrevColumn"] = null;
            
            return chart;
        }
        
        public string hexColor(string chart)
        {
            if(chart == "sampleChart")
            {
                chart = "'rgba(255, 26, 104, 0.2)','rgba(54, 162, 235, 0.2)','rgba(255, 206, 86, 0.2)'";
                chart += ":";
                chart += "'rgba(255, 26, 104, 1)','rgba(54, 162, 235, 1)','rgba(255, 206, 86, 1)'";
            }
            else if(chart == "participateChart2")
            {
                chart = "'rgba(255, 26, 104, 0.2)','rgba(54, 162, 235, 0.2)'";
                chart += ":";
                chart += "'rgba(255, 26, 104, 1)','rgba(54, 162, 235, 1)'";
            }
            else if (chart == "participateChart3")
            {
                chart = "'rgba(255, 26, 104, 0.2)','rgba(54, 162, 235, 0.2)'";
                chart += ":";
                chart += "'rgba(255, 26, 104, 1)','rgba(54, 162, 235, 1)'";
            }
            return chart;
        }

        public string getChartValues(string ColumnName, string query)
        {
            if(Session["PrevColumn"] == null)
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

        //public void makeChart(string chartID, string label, string data)
        //{
        //    string chart = "";
        //    chart = "<canvas id=\"" + chartID + "\"></canvas>";
        //    chart += "<script>";
        //    chart += "new Chart(document.getElementById('" + chartID + "'), {";
        //    chart += "type: 'pie',";
        //    chart += "data: {";
        //    chart += "labels: ['Student', 'Instructor', 'Dean'],";
        //    chart += "datasets: [{";
        //    chart += "label: '# of Users',";
        //    chart += "data: [5, 13, 1],";
        //    chart += "backgroundColor: ['rgba(255, 26, 104, 0.2)','rgba(54, 162, 235, 0.2)','rgba(255, 206, 86, 0.2)'],";
        //    chart += "borderColor: ['rgba(255, 26, 104, 1)','rgba(54, 162, 235, 1)','rgba(255, 206, 86, 1)'],";
        //    chart += "borderWidth: 1";
        //    chart += "}]";
        //    chart += "},";
        //    chart += "options: {";
        //    chart += "layout: {";
        //    chart += "padding: 15";
        //    chart += "},";
        //    chart += "plugins: {";
        //    chart += "tooltip: {";
        //    chart += "enabled: false";
        //    chart += "},";
        //    chart += "labels: {";
        //    chart += "position: 'border',";
        //    chart += "fontStyle: 'bold',";
        //    chart += "fontColor: '#303234',";
        //    chart += "fontSize: 25,";
        //    chart += "textMargin: 6,";
        //    chart += "render: (args) => {";
        //    chart += "if (args.percentage > 1) {";
        //    chart += "const display = args.percentage+'%' +'\\n'+ args.label.toString().replace(\" \",\"" + "\\n" + " \");";
        //    chart += "return display;";
        //    chart += "}";
        //    chart += "}";
        //    chart += "},";
        //    chart += "datalabels: {";
        //    chart += "color: '#303234',";
        //    chart += "align: 'center',";
        //    chart += "font: {";
        //    chart += "size: 18,";
        //    chart += "weight: 'bold',";
        //    chart += "},";
        //    chart += "formatter: (value) => {";
        //    chart += "if (value < 3) {";
        //    chart += "return value;";
        //    chart += "}}}}},";
        //    chart += "plugins: [ChartDataLabels]";
        //    chart += "});";
        //    chart += "</script>";
        //    Literal2.Text = chart;
        //}

        private void showPercentageUsers()
        {
            string chart = "";
                                //!important
            chart = "<canvas id=\"UserChart\"></canvas>";
            chart += "<script>";                    
            chart += "const data = {";
            chart += "labels: [" + getUserResults("role") + "],";
            chart += "datasets: [{";
            chart += "label: '# of Users',";
            chart += "data: [" + getUserResults("COUNT") + "],";
            //chart += "backgroundColor: [\"#7A0DFF\",\"#1275B3\",\"#FFD84D\"],";
            //chart += "borderColor: [\"#5A12B3\",\"#0DA3FF\",\"#FFCD19\"],";
            chart += "backgroundColor: ['rgba(255, 26, 104, 0.2)','rgba(54, 162, 235, 0.2)','rgba(255, 206, 86, 0.2)'],";
            chart += "borderColor: ['rgba(255, 26, 104, 1)','rgba(54, 162, 235, 1)','rgba(255, 206, 86, 1)'],";
            chart += "borderWidth: 1 }]};";
            chart += "const config = {";
            chart += "type: 'pie',"; //chart type
            chart += "data,";
            chart += "options: {";
            chart += "layout: {padding: 15},";
            chart += "plugins: {";
            chart += "tooltip: {enabled: false},";
            chart += "labels: {position: 'border',fontStyle: 'bold',fontSize: 25,textMargin: 6,";
            chart += "render: (args) => {if(args.percentage > 4){return `${args.label}: ${args.percentage}%`;}}},";
            chart += "datalabels: {";
            chart += "font: {size: 18, weight: 'bold'},";
            chart += "formatter: (value) => {";
            chart += "if(value < 3) {return value;}}}}},";
            chart += "plugins: [ChartDataLabels]};";
                                                    //!important
            chart += "new Chart(document.getElementById('UserChart'),config);";
            chart += "</script>";
            //ltUserPercentage.Text = chart;
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
                        cmd.CommandText = "SELECT COUNT(id) AS COUNT,UPPER(LEFT(role,1))+LOWER(SUBSTRING(role,2,LEN(role))) as role FROM STUDENT_TABLE WHERE school_id= '" + Session["school"].ToString() + "' GROUP BY role UNION ALL SELECT COUNT(id)AS COUNT,UPPER(LEFT(role,1))+LOWER(SUBSTRING(role,2,LEN(role))) as role FROM INSTRUCTOR_TABLE WHERE school_id= '" + Session["school"].ToString() + "' GROUP BY role";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                if (ColumnName == "COUNT")
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

        private void showStudPercentageParticipate()
        {
            string chart = "";
            chart = "<canvas id=\"ParticipateChart\"></canvas>";
            chart += "<script>";
            chart += "new Chart(document.getElementById('ParticipateChart'), {";
            chart += "type: 'pie',"; //chart type
            chart += "data: {";
            chart += "labels: ['Participated',['Not Participating']],";
            chart += "datasets: [{";
            chart += "label: '# of Users',";
            chart += "data: ["+ hvStudHasEvaluated.Value +","+ hvStudNotEvaluated.Value +"],";
            chart += "borderWidth: 1 }]},";
            chart += "options: {";
            chart += "layout: {padding: 15},";
            chart += "plugins: {";
            chart += "tooltip: {enabled: false},";
            chart += "labels: {position: 'border',fontStyle: 'bold',fontSize: 25,textMargin: 6,";
            chart += "render: (args) => {if(args.percentage > 4){const display = args.label.toString().replace(\" \",\"" + "\\n" +" \") +'\\n'+ args.percentage+'%'; return display;}}},";
            chart += "datalabels: {";
            chart += "font: {size: 18, weight: 'bold'},";
            chart += "formatter: (value) => {";
            chart += "if(value < 3) {return value;}}}}},";
            chart += "plugins: [ChartDataLabels]});";
            chart += "</script>";
            //ltParticipate.Text = chart;
        }

        private void showInsPercentageParticipate()
        {
            string chart = "";
            chart = "<canvas id=\"ParticipateChart\"></canvas>";
            chart += "<script>";
            chart += "new Chart(document.getElementById('ParticipateChart'), {";
            chart += "type: 'pie',"; //chart type
            chart += "data: {";
            chart += "labels: ['Participated',['Not Participating']],";
            chart += "datasets: [{";
            chart += "label: '# of Users',";
            chart += "data: [" + hvInsHasEvaluated.Value + "," + hvInsNotEvaluated.Value + "],";
            chart += "borderWidth: 1 }]},";
            chart += "options: {";
            chart += "layout: {padding: 15},";
            chart += "plugins: {";
            chart += "tooltip: {enabled: false},";
            chart += "labels: {position: 'border',fontStyle: 'bold',fontSize: 25,textMargin: 6,";
            chart += "render: (args) => {if(args.percentage > 4){const display = args.label.toString().replace(\" \",\"" + "\\n" + " \") +'\\n'+ args.percentage+'%'; return display;}}},";
            chart += "datalabels: {";
            chart += "font: {size: 18, weight: 'bold'},";
            chart += "formatter: (value) => {";
            chart += "if(value < 3) {return value;}}}}},";
            chart += "plugins: [ChartDataLabels]});";
            chart += "</script>";
            //ltParticipate.Text = chart;
        }

        private void showCategoryResult()
        {
            string chart = "";
            chart = "<canvas id=\"CategoryChart\"></canvas>";
            chart += "<script>";                        //and kani
            chart += "new Chart(document.getElementById('CategoryChart'), { type: 'bar', data: { ";
            chart += "labels: [" + getCategoryResults("constructor_name") + "],";
            chart += "datasets: [{";
            chart += "label: 'Average of ',";
            chart += "data: [" + getCategoryResults("AVERAGE") + "],";
            //chart += "data: data,";
            chart += "borderWidth: 1";
            chart += "}]},";
            chart += "options: { indexAxis: 'y' }";
            chart += "});";
            chart += "</script>";
            //ltChartCategory.Text = chart;
        }

        private void showQuestionResult()
        {
            string chart = "";
            chart = "<canvas id=\"QuestionChart\"></canvas>";
            chart += "<script>";
            chart += "new Chart(document.getElementById('QuestionChart'), { type: 'bar', data: { ";
            chart += "labels: [" + getQuestionResults("indicator_name") + "],";
            chart += "datasets: [{";
            chart += "label: 'Average of ',";
            chart += "data: [" + getQuestionResults("AVERAGE") + "],";
            //chart += "data: data,";
            chart += "borderWidth: 1";
            chart += "}]},";
            chart += "options: { indexAxis: 'y' }";
            chart += "});";
            chart += "</script>";
            //ltChartQuestion.Text = chart;
        }

        public string getQuestionResults(string ColumnName)
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
                        cmd.CommandText = "SELECT INDICATOR_TABLE.indicator_id,INDICATOR_TABLE.indicator_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE RIGHT JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id WHERE EVALUATION_TABLE.school_id= '" + Session["school"].ToString() + "' AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY INDICATOR_TABLE.indicator_id,INDICATOR_TABLE.indicator_name";
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

        public string getCategoryResults(string ColumnName)
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
                        cmd.CommandText = "SElECT CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE EVALUATION_TABLE.school_id= '" + Session["school"].ToString() + "' AND CONSTRUCTOR_TABLE.isDeleted IS NULL GROUP BY CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                if(ColumnName == "AVERAGE")
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

        public string getIDinfo(int id, string table, string pk, string infoNeed)
        {
            var value = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + " = '" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            value = reader[infoNeed].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return value;
        }
        protected void totalEvaluator()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {

                    using (var cmd = db.CreateCommand())
                    {
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM INSTRUCTOR_TABLE WHERE school_id=" + Session["school"].ToString();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            hvInstructor.Value = reader["COUNT"].ToString();
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM STUDENT_TABLE WHERE school_id=" + Session["school"].ToString();
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            hvStudent.Value = reader["COUNT"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            //lbltotal1.Text = " Total Number of Evaluators: <b> " + (Convert.ToInt32(hvInstructor.Value) + Convert.ToInt32(hvStudent.Value)+"</b>");
        }
        public void checkStudentEvaluators()
        {
            int count = 0;
            int evaluated = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT id,lname FROM STUDENT_TABLE WHERE school_id='" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                var id = reader["id"].ToString();
                                evaluated = checkifEvaluated(id, evaluated);
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
            hvStudNotEvaluated.Value = (Convert.ToInt32(hvStudent.Value) - evaluated).ToString();
            hvStudHasEvaluated.Value = evaluated.ToString();
            lblStudCount.Text = "Total Number of Students: <b> " + hvStudent.Value.ToString() + "</b>";
        }

        public void checkInstructorEvaluators()
        {
            int count = 0;
            int evaluated = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT id,lname FROM INSTRUCTOR_TABLE WHERE school_id='" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                var id = reader["id"].ToString();
                                evaluated = checkifEvaluated(id, evaluated);
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
            hvInsNotEvaluated.Value = (Convert.ToInt32(hvInstructor.Value) - evaluated).ToString();
            hvInsHasEvaluated.Value = evaluated.ToString();
            lblInsCount.Text = "Total Number of Instructors: <b> " + hvInstructor.Value.ToString() + "</b>";
        }

        public int checkifEvaluated(string id, int count)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {

                    using (var cmd = db.CreateCommand())
                    {
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT evaluator_id FROM EVALUATION_TABLE WHERE evaluator_id='" + id + "' AND school_id=" + Session["school"].ToString();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return count;
        }
    }
}