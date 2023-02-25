<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="SkillsTest.aspx.cs" Inherits="AMBER.Pages.Users.Faculty.SkillsTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script>
        function DeleteConfirm(btnDelete) {

            if (obj.status) {
                obj.status = false;
                return true;
            };

            Swal.fire({
                icon: 'warning',
                title: 'Are you sure?',
                text: 'You won\'t be able to revert this! There will be ' + page /*count unta ni siya*/ + ' rows affected',
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.value) {
                    obj.status = true;
                    //do postback on success
                    obj.ele.click();
                }
            });
            obj.ele = btnDelete;
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-1">
            <div class="col">
                <div class="card" style="background-image: url('../../Pictures/former.png'); background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color:darkslategray">
                                <i class="fa-solid fa-check-to-slot"></i> Skills Test Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <div class="row my-3">
            <div class="col">
                <div class="row">
                    <div class="row">
                        <div class="col-5">
                            <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server" />
                        </div>
                        <div class="col">
                            <div class="row">
                                <div class="col">
                                    <asp:DropDownList ID="InstructorDDL" runat="server" CssClass="form-select"></asp:DropDownList>
                                </div>
                                <div class="col">
                                    <asp:Button ID="btnupload" OnClick="btnupload_Click" CssClass="btn btn-primary" runat="server" Text="Upload CSV File" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:GridView ID="GVSkillsTest" runat="server" class="table table-bordered table-striped" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="GVSkillsTest_RowDeleting">
            <Columns>
                <asp:TemplateField HeaderText="Instructor to be Observed" SortExpression="ins_id">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ins_id") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ins_id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Message From Admin" SortExpression="message">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("message") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("message") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="File" SortExpression="file_name">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("file_name") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("file_name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" OnClick="lnkDownload_Click" runat="server" Text="Download File"
                            CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClientClick="return DeleteConfirm(this);" CommandName="Delete" Text="Delete"></asp:LinkButton>
                        <controlstyle CssClass="btn btn-danger" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable();
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#Table').DataTable();
        });
    </script>
</asp:Content>
