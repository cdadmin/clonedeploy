<%@ Page Title="" Language="C#" MasterPageFile="~/views/masters/Admin.master" AutoEventWireup="true" Inherits="views.admin.AdminScriptEditor" CodeFile="sysprep.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/masters/Admin.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#sysprepSettings').addClass("nav-current");
        });
    </script>

    
</asp:Content>