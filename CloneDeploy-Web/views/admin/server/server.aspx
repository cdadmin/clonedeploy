<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/server/server.master" AutoEventWireup="true" CodeBehind="server.aspx.cs" Inherits="CloneDeploy_Web.views.admin.server.serversettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Server Settings</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHelp" Runat="Server">
    <li role="separator" class="divider"></li>
    <li>
        <a href="<%= ResolveUrl("~/views/help/admin-server.aspx") %>" target="_blank">Help</a>
    </li>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ActionsRightSub">
    <asp:LinkButton ID="btnUpdateSettings" runat="server" Text="Update Server Settings " OnClick="btnUpdateSettings_OnClick" CssClass="btn btn-default"/>
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <span class="caret"></span>
    </button>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#serversub').addClass("nav-current");
        });
    </script>
    <div id="settings">
        <div class="size-4 column">
            Server IP / FQDN:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtIP" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear"/>

        <div class="size-4 column">
            Server Identifier:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtId" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            API Port:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtPort" runat="server" CssClass="textbox"></asp:TextBox>
        </div>

        <br class="clear"/>
        <div class="size-4 column">
            Manual Override API Url:
        </div>
        <div class="size-setting column">
            <asp:CheckBox ID="chkOverride" runat="server"></asp:CheckBox>
        </div>

        <br class="clear"/>
        <br/>
        <div class="size-4 column">
            Base Url:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtWebService" runat="server" CssClass="textbox"></asp:TextBox>
        </div>


        <br class="clear"/>
        <div class="size-4 column">
            TFTP Server IP:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtTftpServerIp" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear"/>
        <div class="size-4 column">
            TFTP Path:
        </div>
        <div class="size-setting column">
            <asp:TextBox ID="txtTFTPPath" runat="server" CssClass="textbox"></asp:TextBox>
        </div>
        <br class="clear"/>

        <div id="divComputerView" runat="server">
            <div class="size-4 column">

                Computer View:
            </div>
            <div class="size-setting column ddl">

                <asp:DropDownList ID="ddlComputerView" runat="server" CssClass="ddlist" ClientIDMode="Static">
                    <asp:ListItem>all</asp:ListItem>
                    <asp:ListItem>search</asp:ListItem>
                </asp:DropDownList>
            </div>
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
                <br class="clear"/>
                <asp:Label ID="lblClientISO" runat="server" CssClass="smalltext"></asp:Label>

            </div>
        </div>

    </div>
</asp:Content>