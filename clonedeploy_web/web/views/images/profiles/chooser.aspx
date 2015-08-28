<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/profiles/profiles.master" AutoEventWireup="true" CodeFile="chooser.aspx.cs" Inherits="views_images_profiles_chooser" %>
<%@ MasterType VirtualPath="~/views/images/profiles/profiles.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub2" Runat="Server">
     <li><%= LinuxEnvironmentProfile.Name %></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent2" Runat="Server">
</asp:Content>

