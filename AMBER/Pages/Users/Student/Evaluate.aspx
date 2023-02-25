<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="Evaluate.aspx.cs" Inherits="AMBER.Pages.Users.Student.Evaluate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .radioButtonList {
            list-style: none;
            margin: 0;
            padding: 0;
        }

            .radioButtonList.horizontal li {
                display: inline;
            }

            .radioButtonList label {
                display: inline;
                margin:7px;
                padding:2px
            }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row mb-3">
            <div class="col">
                <asp:Label runat="server" ID="lblINS" Visible="false"></asp:Label>
                <%--display instructor name--%>
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlInstructor" AutoPostBack="true" CssClass="form-select" runat="server" OnSelectedIndexChanged="ddlInstructor_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <hr />
        <div class="row">
            <asp:GridView ID="GridView1" CssClass="table table-striped table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="indicator_Id" OnSorting="GridView1_Sorting" ShowHeader="false">
                <Columns>
                    <%--<asp:BoundField DataField="row_num" HeaderText="no." SortExpression="row_num"></asp:BoundField>--%>
                    <asp:BoundField DataField="constructor_name" HeaderText="Category" SortExpression="constructor_name"></asp:BoundField>
                    <%--<asp:BoundField DataField="indicator_name" ItemStyle-Width="42%" HeaderText="Question" SortExpression="indicator_name"></asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <div class="card-header"><asp:Label runat="server" Text='<%# Eval("indicator_name")  %>'></asp:Label></div>
                            <asp:RadioButtonList ID="rblScore" CssClass="radioButtonList" runat="server" RepeatDirection="Horizontal">
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
        </div>
        <div class="row row px-3">
            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="btn btn-primary" runat="server" Text="Submit" />
        </div>
    </div>
</asp:Content>
