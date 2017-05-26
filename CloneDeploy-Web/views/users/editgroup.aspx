<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/user.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.users.views_users_editgroup" Codebehind="editgroup.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/users/editgroup.aspx") %>?groupid=<%= CloneDeployUserGroup.Id %>"><%= CloneDeployUserGroup.Name %></a>
    </li>
    <li>General</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/users-neweditgroups.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Update User Group" CssClass="btn btn-default"/>
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
        Group Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtGroupName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Group Role:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlGroupMembership" runat="server" CssClass="ddlist">
            <asp:ListItem>Administrator</asp:ListItem>
            <asp:ListItem>User</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Use LDAP Group:
    </div>
    <div class="size-setting column">
        <asp:CheckBox ID="chkldap" runat="server"></asp:CheckBox>
    </div>
    <br class="clear"/>
    <div id="divldapgroup" runat="server" Visible="False">
        <div class="size-4 column">
            LDAP Group Name:
        </div>
        <div class="size-5 column">
            <asp:TextBox ID="txtLdapGroupName" runat="server" CssClass="textbox"></asp:TextBox>
        </div>


        <br class="clear"/>
    </div>

    <br/>
</asp:Content>