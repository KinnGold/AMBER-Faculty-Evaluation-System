<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="AdminConstructor.aspx.cs" Inherits="AMBER.BM.AdminConstructor" %>

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
                            <h1 style="color: darkslategray"><i class="fa-solid fa-file-circle-question"></i>Category Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col">
                <div class="row">
                    <div class="col">
                        <div class="row my-3">
                            <div class="col">
                                <div class="row">
                                    <asp:Label ID="lblStudentTotal" runat="server"></asp:Label>
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblInstructorTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col d-grid justify-content-end">
                                <a class="btn btn-warning my-auto" data-bs-toggle="modal" data-bs-target="#ConstructorModal">Manage Category</a>
                            </div>
                        </div>
                        <div class="modal fade" id="ConstructorModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel1" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="staticBackdropLabel1">Manage Category</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-group">
                                            <div class="form-floating mb-3">
                                                <asp:TextBox ID="txtConstructor" CssClass="form-control" runat="server"></asp:TextBox>
                                                <label for="txtConstructor">Category<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqConstructor" runat="server" ControlToValidate="txtConstructor" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                            <div class="form-floating mb-3">
                                                <textarea id="txtDesc" runat="server" class="form-control" cols="20" rows="2"></textarea>
                                                <label for="txtDesc">Description<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqConstructor" runat="server" ControlToValidate="txtDesc" ErrorMessage=" Brief explaination for the category" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                            <div class="form-floating mb-3">
                                                <asp:DropDownList ID="VersionDDL" CssClass="form-select" runat="server"></asp:DropDownList>
                                                <label for="VersionDDL">Semester<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqConstructor" runat="server" ControlToValidate="VersionDDL" ErrorMessage="*" ForeColor="Red" InitialValue=" "></asp:RequiredFieldValidator></label>
                                            </div>
                                            <%--<div class="form-floating">
                                                <svg xmlns="http://www.w3.org/2000/svg" data-bs-toggle="tooltip" data-bs-placement="left" data-bs-title="Please select the blank or null space for Department if you are adding a question for the Students. The Department Dropdownlist is intended for Peer and Self-Evaluation for Instructors." width="16" height="16" fill="currentColor" class="bi bi-question-circle-fill" viewBox="0 0 16 16">
                                                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.496 6.033h.825c.138 0 .248-.113.266-.25.09-.656.54-1.134 1.342-1.134.686 0 1.314.343 1.314 1.168 0 .635-.374.927-.965 1.371-.673.489-1.206 1.06-1.168 1.987l.003.217a.25.25 0 0 0 .25.246h.811a.25.25 0 0 0 .25-.25v-.105c0-.718.273-.927 1.01-1.486.609-.463 1.244-.977 1.244-2.056 0-1.511-1.276-2.241-2.673-2.241-1.267 0-2.655.59-2.75 2.286a.237.237 0 0 0 .241.247zm2.325 6.443c.61 0 1.029-.394 1.029-.927 0-.552-.42-.94-1.029-.94-.584 0-1.009.388-1.009.94 0 .533.425.927 1.01.927z" />
                                                </svg>
                                            </div>--%>
                                            <div class="form-floating mb-3">
                                                <asp:DropDownList ID="ddlGroup" CssClass="form-select" runat="server">
                                                    <asp:ListItem>student</asp:ListItem>
                                                    <asp:ListItem>instructor</asp:ListItem>
                                                    <asp:ListItem Selected="True"> </asp:ListItem>
                                                </asp:DropDownList>
                                                <label for="ddlGroup">Assigned to<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqConstructor" runat="server" ControlToValidate="ddlGroup" ErrorMessage="*" ForeColor="Red" InitialValue=" "></asp:RequiredFieldValidator></label>
                                            </div>
                                            <div class="form-floating mb-3">
                                                <asp:TextBox ID="txtWeight" min="1" MaxLength="3" max="100" oninput="this.value = Math.abs(this.value)" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                                                <label for="txtWeight">Weigth<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqConstructor" runat="server" ControlToValidate="txtWeight" ErrorMessage=" % percentage for the category" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                        <asp:Button ID="addConstructor" OnClick="addConstructor_Click" CssClass="btn btn-primary" ValidationGroup="RqConstructor" runat="server" Text="Add" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder ID="plcData" runat="server">
                    <div class="row">
                        <div class="col">
                            <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="constructor_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
                                <Columns>
                                    <%--<asp:BoundField DataField="constructor_id" HeaderText="Category ID" InsertVisible="False" ReadOnly="True" SortExpression="constructor_id" />--%>
                                    <asp:TemplateField HeaderText="Name" SortExpression="constructor_name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtname" CssClass="form-control" runat="server" Text='<%# Bind("constructor_name") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label12" runat="server" Text='<%# Bind("constructor_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="description">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtdesc" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server" Text='<%# Bind("description") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label122" runat="server" Text='<%# Bind("description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category Weight" SortExpression="weight">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtweight" TextMode="Number" min="1" MaxLength="3" max="100" oninput="this.value = Math.abs(this.value)" CssClass="form-control" runat="server" Text='<%# Bind("weight") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1222" runat="server" Text='<%# Bind("weight") %>'></asp:Label>
                                            <asp:Label ID="Label2" runat="server" Text="%"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Assigned to">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDLgroup" CssClass="form-select" SelectedValue='<%# Bind("role") %>' runat="server">
                                                <asp:ListItem>student</asp:ListItem>
                                                <asp:ListItem>instructor</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("role") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Semester"><%--asd--%>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlSEMESTER" runat="server" CssClass="form-select" DataSourceID="SqlDataSource3" DataTextField="SEMESTER" DataValueField="semester_id" SelectedValue='<%# Bind("semester_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT semester_id, (description+' SY '+year) AS SEMESTER FROM SEMESTER_TABLE WHERE school_ID = @school_id AND isDeleted IS NULL">
                                                <SelectParameters>
                                                    <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("SEMESTER") %>' ID="Label11"></asp:Label>
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
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[6, 'asc']],
                stateSave: true,
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#Table').DataTable();
        });
    </script>
</asp:Content>
