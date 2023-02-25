<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="EvaluationRecord.aspx.cs" Inherits="AMBER.Super_Admin.EvaluationRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-5">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-file-circle-question"></i>Evaluation Record</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <div style="width: 100% 1000px !important; max-width: 1000px !important;">
            <div class="row">
                <div class="col">
                    <div class="card shadow-lg">
                        <div class="card-body scrollbar-primary">
                            <div class="row mb-3">
                                <div class="col-8">
                                    <asp:DropDownList ID="DropDownSchool" runat="server" OnSelectedIndexChanged="DropDownSchool_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>
                            <asp:GridView runat="server" ID="GridView1" class="table table-striped table-bordered" AutoGenerateColumns="False" OnRowDeleting="GridView1_RowDeleting" DataKeyNames="eval_id">
                                <Columns>
                                    <asp:TemplateField HeaderText="#" InsertVisible="False" SortExpression="eval_id">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("eval_id") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("eval_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Evaluator ID" SortExpression="evaluator_id">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("evaluator_id") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("evaluator_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Evaluatee Id" SortExpression="evaluatee_id">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("evaluatee_id") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("evaluatee_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Question" SortExpression="indicator_name">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlQuestion" runat="server" CssClass="form-select" DataSourceID="SqlDataSource11" DataTextField="school_name" DataValueField="Id" AutoPostBack="true" SelectedValue='<%# Bind("school_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [indicator_id],[indicator_name] FROM [INDICATOR_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("indicator_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" SortExpression="rate">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("rate") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("rate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department" SortExpression="dept_code">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="deptDDL" runat="server" CssClass="form-select" DataSourceID="SqlDataSource2" DataTextField="dept_code" DataValueField="dept_id" AutoPostBack="true" SelectedValue='<%# Bind("dept_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [dept_id],[dept_code] FROM [DEPARTMENT_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label11" runat="server" Text='<%# Bind("dept_code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comment" SortExpression="comment">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("comment") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("comment") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date" SortExpression="datetime_eval">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("datetime_eval") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label9" runat="server" Text='<%# Bind("datetime_eval") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School" SortExpression="school_name">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlschool" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="school_name" DataValueField="Id" AutoPostBack="true" SelectedValue='<%# Bind("school_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [Id],[school_name] FROM [SCHOOL_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("school_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return DeleteConfirm(this);"><i class="fa-solid fa-trash-can"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ControlStyle CssClass="btn btn-danger" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[4, 'asc']],
                stateSave: true,
                "scrollX": true,
                fixedColumns: {
                    left: 1,
                    right: 1
                }
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#Table').DataTable();
        });
    </script>
</asp:Content>
