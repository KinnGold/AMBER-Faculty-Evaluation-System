<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="SchoolManagement.aspx.cs" Inherits="AMBER.Super_Admin.SchoolManagement" %>

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
                            <h1 style="color: darkslategray"><i class="fa-solid fa-building-columns"></i>
                                School Management </h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:GridView ID="schoolGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="table table-striped table-bordered shadow-lg" OnRowDeleting="schoolGridView_RowDeleting" OnRowCancelingEdit="schoolGridView_RowCancelingEdit" OnRowEditing="schoolGridView_RowEditing" OnRowUpdating="schoolGridView_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Id" InsertVisible="False" SortExpression="Id">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School Name" SortExpression="School">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="form-control" Text='<%# Bind("school_name") %>' ID="txtSchoolName"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblschoolName" runat="server" Text='<%# Bind("school_name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School Phone" SortExpression="school_phone">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="form-control" Text='<%# Bind("school_phone") %>' ID="txtSchoolPhone"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblschoolPhone" runat="server" Text='<%# Bind("school_phone") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School Address" SortExpression="address">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="form-control" Text='<%# Bind("address") %>' ID="txtSchoolAddress"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblschoolAddress" runat="server" Text='<%# Bind("address") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="11%" ItemStyle-CssClass="text-center">
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="True" ID="btnUpdate"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>
                                &nbsp;<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" ID="LinkButton4"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CausesValidation="False" ID="LinkButton5"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle CssClass="btn btn-success" />
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
    <script>
        $(document).ready(function () {
            $('#myTable').DataTable();
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
            });
        });
    </script>
</asp:Content>
