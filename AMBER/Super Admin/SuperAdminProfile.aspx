<%@ Page Title="" Language="C#" MasterPageFile="~/Super Admin/SuperMasterPage.Master" AutoEventWireup="true" CodeBehind="SuperAdminProfile.aspx.cs" Inherits="AMBER.Super_Admin.SuperAdminProfile" %>

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
    <div class="container-fluid min-vh-100 m-0 d-flex flex-column justify-content-center bg-transparent" style="background-image: url(../Pictures/1.png); background-size: cover; height: 100%; width: 100%;">
        <div class="row">
            <div class="col-10 mx-auto">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-7">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col">
                                                <div class="row mb-3">
                                                    <div class="col mx-auto">
                                                        <center>
                                                            <div class="profileBG">
                                                                <asp:Image ID="profilePic" runat="server" Width="140px" Height="140px" />
                                                                <asp:fileupload runat="server" id="updateProfile" oninput="body_profilePic.src=window.URL.createObjectURL(this.files[0])" class="my_profile" onchange="showButton()" xmlns:asp="#unknown" />
                                                            </div>
                                                        </center>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col d-grid mx-auto">
                                                        <center>
                                                            <asp:Button runat="server" Text="Update Profile Photo" class="btn text-warning" Style="width: 250px; display: none;" ID="upload" OnClick="upload_Click" />
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
                                                            <h5>Personal Information
                                                            </h5>
                                                        </center>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col">
                                                        <asp:Label ID="lblusername" runat="server" Text="Username:"></asp:Label>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="labelusername" Font-Bold="true" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col">
                                                        <asp:Label ID="lblname" runat="server" Text="Name:"></asp:Label>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="labelname" Font-Bold="true" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col">
                                                        <asp:Label ID="lblemail" runat="server" Text="Email:"></asp:Label>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="labelemail" Font-Bold="true" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col">
                                                        <asp:Label ID="lblcnum" runat="server" Text="Contact #:"></asp:Label>
                                                    </div>
                                                    <div class="col">
                                                        <asp:Label ID="labelcnum" Font-Bold="true" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-5">
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
                                                    <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#EditNameModal">Edit Your Information
                                                    </a>
                                                </div>
                                                <div class="modal fade" id="EditNameModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel2" aria-hidden="true">
                                                    <div class="modal-dialog modal-dialog-centered">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <h5 class="modal-title" id="staticBackdropLabel2">Edit Your Name</h5>
                                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="form-group">
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditfname" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditfname">First Name<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ValidationGroup="RqEditName" runat="server" ControlToValidate="txteditfname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditmname" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditmname">Middle Name<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ValidationGroup="RqEditName" runat="server" ControlToValidate="txteditmname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditlname" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditlname">Last Name<asp:RequiredFieldValidator ID="RequiredFieldValidator5" SetFocusOnError="true" ValidationGroup="RqEditName" runat="server" ControlToValidate="txteditlname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                <asp:Button ID="btneditname" CssClass="btn btn-success" ValidationGroup="RqEditName" runat="server" Text="Save" OnClick="btneditname_Click" />
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
                                                                    <div class="form-floating">
                                                                        <div class="col-lg-1">
                                                                            <svg xmlns="http://www.w3.org/2000/svg" data-bs-toggle="tooltip" data-bs-placement="left" data-bs-title=" Password must be minimum of 8 and must contain atleast one Upper case and atleast one special character and numbers." width="16" height="16" fill="currentColor" class="bi bi-question-circle-fill" viewBox="0 0 16 16">
                                                                                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.496 6.033h.825c.138 0 .248-.113.266-.25.09-.656.54-1.134 1.342-1.134.686 0 1.314.343 1.314 1.168 0 .635-.374.927-.965 1.371-.673.489-1.206 1.06-1.168 1.987l.003.217a.25.25 0 0 0 .25.246h.811a.25.25 0 0 0 .25-.25v-.105c0-.718.273-.927 1.01-1.486.609-.463 1.244-.977 1.244-2.056 0-1.511-1.276-2.241-2.673-2.241-1.267 0-2.655.59-2.75 2.286a.237.237 0 0 0 .241.247zm2.325 6.443c.61 0 1.029-.394 1.029-.927 0-.552-.42-.94-1.029-.94-.584 0-1.009.388-1.009.94 0 .533.425.927 1.01.927z" />
                                                                            </svg>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-group mb-3">
                                                                        <div class="form-floating">
                                                                            <asp:TextBox ID="txtcurrpass" TextMode="Password" CssClass="form-control" runat="server">
                                                                            </asp:TextBox>
                                                                            <label for="txtcurrpass">
                                                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-lock" viewBox="0 0 16 16">
                                                                                    <path d="M5.338 1.59a61.44 61.44 0 0 0-2.837.856.481.481 0 0 0-.328.39c-.554 4.157.726 7.19 2.253 9.188a10.725 10.725 0 0 0 2.287 2.233c.346.244.652.42.893.533.12.057.218.095.293.118a.55.55 0 0 0 .101.025.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067c-.53 0-1.552.223-2.662.524zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
                                                                                    <path d="M9.5 6.5a1.5 1.5 0 0 1-1 1.415l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99a1.5 1.5 0 1 1 2-1.415z" />
                                                                                </svg>
                                                                                Password<asp:RequiredFieldValidator ID="RequiredFieldValidator9" SetFocusOnError="true" ValidationGroup="RqSavePass" runat="server" ControlToValidate="txtcurrpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                        </div>
                                                                    </div>

                                                                    <div class="input-group mb-3">
                                                                        <div class="form-floating">
                                                                            <asp:TextBox ID="txtnewpass" TextMode="Password" CssClass="form-control" runat="server">
                                                                            </asp:TextBox>
                                                                            <label for="txtnewpass">
                                                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-lock" viewBox="0 0 16 16">
                                                                                    <path d="M5.338 1.59a61.44 61.44 0 0 0-2.837.856.481.481 0 0 0-.328.39c-.554 4.157.726 7.19 2.253 9.188a10.725 10.725 0 0 0 2.287 2.233c.346.244.652.42.893.533.12.057.218.095.293.118a.55.55 0 0 0 .101.025.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067c-.53 0-1.552.223-2.662.524zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
                                                                                    <path d="M9.5 6.5a1.5 1.5 0 0 1-1 1.415l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99a1.5 1.5 0 1 1 2-1.415z" />
                                                                                </svg>
                                                                                Password<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqSavePass" runat="server" ControlToValidate="txtnewpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                        </div>
                                                                    </div>

                                                                    <div class="input-group mb-3">
                                                                        <div class="form-floating">
                                                                            <asp:TextBox ID="txtconfirmnewpass" TextMode="Password" CssClass="form-control" runat="server">
                                                                            </asp:TextBox>
                                                                            <label for="txtconfirmnewpass">
                                                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-shield-lock" viewBox="0 0 16 16">
                                                                                    <path d="M5.338 1.59a61.44 61.44 0 0 0-2.837.856.481.481 0 0 0-.328.39c-.554 4.157.726 7.19 2.253 9.188a10.725 10.725 0 0 0 2.287 2.233c.346.244.652.42.893.533.12.057.218.095.293.118a.55.55 0 0 0 .101.025.615.615 0 0 0 .1-.025c.076-.023.174-.061.294-.118.24-.113.547-.29.893-.533a10.726 10.726 0 0 0 2.287-2.233c1.527-1.997 2.807-5.031 2.253-9.188a.48.48 0 0 0-.328-.39c-.651-.213-1.75-.56-2.837-.855C9.552 1.29 8.531 1.067 8 1.067c-.53 0-1.552.223-2.662.524zM5.072.56C6.157.265 7.31 0 8 0s1.843.265 2.928.56c1.11.3 2.229.655 2.887.87a1.54 1.54 0 0 1 1.044 1.262c.596 4.477-.787 7.795-2.465 9.99a11.775 11.775 0 0 1-2.517 2.453 7.159 7.159 0 0 1-1.048.625c-.28.132-.581.24-.829.24s-.548-.108-.829-.24a7.158 7.158 0 0 1-1.048-.625 11.777 11.777 0 0 1-2.517-2.453C1.928 10.487.545 7.169 1.141 2.692A1.54 1.54 0 0 1 2.185 1.43 62.456 62.456 0 0 1 5.072.56z" />
                                                                                    <path d="M9.5 6.5a1.5 1.5 0 0 1-1 1.415l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99a1.5 1.5 0 1 1 2-1.415z" />
                                                                                </svg>
                                                                                Password<asp:RequiredFieldValidator ID="RequiredFieldValidator10" SetFocusOnError="true" ValidationGroup="RqSavePass" runat="server" ControlToValidate="txtconfirmnewpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                <asp:Button ID="savepassbtn" CssClass="btn btn-success" ValidationGroup="RqSavePass" runat="server" Text="Save" OnClick="savepassbtn_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row pb-3">
                                            <div class="col">
                                                <div class="col d-grid justify-content-evenly d-block">
                                                    <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#EditConModal">Edit Contact Information
                                                    </a>
                                                </div>
                                                <div class="modal fade" id="EditConModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel2" aria-hidden="true">
                                                    <div class="modal-dialog modal-dialog-centered">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <h5 class="modal-title" id="staticBackdropLabel3">Edit Your Contact Information</h5>
                                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="form-group">
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditnum" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditnum">Contact Number<asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ValidationGroup="txteditnum" runat="server" ControlToValidate="txteditnum" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditemail" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditemail">Email<asp:RequiredFieldValidator ID="RequiredFieldValidator7" SetFocusOnError="true" ValidationGroup="RqEditCon" runat="server" ControlToValidate="txteditemail" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                <asp:Button ID="btnconinfo" CssClass="btn btn-success" ValidationGroup="RqEditCon" runat="server" Text="Save" OnClick="btnconinfo_Click" />
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
