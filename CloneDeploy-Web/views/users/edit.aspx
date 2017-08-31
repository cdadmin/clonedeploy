<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/user.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.users.EditUser" Codebehind="edit.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/users/edit.aspx") %>?userid=<%= CloneDeployUser.Id %>"><%= CloneDeployUser.Name %></a>
    </li>
    <li>General</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/users-newedit.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Update User" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>

</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $('#editoption').addClass("nav-current");
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
        <asp:DropDownList ID="ddluserMembership" runat="server" CssClass="ddlist">
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
        <asp:CheckBox ID="chkldap" runat="server" Enabled="False"></asp:CheckBox>
    </div>
    <br class="clear"/>
    <div id="passwords" runat="server">
        <div class="size-4 column">
            User Password:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtUserPwd" runat="server" CssClass="textbox password" TextMode="Password"></asp:TextBox>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            Confirm Password:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtUserPwdConfirm" runat="server" CssClass="textbox password" TextMode="Password"></asp:TextBox>
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
        <p style="margin-bottom: 5px;">
            Imaging Token:
            <asp:LinkButton ID="btnGenKey" runat="server" Text="Generate" OnClick="btnGenKey_OnClick" CssClass="btn btn-default right"/>
        </p>
    </div>
    <div class="size-9 column">
        <asp:TextBox ID="txtToken" runat="server" CssClass="textbox"></asp:TextBox>
    </div>

    <br class="clear"/>
    <br/>

    <div class="size-4 column">
        Notify On Lockout:
    </div>
    <div class="size-5 column">
        <asp:Checkbox ID="chkLockout" runat="server"></asp:Checkbox>
    </div>
    <br class="clear"/>
    <br/>
    <div class="size-4 column">
        Notify On Task Error:
    </div>
    <div class="size-5 column">
        <asp:Checkbox ID="chkError" runat="server"></asp:Checkbox>
    </div>
    <br class="clear"/>
    <br/>
    <div class="size-4 column">
        Notify On Task Complete:
    </div>
    <div class="size-5 column">
        <asp:Checkbox ID="chkComplete" runat="server"></asp:Checkbox>
    </div>
    <br class="clear"/>
    <br/>
    <div class="size-4 column">
        Notify On Image Approved:
    </div>
    <div class="size-5 column">
        <asp:Checkbox ID="chkApproved" runat="server"></asp:Checkbox>
    </div>
    <br class="clear"/>
    <br/>


</asp:Content>