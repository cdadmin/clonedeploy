<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/user.master" AutoEventWireup="true" Inherits="views.users.CreateUser" Codebehind="create.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>New</li>
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
      <li role="separator" class="divider"></li>
     <li><a href="<%= ResolveUrl("~/views/help/users-newedit.aspx#ausers")%>"  target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Add User" CssClass="btn btn-default width_100" />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#createuser').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        User Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        User Role:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddluserMembership" runat="server" CssClass="ddlist" >
            <asp:ListItem>Administrator</asp:ListItem>
            <asp:ListItem>User</asp:ListItem>
            <asp:ListItem>Service Account</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Use LDAP Authentication:
    </div>
    <div class="size-setting column">
        <asp:CheckBox ID="chkldap" runat="server" AutoPostBack="True" OnCheckedChanged="chkldap_OnCheckedChanged"></asp:CheckBox>
    </div>
    <br class="clear" />
    <div id="passwords" runat="server">
    <div class="size-4 column">
        User Password:
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
    <br class="clear" />
        </div>
    <div class="size-4 column">
        Email:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Security Token:
        <asp:LinkButton ID="btnGenKey" runat="server" Text="Generate" OnClick="btnGenKey_OnClick" CssClass="generic-btn" Style="margin: 0"/>
    </div>
    <div class="size-9 column">
        <asp:TextBox ID="txtToken" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <br />
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