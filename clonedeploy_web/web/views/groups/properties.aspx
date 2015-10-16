<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" CodeFile="properties.aspx.cs" Inherits="views_groups_properties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
       <li><a href="<%= ResolveUrl("~/views/groups/edit.aspx") %>?groupid=<%= Group.Id %>" ><%= Group.Name %></a></li>
    <li>Computer Properties</li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SubContent" Runat="Server">
     <script type="text/javascript">
        $(document).ready(function() {
            $('#properties').addClass("nav-current");
        });
    </script>
</asp:Content>

