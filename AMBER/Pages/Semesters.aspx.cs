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
    public partial class Semesters : System.Web.UI.Page
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
                int year = DateTime.Now.Year;
                string schoolyear = "";
                //string selectme = year.ToString() + "-" + (year + 1).ToString();
                for (int i = year; i <= year + 5; i++)
                {
                    schoolyear = i.ToString() + "-" + (i + 1).ToString();
                    ListItem li = new ListItem(schoolyear.ToString());
                    ddlschoolyear.Items.Add(li);
                }
                string selectme = "-school year-";
                ddlschoolyear.Items.Add(selectme);
                ddlschoolyear.Items.FindByText(selectme.ToString()).Selected = true;
                GVbind();
            }
            //chkEvalStatus();
        }
        //void chkEvalStatus()
        //{
        //    try
        //    {
        //        using (var db = new SqlConnection(connDB))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE GETDATE() BETWEEN evaluationStart AND evaluationEnd";
        //                SqlDataReader reader = cmd.ExecuteReader();
        //                if (reader.HasRows == true)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        UpdateStatus(reader["semester_id"].ToString(),"Active");
        //                    }
        //                }
        //                db.Close();
        //                db.Open();
        //                cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE GETDATE() NOT BETWEEN evaluationStart AND evaluationEnd";
        //                reader = cmd.ExecuteReader();
        //                if (reader.HasRows == true)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        UpdateStatus(reader["semester_id"].ToString(), "Inactive");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
        //        Response.Write("<pre>" + ex.ToString() + "</pre>");
        //    }
        //}

        //void UpdateStatus(string id,string status)
        //{
        //    try
        //    {
        //        using (var db = new SqlConnection(connDB))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "UPDATE SEMESTER_TABLE"
        //                    + " SET"
        //                    + " STATUS = @stat"
        //                    + " WHERE SEMESTER_ID = '" + id + "' AND CAMPUS_ID = '" + Session["school"].ToString() + "';";
        //                cmd.Parameters.AddWithValue("@stat", status);
        //                var ctr = cmd.ExecuteNonQuery();
        //                if (ctr >= 1)
        //                {
        //                    //GVbind();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
        //        Response.Write("<pre>" + ex.ToString() + "</pre>");
        //    }
        //}

        protected void btnAddsemester_Click(object sender, EventArgs e)
        {
            string ver = "v" + ddlschoolyear.SelectedValue[2] + ddlschoolyear.SelectedValue[3] + "-" + ddlsemester.SelectedValue[0];
            var sem_name = ddlsemester.SelectedValue;
            var year = ddlschoolyear.SelectedValue;
            var status = ddstatus.SelectedValue;
            var flag = 0;
            if (txtEvalStart.Text == "" && txtEvalEnd.Text == "")
            {
                flag = 1;
            }
            else if(txtEvalStart.Text != "" || txtEvalEnd.Text != "")
            {
                flag = 2;
            }
            if (CheckifSemesterExists())
            {
                Session["temp"] = "Semester";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
            }
            else if(flag != 1)
            {
                if(flag == 2)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong','Invalid Input', 'warning')", true);
                }
                else if (Convert.ToDateTime(txtEvalStart.Text) >= Convert.ToDateTime(txtEvalEnd.Text))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Insert Record!','Evaluation Start is greater than End', 'warning')", true);
                }
                else if (Convert.ToDateTime(txtEvalEnd.Text) <= Convert.ToDateTime(txtEvalStart.Text))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Insert Record!','Evaluation End is less than Start', 'warning')", true);
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
                                var insertcommand = "";
                                cmd.CommandType = CommandType.Text;
                                if (txtEvalEnd.Text != "" && txtEvalStart.Text != "")
                                {
                                    insertcommand = "INSERT INTO SEMESTER_TABLE(description, year, status, version, evaluationStart, evaluationEnd, school_id) VALUES(@name,@year,@status,@version,@start,@end,@school)";
                                }
                                else
                                {
                                    insertcommand = "INSERT INTO SEMESTER_TABLE(description, year, status, version, school_id) VALUES(@name,@year,@status,@version,@school)";
                                }
                                cmd.CommandText = insertcommand;
                                cmd.Parameters.AddWithValue("@name", sem_name);
                                cmd.Parameters.AddWithValue("@year", year);
                                cmd.Parameters.AddWithValue("@status", status);
                                cmd.Parameters.AddWithValue("@version", ver);
                                if (txtEvalEnd.Text != "" && txtEvalStart.Text != "")
                                {
                                    cmd.Parameters.AddWithValue("@start", Convert.ToDateTime(txtEvalStart.Text));
                                    cmd.Parameters.AddWithValue("@end", Convert.ToDateTime(txtEvalEnd.Text));
                                }
                                cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    //clear();
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
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
                }
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
                            var insertcommand = "";
                            cmd.CommandType = CommandType.Text;
                            if (txtEvalEnd.Text != "" && txtEvalStart.Text != "")
                            {
                                insertcommand = "INSERT INTO SEMESTER_TABLE(description, year, status, version, evaluationStart, evaluationEnd, school_id) VALUES(@name,@year,@status,@version,@start,@end,@school)";
                            }
                            else
                            {
                                insertcommand = "INSERT INTO SEMESTER_TABLE(description, year, status, version, school_id) VALUES(@name,@year,@status,@version,@school)";
                            }
                            cmd.CommandText = insertcommand;
                            cmd.Parameters.AddWithValue("@name", sem_name);
                            cmd.Parameters.AddWithValue("@year", year);
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@version", ver);
                            if (txtEvalEnd.Text != "" && txtEvalStart.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@start", Convert.ToDateTime(txtEvalStart.Text));
                                cmd.Parameters.AddWithValue("@end", Convert.ToDateTime(txtEvalEnd.Text));
                            }
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                //clear();
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "InsertSuccess()", true);
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
            }
            
        }
        public string getValue(string id,string n)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE semester_id='"+ id +"'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if(n=="start")
                            {
                                id = reader["EVALUATIONSTART"].ToString();
                            }
                            else
                            {
                                id = reader["EVALUATIONEND"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return id;
        }
        //public void clear()
        //{
        //    txtsemname.Text = String.Empty;
        //}
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT semester_id,description,year,status,version,evaluationStart AS actualStart,evaluationEnd AS actualEnd,REPLACE(FORMAT (evaluationStart, 'yyyy-MM-dd HH:mm:ss'),' ','T')  as evaluationStart,REPLACE(FORMAT (evaluationEnd, 'yyyy-MM-dd HH:mm:ss'),' ','T')  as evaluationEnd,school_id,isDeleted, SUBSTRING(year, 1, 4)+'-01-01T00:00' AS minyear, SUBSTRING(year, 6, 9)+'-12-28T00:00' AS maxyear FROM SEMESTER_TABLE WHERE SCHOOL_ID = @schoolid AND isDeleted IS NULL", db);
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

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = GridView1.DataKeys[e.RowIndex].Value.ToString();
            TextBox desc = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtDesc");
            TextBox year = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtYear");
            TextBox version = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtversion");
            DropDownList stat = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlStatus");
            TextBox start = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtEvalStart");
            TextBox end = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtEvalEnd");
            
            if(start.Text == "" || end.Text == "" || (start.Text == "" && end.Text == ""))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Update Record!','Invalid Time <b>empty</b> entered', 'warning')", true);
            }
            else if(Convert.ToDateTime(start.Text) >= Convert.ToDateTime(end.Text))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Insert Record!','Evaluation Start <b>"+ Convert.ToDateTime(start.Text) + "</b> is Greater than or Equal to schedule End <b>"+ Convert.ToDateTime(end.Text) + "</b>', 'warning')", true);
            }
            else if (cannotBeUpdated(id) == true)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Update Record!','Evaluation has already started and may affect evaluation results', 'warning')", true);
                GridView1.EditIndex = -1;
                GVbind();
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
                            cmd.CommandText = "UPDATE SEMESTER_TABLE SET DESCRIPTION = @desc, YEAR = @year, STATUS = @stat, EVALUATIONSTART = @start, EVALUATIONEND = @end WHERE SEMESTER_ID = '" + id + "' AND SCHOOL_ID = '" + Session["school"].ToString() + "';";
                            cmd.Parameters.AddWithValue("@desc", desc.Text);
                            cmd.Parameters.AddWithValue("@year", year.Text);
                            cmd.Parameters.AddWithValue("@stat", stat.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@start", Convert.ToDateTime(start.Text));
                            cmd.Parameters.AddWithValue("@end", Convert.ToDateTime(end.Text));
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "UpdateSuccess()", true);
                                GridView1.EditIndex = -1;
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
            }
        }

        bool cannotBeUpdated(string id)
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
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE semester_id = '"+ id +"'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if(reader["evaluationStart"].ToString() != "" && reader["evaluationEnd"].ToString() != "")
                            {
                                if (getEvaluateDate() != null)
                                {
                                    if (Convert.ToDateTime(reader["evaluationStart"]) <= /*DateTime.Now*/ Convert.ToDateTime(getEvaluateDate()) && Convert.ToDateTime(reader["evaluationEnd"]) >= /*DateTime.Now*/Convert.ToDateTime(getEvaluateDate()))
                                    {
                                        chkr = true;
                                    }
                                }
                            }
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

        public string getEvaluateDate()
        {
            string start = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT datetime_eval FROM EVALUATION_TABLE WHERE school_id= '"+ Session["school"].ToString() +"'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            start = reader["datetime_eval"].ToString();
                        }
                        else
                        {
                            start = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return start;
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVbind();
        }

        bool isSemesterReferenced(int id, string table, string pk)
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
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + "='" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }

        protected int countaffectrows(int id, string table, string pk)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(" + pk + ") AS COUNT FROM " + table + " WHERE " + pk + "='" + id + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = Int32.Parse(reader["COUNT"].ToString());
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return id;
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

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            string name = getIDinfo(id, "SEMESTER_TABLE", "semester_id", "description");
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        if (isSemesterReferenced(id, "SECTION_TABLE", "semester_id") || isSemesterReferenced(id, "CONSTRUCTOR_TABLE", "semester_id"))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','To avoid this error, you need to delete master records first on <b>SECTION</b> and <b>CATEGORY</b> records with <b>" + name + "</b> and after that you can delete this record. \"<b>Section</b> row/s: " + countaffectrows(id, "SECTION_TABLE", "semester_id") + " and <b>Category</b> row/s:" + countaffectrows(id, "CONSTRUCTOR_TABLE", "semester_id") + "\"', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE SEMESTER_TABLE SET isDeleted= @delete WHERE semester_Id ='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@delete", DateTime.Now);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','semester <b>" + name + "</b> is deleted successfully', 'success')", true);
                                GridView1.EditIndex = -1;
                                GVbind();
                            }
                            //UPDATE
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVbind();
        }
        bool CheckifSemesterExists()
        {
            string ver = "v" + ddlschoolyear.SelectedValue[2] + ddlschoolyear.SelectedValue[3] + "-" + ddlsemester.SelectedValue[0];
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE VERSION ='" + ver + "' AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }
    }
}