<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="AddAdminUser.aspx.cs" Inherits="AMBER.Pages.AddAdminUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-user-gear"></i>Admin User Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row my-3 mb-3">
            <div class="col d-grid align-content-end justify-content-end">
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Admin User</a>
            </div>
        </div>
        <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="staticBackdropLabel">Admin</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtFname" CssClass="form-control" runat="server"></asp:TextBox>
                                <label for="txtFname">First Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtFname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                            </div>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtMName" CssClass="form-control" runat="server"></asp:TextBox>
                                <label for="txtMName">Middle Name</label>
                            </div>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtLname" CssClass="form-control" runat="server"></asp:TextBox>
                                <label for="txtLname">Last Name<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtLname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                            </div>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtEmail" TextMode="Email" CssClass="form-control" runat="server"></asp:TextBox>
                                <label for="txtEmail">Email<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                            </div>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtPhonenum" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                                <label for="txtPhonenum">Phone Number<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtPhonenum" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                            </div>
                            <div class="form-floating mb-3">
                                <asp:TextBox ID="txtposition" CssClass="form-control" runat="server"></asp:TextBox>
                                <label for="txtposition">Work Position<asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtposition" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <asp:Button ID="btnAddAdmin" OnClick="btnAddAdmin_Click" CssClass="btn btn-primary" ValidationGroup="RqRegister" runat="server" Text="Add" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:PlaceHolder ID="plcData" runat="server">
                    <asp:GridView ID="GridView1" DataKeyNames="Id" OnRowDeleting="GridView1_RowDeleting" class="table table-striped table-bordered" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="First Name" SortExpression="fname">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("fname") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("fname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Middle Name" SortExpression="mname">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("mname") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("mname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name" SortExpression="lname">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("lname") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("lname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" SortExpression="email">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("email") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone" SortExpression="contact_no">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("contact_no") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("contact_no") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Work Position" SortExpression="position">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("position") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("position") %>'></asp:Label>
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
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="plcNoData" runat="server">
                    <div class="row mt-5">
                        <div class="col">
                            <div class="card shadow">
                                <div class="card-body">
                                    <center>
                                        <p>
                                            <img src="../../Pictures/nonodata.png" style="height: 350px; width: 350px;" />
                                        </p>
                                        <h1>No Record Found</h1>
                                    </center>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[5, 'asc']],
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
