<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscriptionExpired.aspx.cs" Inherits="AMBER.Pages.SubscriptionExpired" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AMBER - Subscription Expired</title>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../../Scripts/customscrollbar.css" rel="stylesheet" />
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="../../Content/Sidebar/sidebarst.css" rel="stylesheet" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        var page = '<%= Session["temp"] %>';
        var obj = { status: false, ele: null };
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light bg-light mb-0">
            <img src="../Pictures/amberlogo.png" style="height: auto; width: 100px;" class="mx-3" />
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
        </nav>
        <div class="container">
            <div class="row">
                <div class="col">
                    <center>
                        <h3>Your Subscription Plan has expired! Please Renew your Subscription.</h3>
                        <asp:Image ID="Image1" ImageUrl="../Pictures/error.gif" runat="server" />
                    </center>
                </div>
            </div>
            <div class="row">
                <center>
                    <div class="col-6">
                        <asp:LinkButton ID="btnLater" OnClick="btnLater_Click" CssClass="btn btn-secondary" runat="server">Renew Later</asp:LinkButton>
                        <asp:LinkButton ID="btnRenew" OnClick="btnRenew_Click" CssClass="btn btn-primary" runat="server">Renew Now</asp:LinkButton>
                    </div>
                </center>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.min.js" integrity="sha384-IDwe1+LCz02ROU9k972gdyvl+AESN10+x7tBKgc9I5HFtuNz0wWnPclzo6p9vxnk" crossorigin="anonymous"></script>
    <script src="https://kit.fontawesome.com/288b588b0b.js" crossorigin="anonymous"></script>
</body>
</html>
