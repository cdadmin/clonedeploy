<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" CodeFile="security.aspx.cs" Inherits="views_admin_security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Security Settings</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
    $(document).ready(function() {
        $('#security').addClass("nav-current");
    });
</script>
    <div class="size-4 column">
    Server Key:
    <asp:LinkButton ID="btnGenKey" runat="server" Text="Generate" OnClick="btnGenerate_Click" CssClass="submits" Style="margin: 0"/>
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtServerKey" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>

<div class="size-4 column">
    Image Checksum:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlImageChecksum" runat="server" CssClass="ddlist">
        <asp:ListItem>On</asp:ListItem>
        <asp:ListItem>Off</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    Force SSL:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlSSL" runat="server" CssClass="ddlist">
        <asp:ListItem>Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    On Demand Mode:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlOnd" runat="server" CssClass="ddlist">
        <asp:ListItem>Enabled</asp:ListItem>
        <asp:ListItem>Disabled</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    AD Login Domain:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtADLogin" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    Debug Requires Login:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlDebugLogin" runat="server" CssClass="ddlist">
        <asp:ListItem>Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    On Demand Requires Login:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlOndLogin" runat="server" CssClass="ddlist">
        <asp:ListItem>Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    Add Host Requires Login:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlRegisterLogin" runat="server" CssClass="ddlist">
        <asp:ListItem>Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    Web Tasks Require Login:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlWebTasksLogin" runat="server" CssClass="ddlist">
        <asp:ListItem>Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
          <div class="size-4 column">
    &nbsp;
</div>

<div class="size-setting column">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Security Settings" OnClick="btnUpdateSettings_OnClick" CssClass="submits"/>
</div>
<br class="clear"/>
</asp:Content>

