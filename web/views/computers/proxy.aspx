<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" CodeFile="proxy.aspx.cs" Inherits="views_computers_proxy" %>

<asp:Content ID="Content6" ContentPlaceHolderID="BreadcrumbSub" runat="Server">
    <li><a href="<%= ResolveUrl("~/views/computers/edit.aspx") %>?computerid=<%= Computer.Id %>"><%= Computer.Name %></a></li>
    <li>Proxy DHCP Reservation</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
   <li role="separator" class="divider"></li>
    <li><a href="<%= ResolveUrl("~/views/help/computers-proxy.aspx")%>" class="" target="_blank">Help</a></li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="buttonUpdate" runat="server" OnClick="buttonUpdate_OnClick" Text="Update Computer" CssClass="btn btn-default" />
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#proxy').addClass("nav-current");

        });
    </script>

    <div class="size-4 column">
        Enable Proxy DHCP Reservation:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkEnabled" runat="server"></asp:CheckBox>

    </div>

    <br class="clear" />
    <br />

    <div class="size-4 column">
        TFTP Server IP:

    </div>
    <div class="size-5 column">
        <asp:TextBox runat="server" ID="txtTftp" CssClass="textbox" Text="[server-ip]"></asp:TextBox>
    </div>

    <br class="clear" />
    <div class="size-4 column">
        Boot File:
    </div>

    <div class="size-5 column">
        <asp:DropDownList ID="ddlBootFile" runat="server" CssClass="ddlist">
            <asp:ListItem>bios_pxelinux</asp:ListItem>
            <asp:ListItem>bios_ipxe</asp:ListItem>
             <asp:ListItem>bios_x86_winpe</asp:ListItem>
             <asp:ListItem>bios_x64_winpe</asp:ListItem>
             <asp:ListItem>efi_x86_syslinux</asp:ListItem>
            <asp:ListItem>efi_x86_ipxe</asp:ListItem>
            <asp:ListItem>efi_x86_winpe</asp:ListItem>
            <asp:ListItem>efi_x64_syslinux</asp:ListItem>
            <asp:ListItem>efi_x64_ipxe</asp:ListItem>
            <asp:ListItem>efi_x64_grub</asp:ListItem>
            <asp:ListItem>efi_x64_winpe</asp:ListItem>
           
        </asp:DropDownList>
    </div>
    <br class="clear" />




</asp:Content>
