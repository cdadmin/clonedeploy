<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="pxe.aspx.cs" Inherits="views_images_profiles_pxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li><a href="<%= ResolveUrl("~/views/images/profiles/chooser.aspx") %>?imageid=<%= Image.Id %>&profileid=<%= ImageProfile.Id %>&cat=profiles"><%= ImageProfile.Name %></a></li>
    <li>PXE Boot Options</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#pxe').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        Kernel:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlKernel" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Boot Image:
    </div>
    <div class="size-5 column">
        <asp:DropDownList ID="ddlBootImage" runat="server" CssClass="ddlist">
        </asp:DropDownList>
    </div>
    <br class="clear"/>
    <div class="size-4 column">
        Kernel Arguments:
    </div>
    <div class="size-5 column">
        <asp:TextBox ID="txtKernelArgs" runat="server" CssClass="textbox">
        </asp:TextBox>
    </div>
    <br class="clear"/>
    
    
    <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="buttonUpdatePXE" runat="server" OnClick="buttonUpdatePXE_OnClick" Text="Update PXE Boot Options" CssClass="submits" OnClientClick="update_click()"/>
    </div>
</asp:Content>

