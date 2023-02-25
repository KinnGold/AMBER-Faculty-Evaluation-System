<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="DeanPeerResult.aspx.cs" Inherits="AMBER.Pages.Users.Dean.DeanPeerResult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('/Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-briefcase"></i>
                                Peer Evaluation Result</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <asp:PlaceHolder ID="plcData" runat="server">
            <div class="row">
                <div class="col">
                    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                
                                <asp:GridView ID="GridView1" CssClass="table table-bordered" BorderColor="Transparent" runat="server" AutoGenerateColumns="False" DataKeyNames="dept_id" OnRowDataBound="GridView1_RowDataBound" ShowHeader="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="DEPARTMENT" SortExpression="dept_id">
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" Text='<%# Bind("section_name") %>' ID="TextBox1"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <%--<div class="card w-25 h-auto border-0 shadow-lg d-grid justify-content-center" style="padding:5px; background-color: #ebecf0;">
                            <asp:Label runat="server" Text='<%# Bind("dept_name") %>' ID="Label2" Font-Size="Larger" Font-Bold="true" style="text-align: center;"></asp:Label>
                        </div>--%>
                                                <br />
                                                <asp:Label runat="server" Text='<%# Bind("dept_name") %>' ID="Label2" Font-Size="Larger" Font-Bold="true" Style="text-align: center;"></asp:Label>
                                                <asp:GridView ID="GridviewMembers" GridLines="None" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" DataKeyNames="id" OnRowCommand="GridviewMembers_RowCommand">
                                                    <HeaderStyle BackColor="Orange" BorderColor="Transparent" />
                                                    <Columns>
                                                        <asp:BoundField DataField="INSTRUCTOR" ItemStyle-Width="80%" HeaderText="Faculty Member" SortExpression="INSTRUCTOR"></asp:BoundField>
                                                        <asp:BoundField DataField="isEvaluated" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Evaluated?" SortExpression="isEvaluated"></asp:BoundField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnRESULT" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="selectMember" runat="server" Text="VIEW RESULTS" />
                                                            </ItemTemplate>

                                                            <ControlStyle CssClass="btn btn-outline-primary"></ControlStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <script type="text/javascript">
        function customOpen(url) {
            var w = window.open(url, '', 'width=1000,height=600,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
            w.focus();

        }
    </script>
</asp:Content>
