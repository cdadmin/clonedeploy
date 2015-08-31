<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/Admin.master" AutoEventWireup="true" Inherits="views.admin.AdminExport" CodeFile="export.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/admin/Admin.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#exportDatabaseSettings').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        <asp:LinkButton ID="btnExport" runat="server" Text="Export Database" OnClick="btnExport_Click" CssClass="submits"/>
    </div>
</asp:Content>