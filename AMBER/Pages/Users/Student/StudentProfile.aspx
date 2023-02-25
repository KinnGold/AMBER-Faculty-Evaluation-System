<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Users/UsersMP.Master" AutoEventWireup="true" CodeBehind="StudentProfile.aspx.cs" Inherits="AMBER.Pages.Users.Student.StudentProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .profileBG {
            position: relative;
            width: 150px;
            height: 150px;
            border: 5px solid orange;
            border-radius: 50%;
            background-size: 100% 100%;
            overflow: hidden;
        }

        .my_profile {
            position: absolute;
            bottom: 0px;
            outline: none;
            color: transparent;
            width: 100%;
            box-sizing: border-box;
            padding: 0 50px;
            left: 5px;
            cursor: pointer;
            transition: 0.5s;
            background: rgba(0,0,0,0.5);
            opacity: 0;
        }

            .my_profile::-webkit-file-upload-button {
                visibility: hidden;
            }

            .my_profile::before {
                content: '\f030';
                font-family: fontAwesome;
                font-size: 25px;
                color: white;
                display: inline-block;
                -webkit-user-select: none;
            }

            .my_profile:hover {
                opacity: 1;
            }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid min-vh-100 m-0 d-flex flex-column justify-content-center bg-transparent" style="background-image: url(/../../Pictures/1.png); background-size: cover; height: 100%; width: 100%;">
        <div class="row">
            <div class="col-lg-10 mx-auto">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="row">
                                <div class="col-lg-8">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col">
                                                    <div class="row mb-3">
                                                        <div class="col mx-auto">
                                                            <center>
                                                                <div class="profileBG">
                                                                    <asp:Image ID="profilePic" runat="server" Width="150px" Height="150px" />
                                                                    <asp:FileUpload runat="server" oninput="body_profilePic.src=window.URL.createObjectURL(this.files[0])" class="my_profile" ID="updateProfile" xmlns:asp="#unknown" onchange="showButton()" />
                                                                </div>
                                                            </center>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col d-grid mx-auto">
                                                            <center>
                                                                <asp:Button runat="server" Text="Update Profile Photo" class="btn text-warning" Style="width: 250px; display:none;" xmlns:asp="#unknown" ID="upload" OnClick="upload_Click" />
                                                            </center>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <hr />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <center>
                                                                <h5>
                                                                    Personal Information
                                                                </h5>
                                                            </center>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <asp:Label ID="lblidnumber" runat="server" Text="ID Number:"></asp:Label>
                                                        </div>
                                                        <div class="col">
                                                            <asp:Label ID="displayidnum" Font-Bold="true" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col">
                                                            <asp:Label ID="studrole" runat="server" Text="Role:"></asp:Label>
                                                        </div>
                                                        <div class="col">
                                                            <asp:Label ID="displaystudrole" Font-Bold="true" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col">
                                                            <asp:Label ID="studname" runat="server" Text="Name:"></asp:Label>
                                                        </div>
                                                        <div class="col">
                                                            <asp:Label ID="displaystudname" Font-Bold="true" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col">
                                                            <asp:Label ID="insemail" runat="server" Text="Email:"></asp:Label>
                                                        </div>
                                                        <div class="col">
                                                            <asp:Label ID="displayemail" Font-Bold="true" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col">
                                                            <asp:Label ID="phonenumber" runat="server" Text="Contact #:"></asp:Label>
                                                        </div>
                                                        <div class="col">
                                                            <asp:Label ID="displayphonenumber" Font-Bold="true" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col">
                                                            <asp:Label ID="studsection" runat="server" Text="Course & Section:"></asp:Label>
                                                        </div>
                                                        <div class="col">
                                                            <asp:Label ID="displaystudsection" Font-Bold="true" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col">
                                                            <asp:Label ID="studschool" runat="server" Text="School:"></asp:Label>
                                                        </div>
                                                        <div class="col">
                                                            <asp:Label ID="displaystudschool" Font-Bold="true" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="card" style="border: hidden">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col d-block justify-content-center">
                                                    <h1 class="mx-auto" style="font-size: large">Edit Your Information</h1>
                                                    <hr />
                                                </div>
                                            </div>
                                            <div class="row pb-3">
                                                <div class="col">
                                                    <div class="col d-grid justify-content-evenly d-block">
                                                        <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#ReqEditModal">Request Edit Information
                                                        </a>
                                                    </div>
                                                    <div class="modal fade" id="ReqEditModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel2" aria-hidden="true">
                                                        <div class="modal-dialog modal-dialog-centered">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <h5 class="modal-title" id="staticBackdropLabel2">Contact Admin</h5>
                                                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="form-group">
                                                                        <div class="form-floating mb-3">
                                                                            <asp:TextBox ID="txtrequest" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
                                                                            <label for="txtrequest">Message<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqRequest" runat="server" ControlToValidate="txtrequest" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                    <asp:Button ID="btnrequest"  CssClass="btn btn-success" ValidationGroup="RqRequest" runat="server" Text="Send" OnClick="btnrequest_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row pb-3">
                                                <div class="col">
                                                    <div class="col d-grid justify-content-evenly d-block">
                                                        <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#EditPassModal">Edit Password</a>
                                                    </div>
                                                    <div class="modal fade" id="EditPassModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel1" aria-hidden="true">
                                                        <div class="modal-dialog modal-dialog-centered">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <h5 class="modal-title" id="staticBackdropLabel1">Change Password</h5>
                                                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="form-group">
                                                                        <div class="form-floating mb-3">
                                                                            <asp:TextBox ID="txtcurrpass" Type="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                                                            <label for="txtcurrpass">Current Password<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqSavePass" runat="server" ControlToValidate="txtcurrpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                        </div>
                                                                        <div class="input-group mb-3">
                                                                            <div class="form-floating">
                                                                                <asp:TextBox ID="txtnewpass" Type="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                                                            <label for="txtnewpass">New Password<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqSavePass" runat="server" ControlToValidate="txtnewpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                            </div>
                                                                            <button id="show_password" class="btn btn-secondary" type="button">
                                                                                <svg xmlns="http://www.w3.org/2000/svg" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-title=" Password must be minimum of 8 and must contain atleast one Upper case and atleast one special character and numbers." width="16" height="16" fill="currentColor" class="bi bi-question-circle-fill" viewBox="0 0 16 16">
                                                                                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.496 6.033h.825c.138 0 .248-.113.266-.25.09-.656.54-1.134 1.342-1.134.686 0 1.314.343 1.314 1.168 0 .635-.374.927-.965 1.371-.673.489-1.206 1.06-1.168 1.987l.003.217a.25.25 0 0 0 .25.246h.811a.25.25 0 0 0 .25-.25v-.105c0-.718.273-.927 1.01-1.486.609-.463 1.244-.977 1.244-2.056 0-1.511-1.276-2.241-2.673-2.241-1.267 0-2.655.59-2.75 2.286a.237.237 0 0 0 .241.247zm2.325 6.443c.61 0 1.029-.394 1.029-.927 0-.552-.42-.94-1.029-.94-.584 0-1.009.388-1.009.94 0 .533.425.927 1.01.927z" />
                                                                                </svg>
                                                                            </button>
                                                                        </div>
                                                                        <div class="form-floating mb-3">
                                                                            <asp:TextBox ID="txtconfirmnewpass" Type="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                                                            <label for="txtconfirmnewpass">Confirm New Password<asp:RequiredFieldValidator ID="RequiredFieldValidator8" SetFocusOnError="true" ValidationGroup="RqSavePass" runat="server" ControlToValidate="txtconfirmnewpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label><asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password Does not Match" ControlToValidate="txtconfirmnewpass" ControlToCompare="txtnewpass"></asp:CompareValidator>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                    <asp:Button ID="savepassbtn"  CssClass="btn btn-success" ValidationGroup="RqSavePass" runat="server" Text="Save" OnClick="savepassbtn_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function showButton() {
            document.getElementById("<%=upload.ClientID%>").style.display = "";
        }
    </script>
</asp:Content>
