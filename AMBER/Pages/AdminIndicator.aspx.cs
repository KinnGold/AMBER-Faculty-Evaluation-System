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
    public partial class AdminIndicator : System.Web.UI.Page
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
                try
                {
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT DISTINCT CONSTRUCTOR_TABLE.constructor_id, (''+CONSTRUCTOR_TABLE.constructor_name+' ('+CONSTRUCTOR_TABLE.role+') ') AS NAME FROM CONSTRUCTOR_TABLE LEFT JOIN INDICATOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id=INDICATOR_TABLE.constructor_id WHERE CONSTRUCTOR_TABLE.SCHOOL_ID = '" + Session["school"].ToString() + "'";
                            DropDownList1.DataValueField = "constructor_id";
                            DropDownList1.DataTextField = "NAME";
                            DropDownList1.DataSource = cmd.ExecuteReader();
                            DropDownList1.DataBind();
                        }

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                    Response.Write("<pre>" + ex.ToString() + "</pre>");
                }
                GVbind();
            }
            
        }

        public bool checkifEvaluationStarted()
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
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE GETDATE() > evaluationStart AND school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL";
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

        protected void addIndicator_Click(object sender, EventArgs e)
        {
            var indname = indicator1.Text;
            var consId = DropDownList1.SelectedValue;

            if (CheckifIndicatorExists(indname, consId))
            {
                clear();
                Session["temp"] = "Question";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
            }
            else if(checkifEvaluationStarted())
            {
                clear();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Insert Record!','Evaluation Already Started', 'warning')", true);
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
                            cmd.CommandText = "INSERT INTO INDICATOR_TABLE(indicator_name, constructor_id, school_id)"
                                + " VALUES("
                                + "@indname,"
                                + "@consID,"
                                + "@school)";
                            cmd.Parameters.AddWithValue("@indname", indname);
                            cmd.Parameters.AddWithValue("@consID", consId);
                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                indicator1.Text = String.Empty;
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
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT INDICATOR_TABLE.indicator_Id, INDICATOR_TABLE.indicator_name, INDICATOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_id, CONSTRUCTOR_TABLE.constructor_name, CONSTRUCTOR_TABLE.role FROM CONSTRUCTOR_TABLE JOIN INDICATOR_TABLE ON CONSTRUCTOR_TABLE.constructor_id = INDICATOR_TABLE.constructor_id WHERE CONSTRUCTOR_TABLE.school_id = @schoolid AND CONSTRUCTOR_TABLE.isDeleted IS NULL AND INDICATOR_TABLE.isDeleted IS NULL", db);
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

        void clear()
        {
            indicator1.Text = String.Empty;
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            TextBox name = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtName");
            //TextBox score = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtScore");
            DropDownList consid = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlCategory");

            if (cannotBeUpdated(id) == true)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Update Record!','This Indicator Has Been Added to The Evaluation', 'warning')", true);
                GridView1.EditIndex = -1;
                GVbind();
            }
            else
            {
                if (CheckifIndicatorExists(name.Text, consid.SelectedValue))
                {
                    clear();
                    Session["temp"] = "Question";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "exist()", true);
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
                                //+ " INDICATOR_SCORE = @score,"
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE INDICATOR_TABLE"
                                    + " SET"
                                    + " INDICATOR_NAME = @name,"
                                    + " CONSTRUCTOR_ID = @consid"
                                    + " WHERE INDICATOR_ID = '" + id + "';";
                                cmd.Parameters.AddWithValue("@name", name.Text);
                                //cmd.Parameters.AddWithValue("@score", score.Text);
                                cmd.Parameters.AddWithValue("@consid", consid.Text);
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
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GVbind();
        }

        bool isIndicatorReferenced(int id, string table, string pk)
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
                        cmd.CommandText = "SELECT * FROM " + table + " WHERE " + pk + "='" + id + "' AND school_id='" + Session["school"].ToString() + "'";
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
            string name = getIDinfo(id, "CONSTRUCTOR_TABLE", "constructor_id", "constructor_name");
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        if (isIndicatorReferenced(id, "EVALUATION_TABLE", "indicator_id"))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Cannot Delete Record!','this record has been used on <b>EVALUATION</b>.', 'warning')", true);
                            //PROMPT
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE INDICATOR_TABLE SET isDeleted= @delete WHERE indicator_Id ='" + id + "' AND SCHOOL_ID='" + Session["school"].ToString() + "'";
                            cmd.Parameters.AddWithValue("@delete", DateTime.Now);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Deleted!','question <b>" + name + "</b> is deleted successfully', 'success')", true);
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

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GVbind();
        }

        bool CheckifIndicatorExists(string name, string category)
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
                        cmd.CommandText = "SELECT * FROM INDICATOR_TABLE JOIN CONSTRUCTOR_TABLE ON INDICATOR_TABLE.constructor_id=CONSTRUCTOR_TABLE.constructor_id WHERE CONSTRUCTOR_TABLE.role='" + getThisIndicatorGroup(category) + "' AND INDICATOR_TABLE.indicator_name='" + name + "' AND INDICATOR_TABLE.constructor_id='" + category + "' AND INDICATOR_TABLE.school_id='" + Session["school"].ToString() + "'";

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

        public string getThisIndicatorGroup(string id)
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM CONSTRUCTOR_TABLE WHERE constructor_id='" + id + "' AND school_id='" + Session["school"].ToString() + "'";

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = reader["role"].ToString();
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
        bool cannotBeUpdated(int id)
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
                        cmd.CommandText = "SELECT * FROM INDICATOR_TABLE JOIN EVALUATION_TABLE ON EVALUATION_TABLE.indicator_id = INDICATOR_TABLE.indicator_Id WHERE INDICATOR_TABLE.school_id='" + Session["school"].ToString() + "' AND INDICATOR_TABLE.indicator_id='" + id + "' AND INDICATOR_TABLE.isDeleted IS NULL";
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
    }
}