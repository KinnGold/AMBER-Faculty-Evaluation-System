<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OverallGraphicalReports.aspx.cs" Inherits="AMBER.Super_Admin.Reports.OverallGraphicalReports" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.1/jquery.min.js"></script>
    <link href="~/A.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-iYQeCzEYFbKjA/T2uDLTpkwGzCiq6soy8tYaI1GyVh/UjpbCx/TYkiZhlZB6+fzT" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700,800,900" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.12.1/css/jquery.dataTables.css" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/4.1.1/chart.umd.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chartjs-plugin-datalabels/2.2.0/chartjs-plugin-datalabels.min.js" integrity="sha512-JPcRR8yFa8mmCsfrw4TNte1ZvF1e3+1SdGMslZvmrzDYxS69J7J49vkFL8u6u8PlPJK+H3voElBtUCzaXj+6ig==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://unpkg.com/chart.js-plugin-labels-dv/dist/chartjs-plugin-labels.min.js"></script>
    <style type="text/css">
        #categoryTitle {
            font-size: larger;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light bg-light mb-0 p-1 shadow-sm bg-white">
            <a href="/Super%20Admin/SuperAdminLandingPage.aspx" class="nav-link">
                <img src="/../../Pictures/CAPSTONER-Amber.png" style="height: auto; width: 90px;" class="img-fluid mx-3" /></a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="nav navbar-nav">
                    <li class="nav-item">
                        <a href="/Super%20Admin/SuperAdminLandingPage.aspx" class="nav-link">Home</a>
                    </li>
                </ul>
            </div>
        </nav>
        <div class="container p-4 p-md-5 pt-5">
            <div class="row mb-3">
                <div class="col">
                    <div class="card" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                        <div class="card-body">
                            <center>
                                <h1 style="color: darkslategray"><i class="fa-solid fa-chart-pie"></i>
                                    Overall Graphical Reports</h1>
                            </center>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card">
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-8">
                            <asp:DropDownList ID="DropDownSchool" OnSelectedIndexChanged="DropDownSchool_SelectedIndexChanged" runat="server" AutoPostBack="true" CssClass="form-select"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-6">
                            <div class="card">
                                <div class="row">
                                    <div class="col">
                                        <asp:Literal ID="ltUserPercentage" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="card">
                                <div class="row">
                                    <div class="col">
                                        <center>
                                            <h5>Student Participation Chart</h5>
                                        </center>
                                        <asp:Literal ID="ltStudParticipate" runat="server"></asp:Literal>
                                        <center>
                                            <asp:Label ID="lblStudCount" CssClass="text-center" runat="server" Text="Label"></asp:Label>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-6">
                            <div class="card">
                                <div class="row">
                                    <div class="col">
                                        <center>
                                            <h5>Instructor Participation Chart</h5>
                                        </center>
                                        <asp:Literal ID="ltInsParticipate" runat="server"></asp:Literal>
                                       <center>
                                            <asp:Label ID="lblInsCount" CssClass="text-center" runat="server" Text="Label"></asp:Label>
                                       </center>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="card">
                                <div class="row">
                                    <div class="col">
                                        <asp:Literal ID="ltCourseDept" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-6">
                            <div class="card">
                                <div class="row">
                                    <div class="col">
                                        <asp:Literal ID="ltSubscription" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="card">
                                <div class="row">
                                    <div class="col">
                                        <asp:Literal ID="ltSubsection" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-6">
                            <div class="card">
                                <div class="row">
                                    <div class="col">
                                        <asp:Literal ID="ltChartCategory" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="card">
                                <div class="row">
                                    <div class="col">
                                        <asp:Literal ID="ltChartQuestion" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <asp:HiddenField ID="hvStudent" runat="server" />
        <asp:HiddenField ID="hvInstructor" runat="server" />
        <asp:HiddenField ID="hvStudNotEvaluated" runat="server" />
        <asp:HiddenField ID="hvStudHasEvaluated" runat="server" />
        <asp:HiddenField ID="hvInsNotEvaluated" runat="server" />
        <asp:HiddenField ID="hvInsHasEvaluated" runat="server" />
        <asp:HiddenField ID="hvCourse" runat="server" />
        <asp:HiddenField ID="hvDepartment" runat="server" />
        <asp:HiddenField ID="hvSubject" runat="server" />
        <asp:HiddenField ID="hvSection" runat="server" />
        <asp:HiddenField ID="hvSubscribed" runat="server" />
        <asp:HiddenField ID="hvNotSubscribed" runat="server" />
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-u1OknCvxWvY5kfmNBILK2hRnQC3Pr17a+RTT6rIHI7NnikvbZlHgTPOOmMi466C8" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/js/bootstrap.min.js" integrity="sha384-7VPbUDkoPSGFnVtYi0QogXtr74QeVeeIs99Qfg5YCF+TidwNdjvaKZX19NZ/e6oz" crossorigin="anonymous"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.12.1/js/jquery.dataTables.js"></script>
    <script src="https://kit.fontawesome.com/288b588b0b.js" crossorigin="anonymous"></script>
    <script src="/Scripts/BacktoTopBTN/backTotop.js"></script>
</body>
</html>
