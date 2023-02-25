using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace AMBER.Pages.Users.Student
{
    public partial class Evaluateins : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["role"] == null)
            {
                Response.Redirect("/Pages/Users/UsersLogin.aspx");
            }
            if (Session["term"] == null)
            {
                Response.Redirect("/Pages/Users/NoEvaluationAvail.aspx");
            }
            if (!IsPostBack)
            {
                populateDDL();
                GVbind();
                IsEvaluated();
            }
            
            //temporaryRates();
            getsemesterinfo();
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY INDICATOR_TABLE.constructor_id)row_num,indicator_id, indicator_name, CONSTRUCTOR_TABLE.constructor_name FROM INDICATOR_TABLE JOiN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE INDICATOR_TABLE.school_id=@school_id AND CONSTRUCTOR_TABLE.role='student'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        //void temporaryRates()
        //{
        //    ArrayList rates = (ArrayList)Session["rate"];
        //    if(rates != null)
        //    {
        //        for (int i = 0; i < rates.Count; i++)
        //        {
        //            Label3.Text += rates[i];
        //        }
        //    }
        //}
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
                        cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name +' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id=SUBJECT_TABLE.subject_Id WHERE SCHEDULE_TABLE.section_id='" + Session["section"].ToString() + "' AND SCHEDULE_TABLE.school_id = '" + Session["school"].ToString() + "' AND SCHEDULE_TABLE.isDeleted IS NULL";
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
            ddlInstructor.Items.Add("-Schedule-");
            ddlInstructor.Items.FindByText("-Schedule-").Selected = true;
            //ddlInstructor.Items.Insert(0, new ListItem("Select an Instructor..."));
            //ddlInstructor.Items[0].Selected = true;
            //ddlInstructor.Items[0].Attributes["disabled"] = "disabled";
            for(int i=0;i<ddlInstructor.Items.Count;i++)
            {

            }
        }

        public bool checkifnaabasaDDL()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT CONSTRUCTOR_TABLE.constructor_id FROM CONSTRUCTOR_TABLE JOIN EVALUATION_TABLE ON CONSTRUCTOR_TABLE.constructor_id=EVALUATION_TABLE.constructor_id WHERE evaluatee_id='" + getScheduleInstructor(Convert.ToInt32(ddlInstructor.SelectedValue)).ToString() + "' AND EVALUATION_TABLE.evaluator_id='" + Session["studID"] + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "toast()", true);
                            while (reader.Read())
                            {
                                btnSubmit.Enabled = false;
                                chkConfirm.Enabled = false;
                            }
                        }
                        else
                        {
                            btnSubmit.Enabled = true;
                            chkConfirm.Enabled = true;
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
            return true;
        }

        protected void ddlInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var FromDDLid = ddlInstructor.SelectedValue;
            if (FromDDLid == "-Schedule-")
            {
                lblINS.Visible = false;
                lblcommentins.Visible = false;
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
                                lblINS.Text = "Instructor: " + reader["INSTRUCTOR"].ToString();
                                lblcommentins.Visible = true;
                                lblcommentins.Text = "for Prof. " + reader["INSTRUCTOR"].ToString();
                                string imageUrl = "data:image/png;base64," + image(Convert.ToInt32(getScheduleInstructor(Convert.ToInt32(FromDDLid))));
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
        public bool IsRadioButtonListFilled()
        {
            ArrayList studRATEs = new ArrayList();
            int count = 0;
            bool chkr = true;
            var rate = "";
            foreach (GridViewRow row in GridView1.Rows)
            {
                RadioButtonList RBLrate = (row.Cells[0].FindControl("rblScore") as RadioButtonList);
                for (int i = 0; i < RBLrate.Items.Count; i++, count++)
                {
                    if (RBLrate.Items[i].Selected)
                    {
                        rate = RBLrate.Items[i].Value;
                        count = 0;
                        break;
                    }
                }
                studRATEs.Add(rate);
                if (count == RBLrate.Items.Count)
                {
                    studRATEs.Clear();
                    chkr = false;
                    break;
                }
            }
            Session["rate"] = studRATEs;
            return chkr;
        }
        public int getScheduleInstructor(int id)
        {
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
                            id = Convert.ToInt32(reader["id"]);
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
            return id;
        }
        public int getConstructorID(int id)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM INDICATOR_TABLE WHERE indicator_Id = '" + id + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = Convert.ToInt32(reader["constructor_id"]);
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
            return id;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if(IsRadioButtonListFilled())
            {
                if (chkConfirm.Checked == true)
                {
                    int index = 0;
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        int instructor_id = 0;
                        int indicator_id = Convert.ToInt32(GridView1.DataKeys[index].Value);
                        int constructor_id = getConstructorID(indicator_id);
                        if (ddlInstructor.SelectedValue == "-Schedule-")
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please select an instructor to evaluate', 'warning')", true);
                        }
                        else
                        {
                            instructor_id = getScheduleInstructor(Convert.ToInt32(ddlInstructor.SelectedValue));
                            RadioButtonList RBLrate = (row.Cells[0].FindControl("rblScore") as RadioButtonList);
                            for (int i = 0; i < RBLrate.Items.Count; i++)
                            {
                                if (RBLrate.Items[i].Selected)
                                {
                                    var rate = RBLrate.Items[i].Value;
                                    try
                                    {
                                        using (var db = new SqlConnection(connDB))
                                        {
                                            db.Open();
                                            using (var cmd = db.CreateCommand())
                                            {
                                                cmd.CommandType = CommandType.Text;
                                                if(txtComment.InnerText==String.Empty)
                                                {
                                                    cmd.CommandText = "INSERT INTO EVALUATION_TABLE(evaluator_id, evaluatee_id, constructor_id, indicator_id, rate, datetime_eval, school_id)"
                                                    + " VALUES("
                                                    + "@evaluator,"
                                                    + "@evaluatee,"
                                                    + "@constructor,"
                                                    + "@indicator,"
                                                    + "@rate,"
                                                    + "@datetime,"
                                                    + "@school)";
                                                    cmd.Parameters.AddWithValue("@evaluator", Session["studID"]);
                                                    cmd.Parameters.AddWithValue("@evaluatee", instructor_id);
                                                    cmd.Parameters.AddWithValue("@constructor", constructor_id);
                                                    cmd.Parameters.AddWithValue("@indicator", indicator_id);
                                                    cmd.Parameters.AddWithValue("@rate", rate);
                                                    cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                                                    cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                                }
                                                else
                                                {
                                                    cmd.CommandText = "INSERT INTO EVALUATION_TABLE(evaluator_id, evaluatee_id, constructor_id, indicator_id, rate, comment, datetime_eval, school_id)"
                                                    + " VALUES("
                                                    + "@evaluator,"
                                                    + "@evaluatee,"
                                                    + "@constructor,"
                                                    + "@indicator,"
                                                    + "@rate,"
                                                    + "@comment,"
                                                    + "@datetime,"
                                                    + "@school)";
                                                    cmd.Parameters.AddWithValue("@evaluator", Session["studID"]);
                                                    cmd.Parameters.AddWithValue("@evaluatee", instructor_id);
                                                    cmd.Parameters.AddWithValue("@constructor", constructor_id);
                                                    cmd.Parameters.AddWithValue("@indicator", indicator_id);
                                                    cmd.Parameters.AddWithValue("@rate", rate);
                                                    cmd.Parameters.AddWithValue("@comment", txtComment.InnerText);
                                                    cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                                                    cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                                }
                                                
                                                var ctr = cmd.ExecuteNonQuery();
                                                if (ctr >= 1)
                                                {
                                                    GVbind();
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
                                    break;
                                }
                            }
                            index++;
                            btnSubmit.Enabled = false;
                            chkConfirm.Enabled = false;
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Success!','Thank you for participating', 'success')", true);
                        }
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
        public void IsEvaluated()
        {
            for(int i=0;i<ddlInstructor.Items.Count;i++)
            {
                if (ddlInstructor.Items[i].Value == "-Schedule-")
                {

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
                                cmd.CommandText = "SELECT DISTINCT CONSTRUCTOR_TABLE.constructor_id FROM CONSTRUCTOR_TABLE JOIN EVALUATION_TABLE ON CONSTRUCTOR_TABLE.constructor_id=EVALUATION_TABLE.constructor_id WHERE evaluatee_id='" + getScheduleInstructor(Convert.ToInt32(ddlInstructor.Items[i].Value)).ToString() + "' AND EVALUATION_TABLE.evaluator_id='" + Session["studID"] + "'";
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.HasRows == true)
                                {
                                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "toast()", true);
                                    while (reader.Read())
                                    {
                                        btnSubmit.Enabled = false;
                                        chkConfirm.Enabled = false;
                                        ddlInstructor.Items[i].Attributes["disabled"] = "disabled";
                                    }
                                }
                                else
                                {
                                    btnSubmit.Enabled = true;
                                    chkConfirm.Enabled = true;
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
                            lblHeader.Text = "Evaluation " + reader["description"].ToString() +" year " + reader["year"].ToString();

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

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            for (int i = GridView1.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = GridView1.Rows[i];
                GridViewRow previousRow = GridView1.Rows[i - 1];
                for (int j = 0; j < 1; j++)
                {
                    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
    }
}