<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="DeanSubjectManage.aspx.cs" Inherits="AMBER.Pages.Users.Dean.DeanSubjectManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('/Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color:darkslategray"><i class="fa-solid fa-book-open-reader"></i>
                                Subject Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br /><br />
        <div class="row my-3">
            <div class="col">
            </div>
            <div class="col d-grid justify-content-end">
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Subject</a>
            </div>

        </div>
        <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="subject_Id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
            <Columns>
                <%--<asp:BoundField DataField="subject_Id" HeaderText="Subject ID" ReadOnly="True" SortExpression="subject_Id" InsertVisible="False" />--%>
                <asp:TemplateField HeaderText="Subject Code" SortExpression="subject_code">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSubCode" CssClass="form-control" runat="server" Text='<%# Bind("subject_code") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("subject_code") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject Name" SortExpression="subject_name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSubName" runat="server" CssClass="form-control" Text='<%# Bind("subject_name") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("subject_name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Unit">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" TextMode="Number" CssClass="form-control" Text='<%# Bind("unit") %>' ID="txtUNIT"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("unit") %>' ID="Label332"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department" SortExpression="dept_code">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDepCode" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1"  DataTextField="dept_code" DataValueField="dept_id" SelectedValue='<%# Bind("dept_id") %>'>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [dept_id],[dept_code] FROM [DEPARTMENT_TABLE] WHERE (([school_id] = @school_id) AND ([isDeleted] IS NULL))">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# Bind("dept_code") %>'></asp:Label>
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
            </Columns>

        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:amberDB %>' runat="server" SelectCommand="SELECT * FROM [SUBJECT_TABLE]"></asp:SqlDataSource>

    </div>


    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Subject</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtsubcode" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtsubcode">Subject Code<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtsubcode" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtsubname" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtsubname">Subject Name<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtsubname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtUnit" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtUnit">Unit<asp:RequiredFieldValidator ID="RequiredFieldValidator7" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtUnit" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddlInsertDep" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="ddlInsertDep">Department<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlInsertDep" ErrorMessage="*" ForeColor="Red" InitialValue="-department-"></asp:RequiredFieldValidator></label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddsubject" CssClass="btn btn-primary" ValidationGroup="RqRegister" runat="server" Text="Add" OnClick="btnAddsubject_Click"/>
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
                order: [[3, 'asc']],
                stateSave: true,
            });
        });
    </script>
</asp:Content>
