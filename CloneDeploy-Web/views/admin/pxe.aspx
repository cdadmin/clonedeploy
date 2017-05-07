<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="views_admin_pxe" Codebehind="pxe.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>PXE Settings</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-pxe.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update PXE Settings " OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#pxe').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Using Proxy DHCP:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlProxyDHCP" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ProxyDhcp_Changed" AutoPostBack="true">
            <asp:ListItem>Yes</asp:ListItem>
            <asp:ListItem>No</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        PXE Mode:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlPXEMode" runat="server" CssClass="ddlist">
            <asp:ListItem>pxelinux</asp:ListItem>
            <asp:ListItem>ipxe</asp:ListItem>
            <asp:ListItem>syslinux_efi32</asp:ListItem>
            <asp:ListItem>syslinux_efi64</asp:ListItem>
            <asp:ListItem>ipxe_efi32</asp:ListItem>
            <asp:ListItem>ipxe_efi64</asp:ListItem>
            <asp:ListItem>ipxe_efi_snp32</asp:ListItem>
            <asp:ListItem>ipxe_efi_snp64</asp:ListItem>
            <asp:ListItem>ipxe_efi_snponly32</asp:ListItem>
            <asp:ListItem>ipxe_efi_snponly64</asp:ListItem>
            <asp:ListItem>grub</asp:ListItem>
            <asp:ListItem>winpe_bios32</asp:ListItem>
            <asp:ListItem>winpe_bios64</asp:ListItem>
            <asp:ListItem>winpe_efi32</asp:ListItem>
            <asp:ListItem>winpe_efi64</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Proxy Bios PXE Mode:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlProxyBios" runat="server" CssClass="ddlist">
            <asp:ListItem>pxelinux</asp:ListItem>
            <asp:ListItem>ipxe</asp:ListItem>
            <asp:ListItem>winpe</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Proxy Efi32 PXE Mode:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlProxyEfi32" runat="server" CssClass="ddlist">
            <asp:ListItem>syslinux</asp:ListItem>
            <asp:ListItem>ipxe_efi</asp:ListItem>
            <asp:ListItem>ipxe_snp</asp:ListItem>
            <asp:ListItem>ipxe_snponly</asp:ListItem>
            <asp:ListItem>winpe_efi</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Proxy Efi64 PXE Mode:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlProxyEfi64" runat="server" CssClass="ddlist">
            <asp:ListItem>syslinux</asp:ListItem>
            <asp:ListItem>ipxe_efi</asp:ListItem>
            <asp:ListItem>ipxe_snp</asp:ListItem>
            <asp:ListItem>ipxe_snponly</asp:ListItem>
            <asp:ListItem>grub</asp:ListItem>
            <asp:ListItem>winpe_efi</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" CssClass="modaltitle"></asp:Label>
            </h4>

            <div class="confirm-box-btns">
                <asp:LinkButton ID="OkButton" OnClick="OkButton_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>


            </div>
        </div>

    </div>
</asp:Content>