<%@ Page Title="" Language="C#" MasterPageFile="~/views/groups/groups.master" AutoEventWireup="true" Inherits="views.groups.GroupImport" CodeFile="import.aspx.cs" %>

<%@ MasterType VirtualPath="~/views/groups/groups.master" %>
<%@ Reference virtualPath="~/views/masters/Site.master" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#importgroup').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        <asp:FileUpload ID="FileUpload" runat="server"/>
        <asp:LinkButton ID="btnImport" runat="server" Text="Upload" OnClick="btnImport_Click" CssClass="submits"/>
    </div>
    <br class="clear"/>
</asp:Content>