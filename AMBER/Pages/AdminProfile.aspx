<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="AdminProfile.aspx.cs" Inherits="AMBER.Pages.AdminProfile" %>

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
            margin-top: 2rem;
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

        .schoolprofileBG {
            position: relative;
            width: 150px;
            height: 150px;
            border: 5px solid orange;
            border-radius: 50%;
            background-size: 100% 100%;
            overflow: hidden;
            margin-top: 2rem;
        }

        .school_profile {
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

            .school_profile::-webkit-file-upload-button {
                visibility: hidden;
            }

            .school_profile::before {
                content: '\f030';
                font-family: fontAwesome;
                font-size: 25px;
                color: white;
                display: inline-block;
                -webkit-user-select: none;
            }

            .school_profile:hover {
                opacity: 1;
            }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid min-vh-100 m-0 justify-content-center bg-transparent" style="background-image: url(../Pictures/1.png); background-size: cover; height: 100%; width: 100%;">
        <div class="row">
            <%-- L O G O --%>
            <div class="col-11 mx-auto p-3 m-3">
                <div class="card" style="height: 500px">
                    <div class="card-body">
                        <nav class="nav nav-tabs">
                            <a type="button" href="#" class="nav-link active" data-bs-toggle="tab" data-bs-target="#adminprofile"><i class="fa-solid fa-user-gear"></i>Admin Profile</a>
                            <a type="button" href="#" class="nav-link" data-bs-toggle="tab" data-bs-target="#schoolprofile"><i class="fa-solid fa-building-columns"></i>School Profile</a>
                            <asp:PlaceHolder ID="plcSubscription" runat="server">
                                <a type="button" href="#" class="nav-link" data-bs-toggle="tab" data-bs-target="#subscription"><i class="fa-solid fa-credit-card"></i>Subscription</a>
                            </asp:PlaceHolder>

                        </nav>
                        <div class="tab-content">

                            <!-- ADMIN PROFILE TAB PANEL -->
                            <div class="tab-pane active show fade" id="adminprofile">
                                <div class="row">
                                    <div class="col-7">
                                        <div class="row mb-3">
                                            <div class="col mx-auto">
                                                <center>
                                                    <div class="profileBG">
                                                        <asp:Image ID="profilePic" runat="server" Width="140px" Height="140px" />
                                                        <asp:fileupload runat="server" oninput="body_profilePic.src=window.URL.createObjectURL(this.files[0])" id="updateProfile" class="my_profile" onchange="showButton()" xmlns:asp="#unknown" />
                                                    </div>
                                                </center>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col d-grid mx-auto">
                                                <center>
                                                    <asp:Button runat="server" Text="Save Profile Photo" class="btn text-warning" Style="width: 250px; display: none;" ID="upload" OnClick="upload_Click" />
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
                                                <asp:Label ID="lblphone" runat="server" Text="Contact #:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labelphone" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lbldatesigned" runat="server" Text="Date Signed Up:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labeldate" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-5 pt-4">
                                        <div class="row pb-3">
                                            <div class="col d-block justify-content-center">
                                                <h1 style="font-size: large">Edit Your Personal Information</h1>
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
                                                                <asp:Button ID="btneditname" CssClass="btn btn-success" OnClick="btneditname_Click" ValidationGroup="RqEditName" runat="server" Text="Save" />
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
                                                                            <asp:TextBox ID="txtnewpass" Type="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                                                            <label for="txtnewpass">New Password<asp:RequiredFieldValidator ID="RequiredFieldValidator4" SetFocusOnError="true" ValidationGroup="RqSavePass" runat="server" ControlToValidate="txtnewpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                        </div>
                                                                        <button id="show_password" class="btn btn-secondary" type="button">
                                                                            <svg xmlns="http://www.w3.org/2000/svg" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-title=" Password must be minimum of 8 and must contain atleast one Upper case and atleast one special character and numbers." width="16" height="16" fill="currentColor" class="bi bi-question-circle-fill" viewBox="0 0 16 16">
                                                                                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.496 6.033h.825c.138 0 .248-.113.266-.25.09-.656.54-1.134 1.342-1.134.686 0 1.314.343 1.314 1.168 0 .635-.374.927-.965 1.371-.673.489-1.206 1.06-1.168 1.987l.003.217a.25.25 0 0 0 .25.246h.811a.25.25 0 0 0 .25-.25v-.105c0-.718.273-.927 1.01-1.486.609-.463 1.244-.977 1.244-2.056 0-1.511-1.276-2.241-2.673-2.241-1.267 0-2.655.59-2.75 2.286a.237.237 0 0 0 .241.247zm2.325 6.443c.61 0 1.029-.394 1.029-.927 0-.552-.42-.94-1.029-.94-.584 0-1.009.388-1.009.94 0 .533.425.927 1.01.927z" />
                                                                            </svg>
                                                                        </button>
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
                                                                                Confirm New Password<asp:RequiredFieldValidator ID="RequiredFieldValidator10" SetFocusOnError="true" ValidationGroup="RqSavePass" runat="server" ControlToValidate="txtconfirmnewpass" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                <asp:Button ID="savepassbtn" CssClass="btn btn-success" OnClick="savepassbtn_Click" ValidationGroup="RqSavePass" runat="server" Text="Save" />
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
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditschoolposition" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditschoolposition">Work Position<asp:RequiredFieldValidator ID="RequiredFieldValidator8" SetFocusOnError="true" ValidationGroup="RqEditCon" runat="server" ControlToValidate="txteditschoolposition" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                <asp:Button ID="btnconinfo" CssClass="btn btn-success" OnClick="btnconinfo_Click" ValidationGroup="RqEditCon" runat="server" Text="Save" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col">
                                                <div class="col d-grid justify-content-evenly d-block">
                                                    <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#editUsername">Edit Username
                                                    </a>
                                                </div>
                                                <div class="modal fade" id="editUsername" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel2" aria-hidden="true">
                                                    <div class="modal-dialog modal-dialog-centered">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <h5 class="modal-title" id="staticBackdropLabel7">Edit Your Username</h5>
                                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="form-group">
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txtEditUsername" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txtEditUsername">Username<asp:RequiredFieldValidator ID="RequiredFieldValidator14" SetFocusOnError="true" ValidationGroup="RqEditUsername" runat="server" ControlToValidate="txtEditUsername" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                <asp:Button ID="btnEditUsername" OnClick="btnEditUsername_Click" CssClass="btn btn-success" ValidationGroup="RqEditUsername" runat="server" Text="Save" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <!-- SCHOOL PROFILE TAB PANEL -->
                            <div class="tab-pane fade" id="schoolprofile">
                                <div class="row">
                                    <div class="col-7">
                                        <div class="row mb-3">
                                            <div class="col mx-auto">
                                                <center>
                                                    <div class="schoolprofileBG">
                                                        <asp:Image ID="schoolProfilePic" runat="server" Width="140px" Height="140px" />
                                                        <asp:PlaceHolder ID="plcuploadicon" runat="server">
                                                            <asp:fileupload runat="server" oninput="body_schoolProfilePic.src=window.URL.createObjectURL(this.files[0])" id="updateSchoolProfile" class="school_profile" onchange="showSchoolButton()" xmlns:asp="#unknown" />
                                                        </asp:PlaceHolder>
                                                    </div>
                                                </center>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col d-grid mx-auto">
                                                <center>
                                                    <asp:button runat="server" text="Save School Profile Photo" class="btn text-warning" style="width: 250px; display: none;" id="updateschoolPic" onclick="updateschoolPic_Click" xmlns:asp="#unknown" />
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
                                                    <h5>School Information
                                                    </h5>
                                                </center>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lblposition" runat="server" Text="Work Position:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labelposition" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lblschool" runat="server" Text="School:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labelschool" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lbladdress" runat="server" Text="School Address:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labeladdress" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lblschoolphone" runat="server" Text="School Phone:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labelschoolphone" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-5">
                                        <div class="row pt-4">
                                            <div class="col d-block justify-content-center">
                                                <h1 style="font-size: large">Edit Your School Information</h1>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <div class="col d-grid justify-content-evenly d-block">
                                                    <a class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#EditSchoolModal">Edit School Information
                                                    </a>
                                                </div>
                                                <div class="modal fade" id="EditSchoolModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel2" aria-hidden="true">
                                                    <div class="modal-dialog modal-dialog-centered">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <h5 class="modal-title" id="staticBackdropLabel6">Edit Your School Information</h5>
                                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="form-group">
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditschoolname" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditschoolname">School Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="RqEditSchool" runat="server" ControlToValidate="txteditschoolname" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditaddress" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditaddress">School Address<asp:RequiredFieldValidator ID="RequiredFieldValidator11" SetFocusOnError="true" ValidationGroup="RqEditSchool" runat="server" ControlToValidate="txteditaddress" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                    <div class="form-floating mb-3">
                                                                        <asp:TextBox ID="txteditschoolphone" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <label for="txteditschoolphone">School Phone Number<asp:RequiredFieldValidator ID="RequiredFieldValidator12" SetFocusOnError="true" ValidationGroup="RqEditSchool" runat="server" ControlToValidate="txteditschoolphone" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                                                <asp:Button ID="btnsaveschoolinfo" CssClass="btn btn-success" ValidationGroup="RqEditSchool" runat="server" Text="Save" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- SUBSCRIPTION TAB PANEL -->
                            <div class="tab-pane fade" id="subscription">

                                <div class="row">
                                    <div class="col-7">
                                        <div class="row">
                                            <div class="col">
                                                <center>
                                                    <img src="../Pictures/creditcard.png" style="height: 150px; width: 150px;" />
                                                </center>
                                                <hr />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <center>
                                                    <h5>Subscription Information
                                                    </h5>
                                                </center>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lblsubtype" runat="server" Text="Subscription Type:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labelsubtype" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lblsubplan" runat="server" Text="Subscription Plan:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labelsubplan" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lblstart" runat="server" Text="Subscription Start Date:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labelstart" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Label ID="lblend" runat="server" Text="Subscription End Date:"></asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:Label ID="labelend" Font-Bold="true" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-5">

                                        <div class="row pt-4">
                                            <div class="col d-block justify-content-center">
                                                <h1 style="font-size: large">Edit Your Subscription</h1>
                                            </div>
                                        </div>

                                        <asp:PlaceHolder ID="plcsubnow" runat="server">
                                            <div class="row mb-3">
                                                <div class="col">
                                                    <asp:LinkButton ID="subnowbtn" OnClick="subnowbtn_Click" CssClass="btn btn-primary" runat="server">Subscribe Now</asp:LinkButton>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID="plcsubplan" runat="server">
                                            <div class="row mb-3">
                                                <div class="col">
                                                    <asp:LinkButton ID="cancelsubsbtn" CssClass="btn btn-danger" OnClick="cancelsubsbtn_Click" runat="server">Cancel Subscription</asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <asp:LinkButton ID="btnchangeplan" OnClick="btnchangeplan_Click" CssClass="btn btn-warning" runat="server">Change Subscription Plan</asp:LinkButton>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>

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
            function showSchoolButton() {
                document.getElementById("<%=updateschoolPic.ClientID%>").style.display = "";
            }
        </script>
</asp:Content>
