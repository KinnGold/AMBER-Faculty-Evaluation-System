<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TwoFactorAuthentication.aspx.cs" Inherits="AMBER.Super_Admin.TwoFactorAuthentication" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>AMBER - Verify Login</title>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../../Scripts/customscrollbar.css" rel="stylesheet" />
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous"/>
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
        function Verifyerror() {
            Swal.fire({
                icon: 'error',
                title: 'Invalid Authentication Code',
                text: 'Try Again.',
            })
        }
    </script>
</head>
<body>
        <form id="form1" runat="server">
        <div class="container-fluid min-vh-100 m-0 d-flex flex-column justify-content-center bg-transparent" style="background-image: url('../../Pictures/wave.svg'); background-repeat: no-repeat; background-position: bottom">
            <div class="row">
                <div class="col-lg-5 mx-auto">
                    <div class="card">
                        <div class="card-header text-center bg-transparent border-0">
                            <img src="../Pictures/AMBERLOGO1.png" class="img-fluid" />
                        </div>
                        <div class="card-body bg-transparent">
                            <asp:PlaceHolder ID="plcinput" runat="server">
                                <div class="form-group">
                                <div class="row">
                                    <div class="col mx-auto">
                                        <div class="form-floating mb-3">
                                            <asp:TextBox ID="txtcode" CssClass="form-control" runat="server"></asp:TextBox>
                                            <label for="txtcode">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-123" viewBox="0 0 16 16">
                                                    <path d="M2.873 11.297V4.142H1.699L0 5.379v1.137l1.64-1.18h.06v5.961h1.174Zm3.213-5.09v-.063c0-.618.44-1.169 1.196-1.169.676 0 1.174.44 1.174 1.106 0 .624-.42 1.101-.807 1.526L4.99 10.553v.744h4.78v-.99H6.643v-.069L8.41 8.252c.65-.724 1.237-1.332 1.237-2.27C9.646 4.849 8.723 4 7.308 4c-1.573 0-2.36 1.064-2.36 2.15v.057h1.138Zm6.559 1.883h.786c.823 0 1.374.481 1.379 1.179.01.707-.55 1.216-1.421 1.21-.77-.005-1.326-.419-1.379-.953h-1.095c.042 1.053.938 1.918 2.464 1.918 1.478 0 2.642-.839 2.62-2.144-.02-1.143-.922-1.651-1.551-1.714v-.063c.535-.09 1.347-.66 1.326-1.678-.026-1.053-.933-1.855-2.359-1.845-1.5.005-2.317.88-2.348 1.898h1.116c.032-.498.498-.944 1.206-.944.703 0 1.206.435 1.206 1.07.005.64-.504 1.106-1.2 1.106h-.75v.96Z" />
                                                </svg>
                                                Authentication Code
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqVerify" runat="server" ControlToValidate="txtcode" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="d-grid gap-2 col-8 mx-auto pb-1">
                                    <asp:Button ID="btnverifynow" class="btn btn-primary" OnClick="btnverifynow_Click" runat="server" ValidationGroup="RqVerify" Text="Verify" />
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
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.min.js" integrity="sha384-IDwe1+LCz02ROU9k972gdyvl+AESN10+x7tBKgc9I5HFtuNz0wWnPclzo6p9vxnk" crossorigin="anonymous"></script>
</body>
</html>
