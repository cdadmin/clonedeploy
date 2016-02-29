<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" Inherits="views.computers.Addcomputers" CodeFile="create.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>New</li>
    </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
     <asp:LinkButton ID="buttonAddComputer" runat="server" OnClick="ButtonAddComputer_Click" Text="Add Computer" CssClass="submits actions green"/>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">     
        $(document).ready(function() {      
            $('#new').addClass("nav-current");
        });     
    </script>

    <div class="size-4 column">
        Name:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtComputerName" runat="server" CssClass="textbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        MAC Address:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtComputerMac" runat="server" CssClass="textbox" MaxLength="17"></asp:TextBox>
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
        Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtComputerDesc" runat="server" CssClass="descbox" TextMode="MultiLine"></asp:TextBox>
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
   
    <div class="size-4 column">
        Create Another?
      
    </div>

    <div class="size-4 column">
         <asp:CheckBox runat="server" ID="createAnother"/>
    </div>
    <div class="size-5 column">
       
    </div>
</asp:Content>