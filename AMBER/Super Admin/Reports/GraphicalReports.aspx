<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GraphicalReports.aspx.cs" Inherits="AMBER.Super_Admin.Reports.GraphicalReports" %>

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

    <style type="text/css">
        #categoryTitle {
            font-size: larger;
        }

        .container {
            margin-top: 20px;
        }

        .page {
            display: none;
        }

        .page-active {
            display: block;
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
            <div class="row mb-5">
                <div class="col">
                    <div class="card shadow" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                        <div class="card-body">
                            <center>
                                <h1 style="color: darkslategray"><i class="fa-solid fa-chart-pie"></i>
                                    Overall Reports</h1>
                            </center>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container shadow">
                <div class="container">
                    <div class="row mb-3">
                        <div class="col-8">
                            <asp:DropDownList ID="DropDownSchool" OnSelectedIndexChanged="DropDownSchool_SelectedIndexChanged" runat="server" AutoPostBack="true" CssClass="form-select"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="jumbotron page" id="page1">
                        <div class="card">
                            <div class="card-body">
                                <div class="row mb-3">
                                    <div class="col-6">
                                        <h4 class="text-center">Users Chart</h4>
                                        <div class="card">
                                            <canvas id="UserChart"></canvas>
                                            <asp:Label CssClass="text-center" runat="server" ID="lbltotalusers"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <h4 class="text-center">Subscribers Chart</h4>
                                        <div class="card">
                                            <canvas id="subscriberChart"></canvas>
                                            <asp:Label CssClass="text-center" runat="server" ID="lblTotalSubscribers"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="jumbotron page" id="page2">
                    <div class="container">
                        <div class="card">
                            <div class="card-body">
                                <div class="row mb-3">
                                    <div class="col-6">
                                        <h4 class="text-center">Evaluation Chart</h4>
                                        <div class="card">
                                            <canvas id="EvaluatorChart"></canvas>
                                            <asp:Label CssClass="text-center" runat="server" ID="lbltotal1"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <h4 class="text-center">Evaluation Participants Chart</h4>
                                        <div class="card">
                                            <canvas id="isEvaluatedChart"></canvas>
                                            <asp:Label CssClass="text-center" runat="server" ID="Label1">Total Number of Users Participated</asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="jumbotron page" id="page3">
                    <div class="container">
                        <div class="card">
                            <div class="card-body">
                                <div class="row mb-3">
                                    <div class="col-6">
                                        <h4 class="text-center">Courses & Departments Chart</h4>
                                        <div class="card">
                                            <canvas id="courseDeptChart"></canvas>
                                            <asp:Label CssClass="text-center" runat="server" ID="numberOfCourse"></asp:Label>
                                            <asp:Label CssClass="text-center" runat="server" ID="numberOfDepartment"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <h4 class="text-center">Subjects & Sections Chart</h4>
                                        <div class="card">
                                            <canvas id="subSectionChart"></canvas>
                                            <asp:Label CssClass="text-center" runat="server" ID="numberOfSubjects"></asp:Label>
                                            <asp:Label CssClass="text-center" runat="server" ID="numberOfSection"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="jumbotron page mb-3" id="page4">
                    <div class="container">
                        <div class="card">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-6">
                                        <div class="card">
                                            <div class="row">
                                                <div class="col-6">
                                                    <asp:PlaceHolder ID="plcHaveCategory" runat="server">
                                                        <asp:Chart ID="Chart1" runat="server" Width="350px">
                                                            <Titles>
                                                                <asp:Title Text="Overall Category Report"></asp:Title>
                                                            </Titles>
                                                            <Series>
                                                                <asp:Series Name="Series1" ChartArea="ChartArea1" ChartType="Bar"></asp:Series>
                                                            </Series>
                                                            <ChartAreas>
                                                                <asp:ChartArea Name="ChartArea1">
                                                                    <AxisX Title="Categories">
                                                                    </AxisX>
                                                                    <AxisY Title="Total Category Average">
                                                                    </AxisY>
                                                                </asp:ChartArea>
                                                            </ChartAreas>
                                                        </asp:Chart>
                                                    </asp:PlaceHolder>
                                                    <asp:PlaceHolder ID="plcNoCategory" runat="server">
                                                        <p class="text-center" style="font-size: large">Overall Category Report</p>
                                                        <h3 class="text-center" style="font-weight: bold;">No Overall Category Report </h3>
                                                    </asp:PlaceHolder>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <ul id="pagination-demo" class="pagination-lg justify-content-center m-2 mb-2"></ul>
            </div>
        </div>

        <asp:HiddenField ID="hvStudent" runat="server" />
        <asp:HiddenField ID="hvInstructor" runat="server" />
        <asp:HiddenField ID="hvNotEvaluated" runat="server" />
        <asp:HiddenField ID="hvHasEvaluated" runat="server" />
        <asp:HiddenField ID="studentUser" runat="server" />
        <asp:HiddenField ID="instructorUser" runat="server" />
        <asp:HiddenField ID="adminUser" runat="server" />
        <asp:HiddenField ID="subscribed" runat="server" />
        <asp:HiddenField ID="notSubscribed" runat="server" />
        <asp:HiddenField ID="dept" runat="server" />
        <asp:HiddenField ID="section" runat="server" />
        <asp:HiddenField ID="course" runat="server" />
        <asp:HiddenField ID="subject" runat="server" />
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
    <script type="text/javascript" src="https://www.solodev.com/assets/pagination/jquery.twbsPagination.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#pagination-demo').twbsPagination({
                totalPages: 4,
                // the current page that show on start
                startPage: 1,

                // maximum visible pages
                visiblePages: 5,

                initiateStartPageClick: true,

                // template for pagination links
                href: false,

                // variable name in href template for page number
                hrefVariable: '{{number}}',

                // Text labels
                first: 'First',
                prev: 'Previous',
                next: 'Next',
                last: 'Last',

                // carousel-style pagination
                loop: false,

                // callback function
                onPageClick: function (event, page) {
                    $('.page-active').removeClass('page-active');
                    $('#page' + page).addClass('page-active');
                },

                // pagination Classes
                paginationClass: 'pagination',
                nextClass: 'next',
                prevClass: 'prev',
                lastClass: 'last',
                firstClass: 'first',
                pageClass: 'page',
                activeClass: 'active',
                disabledClass: 'disabled'

            });

        });
    </script>
    <script>
        const data3 = [];
        data3[0] = document.getElementById('<%= adminUser.ClientID %>').value;
        data3[1] = document.getElementById('<%= instructorUser.ClientID %>').value;
        data3[2] = document.getElementById('<%= studentUser.ClientID %>').value;

        const user = document.getElementById('UserChart');

        new Chart(user, {
            type: 'pie',
            data: {
                labels: ['Admins', 'Instructors', 'Students'],
                datasets: [{
                    label: 'Total #',
                    data: data3,
                    borderWidth: 1
                }]
            },

        });

        const data = [];
        data[0] = document.getElementById('<%= hvInstructor.ClientID %>').value;
        data[1] = document.getElementById('<%= hvStudent.ClientID %>').value;

        const evaluator = document.getElementById('EvaluatorChart');

        new Chart(evaluator, {
            type: 'pie',
            data: {
                labels: ['Instructor', 'Student'],
                datasets: [{
                    label: 'Total #',
                    data: data,
                    borderWidth: 1
                }]
            },

        });

        const data1 = [];
        data1[0] = document.getElementById('<%= hvNotEvaluated.ClientID %>').value;
        data1[1] = document.getElementById('<%= hvHasEvaluated.ClientID %>').value;

        const isEvaluated = document.getElementById('isEvaluatedChart');

        new Chart(isEvaluated, {
            type: 'pie',
            data: {
                labels: ['Not Participating', 'Has Participated'],
                datasets: [{
                    label: 'Total #',
                    data: data1,
                    borderWidth: 1,
                    backgroundColor: ["#A7C7E7", "#FAA0A0"]
                }]
            },

        });

        const data2 = [];
        data1[0]
        data1[1]

        const HasEvaluated = document.getElementById('PartialEvaluated');

        new Chart(HasEvaluated, {
            type: 'pie',
            data: {
                labels: ['Instructor Participated', 'Student Participated'],
                datasets: [{
                    label: 'Total #',
                    data: data2,
                    borderWidth: 1,
                    backgroundColor: ["#A7C7E7", "#FAA0A0"]
                }]
            },

        });

        const data4 = [];
        data4[0] = document.getElementById('<%= subscribed.ClientID %>').value;
        data4[1] = document.getElementById('<%= notSubscribed.ClientID %>').value;

        const isSubscribed = document.getElementById('subscriberChart');

        new Chart(isSubscribed, {
            type: 'pie',
            data: {
                labels: ['Subscribed', 'Not Subscribed'],
                datasets: [{
                    label: 'Total #',
                    data: data4,
                    borderWidth: 1,
                    backgroundColor: ["#cc99cc", "#99cccc"]
                }]
            },

        });

        const data5 = [];
        data5[0] = document.getElementById('<%= course.ClientID %>').value;
        data5[1] = document.getElementById('<%= dept.ClientID %>').value;

        const coursedept = document.getElementById('courseDeptChart');

        new Chart(coursedept, {
            type: 'pie',
            data: {
                labels: ['Courses', 'Departments'],
                datasets: [{
                    label: 'Total #',
                    data: data5,
                    borderWidth: 1,
                    backgroundColor: ["#E5BA73", "#F6D860"]
                }]
            },

        });

        const data6 = [];
        data6[0] = document.getElementById('<%= section.ClientID %>').value;
        data6[1] = document.getElementById('<%= subject.ClientID %>').value;

        const subsection = document.getElementById('subSectionChart');

        new Chart(subsection, {
            type: 'pie',
            data: {
                labels: ['Sections', 'Subjects'],
                datasets: [{
                    label: 'Total #',
                    data: data6,
                    borderWidth: 1,
                    backgroundColor: ["#386087", "#fe987b"]
                }]
            },

        });
    </script>
</body>
</html>
