<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="OverallRankings.aspx.cs" Inherits="AMBER.Super_Admin.OverallRankings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .profileBG {
            position: relative;
            width: 100px;
            height: 100px;
            border: 5px solid orange;
            border-radius: 50%;
            background-size: 100% 100%;
            overflow: hidden;
        }

        .hiddencol {
            display: none;
        }

        .table a /** FOR THE PAGING ICONS **/ {
            background-color: Transparent;
            padding: 5px 5px 5px 5px;
            color: #fff;
            text-decoration: none;
        }

            .table a:hover /** FOR THE PAGING ICONS HOVER STYLES**/ {
                background-color: black;
                color: #fff;
            }

        /*.table span*/ /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ /*{
            background-color: #c9c9c9;
            color: #000;
            padding: 5px 5px 5px 5px;
        }*/

        .pager {
            background-color: #FFBF00;
            font-family: Arial;
            color: White;
            height: 30px;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-5">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../../Pictures/former.png'); background-repeat: no-repeat; background-size: cover;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-ranking-star"></i>&nbsp;&nbsp;Rankings of Evaluation</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="row mb-3">
            <div class="col-6">
                <asp:DropDownList ID="DropDownSchool" runat="server" OnSelectedIndexChanged="DropDownSchool_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select  shadow"></asp:DropDownList>
            </div>
            <div class="col-6">
                <asp:DropDownList ID="DropDownDepartment" OnSelectedIndexChanged="DropDownDepartment_SelectedIndexChanged" runat="server" AutoPostBack="true" CssClass="form-select  shadow"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:PlaceHolder ID="plcData" runat="server">
                    <div class="card border-0 shadow-lg">
                        <div class="card-body ">
                            <div class="row scrollbar-primary" style="overflow-y: scroll">
                                <asp:GridView runat="server" ID="gvranks" CssClass="table table-hover text-center" BorderStyle="None" Font-Bold="true" Font-Size="X-Large" PagerStyle-CssClass="pager" AllowPaging="true" PageSize="5" OnPageIndexChanging="gvranks_PageIndexChanging" OnRowDataBound="gvranks_RowDataBound" AutoGenerateColumns="False" DataKeyNames="evaluatee_id">
                                    <Columns>
                                        <asp:BoundField DataField="evaluatee_id" ShowHeader="false" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="evaluatee_id" ReadOnly="True" InsertVisible="False" SortExpression="Id"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="profile_picture">
                                            <ItemTemplate>
                                                <div class="profileBG">
                                                    <asp:Image ID="Image1" Width="90px" Height="90px" runat="server" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NAME" HeaderText="Name" ReadOnly="True" InsertVisible="False" SortExpression="NAME" ItemStyle-VerticalAlign="Middle"></asp:BoundField>
                                        <asp:BoundField DataField="AVERAGE" HeaderText="Average" ReadOnly="True" InsertVisible="False" SortExpression="AVERAGE" ItemStyle-VerticalAlign="Middle"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("verbal_rate") %>' ID="lblDesc"></asp:Label>
                                            </ItemTemplate>

                                            <ItemStyle VerticalAlign="Middle"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
</asp:Content>
