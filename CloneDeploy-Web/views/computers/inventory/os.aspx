<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/inventory/inventory.master" AutoEventWireup="true" Inherits="views_computers_inventory_os" Codebehind="os.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Operating System</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#os').addClass("nav-current");
        });
    </script>

    <div class="size-4 column inventory-div">
        Name:
    </div>
    <div class="size-5 column">
        <asp:Label runat="server" ID="lblName" CssClass="label"></asp:Label>
    </div>
    <br class="clear"/>

    <div class="size-4 column inventory-div">
        Version:
    </div>
    <div class="size-5 column">
        <asp:Label runat="server" ID="lblVersion" CssClass="label"></asp:Label>
    </div>
    <br class="clear"/>

    <div class="size-4 column inventory-div">
        Service Pack:
    </div>
    <div class="size-5 column">
        <asp:Label runat="server" ID="lblServicePack" CssClass="label"></asp:Label>
    </div>
    <br class="clear"/>

    <div class="size-4 column inventory-div">
        Service Release:
    </div>
    <div class="size-5 column">
        <asp:Label runat="server" ID="lblServiceRelease" CssClass="label"></asp:Label>
    </div>
    <br class="clear"/>
</asp:Content>