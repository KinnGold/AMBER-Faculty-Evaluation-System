<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="SuperAdminNotifications.aspx.cs" Inherits="AMBER.Super_Admin.SuperAdminNotifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3">
            <div class="col">
                <div class="card" style="background-image: url('../../Pictures/former.png'); background-repeat: no-repeat;">
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
                    <%--<div class="row">
                        <div class="col">
                            <div class="row my-3">
                                <div class="col">
                                </div>
                                <div class="col d-grid justify-content-end">
                                    <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#ReplyModal">Reply</a>
                                </div>
                            </div>
                            <div class="modal fade" id="ReplyModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel1" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="staticBackdropLabel1">Reply To A Notification</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="form-group">
                                                <div class="form-floating mb-3">
                                                    <asp:DropDownList ID="nameDDL" CssClass="form-select" runat="server"></asp:DropDownList>
                                                    <label for="txtname">To</label>
                                                </div>
                                                <div class="form-floating mb-3">
                                                    <asp:TextBox ID="txtreply" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
                                                    <label for="txtreply">Reply Here<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqSendReply" runat="server" ControlToValidate="txtreply" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                            <asp:Button ID="sendreplybtn" CssClass="btn btn-primary" OnClick="sendreplybtn_Click" ValidationGroup="RqSendReply" runat="server" Text="Reply" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                    <div class="row ">
                        <div class="col">
                            <div class="row mb-3">
                                <div class="col-8">
                                    <asp:DropDownList ID="DropDownSchool" runat="server" OnSelectedIndexChanged="DropDownSchool_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>
                            <asp:GridView ID="GridView1" class="table table-striped table-bordered scrollbar-primary" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="GridView1_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Message" SortExpression="message">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtmsg" runat="server" Text='<%# Bind("message") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblmsg" runat="server" Text='<%# Bind("message") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name" SortExpression="fromName">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtfname" runat="server" Text='<%# Bind("fromName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblfname" runat="server" Text='<%# Bind("fromName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Number" SortExpression="fromUserID">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtID" runat="server" Text='<%# Bind("fromUserID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("fromUserID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School" SortExpression="school_id">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("school_id") %>' ID="LabelPLease1"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("school_id") %>' ID="LabelPLease2"></asp:Label>
                                        </ItemTemplate>
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
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="plcnoNotifications" runat="server">
            <div class="row">
                <div class="col">
                    <center>
                        <p>
                            <img src="../../Pictures/notificationsring.png" style="height: 350px; width: 350px;" />
                        </p>
                        <h1>No Recent Notifications</h1>
                    </center>
                </div>
            </div>
        </asp:PlaceHolder>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[4, 'asc']],
                stateSave: true,
                "scrollY": "300px",
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
