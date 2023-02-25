<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="ConstructorReport.aspx.cs" Inherits="AMBER.Pages.ConstructorReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa fa-check-square"></i>
                                Overall Report by Category</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row mb-3">
            <div class="col">
                <div class="card">
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-6">
                                <asp:DropDownList ID="ddlDepartment" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col">
                                <asp:DropDownList ID="ddlPeer" OnSelectedIndexChanged="ddlPeer_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-6">
                                <asp:PlaceHolder ID="plcChart" runat="server">
                                    <asp:Literal ID="ltConstructor" runat="server"></asp:Literal>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="plcNoChart" runat="server">
                                    <center>
                                        <p>
                                            <img src="/Pictures/nonodata.png" style="height: 350px; width: 350px;" />
                                        </p>
                                        <h1>No Record Found</h1>
                                    </center>
                                </asp:PlaceHolder>
                            </div>
                            <div class="col my-auto">
                                <asp:PlaceHolder ID="plcData" runat="server">
                                    <asp:GridView ID="GridView1" CssClass="table table-hover table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="constructor_id">
                                        <Columns>
                                            <asp:BoundField DataField="constructor_name" HeaderStyle-BackColor="DarkOrange" HeaderStyle-HorizontalAlign="Center" HeaderText="Category" SortExpression="constructor_name"></asp:BoundField>
                                            <asp:BoundField DataField="AVERAGE" HeaderStyle-BackColor="DarkOrange" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Average" SortExpression="MEAN"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="plcNoData" runat="server">
                                    <center>
                                        <p>
                                            <img src="/Pictures/nonodata.png" style="height: 350px; width: 350px;" />
                                        </p>
                                        <h1>No Record Found</h1>
                                    </center>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--<asp:PlaceHolder ID="plcData" runat="server">
            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col">
                            <asp:GridView ID="GridView1" CssClass="table table-hover table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="constructor_id">
                                <Columns>
                                    <asp:BoundField DataField="constructor_name" HeaderStyle-BackColor="DarkOrange" HeaderStyle-HorizontalAlign="Center" HeaderText="Category" SortExpression="constructor_name"></asp:BoundField>
                                    <asp:BoundField DataField="AVERAGE" HeaderStyle-BackColor="DarkOrange" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Average" SortExpression="AVERAGE"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>--%>
    </div>
</asp:Content>
