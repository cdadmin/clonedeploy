<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.admin.Client" Codebehind="client.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Client</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-client.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Client Settings " OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
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