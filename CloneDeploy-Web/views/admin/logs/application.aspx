<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/logs/logs.master" AutoEventWireup="true" CodeBehind="application.aspx.cs" Inherits="CloneDeploy_Web.views.admin.logs.application" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Application Logs</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-logs.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnExportLog" runat="server" Text="Export Log" OnClick="btnExportLog_Click" CssClass="btn btn-default width_100"></asp:LinkButton>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#app').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:DropDownList ID="ddlLog" runat="server" CssClass="ddlist" AutoPostBack="True">
        </asp:DropDownList>
    </div>

    <div class="size-4 column" style="float: right; margin: 0;">
        <asp:DropDownList ID="ddlLimit" runat="server" CssClass="ddlist" Style="float: right; width: 75px;" AutoPostBack="true" OnSelectedIndexChanged="ddlLimit_SelectedIndexChanged">
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
        <br class="clear"/>

    </div>
    <br class="clear"/>
    <asp:GridView ID="gvLog" runat="server" CssClass="Gridview log" ShowHeader="false">
    </asp:GridView>

</asp:Content>