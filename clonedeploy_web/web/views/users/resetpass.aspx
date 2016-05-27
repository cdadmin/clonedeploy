<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/user.master" AutoEventWireup="true" Inherits="views.users.ResetPass" CodeFile="resetpass.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Reset Password</li>
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>"  target="_blank">Help</a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
         <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Update User" />

</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <h4 style="margin-bottom: 20px;">Change Password</h4>
     <div class="size-4 column">
        Use LDAP Authentication:
    </div>
    <div class="size-setting column">
        <asp:CheckBox ID="chkldap" runat="server" Enabled="False"></asp:CheckBox>
    </div>
    <br class="clear" />
    <div id="passwords" runat="server">
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
        </div>
    <div class="size-4 column">
        Email:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
     <div class="size-4 column">
        Notify On Lockout:
    </div>
    <div class="size-5 column">
        <asp:Checkbox ID="chkLockout" runat="server"></asp:Checkbox>
    </div>
    <br class="clear"/>
    <br />
    <div class="size-4 column">
        Notify On Task Error:
    </div>
    <div class="size-5 column">
        <asp:Checkbox ID="chkError" runat="server"></asp:Checkbox>
    </div>
    <br class="clear"/>
    <br />
    <div class="size-4 column">
        Notify On Task Complete:
    </div>
    <div class="size-5 column">
        <asp:Checkbox ID="chkComplete" runat="server"></asp:Checkbox>
    </div>
    <br class="clear"/>
    <br />
    <div class="size-4 column">
        Notify On Image Approved:
    </div>
    <div class="size-5 column">
        <asp:Checkbox ID="chkApproved" runat="server"></asp:Checkbox>
    </div>
    <br class="clear"/>
    <br />
   

   
</asp:Content>