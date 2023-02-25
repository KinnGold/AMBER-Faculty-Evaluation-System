<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="SuperAdminUser.aspx.cs" Inherits="AMBER.Super_Admin.SuperAdminUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="row">
                    <div class="col">
                        <div class="row my-3">
                            <div class="col">
                            </div>
                            <div class="col d-grid justify-content-end">
                                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#ConstructorModal">Add Super Admin User</a>
                            </div>
                        </div>
                        <div class="modal fade" id="ConstructorModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel1" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="staticBackdropLabel1">Super Admin</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-group">
                                            <div class="form-floating mb-3">
                                                <asp:TextBox ID="txtusername" CssClass="form-control" runat="server"></asp:TextBox>
                                                <label for="txtusername">Username<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqAddSuperAdmin" runat="server" ControlToValidate="txtusername" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                            <div class="form-floating mb-3">
                                                <asp:TextBox ID="txtfname" CssClass="form-control" runat="server"></asp:TextBox>
                                                <label for="txtfname">First Name<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqAddSuperAdmin" runat="server" ControlToValidate="txtfname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                            <div class="form-floating mb-3">
                                                <asp:TextBox ID="txtmname" CssClass="form-control" runat="server"></asp:TextBox>
                                                <label for="txtmname">Middle Name<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqAddSuperAdmin" runat="server" ControlToValidate="txtmname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                            <div class="form-floating mb-3">
                                                <asp:TextBox ID="txtlname" CssClass="form-control" runat="server"></asp:TextBox>
                                                <label for="txtlname">Last Name<asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" ValidationGroup="RqAddSuperAdmin" runat="server" ControlToValidate="txtlname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                              <div class="form-floating mb-3">
                                                <asp:TextBox ID="txtemail" CssClass="form-control" runat="server"></asp:TextBox>
                                                <label for="txtemail">Email<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqAddSuperAdmin" runat="server" ControlToValidate="txtemail" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                              <div class="form-floating mb-3">
                                                <asp:TextBox ID="txtcnum" CssClass="form-control" runat="server"></asp:TextBox>
                                                <label for="txtcnum">Contact Number<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqAddSuperAdmin" runat="server" ControlToValidate="txtcnum" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                        <asp:Button ID="addSuperAdmin" CssClass="btn btn-primary" ValidationGroup="RqAddSuperAdmin" runat="server" Text="Add" OnClick="addSuperAdmin_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="Id" InsertVisible="False" SortExpression="Id">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="username" SortExpression="username">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("username") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("username") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="password" SortExpression="password">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("password") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("password") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="first_name" SortExpression="first_name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("first_name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("first_name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="middle_name" SortExpression="middle_name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("middle_name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("middle_name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="last_name" SortExpression="last_name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("last_name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("last_name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="email" SortExpression="email">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("email") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="contact_no" SortExpression="contact_no">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("contact_no") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("contact_no") %>'></asp:Label>
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
