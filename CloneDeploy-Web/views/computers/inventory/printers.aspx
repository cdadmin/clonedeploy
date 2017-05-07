<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/inventory/inventory.master" AutoEventWireup="true" Inherits="views_computers_inventory_printers" Codebehind="printers.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Printers</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#printers').addClass("nav-current");
        });
    </script>

    <div class="size-7 column">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="searchbox" OnTextChanged="txtSearch_OnTextChanged"></asp:TextBox>
    </div>
    <br class="clear"/>
    <p class="total">
        <asp:Label ID="lblTotal" runat="server"></asp:Label>
    </p>
    <asp:GridView ID="gvPrinters" runat="server" AllowSorting="True" DataKeyNames="Id" OnSorting="gvPrinters_OnSorting" AutoGenerateColumns="False" CssClass="Gridview" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Model" ItemStyle-CssClass="width_200"></asp:BoundField>
            <asp:BoundField DataField="Uri" HeaderText="Uri" SortExpression="Uri" ItemStyle-CssClass="width_200 mobi-hide-smallest" HeaderStyle-CssClass="mobi-hide-smallest"/>
        </Columns>
        <EmptyDataTemplate>
            No Printers Found
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>