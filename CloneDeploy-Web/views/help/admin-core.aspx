<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#admin').addClass("nav-current");
            $('#admin-core').addClass("nav-current-sub");
        });
    </script>
    <h1>Admin->Core Scripts</h1>
   <p>
        The CloneDeploy Core Client Scripts can be edited here. When modifying the Core Client Scripts you are modifying the
        original files located at the CloneDeploy web directory\private\clientscripts. You should probably make a copy of these files before
        you start editing them.
    </p>
</asp:Content>