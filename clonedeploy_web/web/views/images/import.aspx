<%@ Page Title="" Language="C#" MasterPageFile="~/views/images/images.master" AutoEventWireup="true" Inherits="views.images.ImageImport" CodeFile="import.aspx.cs" %>

<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#importimage').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        <asp:FileUpload ID="FileUpload" runat="server"/>
        <asp:LinkButton ID="btnImport" runat="server" Text="Upload" OnClick="btnImport_Click" CssClass="submits"/>
    </div>
    <br class="clear"/>
</asp:Content>