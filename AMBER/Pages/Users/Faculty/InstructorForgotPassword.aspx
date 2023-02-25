<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstructorForgotPassword.aspx.cs" Inherits="AMBER.Pages.Users.Faculty.InstructorForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMBER - Forgot Password</title>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../../Scripts/customscrollbar.css" rel="stylesheet" />
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous"/>
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="/../../../Content/Sidebar/sidebarst.css" rel="stylesheet" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
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
        function UpdateSuccess() {
            Swal.fire({
                position: 'center',
                showConfirmButton: false,
                timer: 3000,
                icon: 'success',
                title: 'Password Updated Successfully.',
                text: 'Go to Login Now!',
            }).then(function () {
                window.location = "/Pages/Users/UsersLogin.aspx";
            });
        }
        function signupSuccess() {
            var name = '<%= Session["welcome"] %>';
            Swal.fire({
                icon: 'success',
                title: 'User Successfuly Signed up',
                text: 'Welcome ' + name,
            }).then(function () {
                window.location = "LoginPage.aspx";
            });
        }
        function notfound() {
            Swal.fire({
                position: 'center',
                showConfirmButton: false,
                timer: 3000,
                icon: 'question',
                title: 'User not found',
            })
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid min-vh-100 m-0 d-flex flex-column justify-content-center bg-transparent" style="background-image: url('/../../../Pictures/wave.svg'); background-repeat: no-repeat; background-position: bottom">
            <div class="row">
                <%-- L O G O --%>
                <div class="col-lg-4 mx-auto p-0 m-0">
                    <div class="card">
                        <div class="card-header text-center bg-transparent border-0">
                            <img src="/../../Pictures/AMBERLOGO1.png" class="img-fluid" />
                        </div>
                        <div class="card-body bg-transparent">
                            <asp:PlaceHolder ID="newpassplc" runat="server">
                                <div class="form-group">
                                    
                                    <div class="input-group mb-3">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtnewpassword" TextMode="Password" CssClass="form-control" runat="server">
                                            </asp:TextBox>
                                            <label for="txtnewpassword">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-lock" viewBox="0 0 16 16">
                                                    <path d="M5.338 1.59a61.44 61.44 0 0 0-2.837.856.481.481 0 0 0-.328.39c-.554 4.157.726 7.19 2.253 9.188a10.725 10.725 0 0 0 2.287 2.233c.346.244.652.42.893.533.12.057.218.095.293.118a.55.55 0 0 0 .101.025.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067c-.53 0-1.552.223-2.662.524zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
                                                    <path d="M9.5 6.5a1.5 1.5 0 0 1-1 1.415l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99a1.5 1.5 0 1 1 2-1.415z" />
                                                </svg>
                                                Password<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqResetPass" runat="server" ControlToValidate="txtnewpassword" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                        </div>
                                        <button id="show_password" class="btn btn-primary" type="button">
                                            <span class="fa fa-eye icon"></span>
                                        </button>
                                    </div>
                                    <div class="input-group mb-3">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtconfirmpass" TextMode="Password" CssClass="form-control" runat="server">
                                            </asp:TextBox>
                                            <label for="txtconfirmpass">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-lock" viewBox="0 0 16 16">
                                                    <path d="M5.338 1.59a61.44 61.44 0 0 0-2.837.856.481.481 0 0 0-.328.39c-.554 4.157.726 7.19 2.253 9.188a10.725 10.725 0 0 0 2.287 2.233c.346.244.652.42.893.533.12.057.218.095.293.118a.55.55 0 0 0 .101.025.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067c-.53 0-1.552.223-2.662.524zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
                                                    <path d="M9.5 6.5a1.5 1.5 0 0 1-1 1.415l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99a1.5 1.5 0 1 1 2-1.415z" />
                                                </svg>
                                                Password<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqResetPass" runat="server" ControlToValidate="txtconfirmpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                        </div>
                                        <button id="show_password1" class="btn btn-primary" type="button">
                                            <span class="fa fa-eye icon"></span>
                                        </button>
                                    </div>

                                </div>
                                <br />
                                <%-- L O G I N      B T N --%>
                                <div class="d-grid gap-2 col-8 mx-auto pb-1">
                                    <asp:Button ID="btnconfirm" OnClick="btnconfirm_Click" class="btn btn-primary" runat="server" ValidationGroup="RqResetPass" Text="Confirm" />
                                </div>
                                <%-- S I G N    U P     H Y P E R L I N K --%>
                                <div class="d-flex justify-content-center links">
                                    <asp:Label runat="server" Text="Already Done? Go to"></asp:Label>
                                    &nbsp;
                                <a href="/Pages/Users/UsersLogin.aspx">Login</a>
                                </div>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="expiredplc" runat="server">
                                <div class="row">
                                    <div class="col">
                                        <h2>The link you're trying to access is expired!</h2>
                                    </div>
                                </div>
                            </asp:PlaceHolder>
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
                $('#txtnewpassword').attr('type', 'text');
                $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
            },
                function () {
                    //Change the attribute back to password  
                    $('#txtnewpassword').attr('type', 'password');
                    $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                });
        });
        $(document).ready(function () {
            $('#show_password1').hover(function show() {
                //Change the attribute to text  
                $('#txtconfirmpass').attr('type', 'text');
                $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
            },
                function () {
                    //Change the attribute back to password  
                    $('#txtconfirmpass').attr('type', 'password');
                    $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                });
        });
    </script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.min.js" integrity="sha384-IDwe1+LCz02ROU9k972gdyvl+AESN10+x7tBKgc9I5HFtuNz0wWnPclzo6p9vxnk" crossorigin="anonymous"></script>
</body>
</html>
