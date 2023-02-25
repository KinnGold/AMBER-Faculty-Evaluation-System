<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="AMBER.Pages.Users.Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3">
            <div class="col">
                <div class="card" style="background-image: url('../../../Pictures/former.png'); background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray">Notifications Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="plcnotifications" runat="server">
            <div class="row">
                <div class="col">
                    <div class="row">
                        <div class="col">
                            <asp:GridView ID="GridView1" class="table table-striped table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="GridView1_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Message" SortExpression="message">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtmsg" runat="server" Text='<%# Bind("message") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblmsg" runat="server" Text='<%# Bind("message") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From" SortExpression="fromUser_role">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtrole" runat="server" Text='<%# Bind("fromUser_role") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblrole" runat="server" Text='<%# Bind("fromUser_role") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return DeleteConfirm(this);"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ControlStyle CssClass="btn btn-danger" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcnoNotifications" runat="server">
            <div class="row">
                <div class="col">
                    <center>
                        <p>
                            <img src="../../../Pictures/notificationsring.png" style="height: 350px; width: 350px;" />
                        </p>
                        <h1>No Recent Notifications</h1>
                    </center>
                </div>
            </div>
        </asp:PlaceHolder>

    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[1, 'asc']],
                stateSave: true,
                "scrollY": "400px",
                "scrollCollapse": true,
                fixedHeader: true,
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#Table').DataTable();
        });
    </script>
</asp:Content>
