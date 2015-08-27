<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Image.master" AutoEventWireup="true" CodeFile="chooser.aspx.cs" Inherits="views_images_profiles_chooser" %>
<%@ MasterType VirtualPath="~/views/masters/Image.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li><a href="<%= ResolveUrl("~/views/images/edit.aspx") %>?imageid=<%= Master.Image.Id %>" ><%= Master.Image.Name %></a></li>
     <li><a href="<%= ResolveUrl("~/views/images/profiles/search.aspx") %>?imageid=<%= Master.Image.Id %>&subid=profiles">Profiles</a></li>
    <li><%= LinuxEnvironmentProfile.Name %></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Level3" Runat="Server">
      <li><a id="profileoption" href="<%= ResolveUrl("~/views/images/profiles/general.aspx") %>?imageid=<%= Master.Image.Id %>&profileid=<%= Request.QueryString["profileid"] %>&subid=profiles"><span class="sub-nav-text">General</span></a></li>
     <li><a href="<%= ResolveUrl("~/views/images/profiles/pxe.aspx")  %>?imageid=<%= Master.Image.Id %>&profileid=1&subid=profiles"><span class="sub-nav-text">PXE Boot Options</span></a></li>
    <li><a href="<%= ResolveUrl("~/views/images/profiles/upload.aspx")  %>?imageid=<%= Master.Image.Id %>&profileid=1&subid=profiles"><span class="sub-nav-text">Upload Options</span></a></li>
    <li><a href="<%= ResolveUrl("~/views/images/profiles/deploy.aspx")  %>?imageid=<%= Master.Image.Id %>&profileid=1&subid=profiles"><span class="sub-nav-text">Deploy Options</span></a></li>
     <li><a href="<%= ResolveUrl("~/views/images/profiles/partition.aspx")  %>?imageid=<%= Master.Image.Id %>&profileid=1&subid=profiles"><span class="sub-nav-text">Partition</span></a></li> 
    <li><a href="<%= ResolveUrl("~/views/images/profiles/scripts.aspx")  %>?imageid=<%= Master.Image.Id %>&profileid=1&subid=profiles"><span class="sub-nav-text">Scripts</span></a></li>
     <li><a href="<%= ResolveUrl("~/views/images/profiles/sysprep.aspx")  %>?imageid=<%= Master.Image.Id %>&profileid=1&subid=profiles"><span class="sub-nav-text">Sysprep</span></a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SubContent" Runat="Server">
</asp:Content>

