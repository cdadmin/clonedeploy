<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/inventory/inventory.master" AutoEventWireup="true" CodeFile="hardware.aspx.cs" Inherits="views_computers_inventory_hardware" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Hardware</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">     
        $(document).ready(function() {      
            $('#hardware').addClass("nav-current");
        });     
    </script>

    <div class="size-4 column inventory-div">
        Manufacturer:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblManufacturer" CssClass="label"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        Model:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblModel" CssClass="label"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        Serial Number:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblSerial" CssClass="label"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        UUID:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblUuid" CssClass="label"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        Processor:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblProcessor" CssClass="label"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        RAM:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblRam" CssClass="label"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        Hard Drives:
    </div>
    
    <br class="clear"/>
     <asp:GridView ID="gvHdd" runat="server" AllowSorting="true" AutoGenerateColumns="False" CssClass="Gridview" DataKeyNames="Id" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
        </Columns>
        <EmptyDataTemplate>
            No Hard Drives Found
        </EmptyDataTemplate>
    </asp:GridView>
    
       <br class="clear"/>
    
     <div class="size-4 column inventory-div">
        Alternate MAC Addresses:
    </div>
    
    <br class="clear"/>
     <asp:GridView ID="gvAltMacs" runat="server" AllowSorting="true" AutoGenerateColumns="False" CssClass="Gridview" DataKeyNames="Id" AlternatingRowStyle-CssClass="alt">
        <Columns>
            <asp:BoundField DataField="Id" Visible="False"/>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="width_200"/>
        </Columns>
        <EmptyDataTemplate>
            No Alternate MAC Adrresses Found
        </EmptyDataTemplate>
    </asp:GridView>
    
       <br class="clear"/>
      <div class="size-4 column inventory-div">
        Boot Rom:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblBootRom" CssClass="label"></asp:Label>     
    </div>
    <br class="clear"/>
</asp:Content>

