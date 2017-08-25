<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/cluster/cluster.master" AutoEventWireup="true" CodeBehind="newsecondary.aspx.cs" Inherits="CloneDeploy_Web.views.admin.cluster.newsecondary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" runat="server">
    <li>New Secondary Server</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubHelp" runat="server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-newsecondary.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ActionsRightSub" runat="server">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Server Settings " OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span></button>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SubContent2" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#new-secondary').addClass("nav-current");
        });
    </script>

    <div class="size-4 column">
        Base Url:
    </div>
    <div class="size-5 column">
        <asp:TextBox runat="server" Id="txtApi" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Service Account Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox runat="server" Id="txtAccountName" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Service Account Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox runat="server" Id="txtAccountPassword" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
</asp:Content>