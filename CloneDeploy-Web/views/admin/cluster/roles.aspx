<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/cluster/cluster.master" AutoEventWireup="true" CodeBehind="roles.aspx.cs" Inherits="CloneDeploy_Web.views.admin.cluster.roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Server Roles</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-cluster.aspx#admin") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Server Settings " OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#roles').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Server Operation Mode:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlOperationMode" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ddlOperationMode_OnSelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem>Single</asp:ListItem>
            <asp:ListItem>Cluster Primary</asp:ListItem>
            <asp:ListItem>Cluster Secondary</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>

    <div id="divRoles" runat="server">

        <div class="size-4 column">
            Tftp Server:
        </div>
        <div class="size-setting column">
            <asp:CheckBox runat="server" id="chkTftpServer"/>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            Multicast Server:
        </div>
        <div class="size-setting column">
            <asp:CheckBox runat="server" id="chkMulticastServer"/>
        </div>
        <br class="clear"/>
    </div>

</asp:Content>