<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="DeanSectionManage.aspx.cs" Inherits="AMBER.Pages.Users.Dean.DeanSectionManage" %>
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
                            <h1 style="color:darkslategray"><i class="fa-solid fa-users-rectangle"></i>
                                Section Management</h1>
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
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Section</a>
            </div>

        </div>
        <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="section_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
            <Columns>
                <%--<asp:BoundField DataField="section_id" HeaderText="Section ID" ReadOnly="True" SortExpression="Id" InsertVisible="False" />--%>
                <asp:TemplateField HeaderText="Section">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNAME" CssClass="form-control" runat="server" Text='<%# Bind("section_name") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("section_name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Course">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlCOURSE" runat="server" CssClass="form-select" DataSourceID="SqlDataSource4"  DataTextField="COURSE" DataValueField="course_id" SelectedValue='<%# Bind("course_id") %>'>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT course_id,(title+' - '+description) AS COURSE FROM COURSE_TABLE WHERE school_ID = @school_id AND isDeleted IS NULL">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("COURSE") %>' ID="Label3"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Program">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlprogram" runat="server" CssClass="form-select" SelectedValue='<%# Bind("program") %>'>
                            <asp:ListItem Value="1">DAY</asp:ListItem>
                            <asp:ListItem Value="0">EVENING</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("programdesc") %>' ID="Label3411"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Year Level">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlyear" runat="server" CssClass="form-select" SelectedValue='<%# Bind("yearLevel") %>'>
                            <asp:ListItem Value="1">1st year</asp:ListItem>
                            <asp:ListItem Value="2">2nd year</asp:ListItem>
                            <asp:ListItem Value="3">3rd year</asp:ListItem>
                            <asp:ListItem Value="4">4th year</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("yearLeveldesc") %>' ID="Label36411"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Semester"><%--asd--%>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlSEMESTER" runat="server" CssClass="form-select" DataSourceID="SqlDataSource3"  DataTextField="SEMESTER" DataValueField="semester_id" SelectedValue='<%# Bind("semester_id") %>'>
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
            </Columns>
        </asp:GridView>
    </div>
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" EnablePartialRendering="true" runat="server"></asp:ScriptManager>

    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Section</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="form-group">
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlCourse" AutoPostBack="true" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" CssClass="form-select" runat="server"></asp:DropDownList>
                                    <label for="ddlCourse">Course<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlCourse" ErrorMessage="*" ForeColor="Red" InitialValue="-Course-"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlProgram" AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" CssClass="form-select" runat="server">
                                        <asp:ListItem Value="1">DAY</asp:ListItem>
                                        <asp:ListItem Value="0">EVENING</asp:ListItem>
                                        <asp:ListItem Selected="True">-Program-</asp:ListItem>
                                    </asp:DropDownList>
                                    <label for="ddlProgram">Program<asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlProgram" ErrorMessage="*" ForeColor="Red" InitialValue="-Program-"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlyearlevel" AutoPostBack="true" OnSelectedIndexChanged="ddlyearlevel_SelectedIndexChanged" CssClass="form-select" runat="server">
                                        <asp:ListItem Value="1">1st year</asp:ListItem>
                                        <asp:ListItem Value="2">2nd year</asp:ListItem>
                                        <asp:ListItem Value="3">3rd year</asp:ListItem>
                                        <asp:ListItem Value="4">4th year</asp:ListItem>
                                        <asp:ListItem Selected="True">-Year Level-</asp:ListItem>
                                    </asp:DropDownList>
                                    <label for="ddlyearlevel">Year Level<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlyearlevel" ErrorMessage="*" ForeColor="Red" InitialValue="-Year Level-"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlSemester" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                                    <label for="ddlSemester">Semester<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlSemester" ErrorMessage="*" ForeColor="Red" InitialValue="-Semester-"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtsecname" CssClass="form-control" runat="server"></asp:TextBox>
                                    <label for="txtsecname">Section Name<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtsecname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddSection" CssClass="btn btn-primary" ValidationGroup="RqRegister" runat="server" Text="Add" OnClick="btnAddSection_Click" />
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
