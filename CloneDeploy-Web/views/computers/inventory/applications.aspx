<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/inventory/inventory.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.computers.inventory.views_computers_inventory_applications" Codebehind="applications.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Applications</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#applications').addClass("nav-current");
        });
    </script>

    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="txtSearch_OnTextChanged"></asp:TextBox>
    </div>
    <br class="clear"/>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvApplications" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gvApplications_OnSorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>

            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Version" HeaderText="Version" SortExpression="Version" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
            <asp:BoundField DataField="Guid" HeaderText="GUID" SortExpression="Guid" ItemStyle-CssClass="width_200 mobi-hide-smaller" HeaderStyle-CssClass="mobi-hide-smaller"/>
        </Columns>
        <EmptyDataTemplate>
            No Applications Found
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>