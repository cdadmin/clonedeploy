<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="views.admin.AdminExport" CodeFile="export.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Export Database</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
      <a href="<%= ResolveUrl("~/views/help/index.html")%>"  target="_blank">Help</a>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
   <asp:LinkButton ID="btnExportSql" runat="server" Text="Full Database Dump " OnClick="btnExportSql_Click" />
    <asp:LinkButton ID="btnExportCsv" runat="server" Text="CSV Export" OnClick="btnExportCsv_Click" />
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#export').addClass("nav-current");
        });
    </script>
</asp:Content>