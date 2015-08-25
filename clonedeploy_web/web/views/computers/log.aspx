<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Computer.master" AutoEventWireup="true" CodeFile="log.aspx.cs" Inherits="views.hosts.HostLog" %>
<%@ MasterType VirtualPath="~/views/masters/Computer.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#logoption').addClass("nav-current");
        });
    </script>
    <div class="size-4 column" style="float: right; margin: 0;">
        <asp:DropDownList ID="ddlLogType" runat="server" CssClass="ddlist" Style="float: right; width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="ddlLogLimit_SelectedIndexChanged">
            <asp:ListItem>Select A Log</asp:ListItem>
            <asp:ListItem>Upload</asp:ListItem>
            <asp:ListItem>Deploy</asp:ListItem>
        </asp:DropDownList>
        <br class="clear"/>
        <asp:DropDownList ID="ddlLogLimit" runat="server" CssClass="ddlist" Style="float: right; width: 75px;" AutoPostBack="true" OnSelectedIndexChanged="ddlLogLimit_SelectedIndexChanged">
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>75</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
        <br class="clear"/>
        <asp:LinkButton ID="btnExportLog" runat="server" Text="Export Log" CssClass="submits" OnClick="btnExportLog_Click"></asp:LinkButton>
    </div>
    <br class="clear"/>
    <asp:GridView ID="gvHostLog" runat="server" CssClass="Gridview log" ShowHeader="false">
    </asp:GridView>

</asp:Content>