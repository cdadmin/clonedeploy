<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="views.hosts.HostEdit" %>
<%@ MasterType VirtualPath="~/views/computers/computers.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
   <li><a href="<%= ResolveUrl("~/views/computers/edit.aspx") %>?hostid=<%= Master.Computer.Id %>" ><%= Master.Computer.Name %></a></li>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#editoption').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Primary MAC Address:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostMac" runat="server" CssClass="textbox" MaxLength="17"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlHostImage" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="ddlHostImage_OnSelectedIndexChanged"/>
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
        Host Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtHostDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
       <asp:LinkButton ID="buttonUpdateHost" runat="server" OnClick="buttonUpdateHost_Click" Text="Update Host" CssClass="submits"/>
    </div>

</asp:Content>