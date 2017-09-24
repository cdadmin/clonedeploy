<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/dp/dp.master" AutoEventWireup="true" Inherits="CloneDeploy_Web.views.admin.dp.views_admin_dp_create" Codebehind="create.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>New</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-dp.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="buttonAddDp" runat="server" OnClick="buttonAddDp_OnClick" Text="Add Distribution Point" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#create').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Display Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="textbox" ClientIDMode="Static"></asp:TextBox>

    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Server Ip / Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtServer" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Protocol:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlProtocol" runat="server" CssClass="ddlist">
            <asp:ListItem>SMB</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>

    <div class="size-4 column">
        Share Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtShareName" runat="server" CssClass="textbox"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Domain / Workgroup:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtDomain" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>

    <div class="size-4 column">
        Read/Write Username:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRwUsername" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>

    <div class="size-4 column">
        Read/Write Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRwPassword" runat="server" CssClass="textbox" TextMode="Password"/>
    </div>

    <br class="clear"/>

    <div class="size-4 column">
        Read Only Username:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRoUsername" runat="server" CssClass="textbox"/>
    </div>

    <br class="clear"/>

    <div class="size-4 column">
        Read Only Password:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtRoPassword" runat="server" CssClass="textbox" TextMode="Password"/>
    </div>


    <br class="clear"/>
    <div class="size-4 column">
        Primary Distribution Point:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkPrimary" runat="server" OnCheckedChanged="chkPrimary_OnCheckedChanged" AutoPostBack="True"/>
    </div>

    <br class="clear"/>
    <br/>

    <div class="size-4 column">
        Queue Size:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="qSize" runat="server" CssClass="textbox" Text="2"/>
    </div>


    <br class="clear"/>

    <div id="PrimaryParams" runat="server" >
        <div class="size-4 column">
            Location:
        </div>
        <div class="size-5 column">
            <asp:DropDownList ID="ddlPrimaryType" runat="server" CssClass="ddlist" OnSelectedIndexChanged="ddlPrimaryType_OnSelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem>Local</asp:ListItem>
                <asp:ListItem>Remote</asp:ListItem>
            </asp:DropDownList>
        </div>
        <br class="clear"/>
        <div id="PhysicalPath" runat="server" visible="False">
            <div class="size-4 column">
                Physical Path:
            </div>
            <div class="size-1 column">
                <asp:TextBox ID="txtPhysicalPath" runat="server" CssClass="textbox"/>
            </div>

            <br class="clear"/>
        </div>
    </div>


</asp:Content>