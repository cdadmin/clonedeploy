<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" CodeBehind="imageshare.aspx.cs" Inherits="CloneDeploy_Web.views.admin.imageshare" %>
<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Share Settings</li>
    </asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
     <li role="separator" class="divider"></li>
      <li><a href="<%= ResolveUrl("~/views/help/admin-share.aspx")%>"  target="_blank">Help</a></li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Server Settings " OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default" />
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
    $(document).ready(function() {
        $('#imageshare').addClass("nav-current");
    });
</script>
      <div class="size-4 column">
    Share Type:
</div>
<div class="size-setting column">
    <asp:DropDownList runat="server" id="ddlType" CssClass="ddlist">
        <asp:ListItem>Local</asp:ListItem>
        <asp:ListItem>Remote</asp:ListItem>
        </asp:DropDownList>

</div>
<br class="clear"/>
      <div class="size-4 column">
        Server Ip / Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtServer" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
   
    <div class="size-4 column">
        Share Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtShareName" runat="server" CssClass="textbox"/>
    </div>
    <br class="clear" />
     <div class="size-4 column">
        Domain / Workgroup:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDomain" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
    
     <div class="size-4 column">
        Read/Write Username:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRwUsername" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
    
     <div class="size-4 column">
        Read/Write Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRwPassword" runat="server" CssClass="textbox" TextMode="Password"/>
    </div>

    <br class="clear"/>
    
      <div class="size-4 column">
        Read Only Username:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRoUsername" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
    
     <div class="size-4 column">
        Read Only Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRoPassword" runat="server" CssClass="textbox" TextMode="Password"/>
    </div>

    <br class="clear"/>
     <div class="size-4 column">
        Physical Path:
    </div>
    <div class="size-1 column">
        <asp:TextBox ID="txtPhysicalPath" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
     <div class="size-4 column">
        Queue Size:
    </div>
    <div class="size-1 column">
        <asp:TextBox ID="txtQueueSize" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>
</asp:Content>
