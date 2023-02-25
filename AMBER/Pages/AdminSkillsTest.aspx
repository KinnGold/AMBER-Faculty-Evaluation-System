<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="AdminSkillsTest.aspx.cs" Inherits="AMBER.Pages.AdminSkillsTest" %>

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
                            <h1 style="color: darkslategray">
                                <i class="fa-solid fa-check-to-slot"></i>Skills Test Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row mb-5">
            <div class="col">
                <div class="card shadow-lg" style="border-color: transparent;">
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-6 d-grid gap-2">
                                <div class="input-group">
                                     <button id="show_password" class="btn btn-warning" type="button">
                                        <svg xmlns="http://www.w3.org/2000/svg" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-title="This button will Activate or Deactivate the Skills Test Period. If it's Activated, then Skills Test Period is On-going. If it's Deactivated, then Skills Test Period is no longer available." width="16" height="16" fill="currentColor" class="bi bi-question-circle-fill" viewBox="0 0 16 16">
                                            <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.496 6.033h.825c.138 0 .248-.113.266-.25.09-.656.54-1.134 1.342-1.134.686 0 1.314.343 1.314 1.168 0 .635-.374.927-.965 1.371-.673.489-1.206 1.06-1.168 1.987l.003.217a.25.25 0 0 0 .25.246h.811a.25.25 0 0 0 .25-.25v-.105c0-.718.273-.927 1.01-1.486.609-.463 1.244-.977 1.244-2.056 0-1.511-1.276-2.241-2.673-2.241-1.267 0-2.655.59-2.75 2.286a.237.237 0 0 0 .241.247zm2.325 6.443c.61 0 1.029-.394 1.029-.927 0-.552-.42-.94-1.029-.94-.584 0-1.009.388-1.009.94 0 .533.425.927 1.01.927z" />
                                        </svg>
                                    </button>
                                    <asp:LinkButton ID="activateSkillsTest" OnClick="activateSkillsTest_Click" CssClass="btn btn-primary" runat="server">Activate Skills Test Period</asp:LinkButton>
                                    <asp:LinkButton ID="deactivateSkillsTest" runat="server" CssClass="btn btn-danger" OnClick="deactivateSkillsTest_Click">Deactivate Skills Test  Period</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-lg-4">
                                <div class="form-group">
                                    <div class="form-floating">
                                        <asp:DropDownList ID="DepartmentDDL" OnSelectedIndexChanged="DepartmentDDL_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                                        <label for="txtdept">Department</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4">
                                <div class="form-group">
                                    <div class="form-floating">
                                        <asp:DropDownList ID="DeanDDL" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                                        <label for="txtdean">Dean</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4">
                                <div class="form-group">
                                    <div class="form-floating">
                                        <asp:DropDownList ID="InstructorDDL" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                                        <label for="txtinstructor">Instructor</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row ">
                            <asp:Label runat="server" ID="lblDateTime">Skills Test Schedule</asp:Label>
                            <div class="col-6">
                                <asp:TextBox ID="txtDateTime" TextMode="DateTimeLocal" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 d-grid gap-2 mx-auto">
                                <asp:Button ID="btnrequest" OnClick="btnrequest_Click" CssClass="btn btn-primary" ValidationGroup="RqRequest" runat="server" Text="Send Request" />
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="row">
                <div class="col">
                    <div class="card">
                        <div class="card-body shadow-lg">
                            <asp:GridView ID="GVResults" class="table table-bordered table-striped" runat="server" AutoGenerateColumns="False" DataKeyNames="evaluatee_id">
                                <Columns>
                                    <asp:TemplateField HeaderText="Instructor Name" SortExpression="NAME">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("NAME") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Average" SortExpression="AVERAGE">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("AVERAGE") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("AVERAGE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
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
