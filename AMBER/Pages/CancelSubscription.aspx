<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelSubscription.aspx.cs" Inherits="AMBER.Pages.CancelSubscription" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMBER - Contact Admin</title>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="../../Content/Sidebar/sidebarst.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript">
        function SubmittedSuccessful() {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Response Submitted Successfully!',
                text: 'Cancellation of Subscription Susccessful!',
                showConfirmButton: false,
                timer: 3000
            }).then(function () {
                window.location = "AdminLandingPage.aspx";
            });
        }
        function ErrorReason() {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'Input Valid Reason!',
                showConfirmButton: false,
                timer: 3000
            })
        }
        function ErrorSchool() {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'Select School!',
                showConfirmButton: false,
                timer: 3000
            })
        }
    </script>
    <style type="text/css">
        .radioButtonList {
            list-style: none;
            margin: 0;
            padding: 0;
        }

            .radioButtonList.horizontal li {
                display: inline;
            }

            .radioButtonList label {
                display: inline;
                margin: 7px;
                padding: 2px
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid" style="background-image: url(../Pictures/1.png); background-size: cover">
            <div class="row vh-100">
                <%-- L O G O --%>
                <div class="col-lg-5 mx-auto p-2 m-2">
                    <div class="card">
                        <div class="card-header text-center bg-transparent border-0">
                            <img src="../../Pictures/AMBERLOGO1.png" class="img-fluid" />
                        </div>
                        <div class="card-body bg-transparent">
                            <div class="row">
                                <div class="col">
                                    <hr />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <div class="form-floating">
                                        <h5 class="text-bold">Reason for Cancellation
                                        </h5>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:RadioButtonList ID="rblReason" CssClass="radioButtonList" runat="server">
                                        <asp:ListItem Value="value1">I want to change my subscription plan</asp:ListItem>
                                        <asp:ListItem Value="value2">I don't like to subscribe</asp:ListItem>
                                        <asp:ListItem Value="value3">I don't want to use AMBER</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <div class="form-floating mb-3">
                                        <asp:TextBox ID="txtotherreason" CssClass="form-control border-0 border-bottom" runat="server"></asp:TextBox>
                                        <label for="txtotherreason">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-body-text" viewBox="0 0 16 16">
                                                <path fill-rule="evenodd" d="M0 .5A.5.5 0 0 1 .5 0h4a.5.5 0 0 1 0 1h-4A.5.5 0 0 1 0 .5Zm0 2A.5.5 0 0 1 .5 2h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5Zm9 0a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5Zm-9 2A.5.5 0 0 1 .5 4h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1-.5-.5Zm5 0a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5Zm7 0a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1-.5-.5Zm-12 2A.5.5 0 0 1 .5 6h6a.5.5 0 0 1 0 1h-6a.5.5 0 0 1-.5-.5Zm8 0a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5Zm-8 2A.5.5 0 0 1 .5 8h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5Zm7 0a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5Zm-7 2a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 0 1h-8a.5.5 0 0 1-.5-.5Zm0 2a.5.5 0 0 1 .5-.5h4a.5.5 0 0 1 0 1h-4a.5.5 0 0 1-.5-.5Zm0 2a.5.5 0 0 1 .5-.5h2a.5.5 0 0 1 0 1h-2a.5.5 0 0 1-.5-.5Z" />
                                            </svg>
                                            Other Reason, Please Specify..
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="d-grid gap-2 col-8 mx-auto pb-3">
                            <asp:Button ID="btnsubmit" class="btn btn-primary" runat="server" ValidationGroup="RqSubmit" OnClick="btnsubmit_Click" Text="Submit" />
                        </div>
                        <%-- FORGOT PASSWORD SEND EMAIL --%>

                        <div class="d-flex justify-content-center links">
                           
                            <asp:LinkButton ID="adminprofilebtn" OnClick="adminprofilebtn_Click" CssClass="btn btn-warning border-0 bg-transparent" style="text-decoration: underline" runat="server">Go back to Profile</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.min.js" integrity="sha384-IDwe1+LCz02ROU9k972gdyvl+AESN10+x7tBKgc9I5HFtuNz0wWnPclzo6p9vxnk" crossorigin="anonymous"></script>
</body>
</html>
