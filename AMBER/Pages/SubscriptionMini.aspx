<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscriptionMini.aspx.cs" Inherits="AMBER.Pages.SubscriptionSolo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Subscribe to AMBER</title>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-u1OknCvxWvY5kfmNBILK2hRnQC3Pr17a+RTT6rIHI7NnikvbZlHgTPOOmMi466C8" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/js/bootstrap.min.js" integrity="sha384-7VPbUDkoPSGFnVtYi0QogXtr74QeVeeIs99Qfg5YCF+TidwNdjvaKZX19NZ/e6oz" crossorigin="anonymous"></script>
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="../../Content/Sidebar/sidebarstyle.css" rel="stylesheet" />
     <script src="https://kit.fontawesome.com/288b588b0b.js" crossorigin="anonymous"></script>
</head>
<body>
    <form id="form1" runat="server">
            <div class="container-fluid" style="background-image: url(../Pictures/1.png); background-size: cover">
                <div class="row">
                    <div class="col align-self-center">
                        <div class="alert alert-dismissible alert-secondary align-content-center p-5" style="background-color: transparent; border: hidden;">
                            <a href="/Pages/SubscriptionDetails.aspx" style="font-size:large;color:darkslategrey; font-weight:bold;"><i class="fa-solid fa-arrow-left"></i> Back</a>
                            <img src="../Pictures/AMBERLOGO1.png" class="img-fluid" />
                        </div>
                    </div>
                    <div class="col align-self-center">
                        <div class="alert alert-dismissible alert-secondary align-content-center p-5" style="background-color: transparent; border: hidden;">
                            <h1 class="font-weight-bolder">Mini Pack Subscription Plan</h1>
                            <div class="row">
                                <div class="col">
                                    <h5>₱<strong class="font-weight-bolder"> 13000.00</strong>/ 6 Months</h5>
                                    <h5><b style="font-size:large;">✔</b> Add another Admin within the same school to manage</h5>
                                    <h5><b style="font-size:large;">✔</b> 5,000 users is the uploading limit</h5>
                                    <h5><b style="font-size:large;">✔</b> Add User Records/Information and Add Categories & Questions in ONE CLICK!</h5>
                                    <h5><b style="font-size:large;">✔</b> Customize your School Profile</h5>
                                </div>
                            </div>



                            <%--<button type="button" class="btn btn-lg btn-warning m-3" data-bs-toggle="modal" data-bs-target="#subscribeModal">
                          <strong>Subscribe</strong>
                        </button>--%>
                            <div class="container pt-4 px-0">
                                <div id="paypal-button-container"></div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
    </form>
    <script type="text/javascript" src="https://www.paypal.com/sdk/js?client-id=ARcKasaBuNUiGJGiMNJs-WgUFmG5B3hEmsvYtZDs1UiPF20zFXQo1NBLJKEldocuJ9vyzjjQcz_yn9vf&currency=PHP">
    </script>
    <script>
        var name = '<%= Session["fname"] %>';
        var lname = '<%= Session["lname"] %>';
        paypal.Buttons({
            createOrder: function (data, actions) {
                return actions.order.create({
                    payee: {
                        name: {
                            given_name: '<%= Session["fname"] %>',
                            surname: '<%= Session["lname"] %>'
                        },
                        phone: {
                            phone_type: "MOBILE",
                            phone_num: '<%= Session["contact_no"] %>'
                        },
                        email: '<%= Session["email"] %>',
                    },
                    purchase_units: [{
                        amount: {
                            value: '13000'

                        }
                    }]
                });
            },
            onApprove: function (data, actions) {
                return actions.order.capture().then(function (details) {
                    console.log(details)
                    sessionStorage.setItem("plan", "mini");
                    window.location.replace("/pages/PaymentSuccess.aspx");
                });
            },
            onCancel: function (data) {
                window.location.replace("/pages/PaymentFailed.aspx");
            }
        }).render("#paypal-button-container");
    </script>
</body>
</html>
