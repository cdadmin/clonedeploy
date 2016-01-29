<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" Inherits="views.admin.Logview" CodeFile="logview.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Logs</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
      <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
   <asp:LinkButton ID="btnExportLog" runat="server" Text="Export Log" CssClass="submits actions green" OnClick="btnExportLog_Click"></asp:LinkButton>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#log').addClass("nav-current");
        });
    </script>
    <div class="size-7 column">
        <asp:DropDownList ID="ddlLog" runat="server" CssClass="ddlist" AutoPostBack="True">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
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
    <asp:GridView ID="GridView1" runat="server" CssClass="Gridview log" ShowHeader="false">
    </asp:GridView>
</asp:Content>