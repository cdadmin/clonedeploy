<%@ Page Title="" Language="C#" MasterPageFile="~/views/users/User.master" AutoEventWireup="true" Inherits="views.users.ImportUser" CodeFile="import.aspx.cs" %>


<asp:Content ID="Content" ContentPlaceHolderID="SubContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#importuser').addClass("nav-current");
        });
    </script>
    <div class="size-4 column">
        <asp:FileUpload ID="FileUpload" runat="server"/>
        <asp:LinkButton ID="btnImport" runat="server" Text="Upload" OnClick="btnImport_Click" CssClass="submits"/>
    </div>
</asp:Content>