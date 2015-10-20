<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="views.hosts.HostEdit" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
   <li><a href="<%= ResolveUrl("~/views/computers/edit.aspx") %>?hostid=<%= Computer.Id %>" ><%= Computer.Name %></a></li>
    </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="icon help" data-info="Help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <asp:LinkButton ID="buttonUpdateHost" runat="server" OnClick="buttonUpdateHost_Click" Text="Update Host" CssClass="submits"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#general').addClass("nav-current");
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
        Site:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlSite" runat="server" CssClass="ddlist"/>
    </div>

    <br class="clear"/>
     <div class="size-4 column">
        Building:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlBuilding" runat="server" CssClass="ddlist"/>
    </div>

    <br class="clear"/>
     <div class="size-4 column">
        Room:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlRoom" runat="server" CssClass="ddlist"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Custom Attribute 1:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom1" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Custom Attribute 2:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom2" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Custom Attribute 3:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom3" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
         Custom Attribute 4:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom4" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    
    <div class="size-4 column">
        Custom Attribute 5:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtCustom5" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="spacer">
       
    </div>

</asp:Content>