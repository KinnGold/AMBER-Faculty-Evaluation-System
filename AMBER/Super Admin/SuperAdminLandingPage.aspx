<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="SuperAdminLandingPage.aspx.cs" Inherits="AMBER.Super_Admin.SuperAdminLandingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script>

        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');
            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth'
            });
            calendar.render();
        });

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid p-5 min-vh-100 m-0 d-flex flex-column justify-content-center bg-transparent" style="background-image: url(/../../Pictures/1.png);">
        <div class="row">
            <div class="col">
                <section class="mb-3">
                    <div class="row">
                        <div class="col">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-4">
                                            <p>
                                                Welcome
                                                <asp:Label ID="lbladmin" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                            </p>
                                        </div>
                                        <div class="col-4">
                                            <p>
                                                Semester:
                                                <asp:Label ID="lblsem" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                            </p>
                                        </div>
                                        <div class="col-4">
                                            <p>
                                                Evaluation Status:
                                                <asp:Label ID="lblstatus" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <section class="mb-3">
                    <div class="row">
                        <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Admins">Administrators</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="labeladmin" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <i class="fa-solid fa-users-gear" style="height: 100%; width: 100%;"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Instructors">Instructors</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="instlbl" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <i class="fa-solid fa-users-between-lines" style="height: 100%; width: 100%;"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Instructors">Students</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="stdlbl" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <i class="fa-solid fa-users" style="height: 100%; width: 100%;"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <section class="mb-3">
                  <div class="row">
                        <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Schools">Schools</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="lblschools" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <i class="fa-solid fa-school-flag"  style="height: 100%; width: 100%;"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Subscription">Subscriptions</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="lblsubs" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <i class="fa-solid fa-credit-card"  style="height: 100%; width: 100%;"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                      <div class="col-4">
                            <div class="card widget-flat">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-8">
                                            <div class="row">
                                                <div class="col">
                                                    <h5 class="text-muted fw-normal mt-0" title="Number of Evaluation">Evaluated</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <dov clas="col">
                                                    <asp:Label ID="labeleval" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                                                </dov>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <i class="fa-solid fa-list-check" style="height: 100%; width: 100%;"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <section class="mb-3">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-8">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="card-header text-center bg-transparent border-0">
                                                <h2>Calendar</h2>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <div id='calendar'></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
