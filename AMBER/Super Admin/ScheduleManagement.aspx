<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="ScheduleManagement.aspx.cs" Inherits="AMBER.Super_Admin.ScheduleManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        #txtDay td {
            position: relative;
            text-align: center !important;
        }

        #txtDay label {
            position: absolute;
            top: 0px;
            left: 5px;
        }

        #txtDay input {
            position: absolute;
            top: 10px;
            left: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-5">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-calendar-days"></i>
                                Schedule Management</h1>
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
                            <asp:GridView ID="GridView1" class="table table-striped table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="sched_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="Schedule ID" InsertVisible="False" SortExpression="sched_id">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("sched_id") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("sched_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Schedule Code" SortExpression="sched_code">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtschedcode" CssClass="form-control" runat="server" Text='<%# Bind("sched_code") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("sched_code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject" SortExpression="subject_id">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-select" DataSourceID="SqlDataSource9" DataTextField="SUBJECT" DataValueField="subject_id" SelectedValue='<%# Bind("subject_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [subject_id],('('+SUBJECT_TABLE.subject_code+') '+ SUBJECT_TABLE.subject_name) AS SUBJECT FROM [SUBJECT_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("SUBJECT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instructor" SortExpression="INSTRUCTOR">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlInstructor" runat="server" CssClass="form-select" DataSourceID="SqlDataSource8" DataTextField="INSTRUCTOR" DataValueField="Id" SelectedValue='<%# Bind("instructor_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [Id], (INSTRUCTOR_TABLE.fname +' '+ SUBSTRING(INSTRUCTOR_TABLE.mname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname) AS INSTRUCTOR FROM [INSTRUCTOR_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("INSTRUCTOR") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Time" SortExpression="time">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txttime" CssClass="form-control" runat="server" Text='<%# Bind("time") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("time") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day" SortExpression="day">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlDAY" CssClass="form-select" runat="server" SelectedValue='<%# Bind("day") %>'>
                                                <asp:ListItem Value="MON">Monday</asp:ListItem>
                                                <asp:ListItem Value="TUE">Tuesday</asp:ListItem>
                                                <asp:ListItem Value="WED">Wednesday</asp:ListItem>
                                                <asp:ListItem Value="THU">Thursday</asp:ListItem>
                                                <asp:ListItem Value="FRI">Friday</asp:ListItem>
                                                <asp:ListItem Value="SAT">Saturday</asp:ListItem>
                                                <asp:ListItem Value="SUN">Sunday</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("day") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Section" SortExpression="section_id">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlSection" runat="server" CssClass="form-select" DataSourceID="SqlDataSource7" DataTextField="section_name" DataValueField="section_id" SelectedValue='<%# Bind("section_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [section_id],[section_name] FROM [SECTION_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label8" runat="server" Text='<%# Bind("section_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School" SortExpression="school_name">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlschool" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="school_name" DataValueField="Id" AutoPostBack="true" SelectedValue='<%# Bind("sc_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [Id],[school_name] FROM [SCHOOL_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label9" runat="server" Text='<%# Bind("school_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="isDeleted" SortExpression="isDeleted">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox5" CssClass="form-control" ReadOnly="true" runat="server" Text='<%# Bind("isDeleted") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label10" runat="server" Text='<%# Bind("isDeleted") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="Updatebtn" runat="server" CausesValidation="True" CommandName="Update" Text="Update"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="Cancelbtn" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ControlStyle CssClass="btn btn-success"></ControlStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ControlStyle CssClass="btn btn-success"></ControlStyle>
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
                    right: 2
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
