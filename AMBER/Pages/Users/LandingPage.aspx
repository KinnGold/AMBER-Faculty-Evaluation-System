<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="AMBER.BM.Users.LandingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
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
                                                <asp:Label ID="lbluser" Font-Bold="true" runat="server" Text="Label"></asp:Label>
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
                        <div class="col">
                            <div id="carouselExampleIndicators" class="carousel slide" data-bs-ride="carousel">
                                <div class="carousel-indicators">
                                    <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="0" class="active" ariacurrent="true" aria-label="Slide 1"></button>
                                    <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="1" aria-label="Slide 2"></button>
                                    <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="2" aria-label="Slide 3"></button>
                                    <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="3" aria-label="Slide 4"></button>
                                </div>
                                <div class="carousel-inner">
                                    <div class="carousel-item active">
                                        <img src="../../../../Pictures/evaluation.jpg" height="470" class="d-block w-100" />
                                    </div>
                                    <div class="carousel-item">
                                        <img src="../../../../Pictures/evaluation1.jpg" height="470" class="d-block w-100" />
                                    </div>
                                    <div class="carousel-item">
                                        <img src="../../../../Pictures/evaluation2.jpg" height="470" class="d-block w-100" />
                                    </div>
                                    <div class="carousel-item">
                                        <img src="../../../../Pictures/evaluation3.jpg" height="470" class="d-block w-100" />
                                    </div>
                                </div>
                                <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev">
                                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Previous</span>
                                </button>
                                <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next">
                                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Next</span>
                                </button>
                            </div>
                        </div>
                    </div>
                </section>

                <section>
                    <asp:PlaceHolder ID="instructorplc" runat="server">
                        <div class="card">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col">
                                        <div class="row mb-2">
                                            <div class="col">
                                                <center>
                                                    <h2>Primary Features For Faculty Users
                                                    </h2>
                                                </center>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-4">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/self-evaluation%20icon.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Self - Evaluation
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                Self-evaluate to ehance one's skills to promote self-development.
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                            <div class="col-4">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/peer-to-peer%20icon.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Peer-to-Peer Evaluation
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                Evaluate colleague based on his/her performance in the workplace.
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
                                                            <h5>Skills Test
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                Conducting Skills Test if an Instructor receives low evaluation results.
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
                                            <div class="col-3">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/results%20icon.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Results
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                View Results of Evaluation, i.e. Student to Faculty Evaluation, Self-Evaluation, Peer-to-Peer Evaluation, and Skills Test.
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                            <div class="col-3">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/rankings%20icon.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Rankings
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                View Rankings of Faculty Members based on the results of evaluation.
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                            <div class="col-3">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/profile%20icon.png" style="height: 150px; width: 150px;" />
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
                                            <div class="col-3">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/notifications.png" style="height: 150px; width: 150px;" />
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
                                                                Receive notifications on the system.
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

                    <asp:PlaceHolder ID="studentplc" runat="server">
                        <div class="card">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col">
                                        <div class="row mb-2">
                                            <div class="col">
                                                <center>
                                                    <h2>Primary Features For Student Users
                                                    </h2>
                                                </center>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-4">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/student-faculty%20icon.png" style="height: 150px; width: 150px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <h5>Faculty Member Evaluation
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-justify">
                                                                Evaluate a faculty member based on what he/she performs in the class.
                                                            </p>
                                                        </div>
                                                    </div>
                                                </center>
                                            </div>
                                            <div class="col-4">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/profile%20icon.png" style="height: 150px; width: 150px;" />
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
                                            <div class="col-4">
                                                <center>
                                                    <div class="row">
                                                        <div class="col">
                                                            <img src="../../Pictures/notifications.png" style="height: 150px; width: 150px;" />
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
                                                                Receive notifications on the system.
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
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        <div class="modal fade" id="SubscriptionModal" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-lg modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">

                                        <h5 class="modal-title" id="staticBackdropLabel">
                                            <img src="../../Pictures/CAPSTONER-Amber.png" style="height: auto; width: 90px;" class="img-fluid mx-3" />Welcome to AMBER:Faculty Evaluation System</h5>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col">
                                                <h4 style="text-align: justify;">Since you are a new user, we advice you to please do</h4>
                                            </div>
                                        </div>

                                        <div class="row align-content-center">
                                            <div class="col">
                                                <label>
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check2-square" viewBox="0 0 16 16">
                                                        <path d="M3 14.5A1.5 1.5 0 0 1 1.5 13V3A1.5 1.5 0 0 1 3 1.5h8a.5.5 0 0 1 0 1H3a.5.5 0 0 0-.5.5v10a.5.5 0 0 0 .5.5h10a.5.5 0 0 0 .5-.5V8a.5.5 0 0 1 1 0v5a1.5 1.5 0 0 1-1.5 1.5H3z" />
                                                        <path d="m8.354 10.354 7-7a.5.5 0 0 0-.708-.708L8 9.293 5.354 6.646a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0z" />
                                                    </svg>
                                                    Change your password
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
                                                    Check your profile
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
                                                    Update your profile
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                        <asp:LinkButton ID="goToProfile" OnClick="goToProfile_Click" class="btn btn-primary" runat="server">Go To Profile</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </section>
            </div>
        </div>
    </div>

    <script>
        $('.carouselExampleIndicators').carousel({
            interval: 3000
        })
    </script>
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
