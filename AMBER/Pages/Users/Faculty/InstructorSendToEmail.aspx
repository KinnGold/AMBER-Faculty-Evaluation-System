<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstructorSendToEmail.aspx.cs" Inherits="AMBER.Pages.Users.Faculty.InstructorSendToEmail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMBER - Send Code</title>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../../Scripts/customscrollbar.css" rel="stylesheet" />
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous">
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="/../../../Content/Sidebar/sidebarst.css" rel="stylesheet" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function error() {
            Swal.fire({
                icon: 'error',
                title: 'Under Maintenance',
                text: 'Something went wrong!',
            })
        }
        function UpdateSuccess() {
            Swal.fire({
                icon: 'success',
                title: 'Password Updated Successfully.',
                text: 'Go to Login Now!',
            })
        }
        function SendSuccess() {
            Swal.fire({
                icon: 'success',
                title: 'Sent Successfully.',
                text: 'Go check your link on your email.',
            })
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

                            <div class="form-group">
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtemail" CssClass="form-control" runat="server"></asp:TextBox>
                                    <label for="txtemail">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-card-heading" viewBox="0 0 16 16">
                                            <path d="M14.5 3a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-13a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5h13zm-13-1A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h13a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-13z" />
                                            <path d="M3 8.5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zm0 2a.5.5 0 0 1 .5-.5h6a.5.5 0 0 1 0 1h-6a.5.5 0 0 1-.5-.5zm0-5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-9a.5.5 0 0 1-.5-.5v-1z" />
                                        </svg>
                                        Email<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqSendEmail" runat="server" ControlToValidate="txtemail" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-floating">
                                                        <p>Didn't receive the link? Click here to <asp:LinkButton ID="resendemail" CssClass="border-0 bg-transparent text-decoration-underline" OnClick="resendemail_Click" runat="server">Re-send</asp:LinkButton>.</p>
                                                    </div>
                            </div>
                            <br />
                            <%-- L O G I N      B T N --%>
                            <div class="d-grid gap-2 col-8 mx-auto pb-1">
                                <asp:Button ID="btnsendemail" OnClick="btnsendemail_Click" class="btn btn-primary" runat="server" ValidationGroup="RqSendEmail" Text="Send" />
                            </div>

                        </div>
                         <div class="d-flex justify-content-center links">
                            <asp:Label runat="server" Text="Already Done? Go to"></asp:Label>
                            &nbsp;
                                <a href="/Pages/Users/UsersLogin.aspx" class="text-decoration-underline">Login</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
    <script src="https://code.jquery.com/jquery-3.6.1.min.js" integrity="sha256-o88AwQnZB+VDvE9tvIXrMQaPlFFSUTR+nldQm1LuPXQ=" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.min.js" integrity="sha384-IDwe1+LCz02ROU9k972gdyvl+AESN10+x7tBKgc9I5HFtuNz0wWnPclzo6p9vxnk" crossorigin="anonymous"></script>
</body>
</html>
