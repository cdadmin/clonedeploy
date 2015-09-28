<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" CodeFile="server.aspx.cs" Inherits="views_admin_server" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/admin/server.aspx") %>">Server Settings</a></li>
    </asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
    $(document).ready(function() {
        $('#server').addClass("nav-current");
    });
</script>
<div id="settings">
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
    <div class="size-4 column">
    &nbsp;
</div>

<div class="size-setting column">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Server Settings" OnClick="btnUpdateSettings_OnClick" CssClass="submits"/>
</div>
<br class="clear"/>
    </div>
    
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
</asp:Content>

