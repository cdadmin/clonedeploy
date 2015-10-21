<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" Inherits="views.images.ImageCreate" CodeFile="create.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/images/create.aspx") %>">New</a></li>
    <li> My New Long Text 1</li>
    <li> My New Long Text 2</li>
     <li> My New Long Text 3</li>
    </asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#createimage').addClass("nav-current");
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
         <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Add Image" CssClass="submits"/>
    </div>
   
    <br class="clear"/>
</asp:Content>