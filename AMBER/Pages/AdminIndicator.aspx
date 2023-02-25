<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="AdminIndicator.aspx.cs" Inherits="AMBER.BM.AdminIndicator" %>

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
                            <h1 style="color: darkslategray"><i class="fa-solid fa-message"></i>
                                Question Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col">
                <div class="row">
                    <div class="col">
                        <div class="col">
                            <div class="row my-3">

                                <div class="col d-grid justify-content-end mx-auto">
                                    <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#IndicatorModal">Manage Questions</a>
                                </div>
                            </div>
                            <div class="modal fade" id="IndicatorModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="staticBackdropLabel">Manage Questions</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="form-group">
                                                <div class="form-floating mb-3">
                                                    <asp:DropDownList ID="DropDownList1" CssClass="form-select" runat="server"></asp:DropDownList>
                                                    <label for="DropDownList1">Category</label>
                                                </div>
                                                <div class="form-floating mb-3">
                                                    <asp:TextBox ID="indicator1" CssClass="form-control" runat="server"></asp:TextBox>
                                                    <label for="indicator1">Question<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqEvaluation" runat="server" ControlToValidate="indicator1" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                            <asp:Button ID="addIndicator" OnClick="addIndicator_Click" CssClass="btn btn-primary" ValidationGroup="RqEvaluation" runat="server" Text="Add" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:PlaceHolder ID="plcData" runat="server">
                <div class="row">
                    <div class="col">
                        <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="indicator_Id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                            <Columns>
                                <%--<asp:BoundField DataField="indicator_Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />--%>
                                <asp:TemplateField HeaderText="Category">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="NAME" DataValueField="constructor_id" SelectedValue='<%# Bind("constructor_id") %>'>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT (''+CONSTRUCTOR_TABLE.constructor_name+' ('+CONSTRUCTOR_TABLE.role+') ') AS NAME,[CONSTRUCTOR_ID] FROM [CONSTRUCTOR_TABLE] WHERE (([school_id] = @school_id))">
                                            <SelectParameters>
                                                <asp:SessionParameter SessionField="school" Name="school_id" Type="Int32"></asp:SessionParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("constructor_name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" CssClass="form-control" runat="server" Text='<%# Bind("indicator_name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("indicator_name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Assigned">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRole" CssClass="form-control" ReadOnly="true" runat="server" Text='<%# Bind("role") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label42" runat="server" Text='<%# Bind("role") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="12%" ItemStyle-CssClass="text-center">
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="True" ID="btnUpdate"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>&nbsp;<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CausesValidation="False" ID="LinkButton2"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="btn btn-success" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return DeleteConfirm(this);"><i class="fa-solid fa-trash-can"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="btn btn-danger" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
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
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[4, 'asc']],
                stateSave: true,
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#Table').DataTable();
        });
    </script>
</asp:Content>
