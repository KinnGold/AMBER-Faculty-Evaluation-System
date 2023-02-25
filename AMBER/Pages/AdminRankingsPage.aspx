<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="AdminRankingsPage.aspx.cs" Inherits="AMBER.Pages.AdminRankingsPage" %>

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

        .table span /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ {
            /*background-color: #c9c9c9;*/
            color: #000;
            padding: 5px 5px 5px 5px;
        }

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
        <div class="row mb-3">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../../Pictures/former.png'); background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-ranking-star"></i>&nbsp;&nbsp;Individual: Rankings of Evaluation</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="row">
                <div class="col">
                    <div class="row mb-3">
                        <div class="col-8">
                            <asp:DropDownList ID="dropdownDepartment" OnSelectedIndexChanged="dropdownDepartment_SelectedIndexChanged" runat="server" AutoPostBack="true" CssClass="form-select shadow"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="card border-0 shadow-lg">
                        <div class="card-body ">
                            <div class="row scrollbar-primary" style="overflow-y: scroll">
                                <asp:GridView runat="server" ID="gvranks" CssClass="table table-hover text-center" BorderStyle="None" Font-Bold="true" Font-Size="X-Large" PagerStyle-CssClass="pager" AllowPaging="true" PageSize="5" OnPageIndexChanging="gvranks_PageIndexChanging" OnRowDataBound="gvranks_RowDataBound" AutoGenerateColumns="False" DataKeyNames="evaluatee_id">
                                    <Columns>
                                        <asp:BoundField DataField="evaluatee_id" ShowHeader="false" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="evaluatee_id" ReadOnly="True" InsertVisible="False" SortExpression="Id"></asp:BoundField>
                                        <asp:BoundField DataField="dept_id" ShowHeader="false" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HeaderText="evaluatee_id" ReadOnly="True" InsertVisible="False" SortExpression="Id"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="profile_picture">
                                            <ItemTemplate>
                                                <div class="profileBG">
                                                    <asp:Image ID="Image1" Width="90px" Height="90px" runat="server" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-VerticalAlign="Middle"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Average">
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("AVERAGE") %>' ID="txtAVERAGE"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("AVERAGE") %>' ID="lblAVERAGE"></asp:Label>
                                            </ItemTemplate>

                                            <ItemStyle VerticalAlign="Middle"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("verbal_rate") %>' ID="txtDesc"></asp:Label>
                                            </EditItemTemplate>
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
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcNoData" runat="server">
            <div class="row mt-5">
                <div class="col">
                    <div class="card shadow">
                        <div class="card-body">
                            <center>
                                <p>
                                    <img src="../../Pictures/nonodata.png" style="height: 350px; width: 350px;" />
                                </p>
                                <h1>No Record Found</h1>
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
