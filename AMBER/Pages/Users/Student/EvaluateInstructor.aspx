<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="EvaluateInstructor.aspx.cs" Inherits="AMBER.Pages.Users.Student.EvaluateInstructor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Evaluate
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
            .colored-toast.swal2-icon-warning {
  background-color: #f8bb86 !important;
}
    </style>
    <script>
        function toast() {
            Swal.fire({
                icon: 'warning',
                title: 'You already have Evaluated this instructor',
                toast: true,
                position: 'top-right',
                iconColor: 'white',
                customClass: {
                    popup: 'colored-toast'
                },
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true
            })
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row mb-3">
            <div class="col">
                <asp:Label runat="server" ID="lblINS" Visible="false"></asp:Label> <%--display instructor name--%>
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlInstructor" AutoPostBack="true" CssClass="form-select" runat="server" OnSelectedIndexChanged="ddlInstructor_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <hr />
        <div class="row px-3">
            <asp:DropDownList ID="ddlConstructor" OnSelectedIndexChanged="ddlConstructor_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select mb-3" runat="server"></asp:DropDownList>
        </div>
        <div class="row">
            <asp:GridView ID="GridView1" CssClass="table table-striped table-bordered" runat="server" AutoGenerateColumns="False" DataKeyNames="indicator_Id">
                <Columns>
                    <%--<asp:BoundField DataField="indicator_Id" HeaderText="indicator_Id" ReadOnly="True" InsertVisible="False" SortExpression="indicator_Id"></asp:BoundField>
                    <asp:BoundField DataField="indicator_name" HeaderText="indicator_name" SortExpression="indicator_name"></asp:BoundField>
                    <asp:BoundField DataField="indicator_score" HeaderText="indicator_score" SortExpression="indicator_score"></asp:BoundField>
                    <asp:BoundField DataField="constructor_id" HeaderText="constructor_id" SortExpression="constructor_id"></asp:BoundField>
                    <asp:BoundField DataField="campus_id" HeaderText="campus_id" SortExpression="campus_id"></asp:BoundField>--%>
                    <asp:BoundField DataField="indicator_name" ItemStyle-Width="46%"  HeaderText="Question" SortExpression="indicator_name"></asp:BoundField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
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
