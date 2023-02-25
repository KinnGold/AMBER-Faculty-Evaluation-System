<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="InstructorManagement.aspx.cs" Inherits="AMBER.Super_Admin.InstructorManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-5">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('/Pictures/former.png'); background-repeat: no-repeat; background-size: cover;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-user-tie"></i>
                                Faculty Member Management</h1>
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
                            <asp:GridView ID="GridView1" class="table table-striped table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="#" InsertVisible="False" SortExpression="Id">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Number" SortExpression="ins_id">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtidnum" CssClass="form-control" ReadOnly="true" runat="server" Text='<%# Bind("ins_id") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("ins_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="First Name" SortExpression="fname">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtfname" CssClass="form-control" runat="server" Text='<%# Bind("fname") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("fname") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Middle Name" SortExpression="mname">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtmname" CssClass="form-control" runat="server" Text='<%# Bind("mname") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("mname") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Name" SortExpression="lname">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtlname" CssClass="form-control" runat="server" Text='<%# Bind("lname") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("lname") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email" SortExpression="email">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtemail" CssClass="form-control" runat="server" Text='<%# Bind("email") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contact #" SortExpression="phonenum">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtcnum" CssClass="form-control" runat="server" Text='<%# Bind("phonenum") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("phonenum") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Password" SortExpression="password">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtpassword" ReadOnly="true" CssClass="form-control" runat="server" Text='<%# Bind("password") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label8" runat="server" Text="encrypted"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role" SortExpression="role">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtrole" CssClass="form-control" runat="server" Text='<%# Bind("role") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label9" runat="server" Text='<%# Bind("role") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School" SortExpression="school_name">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlschool" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="school_name" DataValueField="Id" AutoPostBack="true" SelectedValue='<%# Bind("school_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [Id],[school_name] FROM [SCHOOL_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label10" runat="server" Text='<%# Bind("school_name") %>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="isDeleted" SortExpression="isDeleted">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtdeleted" CssClass="form-control" ReadOnly="true" runat="server" Text='<%# Bind("isDeleted") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label12" runat="server" Text='<%# Bind("isDeleted") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Forgot Password OTP" SortExpression="forgot_otp">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtforgototp" CssClass="form-control" runat="server" ReadOnly="true" Text='<%# Bind("forgot_otp") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label13" runat="server" Text='<%# Bind("forgot_otp") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Verficiation COde" SortExpression="verify_code">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtverifycode" CssClass="form-control" ReadOnly="true" runat="server" Text='<%# Bind("verify_code") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label14" runat="server" Text='<%# Bind("verify_code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="isVerified" SortExpression="isVerified">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("isVerified") %>' Enabled="false" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("isVerified") %>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lock Status" SortExpression="lock_status">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("lock_status") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("lock_status") %>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lock DateTime" SortExpression="lockdatetime">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtdatetime" CssClass="form-control" ReadOnly="true" runat="server" Text='<%# Bind("lockdatetime") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label15" runat="server" Text='<%# Bind("lockdatetime") %>'></asp:Label>
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
