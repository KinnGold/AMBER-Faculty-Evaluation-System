<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="SchoolUpload.aspx.cs" Inherits="AMBER.Super_Admin.SchoolUpload" %>

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
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" EnablePartialRendering="true" runat="server" />
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-building-columns"></i>
                                School Upload </h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
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
                                <asp:Button ID="btnupload" OnClick="btnupload_Click" CssClass="btn btn-primary" runat="server" Text="Upload CSV File" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-3 d-grid justify-content-end">
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add School</a>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:GridView ID="schoolGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="table table-striped table-bordered shadow-lg" OnRowDeleting="schoolGridView_RowDeleting" OnRowUpdating="schoolGridView_RowUpdating" OnRowCancelingEdit="schoolGridView_RowCancelingEdit" OnRowEditing="schoolGridView_RowEditing">
                    <Columns>
                        <asp:TemplateField HeaderText="Id" InsertVisible="False" SortExpression="Id">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School Name" SortExpression="school_name">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="form-control" Text='<%# Bind("school_name") %>' ID="txtName"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblschoolName" runat="server" Text='<%# Bind("school_name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School Address" SortExpression="school_address">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" CssClass="form-control" Text='<%# Bind("school_address") %>' ID="txtAddress"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblschoolAddress" runat="server" Text='<%# Bind("school_address") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="11%" ItemStyle-CssClass="text-center">
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="True" ID="btnUpdate"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>
                                &nbsp;<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" ID="LinkButton4"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CausesValidation="False" ID="LinkButton5"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle CssClass="btn btn-success" />
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
    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabe">Add School</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtschoolName" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtschoolName">School Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqAddSchool" runat="server" ControlToValidate="txtschoolName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                         <div class="form-floating mb-3">
                            <asp:TextBox ID="txtSchoolAddress" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtSchoolAddress">School Address<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqAddSchool" runat="server" ControlToValidate="txtSchoolAddress" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="Button2" CssClass="btn btn-warning" ValidationGroup="RqAddSchool" OnClick="Button2_Click" runat="server" Text="Add" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="UpdateModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel1">Update School</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtEditSchoolName" CssClass="form-control" runat="server"></asp:TextBox>
                            <label for="txtEditSchoolName">School Name<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqEditName" runat="server" ControlToValidate="txtEditSchoolName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="Button1" CssClass="btn btn-success" ValidationGroup="RqEditName" runat="server" Text="Update" />
                </div>
            </div>
        </div>
    </div>

    <script>
        function OpenModal(event) {
            $('#UpdateModal').modal('show');
        }
    </script>
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
