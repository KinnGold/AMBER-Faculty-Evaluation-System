<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="AdminLandingPage.aspx.cs" Inherits="AMBER.BM.AdminLandingPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="title" runat="server">
    Admin
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid p-5 min-vh-100 m-0 d-flex flex-column justify-content-center bg-transparent" style="background-image: url(/../../Pictures/1.png);">
        <div class="row">
            <div class="col">
                <section class="mb-3">
                    <div class="row">
                        <div class="col">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-4">
                                            <p>
                                                Welcome
                                                <asp:Label ID="lbladmin" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                            </p>
                                        </div>
                                        <div class="col-4">
                                            <p>
                                                Semester:
                                                <asp:Label ID="lblsem" Font-Bold="true" runat="server" Text="No Active Semester"></asp:Label>
                                            </p>
                                        </div>
                                        <div class="col-4">
                                            <p>
                                                Evaluation Period:
                                                <asp:Label ID="lblstatus" Font-Bold="true" runat="server" Text="No Active Evaluation"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <section class="mb-3">
                    <div class="row">
                        <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Instructors">Instructors</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="instlbl" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <label for="btnInstructor"><i class="fa-solid fa-users-between-lines" style="height: 100%; width: 100%; cursor: pointer;"></i></label>
                                            <button type="button" id="btnInstructor" class="btn btn-primary" Style="display: none; visibility: hidden;" data-toggle="modal" data-target="#instructorModal">Launch demo modal</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Student">Students</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="stdlbl" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <label for="btnStudent"><i class="fa-solid fa-users" style="height: 100%; width: 100%; cursor: pointer;"></i></label>
                                            <button type="button" id="btnStudent" class="btn btn-primary" Style="display: none; visibility: hidden;" data-toggle="modal" data-target="#studentModal">Launch demo modal</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Evaluators">Evaluated</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="lbleval" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <i class="fa-solid fa-list-check" style="height: 100%; width: 100%;"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <section class="mb-3">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-8">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="card-header text-center bg-transparent border-0">
                                                <h2>Calendar</h2>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <div id='calendar'></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-4">
                                    <div class="card mb-2">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col">
                                                    <center>
                                                        <h5>Click to Download Sample File for Faculty Member Upload</h5>
                                                    </center>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <p class="text-justify">
                                                        <b>Note</b>: Please don't forget to save the file as <b>.csv</b> format after inserting all of the information of the Faculty Members.
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <asp:GridView ID="GridView1" CssClass="table table-bordered table-striped" ShowHeader="false" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDownload" OnClick="lnkDownload_Click" runat="server" Text='<%# Bind("file_name") %>'
                                                                        CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card mb-2">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col">
                                                    <center>
                                                        <h5>Click to Download Sample File for Student Upload</h5>
                                                    </center>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <p class="text-justify">
                                                        <b>Note</b>: Please don't forget to save the file as <b>.csv</b> format after inserting all of the information of the Student.
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <asp:GridView ID="GridView2" CssClass="table table-bordered table-striped" ShowHeader="false" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDownload2" OnClick="lnkDownload2_Click" runat="server" Text='<%# Bind("file_name") %>'
                                                                        CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <section>
                    <asp:PlaceHolder ID="adminplc" runat="server">
                        <div class="card">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col">
                                        <div class="row mb-2">
                                            <div class="col">
                                                <center>
                                                    <h2>Primary Features For the Admin
                                                    </h2>
                                                </center>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-4">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../Pictures/upload%20users.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>User Management
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                The administrator is the one who manages the users (Faculty Members and Students) , either using bulk upload (in CSV file) or manual upload. In addition, the addministrator is the only one who can edit the information of the Faculty Members and Students since the administrator is the one holding the original institutional data of the users. 
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                            <div class="col-4">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/skills%20test%20icon.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Evaluation Management
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                The administrator of the institution is the one who manages the evaluation, either for its criteria, for the results of the evaluation or for the skills test of the faculty members. In addition, the administrator is the one controlling the evaluation period, either start the evaluation period or end the evaluation period. 
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                            <div class="col-4">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../Pictures/course.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Educational Management
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                The administrator can also manage educational terms that is connected in the institutions like the following: department, course, section, semesters, study load, and subjects. In this way the users will be all interconnected to each other.
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <div class="row">
                                            <div class="col-6">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../Pictures/profile%20icon.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Profile
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                Manage Profile, e.g. Updating Personal Information like name, profile picture and passwords.
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                            <div class="col-6">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../Pictures/notifications.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Notifications
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                Receive notifications from the system.
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </section>
                <div class="modal fade" id="modalPopUp" tabindex="-1" aria-labelledby="staticPopUpBackdropLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg modal-dialog-centered">
                            <div class="modal-content">
                                <div class="modal-header">

                                    <h5 class="modal-title" id="staticPopUpBackdropLabel">
                                        <img src="../Pictures/CAPSTONER-Amber.png" style="height: auto; width: 90px;" class="img-fluid mx-3" />Subscribe to AMBER:Faculty Evaluation System</h5>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col">
                                            <h2>You are using AMBER: Faculty Evaluation System in a Free Trial Mode</h2>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <p style="font-size: large;" class="text-justify">
                                                Subscribe to AMBER to unlock new features
                                            </p>
                                        </div>
                                    </div>

                                    <div class="row align-content-center">
                                        <div class="col">
                                            <label>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                    <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                    <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                </svg>
                                                Faculty Members / Students Record Bulk Upload
                                            </label>
                                        </div>
                                    </div>

                                    <div class="row align-content-center">
                                        <div class="col">
                                            <label>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                    <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                    <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                </svg>
                                                Add Another Admin from your School
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row align-content-center">
                                        <div class="col">
                                            <label>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                    <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                    <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                </svg>
                                                Category & Questions Bulk Upload
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row align-content-center">
                                        <div class="col">
                                            <label>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                    <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                    <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                </svg>
                                                Customize your School Profile
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Subscribe Later</button>
                                    <asp:LinkButton ID="LinkButton1" OnClick="btnSubscribeNow_Click" CssClass="btn btn-primary" runat="server">Subscribe Now</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                <asp:PlaceHolder ID="popupmodal" runat="server">
                    <div class="modal fade" id="SubscriptionModal" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg modal-dialog-centered">
                            <div class="modal-content">
                                <div class="modal-header">

                                    <h5 class="modal-title" id="staticBackdropLabel">
                                        <img src="../Pictures/CAPSTONER-Amber.png" style="height: auto; width: 90px;" class="img-fluid mx-3" />Subscribe to AMBER:Faculty Evaluation System</h5>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col">
                                            <h2>You are using AMBER: Faculty Evaluation System in a Free Trial Mode</h2>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <p style="font-size: large;" class="text-justify">
                                                Subscribe to AMBER to unlock new features
                                            </p>
                                        </div>
                                    </div>

                                    <div class="row align-content-center">
                                        <div class="col">
                                            <label>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                    <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                    <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                </svg>
                                                Faculty Members / Students Record Bulk Upload
                                            </label>
                                        </div>
                                    </div>

                                    <div class="row align-content-center">
                                        <div class="col">
                                            <label>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                    <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                    <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                </svg>
                                                Add Another Admin from your School
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row align-content-center">
                                        <div class="col">
                                            <label>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                    <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                    <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                </svg>
                                                Category & Questions Bulk Upload
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row align-content-center">
                                        <div class="col">
                                            <label>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                    <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                    <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                </svg>
                                                Customize your School Profile
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Subscribe Later</button>
                                    <asp:LinkButton ID="btnSubscribeNow" OnClick="btnSubscribeNow_Click" CssClass="btn btn-primary" runat="server">Subscribe Now</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <div class="modal fade" id="instructorModal" tabindex="-1" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
        <div class="modal-dialog modal-md modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Instructor Evaluated</h5>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col">
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class=" btn btn-secondary close" data-dismiss="modal" aria-label="Close">
                        close
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="studentModal" tabindex="-1" aria-labelledby="exampleModalLongTitle2" aria-hidden="true">
        <div class="modal-dialog modal-md modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle2">Student Evaluated</h5>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col">
                            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class=" btn btn-secondary close" data-dismiss="modal" aria-label="Close">
                        close
                    </button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //$(window).load(function () {
        //    if (!localStorage.getItem('firstLoad')) {
        //        localStorage['firstLoad'] = false;
        //        $('#EvaluateRate').modal('show');
        //    }
        //    else
        //        localStorage.removeItem('firstLoad');
        //});
        $(window).on('load', function () {
            if (!sessionStorage.getItem('shown-modal')) {
                $('#SubscriptionModal').modal('show');
                sessionStorage.setItem('shown-modal', 'true');
            }
        });
    </script>
</asp:Content>
