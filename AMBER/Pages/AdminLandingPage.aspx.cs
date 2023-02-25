using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AMBER.BM
{
    public partial class AdminLandingPage : System.Web.UI.Page
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
                string[] query = { "SELECT * FROM (VALUES('Participated',(SELECT COUNT(DISTINCT evaluator_id) FROM EVALUATION_TABLE JOIN INSTRUCTOR_TABLE ON INSTRUCTOR_TABLE.Id = EVALUATION_TABLE.evaluator_id WHERE EVALUATION_TABLE.school_id = '" + Session["school"].ToString() + "')),('Not Participated',(SELECT COUNT(DISTINCT Id) FROM INSTRUCTOR_TABLE WHERE Id NOT IN (SELECT DISTINCT evaluator_id FROM EVALUATION_TABLE) AND school_id = '" + Session["school"].ToString() + "'))) participateTable(descrip,count)", "SELECT * FROM (VALUES('Participated',(SELECT COUNT(DISTINCT evaluator_id) FROM EVALUATION_TABLE JOIN STUDENT_TABLE ON STUDENT_TABLE.Id = EVALUATION_TABLE.evaluator_id WHERE EVALUATION_TABLE.school_id = '" + Session["school"].ToString() + "')),('Not Participated',(SELECT COUNT(DISTINCT Id) FROM STUDENT_TABLE WHERE Id NOT IN (SELECT DISTINCT evaluator_id FROM EVALUATION_TABLE) AND school_id = '" + Session["school"].ToString() + "'))) participateTable(descrip,count)" };
                lblinsBind();
                lblstudBind();
                GVbind();
                GVbind2();
                checkSubscription();
                getEvaluatedData();
                Literal1.Text = makeChart("insChart", getChartValues("descrip", query[0]), getChartValues("count", query[0]));
                Literal2.Text = makeChart("studChart", getChartValues("descrip", query[1]), getChartValues("count", query[1]));
            }
            lbladmin.Text = Session["fname"].ToString() + " " + Session["mname"].ToString()[0] + ". " + Session["lname"].ToString();
            getTERMstatus();
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
            if (chart != "")
            {
                chart = "'rgba(54, 162, 235, 0.2)','rgba(254, 99, 131, 0.2)','rgba(76, 192, 192, 0.2)','rgba(255, 159, 64, 0.2)','rgba(153, 102, 255, 0.2)','rgba(255, 204, 86, 0.2)','rgba(202, 203, 207, 0.2)'";
                chart += ":";
                chart += "'rgba(54, 162, 235, 1)','rgba(254, 99, 131, 1)','rgba(76, 192, 192, 1)','rgba(255, 159, 64, 1)','rgba(153, 102, 255, 1)','rgba(255, 204, 86, 1)','rgba(202, 203, 207, 1)'";
            }
            else if (chart == "studChart")
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

        void getEvaluatedData()
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
                        cmd.CommandText = "SELECT id,lname FROM STUDENT_TABLE WHERE school_id='" + Session["school"].ToString() + "' UNION SELECT id,lname FROM INSTRUCTOR_TABLE WHERE school_id='" + Session["school"].ToString() + "'";
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
            lbleval.Text = evaluated.ToString();
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
        void checkSubscription()
        {
            var school_id = Session["school"].ToString();
            var Id = Session["id"].ToString();
            string type1 = "Free";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SUBSCRIPTION_TABLE WHERE school_id='" + school_id + "' AND admin_id='" + Id + "' AND subscription_type='" + type1 + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "setSession", "sessionStorage.setItem('shown-modal', 'true');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        void lblinsBind()
        {
            var school_id = Session["school"].ToString();
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS totalRow FROM INSTRUCTOR_TABLE WHERE school_id='" + school_id + "'";
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        db.Close();
                        instlbl.Text = ds.Tables[0].Rows[0]["totalRow"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM FILES_TABLE WHERE file_name='SampleCSVFacultyMembers.csv'", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }
        protected void GVbind2()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM FILES_TABLE WHERE file_name='SampleCSVFStudents.csv'", db);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView2.DataSource = dr;
                    GridView2.DataBind();
                }
            }
        }
        void lblstudBind()
        {
            var school_id = Session["school"].ToString();
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS totalRow FROM STUDENT_TABLE WHERE school_id='" + school_id + "'";
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        db.Close();
                        stdlbl.Text = ds.Tables[0].Rows[0]["totalRow"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes = null;
            string fileName = "";
            string contentType = "";

            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM FILES_TABLE WHERE Id='" + id + "'", db);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    dr.Read();
                    fileName = dr["file_name"].ToString();
                    contentType = dr["content_type"].ToString();
                    bytes = (byte[])dr["data"];
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        protected void lnkDownload2_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes = null;
            string fileName = "";
            string contentType = "";

            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM FILES_TABLE WHERE Id='" + id + "'", db);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    dr.Read();
                    fileName = dr["file_name"].ToString();
                    contentType = dr["content_type"].ToString();
                    bytes = (byte[])dr["data"];
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        // function to get the full month name
        static string getFullName(int month)
        {
            DateTime date = new DateTime(2022, month, 12);

            return date.ToString("MMMM");
        }
        void getTERMstatus()
        {
            DateTime start = DateTime.Now.AddMonths(-1),end = DateTime.Now.AddMonths(-1);
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                string[] year = reader["year"].ToString().Split('-');
                                //compare date time then kuhaa nga semester
                                if (DateTime.Now.Year >= Int32.Parse(year[0]) && DateTime.Now.Year <= Int32.Parse(year[1]))
                                {
                                    var test1 = reader["evaluationStart"].ToString();
                                    if (reader["evaluationStart"].ToString() != "" && reader["evaluationEnd"].ToString() != "")
                                    {
                                        start = Convert.ToDateTime(reader["evaluationStart"]);
                                        end = Convert.ToDateTime(reader["evaluationEnd"]);
                                    }
                                    if(reader["status"].ToString() == "Active" && (DateTime.Now >= start && DateTime.Now <= end))
                                    {
                                        lblsem.Text = reader["description"].ToString() + " SY " + reader["year"].ToString();
                                        //lblstatus.Text = reader["evaluationStart"].ToString() + "-" + reader["evaluationEnd"].ToString();
                                        //IF NULL AYAW PAG SET OG TEXT PRA SA label period

                                        //lblstatus.Text = getFullName(Convert.ToDateTime(reader["evaluationStart"]).Month) + " " + Convert.ToDateTime(reader["evaluationStart"]).Day + ", " + Convert.ToDateTime(reader["evaluationStart"]).Year + " " + Convert.ToDateTime(reader["evaluationStart"]).ToString("hh:mm tt") + " to " + getFullName(Convert.ToDateTime(reader["evaluationStart"]).Month) + " " + Convert.ToDateTime(reader["evaluationEnd"]).Day + ", " + Convert.ToDateTime(reader["evaluationStart"]).Year + " " + Convert.ToDateTime(reader["evaluationStart"]).ToString("hh:mm tt");
                                        lblstatus.Text = start.ToString("dddd, MMMM dd  yyyy hh:mm tt") + " to " + end.ToString("dddd, MMMM dd  yyyy hh:mm tt");
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                        //if (reader.Read())
                        //{
                            
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }

        protected void btnSubscribeNow_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/SubscriptionDetails.aspx");
        }
    }
}