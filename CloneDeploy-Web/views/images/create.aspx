<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.images.ImageCreate" Codebehind="create.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>New</li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Help">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/images-newedit.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Add Image" CssClass="btn btn-default width_100"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Image Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtImageName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Client Imaging Environment:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="ddlEnvironment_OnSelectedIndexChanged">
            <asp:ListItem>linux</asp:ListItem>
            <asp:ListItem>macOS</asp:ListItem>
            <asp:ListItem>winpe</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>

    <div id="imageType" runat="server">
        <div class="size-4 column">
            Image Type:
        </div>
        <div class="size-5 column">
            <asp:DropDownList ID="ddlImageType" runat="server" CssClass="ddlist">
                <asp:ListItem>Block</asp:ListItem>
                <asp:ListItem>File</asp:ListItem>
            </asp:DropDownList>
        </div>
        <br class="clear"/>
    </div>

    <div id="osxImageType" runat="server" Visible="False">
        <div class="size-4 column">
            OSX Image Type:
        </div>
        <div class="size-5 column">
            <asp:DropDownList ID="ddlOsxImageType" runat="server" CssClass="ddlist" AutoPostBack="True" OnSelectedIndexChanged="ddlOsxImageType_OnSelectedIndexChanged">
                <asp:ListItem>thick</asp:ListItem>
                <asp:ListItem>thin</asp:ListItem>
            </asp:DropDownList>
        </div>
        <br class="clear"/>
    </div>

    <div id="thinImage" runat="server" Visible="False">
        <div class="size-4 column">
            Thin Image OS DMG:
        </div>
        <div class="size-5 column">
            <asp:DropDownList ID="ddlThinOS" runat="server" CssClass="ddlist"/>
        </div>
        <br class="clear"/>

        <div class="size-4 column">
            Thin Image Recovery DMG:
        </div>
        <div class="size-5 column">
            <asp:DropDownList ID="ddlThinRecovery" runat="server" CssClass="ddlist"/>
        </div>
        <br class="clear"/>
    </div>

    <div class="size-4 column">
        Image Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtImageDesc" runat="server" TextMode="MultiLine" CssClass="descbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Protected:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkProtected" runat="server"/>
    </div>
    <br class="clear"/>
    <br/>
    <div class="size-4 column">
        Visible On Demand:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkVisible" runat="server"/>
    </div>

</asp:Content>