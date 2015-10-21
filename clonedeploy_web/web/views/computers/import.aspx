<%@ Page Title="" Language="C#" MasterPageFile="~/views/computers/computers.master" AutoEventWireup="true" Inherits="views.hosts.HostImport" CodeFile="import.aspx.cs" %>
<asp:Content ID="Breadcrumb" ContentPlaceHolderID="BreadcrumbSub" Runat="Server">
    <li>Import</li>
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Help">
     <a href="<%= ResolveUrl("~/views/help/index.html")%>" class="icon help" data-info="Help" target="_blank"></a>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="SubPageActionsRight">
   <asp:LinkButton ID="buttonImport" runat="server" Text="Upload" OnClick="ButtonImport_Click" CssClass="submits"/>
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