<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="AdminDepartmentPage.aspx.cs" Inherits="AMBER.BM.AdminDepartmentPage" %>

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
                            <h1 style="color: darkslategray"><i class="fa-solid fa-landmark-flag"></i>
                                Department Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row my-3">
            <div class="col">
            </div>
            <div class="col d-grid justify-content-end">
                <asp:Button ID="btnadd" runat="server" Text="Add Department" CssClass="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal" OnClientClick="return false;" />
            </div>

        </div>
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="row">
                <div class="col">
                    <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="dept_Id" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting" OnRowCancelingEdit="GridView1_RowCancelingEdit">
                        <Columns>
                            <%--<asp:BoundField DataField="dept_Id" HeaderText="Department ID" InsertVisible="False" ReadOnly="True" SortExpression="dept_Id" />--%>
                            <asp:TemplateField HeaderText="Department Code" SortExpression="dept_code">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDepCode" runat="server" CssClass="form-control" Text='<%# Bind("dept_code") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("dept_code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department Name" SortExpression="dept_name">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDepName" runat="server" CssClass="form-control" Text='<%# Bind("dept_name") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("dept_name") %>'></asp:Label>
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
        <%--<asp:SqlDataSource ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:amberDB %>' runat="server" SelectCommand="SELECT * FROM [DEPARTMENT_TABLE]"></asp:SqlDataSource>--%>
    </div>


    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Department</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtdeptCode" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtCourseCode">Department Code<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtdeptCode" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtdeptName" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtDescription">Department Name<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtdeptName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddDepartment" CssClass="btn btn-primary" ValidationGroup="RqRegister" OnClick="btnAddDepartment_Click" runat="server" Text="Add" />
                    <asp:Button ID="btnupdatedept" CssClass="btn btn-success" ValidationGroup="RqRegister" runat="server" Visible="false" Text="Update" />
                </div>
            </div>
        </div>
    </div>
    <!--<div class="modal fade" id="updateModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Department</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtupdatecode" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtCourseCode">Department Code<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtdeptCode" ErrorMessage="*" ForeColor="Red" ></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtupdatename" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtDescription">Department Name<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtdeptName" ErrorMessage="*" ForeColor="Red" ></asp:RequiredFieldValidator></label>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="Button3" CssClass="btn btn-primary" ValidationGroup="RqRegister" OnClick="btnAddDepartment_Click" runat="server" Text="Add" />
                </div>
            </div>
        </div>
    </div>-->
    <script>
        $(document).ready(function () {
            $('#myTable').DataTable();
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[3, 'asc']],
                stateSave: true,
            });
        });
    </script>
</asp:Content>
