<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" Inherits="views.images.ImageEdit" CodeFile="edit.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
   <li><a href="<%= ResolveUrl("~/views/images/edit.aspx") %>?imageid=<%= Image.Id %>" ><%= Image.Name %></a></li>
   <li>General</li>
     </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="submits help" target="_blank"></a>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
       <asp:LinkButton ID="btnUpdateImage" runat="server" OnClick="btnUpdateImage_Click" Text="Update Image" CssClass="submits actions green"/>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $('#edit').addClass("nav-current");
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
        Client Imaging Environment:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="ddlist">
            <asp:ListItem>linux</asp:ListItem>
            <asp:ListItem>osx</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    
    <div id="imageType" runat="server">
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
    </div>
    
    <div id="osxImageType" runat="server" Visible="False">
     <div class="size-4 column">
        OSX Image Type:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlOsxImageType" runat="server" CssClass="ddlist">
            <asp:ListItem>thick</asp:ListItem>
            <asp:ListItem>thin</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    </div>
    
    <div id="thinImage" runat="server" Visible="False">
     <div class="size-4 column">
        Thin Image OS DMG:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlThinOS" runat="server" CssClass="ddlist"/>
    </div>
    <br class="clear"/>
        
        <div class="size-4 column">
        Thin Image Recovery DMG:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlThinRecovery" runat="server" CssClass="ddlist"/>
    </div>
    <br class="clear"/>
    </div>
    
   

    <div class="size-4 column">
        Image Description:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtImageDesc" runat="server" TextMode="MultiLine" CssClass="descbox"></asp:TextBox>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Enabled:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkEnabled" runat="server"/>
    </div>
    <br class="clear"/>
    <br />
    <div class="size-4 column">
        Protected:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkProtected" runat="server"/>
    </div>
    <br class="clear"/>
    <br />
    <div class="size-4 column">
        Visible In On Demand:
    </div>
    <div class="size-5 column">
        <asp:CheckBox ID="chkVisible" runat="server"/>
    </div>
   
    

</asp:Content>