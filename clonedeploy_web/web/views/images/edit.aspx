﻿<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" Inherits="views.images.ImageEdit" CodeFile="edit.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/images/images.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
   <li><a href="<%= ResolveUrl("~/views/images/edit.aspx") %>?imageid=<%= Master.Image.Id %>" ><%= Master.Image.Name %></a></li>
    </asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $('#editoption').addClass("nav-current");
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
        Image OS:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlImageOS" runat="server" CssClass="ddlist">
            <asp:ListItem>Windows</asp:ListItem>
            <asp:ListItem>Linux</asp:ListItem>
            <asp:ListItem>Mac</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    
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
    
    <div class="size-4 column">
        Image Environment:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlImageEnvironment" runat="server" CssClass="ddlist">
            <asp:ListItem>Linux</asp:ListItem>
            <asp:ListItem>Windows</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>

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
    <div class="size-4 column">
        Visible In On Demand:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkVisible" runat="server"/>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnUpdateImage" runat="server" OnClick="btnUpdateImage_Click" Text="Update Image" CssClass="submits"/>
    </div>
    <br class="clear"/>
    <div class="size-5 column" style="margin-left: -15px;">
        <h3>Image Directory Status:</h3>
    </div>
    <br class="clear"/>
    <div class="size-5 column">
        <asp:Label ID="lblImageHold" runat="server"></asp:Label><br/>
        <asp:Label ID="lblImage" runat="server"></asp:Label><br/>
        <asp:Label ID="lblImageHoldStatus" runat="server"></asp:Label><br/>
        <asp:Label ID="lblImageStatus" runat="server"></asp:Label>
        <br class="clear"/>

        <div class="size-5 column">
            <asp:LinkButton ID="btnFixImage" runat="server" OnClick="btnFixImage_Click" Text="Fix Image Directories" CssClass="submits"/>
        </div>
        <br class="clear"/>
    </div>

</asp:Content>