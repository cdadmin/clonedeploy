<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" CodeFile="bootmenu.aspx.cs" Inherits="views.hosts.HostBootMenu" %>
<%@ MasterType VirtualPath="~/views/computers/computers.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#bootmenuoption').addClass("nav-current");
        });
    </script>
    <asp:LinkButton ID="buttonShowCustom" runat="server" Text="Custom" OnClick="buttonShowCustom_OnClick" CssClass="submits static-width-nomarg"/>
    <asp:LinkButton ID="buttonShowActive" runat="server" Text="Active" OnClick="buttonShowActive_OnClick" CssClass="submits static-width-nomarg"/>

    <div id="activebootmenu" runat="server" visible="True">


        <br class="clear"/>
        <div id="divProxy" runat="server" visible="false">


            <div class="size-6 column">
                Select A Menu:
            </div>
            <br class="clear"/>
            <div class="size-4 column">
                <asp:DropDownList ID="ddlProxyMode" runat="server" CssClass="ddlist" OnSelectedIndexChanged="EditProxy_Changed" AutoPostBack="true">
                    <asp:ListItem>bios</asp:ListItem>
                    <asp:ListItem>efi32</asp:ListItem>
                    <asp:ListItem>efi64</asp:ListItem>
                </asp:DropDownList>
            </div>
            <br class="clear"/>
        </div>
        <asp:Label ID="lblActiveBoot" runat="server"></asp:Label> <asp:Label ID="lblFileName1" runat="server"></asp:Label>
        <asp:TextBox ID="txtBootMenu" runat="server" CssClass="descboxboot" Style="font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
        
    </div>
    <br class="clear"/>

    <div id="custombootmenu" runat="server" visible="false">

        <br class="clear"/>
        <div class="size-4 column" style="float: right; margin: 0;">
            <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="ddlist" Style="float: right; margin-right: 5px; margin-top: 5px; width: 200px;" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <br class="clear"/>

        <div class="size-8 column">
            <asp:LinkButton ID="btnSetBootMenu" runat="server" Text="Set" OnClick="btnSetBootMenu_Click" CssClass="submits static-width" Style="float: left;"/>
        </div>
        <div class="size-8 column">
            <asp:LinkButton ID="btnRemoveBootMenu" runat="server" Text="Remove" OnClick="btnRemoveBootMenu_Click" CssClass="submits static-width" Style="float: left;"/>
        </div>
        <br class="clear"/>
        <asp:TextBox ID="txtCustomBootMenu" runat="server" CssClass="descboxboot" Style="font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
    </div>

</asp:Content>