<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Group.master" AutoEventWireup="true" CodeFile="history.aspx.cs" Inherits="views.groups.GroupHistory" %>

<%@ MasterType VirtualPath="~/views/masters/Group.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#historyoption').addClass("nav-current");
        });
    </script>
    <div class="size-4 column" style="float: right; margin: 0;">
        <asp:DropDownList ID="ddlLimit" runat="server" CssClass="ddlist" Style="float: right; width: 75px;" AutoPostBack="true" OnSelectedIndexChanged="ddlLimit_SelectedIndexChanged">
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
    </div>
    <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="_historyevent" HeaderText="Event" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="_historyuser" HeaderText="User" ItemStyle-CssClass="width_200"/>
            <asp:BoundField DataField="_historytime" HeaderText="Date" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
            <asp:BoundField DataField="_historyip" HeaderText="IP" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
            <asp:BoundField DataField="_historynotes" HeaderText="Notes" ItemStyle-CssClass="width_200 mobi-hide-small" HeaderStyle-CssClass="mobi-hide-small"/>
        </Columns>
    </asp:GridView>
</asp:Content>