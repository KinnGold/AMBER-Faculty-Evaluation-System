<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoResult.aspx.cs" Inherits="AMBER.Pages.Reports.NoResult" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.1/jquery.min.js"></script>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT" crossorigin="anonymous"/>
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.12.1/css/jquery.dataTables.css"/>
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container p-4 p-md-5 pt-5">
            <div class="row my-2" >
                <div class="col align-self-center">
                    <div class="card border-0 text-center p-2 bg-transparent">
                        <a href="/Pages/AdminLandingPage.aspx">
                            <img src="/Pictures/AMBERLOGO1.png" class="img-fluid" style="width:400px; height:150px" />
                        </a>
                    </div>
                </div>
                <div class="col align-self-center">
                    <div class="card border-0 shadow-lg w-100">
                <div class="card-body text-center">
                    <br /><br />
                    <div class="row">
                        <div class="col">
                            <i class="fa fa-exclamation-circle sha" style="font-size: 250px; color:orange;"></i>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col">
                            <h1>NO RESULT TO BE SHOWN</h1>
                        </div>
                    </div>
                </div>
                <br />
            </div>
                </div>
               <%-- <div class="col">
                    <div class="card border-0 shadow p-2 text-center" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                        <h1><i class="fa fa-paperclip"></i>&nbsp;RESULT</h1>
                    </div>
                </div>--%>
            </div>
            <br /><br />
            <%--<div class="card border-0 shadow-lg">
                <div class="card-body text-center">
                    <br /><br />
                    <div class="row">
                        <div class="col">
                            <i class="fa fa-exclamation-circle sha" style="font-size: 300px; color:orange;"></i>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col">
                            <h1>NO RESULT TO BE SHOWN</h1>
                        </div>
                    </div>
                </div>
                <br />
            </div>--%>
        </div>
    </form>
    <script type="module" src="https://cdn.jsdelivr.net/combine/npm/chart.js@4,npm/chart.js@4/dist/chart.min.js,npm/chart.js@4/dist/helpers.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4/dist/chart.umd.min.js"></script>
    <script type="module" src="https://cdn.jsdelivr.net/npm/chart.js@4/dist/chart.min.js"></script>
    <script type="module" src="https://cdn.jsdelivr.net/npm/chart.js@4/dist/helpers.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-u1OknCvxWvY5kfmNBILK2hRnQC3Pr17a+RTT6rIHI7NnikvbZlHgTPOOmMi466C8" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/js/bootstrap.min.js" integrity="sha384-7VPbUDkoPSGFnVtYi0QogXtr74QeVeeIs99Qfg5YCF+TidwNdjvaKZX19NZ/e6oz" crossorigin="anonymous"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.12.1/js/jquery.dataTables.js"></script>
    <script src="https://kit.fontawesome.com/288b588b0b.js" crossorigin="anonymous"></script>
</body>
</html>
