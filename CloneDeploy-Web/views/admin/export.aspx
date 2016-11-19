<%@ Page Title="" Language="C#" MasterPageFile="~/views/admin/admin.master" AutoEventWireup="true" Inherits="views.admin.AdminExport" Codebehind="export.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Export Database</li>
</asp:Content>

<asp:Content runat="server" ID="Help" ContentPlaceHolderID="Help">
     <li role="separator" class="divider"></li>
      <li><a href="<%= ResolveUrl("~/views/help/admin-export.aspx")%>"  target="_blank">Help</a></li>
</asp:Content>

<asp:Content runat="server" ID="ActionsRight" ContentPlaceHolderID="SubPageActionsRight">
  <asp:LinkButton ID="btnExportCsv" runat="server" Text="CSV Export" OnClick="btnExportCsv_Click" CssClass="btn btn-default width_100"/>
     <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
  </button>
    
</asp:Content>

<asp:Content runat="server" ID="additional" ContentPlaceHolderID="AdditionalActions">
   

</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#export').addClass("nav-current");
        });
    </script>
</asp:Content>