<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/bootmenu/bootmenu.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.global.boottemplates.views_global_boottemplates_createentry" Codebehind="createentry.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New Boot Menu Entry</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/global-bootmenutemplates.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ActionsRightSub" Runat="Server">
    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_OnClick" Text="Add Entry" CssClass="btn btn-default width_100"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#createentry').addClass("nav-current");
        });
    </script>

    <div class="size-4 column">
        Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>


    <div class="size-4 column">
        Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="descbox"></asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList runat="server" id="ddlType" CssClass="ddlist">
            <asp:ListItem>syslinux/pxelinux</asp:ListItem>
            <asp:ListItem>ipxe</asp:ListItem>
            <asp:ListItem>grub</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Order:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtOrder" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Active:
    </div>
    <div class="size-5 column">
        <asp:CheckBox runat="server" id="chkActive" CssClass=""/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Default:
    </div>
    <div class="size-5 column">
        <asp:CheckBox runat="server" id="chkDefault" CssClass=""/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Contents:
    </div>
    <br class="clear"/>


    <asp:TextBox ID="txtContents" runat="server" CssClass="sysprepcontent" TextMode="MultiLine"></asp:TextBox>


</asp:Content>