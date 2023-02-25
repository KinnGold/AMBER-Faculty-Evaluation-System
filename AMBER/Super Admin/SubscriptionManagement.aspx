<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="SubscriptionManagement.aspx.cs" Inherits="AMBER.Super_Admin.SubscriptionManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="rowm mb-5 shadow-lg">
            <div class="col">
                <div class="card" style="background-image: url('../../Pictures/former.png'); background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-credit-card"></i>
                                Subscription Management</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>

        <div style="width: 100% 1000px !important; max-width: 1000px !important;">
            <div class="row">
                <div class="col">
                    <div class="card shadow-lg">
                        <div class="card-body scrollbar-primary">
                            <asp:GridView ID="GridView1" class="table table-striped table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="sub_id" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="Subscription ID" InsertVisible="False" SortExpression="sub_id">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("sub_id") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("sub_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subscription Type" SortExpression="subscription_type">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtsubtype" CssClass="form-control" runat="server" Text='<%# Bind("subscription_type") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("subscription_type") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subscription Type" SortExpression="subscription_plan">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtsubplan" CssClass="form-control" runat="server" Text='<%# Bind("subscription_plan") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelPlease" runat="server" Text='<%# Bind("subscription_plan") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School" SortExpression="school_name" ControlStyle-Width="200px">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlschool" Enabled="false" runat="server" CssClass="form-select" DataSourceID="SqlDataSource1" DataTextField="school_name" DataValueField="Id" AutoPostBack="true" SelectedValue='<%# Bind("school_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT [Id],[school_name] FROM [SCHOOL_TABLE]"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelPlease5" runat="server" Text='<%# Bind("school_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Admin" SortExpression="NAME" ControlStyle-Width="200px">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlAdmin" runat="server" CssClass="form-select" DataSourceID="SqlDataSource6" DataTextField="NAME" Enabled="false" DataValueField="Id" SelectedValue='<%# Bind("admin_id") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString='<%$ ConnectionStrings:amberDB %>' SelectCommand="SELECT Id, (admin.fname +' '+ SUBSTRING(admin.mname, 1, 1)+'. '+admin.lname) AS NAME FROM ADMIN_TABLE AS admin"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Date" SortExpression="startDate">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtstart" CssClass="form-control" TextMode="DateTimeLocal" Text='<%# Bind("startDate") %>' runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("startDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Date" SortExpression="endDate">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtend" CssClass="form-control" TextMode="DateTimeLocal" Text='<%# Bind("endDate") %>' runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("endDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="status">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("status") %>' Enabled="false" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("status") %>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="Updatebtn" runat="server" CausesValidation="True" CommandName="Update" Text="Update"><i class="fa-regular fa-circle-check"></i></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="Cancelbtn" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ControlStyle CssClass="btn btn-success"></ControlStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ControlStyle CssClass="btn btn-success"></ControlStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return DeleteConfirm(this);"><i class="fa-solid fa-trash-can"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ControlStyle CssClass="btn btn-danger" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".table").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                order: [[4, 'asc']],
                stateSave: true,
                "scrollX": true,
                fixedColumns: {
                    left: 1,
                    right: 2
                }
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#Table').DataTable();
        });
    </script>
</asp:Content>
