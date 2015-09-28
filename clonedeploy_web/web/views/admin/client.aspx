<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" CodeFile="client.aspx.cs" Inherits="views_admin_client" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/admin/client.aspx") %>">Client Settings</a></li>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
    $(document).ready(function() {
        $('#client').addClass("nav-current");
    });
</script>
    <div id="settings">
        <div class="size-4 column">
    Image Transfer Mode:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlImageXfer" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ImageXfer_Changed" AutoPostBack="true">
        <asp:ListItem>nfs</asp:ListItem>
        <asp:ListItem>smb</asp:ListItem>
        <asp:ListItem>nfs+http</asp:ListItem>
        <asp:ListItem>smb+http</asp:ListItem>
        <asp:ListItem>udp+http</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    NFS Upload Path:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtNFSPath" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    NFS Deploy Path:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtNFSDeploy" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    SMB Username:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSMBUser" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    SMB Password:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSMBPass" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    SMB Path:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSMBPath" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    Compression Algorithm:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlCompAlg" runat="server" CssClass="ddlist">
        <asp:ListItem>gzip</asp:ListItem>
        <asp:ListItem>lz4</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    Compression Level:
</div>

<div class="size-setting column">
    <asp:DropDownList ID="ddlCompLevel" runat="server" CssClass="ddlist">
        <asp:ListItem>1</asp:ListItem>
        <asp:ListItem>2</asp:ListItem>
        <asp:ListItem>3</asp:ListItem>
        <asp:ListItem>4</asp:ListItem>
        <asp:ListItem>5</asp:ListItem>
        <asp:ListItem>6</asp:ListItem>
        <asp:ListItem>7</asp:ListItem>
        <asp:ListItem>8</asp:ListItem>
        <asp:ListItem>9</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    Queue Size:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtQSize" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>


<div class="size-4 column">
    Global Host Arguments:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtGlobalHostArgs" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
          <div class="size-4 column">
    &nbsp;
</div>

<div class="size-setting column">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Client Settings" OnClick="btnUpdateSettings_OnClick" CssClass="submits"/>
</div>
<br class="clear"/>
    </div>
</asp:Content>

