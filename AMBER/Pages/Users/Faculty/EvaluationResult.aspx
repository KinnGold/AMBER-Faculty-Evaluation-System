<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="EvaluationResult.aspx.cs" Inherits="AMBER.Pages.Users.Faculty.EvaluationResult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <asp:GridView ID="gridviewConstructors" CssClass="table" runat="server" BorderColor="Transparent" AutoGenerateColumns="False" DataKeyNames="constructor_id" OnSorting="GridView1_Sorting">
            <Columns>
                <asp:BoundField DataField="constructor_name" HeaderText="Category" ReadOnly="True" InsertVisible="False"></asp:BoundField>
                <asp:BoundField DataField="indicator_name" HeaderText="Question"></asp:BoundField>
                <asp:BoundField DataField="AVERAGE" HeaderText="Average rate"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="GridView1" CssClass="table" runat="server" BorderColor="Transparent" AutoGenerateColumns="False" DataKeyNames="constructor_id" OnSorting="GridView1_Sorting">
            <Columns>
                <asp:BoundField DataField="constructor_name" HeaderText="Category" ReadOnly="True" InsertVisible="False"></asp:BoundField>
                <asp:BoundField DataField="indicator_name" HeaderText="Question"></asp:BoundField>
                <asp:BoundField DataField="AVERAGE" HeaderText="Average rate"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
