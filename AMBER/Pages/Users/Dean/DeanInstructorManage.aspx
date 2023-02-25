<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="DeanInstructorManage.aspx.cs" Inherits="AMBER.Pages.Users.Dean.DeanInstructorManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        input::-webkit-outer-spin-button,
        input::-webkit-inner-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
    <script>
        function errorFU() {
            Swal.fire({
                icon: 'error',
                title: 'Something went wrong!',
                text: 'No file uploaded',
            })
        };
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('/Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color:darkslategray"><i class="fa-solid fa-user-tie"></i> 
                                Faculty Member Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br /><br />
        <div class="row my-3">
            <div class="col">
                <div class="row">
                    <div class="col-5">
                        <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col">
                        <div class="row">
                            <div class="col">
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                            <div class="col">
                                <asp:Button ID="Button1" OnClick="Button1_Click" CssClass="btn btn-primary" runat="server" Text="Upload CSV File" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-3 d-grid justify-content-end">
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Instructor</a>
            </div>

        </div>
        <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
            <Columns>
                <asp:BoundField DataField="row_num" HeaderText="#" ItemStyle-Width="1%" ReadOnly="True" SortExpression="Id" InsertVisible="False" />
                <asp:TemplateField HeaderText="ID">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtinsID" CssClass="form-control" runat="server" Text='<%# Bind("ins_id") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblinsId" runat="server" Text='<%# Bind("ins_id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="First Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtfirst" CssClass="form-control" runat="server" Text='<%# Bind("fname") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblfirst" runat="server" Text='<%# Bind("fname") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Middle Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtsecond" CssClass="form-control" runat="server" Text='<%# Bind("mname") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblsecond" runat="server" Text='<%# Bind("mname") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtlast" CssClass="form-control" runat="server" Text='<%# Bind("lname") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbllast" runat="server" Text='<%# Bind("lname") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtmail" CssClass="form-control" runat="server" Text='<%# Bind("email") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblemail" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Phone Number">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtnum" CssClass="form-control" runat="server" Text='<%# Bind("phonenum") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblnum" runat="server" Text='<%# Bind("phonenum") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Role">
                    <EditItemTemplate>
                        <%--<asp:TextBox ID="txtSection" CssClass="form-control" runat="server" Text='<%# Bind("section_id") %>'></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select" SelectedValue='<%# Bind("role") %>'>
                            <asp:ListItem>instructor</asp:ListItem>
                            <asp:ListItem>dean</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Labe2l7" runat="server" Text='<%# Bind("role") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department">
                    <EditItemTemplate>
                        <%--<asp:TextBox ID="txtSection" CssClass="form-control" runat="server" Text='<%# Bind("section_id") %>'></asp:TextBox>--%>
                        <asp:DropDownList ID="GVddldepartment" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="dept_name" DataValueField="dept_id" SelectedValue='<%# Bind("dept_id") %>'></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [dept_id], [dept_name],[dept_code] FROM [DEPARTMENT_TABLE] WHERE (([school_id] = @school_id) AND ([isDeleted] IS NULL))">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("dept_code") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="True" ID="btnUpdate"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>&nbsp;<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                    </ItemTemplate>

                    <ControlStyle CssClass="btn btn-success"></ControlStyle>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Instructor</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtID" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtID">ID<asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtID" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtLname" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtLname">Last Name<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtLname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtFname" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtFname">First Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtFname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtMName" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtMName">Middle Name</label>
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
                            <asp:DropDownList ID="ddlInsertRole" CssClass="form-select" runat="server">
                                <asp:ListItem>dean</asp:ListItem>
                                <asp:ListItem>instructor</asp:ListItem>
                                <asp:ListItem Selected="True">-role-</asp:ListItem>
                            </asp:DropDownList>
                            <label for="ddlInsertRole">Role<asp:RequiredFieldValidator ID="RequiredFieldValidator7" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlInsertRole" ErrorMessage="*" ForeColor="Red" InitialValue="-role-"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddlInsertDep" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="ddlInsertDep">Department<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlInsertDep" ErrorMessage="*" ForeColor="Red" InitialValue="-department-"></asp:RequiredFieldValidator></label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddInstructor" CssClass="btn btn-primary" ValidationGroup="RqRegister" OnClick="btnAddInstructor_Click" runat="server" Text="Add"/>
                </div>
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
