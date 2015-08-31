<%@ Page Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" CodeFile="reports.aspx.cs" Inherits="views.admin.Reports" %>

<%@ MasterType VirtualPath="~/views/admin/Admin.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#reportsSettings').addClass("nav-current");
        });
    </script>
    <div class="size-6 column">
        <h4>Last 5 Logins</h4>
        <asp:GridView ID="gvLastFiveUsers" runat="server" Width="350px" CssClass="Gridview" ShowHeader="false" AlternatingRowStyle-CssClass="alt">
            <EmptyDataTemplate>
                No Data
            </EmptyDataTemplate>
        </asp:GridView>
        <h4>Last 5 Unicasts</h4>
        <asp:GridView ID="gvLastFiveUnicasts" runat="server" CssClass="Gridview" Width="350px" ShowHeader="false" AlternatingRowStyle-CssClass="alt">
            <EmptyDataTemplate>
                No Data
            </EmptyDataTemplate>
        </asp:GridView>
        <h4>Last 5 Multicasts</h4>
        <asp:GridView ID="gvLastFiveMulticasts" runat="server" Width="350px" CssClass="Gridview" ShowHeader="false" AlternatingRowStyle-CssClass="alt">
            <EmptyDataTemplate>
                No Data
            </EmptyDataTemplate>
        </asp:GridView>
        <h4>Top 5 Unicasts</h4>
        <asp:GridView ID="gvTopFiveUnicasts" runat="server" Width="350px" CssClass="Gridview" ShowHeader="false" AlternatingRowStyle-CssClass="alt">
            <EmptyDataTemplate>
                No Data
            </EmptyDataTemplate>
        </asp:GridView>
        <h4>Top 5 Multicasts</h4>
        <asp:GridView ID="gvTopFiveMulticasts" runat="server" Width="350px" CssClass="Gridview" ShowHeader="false" AlternatingRowStyle-CssClass="alt">
            <EmptyDataTemplate>
                No Data
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <div class="size-5 column">
        <h4>User Stats</h4>
        <asp:GridView ID="gvUserStats" runat="server" CssClass="Gridview" Width="500px" ShowHeader="false" AlternatingRowStyle-CssClass="alt">
            <EmptyDataTemplate>
                No Data
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <br class="clear"/>
</asp:Content>