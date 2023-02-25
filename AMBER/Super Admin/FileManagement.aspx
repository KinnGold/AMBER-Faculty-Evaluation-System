<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="FileManagement.aspx.cs" Inherits="AMBER.Super_Admin.FileManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3">
            <div class="col">
                <div class="card" style="background-image: url('/Pictures/former.png'); background-repeat: no-repeat; background-size: cover;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray">
                                <i class="fa-solid fa-users-gear"></i>Files Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <div class="row my-3 mb-5">
            <div class="col-7 mx-auto shadow-lg">
                <br />
                <center>
                    <label for="body_FileUpload1"><i class="fa-solid fa-file-circle-plus text-warning" style="font-size: 250px; cursor: pointer;"></i></label>
                    <asp:FileUpload ID="FileUpload1" onchange="getInfo(this.value);" Style="display: none; visibility: hidden;" CssClass="form-control" runat="server" />
                    <div id="displayInfo" class="p-2">Browse file</div>
                    <asp:Button ID="btnUpload" OnClick="btnLogin_Click" CssClass="btn btn-primary" runat="server" Text="Upload File" />
                </center>
                <br />
            </div>
        </div>
        <div class="row">
            <div class="col">
                <div class="card shadow-lg">
                    <div class="card-body">
                        <asp:GridView ID="GridView1" class="table table-striped table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="Id" InsertVisible="False" SortExpression="Id">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name" SortExpression="file_name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("file_name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("file_name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Type" SortExpression="content_type">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("content_type") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("content_type") %>'></asp:Label>
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
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#myTable').DataTable();
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[1, 'asc']],
                stateSave: true,
            });
        });
    </script>
</asp:Content>
