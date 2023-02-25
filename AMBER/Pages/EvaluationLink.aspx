<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="EvaluationLink.aspx.cs" Inherits="AMBER.Pages.EvaluationLink" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script>
        function SentSuccessful() {
            Swal.fire({
                position: 'center',
                showConfirmButton: false,
                timer: 3000,
                icon: 'success',
                title: ' Sent Successfully!',
            })
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-link"></i>Generate Evaluation Link</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col">
                <div class="card shadow-lg border-0" style="background-color: transparent;">
                    <div class="card-body">
                        <div class="row mb-2">
                            <div class="col-6">
                                <asp:DropDownList ID="departmentDDL" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <center>
                                    <asp:LinkButton ID="btngeneratelink" OnClick="btngeneratelink_Click" runat="server"><i class="fa-solid fa-link text-warning" title="Click Here" style="font-size: 200px; cursor: pointer;"></i></asp:LinkButton>
                                    <h5 style="cursor: pointer;">Click Here to Generate
                                    </h5>
                                </center>
                            </div>
                        </div>
                        <br />
                        <div class="input-group mb-3">
                            <div class="form-floating">
                                <asp:TextBox ID="txtLink" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                <label for="txtLink">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqSend" runat="server" ControlToValidate="txtLink" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>Link goes here</label>
                            </div>
                            <button id="copylink" class="btn btn-primary" title="Copied to Clipboard" type="button">
                                <span class="fa-solid fa-clipboard"></span>
                            </button>
                        </div>
                        <div class="row">
                            <div class="col mx-auto">
                                <center>
                                    <asp:Button runat="server" CssClass="btn btn-warning" Text="Send to Users" ID="btnsend" ValidationGroup="RqSend" OnClick="btnsend_Click" />
                                </center>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/clipboard.js/2.0.10/clipboard.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#copylink").click(function () {
                var id = "#" + "<%= txtLink.ClientID %>";
                $(id).select();
                document.execCommand("copy");

                $('#copylink').tooltip({
                    trigger: 'click',
                    placement: 'bottom'
                });

                function setTooltip(message) {
                    $('#copylink').tooltip('hide')
                        .attr('data-original-title', message)
                        .tooltip('show');
                }

                function hideTooltip() {
                    setTimeout(function () {
                        $('#copylink').tooltip('hide');
                    }, 1000);
                }

                // Clipboard
                var clipboard = new ClipboardJS('#copylink');

                clipboard.on('success', function (e) {
                    setTooltip('Copied!');
                    hideTooltip();

                });

                clipboard.on('error', function (e) {
                    setTooltip('Failed!');
                    hideTooltip();

                });
            });
        });

       // Tooltip
    </script>
</asp:Content>
