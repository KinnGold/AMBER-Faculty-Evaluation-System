<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="VerbalCountPage.aspx.cs" Inherits="AMBER.Pages.Reports.VerbalCountPage" %>

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
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa fa-check-square"></i>
                                Department Verbal Interpretation</h1>
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
                        </div>
                        <div class="row mb-3">
                            <div class="col-6">
                                <asp:PlaceHolder ID="plcChart" runat="server">
                                    <asp:Literal ID="ltCount" runat="server"></asp:Literal>
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
                                    <asp:GridView ID="GridView1" CssClass="table table-hover table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="verbalDesc">
                                        <Columns>
                                            <asp:BoundField DataField="verbalDesc" HeaderStyle-BackColor="DarkOrange" HeaderStyle-HorizontalAlign="Center" HeaderText="Verbal Interpretation" SortExpression="verbalDesc"></asp:BoundField>
                                            <asp:BoundField DataField="count" HeaderStyle-BackColor="DarkOrange" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Count" SortExpression="count"></asp:BoundField>
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
        <div class="row mb-3">
            <div class="col">
                <div class="card">
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-6">
                                <asp:DropDownList ID="ddlVerbal" OnSelectedIndexChanged="ddlVerbal_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" runat="server">
                                    <asp:ListItem Value="HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 5 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 4.5">Outstanding</asp:ListItem>
                                    <asp:ListItem Value="HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 4.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 3.5">Very Satisfactory</asp:ListItem>
                                    <asp:ListItem Value="HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 3.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 2.5">Satisfactory</asp:ListItem>
                                    <asp:ListItem Value="HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 2.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 1.5">Fair</asp:ListItem>
                                    <asp:ListItem Value="HAVING CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) < 1.49 AND CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate AS FLOAT))) > 1">Poor</asp:ListItem>
                                    <asp:ListItem Selected="True" Value=" ">ALL</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col">
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                    <asp:GridView runat="server" ID="gvVerbal" CssClass="table table-hover text-center" BorderStyle="None" Font-Bold="true" Font-Size="X-Large" PagerStyle-CssClass="pager" AutoGenerateColumns="False" OnRowDataBound="gvVerbal_RowDataBound" DataKeyNames="evaluatee_id">
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
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="PlaceHolder2" runat="server">
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
    </div>
</asp:Content>
