<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/bootmenu/bootmenu.master" AutoEventWireup="true" CodeFile="active.aspx.cs" Inherits="views_computers_bootmenu_active" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
    <li>Active</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">     
        $(document).ready(function() {      
            $('#active').addClass("nav-current");
        });     
    </script>
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
        <asp:Label ID="lblActiveBoot" runat="server"></asp:Label> 
        <asp:Label ID="lblFileName" runat="server"></asp:Label>
        <asp:TextBox ID="txtBootMenu" runat="server" CssClass="descboxboot" Style="font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
        
</asp:Content>

