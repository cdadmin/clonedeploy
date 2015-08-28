<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="deploy.aspx.cs" Inherits="views_images_profiles_deploy" %>
<%@ MasterType VirtualPath="~/views/images/profiles/profiles.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li><a href="<%= ResolveUrl("~/views/images/profiles/chooser.aspx") %>?imageid=<%= Master.Image.Id %>&profileid=<%= Master.ImageProfile.Id %>&cat=profiles"><%= Master.ImageProfile.Name %></a></li>
    <li>Deploy Options</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#deploy').addClass("nav-current");
        });
    </script>
    
     <div class="size-9 column">
        Don't Expand Volume
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownNoExpand" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
        
    <div class="size-9 column">
        Expand Partitions Without Resizable Volumes
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkDownExpandNonResizable" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
     
    <div class="size-9 column">
        Align BCD To Partition
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkAlignBCD" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
    
     <div class="size-9 column">
        Run Fix Boot
    </div>
    <div class="size-8 column">
        <asp:CheckBox ID="chkRunFixBoot" runat="server" CssClass="textbox"></asp:CheckBox>
    </div>
    <br class="clear"/>
     <div class="size-4 column">
        &nbsp;
    </div>
    <div class="size-5 column">
        <asp:LinkButton ID="btnUpdateDeploy" runat="server" OnClick="btnUpdateDeploy_OnClick" Text="Update Deploy Options" CssClass="submits"/>
    </div>
</asp:Content>

