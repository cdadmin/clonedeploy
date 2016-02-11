<%@ Page Title="" Language="C#" MasterPageFile="~/views/Site.master" AutoEventWireup="true" CodeFile="firstrun.aspx.cs" Inherits="views_login_firstrun" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Breadcrumb" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubNav" Runat="Server">
    <h4>Clone Deploy Initial Setup</h4>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" Runat="Server">
    <h5>Change The Admin Password:</h5>
    <br />
    <div class="size-4 column">
        New Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtUserPwd" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Confirm Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtUserPwdConfirm" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
     <br class="clear"/>
    <br />
    <h5>Enter The IP Address Of The Clone Deploy Server:</h5>
    <br/>
    <div class="size-4 column">
        Server IP:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtServerIP" runat="server" CssClass="textbox" ></asp:TextBox>
    </div>
    <br class="clear"/>
    <br/>
    
     <h5>An SMB Share Is Used For Uploading And Deploying Images.</h5>
    <h5>Create A Read Only Password And A Read/Write Password For These Accounts.</h5>
    <h5>If You Used The Automated Windows Installer, These Must Match The Values You Used During Installation.</h5>
    <br/>
    <div class="size-4 column">
        Read Only Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtReadOnly" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Read/Write Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtReadWrite" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
    <br class="clear"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageActions" Runat="Server">
      <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Finalize Setup" CssClass="submits actions green"/>
</asp:Content>

