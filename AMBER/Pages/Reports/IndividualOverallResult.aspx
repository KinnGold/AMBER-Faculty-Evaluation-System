<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndividualOverallResult.aspx.cs" Inherits="AMBER.Pages.Reports.IndividualSectionResult" %>

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
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/4.1.1/chart.umd.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chartjs-plugin-datalabels/2.2.0/chartjs-plugin-datalabels.min.js" integrity="sha512-JPcRR8yFa8mmCsfrw4TNte1ZvF1e3+1SdGMslZvmrzDYxS69J7J49vkFL8u6u8PlPJK+H3voElBtUCzaXj+6ig==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://unpkg.com/chart.js-plugin-labels-dv/dist/chartjs-plugin-labels.min.js"></script>
    <style>
        @media print {
            table {
                page-break-after: auto
            }

            tr {
                page-break-inside: avoid;
                page-break-after: auto
            }

            td {
                page-break-inside: avoid;
                page-break-after: auto
            }

            thead {
                display: table-header-group
            }

            tfoot {
                display: table-footer-group
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row my-2">
                <div class="col-3">
                    <div class="text-center p-3 py-0 align-content-center">
                        <a href="/Pages/AdminLandingPage.aspx">
                            <asp:Image ID="schoolProfilePic" Style="border-radius: 50%;" CssClass="img-fluid" runat="server" Width="100px" Height="100px" />
                        </a>
                    </div>
                </div>
                <div class="col-9">
                    <div class="text-center p-3 py-0 align-content-center">
                        <asp:Label ID="labelSchoolName" Font-Bold="true" Font-Size="X-Large" runat="server" Text="Label"></asp:Label>
                    </div>
                    <div class="text-center p-3 py-0 align-content-center">
                        <asp:Label ID="labelSchoolAddress" Font-Bold="true" Font-Size="Large" runat="server" Text="Label"></asp:Label>
                    </div>
                    <div class="text-center p-3 py-0 align-content-center">
                        <asp:Label Font-Bold="true" CssClass="text-center" runat="server" ID="lbldatetime">Today</asp:Label>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col">
                    <div class="card" style="background-image: url('/Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                        <div class="card-body">
                            <center>
                                <h1 style="color: darkslategray"><i class="fa-solid fa-briefcase"></i>OVERALL EVALUATION RESULTS</h1>
                            </center>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col">
                    <table class="table table-bordered" runat="server" id="tblDetails">
                        <thead style="border-color:transparent">
                            <tr>
                                <th style="background-color:darkorange" colspan="3"  scope="col">DETAILS</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td id="celEvalPeriod">Evaluation Period: </td>
                            </tr>
                            <tr>
                                <td id="celInstructorName">Name of the Faculty Member: </td>
                            </tr>
                            <tr>
                                <td id="celSubject">Subject: </td>
                            </tr>
                            <tr>
                                <td id="celGroupCount">Total Faculty members: </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <asp:GridView ID="GridviewGeneralResult" BorderColor="Transparent" RowStyle-Wrap="true" CssClass="table table-bordered"  runat="server" AutoGenerateColumns="False" DataKeyNames="constructor_id">
                        <HeaderStyle BackColor="DarkOrange" BorderColor="Transparent" />
                        <Columns>
                            <%--ARI NKA WEIGTED MEAN--%>
                            <asp:BoundField DataField="constructor_name" ItemStyle-Width="60%"  HeaderText="RESULT" SortExpression="constructor_name"></asp:BoundField>
                            <asp:TemplateField SortExpression="AVERAGE">
                                <ItemTemplate>
                                    <div class="row">
                                        <asp:Label runat="server" style="font-weight:bold; text-align:center" Text='<%# Bind("AVERAGE") %>' ID="Label1"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                
                            </asp:TemplateField>

                            <asp:BoundField DataField="verbal_rate" ItemStyle-Width="43%" SortExpression="verbal_rate"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="GridviewResults" RowStyle-Wrap="true" BorderColor="Transparent" CssClass="table table-bordered" ShowHeader="false" Font-Bold="true" AutoGenerateColumns="false" DataKeyNames="description" runat="server">
                        <Columns>
                            <asp:TemplateField SortExpression="description">
                                <ItemTemplate>
                                    <div class="row">
                                        <asp:Label runat="server" style="text-align:right" Text='<%# Bind("description") %>' ID="Label2"></asp:Label>
                                    </div>
                                </ItemTemplate>

                                <ItemStyle Width="60%"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="test" SortExpression="result">
                                <ItemTemplate>
                                    <div class="row">
                                        <asp:Label runat="server" CssClass="text-center" Text='<%# Bind("result") %>' ID="Label1"></asp:Label>
                                    </div>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:BoundField DataField="verbal_rate" ItemStyle-Width="43%"  SortExpression="verbal_rate"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hfRESULT" runat="server" />
                </div>
            </div>
            <hr />
            <div class="row p-4">
                <div class="col-6">
                    <asp:Literal ID="ltChart" runat="server"></asp:Literal>
                </div>
                <div class="col-6">
                    <asp:Literal ID="ltCount" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <asp:GridView ID="GridView1" ShowHeader="false" BorderColor="Transparent" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound1" DataKeyNames="constructor_id">
                        <Columns>
                            <asp:TemplateField HeaderText="Question" SortExpression="indicator_name">
                                <ItemTemplate>
                                    <asp:Label runat="server" Style="font-weight:bold;font-size:x-large" Text='<%# Bind("constructor_name") %>' ID="Label1"></asp:Label>
                                    <asp:Label runat="server" Text='<%# Bind("AVERAGE") %>' ID="Label2"></asp:Label>
                                    <asp:Label runat="server" Text='<%# Bind("verbal_rate") %>' ID="Label3"></asp:Label>
                                    <asp:GridView ID="GridView2" BorderColor="Transparent" CssClass="table table-bordered" DataKeyNames="indicator_id" AutoGenerateColumns="false" runat="server">
                                        <HeaderStyle BackColor="DarkOrange" HorizontalAlign="Center" BorderColor="Transparent" />
                                        <RowStyle BackColor="#fff3cd"/>
                                        <Columns>
                                            <asp:BoundField DataField="row_num" ItemStyle-Width="1%" HeaderText="#" SortExpression="row_num"></asp:BoundField>
                                            <asp:BoundField DataField="indicator_name" ItemStyle-Width="60%" SortExpression="indicator_name"></asp:BoundField>
                                            <asp:BoundField DataField="AVERAGE" ItemStyle-CssClass="text-center" HeaderText="Rating" SortExpression="AVERAGE"></asp:BoundField>
                                            <asp:BoundField DataField="verbal_rate" ItemStyle-CssClass="text-center" HeaderText="Interpretation" SortExpression="verbal_rate"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <div class="form-group">
                    <label runat="server" id="lblcomment" for="txtcomment" style="font-weight:bold">Comments</label>
                    <asp:TextBox ID="txtcomment" ReadOnly="true" runat="server" CssClass="form-control" Rows="6" TextMode="MultiLine"></asp:TextBox>
                </div>
                </div>
            </div>
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
