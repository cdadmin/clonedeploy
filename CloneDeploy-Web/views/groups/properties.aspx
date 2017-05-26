<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.groups.views_groups_properties" Codebehind="properties.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>
        <a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>"><%= Group.Name %></a>
    </li>
    <li>Computer Properties</li>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/groups-properties.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnSubmit" runat="server" Text="Update Properties" OnClick="btnSubmit_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#properties').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Set As Default For New Group Members:
    </div>
    <div class="size-5 column">
        <asp:CheckBox runat="server" ID="chkDefault" AutoPostBack="True" OnCheckedChanged="chkDefault_OnCheckedChanged"/>
    </div>
    <br class="clear"/>
    <br/>
    <div class="size-4 column">
        Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlComputerImage" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlComputerImage_OnSelectedIndexChanged"/>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkImage"/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Image Profile:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlImageProfile" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkProfile"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Computer Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtComputerDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkDescription"/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Cluster Group:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlClusterGroup" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkClusterGroup"/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Site:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlSite" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkSite"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Building:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlBuilding" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkBuilding"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Room:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlRoom" runat="server" CssClass="ddlist"/>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkRoom"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Custom Attribute 1:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom1" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkCustom1"/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Custom Attribute 2:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom2" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkCustom2"/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Custom Attribute 3:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom3" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkCustom3"/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Custom Attribute 4:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom4" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkCustom4"/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Custom Attribute 5:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom5" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkCustom5"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Enable Proxy DHCP Reservation:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkProxyEnabled" runat="server"></asp:CheckBox>

    </div>
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkProxyReservation"/>
    </div>
    <br class="clear"/>


    <div class="size-4 column">
        TFTP Server IP:

    </div>
    <div class="size-5 column">
        <asp:TextBox runat="server" ID="txtTftp" CssClass="textbox" Text="[server-ip]"></asp:TextBox>
    </div>

    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkTftp"/>
    </div>
    <br class="clear"/>

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
    <div class="size-12 column">
        <asp:CheckBox runat="server" Id="chkBootFile"/>
    </div>
    <br class="clear"/>
</asp:Content>