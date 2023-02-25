<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="IndicatorReports.aspx.cs" Inherits="AMBER.Pages.IndicatorReports" %>

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
                            <h1 style="color: darkslategray"><i class="fa fa-question-circle"></i>
                                Overall Report by Question</h1>
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
                        <asp:Literal ID="ltIndicator" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="card">
                <div class="card-body">
                    <div class="row">
                    <div class="col">
                        <asp:GridView ID="GridView1" CssClass="table table-hover table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="indicator_id" DataSourceID="SqlDataSource1">
                            <Columns>
                                <asp:BoundField DataField="indicator_name" HeaderStyle-BackColor="DarkOrange" HeaderStyle-HorizontalAlign="Center" HeaderText="Question" SortExpression="indicator_name"></asp:BoundField>
                                <asp:BoundField DataField="AVERAGE" HeaderStyle-BackColor="DarkOrange" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Average" SortExpression="AVERAGE"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT INDICATOR_TABLE.indicator_id,INDICATOR_TABLE.indicator_name,CONVERT(DECIMAL(10,2),AVG(CAST(EVALUATION_TABLE.rate as FLOAT)))AS AVERAGE FROM EVALUATION_TABLE RIGHT JOIN INDICATOR_TABLE ON INDICATOR_TABLE.indicator_Id = EVALUATION_TABLE.indicator_id WHERE EVALUATION_TABLE.school_id=@school_id AND INDICATOR_TABLE.isDeleted IS NULL GROUP BY INDICATOR_TABLE.indicator_id,INDICATOR_TABLE.indicator_name ORDER BY AVERAGE DESC">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
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
