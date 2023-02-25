<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscriptionDetails.aspx.cs" Inherits="AMBER.Pages.SubscriptionDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMBER - Subscription Details</title>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../../Scripts/customscrollbar.css" rel="stylesheet" />
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="../../Content/Sidebar/sidebarst.css" rel="stylesheet" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style type="text/css">
        h3 {
            color: darkslategray;
        }

        .card {
            box-shadow: 0px 3px 5px black;
            height: 500px;
            max-height: 100%;
        }

            .card:hover {
                cursor: pointer;
                box-shadow: 0px 12px 30px 0px rgba(0, 0, 0, 0.2);
                transition: all 1s cubic-bezier(0.19, 1, 0.22, 1);
            }

        .btn:hover {
            cursor: pointer;
            transform: scale(1.1);
        }
    </style>
    <script>
        function error() {
            Swal.fire({
                position: 'center',
                showConfirmButton: false,
                timer: 3000,
                icon: 'error',
                title: 'Under Maintenance',
                text: 'Something went wrong!',
            })
        }
        function SendSuccess() {
            Swal.fire({
                position: 'center',
                showConfirmButton: false,
                timer: 3000,
                icon: 'success',
                title: 'Sent Successfully',
                text: 'Go to your email and check.',
            })
        }
        function notFound() {
            Swal.fire({
                position: 'center',
                icon: 'question',
                title: 'User Not Found',
                showConfirmButton: false,
                timer: 3000
            })
        }
        function Wrong() {
            Swal.fire({
                position: 'center',
                icon: 'question',
                title: 'Wrong Username or Password',
                text: 'Your account might get locked after 3 failed attempts!',
                showConfirmButton: false,
                timer: 3000
            })
        }
        function erroremail() {
            Swal.fire({
                position: 'center',
                icon: 'warning',
                title: 'Input your email!',
                showConfirmButton: false,
                timer: 3000
            })
        }
        function accountLocked() {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'Account Locked for 1 minute, please re-login later!',
                showConfirmButton: false,
                timer: 3000
            })
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light bg-light mb-0 p-1 shadow-sm bg-white">
            <a href="/Pages/AdminLandingPage.aspx" class="nav-link">
                <img src="../Pictures/CAPSTONER-Amber.png" style="height: auto; width: 90px;" class="img-fluid mx-3" /></a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="nav navbar-nav">
                    <li class="nav-item">
                        <a href="/Pages/AdminLandingPage.aspx" class="nav-link">Home</a>
                    </li>
                </ul>
            </div>
        </nav>
        <div class="container-fluid m-0 p-0">
            <section class="container-fluid m-0 w-100" style="background-color: white; width: 100%;">
                <div class="row pt-5">
                    <div class="col">
                        <center>
                            <h3 class="text-justify">Why Subscribe?
                            </h3>
                        </center>
                    </div>
                </div>
                <div class="row container mx-auto">
                    <div class="col-4">
                        <center>
                            <div class="row">
                                <div class="col">
                                    <img src="../Pictures/Admin.png" style="height: 150px; width: 150px;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <h5>Add Another Admin User
                                    </h5>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <p class="text-justify" style="color: darkslategray">
                                        Add another admin user from your school to manage.
                                    </p>
                                </div>
                            </div>
                        </center>
                    </div>
                    <div class="col-4">
                        <center>
                            <div class="row">
                                <div class="col">
                                    <img src="../Pictures/Multiple.png" style="height: 150px; width: 150px;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <h5>Record Upload
                                    </h5>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <p class="text-justify">
                                        You can upload combined user records and category/questions record in a CSV file in one click.
                                    </p>
                                </div>
                            </div>
                        </center>
                    </div>
                    <div class="col-4">
                        <center>
                            <div class="row">
                                <div class="col">
                                    <img src="../Pictures/School.png" style="height: 150px; width: 150px;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <h5>Customize Your School Profile
                                    </h5>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <p class="text-justify">
                                        You can customize your school profile i.e, adding school logo.
                                    </p>
                                </div>
                            </div>
                        </center>
                    </div>
                </div>
            </section>

            <section class="container-fluid w-100" style="background-color: #fcb455; width: 100%;">
                <div class="row pt-5">
                    <div class="col">
                        <center>
                            <h3 class="text-justify" style="color: #ffff">Choose Your Subscription Plan
                            </h3>
                        </center>
                    </div>
                </div>
                <div class="row container mx-auto pb-4">
                    <div class="col-4">
                        <div class="card">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col">
                                        <h3 class="text-justify">Mini Pack
                                        </h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <p class="text-justify" style="font-size: medium;">
                                            ₱13,000 / 6 Months
                                        </p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <p class="text-justify">
                                            Up to 5,000 Users.
                                        </p>
                                    </div>
                                </div>
                                <hr />
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                              <b style="font-size:large;">✔</b> Add Other Admin within the same school to manage
                                        </label>
                                    </div>
                                </div>
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                              <b style="font-size:large;">✔</b>5,000 users is the uploading limit
                                        </label>
                                    </div>
                                </div>
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                            <b style="font-size:large;">✔</b> Add User Records/Information and Add Categories & Questions in ONE CLICK!
                                        </label>
                                    </div>
                                </div>
                                <div class="row align-content-center" style="margin-bottom: 73px">
                                    <div class="col">
                                        <label>
                                            <b style="font-size:large;">✔</b>  Customize Your School profile
                                        </label>
                                    </div>
                                </div>
                                <div class="d-grid gap-2 col-8 mx-auto">
                                    <asp:LinkButton OnClick="soloprembtn_Click" ID="soloprembtn" CssClass="btn btn-primary" runat="server">
                                                Subscribe
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-4">
                        <div class="card">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col">
                                        <h3 class="text-justify">Medium Pack
                                        </h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <p class="text-justify" style="font-size: medium;">
                                            ₱15,000 / 6 Months
                                        </p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <p class="text-justify">
                                            Up to 7,000 users
                                        </p>
                                    </div>
                                </div>
                                <hr />
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                            
                                             <b style="font-size:large;">✔</b> Add Other Admin within the same school to manage
                                        </label>
                                    </div>
                                </div>
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                             <b style="font-size:large;">✔</b> 7,000 users is the uploading limit 
                                        </label>
                                    </div>
                                </div>
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                             <b style="font-size:large;">✔</b> Add User Records/Information and Add Categories & Questions in ONE CLICK!
                                        </label>
                                    </div>
                                </div>
                                 <div class="row align-content-center" style="margin-bottom: 68px">
                                    <div class="col">
                                        <label>
                                            <b style="font-size:large;">✔</b> Customize Your School Profile
                                        </label>
                                    </div>
                                </div>
                                <div class="d-grid gap-2 col-8 mx-auto pb-5">
                                    <asp:LinkButton OnClick="dualprembtn_Click" ID="dualprembtn" CssClass="btn btn-primary" runat="server">
                                                Subscribe
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-4">
                        <div class="card">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col">
                                        <h3 class="text-justify">Large Pack
                                        </h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <p class="text-justify" style="font-size: medium;">
                                            ₱20,000 / 6 Months
                                        </p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <p class="text-justify">
                                            Upload Unlimited users
                                        </p>
                                    </div>
                                </div>
                                <hr />
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                             <b style="font-size:large;">✔</b>  Add Other Admin within the same school to manage
                                        </label>
                                    </div>
                                </div>
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                             <b style="font-size:large;">✔</b> There is no uploading limit
                                        </label>
                                    </div>
                                </div>
                                <div class="row align-content-center">
                                    <div class="col">
                                        <label>
                                             <b style="font-size:large;">✔</b> Add User Records/Information and Add Categories & Questions in ONE CLICK!
                                        </label>
                                    </div>
                                </div>
                                 <div class="row align-content-center" style="margin-bottom:68px">
                                    <div class="col">
                                        <label>
                                            <b style="font-size:large;">✔</b> Customize Your School Profile
                                        </label>
                                    </div>
                                </div>
                                <div class="d-grid gap-2 col-8 mx-auto pb-5">
                                    <asp:LinkButton ID="packprembtn" OnClick="packprembtn_Click" CssClass="btn btn-primary" runat="server">
                                                Subscribe
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.6.1.min.js" integrity="sha256-o88AwQnZB+VDvE9tvIXrMQaPlFFSUTR+nldQm1LuPXQ=" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.min.js" integrity="sha384-IDwe1+LCz02ROU9k972gdyvl+AESN10+x7tBKgc9I5HFtuNz0wWnPclzo6p9vxnk" crossorigin="anonymous"></script>
    <script src="https://kit.fontawesome.com/288b588b0b.js" crossorigin="anonymous"></script>
</body>
</html>
