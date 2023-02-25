<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master"  AutoEventWireup="true" CodeBehind="Evaluateins.aspx.cs" Inherits="AMBER.Pages.Users.Student.Evaluateins" %>
<%--MaintainScrollPositionOnPostback="true"--%>
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

        .colored-toast .swal2-title {
            color: white;
        }

        .colored-toast .swal2-close {
            color: white;
        }

        .colored-toast .swal2-html-container {
            color: white;
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
                timer: 5500,
                timerProgressBar: true
            })
        }
    </script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3 d-flex align-items-center">
            <div class="col p-2">
                <div class="profileBG">
                     <asp:Image ID="Image1" Width="100px" Height="100px" runat="server" />
                 </div>
                <asp:Label runat="server" ID="lblINS" Visible="false"></asp:Label> <%--display instructor name--%>
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlInstructor" AutoPostBack="true" CssClass="form-select" runat="server" OnSelectedIndexChanged="ddlInstructor_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <hr />
        <div class="row px-3">
            <div class="card border">
                <h1 class="p-2 text-center"><a class="btn rounded-circle sticky-sm-top mx-2 btn-outline-warning" data-bs-toggle="modal" data-bs-target="#EvaluateRate"><i class="fa-solid fa-leaf"></i></a><asp:Label runat="server" ID="lblHeader" Text='**NO EVALUATION**'></asp:Label></h1>
                <div class="card-body scrollbar-primary" style="height: 60vh; overflow-y: auto">
            <asp:GridView ID="GridView1" CssClass="table table-bordered" BorderColor="Transparent" runat="server" AutoGenerateColumns="False" DataKeyNames="indicator_Id" ShowHeader="false" OnDataBound="GridView1_DataBound">
                <Columns>
                    <%--<asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" CssClass="card-title fw-bold" Text='<%# Bind("constructor_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="constructor_name" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Size="Large" HeaderText="Question"></asp:BoundField>
                    <%--<asp:BoundField DataField="indicator_name" HeaderText="Question"></asp:BoundField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:RadioButtonList ID="rblScore" CssClass="radioButtonList" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Poor</asp:ListItem>
                                <asp:ListItem Value="2">Fair</asp:ListItem>
                                <asp:ListItem Value="3">Satisfactory</asp:ListItem>
                                <asp:ListItem Value="4">Very Satisfactory</asp:ListItem>
                                <asp:ListItem Value="5">Outstanding</asp:ListItem>
                            </asp:RadioButtonList>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <div class="card" style="max-width: 100%">
                                <div class="card-body">
                                    <p class="card-text"><asp:Label runat="server" Text='<%# Eval("row_num") %>'></asp:Label><asp:Label runat="server" Text="">. </asp:Label><asp:Label runat="server" Text='<%# Eval("indicator_name") %>'></asp:Label></p>
                            <asp:RadioButtonList ID="rblScore" CssClass="radioButtonList" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                            </asp:RadioButtonList>
                                    </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                </div>
                </div>
        </div>
        <div class="row p-3">
            <div class="form-group">
                <asp:Label runat="server" ID="lblcomment" >Leave a Comment </asp:Label>
                <asp:Label runat="server" ID="lblcommentins" ></asp:Label>
                <asp:Label runat="server" ID="Label5" > (optional)</asp:Label>
                <textarea runat="server" id="txtComment" class="form-control" cols="20" rows="2"></textarea>
                <div class="col">
                    <asp:CheckBox ID="chkConfirm" runat="server"/>
                    <asp:Label runat="server" CssClass="form-check-label" ID="Label4" >I confirmed that the above responses were accomplished objectively and I read them before answering the items</asp:Label>
                </div>
            </div>
        </div>
        <div class="row px-3">
            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="btn btn-primary" runat="server" Text="Submit" />
        </div>
    </div>
    <div class="modal fade" id="EvaluateRate" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">AMBER:Faculty Evaluation System</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Label runat="server" ID="Label1" style="font-weight:lighter" Text='Instruction: Please evaluate the faculty using the scale below'></asp:Label>
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th scope="col">Scale</th>
                                <th scope="col" style="text-align:center">Descriptive Rating</th>
                                <th scope="col">Description</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th scope="row" style="text-align:center">5</th>
                                <td>Outstanding</td>
                                <td>The Instructor's performance is <span style="font-weight:bold;">EXCEPTIONAL</span> of the job requirements</td>
                            </tr>
                            <tr>
                                <th scope="row" style="text-align:center">4</th>
                                <td>Very Satisfactory</td>
                                <td>The Instructor's performance <span style="font-weight:bold;">MEETS</span> and <span style="font-weight:bold;">EXCEEDS</span> the job requirements</td>
                            </tr>
                            <tr>
                                <th scope="row" style="text-align:center">3</th>
                                <td>Satisfactory</td>
                                <td>The Instructor's performance <span style="font-weight:bold;">MEETS</span> the job requirements</td>
                            </tr>
                            <tr>
                                <th scope="row" style="text-align:center">2</th>
                                <td>Fair</td>
                                <td>The Instructor's performance <span style="font-weight:bold;">NEEDS SOME AREAS</span> of <span style="font-weight:bold;">DEVELOPMENT</span> to meet the job requirements</td>
                            </tr>
                            <tr>
                                <th scope="row" style="text-align:center">1</th>
                                <td>Poor</td>
                                <td>The Instructor's performance <span style="font-weight:bold;">NEEDS DEVELOPMENT</span> to meet the job requirements</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Ok</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //$(window).load(function () {
        //    if (!localStorage.getItem('firstLoad')) {
        //        localStorage['firstLoad'] = false;
        //        $('#EvaluateRate').modal('show');
        //    }
        //    else
        //        localStorage.removeItem('firstLoad');
        //});
        $(window).on('load', function () {
            if (!sessionStorage.getItem('shown-modal')) {
                $('#EvaluateRate').modal('show');
                sessionStorage.setItem('shown-modal', 'true');
            }
        });
    </script>
</asp:Content>
