<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.admin.views_admin_multicast" Codebehind="multicast.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Multicast Settings</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-multicast.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Multicast Settings " OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
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
        Decompress Image:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlDecompress" runat="server" CssClass="ddlist">
            <asp:ListItem>client</asp:ListItem>
            <asp:ListItem>server</asp:ListItem>
        </asp:DropDownList>
    </div>


</asp:Content>