<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactDevelopers.aspx.cs" Inherits="AMBER.Pages.ContactDevelopers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMBER - Contact Developers</title>
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
                title: 'Submitted Successfully!',
                text: 'Please try to Sign Up again after few moments',
                showConfirmButton: false,
                timer: 3000
            })
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
        <div class="jumbotron min-vh-100 m-0 d-flex flex-column justify-content-center bg-transparent" style="background-image: url('/../../Pictures/wave.svg'); background-repeat: no-repeat; background-position: bottom">
            <div class="row">
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
                                        <h5 class="text-bold">Input School Name
                                        </h5>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <div class="form-group">
                                        <div class="form-floating mb-3">
                                            <asp:TextBox ID="txtschoolName" CssClass="form-control" runat="server"></asp:TextBox>
                                            <label for="txtschoolName">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-bank2" viewBox="0 0 16 16">
                                                    <path d="M8.277.084a.5.5 0 0 0-.554 0l-7.5 5A.5.5 0 0 0 .5 6h1.875v7H1.5a.5.5 0 0 0 0 1h13a.5.5 0 1 0 0-1h-.875V6H15.5a.5.5 0 0 0 .277-.916l-7.5-5zM12.375 6v7h-1.25V6h1.25zm-2.5 0v7h-1.25V6h1.25zm-2.5 0v7h-1.25V6h1.25zm-2.5 0v7h-1.25V6h1.25zM8 4a1 1 0 1 1 0-2 1 1 0 0 1 0 2zM.5 15a.5.5 0 0 0 0 1h15a.5.5 0 1 0 0-1H.5z" />
                                                </svg>
                                                School Name
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqSubmit" runat="server" ControlToValidate="txtschoolName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="d-grid gap-2 col-8 mx-auto pb-3">
                            <asp:Button ID="btnLogin" class="btn btn-primary" runat="server" ValidationGroup="RqSubmit" Text="Submit" OnClick="btnLogin_Click" />
                        </div>
                        <%-- FORGOT PASSWORD SEND EMAIL --%>
                        <div class="d-flex justify-content-center links">
                            <asp:Label runat="server" Text="Go Back to"></asp:Label>
                            &nbsp;
                                <a href="/Pages/SignupPage.aspx">Sign Up</a>
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
