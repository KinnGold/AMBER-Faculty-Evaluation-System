<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="StudentEvaluatedform.aspx.cs" Inherits="AMBER.Pages.Users.Student.StudentEvaluatedform" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="eval_id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="rate" ItemStyle-Width="46%"  HeaderText="rate" SortExpression="indicator_name"></asp:BoundField>
            <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:RadioButtonList ID="rblScore" Enabled="false" SelectedValue='<%# Bind("rate") %>' CssClass="radioButtonList" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Poor</asp:ListItem>
                                <asp:ListItem Value="2">Fair</asp:ListItem>
                                <asp:ListItem Value="3">Satisfactory</asp:ListItem>
                                <asp:ListItem Value="4">Very Satisfactory</asp:ListItem>
                                <asp:ListItem Value="5">Outstanding</asp:ListItem>
                            </asp:RadioButtonList>
                            <%--selected item--%>
                        </ItemTemplate>
                    </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [eval_id], [evaluator_id], [evaluatee_id], [indicator_id], [rate] FROM [EVALUATION_TABLE]"></asp:SqlDataSource>
</asp:Content>
