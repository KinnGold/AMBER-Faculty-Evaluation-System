<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sample.aspx.cs" Inherits="AMBER.Pages.Users.Sample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMBER - Users Login</title>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../../Scripts/customscrollbar.css" rel="stylesheet" />
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="../../Content/Sidebar/sidebarst.css" rel="stylesheet" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function error() {
            Swal.fire({
                icon: 'error',
                title: 'Under Maintenance',
                text: 'Something went wrong!',
            })
        }
        function SendSuccess() {
            Swal.fire({
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
                timer: 1500
            })
        }
        function Wrong() {
            Swal.fire({
                position: 'center',
                icon: 'question',
                title: 'Wrong Username or Password',
                text: 'Your account might get locked after 3 failed attempts!',
                showConfirmButton: false,
                timer: 1500
            })
        }
        function erroremail() {
            Swal.fire({
                position: 'center',
                icon: 'warning',
                title: 'Input your email!',
                showConfirmButton: false,
                timer: 1500
            })
        }
        function accountLocked() {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'Account Locked for 1 minute, please re-login later!',
                showConfirmButton: false,
                timer: 1500
            })
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid min-vh-100 m-0 d-flex flex-column justify-content-center bg-transparent" style="background-image: url('../../Pictures/wave.svg'); background-repeat: no-repeat; background-position: bottom">
            <div class="row">
                <%-- L O G O --%>
                <div class="col-lg-4 mx-auto p-0 m-0">
                    <div class="card">
                        <div class="card-header text-center bg-transparent border-0">
                            <a href="/Pages/Amber.aspx">
                                <img src="../../Pictures/AMBERLOGO1.png" class="img-fluid" /></a>
                            <div class="form-floating mx-auto">
                                <h4>Sign in to your AMBER Account</h4>
                            </div>
                        </div>
                        <div class="card-body">
                            <nav class="nav nav-tabs">
                                <button type="button" class="nav-link active" data-bs-toggle="tab" data-bs-target="#instructorLogin">Faculty Member Sign In</button>
                                <button type="button" class="nav-link" data-bs-toggle="tab" data-bs-target="#studentLogin">Student Sign In</button>
                            </nav>
                            <div class="tab-content">

                                <!-- FACULTY MEMBER LOGIN TAB PANEL -->
                                <div class="tab-pane active show fade" id="instructorLogin">
                                    <div class="form-group">
                                        <div class="form-floating m-2">
                                            <center>
                                                <h5>Sign In as Faculty Member
                                                </h5>
                                            </center>
                                        </div>
                                        <div class="form-floating mb-3">
                                            <asp:TextBox ID="txtinsnum" CssClass="form-control" runat="server"></asp:TextBox>
                                            <label for="txtinsnum">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-person-circle" viewBox="0 0 16 16">
                                                    <path d="M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z" />
                                                    <path fill-rule="evenodd" d="M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z" />
                                                </svg>
                                                ID Number
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqLogin" runat="server" ControlToValidate="txtinsnum" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                        </div>
                                        <div class="input-group mb-3">
                                            <div class="form-floating">
                                                <asp:TextBox ID="txtinspass" TextMode="Password" CssClass="form-control" runat="server">
                                                </asp:TextBox>
                                                <label for="txtinspass">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-lock" viewBox="0 0 16 16">
                                                        <path d="M5.338 1.59a61.44 61.44 0 0 0-2.837.856.481.481 0 0 0-.328.39c-.554 4.157.726 7.19 2.253 9.188a10.725 10.725 0 0 0 2.287 2.233c.346.244.652.42.893.533.12.057.218.095.293.118a.55.55 0 0 0 .101.025.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067c-.53 0-1.552.223-2.662.524zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
                                                        <path d="M9.5 6.5a1.5 1.5 0 0 1-1 1.415l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99a1.5 1.5 0 1 1 2-1.415z" />
                                                    </svg>
                                                    Password<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqLogin" runat="server" ControlToValidate="txtinspass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                            <button id="show_password" class="btn btn-primary" type="button">
                                                <span class="fa fa-eye icon"></span>
                                            </button>
                                        </div>
                                        <div class="form-floating">
                                            <div class="d-grid gap-2 col-8 mx-auto mb-2">
                                                <asp:Button ID="btnInsLogin" class="btn btn-primary" runat="server" ValidationGroup="RqLogin" Text="Sign In" />
                                            </div>
                                        </div>
                                        <div class="form-floating">
                                            <div class="row">
                                                <div class="col">
                                                    <div class="d-grid gap-2 col-8 mx-auto">
                                                        <a class="btn btn-warning mx-auto border-0 bg-transparent text-info" data-bs-toggle="modal" data-bs-target="#forgotPassModal">Forgot Password?</a>
                                                    </div>
                                                    <div class="modal fade" id="forgotPassModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel1" aria-hidden="true">
                                                        <div class="modal-dialog modal-dialog-centered">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <h5 class="modal-title" id="staticBackdropLabel1">Send to Email</h5>
                                                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="form-group">
                                                                        <div class="form-floating mb-3">
                                                                            <asp:TextBox ID="txtemail" CssClass="form-control" runat="server"></asp:TextBox>
                                                                            <label for="txtemail">
                                                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-card-heading" viewBox="0 0 16 16">
                                                                                    <path d="M14.5 3a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-13a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5h13zm-13-1A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h13a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-13z" />
                                                                                    <path d="M3 8.5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zm0 2a.5.5 0 0 1 .5-.5h6a.5.5 0 0 1 0 1h-6a.5.5 0 0 1-.5-.5zm0-5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-9a.5.5 0 0 1-.5-.5v-1z" />
                                                                                </svg>
                                                                                Email<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqSendEmail" runat="server" ControlToValidate="txtemail" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                            </label>
                                                                        </div>
                                                                        <div class="form-floating">
                                                                            <p>
                                                                                Didn't receive the link? Click here to
                                                            <asp:LinkButton ID="insresendemail" CssClass="border-0 bg-transparent text-decoration-underline" runat="server">Re-send</asp:LinkButton>.
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                    <asp:Button ID="inssendemail" CssClass="btn btn-warning" ValidationGroup="RqSendEmail" runat="server" Text="Send" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-floating">
                                            <div class="d-flex justify-content-center links">
                                                <asp:Label runat="server" Text="Cannot Login?"></asp:Label>
                                                &nbsp;
                                <a href="/Pages/Users/ContactAdmin.aspx">Contact Your Admin</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <!-- STUDENT LOGIN TAB PANEL -->
                                <div class="tab-pane fade" id="studentLogin">
                                    <div class="form-group">
                                        <div class="form-floating m-2">
                                            <center>
                                                <h5>Sign In as Student
                                                </h5>
                                            </center>
                                        </div>
                                        <div class="form-floating mb-3">
                                            <asp:TextBox ID="txtstudnum" CssClass="form-control" runat="server"></asp:TextBox>
                                            <label for="txtstudnum">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-person-circle" viewBox="0 0 16 16">
                                                    <path d="M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z" />
                                                    <path fill-rule="evenodd" d="M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z" />
                                                </svg>
                                                ID Number
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqLogin" runat="server" ControlToValidate="txtstudnum" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                        </div>
                                        <div class="input-group mb-3">
                                            <div class="form-floating">
                                                <asp:TextBox ID="txtstudpass" TextMode="Password" CssClass="form-control" runat="server">
                                                </asp:TextBox>
                                                <label for="txtstudpass">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-lock" viewBox="0 0 16 16">
                                                        <path d="M5.338 1.59a61.44 61.44 0 0 0-2.837.856.481.481 0 0 0-.328.39c-.554 4.157.726 7.19 2.253 9.188a10.725 10.725 0 0 0 2.287 2.233c.346.244.652.42.893.533.12.057.218.095.293.118a.55.55 0 0 0 .101.025.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067c-.53 0-1.552.223-2.662.524zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
                                                        <path d="M9.5 6.5a1.5 1.5 0 0 1-1 1.415l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99a1.5 1.5 0 1 1 2-1.415z" />
                                                    </svg>
                                                    Password<asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" ValidationGroup="RqLogin" runat="server" ControlToValidate="txtstudpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                            </div>
                                            <button id="show_password1" class="btn btn-primary" type="button">
                                                <span class="fa fa-eye icon"></span>
                                            </button>
                                        </div>
                                        <div class="form-floating">
                                            <div class="d-grid gap-2 col-8 mx-auto mb-2">
                                                <asp:Button ID="studbtnlogin" class="btn btn-primary" runat="server" ValidationGroup="RqLogin" Text="Sign In" />
                                            </div>
                                        </div>
                                        <div class="form-floating">
                                            <div class="row">
                                                <div class="col">
                                                    <div class="d-grid gap-2 col-8 mx-auto">
                                                        <a class="btn btn-warning mx-auto border-0 bg-transparent text-info" data-bs-toggle="modal" data-bs-target="#forgotPassModal1">Forgot Password?</a>
                                                    </div>
                                                    <div class="modal fade" id="forgotPassModal1" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel2" aria-hidden="true">
                                                        <div class="modal-dialog modal-dialog-centered">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <h5 class="modal-title" id="staticBackdropLabel2">Send to Email</h5>
                                                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="form-group">
                                                                        <div class="form-floating mb-3">
                                                                            <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server"></asp:TextBox>
                                                                            <label for="txtemail">
                                                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-card-heading" viewBox="0 0 16 16">
                                                                                    <path d="M14.5 3a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-13a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5h13zm-13-1A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h13a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-13z" />
                                                                                    <path d="M3 8.5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zm0 2a.5.5 0 0 1 .5-.5h6a.5.5 0 0 1 0 1h-6a.5.5 0 0 1-.5-.5zm0-5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-9a.5.5 0 0 1-.5-.5v-1z" />
                                                                                </svg>
                                                                                Email<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="RqSendEmail" runat="server" ControlToValidate="txtemail" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                            </label>
                                                                        </div>
                                                                        <div class="form-floating">
                                                                            <p>
                                                                                Didn't receive the link? Click here to
                                                            <asp:LinkButton ID="studresendemail" CssClass="border-0 bg-transparent text-decoration-underline" runat="server">Re-send</asp:LinkButton>.
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                    <asp:Button ID="studsendemail" CssClass="btn btn-warning" ValidationGroup="RqSendEmail" runat="server" Text="Send" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-floating">
                                            <div class="d-flex justify-content-center links">
                                                <asp:Label runat="server" Text="Cannot Login?"></asp:Label>
                                                &nbsp;
                                <a href="/Pages/Users/ContactAdmin.aspx">Contact Your Admin</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
    <script src="https://code.jquery.com/jquery-3.6.1.min.js" integrity="sha256-o88AwQnZB+VDvE9tvIXrMQaPlFFSUTR+nldQm1LuPXQ=" crossorigin="anonymous"></script>
    <script type="text/javascript">  
        $(document).ready(function () {
            $('#show_password').hover(function show() {
                //Change the attribute to text  
                $('#txtinspass').attr('type', 'text');
                $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
            },
                function () {
                    //Change the attribute back to password  
                    $('#txtinspass').attr('type', 'password');
                    $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                });
        });

        $(document).ready(function () {
            $('#show_password1').hover(function show() {
                //Change the attribute to text  
                $('#txtstudpass').attr('type', 'text');
                $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
            },
                function () {
                    //Change the attribute back to password  
                    $('#txtstudpass').attr('type', 'password');
                    $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                });
        });
    </script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.min.js" integrity="sha384-IDwe1+LCz02ROU9k972gdyvl+AESN10+x7tBKgc9I5HFtuNz0wWnPclzo6p9vxnk" crossorigin="anonymous"></script>
    <script src="https://kit.fontawesome.com/288b588b0b.js" crossorigin="anonymous"></script>
</body>
</html>
