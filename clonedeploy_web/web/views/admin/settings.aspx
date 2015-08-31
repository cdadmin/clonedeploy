<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" Inherits="views.admin.AdminSettings" CodeFile="settings.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/admin/Admin.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        $('#globalSettings').addClass("nav-current");
        $('#ddlHostView').dropdown();
        $('#ddlCompLevel').dropdown();
    });
</script>
<div id="settings">
<h2 class="settingsh2">Server Settings</h2>
<div class="size-4 column">
    Server IP:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtIP" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    Web Server Port:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtPort" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    Image Store Path:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtImagePath" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    Image Hold Path:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtImageHoldPath" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    TFTP Path:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtTFTPPath" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    Web Service:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtWebService" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    
     Host View:
</div>
<div class="size-setting column ddl">
 
    <asp:DropDownList ID="ddlHostView" runat="server" CssClass="ddlist" ClientIDMode="Static">
        <asp:ListItem>all</asp:ListItem>
        <asp:ListItem>search</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<br/>
<h2 class="settingsh2">Client Settings</h2>
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

<div class="size-setting column ddl">
    <asp:DropDownList ID="ddlCompLevel" runat="server" CssClass="ddlist" ClientIDMode="Static">
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
<br/>
<h2 class="settingsh2">PXE Settings</h2>
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
        <asp:ListItem>syslinux_32_efi</asp:ListItem>
        <asp:ListItem>syslinux_64_efi</asp:ListItem>
        <asp:ListItem>ipxe_32_efi</asp:ListItem>
        <asp:ListItem>ipxe_64_efi</asp:ListItem>
        <asp:ListItem>ipxe_32_efi_snp</asp:ListItem>
        <asp:ListItem>ipxe_64_efi_snp</asp:ListItem>
        <asp:ListItem>ipxe_32_efi_snp_only</asp:ListItem>
        <asp:ListItem>ipxe_64_efi_snponly</asp:ListItem>
        <asp:ListItem>grub_64_efi</asp:ListItem>
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
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    Proxy Efi32 PXE Mode:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlProxyEfi32" runat="server" CssClass="ddlist">
        <asp:ListItem>syslinux_32_efi</asp:ListItem>
        <asp:ListItem>ipxe_32_efi</asp:ListItem>
        <asp:ListItem>ipxe_32_efi_snp</asp:ListItem>
        <asp:ListItem>ipxe_32_efi_snponly</asp:ListItem>
    </asp:DropDownList>
</div>
<br class="clear"/>
<div class="size-4 column">
    Proxy Efi64 PXE Mode:
</div>
<div class="size-setting column">
    <asp:DropDownList ID="ddlProxyEfi64" runat="server" CssClass="ddlist">
        <asp:ListItem>syslinux_64_efi</asp:ListItem>
        <asp:ListItem>ipxe_64_efi</asp:ListItem>
        <asp:ListItem>ipxe_64_efi_snp</asp:ListItem>
        <asp:ListItem>ipxe_64_efi_snponly</asp:ListItem>
        <asp:ListItem>grub_64_efi</asp:ListItem>
    </asp:DropDownList>
</div>

<br class="clear"/>
<br/>
<h2 class="settingsh2">Security Settings</h2>
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

<br/>
<h2 class="settingsh2">Multicast Settings</h2>
<div class="size-4 column">
    Sender Arguments (Server):
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSenderArgs" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    Receiver Arguments (Server):
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtRecArgs" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    Receiver Arguments (Client):
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtRecClientArgs" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    UDPcast Start Port:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtStartPort" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
<div class="size-4 column">
    UDPcast End Port:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtEndPort" runat="server" CssClass="textbox"></asp:TextBox>
</div>
    <br class="clear"/>
    <h2 class="settingsh2">E-Mail Settings</h2>
    <div class="size-4 column">
    Smtp Server:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSmtpServer" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
    
     <div class="size-4 column">
    Smtp Port:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSmtpPort" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
     <div class="size-4 column">
    Smtp SSL:
</div>
<div class="size-setting column">
     <asp:DropDownList ID="ddlSmtpSsl" runat="server" CssClass="ddlist">
           <asp:ListItem>Yes</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
    </asp:DropDownList>
</div>
    <br class="clear"/>
     <div class="size-4 column">
    Smtp Mail From:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSmtpFrom" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
     <div class="size-4 column">
    Smtp Mail To:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSmtpTo" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
     <div class="size-4 column">
    Smtp Username:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSmtpUsername" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>
     <div class="size-4 column">
    Smtp Password:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtSmtpPassword" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
</div>
<br class="clear"/>
    
      <div class="size-4 column">
    Successful Login:
</div>
<div class="size-setting column">
    <asp:CheckBox runat="server" id="chkLoginSuccess"/>
</div>
<br class="clear"/>
    <div class="size-4 column">
    Failed Login:
</div>
<div class="size-setting column">
    <asp:CheckBox runat="server" id="chkLoginFailed"/>
</div>

    <br class="clear"/>
    <div class="size-4 column">
    Task Started:
</div>
<div class="size-setting column">
    <asp:CheckBox runat="server" id="chkTaskStarted"/>
</div>
    <br class="clear"/>
    <div class="size-4 column">
    Task Completed:
</div>
<div class="size-setting column">
    <asp:CheckBox runat="server" id="chkTaskCompleted"/>
</div>
<br class="clear"/>
    

    <div class="size-4 column">
    Image Approved:
</div>
<div class="size-setting column">
    <asp:CheckBox runat="server" id="chkImageApproved"/>
</div>
    <br class="clear"/>
    <div class="size-4 column">
    Image Resize Failed:
</div>
<div class="size-setting column">
    <asp:CheckBox runat="server" id="chkResizeFailed"/>
</div>
    <br class="clear" />
    <br />
    <div class="size-4 column">
<asp:LinkButton ID="btnTestEmail" runat="server" Text="Send Test Message" OnClick="btnTestMessage_Click" CssClass="submits left" Style="margin: 0"/>
    <br class="clear"/>
        <p style="font-size:12px;">You Must Update Settings Before Sending A Test Message</p>
    </div>
        <br class="clear" />
    
    
<div class="size-4 column">
    &nbsp;
</div>

<div class="size-setting column">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Settings" OnClick="btnUpdate_Click" CssClass="submits"/>
</div>
<br class="clear"/>
<br/>
<div id="confirmbox" class="confirm-box-outer">
    <div class="confirm-box-inner">
        <h4>
            <asp:Label ID="lblTitle" runat="server" CssClass="modaltitle"></asp:Label>
        </h4>

        <div class="confirm-box-btns">
            <asp:LinkButton ID="OkButton" OnClick="OkButton_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
            <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            <h5 style="color: white;">
                <asp:Label ID="lblClientISO" runat="server" CssClass="modaltitle"></asp:Label>
            </h5>
        </div>
    </div>

</div>
</div>
</asp:Content>