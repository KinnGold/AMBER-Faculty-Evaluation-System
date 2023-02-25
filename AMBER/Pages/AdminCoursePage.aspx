<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="AdminCoursePage.aspx.cs" Inherits="AMBER.BM.AdminCoursePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Admin - Course
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
                            <h1 style="color: darkslategray"><i class="fa-solid fa-scroll"></i>
                                Course Management</h1>
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
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Course</a>
            </div>

        </div>
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="row">
                <div class="col">
                    <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="course_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                        <Columns>
                            <%--<asp:BoundField DataField="course_id" HeaderText="Course ID" InsertVisible="False" ReadOnly="True" SortExpression="course_id" />--%>
                            <%--<asp:BoundField DataField="title" HeaderText="Course Code" SortExpression="title" /> **FIXNAError pa kung mag update og deptcode sa dep nga nag exist nas course**--%>
                            <%--course code--%>
                            <asp:TemplateField HeaderText="Course Code" SortExpression="title">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Text='<%# Bind("title") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("title") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--course name--%>
                            <asp:TemplateField HeaderText="Course Name" SortExpression="description">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text='<%# Bind("description") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--department code--%>
                            <asp:TemplateField HeaderText="Department" SortExpression="dept_code">
                                <EditItemTemplate>
                                    <%--<asp:DropDownList ID="ddlDepCode" SelectedValue='<%# Bind("dept_code") %>' runat="server" CssClass="form-select">
                        </asp:DropDownList>--%>
                                    <asp:DropDownList ID="ddlDepCode" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="dept_code" DataValueField="dept_id" AutoPostBack="true" SelectedValue='<%# Bind("dept_id") %>'>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [dept_id],[dept_code] FROM [DEPARTMENT_TABLE] WHERE (([school_id] = @school_id) AND ([isDeleted] IS NULL))">
                                        <SelectParameters>
                                            <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("dept_code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:CommandField ShowEditButton="True" ControlStyle-CssClass="btn btn-success">
<ControlStyle CssClass="btn btn-success"></ControlStyle>
                </asp:CommandField>--%>
                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="11%" ItemStyle-CssClass="text-center">
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="True" ID="btnUpdate"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>&nbsp;<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                </ItemTemplate>
                                <ControlStyle CssClass="btn btn-success" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false">
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
    </div>

    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Course</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">

                            <asp:DropDownList ID="DropDownList1" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="txtCourseCode">Department</label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtCourseCode" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtCourseCode">Course Code<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtCourseCode" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtDescription">Description<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtDescription" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>


                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddCourse" CssClass="btn btn-primary" ValidationGroup="RqRegister" runat="server" Text="Add" OnClick="btnAddCourse_Click" />
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
                order: [[4, 'asc']],
                stateSave: true,
            });
        });
    </script>
</asp:Content>
