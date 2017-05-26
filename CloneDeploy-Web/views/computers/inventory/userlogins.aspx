<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/inventory/inventory.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.computers.inventory.views_computers_inventory_logins" Codebehind="userlogins.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>User Logins</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#logins').addClass("nav-current");
        });
    </script>

    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="txtSearch_OnTextChanged"></asp:TextBox>
    </div>
    <br class="clear"/>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvLogins" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gvLogins_OnSorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="LoginTime" HeaderText="Login Time" SortExpression="LoginTime" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="LogoutTime" HeaderText="Logout Time" SortExpression="LogoutTime" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
        </Columns>
        <EmptyDataTemplate>
            No Logins Found
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>