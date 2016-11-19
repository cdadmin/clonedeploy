<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/inventory/inventory.master" AutoEventWireup="true" Inherits="views_computers_inventory_general" Codebehind="general.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>General</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">     
        $(document).ready(function() {      
            $('#general').addClass("nav-current");
        });     
    </script>

    <div class="size-4 column inventory-div">
        Last Update:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblLastUpdate" Text="hi" CssClass="label"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        Last Check In:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblLastCheckin"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        IP Address:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblIP"></asp:Label>     
    </div>
    <br class="clear"/>
    
      <div class="size-4 column inventory-div">
        Client Version:
    </div>
    <div class="size-5 column">
      <asp:Label runat="server" ID="lblClientVersion"></asp:Label>     
    </div>
    <br class="clear"/>
</asp:Content>

