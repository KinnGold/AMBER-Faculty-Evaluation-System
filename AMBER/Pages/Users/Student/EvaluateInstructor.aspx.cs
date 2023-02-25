using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AMBER.Pages.Users.Student
{
    public partial class EvaluateInstructor : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null && Session["role"] == null)
            {
                Response.Redirect("/Pages/Users/UsersLogin.aspx");
            }
            if (!IsPostBack)
            {
                populateDDL();
                GVbind();
                
            }
            IsEvaluated();
            //GVbind();
            //disableEvaluatedlist();
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
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name +' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE FROM STUDY_LOAD JOIN SCHEDULE_TABLE ON STUDY_LOAD.sched_id = SCHEDULE_TABLE.sched_Id JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.id WHERE SCHEDULE_TABLE.campus_id = '" + Session["campus"].ToString() + "' AND SCHEDULE_TABLE.isDeleted IS NULL";
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

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT constructor_id, constructor_name FROM CONSTRUCTOR_TABLE WHERE CONSTRUCTOR_TABLE.campus_id='" + Session["campus"].ToString() + "' AND CONSTRUCTOR_TABLE.isDeleted IS NULL";
                        ddlConstructor.DataValueField = "constructor_id";
                        ddlConstructor.DataTextField = "constructor_name";
                        ddlConstructor.DataSource = cmd.ExecuteReader();
                        ddlConstructor.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
        }
        public void disableEvaluatedlist(string id)
        {
            foreach (ListItem li in ddlConstructor.Items)
            {
                int index = -1;
                if (li.Value == id)
                {
                    index++;
                    ddlConstructor.Items[index].Attributes["disabled"] = "disabled";
                }
            }
        }

        public void IsEvaluated()
        {
            if(ddlInstructor.SelectedValue== "-Schedule-")
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
                            cmd.CommandText = "SELECT DISTINCT CONSTRUCTOR_TABLE.constructor_id FROM CONSTRUCTOR_TABLE JOIN EVALUATION_TABLE ON CONSTRUCTOR_TABLE.constructor_id=EVALUATION_TABLE.constructor_id WHERE evaluatee_id='" + getScheduleInstructor(Convert.ToInt32(ddlInstructor.SelectedValue)).ToString() + "' AND EVALUATION_TABLE.constructor_id='" + ddlConstructor.SelectedValue + "' AND EVALUATION_TABLE.evaluator_id='" + Session["studID"] + "'";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.HasRows == true)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "toast()", true);
                                while (reader.Read())
                                {
                                    btnSubmit.Enabled = false;
                                    //disableEvaluatedlist(reader["constructor_id"].ToString());
                                }
                            }
                            else
                            {
                                btnSubmit.Enabled = true;
                            }
                            //if (reader.Read())
                            //{
                            //    id = reader["constructor_id"].ToString();
                            //}
                            //else
                            //{
                            //    id = "-1";
                            //}
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

        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT indicator_id, indicator_name FROM INDICATOR_TABLE WHERE constructor_id='"+ddlConstructor.SelectedValue+"' AND campus_id=@campus_id", db);
                cmd.Parameters.AddWithValue("@campus_id", Session["campus"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void ddlInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlInstructor.Enabled = false;
            var FromDDLid = ddlInstructor.SelectedValue;
            if (FromDDLid == "-Schedule-")
            {
                lblINS.Visible = false;
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
                            cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBSTRING(INSTRUCTOR_TABLE.fname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname+' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE,(INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.Id WHERE SCHEDULE_TABLE.campus_id = '" + Session["campus"].ToString() + "' AND SCHEDULE_TABLE.sched_id='" + FromDDLid + "' AND SCHEDULE_TABLE.isDeleted IS NULL";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                lblINS.Visible = true;
                                lblINS.Text = "Instructor: "+reader["INSTRUCTOR"].ToString();
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
            int count = 0;
            bool chkr = true;
            foreach (GridViewRow row in GridView1.Rows)
            {
                RadioButtonList RBLrate = (row.Cells[1].FindControl("rblScore") as RadioButtonList);
                for (int i = 0; i < RBLrate.Items.Count; i++,count++)
                {
                    if (RBLrate.Items[i].Selected)
                    {
                        var rate = RBLrate.Items[i].Value;
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
                        cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_Id, INSTRUCTOR_TABLE.Id FROM SCHEDULE_TABLE JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id=INSTRUCTOR_TABLE.id WHERE sched_Id = '"+ id +"'";
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var test = IsRadioButtonListFilled();
            if (IsRadioButtonListFilled())
            {
                int index = 0;
                foreach (GridViewRow row in GridView1.Rows)
                {
                    int instructor_id = 0;
                    int indicator_id = Convert.ToInt32(GridView1.DataKeys[index].Value);
                    int constructor_id = Convert.ToInt32(ddlConstructor.SelectedValue);
                    if (ddlInstructor.SelectedValue == "-Schedule-")
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please select an instructor to evaluate', 'warning')", true);
                    }
                    else
                    {
                        instructor_id = getScheduleInstructor(Convert.ToInt32(ddlInstructor.SelectedValue));
                        RadioButtonList RBLrate = (row.Cells[1].FindControl("rblScore") as RadioButtonList);
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
                                            cmd.CommandText = "INSERT INTO EVALUATION_TABLE(evaluator_id, evaluatee_id, constructor_id, indicator_id, rate, comment, datetime_eval, campus_id)"
                                                + " VALUES("
                                                + "@evaluator,"
                                                + "@evaluatee,"
                                                + "@constructor,"
                                                + "@indicator,"
                                                + "@rate,"
                                                + "@comment,"
                                                + "@datetime,"
                                                + "@campus)";
                                            cmd.Parameters.AddWithValue("@evaluator", Session["studID"]);
                                            cmd.Parameters.AddWithValue("@evaluatee", instructor_id);
                                            cmd.Parameters.AddWithValue("@constructor", constructor_id);
                                            cmd.Parameters.AddWithValue("@indicator", indicator_id);
                                            cmd.Parameters.AddWithValue("@rate", rate);
                                            cmd.Parameters.AddWithValue("@comment", "");
                                            cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@campus", Session["campus"].ToString());
                                            var ctr = cmd.ExecuteNonQuery();
                                            if (ctr >= 1)
                                            {
                                                GVbind();
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
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Success!','Thank you for participating', 'success')", true);
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please Fill all rate', 'warning')", true);
            }
        }

        protected void ddlConstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVbind();
        }
    }
}