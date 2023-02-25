<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="AdminSchedulePage.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="AMBER.Pages.AdminSchedulePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        #txtDay td {
            position: relative;
            text-align: center !important;
        }

        #txtDay label {
            position: absolute;
            top: 0px;
            left: 5px;
        }

        #txtDay input {
            position: absolute;
            top: 10px;
            left: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-calendar-days"></i>
                                Schedule Management</h1>
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
                <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#RegisterModal">Add Schedule</a>
            </div>

        </div>
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="row">
                <div class="col">
                    <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="sched_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="MIS Code">
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" CssClass="form-control" Text='<%# Bind("sched_code") %>' ID="txtCode"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("sched_code") %>' ID="Label33"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlSUBJECT" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="SUBJECT" DataValueField="subject_Id" AutoPostBack="true" SelectedValue='<%# Bind("subject_Id") %>'>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT subject_Id,('('+subject_code+') '+ subject_name) AS SUBJECT FROM SUBJECT_TABLE WHERE (([school_id] = @school_id) AND ([isDeleted] IS NULL))">
                                        <SelectParameters>
                                            <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("SUBJECT") %>' ID="Label1"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Instructor">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlINSTRUCTOR" runat="server" CssClass="form-select" DataSourceID="SqlDataSource2" DataTextField="INSTRUCTOR" DataValueField="id" AutoPostBack="true" SelectedValue='<%# Bind("id") %>'>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT id,(fname +' '+ SUBSTRING(mname, 1, 1)+'. '+lname) AS INSTRUCTOR FROM INSTRUCTOR_TABLE WHERE (([school_id] = @school_id) AND ([isDeleted] IS NULL))">
                                        <SelectParameters>
                                            <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("INSTRUCTOR") %>' ID="Label2"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Time">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txttoTimeStart" TextMode="Time" Text='<%# Bind("startTime") %>' CssClass="form-control" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="txttoTimeEnd" TextMode="Time" Text='<%# Bind("endTime") %>' CssClass="form-control" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("time") %>' ID="Label3"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Day">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlDAY" CssClass="form-select" runat="server" SelectedValue='<%# Bind("day") %>'>
                                        <asp:ListItem Value="MON">Monday</asp:ListItem>
                                        <asp:ListItem Value="TUE">Tuesday</asp:ListItem>
                                        <asp:ListItem Value="WED">Wednesday</asp:ListItem>
                                        <asp:ListItem Value="THU">Thursday</asp:ListItem>
                                        <asp:ListItem Value="FRI">Friday</asp:ListItem>
                                        <asp:ListItem Value="SAT">Saturday</asp:ListItem>
                                        <asp:ListItem Value="SUN">Sunday</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("day") %>' ID="Label42"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assigned to">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlSECTION" runat="server" CssClass="form-select" DataSourceID="SqlDataSource3" DataTextField="section_name" DataValueField="section_id" SelectedValue='<%# Bind("section_id") %>'>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT section_id, section_name FROM SECTION_TABLE WHERE (([school_id] = @school_id) AND ([isDeleted] IS NULL))">
                                        <SelectParameters>
                                            <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("section_name") %>' ID="Label4"></asp:Label>
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

                                <ControlStyle CssClass="btn btn-danger"></ControlStyle>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="modal fade" id="RegisterModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">Schedule</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtCode" CssClass="form-control" runat="server"></asp:TextBox>
                                    <label for="txtCode">MIS Code<asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtCode" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlIns" AutoPostBack="true" OnSelectedIndexChanged="ddlIns_SelectedIndexChanged" CssClass="form-select" runat="server"></asp:DropDownList>
                                    <label for="ddlIns">Instructor<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlIns" ErrorMessage="*" ForeColor="Red" InitialValue="-Instructor-"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlSub" CssClass="form-select" runat="server"></asp:DropDownList>
                                    <label for="ddlSub">Subject<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlSub" ErrorMessage="*" ForeColor="Red" InitialValue="-Subject-"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="row">
                                        <div class="form-floating col">
                                            <asp:TextBox ID="txtTimeStart" TextMode="Time" CssClass="form-control" runat="server"></asp:TextBox>
                                            <label for="txtTimeStart" class="mx-2">Time Start<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtTimeStart" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                        </div>
                                        <div class="form-floating col">
                                            <asp:TextBox ID="txtTimeEnd" TextMode="Time" CssClass="form-control" runat="server"></asp:TextBox>
                                            <label for="txtTimeEnd" class="mx-2">Time End<asp:RequiredFieldValidator ID="RequiredFieldValidator7" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtTimeEnd" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                        </div>
                                    </div>

                                </div>
                                <%--<div class="form-floating mb-3">
                                    
                                    <asp:ListBox ID="lbDay" Height="150" CssClass="form-control" AutoPostBack="true" OnTextChanged="lbDay_TextChanged" Rows="10" SelectionMode="Multiple" runat="server">
                                        <asp:ListItem Value="MON">Monday</asp:ListItem>
                                        <asp:ListItem Value="TUE">Tuesday</asp:ListItem>
                                        <asp:ListItem Value="WED">Wednesday</asp:ListItem>
                                        <asp:ListItem Value="THU">Thursday</asp:ListItem>
                                        <asp:ListItem Value="FRI">Friday</asp:ListItem>
                                        <asp:ListItem Value="SAT">Saturday</asp:ListItem>
                                        <asp:ListItem Value="SUN">Sunday</asp:ListItem>
                                    </asp:ListBox>
                                    <label for="lbDay">Day "HOLD ctrl for Multiple Select"</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtDay" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                    <label for="txtDay">Day<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="txtDay" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                </div>--%>
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlDay" CssClass="form-select" runat="server">
                                        <asp:ListItem Value="MON">Monday</asp:ListItem>
                                        <asp:ListItem Value="TUE">Tuesday</asp:ListItem>
                                        <asp:ListItem Value="WED">Wednesday</asp:ListItem>
                                        <asp:ListItem Value="THU">Thursday</asp:ListItem>
                                        <asp:ListItem Value="FRI">Friday</asp:ListItem>
                                        <asp:ListItem Value="SAT">Saturday</asp:ListItem>
                                        <asp:ListItem Value="SUN">Sunday</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="-Day-"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label for="ddlDay">Day<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlDay" ErrorMessage="*" ForeColor="Red" InitialValue="-Day-"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlSection" CssClass="form-select" runat="server"></asp:DropDownList>
                                    <label for="ddlSection">Section<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqRegister" runat="server" ControlToValidate="ddlSection" ErrorMessage="*" ForeColor="Red" InitialValue="-Section-"></asp:RequiredFieldValidator></label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddSchedule" CssClass="btn btn-primary" ValidationGroup="RqRegister" OnClick="btnAddSchedule_Click" runat="server" Text="Add" />
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
                order: [[7, 'asc']],
                stateSave: true,
            });
        });
    </script>
</asp:Content>
