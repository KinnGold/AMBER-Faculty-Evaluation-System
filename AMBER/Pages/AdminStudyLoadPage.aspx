<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="AdminStudyLoadPage.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="AMBER.Pages.AdminStudyLoadPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row my-3">
            <div class="col">
            </div>
            <div class="col d-grid justify-content-end">
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Study Load</a>
            </div>

        </div>
        <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="load_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
            <Columns>
                <asp:TemplateField HeaderText="Section">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlSECTION" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1"  DataTextField="section_name" DataValueField="section_id" SelectedValue='<%# Bind("section_id") %>'>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT section_id, section_name FROM SECTION_TABLE WHERE (([campus_id] = @campus_id) AND ([isDeleted] IS NULL))">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="campus" Name="campus_id" Type="Int32"></asp:SessionParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("section_name") %>' ID="Label4"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Schedule">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlSCHED" runat="server" CssClass="form-select" DataSourceID="SqlDataSource2"  DataTextField="SCHEDULE" DataValueField="sched_id" SelectedValue='<%# Bind("section_id") %>'>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT SCHEDULE_TABLE.sched_id, ('('+SUBJECT_TABLE.subject_code+') '+ SUBSTRING(INSTRUCTOR_TABLE.fname, 1, 1)+'. '+INSTRUCTOR_TABLE.lname+' ['+SCHEDULE_TABLE.time+'] '+SCHEDULE_TABLE.day) AS SCHEDULE FROM SCHEDULE_TABLE JOIN SUBJECT_TABLE ON SCHEDULE_TABLE.subject_id = SUBJECT_TABLE.subject_Id JOIN INSTRUCTOR_TABLE ON SCHEDULE_TABLE.instructor_id = INSTRUCTOR_TABLE.Id WHERE SCHEDULE_TABLE.campus_id = @campus_id AND SCHEDULE_TABLE.isDeleted IS NULL">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="campus" Name="campus_id" Type="Int32"></asp:SessionParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("SCHEDULE") %>' ID="Label2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Semester"><%--asd--%>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlSEMESTER" runat="server" CssClass="form-select" DataSourceID="SqlDataSource3"  DataTextField="SEMESTER" DataValueField="semester_id" SelectedValue='<%# Bind("semester_id") %>'>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT semester_id, (description+' SY '+year) AS SEMESTER FROM SEMESTER_TABLE WHERE CAMPUS_ID = @campus_id AND isDeleted IS NULL AND status = 'ACTIVE'">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="campus" Name="campus_id" Type="Int32"></asp:SessionParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("SEMESTER") %>' ID="Label1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Course">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlCOURSE" runat="server" CssClass="form-select" DataSourceID="SqlDataSource4"  DataTextField="COURSE" DataValueField="course_id" SelectedValue='<%# Bind("course_id") %>'>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT course_id,(title+' - '+description) AS COURSE FROM COURSE_TABLE WHERE CAMPUS_ID = @campus_id AND isDeleted IS NULL">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="campus" Name="campus_id" Type="Int32"></asp:SessionParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("COURSE") %>' ID="Label3"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:CommandField ShowEditButton="True" ControlStyle-CssClass="btn btn-success"/>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text="Delete" CommandName="Delete" CausesValidation="False" ID="LinkButton1" OnClientClick="return DeleteConfirm(this); "></asp:LinkButton>
                    </ItemTemplate>

                    <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Study Load</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddlSection" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="ddlSection">Section<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlSection" ErrorMessage="*" ForeColor="Red" InitialValue="-Section-"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddlSchedule" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="ddlSchedule">Schedule<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlSchedule" ErrorMessage="*" ForeColor="Red" InitialValue="-Schedule-"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddlCourse" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="ddlCourse">Course<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlCourse" ErrorMessage="*" ForeColor="Red" InitialValue="-Course-"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-floating mb-3">
                            <asp:DropDownList ID="ddlSemester" CssClass="form-select" runat="server"></asp:DropDownList>
                            <label for="ddlSemester">Semester<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlSemester" ErrorMessage="*" ForeColor="Red" InitialValue="-Semester-"></asp:RequiredFieldValidator></label>
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
                order: [[5, 'asc']],
                stateSave: true,
            });
        });
    </script>
</asp:Content>
