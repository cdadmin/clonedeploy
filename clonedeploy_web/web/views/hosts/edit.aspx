<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Host.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="views.hosts.HostEdit" %>
<%@ MasterType VirtualPath="~/views/masters/Host.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#editoption').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Host Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host MAC Address:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostMac" runat="server" CssClass="textbox" MaxLength="17"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlHostImage" runat="server" CssClass="ddlist"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host Group:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlHostGroup" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host Kernel:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlHostKernel" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host Boot Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlHostBootImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostArguments" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Host Scripts:
    </div>
    <div class="size-5 column">
        <asp:ListBox ID="lbScripts" runat="server" SelectionMode="Multiple"></asp:ListBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="buttonUpdateHost" runat="server" OnClick="buttonUpdateHost_Click" Text="Update Host" CssClass="submits"/>
    </div>
</asp:Content>