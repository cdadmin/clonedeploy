<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" CodeFile="client.aspx.cs" Inherits="views_admin_client" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/admin/client.aspx") %>">Client Settings</a></li>
    </asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
      <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits actions" target="_blank">Help</a>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Client Settings" OnClick="btnUpdateSettings_OnClick" CssClass="submits actions"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
    $(document).ready(function() {
        $('#client').addClass("nav-current");
    });
</script>
    <div id="settings">
       

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
    </div>
</asp:Content>

