using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AMBER.Pages.Users.Student
{
    public partial class Evaluate : System.Web.UI.Page
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
            GridViewHelper helper = new GridViewHelper(this.GridView1);
            helper.RegisterGroup("constructor_name", true, true);
            helper.GroupHeader += new GroupEvent(helper_GroupHeader);
            helper.ApplyGroupSort();
        }
        private void helper_GroupHeader(string groupName, object[] values, GridViewRow row)
        {
            if (groupName == "constructor_name")
            {
                
                row.BackColor = Color.Orange;
                row.Cells[0].Font.Bold=true;
                row.Cells[0].Font.Size = 15;
                //row.Cells[0].Font.Size=FontUnit.Medium;
                //row.Cells[0].Text = alp++ + row.Cells[0].Text;
                //row.ForeColor = Color.White;
            }
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY INDICATOR_TABLE.constructor_id)row_num,CONSTRUCTOR_TABLE.constructor_id,constructor_name,indicator_id, indicator_name FROM INDICATOR_TABLE JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE INDICATOR_TABLE.school_id=@school_id", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsRadioButtonListFilled())
            {
                int index = 0;
                foreach (GridViewRow row in GridView1.Rows)
                {
                    int instructor_id = 0;
                    int indicator_id = Convert.ToInt32(GridView1.DataKeys[index].Value);
                    int constructor_id = 0;//WALA PANI
                    if (ddlInstructor.SelectedValue == "-Schedule-")
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please select an instructor to evaluate', 'warning')", true);
                    }
                    else
                    {
                        instructor_id = getScheduleInstructor(Convert.ToInt32(ddlInstructor.SelectedValue));
                    }
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
                                        cmd.Parameters.AddWithValue("@comment", "");
                                        cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
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
                }
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Success!','Thank you for participating', 'success')", true);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please Fill all rate', 'warning')", true);
            }
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

        public bool IsRadioButtonListFilled()
        {
            int count = 0;
            bool chkr = true;
            var test = GridView1.Rows.Count;
            var test12 = GridView1.Columns.Count;
            //var indicator_id = GridView1.FindControl("rblScore").ID;
            foreach (GridViewRow row in GridView1.Rows)
            {
                RadioButtonList RBLrate = (row.Cells[0].FindControl("rblScore") as RadioButtonList);
                var test3 = RBLrate.Items.FindByValue("1");
                var test4 = RBLrate.SelectedIndex;
                var test5 = RBLrate.SelectedItem;

                for (int i = 0; i < RBLrate.Items.Count; i++, count++)
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

        protected void ddlInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                            cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBSTRING(INSTRUCTOR_TABLE.fname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname+' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE,(INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.Id WHERE SCHEDULE_TABLE.school_id = '" + Session["school"].ToString() + "' AND SCHEDULE_TABLE.sched_id='" + FromDDLid + "' AND SCHEDULE_TABLE.isDeleted IS NULL";
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                lblINS.Visible = true;
                                lblINS.Text = "Instructor: " + reader["INSTRUCTOR"].ToString();
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
                        cmd.CommandText = "SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name +' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE FROM STUDY_LOAD JOIN SCHEDULE_TABLE ON STUDY_LOAD.sched_id = SCHEDULE_TABLE.sched_Id JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.id WHERE SCHEDULE_TABLE.school_id = '" + Session["school"].ToString() + "' AND SCHEDULE_TABLE.isDeleted IS NULL";
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
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            GVbind();
        }
    }
}