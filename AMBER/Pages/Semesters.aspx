<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Semesters.aspx.cs" Inherits="AMBER.BM.Semesters" %>

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
                                <i class="fa-solid fa-calendar-check"></i>Term Management</h1>
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
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Semester</a>
            </div>

        </div>
        <!--<div class="row">
            <table id="myTable" class="table">
                <thead>
                    <tr>
                        <th>Course Code</th>
                        <th>Course Description</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>BSIT</td>
                        <td>Bachelor of Science in Information technology</td>
                    </tr>
                </tbody>
            </table>
        </div>-->
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="row">
                <div class="col">
                    <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="semester_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                        <Columns>
                            <%--<asp:BoundField DataField="semester_id" HeaderText="ID" ReadOnly="True" SortExpression="Id" />--%>
                            <%--<asp:TemplateField HeaderText="Version" SortExpression="version"> 
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtversion" CssClass="form-control" ReadOnly="true" Text='<%# Bind("version") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="Label4" Text='<%# Bind("version") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Semester Name" SortExpression="description">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Bind("description") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="School Year" SortExpression="year">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtYear" runat="server" ReadOnly="true" CssClass="form-control" Text='<%# Bind("year") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("year") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="status">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" SelectedValue='<%# Bind("status") %>'>
                                        <asp:ListItem>Active</asp:ListItem>
                                        <asp:ListItem>Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label33" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Evaluation Start">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEvalStart" CssClass="form-control" min='<%# Eval("minyear") %>' max='<%# Eval("maxyear") %>' TextMode="DateTimeLocal" Text='<%# Bind("evaluationStart") %>' runat="server"></asp:TextBox><%--min="2022-01-01T00:00" max="2023-01-01T00:00"--%>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label23" runat="server" Text='<%# Bind("actualStart") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Evaluation End">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEvalEnd" CssClass="form-control" TextMode="DateTimeLocal" min='<%# Eval("minyear") %>' max='<%# Eval("maxyear") %>' Text='<%# Bind("evaluationEnd") %>' runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("actualEnd") %>'></asp:Label>
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

    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Semester</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddlsemester" CssClass="form-select" runat="server">
                                <asp:ListItem>1st Semester</asp:ListItem>
                                <asp:ListItem>2nd Semester</asp:ListItem>
                                <asp:ListItem>3rd Trimester</asp:ListItem>
                                <asp:ListItem>Summer Class</asp:ListItem>
                                <asp:ListItem Selected="True">-Select Status-</asp:ListItem>
                            </asp:DropDownList>
                            <label for="txtsemname">Semester Name<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlsemester" ErrorMessage="*" ForeColor="Red" InitialValue="-Select Status-"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddlschoolyear" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="ddlschoolyear">School Year<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlschoolyear" ErrorMessage="*" ForeColor="Red" InitialValue="-school year-"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddstatus" CssClass="form-select" runat="server">
                                <asp:ListItem Text="Active" Value="Active" />
                                <asp:ListItem Text="Inactive" Value="Inactive" />
                                <asp:ListItem Selected="True">-status-</asp:ListItem>
                            </asp:DropDownList>
                            <label for="ddstatus">Status<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddstatus" ErrorMessage="*" ForeColor="Red" InitialValue="-status-"></asp:RequiredFieldValidator></label>
                        </div>
                        <label>Evaluation for this semester<i class="fa-solid fa-circle-info mx-2" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-original-title="You can proceed if you have not yet know the schedule of the evlauation"></i></label>
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtEvalStart" CssClass="form-control" TextMode="DateTimeLocal" runat="server"></asp:TextBox>
                            <label for="txtEvalStart">Evaluation starts at...<%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtEvalStart" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%></label>
                        </div>
                        <hr />
                        <div class="form-floating mb-3">
                            <asp:TextBox ID="txtEvalEnd" CssClass="form-control" TextMode="DateTimeLocal" runat="server"></asp:TextBox>
                            <label for="txtEvalEnd">Evaluation ends on...<%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtEvalEnd" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%></label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddsemester" CssClass="btn btn-primary" ValidationGroup="RqRegister" runat="server" Text="Add" OnClick="btnAddsemester_Click" />
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
                order: [[6, 'asc']],
                stateSave: true,
            });
        });
    </script>
</asp:Content>
