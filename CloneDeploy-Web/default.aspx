<%@ Page Language="C#" AutoEventWireup="true" Inherits="CloneDeploy_Web.Default" Codebehind="default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EDGE"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1"/>
    <title>Log In</title>
    <link href="~/content/css/login.css" rel="stylesheet" type="text/css"/>
    <script type="text/javascript" src="<%= ResolveUrl("~/content/js/jquery-1.9.1.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/content/js/jquery.watermark.min.js") %>"></script>
</head>
<body>
<form id="form1" runat="server">
    <div class="loginwrapper">

        <img src="<%= ResolveUrl("~/") %>content/img/logo.png" alt="logo"/>
        <br class="clear"/>
        <b>Server:</b> <asp:DropDownList runat="server" ID="ddlApplicationServers" CssClass="logo"/>
        <asp:Login ID="WebLogin" runat="server" OnAuthenticate="WebLogin_Authenticate" DestinationPageUrl="~/views/dashboard/dash.aspx">
            <LayoutTemplate>
                <asp:TextBox ID="UserName" runat="server" CssClass="uname" ClientIDMode="Static"></asp:TextBox>
                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="password" ClientIDMode="Static"></asp:TextBox>
                <div>
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In"/>
                </div>
            </LayoutTemplate>
        </asp:Login>
        <div id="error">
            <asp:Label runat="server" ID="SessionExpired" Text="Session Has Expired" Visible="False"></asp:Label>
            <asp:Label ID="lblError" Visible="false" runat="server"></asp:Label>
        </div>
    </div>
</form>
<script type="text/javascript">
    $(document).ready(function() {
        $('.uname').watermark('User Name', { useNative: false });
        $('.password').watermark('Password', { useNative: false });
    });
</script>
</body>
</html>