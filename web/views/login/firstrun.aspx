<%@ Page Title="" Language="C#" MasterPageFile="~/views/site.master" AutoEventWireup="true" CodeFile="firstrun.aspx.cs" Inherits="views_login_firstrun" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Breadcrumb" Runat="Server">
    <h4>CloneDeploy Initial Setup</h4>
    <br/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubNav" Runat="Server">
    &nbsp;
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
    <h5>Enter The IP Address Of The CloneDeploy Server:</h5>
    <br/>
    <div class="size-4 column">
        Server IP:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtServerIP" runat="server" CssClass="textbox" ></asp:TextBox>
    </div>
    <br class="clear"/>
    <br/>
    
     <h5></h5>
    <h5>Enter The SMB Share Passwords That Were Used During Installation.</h5>

    <br/>
    <div class="size-4 column">
        Read Only Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtReadOnly" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Read / Write Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtReadWrite" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
    <br class="clear"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageActions" Runat="Server">
      <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Finalize Setup" CssClass="btn btn-default"/>
</asp:Content>

