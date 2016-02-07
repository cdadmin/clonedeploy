<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" CodeFile="client.aspx.cs" Inherits="views_admin_client" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Client</li>
    </asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
      <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Client Settings" OnClick="btnUpdateSettings_OnClick" CssClass="submits actions green"/>
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
    Global Computer Arguments:
</div>
<div class="size-setting column">
    <asp:TextBox ID="txtGlobalComputerArgs" runat="server" CssClass="textbox"></asp:TextBox>
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
           
           
        </div>
    </div>

</div>
</asp:Content>

