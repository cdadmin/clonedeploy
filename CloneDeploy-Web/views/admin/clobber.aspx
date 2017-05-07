<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="views_admin_clobber" Codebehind="clobber.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Clobber Mode Settings</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-clobber.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
    <a class="confirm btn btn-default" href="#">Update Clobber Settings</a>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#clobber').addClass("nav-current");
        });
    </script>

    <div class="size-4 column">
        Enable Clobber Mode:
    </div>
    <div class="size-setting column">
        <asp:CheckBox runat="server" id="chkClobber"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlComputerImage" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlComputerImage_OnSelectedIndexChanged"/>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Image Profile:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlImageProfile" runat="server" CssClass="ddlist"/>
    </div>

    <br class="clear"/>
    <div class="size-4 column">
        Prompt For Computer Name:
    </div>
    <div class="size-setting column">
        <asp:CheckBox runat="server" id="chkPromptName"/>
    </div>
    <br class="clear"/>

    <div id="confirmbox" class="confirm-box-outer">
        <div class="confirm-box-inner">
            <h4>
                <asp:Label ID="lblTitle" runat="server" Text="Are You Sure You Want To Change Clobber Settings?<br>You Could Potentially Image Non Targeted Computers."></asp:Label>
            </h4>
            <div class="confirm-box-btns">
                <asp:LinkButton ID="ConfirmButton" OnClick="ButtonConfirm_Click" runat="server" Text="Yes" CssClass="confirm_yes"/>
                <asp:LinkButton ID="CancelButton" runat="server" Text="No" CssClass="confirm_no"/>
            </div>
        </div>
    </div>
</asp:Content>