<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" CodeFile="email.aspx.cs" Inherits="views_admin_email" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>E-mail Settings</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
      <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update E-Mail Settings" OnClick="btnUpdateSettings_OnClick" CssClass="submits actions green"/>
<asp:LinkButton ID="btnTestEmail" runat="server" Text="Send Test Message" OnClick="btnTestMessage_Click" CssClass="submits actions"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
    $(document).ready(function() {
        $('#email').addClass("nav-current");
    });
</script>
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

    <br class="clear"/>
        <p style="font-size:12px;">You Must Update Settings Before Sending A Test Message</p>
    </div>

</asp:Content>

