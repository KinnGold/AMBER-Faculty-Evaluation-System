<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="CategoryManagement.aspx.cs" Inherits="AMBER.Super_Admin.CategoryManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-file-circle-question"></i>Category Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div style="width: 100% 1000px !important; max-width: 1000px !important;">
            <div class="card shadow-lg">
                <div class="card-body scrollbar-primary">
                    <div class="row">
                        <div class="col">
                            <div class="row mb-3">
                                <div class="col-8">
                                    <asp:DropDownList ID="DropDownSchool" runat="server" OnSelectedIndexChanged="DropDownSchool_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>
                            <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="constructor_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="#" SortExpression="constructor_id">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label123" runat="server" Text='<%# Bind("constructor_id") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label12" runat="server" Text='<%# Bind("constructor_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name" SortExpression="constructor_name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" CssClass="form-control" runat="server" Text='<%# Bind("constructor_name") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label12345" runat="server" Text='<%# Bind("constructor_name") %>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="Category Weight" SortExpression="weight">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtweight" TextMode="Number" min="1" MaxLength="3" max="100" oninput="this.value = Math.abs(this.value)" CssClass="form-control" runat="server" Text='<%# Bind("weight") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1222" runat="server" Text='<%# Bind("weight") %>'></asp:Label>
                                            <asp:Label ID="Label2" runat="server" Text="%"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="description">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtdesc" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server" Text='<%# Bind("description") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label122" runat="server" Text='<%# Bind("description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Assigned to">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDLgroup" CssClass="form-select" SelectedValue='<%# Bind("role") %>' runat="server">
                                                <asp:ListItem>student</asp:ListItem>
                                                <asp:ListItem>instructor</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("role") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlschool" runat="server" CssClass="form-select" DataSourceID="SqlDataSource5" DataTextField="school_name" DataValueField="Id" SelectedValue='<%# Bind("school_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [Id], [school_name] FROM SCHOOL_TABLE"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("school_name") %>' ID="LabelPLease1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="isDeleted" SortExpression="isDeleted">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDelete" CssClass="form-control" runat="server" Text='<%# Bind("isDeleted") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1211" runat="server" Text='<%# Bind("isDeleted") %>'></asp:Label>
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
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[5, 'asc']],
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
