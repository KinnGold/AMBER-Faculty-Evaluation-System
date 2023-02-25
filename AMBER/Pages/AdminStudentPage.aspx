<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="AdminStudentPage.aspx.cs" Inherits="AMBER.BM.AdminStudentPage" %>

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
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-user-graduate"></i>
                                Student Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row my-3">
            <div class="col">
                <div class="row">
                    <div class="col-5">
                        <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col">
                        <div class="row">
                            <div class="col">
                                <asp:DropDownList ID="ddlSection" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                            <div class="col">
                                <asp:Button ID="Button1" OnClick="Button1_Click" CssClass="btn btn-primary" runat="server" Text="Upload CSV File" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-3 d-grid justify-content-end">
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Student</a>
            </div>

        </div>
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="row">
                <div class="col">
                    <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                        <Columns>
                            <asp:BoundField DataField="row_num" ReadOnly="True" SortExpression="Id" InsertVisible="False" HeaderText="#" />
                            <asp:TemplateField HeaderText="Student ID" SortExpression="Stud_id">
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" CssClass="form-control" Text='<%# Bind("Stud_Id") %>' ID="txtID"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("Stud_Id") %>' ID="Label1"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="First Name">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtfirst" CssClass="form-control" runat="server" Text='<%# Bind("fname") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("fname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Middle Name">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtsecond" CssClass="form-control" runat="server" Text='<%# Bind("mname") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("mname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtthird" CssClass="form-control" runat="server" Text='<%# Bind("lname") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("lname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtmail" CssClass="form-control" runat="server" Text='<%# Bind("email") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone Number">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtnum" CssClass="form-control" runat="server" Text='<%# Bind("phonenum") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("phonenum") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Section">
                                <EditItemTemplate>
                                    <%--<asp:TextBox ID="txtSection" CssClass="form-control" runat="server" Text='<%# Bind("section_id") %>'></asp:TextBox>--%>
                                    <asp:DropDownList ID="GVddlsection" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="section_name" DataValueField="section_id" SelectedValue='<%# Bind("section_id") %>'></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [section_id], [section_name] FROM [SECTION_TABLE] WHERE (([school_id] = @school_id) AND ([isDeleted] IS NULL))">
                                        <SelectParameters>
                                            <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("section_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="True" ID="btnUpdate"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>&nbsp;<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="False" CommandName="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                </ItemTemplate>
                                <ControlStyle CssClass="btn btn-success" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return DeleteConfirm(this);"><i class="fa-solid fa-trash-can"></i></asp:LinkButton>
                                </ItemTemplate>
                                <ControlStyle CssClass="btn btn-danger" />
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>
            </div>
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
        <%--<asp:GridView class="table table-striped table-bordered" ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Student ID" ReadOnly="True" SortExpression="Id" InsertVisible="False" />
                <asp:BoundField DataField="fname" HeaderText="First Name" />
                <asp:BoundField DataField="mname" HeaderText="Middle Name" />
                <asp:BoundField DataField="lname" HeaderText="Last Name" />
                <asp:BoundField DataField="email" HeaderText="Email" />
            </Columns>

        </asp:GridView>--%>
    </div>


    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Student</h5>
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
                            <asp:DropDownList ID="ddlinsertSec" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="ddlinsertSec">Section<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlinsertSec" ErrorMessage="*" ForeColor="Red" InitialValue="-section-"></asp:RequiredFieldValidator></label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddstudent" CssClass="btn btn-primary" ValidationGroup="RqRegister" runat="server" Text="Add" OnClick="btnAddstudent_Click" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('#myTable').DataTable({
                order: [[3, 'asc']],
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                /* order: [[8, 'asc']],*/
                stateSave: true,
            });
        });
    </script>
    <%--order unta nko nga kuhaon ang ID generated pero ig edit mu first row mn so noting this--%>
</asp:Content>
