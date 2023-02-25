<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="DepartmentReport.aspx.cs" Inherits="AMBER.Pages.Reports.DepartmentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3">
            <div class="col">
                <div class="card" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-building-user"></i><i class="fa-solid fa-chart-pie"></i>
                                Department Reports</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br /><br />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%--<asp:UpdatePanel runat="server" ID="updtPanel">
            <ContentTemplate>--%>
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
                        <asp:Literal ID="ltChart" runat="server"></asp:Literal>
                    </div>
                </div>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>
