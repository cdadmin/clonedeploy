<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" CodeFile="multicast.aspx.cs" Inherits="views_admin_multicast" %>
<%@ MasterType VirtualPath="~/views/admin/admin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
      <li>Multicast Settings</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
      <script type="text/javascript">
    $(document).ready(function() {
        $('#multicast').addClass("nav-current");
    });
</script>
    
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
     <div class="size-4 column">
    &nbsp;
</div>

<div class="size-setting column">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Multicast Settings" OnClick="btnUpdateSettings_OnClick" CssClass="submits"/>
</div>
<br class="clear"/>
</asp:Content>

