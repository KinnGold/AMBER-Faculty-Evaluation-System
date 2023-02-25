<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="SkillsTestResults.aspx.cs" Inherits="AMBER.Pages.Users.Faculty.SkillsTestResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('/Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color:darkslategray"><i class="fa-solid fa-square-check"></i>Skills Test Results</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br /><br />
        <asp:PlaceHolder ID="plcgv" runat="server">
            <asp:GridView ID="GridviewGeneralResult" BorderColor="Transparent" RowStyle-Wrap="true" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="constructor_id">
                <HeaderStyle BackColor="DarkOrange" BorderColor="Transparent" />
                <Columns>
                    <%--ARI NKA WEIGTED MEAN--%>
                    <asp:BoundField DataField="constructor_name" ItemStyle-Width="60%" HeaderText="RESULT" SortExpression="constructor_name"></asp:BoundField>
                    <asp:TemplateField SortExpression="AVERAGE">
                        <ItemTemplate>
                            <div class="row">
                                <asp:Label runat="server" Style="font-weight: bold; text-align: center" Text='<%# Bind("AVERAGE") %>' ID="Label1"></asp:Label>
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
                                <asp:Label runat="server" Style="text-align: right" Text='<%# Bind("description") %>' ID="Label2"></asp:Label>
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

                    <asp:BoundField DataField="verbal_rate" ItemStyle-Width="43%" SortExpression="verbal_rate"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="GVResults" class="table" BorderColor="Transparent" runat="server" AutoGenerateColumns="False" OnRowDataBound="GVResults_RowDataBound" DataKeyNames="constructor_id">
                <Columns>
                    <asp:TemplateField SortExpression="constructor_name">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Style="font-weight:bold;font-size:x-large" Text='<%# Bind("constructor_name") %>'></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("AVERAGE") %>'></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("verbal_rate") %>'></asp:Label>
                            <asp:GridView ID="GridView2" BorderColor="Transparent" CssClass="table table-bordered" DataKeyNames="indicator_id" AutoGenerateColumns="false" runat="server">
                                <HeaderStyle BackColor="DarkOrange" HorizontalAlign="Center" BorderColor="Transparent" />
                                <RowStyle BackColor="#fff3cd" />
                                <Columns>
                                    <asp:BoundField DataField="indicator_name" ItemStyle-Width="60%" SortExpression="indicator_name"></asp:BoundField>
                                    <asp:BoundField DataField="AVERAGE" ItemStyle-CssClass="text-center" HeaderText="Rating" SortExpression="AVERAGE"></asp:BoundField>
                                    <asp:BoundField DataField="verbal_rate" ItemStyle-CssClass="text-center" HeaderText="Interpretation" SortExpression="verbal_rate"></asp:BoundField>
                                    <asp:BoundField DataField="PERCENTAGE" ItemStyle-CssClass="text-center" HeaderText="Interpretation" SortExpression="PERCENTAGE"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hfRESULT" runat="server" />
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcerror" runat="server">
            <div class="card border-0 shadow-lg">
                <div class="card-body text-center">
                    <br />
                    <br />
                    <div class="row">
                        <div class="col">
                            <i class="fa fa-exclamation-circle sha" style="font-size: 300px; color: orange;"></i>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col">
                            <h1>Results is not available since you have not been observed for a Skills Test.</h1>
                        </div>
                    </div>
                </div>
                <br />
            </div>
            <%--<div class="row">
                <div class="col">
                    <h2 class="text-center">Results is not available since you have not been observed for a Skills Test.</h2>
                </div>
            </div>--%>
        </asp:PlaceHolder>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.1/jquery.min.js"></script>
</asp:Content>
