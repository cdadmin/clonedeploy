<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="views_admin_munki" Codebehind="munki.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Munki Settings</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-munki.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Munki Settings " OnClick="btnUpdateSettings_OnClick" CssClass=" btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#munki').addClass("nav-current");
        });
    </script>

    <div class="size-4 column">
        Path Type:
    </div>
    <div class="size-setting column">
        <asp:DropDownList ID="ddlPathType" runat="server" CssClass="ddlist">
            <asp:ListItem>Local</asp:ListItem>
            <asp:ListItem>SMB Share</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Base Path:
    </div>
    <div class="size-setting column">
        <asp:TextBox ID="txtBasePath" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        SMB Username:
    </div>
    <div class="size-setting column">
        <asp:TextBox ID="txtSmbUsername" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        SMB Password:
    </div>
    <div class="size-setting column">
        <asp:TextBox ID="txtSmbPassword" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        SMB Domain:
    </div>
    <div class="size-setting column">
        <asp:TextBox ID="txtDomain" runat="server" CssClass="textbox"></asp:TextBox>
    </div>


</asp:Content>