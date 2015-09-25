<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" CodeFile="bootmenu.aspx.cs" Inherits="views.groups.GroupBootMenu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#bootmenuoption').addClass("nav-current");
        });
    </script>

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

</asp:Content>