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

namespace AMBER.Super_Admin.Reports
{
    public partial class GraphicalReports : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["id"] == null && Session["user"] == null && Session["pass"] == null)
            {
                Response.Redirect("/Super%20Admin/SuperAdminLogin.aspx");
            }
            if (!IsPostBack)
            {
                totalEvaluator();
                checkevaluators();
                getCategoryData();
                totalUsers();
                totalSubscribers();
                courseDept();
                subSection();

                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM SCHOOL_TABLE ORDER BY school_name ASC";
                            DropDownSchool.DataValueField = "Id";
                            DropDownSchool.DataTextField = "school_name";
                            DropDownSchool.DataSource = cmd.ExecuteReader();
                            DropDownSchool.DataBind();
                            DropDownSchool.Items.Insert(0, new ListItem("ALL", ""));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                DropDownSchool.Items.Add("Select School");
                DropDownSchool.Items.FindByText("Select School").Selected = true;
            }
        }

        private void getCategoryData()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    Series series = Chart1.Series["Series1"];
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SElECT CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id GROUP BY CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            plcHaveCategory.Visible = true;
                            plcNoCategory.Visible = false;
                            while (reader.Read())
                            {
                                series.Points.AddXY(reader["constructor_name"].ToString(), reader["AVERAGE"]);
                            }
                        }
                        else
                        {
                            plcHaveCategory.Visible = false; ;
                            plcNoCategory.Visible = true;
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

        protected void totalUsers()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {

                    using (var cmd = db.CreateCommand())
                    {
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM ADMIN_TABLE";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            adminUser.Value = reader["COUNT"].ToString();
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM INSTRUCTOR_TABLE";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            instructorUser.Value = reader["COUNT"].ToString();
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM STUDENT_TABLE";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            studentUser.Value = reader["COUNT"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            lbltotalusers.Text = " Total Number of Users: <b> "  + (Convert.ToInt32(adminUser.Value) + Convert.ToInt32(instructorUser.Value) + Convert.ToInt32(studentUser.Value) + "</b>");
        }

        protected void totalSubscribers()
        {
            string sub1 = "Premium";
            string sub2 = "Free";
            try
            {
                using (var db = new SqlConnection(connDB))
                {

                    using (var cmd = db.CreateCommand())
                    {
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(sub_id) AS COUNT FROM SUBSCRIPTION_TABLE WHERE SUBSCRIPTION_TYPE='"+ sub1.ToString() +"'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            subscribed.Value = reader["COUNT"].ToString();
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(sub_id) AS COUNT FROM SUBSCRIPTION_TABLE WHERE SUBSCRIPTION_TYPE='" + sub2.ToString() + "'";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            notSubscribed.Value = reader["COUNT"].ToString();
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            lblTotalSubscribers.Text = " Total Number of Schools Signed Up: <b> " + (Convert.ToInt32(subscribed.Value) + Convert.ToInt32(notSubscribed.Value) + "</b>");
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
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM INSTRUCTOR_TABLE";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            hvInstructor.Value = reader["COUNT"].ToString();
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM STUDENT_TABLE";
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
            lbltotal1.Text = " Total Number of Evaluators: <b> " + (Convert.ToInt32(hvInstructor.Value) + Convert.ToInt32(hvStudent.Value) + "</b>");
        }

        protected void courseDept()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {

                    using (var cmd = db.CreateCommand())
                    {
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(course_id) AS COUNT FROM COURSE_TABLE";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            course.Value = reader["COUNT"].ToString();
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(dept_id) AS COUNT FROM DEPARTMENT_TABLE";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            dept.Value = reader["COUNT"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            numberOfCourse.Text = " Total Number of Courses: <b> " + (Convert.ToInt32(course.Value) + "</b>");
            numberOfDepartment.Text = " Total Number of Departments: <b> " + (Convert.ToInt32(dept.Value) + "</b>");
        }


        protected void subSection()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {

                    using (var cmd = db.CreateCommand())
                    {
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(section_id) AS COUNT FROM SECTION_TABLE";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            section.Value = reader["COUNT"].ToString();
                        }
                        db.Close();
                        db.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(subject_id) AS COUNT FROM SUBJECT_TABLE";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            subject.Value = reader["COUNT"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            numberOfSubjects.Text = " Total Number of Subjects: <b> " + (Convert.ToInt32(subject.Value) + "</b>");
            numberOfSection.Text = " Total Number of Sections: <b> " + (Convert.ToInt32(section.Value) + "</b>");
        }

        public void checkevaluators()
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
                        cmd.CommandText = "SELECT id,lname FROM STUDENT_TABLE UNION SELECT id,lname FROM INSTRUCTOR_TABLE";
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
            hvNotEvaluated.Value = ((Convert.ToInt32(hvInstructor.Value) + Convert.ToInt32(hvStudent.Value)) - evaluated).ToString();
            hvHasEvaluated.Value = evaluated.ToString();
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
                        cmd.CommandText = "SELECT DISTINCT evaluator_id FROM EVALUATION_TABLE WHERE evaluator_id='" + id + "'";
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

        protected void DropDownSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sub1 = "Premium";
            string sub2 = "Free";

            if (DropDownSchool.SelectedItem.Text == "ALL")
            {
                totalEvaluator();
                checkevaluators();
                getCategoryData();
                totalUsers();
                totalSubscribers();
                courseDept();
                subSection();
            }
            else if (DropDownSchool.SelectedItem.Text == "Select School")
            {
                totalEvaluator();
                checkevaluators();
                getCategoryData();
                totalUsers();
                totalSubscribers();
                courseDept();
                subSection();
            }
            else
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        Series series = Chart1.Series["Series1"];
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SElECT CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id=INDICATOR_TABLE.indicator_Id JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE CONSTRUCTOR_TABLE.SCHOOL_ID='" + DropDownSchool.SelectedValue + "' GROUP BY CONSTRUCTOR_TABLE.constructor_id,CONSTRUCTOR_TABLE.constructor_name";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                plcHaveCategory.Visible = true;
                                plcNoCategory.Visible = false;
                                while (reader.Read())
                                {
                                    series.Points.AddXY(reader["constructor_name"].ToString(), reader["AVERAGE"]);
                                }
                            }
                            else
                            {
                                plcHaveCategory.Visible = false; ;
                                plcNoCategory.Visible = true;
                            }
                            db.Close();

                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM ADMIN_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                adminUser.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM INSTRUCTOR_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                instructorUser.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM STUDENT_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                studentUser.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(sub_id) AS COUNT FROM SUBSCRIPTION_TABLE WHERE SUBSCRIPTION_TYPE='" + sub1.ToString() + "' AND SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                subscribed.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(sub_id) AS COUNT FROM SUBSCRIPTION_TABLE WHERE SUBSCRIPTION_TYPE='" + sub2.ToString() + "' AND SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                notSubscribed.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM INSTRUCTOR_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                hvInstructor.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(Id) AS COUNT FROM STUDENT_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                hvStudent.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(course_id) AS COUNT FROM COURSE_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                course.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(dept_id) AS COUNT FROM DEPARTMENT_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                dept.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(section_id) AS COUNT FROM SECTION_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                section.Value = reader["COUNT"].ToString();
                            }
                            db.Close();
                            db.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT COUNT(subject_id) AS COUNT FROM SUBJECT_TABLE WHERE SCHOOL_ID='" + DropDownSchool.SelectedValue + "'";
                            reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                subject.Value = reader["COUNT"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                lbltotalusers.Text = " Total Number of Users: <b> " + (Convert.ToInt32(adminUser.Value) + Convert.ToInt32(instructorUser.Value) + Convert.ToInt32(studentUser.Value) + "</b>");
                lblTotalSubscribers.Text = " Total Number of Schools Signed Up: <b> " + (Convert.ToInt32(subscribed.Value) + Convert.ToInt32(notSubscribed.Value) + "</b>");
                numberOfSubjects.Text = " Total Number of Subjects: <b> " + (Convert.ToInt32(subject.Value) + "</b>");
                numberOfSection.Text = " Total Number of Sections: <b> " + (Convert.ToInt32(section.Value) + "</b>");
                numberOfCourse.Text = " Total Number of Courses: <b> " + (Convert.ToInt32(course.Value) + "</b>");
                numberOfDepartment.Text = " Total Number of Departments: <b> " + (Convert.ToInt32(dept.Value) + "</b>");
                lbltotal1.Text = " Total Number of Evaluators: <b> " + (Convert.ToInt32(hvInstructor.Value) + Convert.ToInt32(hvStudent.Value) + "</b>");

            }
        }
    }
}