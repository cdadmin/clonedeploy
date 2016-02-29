<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" CodeFile="server.aspx.cs" Inherits="views_admin_server" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Server Settings</li>
    </asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
      <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Server Settings" OnClick="btnUpdateSettings_OnClick" CssClass="submits actions green"/>
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
    Manual Override Web Service:
</div>
<div class="size-setting column">
    <asp:CheckBox ID="chkOverride" runat="server" ></asp:CheckBox>
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
    TFTP Path:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtTFTPPath" runat="server" CssClass="textbox"></asp:TextBox>
</div>
<br class="clear"/>

<div class="size-4 column">
    
     Computer View:
</div>
<div class="size-setting column ddl">
 
    <asp:DropDownList ID="ddlComputerView" runat="server" CssClass="ddlist" ClientIDMode="Static">
        <asp:ListItem>all</asp:ListItem>
        <asp:ListItem>search</asp:ListItem>
    </asp:DropDownList>
</div>

    </div>
    
      <div id="confirmbox" class="confirm-box-outer">
    <div class="confirm-box-inner">
        <h4>
            <asp:Label ID="lblTitle" runat="server" CssClass="modaltitle"></asp:Label>
        </h4>

        <div class="confirm-box-btns">
            <asp:LinkButton ID="OkButton" OnClick="OkButton_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
            <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            <br class="clear"/>
                <asp:Label ID="lblClientISO" runat="server" CssClass="smalltext"></asp:Label>
           
        </div>
    </div>

</div>
</asp:Content>

