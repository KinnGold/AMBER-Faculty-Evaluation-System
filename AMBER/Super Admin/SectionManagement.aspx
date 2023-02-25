<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="SectionManagement.aspx.cs" Inherits="AMBER.Super_Admin.SectionManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-5">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('/Pictures/former.png'); background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-users-rectangle"></i>
                                Section Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="card shadow-lg">
                    <div class="card-body scrollbar-primary">
                        <div class="row mb-3">
                            <div class="col-8">
                                <asp:DropDownList ID="DropDownSchool" runat="server" OnSelectedIndexChanged="DropDownSchool_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select"></asp:DropDownList>
                            </div>
                        </div>
                        <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="section_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                            <Columns>
                                <%--<asp:BoundField DataField="section_id" HeaderText="Section ID" ReadOnly="True" SortExpression="Id" InsertVisible="False" />--%>
                                <asp:TemplateField HeaderText="#">
                                    <EditItemTemplate>
                                        <asp:Label ID="LabelPlease3" runat="server" Text='<%# Bind("section_id") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="LabelPlease2" runat="server" Text='<%# Bind("section_id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Section">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNAME" CssClass="form-control" runat="server" Text='<%# Bind("section_name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("section_name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Course">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlCOURSE" runat="server" CssClass="form-select" DataSourceID="SqlDataSource4" DataTextField="COURSE" DataValueField="course_id" SelectedValue='<%# Bind("course_id") %>'>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT course_id,(title+' - '+description) AS COURSE FROM COURSE_TABLE"></asp:SqlDataSource>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("COURSE") %>' ID="Label3"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Program">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlprogram" runat="server" CssClass="form-select" SelectedValue='<%# Bind("program") %>'>
                                            <asp:ListItem Value="1">DAY</asp:ListItem>
                                            <asp:ListItem Value="0">EVENING</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("programdesc") %>' ID="Label3411"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Year Level">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlyear" runat="server" CssClass="form-select" SelectedValue='<%# Bind("yearLevel") %>'>
                                            <asp:ListItem Value="1">1st year</asp:ListItem>
                                            <asp:ListItem Value="2">2nd year</asp:ListItem>
                                            <asp:ListItem Value="3">3rd year</asp:ListItem>
                                            <asp:ListItem Value="4">4th year</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("yearLeveldesc") %>' ID="Label36411"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Semester"><%--asd--%>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlSEMESTER" runat="server" CssClass="form-select" DataSourceID="SqlDataSource3" DataTextField="SEMESTER" DataValueField="semester_id" SelectedValue='<%# Bind("semester_id") %>'>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT semester_id, (description+' SY '+year) AS SEMESTER FROM SEMESTER_TABLE"></asp:SqlDataSource>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("SEMESTER") %>' ID="Label11"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="School">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlschool" runat="server" CssClass="form-select" DataSourceID="SqlDataSource5" DataTextField="school_name" DataValueField="Id" SelectedValue='<%# Bind("sch_id") %>'>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [Id], [school_name] FROM SCHOOL_TABLE"></asp:SqlDataSource>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("school_name") %>' ID="LabelPLease1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="isDeleted">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDel" CssClass="form-control" runat="server" Text='<%# Bind("isDeleted") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="LabelPlease" runat="server" Text='<%# Bind("isDeleted") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="11%" ItemStyle-CssClass="text-center">
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="True" ID="btnUpdate"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>&nbsp;<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="btn btn-success" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return DeleteConfirm(this);"><i class="fa-solid fa-trash-can"></i></asp:LinkButton>
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
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[4, 'asc']],
                stateSave: true,
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#Table').DataTable();
        });
    </script>
</asp:Content>
