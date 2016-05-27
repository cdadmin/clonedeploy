<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" Inherits="views.images.ImageImport" CodeFile="import.aspx.cs" %>

<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Import</li>
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>"  target="_blank">Help</a>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
      <asp:LinkButton ID="btnImport" runat="server" Text="Upload" OnClick="ButtonImport_Click" />
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#import').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        <asp:FileUpload ID="FileUpload" runat="server"/>
      
    </div>
    <br class="clear"/>
</asp:Content>