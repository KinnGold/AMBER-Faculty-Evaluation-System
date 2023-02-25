using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;

namespace AMBER.Pages.Users.Student
{
    public partial class EvaluateForm : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            var sch_id = Session["school"].ToString();
            if (Session["Id"] == null && Session["password"] == null || Session["role"] == null)
            {
                Response.Redirect("https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(sch_id) + "");
            }
            if (Session["term"] == null || CheckifWeightisFilled())
            {
                Response.Redirect("/Pages/Users/NoEvaluationAvail.aspx");
            }
            if (!IsPostBack)
            {
                populateDDL();
                GVbind();
                GVbind2();
            }
            IsEvaluated();
            getsemesterinfo();
        }
        bool CheckifWeightisFilled()
        {
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM CONSTRUCTOR_TABLE WHERE weight = 0 AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            chkr = true;
                        }
                        else
                        {
                            chkr = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AutoRedirect();
        }

        public void AutoRedirect()
        {
            int int_MilliSecondsTimeOut = this.SessionLengthMinutes * 60000;
            var sch_id = Session["school"].ToString();
            string str_Script = @"
               <script type='text/javascript'> 
                   intervalset = window.setInterval('Redirect()'," +
                       int_MilliSecondsTimeOut.ToString() + @");
                   function Redirect()
                   {
                       window.location.href='https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(sch_id) + @"';
                   }
               </script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", str_Script);
        }

        public int SessionLengthMinutes
        {
            get { return Session.Timeout; }
        }

        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY constructor_id)row_num,constructor_id,constructor_name,description FROM CONSTRUCTOR_TABLE WHERE school_id=@school_id AND isDeleted IS NULL AND role='student'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
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
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY constructor_id)row_num,constructor_id,constructor_name,description FROM CONSTRUCTOR_TABLE WHERE school_id=@school_id AND isDeleted IS NULL AND role='student'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView2.DataSource = dr;
                    GridView2.DataBind();
                }
            }
        }

        void populateDDL()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        //SELECT FOR REMOVE FROM DLL *NOT WORKING* :( SO KARUN INSTEAD OF REMOVE DISABLE NLNG
                        //SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name +' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE,EVALUATION_TABLE.rate FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id=SUBJECT_TABLE.subject_Id LEFT JOIN EVALUATION_TABLE ON EVALUATION_TABLE.evaluatee_id=SCHEDULE_TABLE.instructor_id WHERE SCHEDULE_TABLE.section_id='" + Session["section"].ToString() + "' AND SCHEDULE_TABLE.campus_id = '" + Session["campus"].ToString() + "' AND SCHEDULE_TABLE.isDeleted IS NULL AND EVALUATION_TABLE.rate IS NULL

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name +' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id=SUBJECT_TABLE.subject_Id WHERE SCHEDULE_TABLE.section_id='" + Session["section"].ToString() + "' AND SCHEDULE_TABLE.school_id = '" + Session["school"].ToString() + "' AND SCHEDULE_TABLE.isDeleted IS NULL ORDER BY SCHEDULE_TABLE.instructor_id";
                        ddlInstructor.DataValueField = "sched_id";
                        ddlInstructor.DataTextField = "SCHEDULE";
                        ddlInstructor.DataSource = cmd.ExecuteReader();
                        ddlInstructor.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            ddlInstructor.Items.Add(" ");
            ddlInstructor.Items.FindByText(" ").Selected = true;
        }

        public static string convertToRoman(int n)
        {
            string[] roman_symbol = { "MMM", "MM", "M", "CM", "DCCC", "DCC", "DC", "D", "CD", "CCC", "CC", "C", "XC", "LXXX", "LXX", "LX", "L", "XL", "XXX", "XX", "X", "IX", "VIII", "VII", "VI", "V", "IV", "III", "II", "I" };
            int[] int_value = { 3000, 2000, 1000, 900, 800, 700, 600, 500, 400, 300, 200, 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };

            var roman_numerals = new StringBuilder();
            var index_num = 0;
            while (n != 0)
            {
                if (n >= int_value[index_num])
                {
                    n -= int_value[index_num];
                    roman_numerals.Append(roman_symbol[index_num]);
                }
                else
                {
                    index_num++;
                }
            }

            return roman_numerals.ToString();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Label num = (Label)e.Row.FindControl("lblNumber");

            num.Text = "Part "+convertToRoman(Int32.Parse(num.Text))+".";

            GridView gvindicator = (GridView)e.Row.FindControl("GridviewIndicator");
            int category = Int32.Parse(GridView1.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY indicator_id)row_num,indicator_id,indicator_name FROM INDICATOR_TABLE WHERE school_id=@school_id AND isDeleted IS NULL AND constructor_id='" + category +"'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvindicator.DataSource = dr;
                    gvindicator.DataBind();
                }
            }
        }

        public int CategoryCount()
        {
            int count = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(constructor_id) AS COUNT FROM CONSTRUCTOR_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL AND role='student'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            count = Convert.ToInt32(reader["COUNT"]);
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
            return count;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int count = CategoryCount();
            if (IsRadioButtonListFilled())
            {
                if (chkConfirm.Checked == true)
                {
                    if (ddlInstructor.SelectedValue == " ")
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please select an instructor to evaluate', 'warning')", true);
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            GridViewRow MainGvRow = GridView1.Rows[i];
                            GridView ChildGV = MainGvRow.FindControl("GridviewIndicator") as GridView;
                            int index = 0;
                            foreach (GridViewRow row in ChildGV.Rows)
                            {
                                int indicator_id = Convert.ToInt32(ChildGV.DataKeys[index++].Value);
                                int instructor_id = getScheduleInstructor(ddlInstructor.SelectedValue);
                                RadioButtonList RBLrate = (row.Cells[0].FindControl("rblScore") as RadioButtonList);
                                var rate = RBLrate.SelectedValue;
                                try
                                {
                                    using (var db = new SqlConnection(connDB))
                                    {
                                        db.Open();
                                        using (var cmd = db.CreateCommand())
                                        {
                                            cmd.CommandType = CommandType.Text;
                                            if (txtComment.InnerText == String.Empty)
                                            {
                                                cmd.CommandText = "INSERT INTO EVALUATION_TABLE(evaluator_id, evaluatee_id, indicator_id, rate, datetime_eval, school_id)"
                                                + " VALUES("
                                                + "@evaluator,"
                                                + "@evaluatee,"
                                                + "@indicator,"
                                                + "@rate,"
                                                + "@datetime,"
                                                + "@school)";
                                                cmd.Parameters.AddWithValue("@evaluator", Session["studID"]);
                                                cmd.Parameters.AddWithValue("@evaluatee", instructor_id);
                                                cmd.Parameters.AddWithValue("@indicator", indicator_id);
                                                cmd.Parameters.AddWithValue("@rate", rate);
                                                cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                                                cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                            }
                                            else
                                            {
                                                cmd.CommandText = "INSERT INTO EVALUATION_TABLE(evaluator_id, evaluatee_id, indicator_id, rate, comment, datetime_eval, school_id)"
                                                + " VALUES("
                                                + "@evaluator,"
                                                + "@evaluatee,"
                                                + "@indicator,"
                                                + "@rate,"
                                                + "@comment,"
                                                + "@datetime,"
                                                + "@school)";
                                                cmd.Parameters.AddWithValue("@evaluator", Session["studID"]);
                                                cmd.Parameters.AddWithValue("@evaluatee", instructor_id);
                                                cmd.Parameters.AddWithValue("@indicator", indicator_id);
                                                cmd.Parameters.AddWithValue("@rate", rate);
                                                cmd.Parameters.AddWithValue("@comment", txtComment.InnerText);
                                                cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                                                cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                            }
                                            var ctr = cmd.ExecuteNonQuery();
                                            if (ctr >= 1)
                                            {
                                                txtComment.InnerText = String.Empty;
                                                chkConfirm.Checked = false;

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
                            btnSubmit.Enabled = false;
                            chkConfirm.Enabled = false;
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Success!','Thank you for participating', 'success')", true);
                        }
                        GridView1.Visible = false;
                        GridView2.Visible = true;
                        GVbind2();
                        txtComment.InnerText = getComments();
                        GVbind();
                    }
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please confirm before submitting your evaluation', 'warning')", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please Fill all rate', 'warning')", true);
            }
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

        public int getScheduleInstructor(string id)
        {
            int res = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_Id, INSTRUCTOR_TABLE.Id FROM SCHEDULE_TABLE JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id=INSTRUCTOR_TABLE.id WHERE sched_Id = '" + id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            res = Convert.ToInt32(reader["id"]);
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "notfound()", true);
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
            return res;
        }

        protected void ddlInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var FromDDLid = ddlInstructor.SelectedValue;
            if (FromDDLid == " ")
            {
                lblINS.Visible = false;
                lblcommentins.Visible = false;
                Image1.ImageUrl = "~/Pictures/usericon.png";
            }
            else
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBSTRING(INSTRUCTOR_TABLE.fname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname+' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE,(INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.Id WHERE SCHEDULE_TABLE.school_id = '" + Session["school"].ToString() + "' AND SCHEDULE_TABLE.sched_id='" + FromDDLid + "' AND SCHEDULE_TABLE.isDeleted IS NULL";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                lblINS.Visible = true;
                                lblINS.Text = reader["INSTRUCTOR"].ToString();
                                lblcommentins.Visible = true;
                                lblcommentins.Text = "for Prof. " + reader["INSTRUCTOR"].ToString();
                                string imageUrl = "data:image/png;base64," + image(Convert.ToInt32(getScheduleInstructor(FromDDLid)));
                                Image1.ImageUrl = imageUrl;
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
        }
        public bool IsRadioButtonListFilled()
        {
            int constructor = CategoryCount();
            int count = 0;
            bool chkr = true;
            var rate = "";
            for (int i = 0; i < constructor; i++)
            {
                GridViewRow MainGvRow = GridView1.Rows[i];
                GridView ChildGV = MainGvRow.FindControl("GridviewIndicator") as GridView;
                foreach (GridViewRow row in ChildGV.Rows)
                {
                    RadioButtonList RBLrate = (row.Cells[0].FindControl("rblScore") as RadioButtonList);
                    for (int j = 0; j < RBLrate.Items.Count; j++, count++)
                    {
                        if (RBLrate.Items[j].Selected)
                        {
                            rate = RBLrate.Items[j].Value;
                            count = 0;
                            break;
                        }
                    }
                    if (count == RBLrate.Items.Count)
                    {
                        chkr = false;
                        break;
                    }
                }
            }
            return chkr;
        }
        public void IsEvaluated()
        {
            if (ddlInstructor.SelectedValue == " ")
            {
                GridView1.Visible = true;
                GridView2.Visible = false;
            }
            else
            {
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT DISTINCT rate FROM EVALUATION_TABLE WHERE evaluatee_id='" + getScheduleInstructor(ddlInstructor.SelectedValue).ToString() + "' AND evaluator_id='" + Session["studID"] + "'";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.HasRows == true)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "toast()", true);
                                while (reader.Read())
                                {
                                    btnSubmit.Enabled = false;
                                    chkConfirm.Enabled = false;
                                    GridView1.Visible = false;
                                    GridView2.Visible = true;
                                    GVbind2();
                                    txtComment.InnerText = getComments();
                                }
                            }
                            else
                            {
                                btnSubmit.Enabled = true;
                                chkConfirm.Enabled = true;
                                GridView1.Visible = true;
                                GridView2.Visible = false;
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
            }
        }
        public string getComments()
        {
            string res = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT comment FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id = INDICATOR_TABLE.indicator_Id WHERE EVALUATION_TABLE.school_id=202251298 AND EVALUATION_TABLE.evaluator_id = '" + Session["studID"].ToString() + "' AND EVALUATION_TABLE.evaluatee_id = '" + getScheduleInstructor(ddlInstructor.SelectedValue) + "' AND INDICATOR_TABLE.isDeleted IS NULL AND comment IS NOT NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            res = reader["comment"].ToString();

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
            return res;
        }
        void getsemesterinfo()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE status='Active' AND school_id= '" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblHeader.Text = "Evaluation " + reader["description"].ToString() + " year " + reader["year"].ToString();
                        }
                        else
                        {
                            Session["term"] = null;
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
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Label num = (Label)e.Row.FindControl("lblNumber");

            num.Text = "Part " + convertToRoman(Int32.Parse(num.Text)) + ".";

            GridView gvindicator = (GridView)e.Row.FindControl("GridviewIndicator");
            int category = Int32.Parse(GridView1.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY INDICATOR_TABLE.indicator_id)row_num,INDICATOR_TABLE.indicator_id,INDICATOR_TABLE.indicator_name,EVALUATION_TABLE.rate FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id = INDICATOR_TABLE.indicator_Id WHERE EVALUATION_TABLE.school_id=@school_id AND EVALUATION_TABLE.evaluator_id = '" + Session["studID"].ToString() + "' AND EVALUATION_TABLE.evaluatee_id = '" + getScheduleInstructor(ddlInstructor.SelectedValue).ToString() + "' AND INDICATOR_TABLE.isDeleted IS NULL AND INDICATOR_TABLE.constructor_id='" + category + "'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvindicator.DataSource = dr;
                    gvindicator.DataBind();
                }
            }
        }
    }
}